namespace Sniff.Table;

public class Column
{
    public int MinWidth { get; init; } = 20;
    public int MaxWidth { get; init; } = 40;
    public ColumnWidth ColumnWidth { get; init; } = ColumnWidth.Fixed;
    public ColumnPadding ColumnPadding { get; init; } = ColumnPadding.FromRight;
}