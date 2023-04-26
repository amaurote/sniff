namespace Sniff.Services.Types;

public class SearchService : AbstractService // todo rename
{
    public ResultsDto Search()
    {
        Validate();

        var searchOption = Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        var paths = Directory.GetFileSystemEntries(BasePath, SearchPattern, searchOption);
        
        var results = new ResultsDto();
        results.Path = BasePath;
        
        foreach (var path in paths)
        {
            if (Directory.Exists(path))
            {
                results.DirectoryCount++;
                continue;
            }
            results.FileCount++;
            
            var ufm = File.GetUnixFileMode(path);
            if (ufm.HasFlag(UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute))
                results.ExecutableCount++;

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