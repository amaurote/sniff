namespace Sniff;

using System.IO;

public class SearchService
{
    public string BasePath { get; set; } = Directory.GetCurrentDirectory();
    public string SearchPattern { get; set; } = "*";
    public bool Recursive { get; set; } = false;

    public ResultsDto Search()
    {
        // validate
        if (!Path.Exists(BasePath))
            throw new SearchException("Invalid path!");

        if (!Directory.Exists(BasePath))
            throw new SearchException("The path specified is not a directory!");

        // search
        var results = new ResultsDto();
        
        var searchOption = Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        var paths = Directory.GetFiles(BasePath, SearchPattern, searchOption);

        // files and directories
        results.DirectoryCount = Directory.GetDirectories(BasePath, SearchPattern, searchOption).Length;
        results.FileCount = paths.Length;

        foreach (var path in paths)
        {
            // executables
            var ufm = File.GetUnixFileMode(path);
            if (ufm.HasFlag(UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute))
                results.ExecutableCount++;

            // extensions
            var extension = Path.GetExtension(path);

            if (string.IsNullOrEmpty(extension))
                results.WithoutExtensionCount++;
            else if (results.Extensions.TryGetValue(extension, out var value))
                results.Extensions[extension] = ++value;
            else
                results.Extensions.Add(extension, 1);
        }

        return results;
    }
}