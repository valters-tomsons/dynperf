using System.Threading;
using dynperf_server.Domain;
using dynperf_server.Models;
using dynperf_server.Services;

namespace dynperf_server
{
    class Program
    {
        static ManualResetEvent _quitEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            System.Console.CancelKeyPress += (sender, e) =>
            {
                _quitEvent.Set();
                e.Cancel = true;
            };

            new Dynperf();
            System.Console.WriteLine("Dynperf Daemon service running!");

            _quitEvent.WaitOne();

            System.Console.WriteLine();
            System.Console.WriteLine("Dynperf service stopped.");
        }
    }
}