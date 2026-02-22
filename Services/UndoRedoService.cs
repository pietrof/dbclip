namespace DBClip.Services;

public interface IUndoRedoService
{
    void Execute(IUndoableAction action);
    bool CanUndo { get; }
    bool CanRedo { get; }
    void Undo();
    void Redo();
    void Clear();
    event EventHandler? StateChanged;
}

public interface IUndoableAction
{
    void Execute();
    void Undo();
    string Description { get; }
}

public class UndoRedoService : IUndoRedoService
{
    private readonly Stack<IUndoableAction> _undoStack = new();
    private readonly Stack<IUndoableAction> _redoStack = new();
    private const int MaxUndoLevels = 100;

    public bool CanUndo => _undoStack.Count > 0;
    public bool CanRedo => _redoStack.Count > 0;

    public event EventHandler? StateChanged;

    public void Execute(IUndoableAction action)
    {
        action.Execute();
        _undoStack.Push(action);
        _redoStack.Clear();

        while (_undoStack.Count > MaxUndoLevels)
        {
            var items = _undoStack.ToArray();
            _undoStack.Clear();
            for (int i = 0; i < items.Length - 1; i++)
            {
                _undoStack.Push(items[i]);
            }
        }

        StateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Undo()
    {
        if (!CanUndo) return;

        var action = _undoStack.Pop();
        action.Undo();
        _redoStack.Push(action);
        StateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Redo()
    {
        if (!CanRedo) return;

        var action = _redoStack.Pop();
        action.Execute();
        _undoStack.Push(action);
        StateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Clear()
    {
        _undoStack.Clear();
        _redoStack.Clear();
        StateChanged?.Invoke(this, EventArgs.Empty);
    }
}

public class TextChangeAction : IUndoableAction
{
    private readonly Action _execute;
    private readonly Action _undo;
    private readonly string _description;

    public string Description => _description;

    public TextChangeAction(string description, Action execute, Action undo)
    {
        _description = description;
        _execute = execute;
        _undo = undo;
    }

    public void Execute() => _execute();
    public void Undo() => _undo();
}
