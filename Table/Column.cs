namespace Sniff.Table;

public class Column
{
    public int MinWidth { get; set; } = 20;
    public int MaxWidth { get; set; } = 40;
    public ColumnWidth ColumnWidth { get; set; } = ColumnWidth.Fixed;
    public ColumnPadding ColumnPadding { get; set; } = ColumnPadding.FromRight;
}