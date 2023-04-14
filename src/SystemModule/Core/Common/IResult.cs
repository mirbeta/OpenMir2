using SystemModule.Core.Common.Enum;

namespace SystemModule.Core.Common
{
    /// <summary>
    /// 返回通知接口
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        ResultCode ResultCode { get; }

        /// <summary>
        /// 消息
        /// </summary>
        string Message { get; }
    }
}