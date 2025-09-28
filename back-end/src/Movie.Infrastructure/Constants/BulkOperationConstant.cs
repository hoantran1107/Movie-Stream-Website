namespace Movie.Infrastructure.Constants;

internal static class BulkOperationConstant
{
    public const string SetepareValue = ",";
    public const string BulkInsert = @"INSERT INTO tbName (column) VALUES listvalue";
    public const string BulkInsertGetId = @"INSERT INTO tbName(column) VALUES listvalue returning id;";

    public const string BulkUpdateJoin = @"UPDATE tbName SET [#UpdateStatements] WHERE [#Conditions]";

    public const string BulkUpdateTemporaryTableJoin = @"UPDATE [#Table01] AS T1
INNER JOIN [#TempTable01] AS T2 ON ([#InnerJoinConditions])
SET [#UpdateStatements]";

    public const string CreateTempTable =
        @"CREATE TEMPORARY TABLE IF NOT EXISTS [#TempTableName] AS ([#DataSourceQueryForTempTable])";

    public const string BulkDelete = @"DELETE FROM tbName WHERE columnName IN (listValue)";
    public const string BulkDeleteJoin = @"DELETE FROM tbName WHERE [#Conditions]";

    public const string InsertOnDuplicateKey =
        @"INSERT INTO tblName (column) VALUES lstValue ON DUPLICATE KEY UPDATE duplicateKeyUpdate;";
}