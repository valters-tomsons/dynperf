using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Linq;
using dynperf.Repositories;
using dynperf.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace dynperf
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0 && args.Contains("-kill"))
            {
                var procId = Process.GetCurrentProcess().Id;
                var processes = Process.GetProcessesByName("dynperf").Where(x => x.Id != procId).ToList();

                System.Console.WriteLine($"Killing {processes.Count} processes.");

                processes.ForEach(x => x.Kill());
                return;
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSystemd()
                .ConfigureServices((_, services) =>
                {
                    services.AddSingleton<TargetProgramRepository>();
                    services.AddSingleton<TargetProcessMonitor>();

                    services.AddHostedService<DynperfWorker>();
                });
    }
}