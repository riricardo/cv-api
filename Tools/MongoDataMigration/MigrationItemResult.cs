namespace MongoDataMigration;

public class MigrationItemResult
{
    private MigrationItemResult(MigrationItemStatus status, int? statusCode = null, string? responseBody = null)
    {
        Status = status;
        StatusCode = statusCode;
        ResponseBody = responseBody;
    }

    public MigrationItemStatus Status { get; }

    public int? StatusCode { get; }

    public string? ResponseBody { get; }

    public static MigrationItemResult Inserted() => new(MigrationItemStatus.Inserted);

    public static MigrationItemResult Ignored() => new(MigrationItemStatus.Ignored);

    public static MigrationItemResult Failed(int statusCode, string responseBody)
    {
        return new MigrationItemResult(MigrationItemStatus.Failed, statusCode, responseBody);
    }
}

public enum MigrationItemStatus
{
    Inserted,
    Ignored,
    Failed
}
