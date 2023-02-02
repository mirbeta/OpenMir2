using GameGate.Conf;
using System;
using System.Collections.Concurrent;
using NLog;

namespace GameGate
{
    public class MirLog
    {
        private static GateConfig Config => ConfigManager.Instance.GateConfig;
        public readonly ConcurrentQueue<string> MessageLogQueue = new ConcurrentQueue<string>();
        public readonly ConcurrentQueue<string> DebugLogQueue = new ConcurrentQueue<string>();

        public MirLog()
        {
            LogManager.Configuration.Variables["LogLevel"] = "Debug";
            LogManager.ReconfigExistingLoggers(); 
        }

        public void Log(string msg, int msgLevel)
        {
            if (Config.LogLevel >= msgLevel)
            {
                MessageLogQueue.Enqueue(msg);
            }
        }

        public void LogError(string msg)
        {
            MessageLogQueue.Enqueue(msg);
        }
        
        public void LogError(Exception ex)
        {
            MessageLogQueue.Enqueue($"{ex.TargetSite} - {ex}");
        }

        public void DebugLog(string msg)
        {
            if (Config.DebugLog)
            {
                if (DebugLogQueue.Count < 100)
                    DebugLogQueue.Enqueue(msg);
            }
        }
    }
}