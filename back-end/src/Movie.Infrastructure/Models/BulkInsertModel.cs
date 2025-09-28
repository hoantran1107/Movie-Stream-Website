namespace Movie.Infrastructure.Models;

public class BulkInsertModel
{
    /// <summary>
    ///     String list to query sentence (Insert into table(column of table) values(v1),(),...)
    /// </summary>
    public List<object?[]> InsertValues { get; set; } = new();

    /// <summary>
    ///     this variable is table name need insert
    /// </summary>
    public string? TableName { get; set; }

    /// <summary>
    ///     Assemblage all column of table at here
    /// </summary>
    public List<SqlParameterColumnMapping>? SqlParameterColumnMappings { get; set; }
}