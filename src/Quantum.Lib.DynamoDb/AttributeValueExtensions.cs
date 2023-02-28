using System;
using Amazon.DynamoDBv2.Model;

namespace Quantum.Lib.DynamoDb;

public static class AttributeValueExtensions
{
    public static string AsString(this AttributeValue value)
    {
        return value.S;
    }

    public static decimal AsDecimal(this AttributeValue value)
    {
        return decimal.Parse(value.N);
    }

    public static TEnum AsEnum<TEnum>(this AttributeValue value)
        where TEnum : struct
    {
        return Enum.Parse<TEnum>(value.S);
    }

    public static DateTime AsTimestamp(this AttributeValue value)
    {
        var timestamp = long.Parse(value.N);
        var offset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
        return offset.UtcDateTime;
    }
}