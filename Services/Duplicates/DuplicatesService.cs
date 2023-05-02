using System.Security.Cryptography;

namespace Sniff.Services.Duplicates;

using Table;

public class DuplicatesService : AbstractService
{
    public override Table Search() // todo handle 0 bytes files
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

        var table = new Table();
        table.AddAllColumns(GetColumns());
        filtered.ForEach(row => table.AddSingleRow(row.Size.ToString(), row.MD5!, row.Path));
        return table;
    }

    private void CalculateMD5(IEnumerable<DuplicatesRow> duplicatesRows)
    {
        using var md5 = MD5.Create();
        foreach (var row in duplicatesRows)
        {
            // using (var stream = new FileStream(row.Path, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 + 1024, true)) // todo for big files (needs to be tested) 
            using var stream = new FileStream(row.Path, FileMode.Open, FileAccess.Read, FileShare.Read);
            var checksum = md5.ComputeHash(stream); // todo try to use async
            row.MD5 = BitConverter.ToString(checksum).Replace("-", "");
        }
    }

    private Column[] GetColumns()
    {
        var colSize = new Column
        {
            MinWidth = 7,
            MaxWidth = 20,
            ColumnWidth = ColumnWidth.Auto,
            ColumnPadding = ColumnPadding.FromLeft
        };

        var colHash = new Column
        {
            MinWidth = 32,
            MaxWidth = 32,
            ColumnWidth = ColumnWidth.Fixed,
            ColumnPadding = ColumnPadding.None
        };

        var colPath = new Column
        {
            MinWidth = 10,
            MaxWidth = 200,
            ColumnWidth = ColumnWidth.Fixed,
            ColumnPadding = ColumnPadding.None
        };

        return new[] { colSize, colHash, colPath };
    }
}