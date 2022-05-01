namespace Repository.Abstraction;

public class RepositoryExceptionConfiguration : RepositoryException
{
    public string ConfigurationPropertyName { get; set; }

    public RepositoryExceptionConfiguration(string configurationPropertyName)
        : base("Missing configuration parameter", "Missing configuration parameter")
    {
        ConfigurationPropertyName = configurationPropertyName;
    }

    public RepositoryExceptionConfiguration(string configurationPropertyName, Exception inner)
        : base("Missing configuration parameter", "Missing configuration parameter", inner)
    {
        ConfigurationPropertyName = configurationPropertyName;
    }
}
