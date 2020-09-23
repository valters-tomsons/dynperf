using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using dynperf.Models;
using Microsoft.Extensions.Logging;

namespace dynperf.Repositories
{
    public class TargetProgramRepository
    {
        private List<TargetProcessEntry> TargetProcessEntries { get; set; }
        private FileSystemWatcher FileWatcher { get; set; }

        private readonly ILogger<DynperfWorker> _logger;

        private static readonly string ConfigurationFolder = $"{Environment.GetEnvironmentVariable("HOME")}/.local/share/dynperf";
        private const string TargetsFileName = "targets.json";

        public TargetProgramRepository(ILogger<DynperfWorker> logger)
        {
            _logger = logger;

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
                Path = ConfigurationFolder,
                Filter = TargetsFileName,
                NotifyFilter = NotifyFilters.LastWrite
            };

            FileWatcher.Changed += OnTargetsFileChange;
            FileWatcher.EnableRaisingEvents = true;
        }

        private void OnTargetsFileChange(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation("Targets file change detected, loading");
            TargetProcessEntries = LoadTargetList();
        }

        private List<TargetProcessEntry> LoadTargetList()
        {
            var targetsFile = $"{ConfigurationFolder}/{TargetsFileName}";

            if (File.Exists(targetsFile))
            {
                var targetInput = File.ReadAllText(targetsFile);
                var serializedInput = JsonSerializer.Deserialize<List<TargetProcessEntry>>(targetInput);

                _logger.LogInformation($"targets file with {serializedInput.Count} targets loaded");

                return serializedInput;
            }
            else
            {
                WriteDefaultTargets();
            }

            _logger.LogInformation("Default targets file loaded");
            return new List<TargetProcessEntry>();
        }

        private void WriteDefaultTargets()
        {
            var targetsFile = $"{ConfigurationFolder}/{TargetsFileName}";
            File.Copy("Defaults/targets.json", targetsFile);
        }
    }
}