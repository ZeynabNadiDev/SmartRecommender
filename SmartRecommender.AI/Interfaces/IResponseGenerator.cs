using SmartRecommender.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.AI.Interfaces
{
    public interface IResponseGenerator
    {
        Task<string> GenerateResponseAsync(UserIntent intent, IEnumerable<Product> matchedProducts
            , CancellationToken cancellationToken);
    }
}
