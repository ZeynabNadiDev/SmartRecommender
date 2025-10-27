

namespace SmartRecommender.Application.Abstractions.Repositories
{
    public interface IReadOnlyRepository<TEntity, TKey> where TEntity : class
    {
        Task <TEntity?> GetByIdAsync(TKey id,CancellationToken cancellationToken);
        Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    }
}
