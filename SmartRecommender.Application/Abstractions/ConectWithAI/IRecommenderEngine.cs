using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.Application.Abstractions.ConectWithAI
{
    public interface IRecommenderEngine
    {
        Task<UserIntent> ExtractUserIntentAsync(string message, CancellationToken token);
    }
}
