using System;

namespace SystemModule.Dependency.Attribute
{
    /// <summary>
    /// 指定依赖类型。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DependencyTypeAttribute : System.Attribute
    {
        /// <summary>
        /// 初始化一个依赖类型。当确定某个类型仅以某种特定方式注入时，可以过滤不必要的注入操作，以提高效率。
        /// </summary>
        /// <param name="type">可以叠加位域</param>
        public DependencyTypeAttribute(DependencyType type)
        {
            Type = type;
        }

        /// <summary>
        /// 支持类型。
        /// </summary>
        public DependencyType Type { get; }
    }

    /// <summary>
    /// 依赖注入类型。
    /// </summary>
    public enum DependencyType
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        Constructor = 1,

        /// <summary>
        /// 属性
        /// </summary>
        Property = 2,

        /// <summary>
        /// 方法
        /// </summary>
        Method = 4
    }

    /// <summary>
    /// 指定依赖类型，构造函数，可用于构造函数，属性，方法。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Property | AttributeTargets.Method)]
    public class DependencyInjectAttribute : System.Attribute
    {
        /// <summary>
        /// 初始化一个依赖注入对象。并且指定构造参数。
        /// <para>当创建时也指定参数时，会覆盖该设定。</para>
        /// </summary>
        /// <param name="ps"></param>
        public DependencyInjectAttribute(params object[] ps)
        {
            Ps = ps;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="resolveNullIfNoRegistered"></param>
        public DependencyInjectAttribute(bool resolveNullIfNoRegistered)
        {
            ResolveNullIfNoRegistered = resolveNullIfNoRegistered;
        }

        /// <summary>
        /// 构造参数
        /// </summary>
        public object[] Ps { get; }

        /// <summary>
        /// 如果没有注册则返回为空
        /// </summary>
        public bool ResolveNullIfNoRegistered { get; set; }
    }

    /// <summary>
    /// 参数，属性指定性注入。
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public class DependencyParamterInjectAttribute : DependencyInjectAttribute
    {
        /// <summary>
        /// 参数，属性指定性注入。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ps"></param>
        public DependencyParamterInjectAttribute(string key, params object[] ps) : base(ps)
        {
            Key = key;
        }

        /// <summary>
        /// 类型，参数，属性指定性注入。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="ps"></param>
        public DependencyParamterInjectAttribute(Type type, string key, params object[] ps) : base(ps)
        {
            Key = key;
            Type = type;
        }

        /// <summary>
        /// 类型，参数，属性指定性注入。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ps"></param>
        public DependencyParamterInjectAttribute(Type type, params object[] ps) : base(ps)
        {
            Key = string.Empty;
            Type = type;
        }

        /// <summary>
        /// 注入类型
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="resolveNullIfNoRegistered"></param>
        public DependencyParamterInjectAttribute(bool resolveNullIfNoRegistered)
        {
            ResolveNullIfNoRegistered = resolveNullIfNoRegistered;
        }

        /// <summary>
        /// 指定键。
        /// </summary>
        public string Key { get; }
    }
}