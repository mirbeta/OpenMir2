using GameGate.Conf;
using System;
using System.Collections.Concurrent;

namespace GameGate
{
    public class MirLog
    {
        private static readonly MirLog instance = new MirLog();

        public static MirLog Instance => instance;

        private static GateConfig Config => ConfigManager.Instance.GateConfig;
        public readonly ConcurrentQueue<string> MessageLogQueue = new ConcurrentQueue<string>();
        public readonly ConcurrentQueue<string> DebugLogQueue = new ConcurrentQueue<string>();

        public MirLog()
        {

        }

        public void Log(string msg, int msgLevel)
        {
            if (Config.ShowLogLevel >= msgLevel)
            {
                MessageLogQueue.Enqueue(msg);
            }
        }

        public void LogError(Exception ex)
        {
            MessageLogQueue.Enqueue($"{ex.TargetSite} - {ex}");
        }

        public void DebugLog(string msg)
        {
            if (Config.ShowDebugLog)
            {
                if (DebugLogQueue.Count < 100)
                    DebugLogQueue.Enqueue(msg);
            }
        }
    }
}