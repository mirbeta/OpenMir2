using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace SystemModule.Base
{
    public static partial class NativeMethods
    {
        /// <summary>
        /// 检索有关系统当前使用物理和虚拟内存的信息
        /// </summary>
        /// <param name="lpBuffer"></param>
        /// <returns></returns>
        [LibraryImport("Kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool GlobalMemoryStatusEx(ref ServerEnvironment.MemoryInfo lpBuffer);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetProcessTimes(IntPtr hProcess, out FILETIME lpCreationTime, out FILETIME lpExitTime, out FILETIME lpKernelTime, out FILETIME lpUserTime);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern void GetSystemTimeAsFileTime(out FILETIME lpExitTime);
    }
}