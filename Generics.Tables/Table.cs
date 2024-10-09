namespace Generics.Tables;

public class Table<TRow, TColumn, TItem>
{
    public readonly OpenTable<TRow, TColumn, TItem> Open;
    public readonly ExistedTable<TRow, TColumn, TItem> Existed;
    public readonly HashSet<TRow> Rows = new();
    public readonly HashSet<TColumn> Columns = new();
    private readonly Dictionary<(TRow Row, TColumn Column), TItem?> table = new();

    public Table()
    {
        Open = new OpenTable<TRow, TColumn, TItem>(table, Rows, Columns);
        Existed = new ExistedTable<TRow, TColumn, TItem>(table, Rows, Columns);
    }

    public void AddRow(TRow row) => Rows.Add(row);
    public void AddColumn(TColumn column) => Columns.Add(column);
}

public class OpenTable<TRow, TColumn, TItem>
{
    private HashSet<TRow> rows;
    private HashSet<TColumn> columns;
    private readonly Dictionary<(TRow Row, TColumn Column), TItem?> table;

    public OpenTable(
        Dictionary<(TRow Row, TColumn Column), TItem?> table,
        HashSet<TRow> rows, HashSet<TColumn> columns)
    {
        this.table = table;
        this.rows = rows;
        this.columns = columns;
    }

    public TItem? this[TRow row, TColumn column]
    {
        get
        {
            if (!rows.Contains(row) || !columns.Contains(column)) return default;
            if (!table.ContainsKey((row, column)))
                table.Add((row, column), default);
            return table[(row, column)];
        }
        set
        {
            if (!rows.Contains(row) || !columns.Contains(column))
            {
                rows.Add(row);
                columns.Add(column);
            }

            if (table.ContainsKey((row, column)))
                table[(row, column)] = value;
            else
                table.Add((row, column), value);
        }
    }
}

public class ExistedTable<TRow, TColumn, TItem>
{
    private HashSet<TRow> rows;
    private HashSet<TColumn> columns;
    private readonly Dictionary<(TRow Row, TColumn Column), TItem?> table;

    public ExistedTable(
        Dictionary<(TRow Row, TColumn Column), TItem?> table,
        HashSet<TRow> rows, HashSet<TColumn> columns)
    {
        this.table = table;
        this.rows = rows;
        this.columns = columns;
    }

    public TItem? this[TRow row, TColumn column]
    {
        get
        {
            if (!rows.Contains(row) || !columns.Contains(column)) throw new ArgumentException();
            if (!table.ContainsKey((row, column)))
                table.Add((row, column), default);
            return table[(row, column)];
        }
        set
        {
            if (rows.Contains(row) && columns.Contains(column))
                table[(row, column)] = value;
            else
                throw new ArgumentException();
        }
    }
}