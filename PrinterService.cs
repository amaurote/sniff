namespace Sniff;

public class PrinterService
{
    private readonly ResultsDto _data;
    private int _minColumnWidth;
    private readonly int _maxColumnWidth;

    public PrinterService(ResultsDto data, int minColumnWidth = 19, int maxColumnWidth = 40)
    {
        _data = data;
        _minColumnWidth = Math.Max(minColumnWidth, 19);
        _maxColumnWidth = maxColumnWidth;
    }

    public void PrintPage(int page, int limit)
    {
        throw new NotImplementedException();
    }
    
    public void Print(int limit)
    {
        throw new NotImplementedException();
    }
    
    public void Print()
    {
        Console.WriteLine("Files: {0}", _data.FileCount);
        Console.WriteLine("Directories: {0}", _data.DirectoryCount);
        Console.WriteLine("Executables: {0}", _data.ExecutableCount);
        Console.WriteLine("\nTypes:");

        var longest = _data.Extensions.Keys.OrderByDescending(key => key.Length).First().Length;
        _minColumnWidth = Math.Max(_minColumnWidth, longest);

        Console.WriteLine("{0}    {1}", "<without extension>".PadLeft(_minColumnWidth), _data.WithoutExtensionCount);
        foreach (var pair in _data.Extensions.OrderByDescending(key => key.Value).Take(10))
        {
            var extension = pair.Key;
            if (extension.Length > _maxColumnWidth)
                extension = extension[..(_maxColumnWidth - 1)] + '~';

            Console.WriteLine("{0}    {1}", extension.PadLeft(_minColumnWidth), pair.Value);
        }
    }
}