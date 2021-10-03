using System;
using System.Collections.Concurrent;
using System.Threading;
using SystemModule;

namespace GameSvr
{
    public class MirLog
    {
        private readonly ConcurrentQueue<LogInfo> _logqueue = null;
        private readonly ConcurrentQueue<LogInfo> _debugLog = null;
        private readonly ConcurrentQueue<LogInfo> _chatLog = null;
        private Timer _logTime;

        public MirLog()
        {
            _logqueue = new ConcurrentQueue<LogInfo>();
            _debugLog = new ConcurrentQueue<LogInfo>();
            _chatLog = new ConcurrentQueue<LogInfo>();
            _logTime = new Timer(OutLog, null, 0, 1000);
        }

        private void OutLog(object obj)
        {
            while (!_logqueue.IsEmpty)
            {
                LogInfo message;

                if (!_logqueue.TryDequeue(out message)) continue;

                OutMessage(message.Message, message.MessageType, consoleColor: message.MessageColor);
            }

            while (!_debugLog.IsEmpty)
            {
                LogInfo message;

                if (!_debugLog.TryDequeue(out message)) continue;

                OutMessage(message.Message, message.MessageType, consoleColor: message.MessageColor);

            }

            while (!_chatLog.IsEmpty)
            {
                LogInfo message;

                if (!_chatLog.TryDequeue(out message)) continue;

                OutMessage(message.Message, message.MessageType, consoleColor: message.MessageColor);
            }
        }

        //private void _logqueue_ProcessItemFunction(LogInfo message)
        //{
        //    if (M2Share.boStartReady)
        //    {
        //        if (message.MessageLevel == MessageLevel.Low)
        //        {
        //            _logqueue.Enqueue(message);
        //            return;
        //        }
        //        return;
        //    }
        //    _logqueue.Enqueue(message);
        //}

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