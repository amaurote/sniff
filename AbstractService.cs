namespace Sniff;

public abstract class AbstractService
{
    public string BasePath { get; set; } = Directory.GetCurrentDirectory();
    public string SearchPattern { get; set; } = "*";
    public bool Recursive { get; set; } = false;

    protected void Validate()
    {
        if (!Path.Exists(BasePath))
            throw new SearchException("Invalid path!");

        if (!Directory.Exists(BasePath))
            throw new SearchException("The path specified is not a directory!");
    }
}