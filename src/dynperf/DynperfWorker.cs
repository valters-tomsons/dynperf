using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dynperf.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace dynperf
{
    public class DynperfWorker : BackgroundService
    {
        private readonly ILogger<DynperfWorker> _logger;
        private readonly TargetProcessMonitor _monitor;

        private bool PerformanceMode;

        public DynperfWorker(ILogger<DynperfWorker> logger, TargetProcessMonitor monitor)
        {
            _logger = logger;
            _monitor = monitor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            const int delay = 1500;

            TryStartPicom();

            while (!stoppingToken.IsCancellationRequested)
            {
                var procList = await _monitor.GetRunningTargets().ConfigureAwait(false);
                var procCount = procList.Count();
                PrintProcesses(procList, procCount);

                if(procCount > 0 && !PerformanceMode)
                {
                    KillPicom();
                }
                else if (procCount == 0 && PerformanceMode)
                {
                    TryStartPicom();
                }

                await Task.Delay(delay, stoppingToken).ConfigureAwait(false);
            }
        }

        private void PrintProcesses(IEnumerable<string> processes, int length)
        {
            if(length == 0)
            {
                return;
            }

            var sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(processes.ElementAt(i));
                if(i != length - 1)
                {
                    sb.Append(", ");
                }
            }

            _logger.LogInformation($"Targets found: [{sb}]");
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
            _logger.LogInformation("Killing picom");

            foreach (Process proc in Process.GetProcessesByName("picom"))
            {
                proc.Kill();
            }

            PerformanceMode = true;
        }
    }
}
