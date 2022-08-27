using DBSvr.Conf;
using Microsoft.Extensions.Logging;
using System;

namespace DBSvr
{
    public class MirLog
    {
        private readonly ILogger<MirLog> _logger;
        private readonly DBConfig _config = ConfigManager.GetConfig();

        public MirLog(ILogger<MirLog> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string msg)
        {
            _logger.LogInformation($"{msg}");
        }

        public void Enqueue(Exception ex)
        {
            _logger.LogError($"{ex.TargetSite} - {ex}");
        }

        public void LogError(string msg)
        {
            _logger.LogError(msg);
        }

        public void DebugLog(string msg)
        {
            if (_config.ShowDebugLog)
            {
                _logger.LogDebug($"{msg}");
            }
        }

        public void LogWarning(string msg)
        {
            _logger.LogWarning(msg);
        }
    }
}