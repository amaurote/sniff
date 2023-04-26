namespace Sniff.Table;

public class TablePrinter
{
    public List<Column> Columns { get; } = new();
    public string Spacer { get; set; } = "  ";
    private List<string[]> _rows = new();

    public void AddRow(string[] row)
    {
        _rows.Add(row);
    }

    public void PrintTable()
    {
        Validate();

        List<Tuple<Column, int>> columnTuples = new();
        for (var i = 0; i < Columns.Count; i++)
        {
            var column = Columns[i];
            if (column.ColumnWidth == ColumnWidth.Fixed)
            {
                columnTuples.Add(new Tuple<Column, int>(column, column.MaxWidth));
            }
            else
            {
                var width = Math.Clamp(GetLongestString(i), column.MinWidth, column.MaxWidth);
                columnTuples.Add(new Tuple<Column, int>(column, width));
            }
        }

        _rows.ForEach(row =>
        {
            for (var i = 0; i < row.Length; i++)
            {
                var column = columnTuples[i].Item1;
                var width = columnTuples[i].Item2;
                var value = row[i];

                if (value.Length > width)
                    value = value.Substring(0, width - 1) + "~";

                value = column.ColumnPadding == ColumnPadding.FromLeft
                    ? value.PadLeft(width)
                    : value.PadRight(width);

                Console.Write(value);
                if (i + 1 != row.Length)
                    Console.Write(Spacer);
            }

            Console.WriteLine();
        });
    }

    private void Validate()
    {
        _rows.ForEach(row =>
        {
            if (row.Length != Columns.Count)
                throw new InvalidDataException("Data inconsistency!");
        });
    }

    private int GetLongestString(int column)
    {
        var length = 0;
        foreach (var row in _rows)
        {
            length = Math.Max(length, row[column].Length);
        }

        return length;
        
        // todo return _rows.Select(row => row[column].Length).Max();
    }
}