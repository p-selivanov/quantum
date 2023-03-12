using Amazon.Lambda.DynamoDBEvents;

namespace Quantum.Customer.StreamConsumerLambda.Utils;

internal static class DynamoDbStreamRecordExtensions
{
    public static bool IsAttributeChanged(this DynamoDBEvent.DynamodbStreamRecord record, string attributeName)
    {
        var oldValue = record.Dynamodb.OldImage[attributeName];
        var newValue = record.Dynamodb.NewImage[attributeName];

        return
            newValue.S != oldValue.S ||
            newValue.N != oldValue.N;
    }
}
