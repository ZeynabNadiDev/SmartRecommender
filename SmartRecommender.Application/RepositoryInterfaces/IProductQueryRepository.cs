using SmartRecommender.Domain.AI.Models;
using SmartRecommender.Domain.Entities;
using SmartRecommender.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.Application.RepositoryInterfaces
{
    public interface IProductQueryRepository: IReadOnlyRepository<Product>
    {
        Task<List<Product>> GetRecommendedProductsAsync(FeatureVector vector,
            CancellationToken cancellationToken);
    }
}
