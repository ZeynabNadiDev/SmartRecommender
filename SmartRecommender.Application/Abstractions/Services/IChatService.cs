using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.Application.Abstractions.Services
{
    public interface IChatService
    {
        Task<string> GetRecommendationAsync(string message, CancellationToken cancellationToken);
    }
}
