using LoginGate.Conf;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace LoginGate
{
    public class MirLog
    {
        private readonly ILogger<MirLog> _logger;
        private readonly ConfigManager _configManager;

        public MirLog(ILogger<MirLog> logger, ConfigManager configManager)
        {
            _logger = logger;
            _configManager = configManager;
        }

        private bool ShowDebugLog => _configManager.GetConfig.ShowDebugLog;
        private int ShowLogLevel => _configManager.GetConfig.ShowLogLevel;

        public void LogInformation(string msg, int msgLevel)
        {
            if (ShowLogLevel >= msgLevel)
            {
                _logger.LogInformation(msg);
            }
        }

        public void LogError(Exception ex)
        {
            _logger.LogError(ex.StackTrace);
        }
        
        public void LogError(string msg)
        {
            _logger.LogError(msg);
        }

        public void LogDebug(string msg)
        {
            if (ShowDebugLog)
            {
                _logger.LogDebug(msg);
            }
        }
    }
}