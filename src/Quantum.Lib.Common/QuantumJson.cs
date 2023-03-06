using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Quantum.Lib.Common;

public static class QuantumJson
{
    public static readonly JsonSerializerSettings DefaultSettings = new JsonSerializerSettings
    {
        Converters =
        {
            new StringEnumConverter(),
            new SpecifiableConverter(),
        },
        ContractResolver = new JsonContractResolver(),
    };

    public static readonly JsonSerializer DefaultSerializer = JsonSerializer.Create(DefaultSettings);

    /// <summary>
    /// Applies the default Wafer Json settings to the provided settings instance.
    /// </summary>
    public static void ApplyDefaultSettings(JsonSerializerSettings settings)
    {
        settings.Converters.Clear();
        foreach (var converter in DefaultSettings.Converters)
        {
            settings.Converters.Add(converter);
        }

        settings.ContractResolver = DefaultSettings.ContractResolver;
    }
}
