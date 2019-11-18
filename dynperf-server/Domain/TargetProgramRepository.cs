using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using dynperf_server.Models;

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
            FileWatcher = new FileSystemWatcher
            {
                Path = Configuration.ConfigurationFolder,
                Filter = "targets.json",
                NotifyFilter = NotifyFilters.LastWrite
            };

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
                var targetInput = File.ReadAllText(targetsFile);
                var serializedInput = JsonSerializer.Deserialize<List<TargetProcessEntry>>(targetInput);
                Console.WriteLine($"targets file with {serializedInput.Count} targets loaded");
                return serializedInput;
            }
            else
            {
                WriteDefaultTargets();
            }

            Console.WriteLine("Default targets file loaded");
            return new List<TargetProcessEntry>();
        }

        private void WriteDefaultTargets()
        {
            File.Copy("Defaults/targets.json", _configuration.TargetListFilePath);
        }
    }
}