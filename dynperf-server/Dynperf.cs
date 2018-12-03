using dynperf_server.Domain;
using dynperf_server.Services;

namespace dynperf_server
{
    public class Dynperf
    {
        public Dynperf()
        {
            //Build Daemon Service dependencies
            var configuration = new ConfigurationService().Configuration;
            var targetRepository = new TargetProgramRepository(configuration);
            var processMonitor = new ProcessMonitor(targetRepository);

            new DaemonService(configuration, processMonitor).Start();
        }
    }
}