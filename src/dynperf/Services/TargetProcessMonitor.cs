using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace dynperf.Services
{
    public class TargetProcessMonitor
    {
        public int RunningTargetCount()
        {
            return MonitorProcesses().Count();
        }

        private IEnumerable<string> MonitorProcesses()
        {
            var processes = Process.GetProcesses();
            return processes.Where(x => x.ProcessName.Equals("vkcube")).Select(x => x.ProcessName);
        }
    }
}