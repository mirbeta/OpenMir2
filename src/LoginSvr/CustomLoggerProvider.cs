using System;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace LoginSvr
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        public void Dispose() { }

        public ILogger CreateLogger(string categoryName)
        {
            return new CustomConsoleLogger(categoryName);
        }

        public class CustomConsoleLogger : ILogger
        {
            private readonly string _categoryName;

            public CustomConsoleLogger(string categoryName)
            {
                _categoryName = categoryName;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (!IsEnabled(logLevel))
                {
                    return;
                }
                switch (logLevel)
                {
                    case LogLevel.Information:
                        Console.WriteLine($"[{DateTime.Now.ToString(CultureInfo.InvariantCulture)}] {formatter(state, exception)}");
                        break;
                    case LogLevel.Debug:
                    case LogLevel.Error:
                        Line($"[{DateTime.Now.ToString(CultureInfo.InvariantCulture)}] {formatter(state, exception)}", ConsoleColor.Red);
                        break;
                }
            }

            private void Line(string text, ConsoleColor colour = ConsoleColor.Black)
            {
                Console.ForegroundColor = colour;
                Console.WriteLine(text);
                Console.ResetColor();
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }
    }
}