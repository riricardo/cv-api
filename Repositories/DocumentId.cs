namespace cv_api.Repositories;

public static class DocumentId
{
    public static string Get<T>(T document)
    {
        var id = GetValue(document);

        if (string.IsNullOrWhiteSpace(id))
        {
            throw new InvalidOperationException($"{typeof(T).Name} must have an Id value.");
        }

        return id;
    }

    public static string CreateIfMissing<T>(T document)
    {
        var id = GetValue(document);

        if (string.IsNullOrWhiteSpace(id))
        {
            id = Guid.NewGuid().ToString();
            Set(document, id);

            return id;
        }

        if (!Guid.TryParse(id, out _))
        {
            throw new InvalidOperationException($"{typeof(T).Name} Id must be a valid GUID.");
        }

        return id;
    }

    public static void Set<T>(T document, string id)
    {
        var idProperty = typeof(T).GetProperty("Id");

        if (idProperty is null || idProperty.PropertyType != typeof(string) || !idProperty.CanWrite)
        {
            throw new InvalidOperationException($"{typeof(T).Name} must have a writable string Id property.");
        }

        idProperty.SetValue(document, id);
    }

    public static bool IsValid(string id)
    {
        return Guid.TryParse(id, out _);
    }

    public static string GetOrUnknown<T>(T document)
    {
        return GetValue(document) ?? "(no id)";
    }

    private static string? GetValue<T>(T document)
    {
        var idProperty = typeof(T).GetProperty("Id");

        return idProperty?.GetValue(document) as string;
    }
}
