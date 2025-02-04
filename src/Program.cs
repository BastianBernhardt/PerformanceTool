using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;

class Program
{
    // Cache für PerformanceCounter-Instanzen pro Prozess (Key: ProcessId)
    static Dictionary<int, PerformanceCounter> processCpuCounters = new Dictionary<int, PerformanceCounter>();

    // Total CPU PerformanceCounter einmal anlegen
    static PerformanceCounter totalCpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);

    static void Main(string[] args)
    {
        totalCpuCounter.NextValue();

        while (true)
        {
            var processInfo = GetProcessInfo();
            LogProcessInfo(processInfo);
            Thread.Sleep(333); // 3 mal pro Sekunde
        }
    }

    static ProcessInfo GetProcessInfo()
    {
        var processes = Process.GetProcesses();
        var topProcesses = processes.Select(p => new ProcessDetail
        {
            ProcessName = p.ProcessName,
            CpuUsage = GetCpuUsage(p),
            MemoryUsage = p.WorkingSet64
        })
        .OrderByDescending(detail => detail.CpuUsage)
        .Take(5)
        .ToList();

        double totalCpuUsage = GetTotalCpuUsage();
        return new ProcessInfo
        {
            TotalCpuUsage = totalCpuUsage,
            Processes = topProcesses
        };
    }

    static double GetCpuUsage(Process process)
    {
        try
        {
            if (!processCpuCounters.TryGetValue(process.Id, out PerformanceCounter counter))
            {
                counter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName, true);
                counter.NextValue(); // Warm-up
                processCpuCounters[process.Id] = counter;
            }
            double cpuUsage = counter.NextValue() / Environment.ProcessorCount;
            return Math.Round(cpuUsage, 2);
        }
        catch
        {
            processCpuCounters.Remove(process.Id);
            return 0.0;
        }
    }

    static double GetTotalCpuUsage()
    {
        try
        {
            double usage = totalCpuCounter.NextValue();
            return Math.Round(usage, 2);
        }
        catch
        {
            return 0.0;
        }
    }

    static void LogProcessInfo(ProcessInfo processInfo)
    {
        var logEntry = new
        {
            Timestamp = DateTime.Now,
            TotalCpuUsage = processInfo.TotalCpuUsage,
            Processes = processInfo.Processes
        };

        string json = JsonConvert.SerializeObject(logEntry, Formatting.Indented);
        File.AppendAllText("process_log.json", json + Environment.NewLine);
    }
}