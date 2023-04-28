namespace Sniff.Table;

public class Table
{
    private readonly List<Column> _columns = new();
    private readonly List<string[]> _rows = new();

    public string Spacer { get; set; } = "  ";

    public void AddColumn(Column column)
    {
        _columns.Add(column);
    }
    
    public void AddAllColumns(params Column[] columns)
    {
        _columns.AddRange(columns);
    }
    
    public void AddSingleRow(params string[] row)
    {
        _rows.Add(row);
    }

    public IEnumerable<Column> GetColumns()
    {
        return _columns;
    }

    public IEnumerable<string[]> GetRows()
    {
        return _rows;
    }

}