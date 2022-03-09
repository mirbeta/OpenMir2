using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace SelGate
{
    public class LogQueue
    {
        private readonly ConfigManager _configManager;
        private readonly ILogger<LogQueue> _logger;

        public LogQueue(ILogger<LogQueue> logger, ConfigManager configManager)
        {
            _logger = logger;
            _configManager = configManager;
        }

        public readonly ConcurrentQueue<string> MessageLog = new ConcurrentQueue<string>();
        public readonly ConcurrentQueue<string> DebugLog = new ConcurrentQueue<string>();

        public void Enqueue(string msg, int msgLevel)
        {
            if (_configManager.GateConfig.m_nShowLogLevel >= msgLevel)
            {
                if (MessageLog.Count < 100)
                    MessageLog.Enqueue(string.Format("[{0}]: {1}", DateTime.Now, msg));
            }
            _logger.LogInformation(msg);
        }

        public void Enqueue(Exception ex)
        {
            if (MessageLog.Count < 100)
                MessageLog.Enqueue(string.Format("[{0}]: {1} - {2}", DateTime.Now, ex.TargetSite, ex));

            _logger.LogError(ex, ex.Message);
        }

        public void EnqueueDebugging(string msg)
        {
            if (DebugLog.Count < 100)
                DebugLog.Enqueue(string.Format("[{0}]: {1}", DateTime.Now, msg));

            _logger.LogDebug(msg);
        }
    }
}