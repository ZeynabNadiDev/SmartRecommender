using SmartRecommender.Domain.Entities;
using SmartRecommender.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.Application.RepositoryInterfaces
{
    public interface IOrderQueryRepository:IReadOnlyRepository<Order>
    {
        Task<List<Order>> GetOrdersByUserIdAsync(ulong userId, CancellationToken cancellationToken);
    }
}
