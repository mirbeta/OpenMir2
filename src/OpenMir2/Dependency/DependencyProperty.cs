using System;
using System.Diagnostics;

namespace SystemModule.Dependency
{
    /// <summary>
    /// IDependencyProperty
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface IDependencyProperty<out TValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        TValue DefauleValue { get; }

        /// <summary>
        /// 属性名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 所属类型
        /// </summary>
        Type Owner { get; }
    }

    /// <summary>
    /// 依赖项属性
    /// </summary>
    [DebuggerDisplay("Name={Name},Type={ValueType}")]
    public class DependencyProperty<TValue> : IDependencyProperty<TValue>
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        protected string m_name;

        /// <summary>
        /// 所属类型
        /// </summary>
        protected Type m_owner;

        private TValue m_value;

        /// <summary>
        /// 依赖项属性
        /// </summary>
        protected DependencyProperty()
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public TValue DefauleValue => m_value;

        /// <summary>
        ///<inheritdoc/>
        /// </summary>
        public string Name => m_name;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Type Owner => m_owner;

        /// <summary>
        /// 注册依赖项属性。
        /// <para>依赖属性的默认值，可能会应用于所有的<see cref="IDependencyObject"/></para>
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="owner"></param>
        /// <param name="value">依赖项属性值，一般该值应该是值类型，因为它可能会被用于多个依赖对象。</param>
        /// <returns></returns>
        public static DependencyProperty<TValue> Register(string propertyName, Type owner, TValue value)
        {
            DependencyProperty<TValue> dp = new DependencyProperty<TValue>
            {
                m_name = propertyName,
                m_owner = owner,
                m_value = value
            };
            return dp;
        }
    }
}