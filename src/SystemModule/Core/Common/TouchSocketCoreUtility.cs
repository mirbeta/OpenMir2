using System;
using System.Collections;

namespace SystemModule.Core.Common
{
    /// <summary>
    /// 常量
    /// </summary>
    public class TouchSocketCoreUtility
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public static readonly Type stringType = typeof(string);
        public static readonly Type byteType = typeof(byte);
        public static readonly Type sbyteType = typeof(sbyte);
        public static readonly Type shortType = typeof(short);
        public static readonly Type objType = typeof(object);
        public static readonly Type ushortType = typeof(ushort);
        public static readonly Type intType = typeof(int);
        public static readonly Type uintType = typeof(uint);
        public static readonly Type boolType = typeof(bool);
        public static readonly Type charType = typeof(char);
        public static readonly Type longType = typeof(long);
        public static readonly Type ulongType = typeof(ulong);
        public static readonly Type floatType = typeof(float);
        public static readonly Type doubleType = typeof(double);
        public static readonly Type decimalType = typeof(decimal);
        public static readonly Type dateTimeType = typeof(DateTime);
        public static readonly Type bytesType = typeof(byte[]);
        public static readonly Type dicType = typeof(IDictionary);
        public static readonly Type iEnumerableType = typeof(IEnumerable);
        public static readonly Type arrayType = typeof(Array);
        public static readonly Type listType = typeof(IList);
        public static readonly Type nullableType = typeof(Nullable<>);

        public static readonly byte[] ZeroBytes = new byte[0];
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    }
}