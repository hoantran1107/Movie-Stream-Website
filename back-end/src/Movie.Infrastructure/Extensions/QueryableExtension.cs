using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Movie.Infrastructure.Extensions;

public static class IQueryableExtensions
{
    public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
    {
        var enumerator = query.Provider.Execute<IEnumerable<TEntity>>(query.Expression).GetEnumerator();
        var relationalCommandCache = enumerator.Private("_relationalCommandCache");
        var selectExpression = relationalCommandCache.Private<SelectExpression>("_selectExpression");
        var factory = relationalCommandCache.Private<IQuerySqlGeneratorFactory>("_querySqlGeneratorFactory");
        var sqlGenerator = factory.Create();
        var command = sqlGenerator.GetCommand(selectExpression);

        var sql = command.CommandText;
        return sql;
    }

    public static IReadOnlyList<ProjectionExpression> GetProjections<TEntity>(this IQueryable<TEntity> query)
        where TEntity : class
    {
        var enumerator = query.Provider.Execute<IEnumerable<TEntity>>(query.Expression).GetEnumerator();
        var relationalCommandCache = enumerator.Private("_relationalCommandCache");
        var selectExpression = relationalCommandCache.Private<SelectExpression>("_selectExpression");
        if (selectExpression.Projection.Any()) return selectExpression.Projection;

        return null;
    }

    private static object Private(this object obj, string privateField)
    {
        return obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
    }

    private static T Private<T>(this object obj, string privateField)
    {
        return (T)obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
    }
}