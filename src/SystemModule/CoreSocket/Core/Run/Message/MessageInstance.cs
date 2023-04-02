using System;
using System.Reflection;

namespace TouchSocket.Core;

/// <summary>
/// MessageInstance
/// </summary>
public class MessageInstance : Method
{
    private readonly WeakReference<object> weakReference;

    /// <summary>
    /// MessageInstance
    /// </summary>
    /// <param name="method"></param>
    /// <param name="messageObject"></param>
    public MessageInstance(MethodInfo method, object messageObject) : base(method)
    {
        weakReference = new WeakReference<object>(messageObject);
    }

    /// <summary>
    /// 承载消息的实体
    /// </summary>
    public object MessageObject
    {
        get
        {
            weakReference.TryGetTarget(out object target);
            return target;
        }
    }

    /// <summary>
    /// 弱引用。
    /// </summary>
    public WeakReference<object> WeakReference => weakReference;
}