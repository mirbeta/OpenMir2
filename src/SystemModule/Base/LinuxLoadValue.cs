using System;
using System.IO;

namespace SystemModule.Base
{
    internal static class LinuxLoadValue
    {
        private static CpuRunInfo previousCpuOccupy = null;
        private static readonly object syncobj = new object();

        private class CpuRunInfo
        {
            public string Name;
            public long User;
            public long Nice;
            public long System;
            public long Idle;
            public long Lowait;
            public long Irq;
            public long Softirq;
        }

        public static double CpuLoad { get; private set; }

        public static void Refresh()
        {
            CpuLoad = QueryCpuLoad();
        }

        private static double QueryCpuLoad(bool a = true)
        {
            lock (syncobj)
            {
                CpuRunInfo currentCpuOccupy = get_cpuoccupy();
                if (currentCpuOccupy == null || previousCpuOccupy == null)
                {
                    previousCpuOccupy = currentCpuOccupy;
                    return 0;
                }
                try
                {
                    long od = previousCpuOccupy.User + previousCpuOccupy.Nice + previousCpuOccupy.System + previousCpuOccupy.Idle + previousCpuOccupy.Lowait + previousCpuOccupy.Irq + previousCpuOccupy.Softirq;//第一次(用户+优先级+系统+空闲)的时间再赋给od
                    long nd = currentCpuOccupy.User + currentCpuOccupy.Nice + currentCpuOccupy.System + currentCpuOccupy.Idle + currentCpuOccupy.Lowait + currentCpuOccupy.Irq + currentCpuOccupy.Softirq;//第二次(用户+优先级+系统+空闲)的时间再赋给od
                    double sum = nd - od;
                    double idle = currentCpuOccupy.Idle - previousCpuOccupy.Idle;
                    double cpuUse = idle / sum;
                    if (!a)
                    {
                        idle = currentCpuOccupy.User + currentCpuOccupy.System + currentCpuOccupy.Nice - previousCpuOccupy.User - previousCpuOccupy.System - previousCpuOccupy.Nice;
                        cpuUse = idle / sum;
                    }
                    cpuUse = cpuUse * 100 / Environment.ProcessorCount;
                    return cpuUse;
                }
                finally
                {
                    previousCpuOccupy = currentCpuOccupy;
                }
            }
        }

        private static string ReadArgumentValue(StreamReader sr)
        {
            string values = null;
            if (sr != null)
            {
                while (!sr.EndOfStream)
                {
                    char ch = (char)sr.Read();
                    if (ch == ' ')
                    {
                        if (values == null)
                        {
                            continue;
                        }
                        break;
                    }
                    values += ch;
                }
            }
            return values;
        }

        private static long ReadArgumentValueInt64(StreamReader sr)
        {
            string s = ReadArgumentValue(sr);
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }
            long.TryParse(s, out long r);
            return r;
        }

        private static CpuRunInfo get_cpuoccupy()
        {
            const string path = "/proc/stat";
            if (!File.Exists(path))
            {
                return null;
            }
            FileStream stat = null;
            try
            {
                stat = File.OpenRead(path);
            }
            catch (Exception)
            {
                return null;
            }
            using StreamReader sr = new StreamReader(stat);
            CpuRunInfo occupy = new CpuRunInfo();
            try
            {
                occupy.Name = ReadArgumentValue(sr);
                occupy.User = ReadArgumentValueInt64(sr);
                occupy.Nice = ReadArgumentValueInt64(sr);
                occupy.System = ReadArgumentValueInt64(sr);
                occupy.Idle = ReadArgumentValueInt64(sr);
                occupy.Lowait = ReadArgumentValueInt64(sr);
                occupy.Irq = ReadArgumentValueInt64(sr);
                occupy.Softirq = ReadArgumentValueInt64(sr);
            }
            catch (Exception)
            {
                return null;
            }
            return occupy;
        }

        private static ulong AnalysisMeminfo(string line, string key)
        {
            int i = line.IndexOf(':');
            if (i < 0)
            {
                return 0;
            }
            string lk = line[..i];
            if (string.IsNullOrEmpty(lk))
            {
                return 0;
            }
            if (lk != key)
            {
                return 0;
            }
            line = line[(i + 1)..].TrimStart();
            if (string.IsNullOrEmpty(line))
            {
                return 0;
            }
            string[] sp = line.Split(' ');
            if (sp.Length <= 0)
            {
                return 0;
            }
            line = sp[0];
            if (string.IsNullOrEmpty(line))
            {
                return 0;
            }
            ulong.TryParse(line, out ulong n);
            return n * 1024;
        }

        public static void GlobalMemoryStatus(ref ServerEnvironment.MemoryInfo mi)
        {
            const string path = "/proc/meminfo";
            if (!File.Exists(path))
            {
                return;
            }
            FileStream stat = null;
            try
            {
                stat = File.OpenRead(path);
            }
            catch (Exception)
            {
                return;
            }
            using StreamReader sr = new StreamReader(stat);
            try
            {
                string line;
                ulong memFree = 0;
                ulong inactive = 0;
                while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                {
                    ulong value = AnalysisMeminfo(line, "MemTotal");
                    if (value != 0)
                    {
                        mi.ullTotalPhys = value;
                        continue;
                    }
                    value = AnalysisMeminfo(line, "MemAvailable");
                    if (value != 0)
                    {
                        mi.ullAvailPhys = value;
                        continue;
                    }
                    value = AnalysisMeminfo(line, "MemFree");
                    if (value != 0)
                    {
                        memFree = value;
                        continue;
                    }
                    value = AnalysisMeminfo(line, "Inactive");
                    if (value != 0)
                    {
                        inactive = value;
                        continue;
                    }
                    value = AnalysisMeminfo(line, "SwapTotal");
                    if (value != 0)
                    {
                        mi.ullTotalVirtual = value;
                        continue;
                    }
                    value = AnalysisMeminfo(line, "SwapFree");
                    if (value != 0)
                    {
                        mi.ullAvailVirtual = value;
                        continue;
                    }
                }
                ulong memUsed = mi.ullTotalPhys - memFree - inactive;
                mi.dwMemoryLoad = (uint)(memUsed * 100 / mi.ullTotalPhys);
            }
            catch (Exception)
            {
                throw new Exception("无法获得内存信息");
            }
        }
    }
}