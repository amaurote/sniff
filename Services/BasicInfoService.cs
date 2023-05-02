namespace Sniff.Services;

using Table;

public class BasicInfoService : AbstractService
{
    public override Table Search()
    {
        Validate();

        var searchOption = Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        var files = Directory.GetFiles(BasePath, SearchPattern, searchOption);

        var fileCount = files.Length;
        var dirCount = Directory.GetDirectories(BasePath, SearchPattern, searchOption).Length;
        var exeCount = files
            .Select(File.GetUnixFileMode)
            .Count(ufm =>
                ufm.HasFlag(UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute));

        var labelCol = new Column
        {
            MinWidth = 0,
            MaxWidth = 100,
            ColumnWidth = ColumnWidth.Auto,
            ColumnPadding = ColumnPadding.FromLeft
        };

        var resCol = new Column
        {
            MinWidth = 0,
            MaxWidth = 200,
            ColumnWidth = ColumnWidth.Auto,
            ColumnPadding = ColumnPadding.None
        };

        var table = new Table();
        table.AddAllColumns(labelCol, resCol);
        table.AddSingleRow("Path:", BasePath);
        table.AddSingleRow("Search option:", Recursive ? "THIS AND ALL SUB-DIRECTORIES" : "THIS DIRECTORY ONLY");
        table.AddSingleRow("Search pattern:", $"\"{SearchPattern}\"");
        table.AddSingleRow("", "");
        table.AddSingleRow("Files count:", fileCount.ToString());
        table.AddSingleRow("Directories count:", dirCount.ToString());
        table.AddSingleRow("Executables count:", exeCount.ToString());
        return table;
    }
}