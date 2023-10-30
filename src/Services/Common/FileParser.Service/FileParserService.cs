namespace FileParser.Service;

public class FileParserService : IFileParserService
{
    public IEnumerable<string> GetFullPaths(IEnumerable<string> filePaths, string? basePath = null)
    {
        // не прверяем basePath, тем самым давая пользователю возможность передавать как относительный так и абсолютный пути в filePaths

        var fullPaths = new List<string>();

        foreach (var path in filePaths)
        {
            fullPaths.Add(basePath != null ? Path.Combine(basePath, path) : path);
        }

        return fullPaths;
    }

    public async Task<IEnumerable<string>> GetFilesToStringsAsync(IEnumerable<string> fullPaths)
    {
        var filesToStrings = new List<string>();

        foreach (var fullPath in fullPaths)
        {
            var filetoString = await ReadFileAsync(fullPath);
            filesToStrings.Add(filetoString);
        }

        return filesToStrings;
    }

    private async Task<string> ReadFileAsync(string fullPath)
    {
        using (var reader = new StreamReader(fullPath))
            return await reader.ReadToEndAsync();
    }
}