namespace cv_api.Repositories;

public static class DocumentId
{
    public static string Get<T>(T document)
    {
        var idProperty = typeof(T).GetProperty("Id");
        var id = idProperty?.GetValue(document) as string;

        if (string.IsNullOrWhiteSpace(id))
        {
            throw new InvalidOperationException($"{typeof(T).Name} must have an Id value.");
        }

        return id;
    }
}
