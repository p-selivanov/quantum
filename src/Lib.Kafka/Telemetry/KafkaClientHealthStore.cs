using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Quantum.Lib.Kafka.Telemetry;

/// <summary>
/// A singleton store for recording kafka client connection health.
/// </summary>
public static class KafkaClientHealthStore
{
    private static readonly ConcurrentDictionary<string, HealthStatus> ClientHealthStatuses = new();

    public static void ReportHealthy(string clientName)
    {
        ClientHealthStatuses[clientName] = HealthStatus.Healthy;
    }

    public static void ReportDegraded(string clientName)
    {
        if (!ClientHealthStatuses.ContainsKey(clientName) || ClientHealthStatuses[clientName] != HealthStatus.Unhealthy)
        {
            ClientHealthStatuses[clientName] = HealthStatus.Degraded;
        }
    }

    public static void ReportUnhealthy(string clientName)
    {
        ClientHealthStatuses[clientName] = HealthStatus.Unhealthy;
    }

    public static IReadOnlyDictionary<string, HealthStatus> GetClientHealthStatuses()
    {
        return new ReadOnlyDictionary<string, HealthStatus>(ClientHealthStatuses);
    }
}
