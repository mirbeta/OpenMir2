using SystemModule.CoreSocket;

namespace SystemModule.CoreSocket
{
    /// <summary>
    /// 消息事件
    /// </summary>
    public class MsgEventArgs : TouchSocketEventArgs
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="mes"></param>
        public MsgEventArgs(string mes)
        {
            Message = mes;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MsgEventArgs()
        {
        }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }
}