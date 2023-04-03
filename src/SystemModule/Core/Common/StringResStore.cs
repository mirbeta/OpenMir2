using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using SystemModule.Extensions;

namespace SystemModule.CoreSocket
{
    /// <summary>
    /// 字符串资源字典
    /// </summary>
    public static class StringResStore
    {
        private static readonly ConcurrentDictionary<Enum, string> m_cache = new ConcurrentDictionary<Enum, string>();

        /// <summary>
        /// 获取资源字符
        /// </summary>
        /// <param name="enum"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum @enum, params object[] objs)
        {
            if (m_cache.TryGetValue(@enum, out string str))
            {
                if (string.IsNullOrEmpty(str))
                {
                    return @enum.ToString();
                }
                else
                {
                    return str.Format(objs);
                }
            }

            if (@enum.GetAttribute<DescriptionAttribute>() is DescriptionAttribute description)
            {
                string res = description.Description;
                m_cache.TryAdd(@enum, res);
                if (!string.IsNullOrEmpty(res))
                {
                    if (objs.Length > 0)
                    {
                        return res.Format(objs);
                    }
                    else
                    {
                        return res;
                    }
                }
            }
            return @enum.ToString();
        }
    }
}