using System;

namespace SystemModule.CoreSocket
{
    /// <summary>
    /// 注册为消息
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class AppMessageAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppMessageAttribute()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="token"></param>
        public AppMessageAttribute(string token)
        {
            Token = token;
        }

        /// <summary>
        /// 标识
        /// </summary>
        public string Token { get; set; }
    }
}