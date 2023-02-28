using System;
using Amazon.DynamoDBv2.Model;

namespace Quantum.Lib.DynamoDb;

public static class AttributeValueFactory
{
    public static AttributeValue FromString(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return new AttributeValue
            {
                NULL = true,
            };
        }

        return new AttributeValue(value);
    }

    public static AttributeValue FromInt(int value)
    {
        return new AttributeValue
        {
            N = value.ToString(),
        };
    }

    public static AttributeValue FromDecimal(decimal value)
    {
        return new AttributeValue
        {
            N = value.ToString(),
        };
    }

    public static AttributeValue FromEnum<TEnum>(TEnum value)
        where TEnum : Enum
    {
        return FromString(value.ToString());
    }

    public static AttributeValue FromTimestamp(DateTime timestamp)
    {
        var unixTime = ((DateTimeOffset)timestamp).ToUnixTimeMilliseconds();
        return new AttributeValue
        {
            N = unixTime.ToString(),
        };
    }
}
