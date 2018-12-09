using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dynperf_server.Domain;

namespace dynperf_server.Services
{
    public class ProcessMonitor
    {
        private readonly TargetProgramRepository _targetPrograms;

        public ProcessMonitor(TargetProgramRepository targetPrograms)
        {
            _targetPrograms = targetPrograms;
        }

        public List<Process> GetAllRunningTargetPrograms()
        {
            var processes = Process.GetProcesses();
            var targetProcesses = processes.Where(x => FilterTarget(x)).ToList();
            return targetProcesses;
        }

        private bool FilterTarget(Process target)
        {
            if (_targetPrograms.GetEntries().Any(x => target.ProcessName.Contains(x.ProcessName)))
            {
                return true;
            }

            return false;
        }
    }
}