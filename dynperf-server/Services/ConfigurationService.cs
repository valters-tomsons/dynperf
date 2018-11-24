using System.IO;
using dynperf_server.Models;
using Newtonsoft.Json;

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
                return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(ConfigFile));
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

            using(var writer = File.CreateText(ConfigFile))
            {
                var serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, defaultConfig);
            }
        }

    }
}