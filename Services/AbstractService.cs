namespace Sniff.Services;

using Table;

public abstract class AbstractService
{
    public static string BasePath { get; set; } = Directory.GetCurrentDirectory();
    public static string SearchPattern { get; set; } = "*";
    public static bool Recursive { get; set; } = false;

    public abstract Table Search();
    
    protected void Validate()
    {
        if (!Path.Exists(BasePath))
            throw new IOException("Invalid path!");

        if (!Directory.Exists(BasePath))
            throw new IOException("The path specified is not a directory!");
    }
}