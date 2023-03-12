using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Quantum.Lib.Kafka.Telemetry;

public static class HealthChecksBuilderExtensions
{
    /// <summary>
    /// Adds Kafka connection health check.
    /// </summary>
    public static IHealthChecksBuilder AddKafka(
        this IHealthChecksBuilder builder,
        string name = "kafka",
        HealthStatus? failureStatus = null,
        IEnumerable<string> tags = null)
    {
        return builder.AddCheck<KafkaHealthCheck>(name, failureStatus, tags);
    }
}
