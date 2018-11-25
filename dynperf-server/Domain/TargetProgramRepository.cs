using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using dynperf_server.Models;
using Newtonsoft.Json;

namespace dynperf_server.Domain
{
    public class TargetProgramRepository
    {
        private readonly Configuration _configuration;
        private List<TargetProcessEntry> TargetProcessEntries { get; set; }
        private FileSystemWatcher FileWatcher { get; set; }

        public TargetProgramRepository(Configuration configuration)
        {
            _configuration = configuration;
            TargetProcessEntries = LoadTargetList();
            InitializeFileWatcher();
        }

        public ReadOnlyCollection<TargetProcessEntry> GetEntries()
        {
            return TargetProcessEntries.AsReadOnly();
        }

        private void InitializeFileWatcher()
        {
            FileWatcher = new FileSystemWatcher();
            FileWatcher.Path = Configuration.ConfigurationFolder;
            FileWatcher.Filter = "targets.json";
            FileWatcher.NotifyFilter = NotifyFilters.LastWrite;

            FileWatcher.Changed += OnTargetsFileChange;
            FileWatcher.EnableRaisingEvents = true;
        }

        private void OnTargetsFileChange(object sender, FileSystemEventArgs e)
        {
            System.Console.WriteLine("Targets file change detected, loading");
            var newList = LoadTargetList();
            TargetProcessEntries = newList;
        }

        private List<TargetProcessEntry> LoadTargetList()
        {
            var targetsFile = _configuration.TargetListFilePath;

            if (File.Exists(_configuration.TargetListFilePath))
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