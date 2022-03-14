using LoginGate.Conf;
using System;
using System.Collections.Concurrent;

namespace LoginGate
{
    public class LogQueue
    {
        private readonly ConfigManager _configManager;

        public LogQueue(ConfigManager configManager)
        {
            _configManager = configManager;
        }

        public readonly ConcurrentQueue<string> MessageLog = new ConcurrentQueue<string>();
        public readonly ConcurrentQueue<string> DebugLog = new ConcurrentQueue<string>();

        public bool ShowDebugLog => _configManager.GateConfig.ShowDebugLog;
        public int ShowLogLevel => _configManager.GateConfig.m_nShowLogLevel;
        
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