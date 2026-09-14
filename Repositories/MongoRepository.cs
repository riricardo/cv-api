using MongoDB.Driver;
using cv_api.Data;

namespace cv_api.Repositories;

public class MongoRepository<T> : IRepository<T>
    where T : class
{
    private readonly IMongoCollection<T> _collection;

    public MongoRepository(MongoDbContext mongoDbContext)
    {
        _collection = mongoDbContext.Database.GetCollection<T>(MongoCollectionNames.GetName<T>());
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _collection.Find(Builders<T>.Filter.Empty).ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _collection.Find(BuildIdFilter(id)).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _collection.Find(BuildIdFilter(id)).AnyAsync(cancellationToken);
    }

    public async Task CreateAsync(T document, CancellationToken cancellationToken = default)
    {
        await _collection.InsertOneAsync(document, cancellationToken: cancellationToken);
    }

    public async Task<bool> UpdateAsync(string id, T document, CancellationToken cancellationToken = default)
    {
        var result = await _collection.ReplaceOneAsync(
            BuildIdFilter(id),
            document,
            cancellationToken: cancellationToken
        );

        return result.MatchedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await _collection.DeleteOneAsync(BuildIdFilter(id), cancellationToken);

        return result.DeletedCount > 0;
    }

    private static FilterDefinition<T> BuildIdFilter(string id)
    {
        return Builders<T>.Filter.Or(
            Builders<T>.Filter.Eq("_id", id),
            Builders<T>.Filter.Eq("Id", id)
        );
    }
}
