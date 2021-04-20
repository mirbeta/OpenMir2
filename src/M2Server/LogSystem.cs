using mSystemModule;
using System;

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
                if (message.MessageType == MessageType.Success)
                {
                    OutSuccessMessage(message.Message);
                    return;
                }
                OutErrorMessage(message.Message);
                return;
            }
            if (message.MessageType == MessageType.Error)
            {
                _logqueue.Enqueue(message);
                return;
            }
            OutSuccessMessage(message.Message);
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

        public void Error(string message, MessageType messageType, MessageLevel messageLevel = MessageLevel.None)
        {
            _logqueue.Enqueue(new LogInfo()
            {
                Message = message,
                MessageType = messageType,
                MessageLevel = messageLevel
            });
        }

        private void OutSuccessMessage(string Msg, MessageType messageType = MessageType.Success)
        {
            Console.WriteLine('[' + DateTime.Now.ToString() + "] " + Msg);
        }

        /// <summary>
        /// 显示错误的信息
        /// </summary>
        /// <param name="message"></param>
        private void OutErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }

    public class LogInfo
    {
        public string Message;
        public MessageType MessageType;
        public MessageLevel MessageLevel;
    }
}