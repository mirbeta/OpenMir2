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
    public class ServerEnvironment
    {
        private static readonly Stopwatch _getMemoryStatusWath = new Stopwatch();
        private static MemoryInfo memoryInfo = new MemoryInfo();
        private static long g_llInOutNicBytesReceived = 0;
        private static long g_llInOutNicBytesSent = 0;
        private static NetworkInterface m_poInOutNetworkInterface = null;
        private static ConcurrentDictionary<int, Stopwatch> m_poStopWatchTable = new ConcurrentDictionary<int, Stopwatch>();

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GlobalMemoryStatusEx(ref MemoryInfo memory);

        static ServerEnvironment()
        {

        }

        public static unsafe MemoryInfo GetMemoryStatus()
        {
            lock (_getMemoryStatusWath)
            {
                if (!_getMemoryStatusWath.IsRunning || _getMemoryStatusWath.ElapsedMilliseconds >= 500)
                {
                    _getMemoryStatusWath.Restart();
                    memoryInfo.Length = (uint)sizeof(MemoryInfo);
                    if (IsWindows())
                    {
                        GlobalMemoryStatusEx(ref memoryInfo);
                    }
                    else
                    {
                        CPULinuxLoadValue.GlobalMemoryStatus(ref memoryInfo);
                    }
                }
            }
            return memoryInfo;
        }

        public static void GetCPULoad()
        {
            CPULinuxLoadValue.Refresh();
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

        public unsafe static bool Equals(IPAddress x, IPAddress y)
        {
            if (x == null && y == null)
                return true;
            if (x.AddressFamily != y.AddressFamily)
                return false;

            byte[] bx = x.GetAddressBytes();
            byte[] by = y.GetAddressBytes();
            if (bx.Length != by.Length)
                return false;

            fixed (byte* pinnedX = bx)
            {
                fixed (byte* pinnedY = by)
                {
                    if (bx.Length == 4)
                        return *(uint*)pinnedX == *(uint*)pinnedY; // 32bit
                    else if (bx.Length == 8)
                        return *(ulong*)pinnedX == *(ulong*)pinnedY; // 64bit
                    else if (bx.Length == 16)
                        return *(decimal*)pinnedX == *(decimal*)pinnedY; // 128bit
                    else if (bx.Length == 2)
                        return *(ushort*)pinnedX == *(ushort*)pinnedY; // 16bit
                    else if (bx.Length == 1)
                        return *pinnedX == *pinnedY;
                    else
                    {
                        for (int i = 0; i < bx.Length; ++i)
                            if (pinnedX[i] != pinnedY[i])
                                return false;
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

                foreach (var addressInfo in ni.GetIPProperties().UnicastAddresses)
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

        public static double CPULOAD
        {
            get
            {
                if (IsWindows())
                {
                    return CPUWin32LoadValue.CPULOAD;
                }
                return CPULinuxLoadValue.CPULOAD;
            }
        }

        public static long AvailablePhysicalMemory => GetMemoryStatus().AvailPhys;

        /// <summary>
        /// 获取物理内存已使用大小
        /// </summary>
        public static long UsedPhysicalMemory
        {
            get
            {
                MemoryInfo mi = GetMemoryStatus();
                return mi.TotalPhys - mi.AvailPhys;
            }
        }
        
        public static long UsedVirtualMemory
        {
            get
            {
                MemoryInfo mi = GetMemoryStatus();
                return mi.TotalVirtual - mi.AvailVirtual;
            }
        }

        public static bool IsWindows()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        public static long TotalPhysicalMemory => GetMemoryStatus().TotalPhys;

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
            double dblUseLoad = CPULOAD;
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

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MemoryInfo
        {
            /// <summary>
            /// 当前结构体大小
            /// </summary>
            public uint Length;
            /// <summary>
            /// 当前内存使用率
            /// </summary>
            public uint MemoryLoad;
            /// <summary>
            /// 总计物理内存大小
            /// </summary>
            public long TotalPhys;
            /// <summary>
            /// 可用物理内存大小
            /// </summary>
            public long AvailPhys;
            /// <summary>
            /// 总计交换文件大小
            /// </summary>
            public long TotalPageFile;
            /// <summary>
            /// 可用交互分区大小
            /// </summary>
            public long AvailPageFile;
            /// <summary>
            /// 总计虚拟内存大小
            /// </summary>
            public long TotalVirtual;
            /// <summary>
            /// 可用虚拟内存大小
            /// </summary>
            public long AvailVirtual;
            /// <summary>
            /// 保存字段，始终为0
            /// </summary>
            public long AvailExtendedVirtual;
        }
    }
}