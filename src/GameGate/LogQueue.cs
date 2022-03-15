using System;
using System.Collections.Concurrent;

namespace GameGate
{
    public class LogQueue
    {
        private static readonly LogQueue instance = new LogQueue();

        public static LogQueue Instance
        {
            get { return instance; }
        }

        private ConfigManager _configManager => ConfigManager.Instance;
        public readonly ConcurrentQueue<string> MessageLog = new ConcurrentQueue<string>();
        public readonly ConcurrentQueue<string> DebugLog = new ConcurrentQueue<string>();

        public LogQueue()
        {
            
        }

        public bool ShowDebugLog => _configManager.GateConfig.ShowDebugLog;
        public int ShowLogLevel => _configManager.GateConfig.ShowLogLevel;

        public void Enqueue(string msg, int msgLevel)
        {
            if (ShowLogLevel >= msgLevel)
            {
                if (MessageLog.Count < 100)
                    MessageLog.Enqueue(string.Format("[{0}]: {1}", DateTime.Now, msg));
            }
        }

        public void Enqueue(Exception ex)
        {
            if (MessageLog.Count < 100)
                MessageLog.Enqueue(string.Format("[{0}]: {1} - {2}", DateTime.Now, ex.TargetSite, ex));
        }

        public void EnqueueDebugging(string msg)
        {
            if (ShowDebugLog)
            {
                if (DebugLog.Count < 100)
                    DebugLog.Enqueue(string.Format("[{0}]: {1}", DateTime.Now, msg));
            }
        }
    }
}