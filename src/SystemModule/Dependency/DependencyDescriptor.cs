using System;

namespace SystemModule.Dependency
{
    /// <summary>
    /// 注入依赖对象
    /// </summary>
    public class DependencyDescriptor
    {
        /// <summary>
        /// 初始化一个单例实例。
        /// </summary>
        /// <param name="fromType"></param>
        /// <param name="instance"></param>
        public DependencyDescriptor(Type fromType, object instance)
        {
            FromType = fromType;
            ToInstance = instance;
            Lifetime = Lifetime.Singleton;
            ToType = instance.GetType();
        }

        /// <summary>
        /// 初始化一个完整的服务注册
        /// </summary>
        /// <param name="fromType"></param>
        /// <param name="toType"></param>
        /// <param name="lifetime"></param>
        public DependencyDescriptor(Type fromType, Type toType, Lifetime lifetime)
        {
            FromType = fromType;
            Lifetime = lifetime;
            ToType = toType;
        }

        /// <summary>
        /// 初始化一个简单的的服务描述
        /// </summary>
        /// <param name="fromType"></param>
        public DependencyDescriptor(Type fromType)
        {
            FromType = fromType;
        }

        /// <summary>
        /// 实例化工厂委托
        /// </summary>
        public Func<IContainer, object> ImplementationFactory { get; set; }

        /// <summary>
        /// 实例类型
        /// </summary>
        public Type ToType { get; }

        /// <summary>
        /// 实例
        /// </summary>
        public object ToInstance { get; set; }

        /// <summary>
        /// 生命周期
        /// </summary>
        public Lifetime Lifetime { get; }

        /// <summary>
        /// 注册类型
        /// </summary>
        public Type FromType { get; }
    }
}