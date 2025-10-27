using Microsoft.EntityFrameworkCore;
using SmartRecommender.Application.Abstractions.Repositories;
using SmartRecommender.Domain.Entities;
using SmartRecommender.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.Infrastructure.Repositories
{
    public class OrderRepository : ReadOnlyRepository<Order, int>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }
        public async Task<IReadOnlyList<Order>> GetOrdersByUserIdAsync(ulong userId, CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking()
                .Where(o => o.UserId == userId)
                .Include(o => o.Products)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync(cancellationToken);
        }
    }
}
