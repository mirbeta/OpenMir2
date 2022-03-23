using System;
using System.Collections.Concurrent;

namespace LoginSvr
{
    public class LogQueue
    {
        public readonly ConcurrentQueue<string> MessageLog = new ConcurrentQueue<string>();
        public readonly ConcurrentQueue<string> DebugLog = new ConcurrentQueue<string>();
        private static readonly LogQueue instance = new LogQueue();

        public static LogQueue Instance
        {
            get { return instance; }
        }

        public void Enqueue(string msg)
        {
            if (MessageLog.Count < 100)
                MessageLog.Enqueue(string.Format("[{0}]: {1}", DateTime.Now, msg));
        }

        public void Enqueue(Exception ex)
        {
            if (MessageLog.Count < 100)
                MessageLog.Enqueue(string.Format("[{0}]: {1} - {2}", DateTime.Now, ex.TargetSite, ex));
        }

        public void EnqueueDebugging(string msg)
        {
            if (DebugLog.Count < 100)
                DebugLog.Enqueue(string.Format("[{0}]: {1}", DateTime.Now, msg));
        }
    }
}