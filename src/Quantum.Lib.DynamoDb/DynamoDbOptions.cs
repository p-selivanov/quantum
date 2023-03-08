namespace Quantum.Lib.DynamoDb;

public class DynamoDbOptions
{
    public const string DefaultLocalStackUri = "http://localhost:4566";

    /// <summary>
    /// Gets or sets AWS region.<br/>
    /// If you provide region 'localstack' - the client will point to Localstack DynamoDb using 'LocalstackUri'.
    /// </summary>
    public string Region { get; set; }

    /// <summary>
    /// Gets or sets LocalStack URI. Default: 'http://localhost:4566'.
    /// </summary>
    public string LocalStackUri { get; set; }
}
