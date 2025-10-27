using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.AI.Interfaces
{
    public interface IIntentExtractor
    {
        Task<UserIntent> ExtractIntentAsync(string userInput,
            CancellationToken cancellationToken);
    }
}
