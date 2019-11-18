using System;

namespace dynperf_server.Models
{
    public class Configuration
    {
        public static string ConfigurationFolder = $"{Environment.GetEnvironmentVariable("HOME")}/.local/share/dynperf";

        public Configuration()
        {
            TargetListFilePath = $"{ConfigurationFolder}/targets.json";

            KillProcess = "picom";
            RestoreCommand = "picom";
            ScanIntervalMs = 1500;
        }

        public string TargetListFilePath { get; set; }
        public string KillProcess { get; set; }
        public string RestoreCommand { get; set; }
        public double ScanIntervalMs { get; set; }
    }
}