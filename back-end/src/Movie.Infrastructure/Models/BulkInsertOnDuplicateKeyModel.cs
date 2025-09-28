namespace Movie.Infrastructure.Models;

public class InsertOnDuplicateKeyModel
{
    public List<object[]> InsertValues { get; set; }
    public string TableName { get; set; }
    public List<SqlParameterColumnMapping> SqlParameterColumnMappings { get; set; }
    public List<string> DuplicateKeyUpdateValues { get; set; }
}