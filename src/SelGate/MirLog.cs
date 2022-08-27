using Microsoft.Extensions.Logging;
using SelGate.Conf;
using System;

namespace SelGate
{
    public class MirLog
    {
        private readonly ILogger<MirLog> _logger;
        private readonly ConfigManager _configManager;

        public MirLog(ILogger<MirLog> logger, ConfigManager configManager)
        {
            _configManager = configManager;
            _logger = logger;
        }

        private bool ShowDebugLog => _configManager.GateConfig.ShowDebugLog;
        private int ShowLogLevel => _configManager.GateConfig.ShowLogLevel;
        
        public void LogInformation(string msg, int msgLevel)
        {
            if (ShowLogLevel < msgLevel)
                return;
            _logger.LogInformation($"{msg}");
        }

        public void Enqueue(Exception ex)
        {
            _logger.LogError($"{ex.TargetSite} - {ex}");
        }

        public void LogWarning(string msg)
        {
            _logger.LogWarning($"{msg}");
        }
        
        public void LogDebug(string msg)
        {
            if (!ShowDebugLog)
                return;
            _logger.LogDebug($"{msg}");
        }
    }
}