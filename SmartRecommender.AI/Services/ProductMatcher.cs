using SmartRecommender.AI.Interfaces;
using SmartRecommender.Application.Abstractions.Repositories;
using SmartRecommender.Domain.Entities;
using SmartRecommender.Domain.AI.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SmartRecommender.AI.Services
{
    public class ProductMatcher : IProductMatcher
    {
        private readonly IProductRepository _productRepository;

        public ProductMatcher(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> MatchProductsAsync(UserIntent intent, CancellationToken cancellationToken = default)
        {
            // ✅ This method simply delegates to repository but may include
            // future scoring / ranking logic before returning results.
            var products = await _productRepository.MatchProductsAsync(intent, cancellationToken);
            return products;
        }
    }
}
