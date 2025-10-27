using SmartRecommender.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.AI.Interfaces
{
    public interface IProductMatcher
    {
        Task<IEnumerable<Product>> MatchProductsAsync(UserIntent intent,
            CancellationToken cancellationToken);
    }
}
