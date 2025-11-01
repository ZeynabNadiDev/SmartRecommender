using Microsoft.EntityFrameworkCore;
using SmartRecommender.Application.Abstractions.Repositories;
using SmartRecommender.Domain.AI.Models;
using SmartRecommender.Domain.Entities;
using SmartRecommender.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartRecommender.Infrastructure.Repositories
{
    public class ProductRepository : ReadOnlyRepository<Product, int>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public async Task<IReadOnlyList<Product>> MatchProductsAsync(UserIntent intent, CancellationToken cancellationToken)
        {
            IQueryable<Product> baseQuery = _dbSet
                .Include(p => p.Category)
                .AsQueryable();

            //-------------------------------------------------
            // 1. Category Filter
            //-------------------------------------------------
            IQueryable<Product> categoryQuery = baseQuery.Where(p => false);
            string category = NormalizeCategory(intent.Category);

            if (!string.IsNullOrWhiteSpace(category))
            {
                string pattern = $"%{category.Replace(" ", "").Replace("‌", "")}%";
                categoryQuery = baseQuery.Where(p =>
                    EF.Functions.Like(
                        p.Category.Name.Replace(" ", "").Replace("‌", ""), pattern
                    ));
            }

            //-------------------------------------------------
            // 2. Keyword Filter
            //-------------------------------------------------
            IQueryable<Product> keywordQuery = baseQuery.Where(p => false);

            if (intent.Keywords != null && intent.Keywords.Any())
            {
                foreach (string kw in intent.Keywords.Select(k => k.Trim().ToLower()))
                {
                    keywordQuery = keywordQuery.Concat(baseQuery.Where(p =>
                        EF.Functions.Like(p.Name.ToLower(), $"%{kw}%") ||
                        (p.Description != null && EF.Functions.Like(p.Description.ToLower(), $"%{kw}%"))
                    ));
                }
            }

            //-------------------------------------------------
            // 3. Combine Category + Keyword (OR)
            //-------------------------------------------------
            IQueryable<Product> combined = categoryQuery.Concat(keywordQuery).Distinct();

            if (string.IsNullOrWhiteSpace(category) && (intent.Keywords == null || !intent.Keywords.Any()))
                combined = baseQuery;

            //-------------------------------------------------
            // 4. Price Filters
            //-------------------------------------------------
            if (intent.Filters.MinPrice.HasValue)
                combined = combined.Where(p => p.Price >= intent.Filters.MinPrice.Value);

            if (intent.Filters.MaxPrice.HasValue)
                combined = combined.Where(p => p.Price <= intent.Filters.MaxPrice.Value);

            //-------------------------------------------------
            // 5. Sorting by PurchaseIntent
            //-------------------------------------------------
            ApplySortingByIntent(ref combined, intent);

            int takeCount = intent.Confidence >= 0.8f ? 25 : 50;

            return await combined.AsNoTracking()
                                 .Take(takeCount)
                                 .ToListAsync(cancellationToken);
        }

        //-------------------------------------------------
        // ✅ Category Normalizer (Persian ↔ English)
        //-------------------------------------------------
        private static string NormalizeCategory(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            input = input.Trim().Replace("‌", "").Replace(" ", "").ToLower();

            return input switch
            {
                "laptop" or "notebook" or "computer" or "لپتاپ" or "لپتا" or "لپ‌تاپ" => "لپ‌تاپ",
                "mobile" or "phone" or "cellphone" or "smartphone" or "موبایل" => "موبایل و گجت",
                "tv" or "audio" or "sound" or "speaker" or "صوتی" or "تصویری" => "صوتی تصویری",
                "home" or "house" or "kitchen" or "appliance" or "خانگی" => "لوازم خانگی",
                "clothing" or "fashion" or "dress" or "پوشاک" => "پوشاک",
                "book" or "study" or "novel" or "کتاب" => "کتاب و فرهنگی",
                "sport" or "gym" or "ورزشی" => "ورزشی",
                "beauty" or "health" or "healthcare" or "زیبایی" or "سلامتی" => "زیبایی و سلامتی",
                _ => input
            };
        }

        //-------------------------------------------------
        // ✅ Sorting Helper
        //-------------------------------------------------
        private static void ApplySortingByIntent(ref IQueryable<Product> query, UserIntent intent)
        {
            if (string.IsNullOrWhiteSpace(intent.PurchaseIntent)) return;

            string type = intent.PurchaseIntent.Trim().ToLower();

            if (type == "readytobuy")
                query = query.OrderByDescending(p => p.Discount ?? 0).ThenBy(p => p.Price);
            else if (type == "interested")
                query = query.OrderByDescending(p => p.AverageRating)
                             .ThenByDescending(p => p.PopularityScore);
            else if (type == "browsing")
                query = query.OrderByDescending(p => p.PopularityScore);
        }
    }
}
