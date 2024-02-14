using MQTTnet.Diagnostics;
using OpenMir2;
using System;

namespace GameGate
{
    public class QueueConsoleLogger : IMqttNetLogger
    {
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
                    LogService.Info(message);
                    break;

                case MqttNetLogLevel.Info:
                    if (parameters?.Length > 0)
                    {
                        message = string.Format(message, parameters);
                    }
                    LogService.Info(message);
                    break;

                case MqttNetLogLevel.Warning:
                    if (parameters?.Length > 0)
                    {
                        message = string.Format(message, parameters);
                    }
                    LogService.Warn(message);
                    break;

                case MqttNetLogLevel.Error:
                    if (parameters?.Length > 0)
                    {
                        message = string.Format(message, parameters);
                    }
                    LogService.Error(message);
                    break;
            }
        }
    }
}