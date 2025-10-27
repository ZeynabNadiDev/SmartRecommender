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
        public async Task<string> GenerateResponseAsync(UserIntent userIntent, IEnumerable<Product> matchedProducts,CancellationToken cancellationToken)
        {
            var productList = string.Join("\n", matchedProducts.Select(p =>
                $"- {p.Name} ({p.Category.Name}) | قیمت: {p.Price} | توضیح: {p.Description}"
            ));
            var prompt = $@"
             User Intent:
             {JsonSerializer.Serialize(userIntent, new JsonSerializerOptions { WriteIndented = true })}

             Matched Products:
             {productList}

             Please generate a friendly, concise, and helpful recommendation message for the user.
             Use Persian language and suggest the best product(s) naturally.
             ";

            var requestData = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "system", content = "You are SmartRecommender AI, a helpful Persian shopping assistant." },
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

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"OpenAI API Error: {response.StatusCode} -> {jsonResponse}");
          
            using var doc = JsonDocument.Parse(jsonResponse);
            var message = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return message ?? "نتوانستم پاسخی تولید کنم.";
        }
    }
}
