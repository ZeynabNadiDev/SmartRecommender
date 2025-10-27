using SmartRecommender.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.AI.Interfaces
{
    public interface IAiRecommenderService
    {
        Task<UserIntent> ExtractUserIntentAsync(string userInput,
            CancellationToken cancellationToken);
    }
}
