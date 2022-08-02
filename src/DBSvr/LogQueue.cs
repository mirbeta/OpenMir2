using System;
using System.Collections.Concurrent;

namespace DBSvr
{
    public class LogQueue
    {
        private DBConfig Config = ConfigManager.GetConfig();

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

        public bool ShowDebugLog => Config.ShowDebugLog;
        public int ShowLogLevel => Config.ShowLogLevel;

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