using Microsoft.EntityFrameworkCore;
using SmartRecommender.Application.RepositoryInterfaces;
using SmartRecommender.Domain.Entities;
using SmartRecommender.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.Infrastructure.Repository
{
    public class OrderQueryRepository:ReadOnlyRepository<Order>,IOrderQueryRepository
    {
        public OrderQueryRepository(AppDbContext context) : base(context) { }
        public async Task<List<Order>> GetOrdersByUserIdAsync(ulong userId,
            CancellationToken cancellationToken)
        {
            return await _entities.AsNoTracking()
                .Include(o => o.Products)
                .Where(o => o.UserId == userId)
                .ToListAsync(cancellationToken);
        }
    }
}
