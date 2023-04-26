namespace Sniff.Services.Duplicates;

public class DuplicatesRow
{
    public string Path;
    public long Size;
    public string? MD5 = null;

    public DuplicatesRow(string path, long size)
    {
        Path = path;
        Size = size;
    }
}