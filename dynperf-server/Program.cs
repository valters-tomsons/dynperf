using dynperf_server.Domain;
using dynperf_server.Models;
using dynperf_server.Services;

namespace dynperf_server
{
    class Program
    {
        static void Main(string[] args)
        {
            //Build Dependencies
            var configService = new ConfigurationService();
            var configuration = configService.Configuration;
            var targetRepository = new TargetProgramRepository(configuration);
            var processMonitor = new ProcessMonitor(targetRepository);

            new DaemonService(configuration, processMonitor).Start();

            System.Console.ReadLine();
        }
    }
}