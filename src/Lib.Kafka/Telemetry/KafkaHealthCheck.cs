using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Quantum.Lib.Kafka.Telemetry;

internal class KafkaHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var clientStatuses = KafkaClientHealthStore.GetClientHealthStatuses();

        HealthCheckResult result;
        if (clientStatuses.Any())
        {
            var status = clientStatuses.Values.Min();
            string description = null;
            var data = clientStatuses.ToDictionary(x => x.Key, x => (object)x.Value);

            if (status != HealthStatus.Healthy)
            {
                var statusStrings = clientStatuses
                    .Where(x => x.Value != HealthStatus.Healthy)
                    .Select(x => $"{x.Key} is {x.Value}");

                description = string.Join("; ", statusStrings);
            }

            result = new HealthCheckResult(
                status: clientStatuses.Values.Min(),
                description: description,
                data: data);
        }
        else
        {
            result = new HealthCheckResult(HealthStatus.Healthy);
        }

        return Task.FromResult(result);
    }
}
