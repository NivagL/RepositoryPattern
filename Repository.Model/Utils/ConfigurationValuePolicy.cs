using Microsoft.Extensions.Configuration;

namespace Repository.Model;

public class ConfigurationValuePolicy : IConfigurationValuePolicy
{
    private readonly IConfiguration Configuration;

    public ConfigurationValuePolicy(
        IConfiguration configuration
        )
    {
        Configuration = configuration;
    }

    public T GetValue<T>(string configPath, string section, string name, T defaultValue)
    {
        var value = default(T);

        if (!string.IsNullOrEmpty(configPath))
        {
            value = Configuration.GetValue<T>($"{configPath}:{section}:{name}");
            if (value != null && !value.Equals(default(T)))
                return value;

            value = Configuration.GetValue<T>($"{configPath}:{name}");
            if (value != null && !value.Equals(default(T)))
                return value;

            return defaultValue;
        }

        if (!string.IsNullOrEmpty(section))
        {
            value = Configuration.GetValue<T>($"{section}:{name}");
            if (value != null && !value.Equals(default(T)))
                return value;

            return defaultValue;
        }

        value = Configuration.GetValue<T>($"{name}");
        if (value != null && !value.Equals(default(T)))
            return value;

        value = defaultValue;
        return value;
    }
}
