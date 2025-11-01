using SmartRecommender.AI.Interfaces;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace SmartRecommender.AI.Services
{
    public class IntentExtractor : IIntentExtractor
    {
        private readonly HttpClient _httpClient;
        private readonly string _model;
        private readonly double _temperature;
        private const string OpenAIEndpoint = "https://api.openai.com/v1/chat/completions";

        public IntentExtractor(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;

            var apiKey = configuration["OpenAI:ApiKey"]
                ?? throw new ArgumentNullException("OpenAI:ApiKey not found in configuration.");

            // Configure HTTP client only once
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            _httpClient.Timeout = TimeSpan.FromSeconds(60);

            _model = configuration["OpenAI:Model"] ?? "gpt-4o-mini";
            _temperature = double.TryParse(configuration["OpenAI:Temperature"], out var temp) ? temp : 0.2;
        }

        public async Task<UserIntent> ExtractIntentAsync(string userInput, CancellationToken cancellationToken = default)
        {
            // Main prompt for semantic interpretation
            var prompt = $@"
            You are an intelligent shopping assistant that understands natural language (including Persian/Farsi).
            Your task is to interpret the user’s intent and convert it into structured JSON for an e-commerce recommender system.

             Extract intent fields in the following JSON format:
              {{
                 ""ActionType"": string,
                 ""Category"": string,
                 ""Keywords"": [string],
                 ""Filters"": {{
                 ""MinPrice"": decimal | null,
                 ""MaxPrice"": decimal | null,
                 ""Brand"": string | null,
                 ""Color"": string | null,
                 ""Feature"": string | null
              }},
                  ""Confidence"": float,
                  ""UserId"": string | null,
                  ""DeviceType"": string | null,
                  ""Region"": string | null,
                  ""Season"": string | null,
                  ""PurchaseIntent"": string | null
             }}

            Guidelines:
            - Use null for missing or uncertain data.
            - Convert Farsi expressions like 'زیر ۵۰۰۰ تومن' or 'less than 5000 toman' → MaxPrice: 5000.
            - Convert expressions like 'بیشتر از ۱۰۰۰۰' or 'more than 10000' → MinPrice: 10000.
            - Output strictly valid JSON only (no markdown, no commentary).
            User message:
            { userInput}
            
            ";

            var body = new
            {
                model = _model,
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content = "You are a smart human‑like intent interpreter for a recommender system. " +
                                  "Understand the user's message in any language and respond ONLY with valid JSON."
                    },
                    new { role = "user", content = prompt }
                },
                temperature = _temperature
            };

            var jsonBody = JsonSerializer.Serialize(body);
            var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(OpenAIEndpoint, httpContent, cancellationToken);
                var responseText = await response.Content.ReadAsStringAsync(cancellationToken);

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"OpenAI API Error: {response.StatusCode} -> {responseText}");

                using var doc = JsonDocument.Parse(responseText);
                var contentText = doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString()?.Trim();

                if (string.IsNullOrWhiteSpace(contentText))
                    throw new JsonException("Empty AI response content.");

                // Clean possible formatting artifacts
                contentText = contentText.Trim('`', '\n', '\r', ' ');

                var userIntent = JsonSerializer.Deserialize<UserIntent>(
                    contentText,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        ReadCommentHandling = JsonCommentHandling.Skip
                    });

                return userIntent ?? new UserIntent();
            }
            catch (Exception ex) when (ex is TaskCanceledException || ex is HttpRequestException || ex is JsonException)
            {
                // Return safe fallback structure on failure
                return new UserIntent();
            }
        }
    }
}
