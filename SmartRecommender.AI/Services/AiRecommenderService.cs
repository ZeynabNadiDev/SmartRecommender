using SmartRecommender.AI.Interfaces;
using SmartRecommender.Application.Abstractions.ConectWithAI;
using SmartRecommender.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.AI.Services
{
    public class AiRecommenderService: IAiRecommenderService, IRecommenderEngine
    {
        private readonly IIntentExtractor _intentExtractor;
        private readonly IProductMatcher _productMatcher;
        private readonly IResponseGenerator _responseGenerator;

        public AiRecommenderService(
            IIntentExtractor intentExtractor,
            IProductMatcher productMatcher,
            IResponseGenerator responseGenerator)
        {
            _intentExtractor = intentExtractor;
            _productMatcher = productMatcher;
            _responseGenerator = responseGenerator;
        }

        public async Task<string> GetRecommendationAsync(string userMessage, CancellationToken cancellationToken)
        {
            var intent = await _intentExtractor.ExtractIntentAsync(userMessage, cancellationToken);
            var matchedProducts = await _productMatcher.MatchProductsAsync(intent, cancellationToken);
            var response = await _responseGenerator.GenerateResponseAsync(intent, matchedProducts, cancellationToken);
            return response;
        }
    }
}
