using System.Security.Cryptography;

namespace Sniff.Services.Duplicates;

public class DuplicatesService : AbstractService
{
    public void Search() // todo handle 0 bytes files
    {
        Validate();

        var searchOption = Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        var paths = Directory.GetFiles(BasePath, SearchPattern, searchOption);

        var results = new List<DuplicatesRow>();
        foreach (var path in paths)
        {
            results.Add(new DuplicatesRow(path, new FileInfo(path).Length));
        }

        var duplicates = results
            .GroupBy(row => row.Size)
            .Where(g => g.Count() > 1)
            .SelectMany(x => x)
            .ToList();

        CalculateMD5(duplicates);

        var filtered = duplicates
            .GroupBy(row => row.MD5)
            .Where(g => g.Count() > 1)
            .SelectMany(x => x)
            .ToList();
        
        filtered.ForEach(row => Console.WriteLine("Size: {0}, MD5: {1}, Path: {2}", row.Size, row.MD5, row.Path));
    }

    private void CalculateMD5(IEnumerable<DuplicatesRow> duplicatesRows)
    {
        using (var md5 = MD5.Create())
        {
            foreach (var row in duplicatesRows)
            {
                // using (var stream = new FileStream(row.Path, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 + 1024, true)) // todo for big files (needs to be tested) 
                using (var stream = new FileStream(row.Path, FileMode.Open, FileAccess.Read, FileShare.Read)) // for small files ??
                {
                    var checksum = md5.ComputeHash(stream); // todo try to use async
                    row.MD5 = BitConverter.ToString(checksum).Replace("-", "");
                }
            }
        }
    }
}