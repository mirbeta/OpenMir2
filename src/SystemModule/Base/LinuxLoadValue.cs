using System;
using System.IO;

namespace SystemModule.Base
{
    internal static class LinuxLoadValue
    {
        private static CPU_OCCUPY previous_cpu_occupy = null;
        private static readonly object syncobj = new object();

        private class CPU_OCCUPY
        {
            public string name;
            public long user;
            public long nice;
            public long system;
            public long idle;
            public long lowait;
            public long irq;
            public long softirq;
        }

        public static double CPULOAD { get; private set; }

        public static void Refresh()
        {
            CPULOAD = QUERY_CPULOAD();
        }

        private static double QUERY_CPULOAD(bool a = true)
        {
            lock (syncobj)
            {
                var current_cpu_occupy = get_cpuoccupy();
                if (current_cpu_occupy == null || previous_cpu_occupy == null)
                {
                    previous_cpu_occupy = current_cpu_occupy;
                    return 0;
                }
                try
                {
                    var od = previous_cpu_occupy.user + previous_cpu_occupy.nice + previous_cpu_occupy.system + previous_cpu_occupy.idle + previous_cpu_occupy.lowait + previous_cpu_occupy.irq + previous_cpu_occupy.softirq;//第一次(用户+优先级+系统+空闲)的时间再赋给od
                    var nd = current_cpu_occupy.user + current_cpu_occupy.nice + current_cpu_occupy.system + current_cpu_occupy.idle + current_cpu_occupy.lowait + current_cpu_occupy.irq + current_cpu_occupy.softirq;//第二次(用户+优先级+系统+空闲)的时间再赋给od
                    double sum = nd - od;
                    double idle = current_cpu_occupy.idle - previous_cpu_occupy.idle;
                    var cpu_use = idle / sum;
                    if (!a)
                    {
                        idle = current_cpu_occupy.user + current_cpu_occupy.system + current_cpu_occupy.nice - previous_cpu_occupy.user - previous_cpu_occupy.system - previous_cpu_occupy.nice;
                        cpu_use = idle / sum;
                    }
                    cpu_use = cpu_use * 100 / Environment.ProcessorCount;
                    return cpu_use;
                }
                finally
                {
                    previous_cpu_occupy = current_cpu_occupy;
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
                    var ch = (char)sr.Read();
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
            var s = ReadArgumentValue(sr);
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }
            long r;
            long.TryParse(s, out r);
            return r;
        }

        private static CPU_OCCUPY get_cpuoccupy()
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
            using var sr = new StreamReader(stat);
            var occupy = new CPU_OCCUPY();
            try
            {
                occupy.name = ReadArgumentValue(sr);
                occupy.user = ReadArgumentValueInt64(sr);
                occupy.nice = ReadArgumentValueInt64(sr);
                occupy.system = ReadArgumentValueInt64(sr);
                occupy.idle = ReadArgumentValueInt64(sr);
                occupy.lowait = ReadArgumentValueInt64(sr);
                occupy.irq = ReadArgumentValueInt64(sr);
                occupy.softirq = ReadArgumentValueInt64(sr);
            }
            catch (Exception)
            {
                return null;
            }
            return occupy;
        }

        private static ulong AnalysisMeminfo(string line,string key)
        {
            var i = line.IndexOf(':');
            if (i < 0)
            {
                return 0;
            }
            var lk = line[..i];
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
            var sp = line.Split(' ');
            if (sp.Length <= 0)
            {
                return 0;
            }
            line = sp[0];
            if (string.IsNullOrEmpty(line))
            {
                return 0;
            }
            ulong.TryParse(line, out var n);
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
            using var sr = new StreamReader(stat);
            try
            {
                string line;
                ulong memFree = 0;
                ulong inactive = 0;
                while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                {
                    var value = AnalysisMeminfo(line, "MemTotal");
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
                var memUsed = mi.ullTotalPhys - memFree - inactive;
                mi.dwMemoryLoad = (uint)(memUsed * 100 / mi.ullTotalPhys);
            }
            catch (Exception)
            {
                throw new Exception("无法获得内存信息");
            }
        }
    }
}
