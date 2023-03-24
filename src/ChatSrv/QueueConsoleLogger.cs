using MQTTnet.Diagnostics;
using System;

namespace GameGate
{
    public class QueueConsoleLogger : IMqttNetLogger
    {
        readonly object _consoleSyncRoot = new();

        public bool IsEnabled => true;

        public void Publish(MqttNetLogLevel logLevel, string source, string message, object[]? parameters, Exception? exception)
        {
            var foregroundColor = ConsoleColor.White;
            switch (logLevel)
            {
                case MqttNetLogLevel.Verbose:
                    foregroundColor = ConsoleColor.White;
                    break;

                case MqttNetLogLevel.Info:
                    foregroundColor = ConsoleColor.Green;
                    break;

                case MqttNetLogLevel.Warning:
                    foregroundColor = ConsoleColor.DarkYellow;
                    break;

                case MqttNetLogLevel.Error:
                    foregroundColor = ConsoleColor.Red;
                    break;
            }

            if (parameters?.Length > 0)
            {
                message = string.Format(message, parameters);
            }

            lock (_consoleSyncRoot)
            {
                Console.ForegroundColor = foregroundColor;
                Console.WriteLine(message);

                if (exception != null)
                {
                    Console.WriteLine(exception);
                }
            }
        }
    }
}