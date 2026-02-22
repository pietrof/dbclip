using System.Data;
using System.Windows.Forms;

namespace DBClip.Services;

public interface IClipboardService
{
    void CopyToClipboard(DataGridView grid);
    void CutToClipboard(DataGridView grid);
    bool PasteFromClipboard(DataGridView grid, IDataObject? data);
}

public class ClipboardService : IClipboardService
{
    public void CopyToClipboard(DataGridView grid)
    {
        if (grid.SelectedCells.Count == 0) return;

        var data = GetSelectedData(grid);
        Clipboard.SetDataObject(data, true);
    }

    public void CutToClipboard(DataGridView grid)
    {
        if (grid.SelectedCells.Count == 0) return;

        var data = GetSelectedData(grid);
        Clipboard.SetDataObject(data, true);

        foreach (DataGridViewCell cell in grid.SelectedCells)
        {
            if (cell.IsInEditMode)
            {
                grid.EndEdit();
            }
            cell.Value = DBNull.Value;
        }
    }

    public bool PasteFromClipboard(DataGridView grid, IDataObject? data)
    {
        if (grid.DataSource is not DataTable dataTable) return false;

        data ??= Clipboard.GetDataObject();
        if (data == null || !data.GetDataPresent(typeof(string))) return false;

        var text = data.GetData(typeof(string)) as string;
        if (string.IsNullOrEmpty(text)) return false;

        var rows = text.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        if (rows.Length == 0) return false;

        var startRow = grid.SelectedCells.Cast<DataGridViewCell>()
            .OrderBy(c => c.RowIndex)
            .First().RowIndex;

        var startCol = grid.SelectedCells.Cast<DataGridViewCell>()
            .OrderBy(c => c.ColumnIndex)
            .First().ColumnIndex;

        grid.BeginEdit(false);

        try
        {
            if (rows.Length == 1 && !rows[0].Contains('\t'))
            {
                if (grid.CurrentCell != null)
                {
                    grid.CurrentCell.Value = rows[0];
                }
            }
            else
            {
                for (int i = 0; i < rows.Length; i++)
                {
                    var cells = rows[i].Split('\t');
                    var rowIndex = startRow + i;

                    if (rowIndex >= dataTable.Rows.Count)
                    {
                        dataTable.Rows.Add(dataTable.NewRow());
                    }

                    for (int j = 0; j < cells.Length; j++)
                    {
                        var colIndex = startCol + j;
                        if (colIndex < dataTable.Columns.Count)
                        {
                            dataTable.Rows[rowIndex][colIndex] = cells[j];
                        }
                    }
                }
            }

            grid.EndEdit();
            return true;
        }
        catch
        {
            grid.CancelEdit();
            return false;
        }
    }

    private DataObject GetSelectedData(DataGridView grid)
    {
        var selectedCells = grid.SelectedCells.Cast<DataGridViewCell>()
            .OrderBy(c => c.RowIndex)
            .ThenBy(c => c.ColumnIndex)
            .ToList();

        if (selectedCells.Count == 0) return new DataObject();

        var minRow = selectedCells.Min(c => c.RowIndex);
        var maxRow = selectedCells.Max(c => c.RowIndex);
        var minCol = selectedCells.Min(c => c.ColumnIndex);
        var maxCol = selectedCells.Max(c => c.ColumnIndex);

        var text = new System.Text.StringBuilder();

        for (int row = minRow; row <= maxRow; row++)
        {
            for (int col = minCol; col <= maxCol; col++)
            {
                var cell = grid.Rows[row].Cells[col];
                var value = cell.Value?.ToString() ?? "";
                text.Append(value);
                if (col < maxCol) text.Append('\t');
            }
            if (row < maxRow) text.AppendLine();
        }

        return new DataObject(text.ToString());
    }
}
