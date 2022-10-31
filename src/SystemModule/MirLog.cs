using NLog;
using System;

namespace SystemModule
{
    public class MirLog
    {
        private readonly ILogger _logger;

        public MirLog()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void LogInformation(string msg)
        {
            _logger.Info(msg);
        }

        public void LogInformation(string msg, int level)
        {
            _logger.Info(msg);
        }

        public void LogError(string msg)
        {
            _logger.Error(msg);
        }

        public void LogError(Exception ex)
        {
            _logger.Error(ex);
        }

        public void DebugLog(string msg)
        {
            _logger.Debug(msg);
        }

        public void LogWarning(string msg)
        {
            _logger.Warn(msg);
        }
    }
}