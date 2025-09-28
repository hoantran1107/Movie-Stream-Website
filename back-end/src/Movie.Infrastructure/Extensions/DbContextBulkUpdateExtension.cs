using System.Text;
using GenericRepository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Movie.Infrastructure.Constants;
using Movie.Infrastructure.Exceptions;
using Npgsql;

namespace Movie.Infrastructure.Extensions;

public static partial class DbContextExtension
{
    /// <summary>
    ///     Only tables with primary keys will be updated.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="bulkUpdateValue">If defined, only these columns will be updated.</param>
    /// <param name="timeout"></param>
    public static void ExecuteBulkUpdateByIdentityKey(
        this DbContext dbContext, BulkUpdateByIdentityKeyModel bulkUpdateValue, int timeout = 60)
    {
        if (bulkUpdateValue == null || !bulkUpdateValue.SqlParameterColumnMappings.Any() ||
            !bulkUpdateValue.UpdateValues.Any()
            || bulkUpdateValue.SqlParameterKeyColumnMappings == null || bulkUpdateValue.KeyValues == null)
            throw new BulkUpdateException("Invalid bulkUpdateValue model");

        if (bulkUpdateValue.SqlParameterColumnMappings.Count != bulkUpdateValue.UpdateValues.FirstOrDefault()?.Length)
            throw new BulkUpdateException(
                "Invalid bulkUpdateValue model: mismatch columns-size between definition and values");

        // compare key values with one primary key.
        if (bulkUpdateValue.KeyValues.FirstOrDefault()?.Length != 1)
            throw new BulkUpdateException(
                "Invalid bulkUpdateValue model: mismatch columns-size between key definition and key values");

        try
        {
            using NpgsqlCommand command = new(null, (NpgsqlConnection)dbContext.Database.GetDbConnection());
            command.CommandText = PrepareBulkUpdateByIdentityKey(bulkUpdateValue);
            command.CommandTimeout = timeout;
            for (var indexRow = 0; indexRow < bulkUpdateValue.UpdateValues.Count; indexRow++)
            for (var indexCol = 0; indexCol < bulkUpdateValue.SqlParameterColumnMappings.Count; indexCol++)
            {
                var parameter = command.CreateParameter();
                var currentIndexParam = indexRow * bulkUpdateValue.SqlParameterColumnMappings.Count + indexCol;
                parameter.ParameterName = $"@p{currentIndexParam}";
                parameter.NpgsqlDbType = bulkUpdateValue.SqlParameterColumnMappings[indexCol].NpgsqlDbType;
                parameter.Value = bulkUpdateValue.UpdateValues[indexRow][indexCol] ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }

            for (var index = 0; index < bulkUpdateValue.KeyValues.Count; index++)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = $"@kp{index}";
                parameter.NpgsqlDbType = bulkUpdateValue.SqlParameterKeyColumnMappings.NpgsqlDbType;
                parameter.Value = bulkUpdateValue.KeyValues[index][0];
                command.Parameters.Add(parameter);
            }

            var trans = dbContext.Database.CurrentTransaction?.GetDbTransaction();

            if (trans != null) command.Transaction = (NpgsqlTransaction?)trans;
            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw new BulkUpdateException(e, "Bulk update by identity key error");
        }
    }

    /// <summary>
    ///     Only tables with composite keys will be updated.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="bulkUpdateValue">If defined, only these columns will be updated.</param>
    /// <param name="timeout"></param>
    public static void ExecuteBulkUpdateByCompositeKey(
        this DbContext dbContext,
        BulkUpdateByCompositeKeyModel bulkUpdateValue, int timeout = 60)
    {
        if (bulkUpdateValue == null || !bulkUpdateValue.SqlParameterColumnMappings.Any() ||
            !bulkUpdateValue.UpdateValues.Any()
            || !bulkUpdateValue.SqlParameterKeyColumnMappings.Any() || !bulkUpdateValue.KeyValues.Any())
            throw new BulkUpdateException("Invalid bulkUpdateValue model");

        if (bulkUpdateValue.SqlParameterColumnMappings.Count != bulkUpdateValue.UpdateValues.FirstOrDefault()?.Length)
            throw new BulkUpdateException(
                "Invalid bulkUpdateValue model: mismatch columns-size between definition and values");

        if (bulkUpdateValue.UpdateValues.Count != bulkUpdateValue.KeyValues.Count)
            throw new BulkUpdateException(
                "Invalid bulkUpdateValue model: mismatch columns-size between values and key values");

        if (bulkUpdateValue.SqlParameterKeyColumnMappings.Count != bulkUpdateValue.KeyValues.FirstOrDefault()?.Length)
            throw new BulkUpdateException(
                "Invalid bulkUpdateValue model: mismatch columns-size between key definition and key values");

        try
        {
            using NpgsqlCommand command = new(null, (NpgsqlConnection)dbContext.Database.GetDbConnection());
            command.CommandText = PrepareBulkUpdateByCompositeKey(bulkUpdateValue);
            command.CommandTimeout = timeout;
            for (var indexRow = 0; indexRow < bulkUpdateValue.UpdateValues.Count; indexRow++)
            for (var indexCol = 0; indexCol < bulkUpdateValue.SqlParameterColumnMappings.Count; indexCol++)
            {
                var parameter = command.CreateParameter();
                var currentIndexParam = indexRow * bulkUpdateValue.SqlParameterColumnMappings.Count + indexCol;
                parameter.ParameterName = $"@p{currentIndexParam}";
                parameter.NpgsqlDbType = bulkUpdateValue.SqlParameterColumnMappings[indexCol].NpgsqlDbType;
                parameter.Value = bulkUpdateValue.UpdateValues[indexRow][indexCol] ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }

            for (var indexRow = 0; indexRow < bulkUpdateValue.KeyValues.Count; indexRow++)
            for (var indexCol = 0; indexCol < bulkUpdateValue.SqlParameterKeyColumnMappings.Count; indexCol++)
            {
                var parameter = command.CreateParameter();
                var currentIndexParam = indexRow * bulkUpdateValue.SqlParameterKeyColumnMappings.Count + indexCol;
                parameter.ParameterName = $"@kp{currentIndexParam}";
                parameter.NpgsqlDbType = bulkUpdateValue.SqlParameterKeyColumnMappings[indexCol].NpgsqlDbType;
                parameter.Value = bulkUpdateValue.KeyValues[indexRow][indexCol];
                command.Parameters.Add(parameter);
            }

            var trans = dbContext.Database.CurrentTransaction?.GetDbTransaction();

            if (trans != null) command.Transaction = (NpgsqlTransaction?)trans;
            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw new BulkUpdateException(e, "Bulk update by composite key error");
        }
    }


    /// <summary>
    ///     Generate bulk update command
    /// </summary>
    /// <param name="bulkUpdateValue"></param>
    /// <returns></returns>
    private static string PrepareBulkUpdateByCompositeKey(BulkUpdateByCompositeKeyModel bulkUpdateValue)
    {
        StringBuilder sCommand;

        //Build up string of parameter conditions
        //List of parameter conditions
        var columnsKeySize = bulkUpdateValue.SqlParameterKeyColumnMappings.Count;
        var conditionList = string.Empty;
        var conditionParameterList = new string[bulkUpdateValue.KeyValues.Count];

        for (var indexRow = 0; indexRow < bulkUpdateValue.KeyValues.Count; indexRow++)
        for (var indexCol = 0; indexCol < bulkUpdateValue.SqlParameterKeyColumnMappings.Count; indexCol++)
        {
            var param = $"@kp{indexRow * columnsKeySize + indexCol}";
            if (indexCol == 0)
                conditionParameterList[indexRow] =
                    $"WHEN {bulkUpdateValue.SqlParameterKeyColumnMappings[indexCol].ColumnName} = {param}";
            else
                conditionParameterList[indexRow] +=
                    $" AND {bulkUpdateValue.SqlParameterKeyColumnMappings[indexCol].ColumnName} = {param}";
        }

        //Build up string of parameter
        //List of parameter
        var columnsSize = bulkUpdateValue.SqlParameterColumnMappings.Count;
        var parameterList = new StringBuilder();
        for (var indexRow = 0; indexRow < bulkUpdateValue.SqlParameterColumnMappings.Count; indexRow++)
        {
            if (indexRow == 0)
                parameterList.Append($"{bulkUpdateValue.SqlParameterColumnMappings[indexRow].ColumnName} = (CASE");
            else
                parameterList.Append(
                    $", {bulkUpdateValue.SqlParameterColumnMappings[indexRow].ColumnName} = (CASE");

            for (var indexCol = 0; indexCol < bulkUpdateValue.UpdateValues.Count; indexCol++)
                parameterList.Append(
                    $" {conditionParameterList[indexCol]} THEN @p{indexCol * columnsSize + indexRow}");

            parameterList.Append(" END)");
        }

        //Build up string of conditions
        //List of conditions
        // Example: WHEN `claimStatusId` = @kp0 AND `abonnement` = @kp1 
        // WHEN `claimStatusId` = @kp2 AND `abonnement` = @kp3
        // -> (`claimStatusId` = @kp0 AND `abonnement` = @kp1) OR (`claimStatusId` = @kp2 AND `abonnement` = @kp3
        conditionList += string.Join(") OR ", conditionParameterList).Replace("WHEN ", "(");
        // (`claimStatusId` = @kp0 AND `abonnement` = @kp1) OR (`claimStatusId` = @kp2 AND `abonnement` = @kp3)
        conditionList = conditionList.Insert(conditionList.Length, ")");

        sCommand = new StringBuilder(BulkOperationConstant.BulkUpdateJoin.Replace("tbName", bulkUpdateValue.TableName)
            .Replace("[#UpdateStatements]", parameterList.ToString())
            .Replace("[#Conditions]", conditionList));
        var query = sCommand.ToString();
        return query;
    }

    /// <summary>
    ///     Generate bulk update command
    /// </summary>
    /// <param name="bulkUpdateValue"></param>
    /// <returns></returns>
    private static string PrepareBulkUpdateByIdentityKey(BulkUpdateByIdentityKeyModel bulkUpdateValue)
    {
        //Build up string of parameters conditions
        //List of parameter conditions
        var conditionList = $"{bulkUpdateValue.SqlParameterKeyColumnMappings.ColumnName} IN (";
        var conditionParameterList = new string[bulkUpdateValue.KeyValues.Count];

        for (var index = 0; index < bulkUpdateValue.KeyValues.Count; index++)
        {
            var param = $"@kp{index}";
            conditionParameterList[index] =
                $"WHEN {bulkUpdateValue.SqlParameterKeyColumnMappings.ColumnName} = {param}";
            if (index == 0)
                conditionList += $"{param}";
            else
                conditionList += $", {param}";

            if (index == bulkUpdateValue.KeyValues.Count - 1)
                conditionList += ")";
        }

        //Build up string of parameters
        //List of parameter
        var columnsSize = bulkUpdateValue.SqlParameterColumnMappings.Count;
        var parameterList = string.Empty;
        for (var indexRow = 0; indexRow < bulkUpdateValue.SqlParameterColumnMappings.Count; indexRow++)
        {
            if (indexRow == 0)
                parameterList += $"{bulkUpdateValue.SqlParameterColumnMappings[indexRow].ColumnName} = (CASE";
            else
                parameterList += $", {bulkUpdateValue.SqlParameterColumnMappings[indexRow].ColumnName} = (CASE";

            for (var indexCol = 0; indexCol < bulkUpdateValue.UpdateValues.Count; indexCol++)
                parameterList += $" {conditionParameterList[indexCol]} THEN @p{indexCol * columnsSize + indexRow}";
            parameterList += " END)";
        }

        var sCommand = new StringBuilder(BulkOperationConstant.BulkUpdateJoin
            .Replace("tbName", bulkUpdateValue.TableName)
            .Replace("[#UpdateStatements]", parameterList)
            .Replace("[#Conditions]", conditionList));
        var query = sCommand.ToString();
        return query;
    }
}