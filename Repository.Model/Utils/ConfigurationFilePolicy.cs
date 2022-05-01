namespace Repository.Model;

public class ConfigurationFilePolicy : IConfigurationFilePolicy
{
    public string Environment { get; set; } = string.Empty;
    public string Directory { get; set; } = string.Empty;

    public ConfigurationFilePolicy()
    {
        Directory = AppDomain.CurrentDomain.BaseDirectory;
        Environment = "Development";
    }

    public IEnumerable<string> Files
    {
        get
        {
            var configurationFiles = new List<string>();
            var candidateFiles = new List<string>()
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

            foreach(var candidateFile in candidateFiles)
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
