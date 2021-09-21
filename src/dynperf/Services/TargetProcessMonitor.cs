using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using dynperf.Repositories;
using dynperf.Models;

namespace dynperf.Services
{
    public class TargetProcessMonitor
    {
        private readonly TargetProgramRepository _targetRepo;
        private ReadOnlyCollection<TargetProcessEntry> Targets { get; set; }

        public TargetProcessMonitor(TargetProgramRepository targetRepo)
        {
            _targetRepo = targetRepo;
        }

        public async Task<IEnumerable<string>> GetRunningTargets()
        {
            Targets = await _targetRepo.GetEntries().ConfigureAwait(false);
            return MonitorProcesses();
        }

        private IEnumerable<string> MonitorProcesses()
        {
            var processes = Process.GetProcesses();
            var targetProcesses = processes.Where(FilterTarget);
            return targetProcesses.Select(x => x.ProcessName);
        }

        private bool FilterTarget(Process target)
        {
            return Targets.Any(x => target.ProcessName.Equals(x.ProcessName));
        }
    }
}