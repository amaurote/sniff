using Sniff.Services.BasicInfo;
using Sniff.Services.Duplicates;
using Sniff.Services.Types;
using Sniff.Table;

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
     *  sniff duplicates -r
     *  sniff sniff
     */
    
    private static void Main(string[] args)
    {
        var basicInfoService = new BasicInfoService()
        {
            BasePath = "/home/nineveh/dev/tools/godot/",
            Recursive = true,
            // SearchPattern = "*.j???"
        };
        
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

        var basic = basicInfoService.Search();
        var types = searchService.Search();
        var duples = duplicateService.Search();

        // TablePrinter.Print(basic);
        // Console.WriteLine();
        // TablePrinter.Print(types);
        TablePrinter.Print(duples);


    }
}