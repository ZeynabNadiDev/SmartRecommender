using SmartRecommender.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRecommender.Domain.Repositories
{
    public interface IUserRepository:IReadOnlyRepository<User,int>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
       
    }
}
