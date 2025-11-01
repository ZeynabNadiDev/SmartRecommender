using Microsoft.Extensions.Configuration;
using SmartRecommender.AI.Interfaces;
using SmartRecommender.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartRecommender.AI.Services
{
    public class ResponseGenerator : IResponseGenerator
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        private readonly string _model;
        public ResponseGenerator(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            _apiKey = _configuration["OpenAI:ApiKey"] ?? throw new Exception("OpenAI API key not found in appsettings.json.");
            _model = _configuration["OpenAI:Model"] ?? "gpt-4o-mini";
        }
        public async Task<string> GenerateResponseAsync(
    UserIntent userIntent,
    IEnumerable<Product> matchedProducts,
    CancellationToken cancellationToken)
        {
            // If no products were found in the database
            if (matchedProducts == null || !matchedProducts.Any())
                return "متأسفم، بر اساس توضیحات شما هیچ کالای مرتبطی در فروشگاه پیدا نشد.";

            // Generate a human-readable product list from the database
            var productList = string.Join("\n", matchedProducts.Select(p =>
                $"- {p.Name} (Category: {p.Category?.Name ?? "Unknown"}) | Price: {p.Price:N0} Toman | Description: {p.Description}"
            ));

            // Strict prompt: forces the model to use only the database products provided below
            var prompt = $@"
            You are SmartRecommender, the AI assistant of an online shopping platform.
            Use ONLY the following list of products (which are fetched directly from the internal database). 
            Do NOT create or invent new products, names, or details.

            Goal:
            Based on the user's intent, recommend 2–3 products from the list below that match the intent best.
            Write the response in natural Persian (Farsi), friendly, concise, and human-like.

            If no relevant products are found, respond with:
             ""هیچ کالای مرتبطی در حال حاضر موجود نیست.""

            User Intent:
            {JsonSerializer.Serialize(userIntent, new JsonSerializerOptions { WriteIndented = true })}

            Product List:
            {productList}
            ";

            var requestData = new
            {
                model = _model,
                messages = new[]
                {
            new { role = "system", content = "You are SmartRecommender AI, a helpful Persian-speaking assistant that ONLY recommends from local database products." },
            new { role = "user", content = prompt }
        }
            };

            var content = new StringContent(
                JsonSerializer.Serialize(requestData),
                Encoding.UTF8,
                "application/json"
            );

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content, cancellationToken);
            var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"OpenAI API Error: {response.StatusCode} -> {jsonResponse}");

            using var doc = JsonDocument.Parse(jsonResponse);
            var message = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return string.IsNullOrWhiteSpace(message)
                ? "نتوانستم پاسخی تولید کنم."
                : message.Trim();
        }

    }
}
