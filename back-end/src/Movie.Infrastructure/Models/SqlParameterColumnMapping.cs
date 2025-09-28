using NpgsqlTypes;

namespace Movie.Infrastructure.Models;

public class SqlParameterColumnMapping
{
    public NpgsqlDbType NpgsqlDbType { get; set; }
    public string ColumnName { get; set; }
}