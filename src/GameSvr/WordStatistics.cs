using NLog;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace GameSvr
{
    /// <summary>
    /// 统计系统运行状态 
    /// 仅支持Windows系统
    /// </summary>
    //[SupportedOSPlatform("windows")]
    public class WordStatistics
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly string processName;
        private readonly PerformanceCounter MemoryCounter;
        private readonly PerformanceCounter CpuCounter;

        public WordStatistics()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                processName = Process.GetCurrentProcess().ProcessName;
                MemoryCounter = new PerformanceCounter();
                CpuCounter = new PerformanceCounter();
            }
            else
            {
                _logger.Warn("当前运行环境非Windows环境,系统统计信息受限.");
            }
        }

        public void ShowServerState()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _logger.Debug($"CPU:[{GetCpuUsageForProcess()}] 使用内存率:[{HUtil32.FormatBytesValue(GC.GetTotalMemory(false))}] 空闲内存:[{HUtil32.FormatBytesValue(GetFreePhysicalMemory())}]  ");
                _logger.Debug($"虚拟内存信息:[{GetMemoryVData()}] 虚拟内存大小:[{HUtil32.FormatBytesValue(GetTotalVirtualMemory())}] 虚拟内存使用:[{HUtil32.FormatBytesValue(GetUsageVirtualMemory())}%]");
                _logger.Debug($"线程数:[{GetThreadCount()}]");
            }
            ShowGCStatus();
            GetRunTime();
        }

        private void GetRunTime()
        {
            var ts = DateTimeOffset.Now - DateTimeOffset.FromUnixTimeMilliseconds(M2Share.StartTime);
            _logger.Debug($"服务器运行:[{ts.Days}天{ts.Hours}小时{ts.Minutes}分{ts.Seconds}秒]");
        }

        private void ShowGCStatus()
        {
            _logger.Debug($"GC回收:[{GC.CollectionCount(0)}] 分配内存:[{HUtil32.FormatBytesValue(GC.GetTotalMemory(false))}]");
            GC.Collect(0, GCCollectionMode.Forced, false);
        }

        /// <summary>
        /// 获取CPU使用率
        /// </summary>
        /// <returns></returns>
        private string GetProcessorData()
        {
            var d = GetCounterValue(CpuCounter, "Processor", "% Processor Time", processName);
            return d.ToString("F") + "%";
        }

        private static string GetCpuUsageForProcess()
        {
            var startTime = DateTime.UtcNow;
            var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
            Thread.Sleep(500);
            var endTime = DateTime.UtcNow;
            var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;
            var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
            return (cpuUsageTotal * 100).ToString("F") + "%";
        }

        /// <summary>
        /// 获取当前程序线程数
        /// </summary>
        /// <returns></returns>
        private float GetThreadCount()
        {
            return GetCounterValue(CpuCounter, "Process", "Thread Count", processName);
        }

        /// <summary>
        /// 获取工作集内存大小
        /// </summary>
        /// <returns></returns>
        private float GetWorkingSet()
        {
            return GetCounterValue(MemoryCounter, "Memory", "Working Set", processName);
        }

        /// <summary>
        /// 获取虚拟内存使用率详情
        /// </summary>
        /// <returns></returns>
        private string GetMemoryVData()
        {
            float d = GetCounterValue(MemoryCounter, "Memory", "% Committed Bytes In Use", null);
            var str = d.ToString("F") + "% (";
            d = GetCounterValue(MemoryCounter, "Memory", "Committed Bytes", null);
            str += HUtil32.FormatBytesValue(d) + " / ";
            d = GetCounterValue(MemoryCounter, "Memory", "Commit Limit", null);
            return str + HUtil32.FormatBytesValue(d) + ") ";
        }

        /// <summary>
        /// 获取虚拟内存使用率
        /// </summary>
        /// <returns></returns>
        private float GetUsageVirtualMemory()
        {
            return GetCounterValue(MemoryCounter, "Memory", "% Committed Bytes In Use", null);
        }

        /// <summary>
        /// 获取虚拟内存已用大小
        /// </summary>
        /// <returns></returns>
        private float GetUsedVirtualMemory()
        {
            return GetCounterValue(MemoryCounter, "Memory", "Committed Bytes", null);
        }

        /// <summary>
        /// 获取虚拟内存总大小
        /// </summary>
        /// <returns></returns>
        private float GetTotalVirtualMemory()
        {
            return GetCounterValue(MemoryCounter, "Memory", "Commit Limit", null);
        }

        /// <summary>
        /// 获取空闲的物理内存数，单位B
        /// </summary>
        /// <returns></returns>
        private float GetFreePhysicalMemory()
        {
            return GetCounterValue(MemoryCounter, "Memory", "Available Bytes", null);
        }
        
        private static float GetCounterValue(PerformanceCounter pc, string categoryName, string counterName, string instanceName)
        {
            pc.CategoryName = categoryName;
            pc.CounterName = counterName;
            pc.InstanceName = instanceName;
            return pc.NextValue();
        }
    }
}