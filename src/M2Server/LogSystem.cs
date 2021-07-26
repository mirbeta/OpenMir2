using System;
using SystemModule.Common;

namespace M2Server
{
    public class LogSystem
    {
        private readonly AsynQueue<LogInfo> _logqueue = null;

        public LogSystem()
        {
            _logqueue = new AsynQueue<LogInfo>();
            _logqueue.ProcessItemFunction += _logqueue_ProcessItemFunction;
        }

        private void _logqueue_ProcessItemFunction(LogInfo message)
        {
            if (M2Share.boStartReady)
            {
                if (message.MessageLevel == MessageLevel.Low)
                {
                    _logqueue.Enqueue(message);
                    return;
                }
                OutMessage(message.Message, message.MessageType, consoleColor: message.MessageColor);
                return;
            }
            _logqueue.Enqueue(message);
        }

        public void LogInfo(string message, MessageType messageType, MessageLevel messageLevel = MessageLevel.None)
        {
            _logqueue.Enqueue(new LogInfo()
            {
                Message = message,
                MessageType = messageType,
                MessageLevel = messageLevel
            });
        }

        public void LogInfo(string message, MessageType messageType, MessageLevel messageLevel = MessageLevel.None, ConsoleColor messageColor = ConsoleColor.Black)
        {
            _logqueue.Enqueue(new LogInfo()
            {
                Message = message,
                MessageType = messageType,
                MessageLevel = messageLevel,
                MessageColor = messageColor
            });
        }

        public void Error(string message, MessageType messageType, MessageLevel messageLevel = MessageLevel.None)
        {
            _logqueue.Enqueue(new LogInfo()
            {
                Message = message,
                MessageType = messageType,
                MessageLevel = messageLevel
            });
        }

        private void OutMessage(string Msg, MessageType messageType = MessageType.Success, ConsoleColor consoleColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine('[' + DateTime.Now.ToString() + "] " + Msg);
            Console.ResetColor();
        }
    }

    public class LogInfo
    {
        public string Message;
        public MessageType MessageType;
        public MessageLevel MessageLevel;
        public ConsoleColor MessageColor;
    }
}