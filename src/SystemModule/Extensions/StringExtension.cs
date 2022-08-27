using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemModule.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// 字符串转Byte数组
        /// </summary>
        /// <returns></returns>
        public static byte[] ToByte(this string source, int size)
        {
            var strBuff = HUtil32.StringToByteAry(source, out int strLen);
            strBuff[0] = (byte)strLen;
            Array.Resize(ref strBuff, size);
            return strBuff;
        }

        public static byte[] ToByte(this string source, int size, int miniLen)
        {
            var strBuff = HUtil32.StringToByteAry(source, out int strLen);
            strBuff[0] = (byte)strLen;
            if (size < miniLen)
            {
                size = miniLen;
            }
            Array.Resize(ref strBuff, size);
            return strBuff;
        }

        public static string[] ToStringArrT<T>(IEnumerable<T> collection, Func<T, object> converter)
        {
            var strArr = new string[collection.Count()];
            var colEnum = collection.GetEnumerator();
            var i = 0;
            while (colEnum.MoveNext())
            {
                var cur = colEnum.Current;
                if (!Equals(cur, default(T)))
                {
                    strArr[i++] = (converter != null ? converter(cur) : cur).ToString();
                }
            }
            return strArr;
        }

        public static string[] ToStringArrT<T>(IEnumerable<T> collection)
        {
            return ToStringArrT(collection, null);
        }

        /// <summary>
        /// Returns the string representation of an IEnumerable (all elements, joined by comma)
        /// </summary>
        /// <param name="conj">The conjunction to be used between each elements of the collection</param>
        public static string ToString<T>(this IEnumerable<T> collection, string conj)
        {
            string vals;
            if (collection != null)
            {
                vals = string.Join(conj, ToStringArrT(collection));
            }
            else
                vals = "(null)";

            return vals;
        }
    }
}