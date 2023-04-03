namespace PluginSystem
{
    /// <summary>
    /// 标识该接口应当还会触发异步接口。
    /// 异步接口方法的返回值应该为Task，且必须以Async结尾。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class AsyncRaiserAttribute : Attribute
    {
    }
}