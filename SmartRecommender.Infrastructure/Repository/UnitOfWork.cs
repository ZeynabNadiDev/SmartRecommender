using SmartRecommender.Application.RepositoryInterfaces;
using SmartRecommender.Domain.RepositoryInterfaces;
using SmartRecommender.Infrastructure.Context;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

  

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
       
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        => _context.SaveChangesAsync(cancellationToken);

    public void Dispose() => _context.Dispose();
}
