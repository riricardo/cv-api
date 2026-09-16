namespace cv_api.Repositories;

public static class DocumentTimestamps
{
    public static void SetCreated<T>(T document, DateTime timestamp)
    {
        SetValue(document, "CreatedAt", timestamp);
        SetValue(document, "UpdatedAt", timestamp);
    }

    public static void SetUpdated<T>(T document, T currentDocument, DateTime timestamp)
    {
        var createdAt = GetValue(currentDocument, "CreatedAt");

        if (createdAt is not null)
        {
            SetValue(document, "CreatedAt", createdAt.Value);
        }

        SetValue(document, "UpdatedAt", timestamp);
    }

    private static DateTime? GetValue<T>(T document, string propertyName)
    {
        var property = typeof(T).GetProperty(propertyName);

        return property?.GetValue(document) as DateTime?;
    }

    private static void SetValue<T>(T document, string propertyName, DateTime value)
    {
        var property = typeof(T).GetProperty(propertyName);

        if (property is null || property.PropertyType != typeof(DateTime) || !property.CanWrite)
        {
            return;
        }

        property.SetValue(document, value);
    }
}
