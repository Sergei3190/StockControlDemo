namespace FileParser.Service;

public interface IFileParserService
{
    IEnumerable<string> GetFullPaths(IEnumerable<string> filePaths, string? basePath = null);
    Task<IEnumerable<string>> GetFilesToStringsAsync(IEnumerable<string> fullPaths);
}