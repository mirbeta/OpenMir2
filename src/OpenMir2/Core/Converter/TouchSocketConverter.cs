using System;
using System.Collections.Generic;

namespace SystemModule.Core.Converter
{
    /// <summary>
    /// 转换器
    /// </summary>
    public class TouchSocketConverter<TSource>
    {
        private readonly List<IConverter<TSource>> m_converters = new List<IConverter<TSource>>();

        /// <summary>
        /// 添加插件
        /// </summary>
        /// <param name="converter">插件</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Add(IConverter<TSource> converter)
        {
            if (converter == null)
            {
                throw new ArgumentNullException();
            }
            foreach (IConverter<TSource> item in m_converters)
            {
                if (item.GetType() == converter.GetType())
                {
                    return;
                }
            }

            m_converters.Add(converter);

            m_converters.Sort(delegate (IConverter<TSource> x, IConverter<TSource> y)
            {
                if (x.Order == y.Order)
                {
                    return 0;
                }
                else if (x.Order > y.Order)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            });
        }

        /// <summary>
        /// 清除所有转化器
        /// </summary>
        public void Clear()
        {
            m_converters.Clear();
        }

        /// <summary>
        /// 将源数据转换目标类型对象
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public object ConvertFrom(TSource source, Type targetType)
        {
            object result;
            foreach (IConverter<TSource> item in m_converters)
            {
                if (item.TryConvertFrom(source, targetType, out result))
                {
                    return result;
                }
            }

            throw new Exception($"{source}无法转换为{targetType}类型。");
        }

        /// <summary>
        /// 将目标类型对象转换源数据
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public TSource ConvertTo(object target)
        {
            foreach (IConverter<TSource> item in m_converters)
            {
                if (item.TryConvertTo(target, out TSource source))
                {
                    return source;
                }
            }

            throw new Exception($"{target}无法转换为{typeof(TSource)}类型。");
        }

        /// <summary>
        /// 移除插件
        /// </summary>
        /// <param name="converter"></param>
        public void Remove(IConverter<TSource> converter)
        {
            if (converter == null)
            {
                throw new ArgumentNullException();
            }
            m_converters.Remove(converter);
        }

        /// <summary>
        /// 移除插件
        /// </summary>
        /// <param name="type"></param>
        public void Remove(Type type)
        {
            for (int i = m_converters.Count - 1; i >= 0; i--)
            {
                IConverter<TSource> plugin = m_converters[i];
                if (plugin.GetType() == type)
                {
                    m_converters.RemoveAt(i);
                }
            }
        }
    }
}