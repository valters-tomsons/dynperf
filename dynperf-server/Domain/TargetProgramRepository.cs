using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using dynperf_server.Models;
using Newtonsoft.Json;

namespace dynperf_server.Domain
{
    public class TargetProgramRepository
    {
        private readonly Configuration _configuration;
        private List<TargetProcessEntry> TargetProcessEntries { get; set; }

        public TargetProgramRepository(Configuration configuration)
        {
            _configuration = configuration;
            TargetProcessEntries = LoadTargetList(); 
        }

        public void Add(TargetProcessEntry entry)
        {
            TargetProcessEntries.Add(entry);
        }

        public ReadOnlyCollection<TargetProcessEntry> GetEntries()
        {
            return TargetProcessEntries.AsReadOnly();
        }

        private List<TargetProcessEntry> LoadTargetList()
        {
            var targetsFile = _configuration.TargetListFilePath;

            if(File.Exists(_configuration.TargetListFilePath))
            {
                return JsonConvert.DeserializeObject<List<TargetProcessEntry>>(File.ReadAllText(targetsFile));
            }
            else
            {
                WriteDefaultTargets();
            }

            return new List<TargetProcessEntry>();
        }

        private void WriteDefaultTargets()
        {
            using(var writer = File.CreateText(_configuration.TargetListFilePath))
            {
                var serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, DefaultTargetDefinition.DefaultsList);
            }
        }
    }
}