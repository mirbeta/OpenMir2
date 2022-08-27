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
        public readonly ConcurrentQueue<string> MessageLog = new ConcurrentQueue<string>();
        public readonly ConcurrentQueue<string> DebugLog = new ConcurrentQueue<string>();

        public MirLog()
        {

        }

        public void Enqueue(string msg, int msgLevel)
        {
            if (Config.ShowLogLevel >= msgLevel)
            {
                if (MessageLog.Count < 100)
                    MessageLog.Enqueue($"[{DateTime.Now}]: {msg}");
            }
        }

        public void Enqueue(Exception ex)
        {
            if (MessageLog.Count < 100)
                MessageLog.Enqueue($"[{DateTime.Now}]: {ex.TargetSite} - {ex}");
        }

        public void EnqueueDebugging(string msg)
        {
            if (Config.ShowDebugLog)
            {
                if (DebugLog.Count < 100)
                    DebugLog.Enqueue($"[{DateTime.Now}]: {msg}");
            }
        }
    }
}