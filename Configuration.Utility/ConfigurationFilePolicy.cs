namespace Configuration.Utility;

public class ConfigurationFilePolicy : IConfigurationFilePolicy
{
    private readonly ConfigurationFilePolicyEnum FilePolicy;
    public string Environment { get; set; } = string.Empty;
    public string Directory { get; set; } = string.Empty;
    public string Pattern { get; set; } = string.Empty;

    public ConfigurationFilePolicy(ConfigurationFilePolicyEnum filePolicy)
    {
        FilePolicy = filePolicy;
        Directory = AppDomain.CurrentDomain.BaseDirectory;
        Environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (string.IsNullOrEmpty(Environment))
            Environment = "Development";
    }

    private IEnumerable<string> CandidateFiles
    {
        get
        {
            var candidateFiles = new List<string>();
            switch (FilePolicy)
            {
                case (ConfigurationFilePolicyEnum.Common):
                    return new List<string>()
                    {
                        $"{Directory}settings.json",
                        $"{Directory}settings.{Environment}.json",
                        $"{Directory}appsettings.json",
                        $"{Directory}appsettings.{Environment}.json",
                        $"{Directory}Configuration\\settings.json",
                        $"{Directory}Configuration\\settings.{Environment}.json",
                        $"{Directory}Configuration\\appsettings.json",
                        $"{Directory}Configuration\\appsettings.{Environment}.json",
                    };
            }
            return candidateFiles;
        }
    }

    public IEnumerable<string> Files
    {
        get
        {
            var configurationFiles = new List<string>();
            foreach (var candidateFile in CandidateFiles)
            {
                if (File.Exists(candidateFile))
                {
                    configurationFiles.Add(candidateFile);
                }
            }
            return configurationFiles;
        }
    }
}
