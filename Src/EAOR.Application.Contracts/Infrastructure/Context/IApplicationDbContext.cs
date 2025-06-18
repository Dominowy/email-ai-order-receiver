

using Microsoft.EntityFrameworkCore;

namespace EAOR.Application.Contracts.Infrastructure.Context
{
    public interface IApplicationDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
