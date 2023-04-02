//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TouchSocket.Core
//{
//    /// <summary>
//    /// 异常助手
//    /// </summary>
//    public static class ThrowHelper
//    {
//        private static readonly ConcurrentDictionary<Enum, Func<string, Exception, Exception>> m_pairs =
//            new ConcurrentDictionary<Enum, Func<string, Exception, Exception>>();

//        /// <summary>
//        /// 添加抛出规则。
//        /// </summary>
//        /// <param name="enum"></param>
//        /// <param name="func"></param>
//        public static void Add(Enum @enum,Func<string,Exception,Exception> func)
//        {
//            if (@enum is null)
//            {
//                throw new ArgumentNullException(nameof(@enum));
//            }

//            if (func is null)
//            {
//                throw new ArgumentNullException(nameof(func));
//            }

//            m_pairs.TryRemove(@enum,out _);
//            m_pairs.TryAdd(@enum,func);
//        }

//        public static Exception Throw(Enum @enum,string msg,Exception exception)
//        {
//        }

//    }
//}