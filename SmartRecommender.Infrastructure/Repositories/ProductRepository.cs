using Microsoft.EntityFrameworkCore;
using SmartRecommender.Application.Abstractions.Repositories;
using SmartRecommender.Domain.AI.Models;
using SmartRecommender.Domain.Entities;
using SmartRecommender.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.Infrastructure.Repositories
{
    public class ProductRepository : ReadOnlyRepository<Product,int>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) {}

        public async Task<IReadOnlyList<Product>> MatchProductsAsync(UserIntent intent, CancellationToken cancellationToken)
        {
            IQueryable<Product> query = _dbSet
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(intent.Category))
            {
                var categoryName = intent.Category.Trim().ToLower();
                query = query.Where(p => p.Category.Name.ToLower() == categoryName);
            }
            if (intent.Filters.MinPrice.HasValue)
                query = query.Where(p => p.Price >= intent.Filters.MinPrice.Value);

            if (intent.Filters.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= intent.Filters.MaxPrice.Value);

            if (intent.Keywords.Any())
            {
                foreach (var keyword in intent.Keywords)
                {
                    var lowerKeyword = keyword.ToLower();
                    query = query.Where(p =>
                        p.Name.ToLower().Contains(lowerKeyword) ||
                        (p.Description != null && p.Description.ToLower().Contains(lowerKeyword)));
                }
            }
            if (!string.IsNullOrEmpty(intent.Filters.Brand))
            {
                query = query.Where(p => EF.Property<string>(p, "Brand") == intent.Filters.Brand);
            }
            if (!string.IsNullOrEmpty(intent.Filters.Color))
            {
                query = query.Where(p => EF.Property<string>(p, "Color") == intent.Filters.Color);
            }

            if (!string.IsNullOrEmpty(intent.Filters.Feature))
            {
                query = query.Where(p => EF.Property<string>(p, "Feature").Contains(intent.Filters.Feature));
            }
            if (!string.IsNullOrEmpty(intent.PurchaseIntent))
            {
                switch (intent.PurchaseIntent.ToLower())
                {
                    case "immediatebuy":
                        query = query.OrderByDescending(p => p.Discount.HasValue)
                                     .ThenBy(p => p.Price);
                        break;
                    case "research":
                        query = query.OrderByDescending(p => p.AverageRating)
                                    .ThenByDescending(p => p.PopularityScore);
                        break;
                }
            }
            if (intent.Confidence >= 0.8f)
                query = query.Take(25);
            else
                query = query.Take(50);

            return await query.AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
