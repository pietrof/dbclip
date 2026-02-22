using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace DBClip.Models;

public enum NodeType
{
    Folder,
    Script
}

public class ScriptNode
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "New Script";
    public string ScriptContent { get; set; } = "";
    public NodeType NodeType { get; set; } = NodeType.Script;
    public ObservableCollection<ScriptNode> Children { get; set; } = new();
    public string? ParentId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime ModifiedDate { get; set; } = DateTime.Now;
    public int SortOrder { get; set; }

    [JsonIgnore]
    public ScriptNode? Parent { get; set; }
}

public class ScriptTree
{
    public ObservableCollection<ScriptNode> RootNodes { get; set; } = new();
    public DateTime LastModified { get; set; } = DateTime.Now;
}

public class DatabaseSettings
{
    public string Name { get; set; } = "Default";
    public string Provider { get; set; } = "SqlServer";
    public string ConnectionString { get; set; } = "";
    public bool SavePassword { get; set; } = false;
}

public class AppSettings
{
    public List<DatabaseSettings> Databases { get; set; } = new() { new DatabaseSettings() };
    public string? SelectedDatabaseName { get; set; }
    public string LastSelectedNodeId { get; set; } = "";
    public double WindowLeft { get; set; } = double.NaN;
    public double WindowTop { get; set; } = double.NaN;
    public double WindowWidth { get; set; } = 1200;
    public double WindowHeight { get; set; } = 800;
    public bool IsMaximized { get; set; } = false;
}

public class RowContext
{
    public Dictionary<string, object?> Values { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
