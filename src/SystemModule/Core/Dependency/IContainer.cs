using System.Collections.Generic;

namespace SystemModule.CoreSocket
{
    /// <summary>
    /// 注入容器接口
    /// </summary>
    public interface IContainer : IContainerProvider, IEnumerable<DependencyDescriptor>
    {
        /// <summary>
        /// 添加类型描述符。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="descriptor"></param>
        void Register(DependencyDescriptor descriptor, string key = "");

        /// <summary>
        /// 移除注册信息。
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        void Unregister(DependencyDescriptor descriptor, string key = "");
    }
}