using SmartRecommender.Domain.AI.Models;
using SmartRecommender.Domain.Entities;

namespace SmartRecommender.Application.Abstractions.Repositories
{
    public interface IProductRepository:IReadOnlyRepository<Product,int>
    {
        Task<IReadOnlyList<Product>> MatchProductsAsync(UserIntent intent,
            CancellationToken cancellationToken);
    }
}
