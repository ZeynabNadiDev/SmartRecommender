using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.Domain.RepositoryInterfaces
{
    public interface IReadOnlyRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync<TKey>(TKey key, CancellationToken cancellationToken);
        Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        IQueryable<TEntity> Query();
    }
}
