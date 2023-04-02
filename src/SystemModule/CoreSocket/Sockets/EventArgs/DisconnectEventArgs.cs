
namespace SystemModule.CoreSocket
{
    /// <summary>
    /// 断开连接事件参数
    /// </summary>
    public class DisconnectEventArgs : MsgEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manual"></param>
        /// <param name="mes"></param>
        public DisconnectEventArgs(bool manual, string mes) : base(mes)
        {
            Manual = manual;
        }

        /// <summary>
        /// 是否为主动行为。
        /// </summary>
        public bool Manual { get; private set; }
    }
}