namespace MongoDataMigration;

public record MigrationLogResult(
    string Route,
    string Id,
    string Status,
    bool IsError,
    string? Details = null
);
