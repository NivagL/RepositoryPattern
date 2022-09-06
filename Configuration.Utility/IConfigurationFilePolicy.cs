namespace Configuration.Utility;

public interface IConfigurationFilePolicy
{
    IEnumerable<string> Files { get; }
}
