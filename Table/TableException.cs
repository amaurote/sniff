namespace Sniff.Table;

public class TableException : IOException
{
    public TableException(string? message) : base(message)
    {
    }
}