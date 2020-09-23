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