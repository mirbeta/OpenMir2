using NLog;

namespace GameSvr
{
    public class MirLog
    {
        private readonly Logger _logger;

        public MirLog()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }
        
        public void Error(string message)
        {
            _logger.Error(message);
        }
        
        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }
    }
}