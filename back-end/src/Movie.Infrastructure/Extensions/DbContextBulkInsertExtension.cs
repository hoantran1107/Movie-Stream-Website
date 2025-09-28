using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Movie.Infrastructure.Constants;
using Movie.Infrastructure.Exceptions;
using Movie.Infrastructure.Models;
using Npgsql;

namespace Movie.Infrastructure.Extensions;

public static partial class DbContextExtension
{
    /// <summary>
    ///     Bulkinsert a collection from memory into a certain table and receive a list of ids just be generated
    /// </summary>
    /// <param name="bulkInsertValue">Insert value input is a list query sentences</param>
    /// <param name="timeout"></param>
    /// <returns> get about an id list after inserted into a database </returns>
    public static List<int> ExecuteBulkInsertAndGetIds(this DbContext dbContext, BulkInsertModel bulkInsertValue,
        int timeout = 60)
    {
        if (bulkInsertValue == null || !bulkInsertValue.SqlParameterColumnMappings.Any() ||
            !bulkInsertValue.InsertValues.Any()) throw new BulkInsertException("Invalid bulkInsertValue model");

        if (bulkInsertValue.SqlParameterColumnMappings.Count != bulkInsertValue.InsertValues.FirstOrDefault()?.Length)
            throw new BulkInsertException(
                "Invalid bulkInsertValue model: mismatch columns-size between definition and values");

        var ids = new List<int>();
        try
        {
            using (NpgsqlCommand command = new(null, (NpgsqlConnection)dbContext.Database.GetDbConnection()))
            {
                command.CommandText = PrepareBulkInsert(bulkInsertValue, true);
                command.CommandTimeout = timeout;
                for (var indexRow = 0; indexRow < bulkInsertValue.InsertValues.Count; indexRow++)
                for (var indexCol = 0; indexCol < bulkInsertValue.SqlParameterColumnMappings.Count; indexCol++)
                {
                    var parameter = command.CreateParameter();
                    var currentIndexParam = indexRow * bulkInsertValue.SqlParameterColumnMappings.Count + indexCol;
                    parameter.ParameterName = $"@p{currentIndexParam}";
                    parameter.NpgsqlDbType = bulkInsertValue.SqlParameterColumnMappings[indexCol].NpgsqlDbType;
                    parameter.Value = bulkInsertValue.InsertValues[indexRow][indexCol] ?? DBNull.Value;
                    command.Parameters.Add(parameter);
                }

                var trans = dbContext.Database.CurrentTransaction?.GetDbTransaction();

                if (trans != null) command.Transaction = (NpgsqlTransaction?)trans;
                using var result = command.ExecuteReader();
                while (result.Read())
                {
                    var insertedId = result.GetInt32(0); // Assuming id is of type int
                    ids.Add(insertedId);
                }
            }

            return ids;
        }
        catch (Exception e)
        {
            throw new BulkInsertException(e, "Bulk insert get id error");
        }
    }

    /// <summary>
    ///     Bulkinsert a collection from memory into a certain table
    /// </summary>
    /// <param name="bulkInsertValue">Insert value input is a list query sentences</param>
    /// <param name="timeout"></param>
    public static void ExecuteBulkInsert(this DbContext dbContext, BulkInsertModel bulkInsertValue, int timeout = 60)
    {
        if (bulkInsertValue == null || !bulkInsertValue.SqlParameterColumnMappings.Any() ||
            !bulkInsertValue.InsertValues.Any()) throw new BulkInsertException("Invalid bulkInsertValue model");
        if (bulkInsertValue.SqlParameterColumnMappings.Count != bulkInsertValue.InsertValues.FirstOrDefault()?.Length)
            throw new BulkInsertException(
                "Invalid bulkInsertValue model: mismatch columns-size between definition and values");
        try
        {
            using NpgsqlCommand command = new(null, (NpgsqlConnection)dbContext.Database.GetDbConnection());

            //Prepare query
            command.CommandText = PrepareBulkInsert(bulkInsertValue, false);
            command.CommandTimeout = timeout;
            for (var indexRow = 0; indexRow < bulkInsertValue.InsertValues.Count; indexRow++)
            for (var indexCol = 0; indexCol < bulkInsertValue.SqlParameterColumnMappings.Count; indexCol++)
            {
                var parameter = command.CreateParameter();
                var currentIndexParam = indexRow * bulkInsertValue.SqlParameterColumnMappings.Count + indexCol;
                parameter.ParameterName = $"@p{currentIndexParam}";
                parameter.NpgsqlDbType = bulkInsertValue.SqlParameterColumnMappings[indexCol].NpgsqlDbType;
                parameter.Value = bulkInsertValue.InsertValues[indexRow][indexCol] ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }

            var trans = dbContext.Database.CurrentTransaction?.GetDbTransaction();
            if (trans != null) command.Transaction = (NpgsqlTransaction?)trans;
            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw new BulkInsertException(e, "Bulk insert error");
        }
    }

    /// <summary>
    ///     Insert on dulicate key <br />
    ///     Document: https://dev.mysql.com/doc/refman/8.0/en/insert-on-duplicate.html
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="insertOnDuplicateKeyModel"></param>
    /// <param name="timeout"></param>
    public static void ExecuteInsertOnDuplicateKey(this DbContext dbContext,
        InsertOnDuplicateKeyModel insertOnDuplicateKeyModel, int timeout = 60)
    {
        if (insertOnDuplicateKeyModel == null || !insertOnDuplicateKeyModel.SqlParameterColumnMappings.Any() ||
            !insertOnDuplicateKeyModel.InsertValues.Any())
            throw new BulkInsertException("Invalid insertOnDuplicateKey model");

        if (insertOnDuplicateKeyModel.SqlParameterColumnMappings.Count !=
            insertOnDuplicateKeyModel.InsertValues.FirstOrDefault()?.Length)
            throw new BulkInsertException(
                "Invalid insertOnDuplicateKey model: mismatch columns-size between definition and values");

        try
        {
            NpgsqlCommand command = new(null, (NpgsqlConnection)dbContext.Database.GetDbConnection());
            command.CommandText = PrepareInsertDuplicateKey(insertOnDuplicateKeyModel);
            command.CommandTimeout = timeout;
            for (var indexRow = 0; indexRow < insertOnDuplicateKeyModel.InsertValues.Count; indexRow++)
            for (var indexCol = 0; indexCol < insertOnDuplicateKeyModel.SqlParameterColumnMappings.Count; indexCol++)
            {
                var parameter = command.CreateParameter();
                var currentIndexParam =
                    indexRow * insertOnDuplicateKeyModel.SqlParameterColumnMappings.Count + indexCol;
                parameter.ParameterName = $"@p{currentIndexParam}";
                parameter.NpgsqlDbType = insertOnDuplicateKeyModel.SqlParameterColumnMappings[indexCol].NpgsqlDbType;
                parameter.Value = insertOnDuplicateKeyModel.InsertValues[indexRow][indexCol];
                command.Parameters.Add(parameter);
            }

            var trans = dbContext.Database.CurrentTransaction?.GetDbTransaction();

            if (trans != null) command.Transaction = (NpgsqlTransaction?)trans;
            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw new BulkInsertException(e, "Insert on duplicate key error");
        }
    }

    /// <summary>
    ///     It can occur 2 case<br />
    ///     1: Not Get id just inserted (field name is row_id)<br />
    ///     Case 2: Get id just inserted
    /// </summary>
    /// <param name="bulkInsertValue"></param>
    /// <param name="isGetId">
    ///     if this variable is true is just normal insert. Otherwise, function will get id list, other
    ///     function can use it.
    /// </param>
    /// <returns></returns>
    private static string PrepareBulkInsert(BulkInsertModel bulkInsertValue, bool isGetId)
    {
        //Build up string of columns need to be inserted
        var columnsStr = string.Join(", ", bulkInsertValue.SqlParameterColumnMappings
            .Select(r => $"\"{r.ColumnName}\"").ToList());

        StringBuilder sCommand;

        var columnsSize = bulkInsertValue.SqlParameterColumnMappings.Count;

        var parameters = string.Join(BulkOperationConstant.SetepareValue,
            bulkInsertValue.InsertValues.Select((dataRow, indexRow) =>
                "(" + string.Join(
                    BulkOperationConstant.SetepareValue,
                    dataRow.Select((data, index) => $"@p{indexRow * columnsSize + index}")) + ")"
            ));

        if (!isGetId)
            sCommand = new StringBuilder(BulkOperationConstant.BulkInsert.Replace("tbName", bulkInsertValue.TableName)
                .Replace("column", columnsStr)
                .Replace("listvalue", parameters));
        else
            sCommand = new StringBuilder(BulkOperationConstant.BulkInsertGetId
                .Replace("tbName", bulkInsertValue.TableName)
                .Replace("column", columnsStr)
                .Replace("listvalue", parameters));
        var query = sCommand.ToString();
        return query;
    }

    /// <summary>
    ///     Document: https://dev.mysql.com/doc/refman/8.0/en/insert-on-duplicate.html
    /// </summary>
    /// <param name="insertOnDuplicateKeyModel"></param>
    /// <returns></returns>
    private static string PrepareInsertDuplicateKey(InsertOnDuplicateKeyModel insertOnDuplicateKeyModel)
    {
        //Build up string of columns need to be inserted
        var columnsStr = string.Join(", ", insertOnDuplicateKeyModel.SqlParameterColumnMappings
            .Select(r => $"`{r.ColumnName}`").ToList());
        StringBuilder sCommand;

        var columnsSize = insertOnDuplicateKeyModel.SqlParameterColumnMappings.Count;

        var parameters = string.Join(BulkOperationConstant.SetepareValue,
            insertOnDuplicateKeyModel.InsertValues.Select((dataRow, indexRow) =>
                "(" + string.Join(
                    BulkOperationConstant.SetepareValue,
                    dataRow.Select((data, index) => $"@p{indexRow * columnsSize + index}")) + ")"
            ));
        var updateValue = string.Join(BulkOperationConstant.SetepareValue,
            insertOnDuplicateKeyModel.DuplicateKeyUpdateValues.Select(e => $"`{e}` = VALUES(`{e}`)"));

        sCommand = new StringBuilder(BulkOperationConstant.InsertOnDuplicateKey
            .Replace("tblName", insertOnDuplicateKeyModel.TableName)
            .Replace("column", columnsStr)
            .Replace("lstValue", parameters)
            .Replace("duplicateKeyUpdate", updateValue));
        var query = sCommand.ToString();
        return query;
    }
}