namespace MongoDataMigration;

public static class ConsolePrompt
{
    public static string ReadValue(string label, string defaultValue)
    {
        Console.Write($"{label} [{defaultValue}]: ");
        var value = Console.ReadLine();

        return string.IsNullOrWhiteSpace(value) ? defaultValue : value.Trim();
    }

    public static string ReadSecret(string label)
    {
        Console.Write($"{label}: ");
        var secret = string.Empty;

        while (true)
        {
            var key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                return secret;
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (secret.Length > 0)
                {
                    secret = secret[..^1];
                }

                continue;
            }

            secret += key.KeyChar;
        }
    }
}
