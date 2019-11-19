using System;
using System.Diagnostics;
using System.Timers;
using dynperf_server.Models;

namespace dynperf_server.Services
{
    public class DaemonService
    {
        private readonly Configuration _configuration;
        private readonly ProcessMonitor _processMonitor;
        private bool isKilled = false;
        private System.Timers.Timer ServiceTimer;

        public DaemonService(Configuration configuration, ProcessMonitor processMonitor)
        {
            _configuration = configuration;
            _processMonitor = processMonitor;

            InitializeTimer();
        }

        public void Start()
        {
            RestoreProcess();
            ServiceTimer.Start();
        }

        private void InitializeTimer()
        {
            ServiceTimer = new System.Timers.Timer
            {
                Interval = _configuration.ScanIntervalMs
            };

            ServiceTimer.Elapsed += ServiceTimerTick;
        }

        private void ServiceTimerTick(object sender, ElapsedEventArgs e)
        {
            var runningProcesses = _processMonitor.GetAllRunningTargetPrograms();

            if (runningProcesses.Count > 0)
            {
                if (!isKilled)
                {
                    Console.WriteLine($"found {runningProcesses.Count} targets running");
                }
                KillProcess();
            }
            else
            {
                if (isKilled)
                {
                    Console.WriteLine("0 targets found, restoring");
                    RestoreProcess();
                }
            }
        }

        private void KillProcess()
        {
            var target = _configuration.KillProcess;

            foreach (Process proc in Process.GetProcessesByName(target))
            {
                isKilled = true;
                proc.Kill();
            }
        }

        private void RestoreProcess()
        {
            isKilled = false;
            var restoreCmd = _configuration.RestoreCommand;

            RunBashCommand(restoreCmd);
        }

        private Process RunBashCommand(string command)
        {
            Process proc = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                FileName = "/bin/bash",
                Arguments = $"-c \"{command} &>/dev/null &\"",
                }
            };

            proc.Start();
            return proc;
        }
    }
}