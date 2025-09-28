namespace Movie.Infrastructure.Models;

public class BulkDeleteModel
{
    /// <summary>
    ///     List of integer (ID) to query sentence (Delete from table where id in...)
    /// </summary>
    public List<object> ListIDs { get; set; }

    /// <summary>
    ///     this variable is column name need to check condition
    /// </summary>
    public string ColumnName { get; set; }

    /// <summary>
    ///     this variable is table name need insert
    /// </summary>
    public string TableName { get; set; }
}

public class BulkDeleteByCompositeKeyModel
{
    /// <summary>
    ///     this variable is table name need delete
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    ///     String list to query sentence (Update set (case when then end) where(v1),(),...)
    /// </summary>
    public List<object[]> KeyValues { get; set; }

    /// <summary>
    ///     Assemblage all column conditions CompositeKey) at here
    /// </summary>
    public List<string> ColumnNames { get; set; }
}