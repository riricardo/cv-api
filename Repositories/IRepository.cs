using System.Linq.Expressions;

namespace cv_api.Repositories;

public interface IRepository<T>
    where T : class
{
    Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default
    );

    Task<T?> FirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default
    );

    Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<T>> GetByIdsAsync(
        IReadOnlyCollection<string> ids,
        CancellationToken cancellationToken = default
    );

    Task<bool> ExistsByIdAsync(string id, CancellationToken cancellationToken = default);

    Task CreateAsync(T document, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(string id, T document, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
