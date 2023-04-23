namespace Sniff;

public class SearchException : IOException
{
    public SearchException(string message) : base(message)
    {
    }
}