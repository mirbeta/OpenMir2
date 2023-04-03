using System;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;

namespace SystemModule.Base
{
    internal static class WindowsLoadValue
    {
        private static ulong g_tsSysDeltaTime = 0;
        private static ulong g_tsSysLastTime = 0;

        private static DateTime FiletimeToDateTime(FILETIME fileTime)
        {
            //NB! uint conversion must be done on both fields before ulong conversion
            ulong hFT2 = unchecked((ulong)(uint)fileTime.dwHighDateTime << 32 | (uint)fileTime.dwLowDateTime);
            return DateTime.FromFileTimeUtc((long)hFT2);
        }

        private static TimeSpan FiletimeToTimeSpan(FILETIME fileTime)
        {
            //NB! uint conversion must be done on both fields before ulong conversion
            ulong hFT2 = unchecked((ulong)(uint)fileTime.dwHighDateTime << 32 |
                                 (uint)fileTime.dwLowDateTime);
            return TimeSpan.FromTicks((long)hFT2);
        }

        private static ulong FiletimeToUlong(FILETIME fileTime)
        {
            //NB! uint conversion must be done on both fields before ulong conversion
            ulong hFT2 = unchecked((ulong)(uint)fileTime.dwHighDateTime << 32 |
                                 (uint)fileTime.dwLowDateTime);
            return hFT2;
        }

        private static double QUERY_CPULOAD()
        {
            if (!ServerEnvironment.IsWindows())
            {
                return 0;
            }

            NativeMethods.GetSystemTimeAsFileTime(out FILETIME ftNow);

            if (!NativeMethods.GetProcessTimes(Process.GetCurrentProcess().Handle, out FILETIME ftCreation, out FILETIME ftExit, out FILETIME ftKernel, out FILETIME ftUser))
            {
                return 0;
            }

            ulong tsCpuUsageTime = FiletimeToUlong(ftKernel) + FiletimeToUlong(ftUser);

            if (g_tsSysDeltaTime == 0)
            {
                g_tsSysDeltaTime = tsCpuUsageTime;
                return 0;
            }

            ulong ftSystemNowTime = FiletimeToUlong(ftNow);
            ulong tsSysTimeDelta = ftSystemNowTime - g_tsSysLastTime;
            ulong tsSystemTimeDelta = tsCpuUsageTime - g_tsSysDeltaTime;

            double cpu_load = (tsSystemTimeDelta * 100.00d + tsSysTimeDelta / 2.00d) / tsSysTimeDelta;
            g_tsSysLastTime = ftSystemNowTime;
            g_tsSysDeltaTime = tsCpuUsageTime;

            cpu_load = cpu_load / Environment.ProcessorCount;
            if (cpu_load < 0 ||
                double.IsInfinity(cpu_load) ||
                double.IsNaN(cpu_load) ||
                double.IsNegativeInfinity(cpu_load) ||
                double.IsPositiveInfinity(cpu_load))
            {
                cpu_load = 0;
            }

            return cpu_load;
        }

        public static double CPULOAD { get; private set; }

        public static void Refresh() => CPULOAD = QUERY_CPULOAD();
    }
}