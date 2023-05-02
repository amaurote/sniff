using Sniff.Table;

namespace Sniff.Services;

public class TypesService : AbstractService // todo rename
{
    public override Table.Table Search()
    {
        Validate();

        var searchOption = Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        var paths = Directory.GetFiles(BasePath, SearchPattern, searchOption);

        var withoutExtension = 0;
        var extensions = new Dictionary<string, int>();

        foreach (var path in paths)
        {
            var extension = Path.GetExtension(path);
            if (string.IsNullOrEmpty(extension))
                withoutExtension++;
            else if (extensions.TryGetValue(extension, out var value))
                extensions[extension] = ++value;
            else
                extensions.Add(extension, 1);
        }

        var colType = new Column
        {
            MinWidth = 19,
            MaxWidth = 30,
            ColumnWidth = ColumnWidth.Auto,
            ColumnPadding = ColumnPadding.FromLeft
        };

        var colCount = new Column
        {
            MinWidth = 0,
            MaxWidth = 15,
            ColumnWidth = ColumnWidth.Fixed,
            ColumnPadding = ColumnPadding.None
        };

        var table = new Table.Table();
        table.AddAllColumns(colType, colCount);
        table.AddSingleRow("<without extension>", withoutExtension.ToString());
        foreach (var (key, value) in extensions.OrderByDescending(key => key.Value))
        {
            table.AddSingleRow(key, value.ToString());
        }

        return table;
    }
}