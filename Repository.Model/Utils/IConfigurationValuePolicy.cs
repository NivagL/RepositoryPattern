namespace Repository.Model;

public interface IConfigurationValuePolicy
{
    T GetValue<T>(string configPath, string section, string name, T defaultValue);
}
