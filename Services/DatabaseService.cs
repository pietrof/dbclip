using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Text.RegularExpressions;
using DBClip.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using Npgsql;

namespace DBClip.Services;

public interface IDatabaseService
{
    Task<bool> TestConnectionAsync(DatabaseSettings settings);
    Task<(List<DataTable> Data, string? Error, TimeSpan Elapsed)> ExecuteQueryAsync(DatabaseSettings settings, string query, CancellationToken cancellationToken = default);
    void CancelQuery();
}

public class DatabaseService : IDatabaseService
{
    private CancellationTokenSource? _cancellationTokenSource;

    public async Task<bool> TestConnectionAsync(DatabaseSettings settings)
    {
        try
        {
            using var conn = CreateConnection(settings);
            await conn.OpenAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<(List<DataTable> Data, string? Error, TimeSpan Elapsed)> ExecuteQueryAsync(
        DatabaseSettings settings, string query, CancellationToken cancellationToken = default)
    {
        var startTime = DateTime.Now;
        var dataTables = new List<DataTable>();

        try
        {
            using var conn = CreateConnection(settings);
            await conn.OpenAsync(cancellationToken);

            using var cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.CommandTimeout = 300;

            using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
            
            do
            {
                var schemaTable = reader.GetColumnSchema();
                var dataTable = new DataTable();
                
                foreach (var column in schemaTable)
                {
                    var dataColumn = new DataColumn(column.ColumnName, column.DataType ?? typeof(object));
                    dataTable.Columns.Add(dataColumn);
                }
                
                while (reader.Read())
                {
                    var row = dataTable.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[i] = reader.IsDBNull(i) ? DBNull.Value : reader.GetValue(i);
                    }
                    dataTable.Rows.Add(row);
                }
                
                dataTables.Add(dataTable);
            } while (!reader.IsClosed && reader.NextResult());

            var elapsed = DateTime.Now - startTime;
            return (dataTables, null, elapsed);
        }
        catch (Exception ex)
        {
            var elapsed = DateTime.Now - startTime;
            return (new List<DataTable>(), ex.Message, elapsed);
        }
    }

    private DbConnection CreateConnection(DatabaseSettings settings)
    {
        return settings.Provider switch
        {
            "SqlServer" => new SqlConnection(settings.ConnectionString),
            "PostgreSQL" => new NpgsqlConnection(settings.ConnectionString),
            "MySQL" => new MySqlConnection(settings.ConnectionString),
            "SQLite" => new SqliteConnection(settings.ConnectionString),
            "Sybase" => new OdbcConnection(settings.ConnectionString),
            _ => throw new NotSupportedException($"Provider {settings.Provider} is not supported")
        };
    }

    public void CancelQuery()
    {
        _cancellationTokenSource?.Cancel();
    }
}

public interface ITemplateParser
{
    List<string> ExtractVariables(string script);
    string ReplaceVariables(string script, Dictionary<string, string> values);
}

public class TemplateParser : ITemplateParser
{
    private static readonly Regex TemplateRegex = new(@"\{(\w+)\}", RegexOptions.Compiled);

    public List<string> ExtractVariables(string script)
    {
        var matches = TemplateRegex.Matches(script);
        return matches.Select(m => m.Groups[1].Value).Distinct().ToList();
    }

    public string ReplaceVariables(string script, Dictionary<string, string> values)
    {
        return TemplateRegex.Replace(script, m =>
        {
            var varName = m.Groups[1].Value;
            return values.TryGetValue(varName, out var value) ? value : m.Value;
        });
    }
}
