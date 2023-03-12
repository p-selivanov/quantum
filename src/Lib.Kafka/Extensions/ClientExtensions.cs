using Confluent.Kafka;

namespace Quantum.Lib.Kafka.Extensions;

public static class ClientExtensions
{
    /// <summary>
    /// Parses Kafka client connection ID from the client name.
    /// </summary>
    public static int GetClientId(this IClient client)
    {
        if (string.IsNullOrEmpty(client?.Name))
        {
            return 0;
        }

        var index = client.Name.LastIndexOf('-');
        if (index < 0)
        {
            return 0;
        }

        var idString = client.Name.Substring(index + 1);

        if (!int.TryParse(idString, out var id))
        {
            return 0;
        }

        return id;
    }
}
