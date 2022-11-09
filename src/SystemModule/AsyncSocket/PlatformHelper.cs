using System;

namespace SystemModule.AsyncSocket
{
    internal static class PlatformHelper
    {
        private const int PROCESSOR_COUNT_REFRESH_INTERVAL_MS = 30000;

        private static volatile int s_processorCount;

        private static volatile int s_lastProcessorCountRefreshTicks;

        internal static int ProcessorCount
        {
            get
            {
                int tickCount = Environment.TickCount;
                int num = PlatformHelper.s_processorCount;
                if (num == 0 || tickCount - PlatformHelper.s_lastProcessorCountRefreshTicks >= 30000)
                {
                    num = (PlatformHelper.s_processorCount = Environment.ProcessorCount);
                    PlatformHelper.s_lastProcessorCountRefreshTicks = tickCount;
                }
                return num;
            }
        }

        internal static bool IsSingleProcessor
        {
            get
            {
                return PlatformHelper.ProcessorCount == 1;
            }
        }
    }
}