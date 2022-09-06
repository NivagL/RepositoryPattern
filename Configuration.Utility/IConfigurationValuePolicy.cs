namespace Configuration.Utility;

public interface IConfigurationValuePolicy
{
    T GetValue<T>(string configPath, string section, string name, T defaultValue);
}
