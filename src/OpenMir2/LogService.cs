using Serilog;
using System;

namespace OpenMir2
{
    public static class LogService
    {
        public static ILogger Logger;

        public static void Info(string message)
        {
            Logger.Information(message);
        }

        public static void Info(string message, params object[] args)
        {
            Logger.Information(message, args);
        }

        public static void Error(string message)
        {
            Logger.Error(message);
        }

        public static void Error(string message, Exception ex)
        {
            Logger.Error(ex, message);
        }

        public static void Error(Exception ex)
        {
            Logger.Error(ex.Message, ex);
        }

        public static void Debug(string message)
        {
            Logger.Debug(message);
        }

        public static void Debug(string message, params object[] args)
        {
            Logger.Debug(message, args);
        }

        public static void Warn(string message)
        {
            Logger.Warning(message);
        }

        public static void Fatal(string message)
        {
            Logger.Fatal(message);
        }
    }
}