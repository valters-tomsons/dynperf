using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using dynperf.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace dynperf
{
    public class DynperfWorker : BackgroundService
    {
        private readonly ILogger<DynperfWorker> _logger;
        private readonly IConfigurationRoot _config;
        private readonly TargetProcessMonitor _monitor;

        private bool PerformanceMode = false;

        public DynperfWorker(ILogger<DynperfWorker> logger, IConfigurationRoot config, TargetProcessMonitor monitor)
        {
            _logger = logger;
            _config = config;
            _monitor = monitor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var delay = _config.GetValue("ScanIntervalMs", 1500);

            while (!stoppingToken.IsCancellationRequested)
            {
                var proc = _monitor.RunningTargetCount();

                if(proc > 0 && !PerformanceMode)
                {
                    KillPicom();
                }
                else if (proc == 0 && PerformanceMode)
                {
                    TryStartPicom();
                }

                await Task.Delay(delay, stoppingToken).ConfigureAwait(false);
            }
        }

        private bool TryStartPicom()
        {
            _logger.LogInformation("Trying to starting picom");

            var proc = new Process() { StartInfo = new ProcessStartInfo("/usr/bin/picom") };

            PerformanceMode = false;
            return proc.Start();
        }

        private void KillPicom()
        {
            foreach (Process proc in Process.GetProcessesByName("picom"))
            {
                proc.Kill();
            }

            PerformanceMode = true;
        }
    }
}
