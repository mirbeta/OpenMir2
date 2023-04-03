namespace SystemModule.CoreSocket
{
    /// <summary>
    /// 规范写端口，提供更多扩展
    /// </summary>
    public interface IWrite
    {
        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        void Write(byte[] buffer, int offset, int length);

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="buffer"></param>
        void Write(byte[] buffer);
    }
}