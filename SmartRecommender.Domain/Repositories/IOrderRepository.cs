using SmartRecommender.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.Domain.Repositories
{
    public interface IOrderRepository:IReadOnlyRepository<Order,int>
    {
        Task<IReadOnlyList<Order>> GetOrdersByUserIdAsync(ulong userId,
            CancellationToken cancellationToken);
    }
}
