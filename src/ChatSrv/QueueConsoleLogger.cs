using MQTTnet.Diagnostics;
using NLog;
using System;

namespace GameGate
{
    public class QueueConsoleLogger : IMqttNetLogger
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public bool IsEnabled => true;

        public void Publish(MqttNetLogLevel logLevel, string source, string message, object[]? parameters, Exception? exception)
        {
            switch (logLevel)
            {
                case MqttNetLogLevel.Verbose:
                    if (parameters?.Length > 0)
                    {
                        message = string.Format(message, parameters);
                    }
                    _logger.Trace(message);
                    break;

                case MqttNetLogLevel.Info:
                    if (parameters?.Length > 0)
                    {
                        message = string.Format(message, parameters);
                    }
                    _logger.Info(message);
                    break;

                case MqttNetLogLevel.Warning:
                    if (parameters?.Length > 0)
                    {
                        message = string.Format(message, parameters);
                    }
                    _logger.Warn(message);
                    break;

                case MqttNetLogLevel.Error:
                    if (parameters?.Length > 0)
                    {
                        message = string.Format(message, parameters);
                    }
                    _logger.Error(message);
                    break;
            }
        }
    }
}