using System;
using System.IO;

namespace GreenShop;

internal static class DotEnvFile
{
    private static readonly object SyncRoot = new();
    private static bool _loaded;

    public static void Load()
    {
        lock (SyncRoot)
        {
            if (_loaded)
            {
                return;
            }

            var path = FindProjectEnvFile();
            if (path is null)
            {
                _loaded = true;
                return;
            }

            foreach (var rawLine in File.ReadLines(path))
            {
                var line = rawLine.Trim();
                if (line.Length == 0 || line.StartsWith('#'))
                {
                    continue;
                }

                if (line.StartsWith("export ", StringComparison.Ordinal))
                {
                    line = line[7..].TrimStart();
                }

                var separator = line.IndexOf('=');
                if (separator <= 0)
                {
                    continue;
                }

                var name = line[..separator].Trim();
                if (name.Length == 0 || Environment.GetEnvironmentVariable(name) is not null)
                {
                    continue;
                }

                var value = line[(separator + 1)..].Trim();
                if (value.Length >= 2 &&
                    ((value[0] == '"' && value[^1] == '"') ||
                     (value[0] == '\'' && value[^1] == '\'')))
                {
                    var quote = value[0];
                    value = value[1..^1];
                    if (quote == '"')
                    {
                        value = UnescapeDoubleQuotedValue(value);
                    }
                }

                Environment.SetEnvironmentVariable(name, value);
            }

            _loaded = true;
        }
    }

    private static string? FindProjectEnvFile()
    {
        var startPaths = new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory };
        foreach (var startPath in startPaths)
        {
            var directory = new DirectoryInfo(startPath);
            while (directory is not null)
            {
                string? candidate = null;
                if (File.Exists(Path.Combine(directory.FullName, "GreenShop.csproj")))
                {
                    candidate = Path.Combine(directory.FullName, ".env");
                }
                else if (File.Exists(Path.Combine(directory.FullName, "GreenShop", "GreenShop.csproj")))
                {
                    candidate = Path.Combine(directory.FullName, "GreenShop", ".env");
                }

                if (candidate is not null && File.Exists(candidate))
                {
                    return candidate;
                }

                directory = directory.Parent;
            }
        }

        return null;
    }

    private static string UnescapeDoubleQuotedValue(string value)
    {
        var result = new System.Text.StringBuilder(value.Length);
        for (var i = 0; i < value.Length; i++)
        {
            if (value[i] == '\\' && i + 1 < value.Length)
            {
                var escaped = value[++i];
                result.Append(escaped switch
                {
                    '\\' => '\\',
                    '"' => '"',
                    'n' => '\n',
                    'r' => '\r',
                    't' => '\t',
                    _ => '\\'
                });
                if (escaped is not ('\\' or '"' or 'n' or 'r' or 't'))
                {
                    result.Append(escaped);
                }
            }
            else
            {
                result.Append(value[i]);
            }
        }

        return result.ToString();
    }
}
