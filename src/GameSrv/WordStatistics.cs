using OpenMir2.Base;
using System.Reflection;
using System.Runtime.InteropServices;

namespace GameSrv
{
    /// <summary>
    /// 统计系统运行状态 
    /// </summary>
    public class WordStatistics
    {

        private readonly string processName;
        private readonly PerformanceCounter MemoryCounter;
        private readonly PerformanceCounter CpuCounter;
        private readonly string AppVersion;
        private const string TITLE_FORMAT_S = @"[{0}] - Legend of Mir 2 Game Server [{8}] - {1} - Players: {3} (max:{4}) - {2} - Threads[S:{5:0000},U:{6:0000},A:{7:0000}]ms";

        public WordStatistics()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                processName = Process.GetCurrentProcess().ProcessName;
                MemoryCounter = new PerformanceCounter();
                CpuCounter = new PerformanceCounter();
            }
            AppVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Error";
        }

        public void ShowServerStatus()
        {
            LogService.Debug("{0}", "=".PadLeft(64, '='));
            LogService.Info(string.Format(TITLE_FORMAT_S, SystemShare.Config.ServerName, DateTimeOffset.Now.ToString("G"),
            GameShare.NetworkMonitor.UpdateStatsAsync(1000), SystemShare.WorldEngine.OnlinePlayObject, SystemShare.WorldEngine.PlayObjectCount,
            GameShare.SystemProcess.ElapsedMilliseconds, GameShare.UserProcessor.ElapsedMilliseconds,
            0, AppVersion));

            SystemShare.ActorMgr.Analytics();

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))//todo 待实现MACOS下的状态显示
            {
                ServerEnvironment.GetCPULoad();
                ServerEnvironment.MemoryInfo memoryInfo = ServerEnvironment.GetMemoryStatus();
                LogService.Debug("CPU使用率:[{0}%]", ServerEnvironment.CpuLoad.ToString("F"));
                LogService.Debug($"物理内存:[{HUtil32.FormatBytesValue(memoryInfo.ullTotalPhys)}] 内存使用率:[{memoryInfo.dwMemoryLoad}%] 空闲内存:[{HUtil32.FormatBytesValue(memoryInfo.ullAvailPhys)}]");
                LogService.Debug($"虚拟内存:[{HUtil32.FormatBytesValue(memoryInfo.ullTotalVirtual)}] 虚拟内存使用率:[{ServerEnvironment.VirtualMemoryLoad}%] 空闲虚拟内存:[{HUtil32.FormatBytesValue(memoryInfo.ullAvailVirtual)}]");
                LogService.Debug($"使用内存:[{HUtil32.FormatBytesValue(ServerEnvironment.UsedPhysicalMemory)}] 工作内存:[{HUtil32.FormatBytesValue(ServerEnvironment.PrivateWorkingSet)}] GC内存:[{HUtil32.FormatBytesValue(GC.GetTotalMemory(false))}] ");
            }
            ShowGCStatus();
            TimeSpan ts = DateTimeOffset.Now - DateTimeOffset.FromUnixTimeMilliseconds(GameShare.StartTime);
            LogService.Debug("{0}", $"Server Start Time: {DateTimeOffset.FromUnixTimeMilliseconds(GameShare.StartTime):G}");
            LogService.Debug("{0}", $"Total Online Time: {(int)ts.TotalDays} days, {ts.Hours} hours, {ts.Minutes} minutes, {ts.Seconds} seconds");
            LogService.Debug("{0}", $"Online Players[{SystemShare.WorldEngine.OnlinePlayObject}], Max Online Players[{SystemShare.WorldEngine.PlayObjectCount}], Offline Players[{SystemShare.WorldEngine.OfflinePlayCount}], Role Count[{SystemShare.WorldEngine.PlayObjectCount}]");
            LogService.Debug("{0}", $"Total Bytes Sent: {HUtil32.FormatBytesValue(GameShare.NetworkMonitor.TotalBytesSent)}, Total Packets Sent: {HUtil32.FormatBytesValue(GameShare.NetworkMonitor.TotalPacketsSent)}");
            LogService.Debug("{0}", $"Total Bytes Recv: {HUtil32.FormatBytesValue(GameShare.NetworkMonitor.TotalBytesRecv)}, Total Packets Recv: {HUtil32.FormatBytesValue(GameShare.NetworkMonitor.TotalPacketsRecv)}");
            LogService.Debug("{0}", $"System Thread: {GameShare.SystemProcess.ElapsedMilliseconds:N0}ms");
            //LogService.Debug("{0} - {1}", $"User Thread: [{GameShare.UserProcessor.ElapsedMilliseconds:N0}ms]", $"RobotUser Thread: [{GameShare.RobotProcessor.ElapsedMilliseconds:N0}ms] Online/Queue:({SystemShare.WorldEngine.RobotPlayerCount}/{SystemShare.WorldEngine.RobotLogonQueueCount})");
            LogService.Debug("{0} - {1}", $"Event Thread: [{GameShare.EventProcessor.ElapsedMilliseconds:N0}ms]", $"Storage Thread: [{GameShare.CharacterDataProcessor.ElapsedMilliseconds:N0}ms]");
            LogService.Debug("{0} - {1}", $"Merchant Thread: [{GameShare.MerchantProcessor.ElapsedMilliseconds:N0}ms]", $"TimedBot Thread: [{GameShare.TimedRobotProcessor.ElapsedMilliseconds:N0}ms]");
            LogService.Debug("{0} {1}", $"Generator Thread: [{GameShare.GeneratorProcessor.ElapsedMilliseconds}ms]", $"Identities Remaining: ");
            //LogService.Debug("{0}", $"\tMonster: {IdentityGenerator.Monster.IdentitiesCount()}");
            //LogService.Debug("{0}", $"\tFurniture: {IdentityGenerator.Furniture.IdentitiesCount()}");
            //LogService.Debug("{0}", $"\tMapItem: {IdentityGenerator.MapItem.IdentitiesCount()}");
            //LogService.Debug("{0}", $"\tTraps: {IdentityGenerator.Traps.IdentitiesCount()}");
            LogService.Debug("{0}", "=".PadLeft(64, '='));
            FreeMemory();
            //GetRunTime();
        }

        private void FreeMemory()
        {
            GCMemoryInfo gcMemoryInfo = GC.GetGCMemoryInfo();
            if (gcMemoryInfo.TotalCommittedBytes > gcMemoryInfo.TotalAvailableMemoryBytes / 2)
            {
                LogService.Debug("释放内存...");
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive);
            }
        }

        private void GetRunTime()
        {
            TimeSpan ts = DateTimeOffset.Now - DateTimeOffset.FromUnixTimeMilliseconds(GameShare.StartTime);
            LogService.Debug($"服务器运行:[{ts.Days}天{ts.Hours}小时{ts.Minutes}分{ts.Seconds}秒]");
        }

        private void ShowGCStatus()
        {
            LogService.Debug($"GC回收:[{GC.CollectionCount(0)}]次 GC内存:[{HUtil32.FormatBytesValue(GC.GetTotalMemory(false))}] ");
            GC.Collect(0, GCCollectionMode.Forced, false);
        }

        /// <summary>
        /// 获取CPU使用率
        /// </summary>
        /// <returns></returns>
        private string GetProcessorData()
        {
            float d = GetCounterValue(CpuCounter, "Processor", "% Processor Time", processName);
            return d.ToString("F") + "%";
        }

        private static string GetCpuUsageForProcess()
        {
            DateTime startTime = DateTime.UtcNow;
            TimeSpan startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
            Thread.Sleep(500);
            DateTime endTime = DateTime.UtcNow;
            TimeSpan endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
            double cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            double totalMsPassed = (endTime - startTime).TotalMilliseconds;
            double cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
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
            string str = d.ToString("F") + "% (";
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