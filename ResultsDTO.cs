namespace Sniff;

public class ResultsDto
{
    public string Path = "";
    public int FileCount;
    public int DirectoryCount;
    public int ExecutableCount;
    public int WithoutExtensionCount;
    public readonly Dictionary<string, int> Extensions = new();
}