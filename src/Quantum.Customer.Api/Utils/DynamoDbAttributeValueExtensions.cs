using System;
using Amazon.DynamoDBv2.Model;

namespace Quantum.Customer.Api.Utils;

public static class DynamoDbAttributeValueExtensions
{
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
