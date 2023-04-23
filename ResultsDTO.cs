namespace Sniff;

public class ResultsDto
{
    public int FileCount;
    public int DirectoryCount;
    public int ExecutableCount;
    public int WithoutExtensionCount;
    public Dictionary<string, int> Extensions = new();
}