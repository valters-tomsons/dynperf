using System;

namespace dynperf_server.Models
{
    public class Configuration
    {
        public static string ConfigurationFolder = $"{Environment.GetEnvironmentVariable("HOME")}/.local/share/dynperf";

        public Configuration()
        {
            TargetListFilePath = $"{ConfigurationFolder}/targets.json";

            KillProcess = "compton";
            RestoreCommand = "compton --config ~/.config/compton.conf";
            ScanIntervalMs = 1500;
            PrintStatusMessages = false;
        }

        public string TargetListFilePath { get; set; }

        public string KillProcess { get; set; }
        public string RestoreCommand { get; set; }
        public double ScanIntervalMs { get; set; }
        public bool PrintStatusMessages { get; set; }
    }
}