namespace Sniff;

internal static class Program
{
    /*
     *  sniff
     *  sniff -r
     *  sniff -r /home/alfonz/dev/java
     *  sniff -r -p --dir /home/alfonz/dev/java --pattern "*.j???"
     *  sniff --pattern "*.???"
     */
    
    private static void Main(string[] args)
    {
        var searchService = new SearchService
        {
            BasePath = "/home/nineveh/dev/tools/godot/",
            Recursive = true,
            // SearchPattern = "*.j???"
        };

        var results = searchService.Search();

        var printer = new PrinterService(results);
        
        printer.PrintBasicInfo();
        Console.WriteLine("\nTypes:");
        // printer.PrintExtensions();
        // printer.PrintExtensions(10);
        printer.PrintExtensionsPage(Console.LargestWindowHeight - 4, 1);
    }
}