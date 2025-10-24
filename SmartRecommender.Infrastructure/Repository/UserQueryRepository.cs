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
    public class UserQueryRepository : ReadOnlyRepository<User>, IUserQueryRepository
    {
        public UserQueryRepository(AppDbContext context) : base(context) { }

        public async Task<User?> GetUserByEmailAsync(string email, CancellationToken ct)
        {
            return await _entities.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
                
        }
    }
}
