using System.IO;
using System.Text.Json;
using DBClip.Models;

namespace DBClip.Services;

public interface ISettingsService
{
    AppSettings Settings { get; }
    Task LoadAsync();
    Task SaveAsync();
}

public class SettingsService : ISettingsService
{
    private readonly string _filePath;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public AppSettings Settings { get; private set; } = new();

    public SettingsService()
    {
        var appDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "DBClip");
        Directory.CreateDirectory(appDataPath);
        _filePath = Path.Combine(appDataPath, "settings.json");
    }

    public async Task LoadAsync()
    {
        if (!File.Exists(_filePath))
        {
            Settings = new AppSettings();
            return;
        }

        try
        {
            var json = await File.ReadAllTextAsync(_filePath);
            var loaded = JsonSerializer.Deserialize<AppSettings>(json, JsonOptions) ?? new AppSettings();
            
            foreach (var db in loaded.Databases)
            {
                db.ConnectionString = EncryptionHelper.Decrypt(db.ConnectionString);
            }
            
            Settings = loaded;
        }
        catch
        {
            Settings = new AppSettings();
        }
    }

    public async Task SaveAsync()
    {
        var settingsToSave = new AppSettings
        {
            Databases = Settings.Databases.Select(d => new DatabaseSettings
            {
                Name = d.Name,
                Provider = d.Provider,
                ConnectionString = EncryptionHelper.Encrypt(d.ConnectionString),
                SavePassword = d.SavePassword
            }).ToList(),
            SelectedDatabaseName = Settings.SelectedDatabaseName,
            LastSelectedNodeId = Settings.LastSelectedNodeId,
            WindowLeft = Settings.WindowLeft,
            WindowTop = Settings.WindowTop,
            WindowWidth = Settings.WindowWidth,
            WindowHeight = Settings.WindowHeight,
            IsMaximized = Settings.IsMaximized
        };
        
        var json = JsonSerializer.Serialize(settingsToSave, JsonOptions);
        await File.WriteAllTextAsync(_filePath, json);
    }
}
