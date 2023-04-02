namespace SystemModule.CoreSocket;

/// <summary>
/// 结果类型
/// </summary>
public enum ResultCode
{
    /// <summary>
    /// 默认
    /// </summary>
    Default,

    /// <summary>
    /// 错误
    /// </summary>
    Error,

    /// <summary>
    /// 异常
    /// </summary>
    Exception,

    /// <summary>
    /// 成功
    /// </summary>
    Success,

    /// <summary>
    /// 失败
    /// </summary>
    Fail,

    /// <summary>
    /// 操作超时
    /// </summary>
    Overtime,

    /// <summary>
    /// 操作取消
    /// </summary>
    Canceled
}