namespace SystemModule.CoreSocket
{
    /// <summary>
    /// 具有发送动作的基类。
    /// </summary>
    public interface ISenderBase
    {
        /// <summary>
        /// 表示对象能否顺利执行发送操作。
        /// <para>由于高并发，当该值为True时，也不一定完全能执行。</para>
        /// </summary>
        bool CanSend { get; }
    }
}