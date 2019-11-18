using System.IO;
using System.Text.Json;
using dynperf_server.Models;

namespace dynperf_server.Services
{
    public class ConfigurationService
    {
        public Configuration Configuration { get; private set; }
        private string ConfigFile { get; set; }

        public ConfigurationService()
        {
            ConfigFile = $"{Configuration.ConfigurationFolder}/config.json";
            Configuration = LoadConfiguration();
        }

        private Configuration LoadConfiguration()
        {
            if (File.Exists(ConfigFile))
            {
                return JsonSerializer.Deserialize<Configuration>(File.ReadAllText(ConfigFile));
            }
            else
            {
                WriteDefaultConfiguration();
            }

            return new Configuration();
        }

        private void WriteDefaultConfiguration()
        {
            Directory.CreateDirectory(Configuration.ConfigurationFolder);

            var defaultConfig = new Configuration();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                AllowTrailingCommas = true
            };

            var configJson = JsonSerializer.Serialize(defaultConfig, options);
            File.WriteAllText(ConfigFile, configJson);

            System.Console.WriteLine(configJson);
        }

    }
}