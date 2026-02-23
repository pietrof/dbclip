using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DBClip.Models;
using DBClip.Services;
using DBClip.ViewModels;

namespace DBClip.Views;

public partial class MainForm : Form
{
    private readonly MainViewModel _viewModel;
    private TreeNode? _dragSourceNode;
    private bool _isPopulatingTree;
    private bool _isHighlighting;

    public MainForm()
    {
        InitializeComponent();
        
        var settingsService = new SettingsService();
        _ = settingsService.LoadAsync();
        
        ApplyWindowPosition(settingsService.Settings);

        var scriptRepo = new JsonScriptRepository();
        var databaseService = new DatabaseService();
        var templateParser = new TemplateParser();
        var clipboardService = new ClipboardService();
        var undoRedoService = new UndoRedoService();

        _viewModel = new MainViewModel(
            scriptRepo,
            settingsService,
            databaseService,
            templateParser,
            clipboardService,
            undoRedoService);

        _viewModel.SelectedNodeChanged += ViewModel_SelectedNodeChanged;
        _viewModel.ScriptChanged += ViewModel_ScriptChanged;
        _viewModel.ResultsChanged += ViewModel_ResultsChanged;
        _viewModel.StatusChanged += ViewModel_StatusChanged;
        _viewModel.UndoRedoStateChanged += ViewModel_UndoRedoStateChanged;

        SetupImageList();
        SetupKeyboardShortcuts();
        SetupSplitters();
        SetupSearchControls();
        SetupContextMenus();

        Load += MainForm_Load;
        Shown += MainForm_Shown;
        FormClosing += MainForm_FormClosing;
        Resize += MainForm_Resize;
    }

    private void MainForm_Resize(object? sender, EventArgs e)
    {
        ResetWindowLayout();
    }

    private async void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (_viewModel?.SettingsService != null)
        {
            var settings = _viewModel.SettingsService.Settings;
            settings.WindowLeft = Location.X;
            settings.WindowTop = Location.Y;
            settings.WindowWidth = Size.Width;
            settings.WindowHeight = Size.Height;
            settings.IsMaximized = WindowState == FormWindowState.Maximized;
            await _viewModel.SettingsService.SaveAsync();
        }
    }

    private void ApplyWindowPosition(AppSettings settings)
    {
        try
        {
            if (!double.IsNaN(settings.WindowLeft) && !double.IsNaN(settings.WindowTop))
            {
                var screen = Screen.FromPoint(new Point((int)settings.WindowLeft, (int)settings.WindowTop));
                var bounds = screen.WorkingArea;
                
                if (settings.WindowLeft >= bounds.Left && settings.WindowLeft < bounds.Right - 100 &&
                    settings.WindowTop >= bounds.Top && settings.WindowTop < bounds.Bottom - 100)
                {
                    StartPosition = FormStartPosition.Manual;
                    Location = new Point((int)settings.WindowLeft, (int)settings.WindowTop);
                }
            }

            if (settings.WindowWidth > 0 && settings.WindowHeight > 0)
            {
                Size = new Size((int)settings.WindowWidth, (int)settings.WindowHeight);
            }

            if (settings.IsMaximized)
            {
                WindowState = FormWindowState.Maximized;
            }
        }
        catch { }

        try
        {
            ResetWindowLayout();
        }
        catch { }
    }

    private void MainForm_Shown(object? sender, EventArgs e)
    {
    }

    private void SetupSplitters()
    {
    }

    private void SetupSearchControls()
    {
        txtSearch.TextChanged += TxtSearch_TextChanged;
        btnClearSearch.Click += BtnClearSearch_Click;
    }

    private void SetupContextMenus()
    {
        var editorContextMenu = new ContextMenuStrip();
        var copyWithValuesItem = new ToolStripMenuItem("Copy with Values");
        copyWithValuesItem.Click += EditorCopyWithValues_Click;
        editorContextMenu.Items.Add(copyWithValuesItem);
        txtScriptEditor.ContextMenuStrip = editorContextMenu;

        var treeViewContextMenu = new ContextMenuStrip();
        
        var addScriptItem = new ToolStripMenuItem("Add New Script");
        addScriptItem.Click += (s, e) => btnAddScript_Click(s, e);
        treeViewContextMenu.Items.Add(addScriptItem);
        
        var addFolderItem = new ToolStripMenuItem("Add New Folder");
        addFolderItem.Click += (s, e) => btnAddFolder_Click(s, e);
        treeViewContextMenu.Items.Add(addFolderItem);
        
        treeViewContextMenu.Items.Add(new ToolStripSeparator());
        
        var duplicateItem = new ToolStripMenuItem("Duplicate");
        duplicateItem.Click += TreeViewDuplicate_Click;
        treeViewContextMenu.Items.Add(duplicateItem);
        
        var duplicateWithValuesItem = new ToolStripMenuItem("Duplicate with Values");
        duplicateWithValuesItem.Click += TreeViewDuplicateWithValues_Click;
        treeViewContextMenu.Items.Add(duplicateWithValuesItem);
        
        treeViewContextMenu.Items.Add(new ToolStripSeparator());
        
        var deleteItem = new ToolStripMenuItem("Delete");
        deleteItem.Click += (s, e) => DeleteSelectedNode();
        treeViewContextMenu.Items.Add(deleteItem);
        
        treeViewScripts.ContextMenuStrip = treeViewContextMenu;
    }

    private void EditorCopyWithValues_Click(object? sender, EventArgs e)
    {
        var textWithValues = _viewModel.GetScriptWithValuesApplied();
        if (!string.IsNullOrEmpty(textWithValues))
        {
            Clipboard.SetText(textWithValues);
        }
    }

    private void TreeViewDuplicate_Click(object? sender, EventArgs e)
    {
        if (treeViewScripts.SelectedNode?.Tag is ScriptNode selectedNode)
        {
            var newNode = _viewModel.DuplicateNode(selectedNode);
            if (newNode != null)
            {
                PopulateTreeView();
                _viewModel.SelectNode(newNode);
            }
        }
    }

    private void TreeViewDuplicateWithValues_Click(object? sender, EventArgs e)
    {
        if (treeViewScripts.SelectedNode?.Tag is ScriptNode selectedNode)
        {
            var newNode = _viewModel.DuplicateNodeWithValues(selectedNode);
            if (newNode != null)
            {
                PopulateTreeView();
                _viewModel.SelectNode(newNode);
            }
        }
    }

    private async void TxtSearch_TextChanged(object? sender, EventArgs e)
    {
        var searchText = txtSearch.Text;
        
        if (string.IsNullOrEmpty(searchText) || searchText.Length <= 2)
        {
            await Task.Run(() => this.Invoke(() => PopulateTreeView()));
            return;
        }

        await FilterTreeViewAsync(searchText);
    }

    private async Task FilterTreeViewAsync(string searchText)
    {
        var searchLower = searchText.ToLowerInvariant();
        
        var filteredNodes = await Task.Run(() =>
        {
            return FilterNodes(_viewModel.RootNodes.ToList(), searchLower);
        });

        await Task.Run(() => this.Invoke(() =>
        {
            _isPopulatingTree = true;
            treeViewScripts.Nodes.Clear();

            foreach (var node in filteredNodes)
            {
                var treeNode = CreateFilteredTreeNode(node);
                treeViewScripts.Nodes.Add(treeNode);
            }

            treeViewScripts.ExpandAll();
            _isPopulatingTree = false;
        }));
    }

    private List<ScriptNode> FilterNodes(List<ScriptNode> nodes, string searchLower)
    {
        var result = new List<ScriptNode>();
        
        foreach (var node in nodes)
        {
            var filteredChildren = node.Children.Count > 0 
                ? FilterNodes(node.Children.ToList(), searchLower) 
                : new List<ScriptNode>();
            
            var nameMatches = node.Name.ToLowerInvariant().Contains(searchLower);
            var scriptMatches = node.NodeType == NodeType.Script && 
                                node.ScriptContent.ToLowerInvariant().Contains(searchLower);
            
            if (nameMatches || scriptMatches || filteredChildren.Count > 0)
            {
                var newNode = new ScriptNode
                {
                    Id = node.Id,
                    Name = node.Name,
                    ScriptContent = node.ScriptContent,
                    NodeType = node.NodeType,
                    ParentId = node.ParentId,
                    CreatedDate = node.CreatedDate,
                    ModifiedDate = node.ModifiedDate,
                    SortOrder = node.SortOrder,
                    Children = new System.Collections.ObjectModel.ObservableCollection<ScriptNode>(filteredChildren)
                };
                result.Add(newNode);
            }
        }
        
        return result;
    }

    private TreeNode CreateFilteredTreeNode(ScriptNode node)
    {
        var imageIndex = node.NodeType == NodeType.Folder ? 0 : 1;
        var treeNode = new TreeNode(node.Name, imageIndex, imageIndex);
        treeNode.Tag = node;

        foreach (var child in node.Children)
        {
            treeNode.Nodes.Add(CreateFilteredTreeNode(child));
        }

        return treeNode;
    }

    private void BtnClearSearch_Click(object? sender, EventArgs e)
    {
        txtSearch.Text = "";
    }

    private void SetupImageList()
    {
        var imageList = new ImageList();
        imageList.Images.Add("Folder", CreateColoredIcon(Color.FromArgb(255, 194, 134)));
        imageList.Images.Add("Script", CreateColoredIcon(Color.FromArgb(86, 156, 214)));
        treeViewScripts.ImageList = imageList;
    }

    private Bitmap CreateColoredIcon(Color color)
    {
        var bmp = new Bitmap(16, 16);
        using var g = Graphics.FromImage(bmp);
        g.Clear(Color.Transparent);
        using var brush = new SolidBrush(color);
        g.FillRectangle(brush, 2, 2, 12, 12);
        using var pen = new Pen(Color.FromArgb(50, 0, 0, 0), 1);
        g.DrawRectangle(pen, 2, 2, 12, 12);
        return bmp;
    }

    private void SetupKeyboardShortcuts()
    {
        KeyPreview = true;
        KeyDown += (s, e) =>
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                _viewModel.SaveCurrentScript();
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.Z)
            {
                _viewModel.Undo();
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                _viewModel.Redo();
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.F5)
            {
                btnPlay_Click(s, e);
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.F5)
            {
                btnPlay_Click(s, e);
                e.SuppressKeyPress = true;
            }
        };
    }

    private void chkCacheResults_Click(object? sender, EventArgs e)
    {
        chkCacheResults.Checked = !chkCacheResults.Checked;
        
        if (chkCacheResults.Checked)
        {
            chkCacheResults.Image = CreateCacheOnIcon();
            chkCacheResults.Text = "Cache On";
        }
        else
        {
            chkCacheResults.Image = CreateCacheOffIcon();
            chkCacheResults.Text = "Cache Off";
        }
    }

    private void btnStop_Click(object? sender, EventArgs e)
    {
        _viewModel.CancelExecution();
        _viewModel.IsExecuting = false;
        _viewModel.StatusMessage = "Query cancelled";
        UpdateUI();
    }

    private void numRowCount_Leave(object? sender, EventArgs e)
    {
        if (int.TryParse(numRowCount.Text, out var rowCount) && rowCount >= 0)
        {
            _viewModel.RowCount = rowCount;
        }
        else
        {
            numRowCount.Text = _viewModel.RowCount.ToString();
        }
    }

    private async void MainForm_Load(object sender, EventArgs e)
    {
        ValidateAndFixSplitters();
        
        await _viewModel.InitializeAsync();
        PopulateTreeView();
        PopulateDatabaseDropdown();
        UpdateUI();
    }

    private void ValidateAndFixSplitters()
    {
        try
        {
            var totalWidth = splitContainerMain.Width;
            if (totalWidth > 0)
            {
                var minSplitter1 = splitContainerMain.Panel1MinSize;
                var maxSplitter1 = totalWidth - splitContainerMain.Panel2MinSize - splitContainerMain.SplitterWidth;
                
                if (splitContainerMain.SplitterDistance < minSplitter1 || splitContainerMain.SplitterDistance > maxSplitter1)
                {
                    splitContainerMain.SplitterDistance = (int)(totalWidth * 0.20);
                }
            }

            var rightPanelWidth = splitContainerMiddle.Width;
            if (rightPanelWidth > 0)
            {
                var minSplitter2 = splitContainerMiddle.Panel1MinSize;
                var maxSplitter2 = rightPanelWidth - splitContainerMiddle.Panel2MinSize - splitContainerMiddle.SplitterWidth;
                
                if (splitContainerMiddle.SplitterDistance < minSplitter2 || splitContainerMiddle.SplitterDistance > maxSplitter2)
                {
                    splitContainerMiddle.SplitterDistance = (int)(rightPanelWidth * 0.75);
                }
            }
        }
        catch
        {
            try
            {
                ResetWindowLayout();
            }
            catch { }
        }
    }

    private void PopulateTreeView()
    {
        _isPopulatingTree = true;
        treeViewScripts.Nodes.Clear();

        foreach (var node in _viewModel.RootNodes)
        {
            var treeNode = CreateTreeNode(node);
            treeViewScripts.Nodes.Add(treeNode);
        }

        treeViewScripts.ExpandAll();
        _isPopulatingTree = false;
    }

    private TreeNode CreateTreeNode(ScriptNode node)
    {
        var imageIndex = node.NodeType == NodeType.Folder ? 0 : 1;
        var treeNode = new TreeNode(node.Name, imageIndex, imageIndex);
        treeNode.Tag = node;

        foreach (var child in node.Children)
        {
            treeNode.Nodes.Add(CreateTreeNode(child));
        }

        return treeNode;
    }

    private void ViewModel_SelectedNodeChanged(object? sender, EventArgs e)
    {
        txtScriptEditor.Text = _viewModel.CurrentScript;
        
        if (chkCacheResults.Checked && _viewModel.SelectedNode != null)
        {
            var (results, _) = _viewModel.GetCachedResults(_viewModel.SelectedNode.Id);
            if (results != null)
            {
                _viewModel.QueryResults = results;
                _viewModel.LastError = null;
                _viewModel.CachedError = null;
                ViewModel_ResultsChanged(this, EventArgs.Empty);
                return;
            }
            
            if (_viewModel.CachedError != null)
            {
                _viewModel.LastError = _viewModel.CachedError;
                _viewModel.QueryResults = null;
                ViewModel_ResultsChanged(this, EventArgs.Empty);
                return;
            }
        }
        
        UpdateUI();
    }

    private void ViewModel_ScriptChanged(object? sender, EventArgs e)
    {
        UpdateUI();
    }

    private void ViewModel_ResultsChanged(object? sender, EventArgs e)
    {
        tabControlResults.TabPages.Clear();
        
        if (_viewModel.LastError != null)
        {
            var errorTable = new DataTable("Error");
            errorTable.Columns.Add("Error", typeof(string));
            var row = errorTable.NewRow();
            row["Error"] = _viewModel.LastError;
            errorTable.Rows.Add(row);
            
                var tabPage = new TabPage("Error");
            var dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;
            dgv.DataSource = errorTable;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.CellClick += dgvResults_CellClick;
            dgv.KeyDown += dgvResults_KeyDown;
            tabPage.Controls.Add(dgv);
            tabControlResults.TabPages.Add(tabPage);
        }
        else if (_viewModel.QueryResults != null)
        {
            for (int i = 0; i < _viewModel.QueryResults.Count; i++)
            {
                var dataTable = _viewModel.QueryResults[i];
                var tabName = dataTable.TableName != null && !string.IsNullOrEmpty(dataTable.TableName) 
                    ? dataTable.TableName 
                    : $"Result {i + 1}";
                
                var tabPage = new TabPage(tabName);
                var dgv = new DataGridView();
                dgv.Dock = DockStyle.Fill;
                dgv.DataSource = dataTable;
                dgv.ReadOnly = true;
                dgv.AllowUserToAddRows = false;
                dgv.AllowUserToDeleteRows = false;
                dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
                dgv.RowHeadersVisible = false;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgv.CellClick += dgvResults_CellClick;
                dgv.KeyDown += dgvResults_KeyDown;
                tabPage.Controls.Add(dgv);
                tabControlResults.TabPages.Add(tabPage);
            }
        }
    }

    private void ViewModel_StatusChanged(object? sender, EventArgs e)
    {
        lblStatus.Text = _viewModel.StatusMessage;
    }

    private void ViewModel_UndoRedoStateChanged(object? sender, EventArgs e)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        btnUndo.Enabled = _viewModel.CanUndo;
        btnRedo.Enabled = _viewModel.CanRedo;
        btnDelete.Enabled = _viewModel.SelectedNode != null;
        btnPlay.Enabled = _viewModel.SelectedNode != null && !_viewModel.IsExecuting;
        btnStop.Enabled = _viewModel.IsExecuting;
    }

    private void treeViewScripts_AfterSelect(object sender, TreeViewEventArgs e)
    {
        if (_isPopulatingTree) return;

        var treeNode = e.Node;
        if (treeNode?.Tag is ScriptNode node)
        {
            _viewModel.SelectNode(node);
        }
    }

    private void treeViewScripts_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            var hitTest = treeViewScripts.HitTest(e.Location);
            if (hitTest.Node != null)
            {
                treeViewScripts.SelectedNode = hitTest.Node;
            }
        }
    }

    private void treeViewScripts_AfterLabelEdit(object? sender, NodeLabelEditEventArgs e)
    {
        if (e.Label != null && e.Node?.Tag is ScriptNode node)
        {
            _viewModel.RenameNode(node, e.Label);
        }
    }

    private void treeViewScripts_ItemDrag(object sender, ItemDragEventArgs e)
    {
        if (e.Button == MouseButtons.Left && e.Item is TreeNode treeNode)
        {
            _dragSourceNode = treeNode;
            DoDragDrop(e.Item, DragDropEffects.Move);
        }
    }

    private void treeViewScripts_DragEnter(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(typeof(string)))
        {
            e.Effect = DragDropEffects.Copy;
        }
        else if (e.Data.GetDataPresent(typeof(TreeNode)))
        {
            e.Effect = DragDropEffects.Move;
        }
        else
        {
            e.Effect = DragDropEffects.None;
        }
    }

    private void treeViewScripts_DragOver(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(typeof(string)))
        {
            e.Effect = DragDropEffects.Copy;
        }
        else if (e.Data.GetDataPresent(typeof(TreeNode)))
        {
            e.Effect = DragDropEffects.Move;
        }
        else
        {
            e.Effect = DragDropEffects.None;
        }
    }

    private void treeViewScripts_DragDrop(object sender, DragEventArgs e)
    {
        // Handle external text drops (e.g., from other applications)
        if (e.Data.GetDataPresent(typeof(string)))
        {
            var text = e.Data.GetData(typeof(string)) as string;
            if (!string.IsNullOrWhiteSpace(text))
            {
                var targetPoint = treeViewScripts.PointToClient(new Point(e.X, e.Y));
                var targetNode = treeViewScripts.GetNodeAt(targetPoint);

                ScriptNode? parentNode = null;
                if (targetNode != null)
                {
                    var scriptNode = targetNode.Tag as ScriptNode;
                    if (scriptNode?.NodeType == NodeType.Folder)
                    {
                        parentNode = scriptNode;
                    }
                    else if (targetNode.Parent?.Tag is ScriptNode parent)
                    {
                        parentNode = parent;
                    }
                }

                var newNode = _viewModel.AddNode("Imported Script", NodeType.Script, parentNode);
                newNode.ScriptContent = text;
                _viewModel.SaveCurrentScript();
                _ = _viewModel.SaveScriptsAsync();
                PopulateTreeView();
                
                var treeNode = FindTreeNodeById(newNode.Id);
                if (treeNode != null)
                {
                    treeViewScripts.SelectedNode = treeNode;
                    treeNode.BeginEdit();
                }
            }
            _dragSourceNode = null;
            return;
        }

        // Handle internal tree node moves
        if (_dragSourceNode == null) return;

        var dropPoint = treeViewScripts.PointToClient(new Point(e.X, e.Y));
        var dropTargetNode = treeViewScripts.GetNodeAt(dropPoint);

        if (dropTargetNode != null && _dragSourceNode != dropTargetNode && !IsDescendant(dropTargetNode, _dragSourceNode))
        {
            var sourceScriptNode = _dragSourceNode.Tag as ScriptNode;
            var targetScriptNode = dropTargetNode.Tag as ScriptNode;

            if (sourceScriptNode != null && targetScriptNode != null)
            {
                ScriptNode? newParent = null;
                if (targetScriptNode.NodeType == NodeType.Folder)
                {
                    newParent = targetScriptNode;
                }
                else if (dropTargetNode.Parent != null)
                {
                    newParent = dropTargetNode.Parent.Tag as ScriptNode;
                }

                var newIndex = dropTargetNode.Index + (dropPoint.Y < dropTargetNode.Bounds.Y - dropTargetNode.Bounds.Height / 2 ? 0 : 1);
                _viewModel.MoveNode(sourceScriptNode, newParent, newIndex);

                PopulateTreeView();
            }
        }
        else if (dropTargetNode == null && _dragSourceNode != null)
        {
            // Dropped on empty area - move to root
            var sourceScriptNode = _dragSourceNode.Tag as ScriptNode;
            if (sourceScriptNode != null)
            {
                _viewModel.MoveNode(sourceScriptNode, null, _viewModel.RootNodes.Count);
                PopulateTreeView();
            }
        }

        _dragSourceNode = null;
    }

    private bool IsDescendant(TreeNode potentialDescendant, TreeNode ancestor)
    {
        while (potentialDescendant != null)
        {
            if (potentialDescendant == ancestor) return true;
            potentialDescendant = potentialDescendant.Parent;
        }
        return false;
    }

    private void txtScriptEditor_TextChanged(object sender, EventArgs e)
    {
        if (_viewModel.SelectedNode != null)
        {
            _viewModel.OnScriptTextChanged(txtScriptEditor.Text);
        }
        
        ApplySqlSyntaxHighlighting();
    }

    // private bool _isHighlighting;
    private void ApplySqlSyntaxHighlighting()
    {
        if (_isHighlighting) return;
        _isHighlighting = true;

        try
        {
            var text = txtScriptEditor.Text;
            var selectionStart = txtScriptEditor.SelectionStart;
            var selectionLength = txtScriptEditor.SelectionLength;

            txtScriptEditor.SuspendLayout();
            
            var originalColor = txtScriptEditor.ForeColor;
            txtScriptEditor.SelectAll();
            txtScriptEditor.SelectionColor = originalColor;

            var keywords = new[] { "SELECT", "FROM", "WHERE", "AND", "OR", "NOT", "IN", "LIKE", "BETWEEN", "JOIN", "LEFT", "RIGHT", "INNER", "OUTER", "FULL", "CROSS", "ON", "AS", "ORDER", "BY", "GROUP", "HAVING", "LIMIT", "OFFSET", "INSERT", "INTO", "VALUES", "UPDATE", "SET", "DELETE", "CREATE", "TABLE", "DROP", "ALTER", "INDEX", "VIEW", "TRIGGER", "PROCEDURE", "FUNCTION", "BEGIN", "END", "IF", "ELSE", "CASE", "WHEN", "THEN", "NULL", "IS", "EXISTS", "DISTINCT", "TOP", "UNION", "ALL", "ASC", "DESC", "PRIMARY", "KEY", "FOREIGN", "REFERENCES", "CONSTRAINT", "DEFAULT", "CHECK", "UNIQUE", "CASCADE", "WITH", "RECURSIVE", "OVER", "PARTITION", "ROW_NUMBER", "RANK", "DENSE_RANK", "LAG", "LEAD", "SUM", "COUNT", "AVG", "MIN", "MAX", "CAST", "CONVERT", "COALESCE", "NULLIF", "GETDATE", "DATEADD", "DATEDIFF", "YEAR", "MONTH", "DAY", "DECLARE", "EXEC", "EXECUTE", "PRINT", "RAISERROR", "TRY", "CATCH", "THROW", "OPEN", "CLOSE", "DEALLOCATE", "FETCH", "INTO", "OUT", "OUTPUT", "MERGE", "USING", "WHEN", "MATCHED", "SOURCE", "TARGET" };
            var functions = new[] { "COUNT", "SUM", "AVG", "MIN", "MAX", "LEN", "SUBSTRING", "TRIM", "LTRIM", "RTRIM", "UPPER", "LOWER", "REPLACE", "CHARINDEX", "PATINDEX", "CONCAT", "FORMAT", "ISNULL", "IIF", "CHOOSE", "ROW_NUMBER", "RANK", "DENSE_RANK", "NTILE", "LAG", "LEAD", "FIRST_VALUE", "LAST_VALUE", "NTH_VALUE", "OVER", "PARTITION", "ROW_NUMBER", "ABS", "CEILING", "FLOOR", "ROUND", "SQRT", "POWER", "LOG", "EXP", "SIN", "COS", "TAN", "ASIN", "ACOS", "ATAN" };
            var dataTypes = new[] { "INT", "INTEGER", "BIGINT", "SMALLINT", "TINYINT", "BIT", "DECIMAL", "NUMERIC", "MONEY", "SMALLMONEY", "FLOAT", "REAL", "DATETIME", "DATETIME2", "DATE", "TIME", "DATETIMEOFFSET", "TIMESTAMP", "CHAR", "VARCHAR", "NCHAR", "NVARCHAR", "TEXT", "NTEXT", "BINARY", "VARBINARY", "IMAGE", "XML", "JSON", "UNIQUEIDENTIFIER", "GUID", "BOOLEAN", "BOOL", "UUID", "SERIAL", "MONEY", "BYTEA", "INTERVAL" };
            var keywordColor = Color.FromArgb(86, 156, 214);
            var functionColor = Color.FromArgb(220, 220, 170);
            var stringColor = Color.FromArgb(206, 145, 120);
            var commentColor = Color.FromArgb(106, 153, 85);
            var numberColor = Color.FromArgb(181, 206, 168);
            var dataTypeColor = Color.FromArgb(78, 201, 176);

            var stringPattern = @"'(?:[^']|'')*'";
            var commentPattern = @"--.*$|/\*[\s\S]*?\*/";
            var numberPattern = @"\b\d+\.?\d*\b";
            var wordPattern = @"\b\w+\b";

            foreach (System.Text.RegularExpressions.Match commentMatch in System.Text.RegularExpressions.Regex.Matches(text, commentPattern, System.Text.RegularExpressions.RegexOptions.Multiline))
            {
                txtScriptEditor.Select(commentMatch.Index, commentMatch.Length);
                txtScriptEditor.SelectionColor = commentColor;
            }

            foreach (System.Text.RegularExpressions.Match stringMatch in System.Text.RegularExpressions.Regex.Matches(text, stringPattern))
            {
                txtScriptEditor.Select(stringMatch.Index, stringMatch.Length);
                txtScriptEditor.SelectionColor = stringColor;
            }

            foreach (System.Text.RegularExpressions.Match numberMatch in System.Text.RegularExpressions.Regex.Matches(text, numberPattern))
            {
                txtScriptEditor.Select(numberMatch.Index, numberMatch.Length);
                txtScriptEditor.SelectionColor = numberColor;
            }

            foreach (System.Text.RegularExpressions.Match wordMatch in System.Text.RegularExpressions.Regex.Matches(text, wordPattern, System.Text.RegularExpressions.RegexOptions.Multiline))
            {
                var word = wordMatch.Value.ToUpper();
                if (keywords.Contains(word))
                {
                    txtScriptEditor.Select(wordMatch.Index, wordMatch.Length);
                    txtScriptEditor.SelectionColor = keywordColor;
                }
                else if (functions.Contains(word))
                {
                    txtScriptEditor.Select(wordMatch.Index, wordMatch.Length);
                    txtScriptEditor.SelectionColor = functionColor;
                }
                else if (dataTypes.Contains(word))
                {
                    txtScriptEditor.Select(wordMatch.Index, wordMatch.Length);
                    txtScriptEditor.SelectionColor = dataTypeColor;
                }
            }

            txtScriptEditor.Select(selectionStart, selectionLength);
            txtScriptEditor.SelectionStart = selectionStart;
            txtScriptEditor.ResumeLayout();
        }
        finally
        {
            _isHighlighting = false;
        }
    }

    // private void ApplySqlSyntaxHighlighting() { }

    private async void btnPlay_Click(object sender, EventArgs e)
    {
        var scriptToRun = txtScriptEditor.SelectionLength > 0 
            ? txtScriptEditor.SelectedText 
            : null;
        
        var templateVars = _viewModel.GetTemplateVariables(scriptToRun);

        if (templateVars.Count > 0)
        {
            using var dialog = new TemplatePromptDialog(templateVars, _viewModel.CurrentRowContext);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                await _viewModel.ExecuteScriptAsync(dialog.TemplateValues, scriptToRun);
            }
        }
        else
        {
            await _viewModel.ExecuteScriptAsync(null, scriptToRun);
        }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        _viewModel.SaveCurrentScript();
        _viewModel.SaveScriptsAsync();
    }

    private void btnDatabase_Click(object sender, EventArgs e)
    {
        using var dialog = new DatabaseSettingsDialog(_viewModel.Databases);
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            PopulateDatabaseDropdown();
            _viewModel.SaveScriptsAsync();
        }
    }

    private void PopulateDatabaseDropdown()
    {
        cmbDatabaseSelect.Items.Clear();
        foreach (var db in _viewModel.Databases)
        {
            cmbDatabaseSelect.Items.Add(db.Name);
        }
        
        var currentDb = _viewModel.CurrentDatabase;
        if (currentDb != null)
        {
            cmbDatabaseSelect.SelectedItem = currentDb.Name;
        }
        else if (cmbDatabaseSelect.Items.Count > 0)
        {
            cmbDatabaseSelect.SelectedIndex = 0;
        }
    }

    private void cmbDatabaseSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbDatabaseSelect.SelectedItem is string dbName)
        {
            _viewModel.SelectDatabase(dbName);
        }
    }

    private void btnAddScript_Click(object sender, EventArgs e)
    {
        AddNewScript();
    }

    private void btnAddFolder_Click(object sender, EventArgs e)
    {
        AddNewFolder();
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        DeleteSelectedNode();
    }

    private void btnUndo_Click(object sender, EventArgs e)
    {
        _viewModel.Undo();
        txtScriptEditor.Text = _viewModel.CurrentScript;
    }

    private void btnRedo_Click(object sender, EventArgs e)
    {
        _viewModel.Redo();
        txtScriptEditor.Text = _viewModel.CurrentScript;
    }

    private void btnResetLayout_Click(object sender, EventArgs e)
    {
        ResetWindowLayout();
    }

    private void ResetWindowLayout()
    {
        try
        {
            splitContainerMain.IsSplitterFixed = false;
            splitContainerMiddle.IsSplitterFixed = false;
            
            var totalWidth = splitContainerMain.Width;
            if (totalWidth > 0)
            {
                splitContainerMain.SplitterDistance = (int)(totalWidth * 0.20);
            }

            var middlePanelWidth = splitContainerMiddle.Width;
            if (middlePanelWidth > 0)
            {
                splitContainerMiddle.SplitterDistance = (int)(middlePanelWidth * 0.75);
            }

            tableLayoutPanelRight.RowStyles[0].SizeType = SizeType.Percent;
            tableLayoutPanelRight.RowStyles[0].Height = 50F;
            tableLayoutPanelRight.RowStyles[2].SizeType = SizeType.Percent;
            tableLayoutPanelRight.RowStyles[2].Height = 50F;
        }
        catch { }
    }

    private void addScriptToolStripMenuItem_Click(object sender, EventArgs e)
    {
        AddNewScript();
    }

    private void addFolderToolStripMenuItem_Click(object sender, EventArgs e)
    {
        AddNewFolder();
    }

    private void renameToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (treeViewScripts.SelectedNode != null)
        {
            treeViewScripts.SelectedNode.BeginEdit();
        }
    }

    private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        DeleteSelectedNode();
    }

    private void duplicateToolStripMenuItem_Click(object sender, EventArgs e)
    {
        DuplicateSelectedNode();
    }

    private void DuplicateSelectedNode()
    {
        if (treeViewScripts.SelectedNode?.Tag is ScriptNode selectedNode)
        {
            var newNode = _viewModel.DuplicateNode(selectedNode);
            PopulateTreeView();
            
            var treeNode = FindTreeNodeById(newNode.Id);
            if (treeNode != null)
            {
                treeViewScripts.SelectedNode = treeNode;
                treeNode.BeginEdit();
            }
        }
    }

    private void AddNewScript()
    {
        ScriptNode? parent = null;
        if (treeViewScripts.SelectedNode?.Tag is ScriptNode selectedNode)
        {
            if (selectedNode.NodeType == NodeType.Folder)
            {
                parent = selectedNode;
            }
            else
            {
                parent = selectedNode.Parent;
            }
        }

        var node = _viewModel.AddNode("New Script", NodeType.Script, parent);
        PopulateTreeView();

        var treeNode = FindTreeNodeById(node.Id);
        if (treeNode != null)
        {
            treeViewScripts.SelectedNode = treeNode;
            treeNode.BeginEdit();
        }
    }

    private void AddNewFolder()
    {
        ScriptNode? parent = null;
        if (treeViewScripts.SelectedNode?.Tag is ScriptNode selectedNode)
        {
            if (selectedNode.NodeType == NodeType.Folder)
            {
                parent = selectedNode;
            }
            else
            {
                parent = selectedNode.Parent;
            }
        }

        var node = _viewModel.AddNode("New Folder", NodeType.Folder, parent);
        PopulateTreeView();

        var treeNode = FindTreeNodeById(node.Id);
        if (treeNode != null)
        {
            treeViewScripts.SelectedNode = treeNode;
            treeNode.BeginEdit();
        }
    }

    private void DeleteSelectedNode()
    {
        if (treeViewScripts.SelectedNode?.Tag is ScriptNode node)
        {
            if (MessageBox.Show($"Delete '{node.Name}'?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _viewModel.DeleteNode(node);
                PopulateTreeView();
            }
        }
    }

    private TreeNode? FindTreeNodeById(string id)
    {
        return FindTreeNodeById(treeViewScripts.Nodes, id);
    }

    private TreeNode? FindTreeNodeById(TreeNodeCollection nodes, string id)
    {
        foreach (TreeNode node in nodes)
        {
            if (node.Tag is ScriptNode scriptNode && scriptNode.Id == id)
                return node;

            var found = FindTreeNodeById(node.Nodes, id);
            if (found != null) return found;
        }
        return null;
    }

    private void dgvResults_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        if (sender is DataGridView dgv && e.RowIndex >= 0 && dgv.Rows[e.RowIndex] is DataGridViewRow row)
        {
            _viewModel.UpdateRowContext(row);
            UpdateContextGrid();
        }
    }

    private void UpdateContextGrid()
    {
        dgvContext.Rows.Clear();
        foreach (var kvp in _viewModel.CurrentRowContext.Values)
        {
            dgvContext.Rows.Add(kvp.Key, kvp.Value?.ToString() ?? "");
        }
    }

    private void dgvResults_KeyDown(object sender, KeyEventArgs e)
    {
        if (sender is DataGridView dgv)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                _viewModel.CopyGrid(dgv);
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.X)
            {
                _viewModel.CutGrid(dgv);
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                var data = Clipboard.GetDataObject();
                if (_viewModel.PasteGrid(dgv, data))
                {
                    e.SuppressKeyPress = true;
                }
            }
        }
    }
}
