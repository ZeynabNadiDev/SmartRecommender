using SmartRecommender.Application.Abstractions.ConectWithAI;
using SmartRecommender.Application.Abstractions.Services;
using System.Threading;
using System.Threading.Tasks;

namespace SmartRecommender.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IRecommenderEngine _recommender;

        public ChatService(IRecommenderEngine recommender)
        {
            _recommender = recommender;
        }
        public async Task<string> GetRecommendationAsync(string message, CancellationToken cancellationToken)
        {
            var response = await _recommender.GetRecommendationAsync(message, cancellationToken);
            return response;
        }
    }
}
