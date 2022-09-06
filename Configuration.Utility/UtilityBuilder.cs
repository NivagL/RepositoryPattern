using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Configuration.Utility;

public class UtilityBuilder
{
    public string ConfigurationFolder { get; set; } = string.Empty;
    public IServiceCollection Services { get; } = new ServiceCollection();

    public UtilityBuilder()
    {
        //Order is very important here

        Services.AddSingleton<IConfigurationFilePolicy>(_ =>
        {
            var filePolicy = new ConfigurationFilePolicy(ConfigurationFilePolicyEnum.CommonFiles);
            filePolicy.Directory = $"{filePolicy.Directory}{ConfigurationFolder}\\";
            return filePolicy;
        });

        Services.AddSingleton<IConfiguration>(_ => {
            return Configuration;
        });

        Services.AddLogging(builder => builder.AddConsole().AddDebug());

        Services.AddSingleton<IConfigurationValuePolicy>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            return new ConfigurationValuePolicy(configuration);
        });
    }

    public IServiceProvider Provider 
    { 
        get
        {
            return Services.BuildServiceProvider();
        }
    }

    public ILogger<T> Logger<T>()
    {
        var provider = Services.BuildServiceProvider();
        var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
        return loggerFactory.CreateLogger<T>();
    }

    public IConfiguration Configuration
    {
        get
        {
            var configFiles = Provider.GetRequiredService<IConfigurationFilePolicy>();  

            var builder = new ConfigurationBuilder();

            builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
            builder.AddEnvironmentVariables();

            foreach (var configurationFile in configFiles.Files)
                builder.AddJsonFile(configurationFile, optional: false);

            return builder.Build();
        }
    }
}
