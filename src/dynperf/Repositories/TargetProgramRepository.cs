using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using dynperf.Builders;
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

            InitializeFileWatcher();
        }

        public async Task<ReadOnlyCollection<TargetProcessEntry>> GetEntries()
        {
            if (TargetProcessEntries == null)
            {
                TargetProcessEntries = await LoadTargetList().ConfigureAwait(false);
            }

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

        private async void OnTargetsFileChange(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation("Targets file change detected, loading");
            TargetProcessEntries = await LoadTargetList().ConfigureAwait(false);
        }

        private async Task<List<TargetProcessEntry>> LoadTargetList()
        {
            var targetsFile = $"{ConfigurationFolder}/{TargetsFileName}";

            if (File.Exists(targetsFile))
            {
                using var stream = new FileStream(targetsFile, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4092, useAsync: true);
                var serializedInput = await JsonSerializer.DeserializeAsync<List<TargetProcessEntry>>(stream).ConfigureAwait(false);
                _logger.LogInformation($"targets file with {serializedInput.Count} targets loaded");
                return serializedInput;
            }

            await WriteDefaultTargets().ConfigureAwait(false);
            _logger.LogWarning("Writing defaults json");
            return new List<TargetProcessEntry>();
        }

        private async Task WriteDefaultTargets()
        {
            var targetsFile = $"{ConfigurationFolder}/{TargetsFileName}";
            var defaults = DefaultTargets.GetDefaults();

            using var stream = new FileStream(targetsFile, FileMode.CreateNew, FileAccess.Write, FileShare.Write, bufferSize: 4092, useAsync: true);
            await JsonSerializer.SerializeAsync(stream, defaults, new JsonSerializerOptions() { WriteIndented = true }).ConfigureAwait(false);
        }
    }
}