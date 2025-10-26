using Microsoft.EntityFrameworkCore;
using SmartRecommender.Application.RepositoryInterfaces;
using SmartRecommender.Domain.AI.Models;
using SmartRecommender.Domain.Entities;
using SmartRecommender.Domain.RepositoryInterfaces;
using SmartRecommender.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.Infrastructure.Repository
{
    public class ProductQueryRepository:ReadOnlyRepository<Product>,IProductQueryRepository
    {
        public ProductQueryRepository(AppDbContext context) : base(context) { }

        public async Task  <List<Product>> GetRecommendedProductsAsync(FeatureVector vector, CancellationToken cancellationToken)
        {
            var query = _entities.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(vector.Category))
                query = query.Where(p => p.Category.Name == vector.Category);
            if (vector.MinPrice.HasValue)
                query = query.Where(p => p.Price >= vector.MinPrice.Value);

            if (vector.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= vector.MaxPrice.Value);

            if (vector.MinRating.HasValue)
                query = query.Where(p => p.AverageRating >= vector.MinRating.Value);

            return await  query.OrderByDescending(p=>p.PopularityScore).Take(10).ToListAsync(cancellationToken);
        }
    }
}
