using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using dynperf_server.Domain;
using dynperf_server.Models;

namespace dynperf_server.Services
{
    public class ProcessMonitor
    {
        private readonly TargetProgramRepository _targetPrograms;
        private ReadOnlyCollection<TargetProcessEntry> _targetCache;

        public ProcessMonitor(TargetProgramRepository targetPrograms)
        {
            _targetPrograms = targetPrograms;
        }

        public List<Process> GetAllRunningTargetPrograms()
        {
            _targetCache = _targetPrograms.GetEntries();

            var processes = Process.GetProcesses();
            var targetProcesses = processes.Where(x => FilterTarget(x)).ToList();

            return targetProcesses;
        }

        private bool FilterTarget(Process target)
        {
            if (_targetCache.Any(x => target.ProcessName.Equals(x.ProcessName)))
            {
                return true;
            }

            return false;
        }
    }
}