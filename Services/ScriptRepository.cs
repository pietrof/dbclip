using System.IO;
using System.Text.Json;
using DBClip.Models;

namespace DBClip.Services;

public interface IScriptRepository
{
    Task<ScriptTree> LoadAsync();
    Task SaveAsync(ScriptTree tree);
}

public class JsonScriptRepository : IScriptRepository
{
    private readonly string _filePath;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public JsonScriptRepository()
    {
        var appDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "DBClip");
        Directory.CreateDirectory(appDataPath);
        _filePath = Path.Combine(appDataPath, "scripts.json");
    }

    public async Task<ScriptTree> LoadAsync()
    {
        if (!File.Exists(_filePath))
        {
            return CreateDefaultTree();
        }

        try
        {
            var json = await File.ReadAllTextAsync(_filePath);
            var tree = JsonSerializer.Deserialize<ScriptTree>(json, JsonOptions);
            return tree ?? CreateDefaultTree();
        }
        catch
        {
            return CreateDefaultTree();
        }
    }

    public async Task SaveAsync(ScriptTree tree)
    {
        tree.LastModified = DateTime.Now;
        var json = JsonSerializer.Serialize(tree, JsonOptions);
        await File.WriteAllTextAsync(_filePath, json);
    }

    private ScriptTree CreateDefaultTree()
    {
        var tree = new ScriptTree();
        var sampleNode = new ScriptNode
        {
            Name = "Sample Query",
            ScriptContent = "-- Enter your SQL query here\nSELECT * FROM Users WHERE Id = {id}",
            NodeType = NodeType.Script
        };
        tree.RootNodes.Add(sampleNode);
        return tree;
    }
}
