using System.Collections.ObjectModel;
using System.Data;
using System.Timers;
using DBClip.Models;
using DBClip.Services;

namespace DBClip.ViewModels;

public class MainViewModel
{
    private readonly IScriptRepository _scriptRepository;
    private readonly ISettingsService _settingsService;
    private readonly IDatabaseService _databaseService;
    private readonly ITemplateParser _templateParser;
    private readonly IClipboardService _clipboardService;
    private readonly IUndoRedoService _undoRedoService;
    private readonly System.Timers.Timer _autoSaveTimer;
    private CancellationTokenSource? _cancellationTokenSource;
    private bool _hasUnsavedChanges;
    private string _currentScriptBeforeChange = "";
    private readonly Dictionary<string, List<DataTable>> _resultsCache = new();

    public ScriptTree ScriptTree { get; } = new();
    public ObservableCollection<ScriptNode> RootNodes => ScriptTree.RootNodes;
    public List<DatabaseSettings> Databases => _settingsService.Settings.Databases;
    public DatabaseSettings? CurrentDatabase { get; private set; }
    public RowContext CurrentRowContext { get; } = new();

    public ScriptNode? SelectedNode { get; private set; }
    public string CurrentScript { get; private set; } = "";
    public List<DataTable>? QueryResults { get; set; }
    public string? LastError { get; set; }
    public string? CachedError { get; set; }
    public int RowCount { get; set; } = 1000;
    public string StatusMessage { get; set; } = "Ready";
    public TimeSpan LastExecutionTime { get; private set; }
    public bool IsExecuting { get; set; }

    public ISettingsService SettingsService => _settingsService;

    public bool CanUndo => _undoRedoService.CanUndo;
    public bool CanRedo => _undoRedoService.CanRedo;

    public event EventHandler? SelectedNodeChanged;
    public event EventHandler? ScriptChanged;
    public event EventHandler? ResultsChanged;
    public event EventHandler? StatusChanged;
    public event EventHandler? UndoRedoStateChanged;

    public MainViewModel(
        IScriptRepository scriptRepository,
        ISettingsService settingsService,
        IDatabaseService databaseService,
        ITemplateParser templateParser,
        IClipboardService clipboardService,
        IUndoRedoService undoRedoService)
    {
        _scriptRepository = scriptRepository;
        _settingsService = settingsService;
        _databaseService = databaseService;
        _templateParser = templateParser;
        _clipboardService = clipboardService;
        _undoRedoService = undoRedoService;

        _autoSaveTimer = new System.Timers.Timer(30000);
        _autoSaveTimer.Elapsed += OnAutoSaveTimerElapsed;
        _autoSaveTimer.AutoReset = false;

        _undoRedoService.StateChanged += (s, e) => UndoRedoStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task InitializeAsync()
    {
        await _settingsService.LoadAsync();
        
        var selectedName = _settingsService.Settings.SelectedDatabaseName;
        CurrentDatabase = Databases.FirstOrDefault(d => d.Name == selectedName) ?? Databases.FirstOrDefault();
        
        var tree = await _scriptRepository.LoadAsync();
        ScriptTree.RootNodes.Clear();
        foreach (var node in tree.RootNodes)
        {
            RebuildParentReferences(node, null);
            ScriptTree.RootNodes.Add(node);
        }

        if (!string.IsNullOrEmpty(_settingsService.Settings.LastSelectedNodeId))
        {
            SelectedNode = FindNodeById(ScriptTree.RootNodes, _settingsService.Settings.LastSelectedNodeId);
            if (SelectedNode != null)
            {
                CurrentScript = SelectedNode.ScriptContent;
                _currentScriptBeforeChange = CurrentScript;
            }
        }

        SelectedNodeChanged?.Invoke(this, EventArgs.Empty);
        ScriptChanged?.Invoke(this, EventArgs.Empty);
    }

    private void RebuildParentReferences(ScriptNode node, ScriptNode? parent)
    {
        node.Parent = parent;
        foreach (var child in node.Children)
        {
            RebuildParentReferences(child, node);
        }
    }

    private ScriptNode? FindNodeById(ObservableCollection<ScriptNode> nodes, string id)
    {
        foreach (var node in nodes)
        {
            if (node.Id == id) return node;
            var found = FindNodeById(node.Children, id);
            if (found != null) return found;
        }
        return null;
    }

    public void SelectNode(ScriptNode? node)
    {
        if (SelectedNode != null && _hasUnsavedChanges)
        {
            SaveCurrentScript();
        }

        SelectedNode = node;
        CurrentScript = node?.ScriptContent ?? "";
        _currentScriptBeforeChange = CurrentScript;
        _undoRedoService.Clear();
        _hasUnsavedChanges = false;

        _settingsService.Settings.LastSelectedNodeId = node?.Id ?? "";
        _ = _settingsService.SaveAsync();

        SelectedNodeChanged?.Invoke(this, EventArgs.Empty);
        ScriptChanged?.Invoke(this, EventArgs.Empty);
    }

    public void BeginScriptEdit()
    {
        _currentScriptBeforeChange = CurrentScript;
    }

    public void OnScriptTextChanged(string newText)
    {
        if (newText == _currentScriptBeforeChange) return;

        var oldText = _currentScriptBeforeChange;
        _hasUnsavedChanges = true;
        CurrentScript = newText;

        _undoRedoService.Execute(new TextChangeAction("Edit Script",
            () => { },
            () => { }));

        _autoSaveTimer.Stop();
        _autoSaveTimer.Start();
    }

    private void OnAutoSaveTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        SaveCurrentScript();
    }

    public async void SaveCurrentScript()
    {
        if (SelectedNode == null || !_hasUnsavedChanges) return;

        SelectedNode.ScriptContent = CurrentScript;
        SelectedNode.ModifiedDate = DateTime.Now;
        _hasUnsavedChanges = false;

        await SaveScriptsAsync();
    }

    public async Task SaveScriptsAsync()
    {
        await _scriptRepository.SaveAsync(ScriptTree);
    }

    public ScriptNode AddNode(string name, NodeType nodeType, ScriptNode? parent = null)
    {
        var node = new ScriptNode
        {
            Name = name,
            NodeType = nodeType,
            ParentId = parent?.Id
        };
        node.Parent = parent;

        if (parent == null)
        {
            node.SortOrder = RootNodes.Count;
            RootNodes.Add(node);
        }
        else
        {
            node.SortOrder = parent.Children.Count;
            parent.Children.Add(node);
        }

        _ = SaveScriptsAsync();
        return node;
    }

    public void DeleteNode(ScriptNode node)
    {
        if (node.Parent != null)
        {
            node.Parent.Children.Remove(node);
        }
        else
        {
            RootNodes.Remove(node);
        }

        if (SelectedNode == node)
        {
            SelectedNode = null;
            CurrentScript = "";
            SelectedNodeChanged?.Invoke(this, EventArgs.Empty);
        }

        _ = SaveScriptsAsync();
    }

    public ScriptNode DuplicateNode(ScriptNode sourceNode)
    {
        var parent = sourceNode.Parent;
        var newNode = new ScriptNode
        {
            Name = sourceNode.Name + " (Copy)",
            NodeType = sourceNode.NodeType,
            ScriptContent = sourceNode.ScriptContent,
            ParentId = parent?.Id,
            Parent = parent
        };

        if (parent == null)
        {
            newNode.SortOrder = RootNodes.Count;
            RootNodes.Add(newNode);
        }
        else
        {
            newNode.SortOrder = parent.Children.Count;
            parent.Children.Add(newNode);
        }

        _ = SaveScriptsAsync();
        return newNode;
    }

    public string GetScriptWithValuesApplied(string? script = null)
    {
        var scriptText = script ?? CurrentScript;
        if (string.IsNullOrEmpty(scriptText)) return scriptText;

        var values = CurrentRowContext.Values
            .Where(kvp => kvp.Value != null)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.ToString() ?? "");
        
        return ReplaceTemplateVariables(values, scriptText);
    }

    public ScriptNode? DuplicateNodeWithValues(ScriptNode sourceNode)
    {
        var scriptWithValues = GetScriptWithValuesApplied(sourceNode.ScriptContent);
        
        var parent = sourceNode.Parent;
        var newNode = new ScriptNode
        {
            Name = sourceNode.Name + " (Values)",
            NodeType = NodeType.Script,
            ScriptContent = scriptWithValues,
            ParentId = parent?.Id,
            Parent = parent
        };

        if (parent == null)
        {
            newNode.SortOrder = RootNodes.Count;
            RootNodes.Add(newNode);
        }
        else
        {
            newNode.SortOrder = parent.Children.Count;
            parent.Children.Add(newNode);
        }

        _ = SaveScriptsAsync();
        return newNode;
    }

    public void MoveNode(ScriptNode node, ScriptNode? newParent, int index)
    {
        if (node.Parent != null)
        {
            node.Parent.Children.Remove(node);
        }
        else
        {
            RootNodes.Remove(node);
        }

        node.Parent = newParent;
        node.ParentId = newParent?.Id;

        if (newParent == null)
        {
            if (index >= RootNodes.Count)
                RootNodes.Add(node);
            else
                RootNodes.Insert(index, node);
        }
        else
        {
            if (index >= newParent.Children.Count)
                newParent.Children.Add(node);
            else
                newParent.Children.Insert(index, node);
        }

        _ = SaveScriptsAsync();
    }

    public void RenameNode(ScriptNode node, string newName)
    {
        var oldName = node.Name;
        node.Name = newName;
        node.ModifiedDate = DateTime.Now;

        _undoRedoService.Execute(new TextChangeAction("Rename",
            () => { },
            () => { node.Name = oldName; }));

        _ = SaveScriptsAsync();
    }

    public List<string> GetTemplateVariables(string? script = null)
    {
        var scriptToParse = script ?? CurrentScript;
        return _templateParser.ExtractVariables(scriptToParse);
    }

    public string ReplaceTemplateVariables(Dictionary<string, string> values, string? script = null)
    {
        var scriptToReplace = script ?? CurrentScript;
        return _templateParser.ReplaceVariables(scriptToReplace, values);
    }

    public async Task ExecuteScriptAsync(Dictionary<string, string>? templateValues = null, string? script = null)
    {
        var scriptToRun = script ?? CurrentScript;
        
        if (string.IsNullOrWhiteSpace(scriptToRun))
        {
            StatusMessage = "No script selected or script is empty";
            StatusChanged?.Invoke(this, EventArgs.Empty);
            return;
        }

        var finalScript = templateValues != null && templateValues.Count > 0
            ? ReplaceTemplateVariables(templateValues, scriptToRun)
            : scriptToRun;

        if (RowCount > 0)
        {
            finalScript = $"SET ROWCOUNT {RowCount}\n{finalScript}\nSET ROWCOUNT 0";
        }

        _cancellationTokenSource = new CancellationTokenSource();
        
        IsExecuting = true;
        StatusMessage = "Executing...";
        StatusChanged?.Invoke(this, EventArgs.Empty);

        var result = await _databaseService.ExecuteQueryAsync(CurrentDatabase!, finalScript, _cancellationTokenSource.Token);

        IsExecuting = false;
        LastExecutionTime = result.Elapsed;

        if (result.Error != null)
        {
            LastError = result.Error;
            CachedError = result.Error;
            StatusMessage = $"Error: {result.Error} ({result.Elapsed.TotalSeconds:F2}s)";
            QueryResults = null;
        }
        else
        {
            LastError = null;
            CachedError = null;
            QueryResults = result.Data;
            CacheResults(SelectedNode!.Id, result.Data);
            var totalRows = result.Data.Sum(dt => dt.Rows.Count);
            var resultSetInfo = result.Data.Count > 1 
                ? $" ({result.Data.Count} result sets)" 
                : result.Data.Count == 1 && totalRows > 0 ? $" ({totalRows} rows)" : "";
            StatusMessage = $"Query completed{resultSetInfo} ({result.Elapsed.TotalSeconds:F2}s)";
        }

        ResultsChanged?.Invoke(this, EventArgs.Empty);
        StatusChanged?.Invoke(this, EventArgs.Empty);
    }

    public void CancelExecution()
    {
        _cancellationTokenSource?.Cancel();
        _databaseService.CancelQuery();
    }

    public void CacheResults(string nodeId, List<DataTable> results)
    {
        _resultsCache[nodeId] = results.Select(dt => dt.Copy()).ToList();
    }

    public (List<DataTable>? Results, string? Error) GetCachedResults(string nodeId)
    {
        if (_resultsCache.TryGetValue(nodeId, out var results))
        {
            return (results.Select(dt => dt.Copy()).ToList(), null);
        }
        return (null, null);
    }

    public async Task<bool> TestDatabaseConnectionAsync()
    {
        return await _databaseService.TestConnectionAsync(CurrentDatabase!);
    }

    public void SelectDatabase(string name)
    {
        CurrentDatabase = Databases.FirstOrDefault(d => d.Name == name);
        _settingsService.Settings.SelectedDatabaseName = name;
        _ = _settingsService.SaveAsync();
    }

    public void UpdateRowContext(DataGridViewRow row)
    {
        foreach (DataGridViewCell cell in row.Cells)
        {
            if (cell.OwningColumn.DataPropertyName != null)
            {
                CurrentRowContext.Values[cell.OwningColumn.DataPropertyName] = cell.Value;
            }
        }
        CurrentRowContext.CreatedAt = DateTime.Now;
    }

    public void Undo()
    {
        _undoRedoService.Undo();
    }

    public void Redo()
    {
        _undoRedoService.Redo();
    }

    public void CopyGrid(DataGridView grid) => _clipboardService.CopyToClipboard(grid);
    public void CutGrid(DataGridView grid) => _clipboardService.CutToClipboard(grid);
    public bool PasteGrid(DataGridView grid, IDataObject? data) => _clipboardService.PasteFromClipboard(grid, data);
}
