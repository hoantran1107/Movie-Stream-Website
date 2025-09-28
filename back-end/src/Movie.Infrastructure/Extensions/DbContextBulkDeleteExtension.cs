using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Movie.Infrastructure.Constants;
using Movie.Infrastructure.Exceptions;
using Movie.Infrastructure.Models;

namespace Movie.Infrastructure.Extensions;

public static partial class DbContextExtension
{
    /// <summary>
    ///     Only tables with primary keys will be deleted.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="bulkDeleteModel"></param>
    public static void ExecuteBulkDelete(this DbContext dbContext, BulkDeleteModel bulkDeleteModel, int timeout = 60)
    {
        try
        {
            //check listID is null
            if (!bulkDeleteModel.ListIDs.Any())
                throw new BulkDeleteException("Invalid bulkDeleteModel, ListIDs must be not null");

            using var command = dbContext.Database.GetDbConnection().CreateCommand();
            for (var index = 0; index < bulkDeleteModel.ListIDs.Count; index++)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = $"@p{index}";
                parameter.Value = bulkDeleteModel.ListIDs[index];
                command.Parameters.Add(parameter);
            }

            command.CommandText = PrepareBulkDelete(bulkDeleteModel);
            command.CommandTimeout = timeout;

            var trans = dbContext.Database.CurrentTransaction?.GetDbTransaction();

            if (trans != null) command.Transaction = trans;

            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw new BulkDeleteException(e, "Bulk delete error");
        }
    }

    /// <summary>
    ///     Generate bulk delete command
    /// </summary>
    /// <param name="bulkDeleteModel"></param>
    /// <returns></returns>
    private static string PrepareBulkDelete(BulkDeleteModel bulkDeleteModel)
    {
        var listValue = new StringBuilder();
        StringBuilder sCommand;

        var parameterList = new StringBuilder();
        var parameters = string.Join(
            BulkOperationConstant.SetepareValue,
            bulkDeleteModel.ListIDs.Select((data, index) => $"@p{index}"));

        sCommand = new StringBuilder(BulkOperationConstant.BulkDelete.Replace("tbName", bulkDeleteModel.TableName)
            .Replace("columnName", bulkDeleteModel.ColumnName)
            .Replace("listValue", parameters));
        var query = sCommand.ToString();
        return query;
    }

    /// <summary>
    ///     Only tables with composite keys will be deleted.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="bulkDeleteValue"></param>
    /// <param name="timeout"></param>
    public static void ExecuteBulkDeleteByCompositeKey(
        this DbContext dbContext,
        BulkDeleteByCompositeKeyModel bulkDeleteValue, int timeout = 60)
    {
        if (bulkDeleteValue == null || !bulkDeleteValue.KeyValues.Any() || !bulkDeleteValue.ColumnNames.Any())
            throw new BulkDeleteException("Invalid bulkDeleteValue model");

        if (bulkDeleteValue.KeyValues.FirstOrDefault()?.Length != bulkDeleteValue.ColumnNames.Count)
            throw new BulkDeleteException(
                "Invalid bulkDeleteValue model: mismatch columns-size between definition and values");

        try
        {
            using (var command = dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = PrepareBulkUpdateByCompositeKey(bulkDeleteValue);
                command.CommandTimeout = timeout;
                for (var indexRow = 0; indexRow < bulkDeleteValue.KeyValues.Count; indexRow++)
                for (var indexCol = 0; indexCol < bulkDeleteValue.ColumnNames.Count; indexCol++)
                {
                    var parameter = command.CreateParameter();
                    var currentIndexParam = indexRow * bulkDeleteValue.ColumnNames.Count + indexCol;
                    parameter.ParameterName = $"@p{currentIndexParam}";
                    parameter.Value = bulkDeleteValue.KeyValues[indexRow][indexCol];
                    command.Parameters.Add(parameter);
                }

                var trans = dbContext.Database.CurrentTransaction?.GetDbTransaction();

                if (trans != null) command.Transaction = trans;

                command.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            throw new BulkDeleteException(e, "Bulk delete by composite key error");
        }
    }

    /// <summary>
    ///     Generate bulk delete command
    /// </summary>
    /// <param name="bulkDeleteValue"></param>
    /// <returns></returns>
    private static string PrepareBulkUpdateByCompositeKey(BulkDeleteByCompositeKeyModel bulkDeleteValue)
    {
        StringBuilder sCommand;

        //Build up string of parameters conditions
        //List of parameter conditions
        var columnsKeySize = bulkDeleteValue.ColumnNames.Count;
        var conditionList = string.Empty;
        var conditionParameterList = new string[bulkDeleteValue.KeyValues.Count];

        for (var indexRow = 0; indexRow < bulkDeleteValue.KeyValues.Count; indexRow++)
        for (var indexCol = 0; indexCol < bulkDeleteValue.ColumnNames.Count; indexCol++)
        {
            var param = $"@p{indexRow * columnsKeySize + indexCol}";
            if (indexCol == 0)
                conditionParameterList[indexRow] = $"WHEN `{bulkDeleteValue.ColumnNames[indexCol]}` = {param}";
            else
                conditionParameterList[indexRow] += $" AND `{bulkDeleteValue.ColumnNames[indexCol]}` = {param}";
        }

        //Build up string of conditions
        //List of conditions
        conditionList += string.Join(") OR ", conditionParameterList).Replace("WHEN ", "(");
        conditionList = conditionList.Insert(conditionList.Length, ")");

        sCommand = new StringBuilder(BulkOperationConstant.BulkDeleteJoin.Replace("tbName", bulkDeleteValue.TableName)
            .Replace("[#Conditions]", conditionList));
        var query = sCommand.ToString();
        return query;
    }
}