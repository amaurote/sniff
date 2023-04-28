namespace Sniff.Table;

public static class TablePrinter
{
    public static void Print(Table table)
    {
        var columns = table.GetColumns().ToList();
        var rows = table.GetRows().ToList();
        
        if(rows.Any(row => row.Length != columns.Count))
            throw new InvalidDataException("Data inconsistency!");

        List<Tuple<Column, int>> columnTuples = new();
        for (var i = 0; i < columns.Count; i++)
        {
            var column = columns[i];
            if (column.ColumnWidth == ColumnWidth.Fixed)
            {
                columnTuples.Add(new Tuple<Column, int>(column, column.MaxWidth));
            }
            else
            {
                var longest = rows.Select(row => row[i].Length).Max();
                var width = Math.Clamp(longest, column.MinWidth, column.MaxWidth);
                columnTuples.Add(new Tuple<Column, int>(column, width));
            }
        }

        rows.ForEach(row =>
        {
            for (var i = 0; i < row.Length; i++)
            {
                var column = columnTuples[i].Item1;
                var width = columnTuples[i].Item2;
                var value = row[i];

                if (value.Length > width)
                    value = value.Substring(0, width - 1) + "~";

                value = column.ColumnPadding switch
                {
                    ColumnPadding.FromLeft => value.PadLeft(width),
                    ColumnPadding.FromRight => value.PadRight(width),
                    _ => value
                };

                Console.Write(value);
                if (i + 1 != row.Length)
                    Console.Write(table.Spacer);
            }

            Console.WriteLine();
        });
    }
}