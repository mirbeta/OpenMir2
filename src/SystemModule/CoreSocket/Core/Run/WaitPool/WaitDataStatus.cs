namespace TouchSocket.Core;

/// <summary>
/// 等待数据状态
/// </summary>
public enum WaitDataStatus : byte
{
    /// <summary>
    /// 默认
    /// </summary>
    Default,

    /// <summary>
    /// 收到信号运行
    /// </summary>
    SetRunning,

    /// <summary>
    /// 超时
    /// </summary>
    Overtime,

    /// <summary>
    /// 已取消
    /// </summary>
    Canceled,

    /// <summary>
    /// 已释放
    /// </summary>
    Disposed
}