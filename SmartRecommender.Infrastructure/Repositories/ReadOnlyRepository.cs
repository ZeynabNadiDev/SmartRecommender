using Microsoft.EntityFrameworkCore;
using SmartRecommender.Application.Abstractions.Repositories;
using SmartRecommender.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.Infrastructure.Repositories
{
    public class ReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity,TKey>
    where TEntity : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public ReadOnlyRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
            _dbSet=_context.Set<TEntity>();
        }
        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken)
        {
            return await _dbSet.FindAsync(new object?[] { id }, cancellationToken);
        }
    }
}
