using SmartRecommender.AI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SmartRecommender.AI.Services
{
    public class IntentExtractor : IIntentExtractor
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        private readonly string _model;
        private readonly double _temperature;
    
    public IntentExtractor(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            _apiKey = _configuration["OpenAI:ApiKey"];
            _model = _configuration["OpenAI:Model"] ?? "gpt-4o-mini";
            _temperature = double.Parse(_configuration["OpenAI:Temperature"] ?? "0.2");
        }
        public async Task<UserIntent> ExtractIntentAsync(string userInput, CancellationToken cancellationToken = default)
        {
            var prompt = $@"
            You are an intent extraction AI for an e-commerce recommender system.
            Extract these fields from the user message:
            - Category (ex: Mobile, Laptop, Headphone)
            - Keywords (main concepts)
            - Region (geographic location if mentioned)
            - DeviceType (optional)
            - PurchaseIntent (Yes/No depending on action verbs)
             Return JSON only, with this shape:
            {{ ""Category"": string, ""Keywords"": [string], ""Region"": string,
            ""DeviceType"": string, ""PurchaseIntent"": bool }}
             Message: ""{userInput}""
             ";

            var requestBody = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "system", content = "You are a precise intent" +
                    " extractor for recommender models." },
                    new { role = "user", content = prompt }
                },
                temperature = _temperature
            };
            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, 
                "application/json");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content, cancellationToken);
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"OpenAI API Error: {response.StatusCode} -> {responseString}");
            }
            using var doc = JsonDocument.Parse(responseString);
            var messageText = doc.RootElement
                                 .GetProperty("choices")[0]
                                 .GetProperty("message")
                                 .GetProperty("content")
                                 .GetString();
            var intent = JsonSerializer.Deserialize<UserIntent>(messageText, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            intent ??= new UserIntent
            {
                Category = string.Empty,
                Keywords = new(),
                Region = string.Empty,
                DeviceType = string.Empty,
                PurchaseIntent = "Browsing"
            };

            return intent;

        }
    }
 }
