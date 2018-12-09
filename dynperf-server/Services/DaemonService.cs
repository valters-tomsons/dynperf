using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
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
            ServiceTimer.Start();
        }

        private void InitializeTimer()
        {
            ServiceTimer = new System.Timers.Timer();
            ServiceTimer.Interval = _configuration.ScanIntervalMs;
            ServiceTimer.Elapsed += ServiceTimerTick;
        }

        private void ServiceTimerTick(object sender, ElapsedEventArgs e)
        {
            var runningProcesses = _processMonitor.GetAllRunningTargetPrograms();

            if (runningProcesses.Count > 0)
            {
                PrintProcesses(runningProcesses);
                KillProcess();
            }
            else
            {
                if (isKilled)
                {
                    RestoreProcess();
                }
            }
        }

        private void PrintProcesses(List<Process> processes)
        {
            foreach (var proc in processes)
            {
                System.Console.WriteLine($"Found Process: {proc.ProcessName}");
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
            System.Console.WriteLine("Restoring process");

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