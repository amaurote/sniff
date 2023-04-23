namespace Sniff;

internal static class Program
{
    private static void Main(string[] args)
    {
        var searchService = new SearchService
        {
            BasePath = "/home/nineveh/dev/tools/godot/",
            Recursive = true,
            // SearchPattern = "*.java"
        };

        var results = searchService.Search();

        var printer = new PrinterService(results);
        
        printer.PrintBasicInfo();
        Console.WriteLine("\nTypes:");
        printer.PrintExtensions();
        // printer.PrintExtensions(10);
        // printer.PrintExtensionsPage(13, 2);
    }
}