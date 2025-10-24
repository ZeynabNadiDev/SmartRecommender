using Microsoft.EntityFrameworkCore;
using SmartRecommender.Domain.RepositoryInterfaces;
using SmartRecommender.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.Infrastructure.Repository
{
    public class ReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _entities;

        public ReadOnlyRepository(AppDbContext context)
        {
            _context = context;
            _entities = context.Set<TEntity>();
        }

        public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _entities.ToListAsync(cancellationToken);
        }

        public async Task<TEntity?> GetByIdAsync<TKey>(TKey key, CancellationToken cancellationToken)
        {
            return await _entities.FindAsync([key], cancellationToken);
        }
        public IQueryable<TEntity> Query()
        {
          return  _entities.AsNoTracking();
        }
    }
}
