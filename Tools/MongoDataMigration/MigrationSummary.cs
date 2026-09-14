namespace MongoDataMigration;

public class MigrationSummary
{
    public int Inserted { get; set; }

    public int Ignored { get; set; }

    public int Failed { get; set; }
}
