using LoginGate.Conf;
using System;
using System.Collections.Concurrent;

namespace LoginGate
{
    public class LogQueue
    {
        private ConfigManager _configManager => ConfigManager.Instance;

        private static readonly LogQueue instance = new LogQueue();

        public static LogQueue Instance
        {
            get { return instance; }
        }

        public LogQueue()
        {

        }

        public readonly ConcurrentQueue<string> MessageLogQueue = new ConcurrentQueue<string>();
        public readonly ConcurrentQueue<string> DebugLogQueue = new ConcurrentQueue<string>();

        public bool ShowDebugLog => _configManager.GateConfig.ShowDebugLog;
        public int ShowLogLevel => _configManager.GateConfig.m_nShowLogLevel;

        public void Enqueue(string msg, int msgLevel)
        {
            if (ShowLogLevel >= msgLevel)
            {
                if (MessageLogQueue.Count < 100)
                    MessageLogQueue.Enqueue(string.Format("[{0}]: {1}", DateTime.Now, msg));
            }
        }

        public void Enqueue(Exception ex)
        {
            if (MessageLogQueue.Count < 100)
                MessageLogQueue.Enqueue(string.Format("[{0}]: {1} - {2}", DateTime.Now, ex.TargetSite, ex));
        }

        public void DebugLog(string msg)
        {
            if (ShowDebugLog)
            {
                if (DebugLogQueue.Count < 100)
                    DebugLogQueue.Enqueue(string.Format("[{0}]: {1}", DateTime.Now, msg));
            }
        }
    }
}