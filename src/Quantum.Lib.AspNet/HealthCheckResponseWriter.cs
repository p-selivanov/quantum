using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace Quantum.Lib.AspNet
{
    public static class HealthCheckResponseWriter
    {
        private static readonly string Version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        public static Task WriteResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            using var stringWriter = new StringWriter();
            using var jsonWriter = new JsonTextWriter(stringWriter);

            jsonWriter.WriteStartObject();

            jsonWriter.WritePropertyName("status");
            jsonWriter.WriteValue(result.Status.ToString());

            jsonWriter.WritePropertyName("version");
            jsonWriter.WriteValue("v" + Version);

            jsonWriter.WriteEndObject();

            return context.Response.WriteAsync(stringWriter.ToString());
        }
    }
}
