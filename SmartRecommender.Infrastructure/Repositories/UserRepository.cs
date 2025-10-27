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
    public class UserRepository: ReadOnlyRepository<User, int>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
          return await _dbSet.AsNoTracking().
                FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower(), cancellationToken);
        }
    }
}
