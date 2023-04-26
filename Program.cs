using Sniff.Services.Duplicates;
using Sniff.Services.Types;

namespace Sniff;

internal static class Program
{
    /*
     *  sniff
     *  sniff -r
     *  sniff -r /home/alfonz/dev/java
     *  sniff -r -p --dir /home/alfonz/dev/java --pattern "*.j???"
     *  sniff --pattern "*.???"
     *
     *  sniff duplicatxes -r
     *  sniff sniff
     */
    
    private static void Main(string[] args)
    {
        var searchService = new SearchService
        {
            BasePath = "/home/nineveh/dev/tools/godot/",
            Recursive = true,
            // SearchPattern = "*.j???"
        };
        
        var duplicateService = new DuplicatesService()
        {
            BasePath = "/home/nineveh/dev/",
            Recursive = true,
            // SearchPattern = "*.j???"
        };

        duplicateService.Search();

        var results = searchService.Search();

        var printer = new PrinterService(results);
        
        printer.PrintBasicInfo();
        Console.WriteLine("\nTypes:");
        // printer.PrintExtensions();
        // printer.PrintExtensions(10);
        printer.PrintExtensionsPage(Console.LargestWindowHeight - 4, 1);
    }
}