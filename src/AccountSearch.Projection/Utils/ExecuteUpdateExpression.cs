using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Quantum.AccountSearch.Projection.Utils;

internal static class ExecuteUpdateExpression
{
    public static Expression<Func<SetPropertyCalls<TSource>, SetPropertyCalls<TSource>>> Empty<TSource>()
    {
        return x => x;
    }

    public static bool IsEmpty<TSource>(this Expression<Func<SetPropertyCalls<TSource>, SetPropertyCalls<TSource>>> updateExpression)
    {
        return updateExpression.Body is ParameterExpression;
    }

    public static Expression<Func<SetPropertyCalls<TSource>, SetPropertyCalls<TSource>>> SetPropertyEx<TSource, TProperty>(
        this Expression<Func<SetPropertyCalls<TSource>, SetPropertyCalls<TSource>>> updateExpression,
        Expression<Func<TSource, TProperty>> propertyExpression,
        TProperty value)
    {
        var setPropertyCallsParam = updateExpression.Parameters.First();

        var setBody = Expression.Call(
            updateExpression.Body,
            "SetProperty",
            new[] { typeof(TProperty) },
            propertyExpression,
            Expression.Constant(value));

        return Expression.Lambda<Func<SetPropertyCalls<TSource>, SetPropertyCalls<TSource>>>(
            setBody,
            setPropertyCallsParam);
    }
}
