using SmartRecommender.Application.Abstractions.ConectWithAI;
using SmartRecommender.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.Application.Services
{
    public class ChatService:IChatService
    {
        private readonly IRecommenderEngine _recommender;
        public ChatService(IRecommenderEngine recommender)
        {
            _recommender = recommender;
        }

        public async Task <string> GetRecommendationAsync(string message, CancellationToken cancellationToken)
        {
            var userIntent = await _recommender.ExtractUserIntentAsync(message, cancellationToken);
            return $"Intent={userIntent.PurchaseIntent}, Keywords={string.Join(", ", userIntent.Keywords)}";
        }
    }
}
