using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dynperf.Repositories;

namespace dynperf.Services
{
    public class TargetProcessMonitor
    {
        private readonly TargetProgramRepository _targetRepo;

        public TargetProcessMonitor(TargetProgramRepository targetRepo)
        {
            _targetRepo = targetRepo;
        }

        public int RunningTargetCount()
        {
            return MonitorProcesses().Count();
        }

        private IEnumerable<string> MonitorProcesses()
        {
            var processes = Process.GetProcesses();
            var targetProcesses = processes.Where(FilterTarget);
            return targetProcesses.Select(x => x.ProcessName);
        }

        private bool FilterTarget(Process target)
        {
            var targets = _targetRepo.GetEntries();
            return targets.Any(x => target.ProcessName.Equals(x.ProcessName));
        }
    }
}