namespace Sniff;

public class PrinterService
{
    private readonly ResultsDto _data;
    private int _minColumnWidth;
    private int _maxColumnWidth;
    private List<KeyValuePair<string, int>> _orderedExtensions;

    public PrinterService(ResultsDto data, int minColumnWidth = 19, int maxColumnWidth = 40)
    {
        _minColumnWidth = Math.Max(minColumnWidth, 19);
        _maxColumnWidth = maxColumnWidth;
        _data = data;

        _orderedExtensions = new List<KeyValuePair<string, int>>
            { new("<without extension>", _data.WithoutExtensionCount) };
        _orderedExtensions.InsertRange(1, _data.Extensions.OrderByDescending(key => key.Value).ToList());
    }

    public void PrintBasicInfo()
    {
        Console.WriteLine("Working Dir: {0}", _data.Path);
        Console.WriteLine("Files: {0}", _data.FileCount);
        Console.WriteLine("Directories: {0}", _data.DirectoryCount);
        if (_data.ExecutableCount > 0)
            Console.WriteLine("Executables: {0}", _data.ExecutableCount);
    }

    public void PrintExtensionsPage(int pageSize, int page) =>
        WriteLines(_orderedExtensions.Skip((page - 1) * pageSize).Take(pageSize).ToList());

    public void PrintExtensions(int limit) => 
        WriteLines(_orderedExtensions.Take(limit).ToList());

    public void PrintExtensions() => 
        WriteLines(_orderedExtensions);

    private void WriteLines(List<KeyValuePair<string, int>> pairs)
    {
        var longest = pairs.MaxBy(row => row.Key.Length).Key.Length;
        _minColumnWidth = Math.Max(_minColumnWidth, longest);

        foreach (var pair in pairs)
        {
            var extension = pair.Key;
            if (extension.Length > _maxColumnWidth)
                extension = extension.Substring(0, _maxColumnWidth - 1) + '~';

            Console.WriteLine("{0}    {1}", extension.PadLeft(_minColumnWidth), pair.Value);
        }
    }
}