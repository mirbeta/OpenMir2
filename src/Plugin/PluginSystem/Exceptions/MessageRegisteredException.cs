using System;

namespace PluginEngine.Exceptions
{
    /// <summary>
    /// 消息已注册
    /// </summary>
    public class MessageRegisteredException : Exception
    {
        /// <summary>
        ///构造函数
        /// </summary>
        /// <param name="mes"></param>
        public MessageRegisteredException(string mes) : base(mes)
        {
        }
    }
}