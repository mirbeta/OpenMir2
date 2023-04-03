using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace SystemModule.Base
{
    public static class ServerEnvironment
    {
        private static MemoryInfo memoryInfo = default;
        private static long g_llInOutNicBytesReceived = 0;
        private static long g_llInOutNicBytesSent = 0;
        private static NetworkInterface m_poInOutNetworkInterface = null;
        private static readonly ConcurrentDictionary<int, Stopwatch> m_poStopWatchTable = new ConcurrentDictionary<int, Stopwatch>();

        static ServerEnvironment()
        {

        }

        public static unsafe MemoryInfo GetMemoryStatus()
        {
            memoryInfo.Refresh();
            if (IsWindows())
            {
                if (!NativeMethods.GlobalMemoryStatusEx(ref memoryInfo))
                {
                    throw new Exception("无法获得内存信息");
                }
            }
            else
            {
                LinuxLoadValue.GlobalMemoryStatus(ref memoryInfo);
            }
            return memoryInfo;
        }

        public static void GetCPULoad()
        {
            if (IsWindows())
            {
                WindowsLoadValue.Refresh();
            }
            else
            {
                LinuxLoadValue.Refresh();
            }
        }

        public static void GetGetWorkInfo()
        {
            // 刷新当前出入流量网卡的信息
            do
            {
                NetworkInterface poInOutNetworkInterface = QUERY_INOUT_NETWORK_INTERFACE();
                if (m_poInOutNetworkInterface == null || m_poInOutNetworkInterface.Name != poInOutNetworkInterface?.Name)
                {
                    IPInterfaceStatistics poIPInterfaceStatistics = poInOutNetworkInterface?.GetIPStatistics();
                    if (poIPInterfaceStatistics != null)
                    {
                        g_llInOutNicBytesReceived = poIPInterfaceStatistics.BytesReceived;
                        g_llInOutNicBytesSent = poIPInterfaceStatistics.BytesSent;
                    }
                    PerSecondBytesSent = 0;
                    PerSecondBytesReceived = 0;
                }
                else
                {
                    IPInterfaceStatistics poIPInterfaceStatistics = poInOutNetworkInterface.GetIPStatistics();
                    if (poIPInterfaceStatistics != null)
                    {
                        PerSecondBytesSent = poIPInterfaceStatistics.BytesReceived - g_llInOutNicBytesReceived;
                        PerSecondBytesReceived = poIPInterfaceStatistics.BytesSent - g_llInOutNicBytesSent;
                        g_llInOutNicBytesReceived = poIPInterfaceStatistics.BytesReceived;
                        g_llInOutNicBytesSent = poIPInterfaceStatistics.BytesSent;
                    }
                }
                m_poInOutNetworkInterface = poInOutNetworkInterface;
            } while (false);
        }

        private static unsafe bool Equals(IPAddress x, IPAddress y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x.AddressFamily != y.AddressFamily)
            {
                return false;
            }

            byte[] bx = x.GetAddressBytes();
            byte[] by = y.GetAddressBytes();
            if (bx.Length != by.Length)
            {
                return false;
            }

            fixed (byte* pinnedX = bx)
            {
                fixed (byte* pinnedY = by)
                {
                    if (bx.Length == 4)
                    {
                        return *(uint*)pinnedX == *(uint*)pinnedY; // 32bit
                    }
                    else if (bx.Length == 8)
                    {
                        return *(ulong*)pinnedX == *(ulong*)pinnedY; // 64bit
                    }
                    else if (bx.Length == 16)
                    {
                        return *(decimal*)pinnedX == *(decimal*)pinnedY; // 128bit
                    }
                    else if (bx.Length == 2)
                    {
                        return *(ushort*)pinnedX == *(ushort*)pinnedY; // 16bit
                    }
                    else if (bx.Length == 1)
                    {
                        return *pinnedX == *pinnedY;
                    }
                    else
                    {
                        for (int i = 0; i < bx.Length; ++i)
                        {
                            if (pinnedX[i] != pinnedY[i])
                            {
                                return false;
                            }
                        }

                        return true;
                    }
                }
            }
        }

        private static NetworkInterface QUERY_INOUT_NETWORK_INTERFACE()
        {
            return NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(ni =>
            {
                if (ni.NetworkInterfaceType != NetworkInterfaceType.Ethernet &&
                    ni.NetworkInterfaceType != NetworkInterfaceType.Wireless80211 &&
                    ni.NetworkInterfaceType != NetworkInterfaceType.Ppp) // PPPOE宽带拨号
                {
                    return false;
                }

                if (ni.OperationalStatus != OperationalStatus.Up)
                {
                    return false;
                }

                foreach (UnicastIPAddressInformation addressInfo in ni.GetIPProperties().UnicastAddresses)
                {
                    if (addressInfo.Address.AddressFamily != AddressFamily.InterNetwork)
                    {
                        continue;
                    }

                    if (Equals(addressInfo.Address, IPAddress.Any)
                        || Equals(addressInfo.Address, IPAddress.None)
                        || Equals(addressInfo.Address, IPAddress.Broadcast)
                        || Equals(addressInfo.Address, IPAddress.Loopback))
                    {
                        continue;
                    }

                    if (IsWindows())
                    {
                        if (addressInfo.DuplicateAddressDetectionState == DuplicateAddressDetectionState.Preferred)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            });
        }

        public static long PerSecondBytesSent { get; set; }

        public static long PerSecondBytesReceived { get; set; }

        public static double CpuLoad => IsWindows() ? WindowsLoadValue.CPULOAD : LinuxLoadValue.CpuLoad;

        public static ulong AvailablePhysicalMemory => memoryInfo.ullAvailPhys;

        /// <summary>
        /// 获取物理内存已使用大小
        /// </summary>
        public static ulong UsedPhysicalMemory => memoryInfo.ullTotalPhys - memoryInfo.ullAvailPhys;

        /// <summary>
        /// 获取虚拟内存已使用大小
        /// </summary>
        public static ulong UsedVirtualMemory => memoryInfo.ullTotalVirtual - memoryInfo.ullAvailVirtual;

        /// <summary>
        /// 虚拟内存使用率
        /// </summary>
        public static ulong VirtualMemoryLoad => (memoryInfo.ullTotalVirtual - memoryInfo.ullAvailVirtual) * 100 / memoryInfo.ullTotalVirtual;

        public static bool IsWindows()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        public static ulong TotalPhysicalMemory => memoryInfo.ullTotalPhys;

        public static long PrivateWorkingSet => Environment.WorkingSet;

        public static int ProcessorCount => Environment.ProcessorCount;

        public static NetworkInterface GetInOutNetworkInterface() => m_poInOutNetworkInterface;

        public static int ClockSleepTime(int maxConcurrent)
        {
            int intManagedThreadId = Thread.CurrentThread.ManagedThreadId;
            lock (m_poStopWatchTable)
            {
                if (!m_poStopWatchTable.TryGetValue(intManagedThreadId, out Stopwatch poStopWatch) || poStopWatch == null)
                {
                    poStopWatch = new Stopwatch();
                    poStopWatch.Start();
                    m_poStopWatchTable[intManagedThreadId] = poStopWatch;
                }
                long llElapsedWatchTicks = poStopWatch.ElapsedTicks;
                double dblTotalMilliseconds = llElapsedWatchTicks / 10000.00;
                if (dblTotalMilliseconds < 1)
                {
                    return 0;
                }
                poStopWatch.Restart();
            }
            const double MAX_USE_LOAD = 100;
            if (maxConcurrent <= 0)
            {
                return 0;
            }
            double dblUseLoad = CpuLoad;
            if (dblUseLoad < MAX_USE_LOAD) // 控制CPU利用率
            {
                return 0;
            }
            else
            {
                double dblAviLoad = unchecked(dblUseLoad - MAX_USE_LOAD);
                double dblSleepTime = unchecked(dblAviLoad / maxConcurrent);
                dblSleepTime = Math.Ceiling(dblSleepTime);
                if (dblSleepTime < 1)
                {
                    dblSleepTime = 1;
                }
                return unchecked((int)dblSleepTime);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MemoryInfo
        {
            /// <summary>
            /// 结构的大小，以字节为单位，必须在调用 GlobalMemoryStatusEx 之前设置此成员，可以用 Init 方法提前处理
            /// </summary>
            /// <remarks>应当使用本对象提供的 Init ，而不是使用构造函数！</remarks>
            internal uint dwLength;
            /// <summary>
            /// 一个介于 0 和 100 之间的数字，用于指定正在使用的物理内存的大致百分比（0 表示没有内存使用，100 表示内存已满）。
            /// </summary>
            public uint dwMemoryLoad;
            /// <summary>
            /// 实际物理内存量，以字节为单位
            /// </summary>
            public ulong ullTotalPhys;
            /// <summary>
            /// 当前可用的物理内存量，以字节为单位。这是可以立即重用而无需先将其内容写入磁盘的物理内存量。它是备用列表、空闲列表和零列表的大小之和
            /// </summary>
            public ulong ullAvailPhys;
            /// <summary>
            /// 系统或当前进程的当前已提交内存限制，以字节为单位，以较小者为准。要获得系统范围的承诺内存限制，请调用GetPerformanceInfo
            /// </summary>
            public ulong ullTotalPageFile;
            /// <summary>
            /// 当前进程可以提交的最大内存量，以字节为单位。该值等于或小于系统范围的可用提交值。要计算整个系统的可承诺值，调用GetPerformanceInfo核减价值CommitTotal从价值CommitLimit
            /// </summary>
            public ulong ullAvailPageFile;
            /// <summary>
            /// 调用进程的虚拟地址空间的用户模式部分的大小，以字节为单位。该值取决于进程类型、处理器类型和操作系统的配置。例如，对于 x86 处理器上的大多数 32 位进程，此值约为 2 GB，对于在启用4 GB 调整的系统上运行的具有大地址感知能力的 32 位进程约为 3 GB 。
            /// </summary>
            public ulong ullTotalVirtual;
            /// <summary>
            /// 当前在调用进程的虚拟地址空间的用户模式部分中未保留和未提交的内存量，以字节为单位
            /// </summary>
            public ulong ullAvailVirtual;
            /// <summary>
            /// 预订的。该值始终为 0
            /// </summary>
            internal ulong ullAvailExtendedVirtual;

            internal void Refresh()
            {
                dwLength = checked((uint)Marshal.SizeOf(typeof(MemoryInfo)));
            }
        }
    }
}