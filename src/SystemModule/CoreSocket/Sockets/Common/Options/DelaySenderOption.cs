namespace TouchSocket.Sockets
{
    /// <summary>
    /// DelaySenderOption
    /// </summary>
    public class DelaySenderOption
    {
        /// <summary>
        /// 延迟队列最大尺寸，默认1024*1024*10字节。
        /// </summary>
        public int QueueLength { get; set; } = 1024 * 1024 * 10;

        /// <summary>
        /// 延迟包最大尺寸，默认1024*512字节。
        /// </summary>
        public int DelayLength { get; set; } = 1024 * 512;
    }
}