namespace cv_api.Repositories;

public interface IRepository<T>
    where T : class
{
    Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<bool> ExistsByIdAsync(string id, CancellationToken cancellationToken = default);

    Task CreateAsync(T document, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(string id, T document, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
}
