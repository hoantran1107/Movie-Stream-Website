using Movie.Infrastructure.Models;

namespace GenericRepository.Models;

public class BulkUpdateByIdentityKeyModel
{
    /// <summary>
    ///     this variable is table name need update
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    ///     String list to query sentence (Update set (case when then end) where(v1),(),...)
    /// </summary>
    public List<object[]> UpdateValues { get; set; }

    /// <summary>
    ///     Assemblage all column of table need to update at here
    ///     The unique keys column must be insert after the non unique columns.
    /// </summary>
    public List<SqlParameterColumnMapping> SqlParameterColumnMappings { get; set; }

    /// <summary>
    ///     String list to query sentence (Update set (case when then end) where(v1),(),...)
    /// </summary>
    public List<object[]> KeyValues { get; set; }

    /// <summary>
    ///     Assemblage all column conditions primary key at here
    /// </summary>
    public SqlParameterColumnMapping SqlParameterKeyColumnMappings { get; set; }
}

public class BulkUpdateByCompositeKeyModel
{
    /// <summary>
    ///     this variable is table name need update
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    ///     String list to query sentence (Update set (case when then end) where(v1),(),...)
    /// </summary>
    public List<object[]> UpdateValues { get; set; }

    /// <summary>
    ///     Assemblage all column of table need to update at here <br />
    ///     The unique keys column must be insert after the non unique columns.
    /// </summary>
    public List<SqlParameterColumnMapping> SqlParameterColumnMappings { get; set; }

    /// <summary>
    ///     String list to query sentence (Update set (case when then end) where(v1),(),...)
    /// </summary>
    public List<object[]> KeyValues { get; set; }

    /// <summary>
    ///     Assemblage all column conditions CompositeKey) at here
    /// </summary>
    public List<SqlParameterColumnMapping> SqlParameterKeyColumnMappings { get; set; }
}