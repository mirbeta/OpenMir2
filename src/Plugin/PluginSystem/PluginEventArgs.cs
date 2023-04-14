namespace PluginEngine
{
    /// <summary>
    /// TouchSocketEventArgs
    /// </summary>
    public class PluginEventArgs : EventArgs
    {
        /// <summary>
        /// 是否允许操作
        /// </summary>
        public bool IsPermitOperation { get; set; }

        /// <summary>
        /// 是否已处理
        /// </summary>
        public bool Handled { get; set; }
    }
}
