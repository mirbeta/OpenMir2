using LoginSvr.Conf;
using Microsoft.Extensions.Logging;
using System;

namespace LoginSvr
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

        public void Information(string msg, int msgLevel = 1)
        {
            if (_configManager.Config.ShowLogLevel >= msgLevel)
            {
                _logger.LogInformation(msg);
            }
        }
        
        public void Warn(string msg)
        {
            _logger.LogWarning(msg);
        }

        public void LogError(Exception ex)
        {
            _logger.LogError(ex.StackTrace);
        }

        public void LogDebug(string msg)
        {
            if (_configManager.Config.ShowDebugLog)
            {
                _logger.LogDebug(msg);
            }
        }
    }
}