using System;
using System.Linq;
using System.Reflection;
using TouchSocket.Core;

namespace SystemModule.Extensions
{
    /// <summary>
    /// TypeExtension
    /// </summary>
    public static class TypeExtension
    {
        #region Type扩展

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetRefOutType(this Type type)
        {
            if (type.IsByRef)
            {
                return type.GetElementType();
            }
            else
            {
                return type;
            }
        }

        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static object GetDefault(this Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }

        /// <summary>
        /// 判断是否为静态类。
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static bool IsStatic(this Type targetType)
        {
            return targetType.IsAbstract && targetType.IsSealed;
        }

        /// <summary>
        /// 判断为结构体
        /// </summary>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static bool IsStruct(this Type targetType)
        {
            if (!targetType.IsPrimitive && !targetType.IsClass && !targetType.IsEnum && targetType.IsValueType)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断该类型是否为可空类型
        /// </summary>
        /// <param name="theType"></param>
        /// <returns></returns>
        public static bool IsNullableType(this Type theType)
        {
            return (theType.IsGenericType && theType.
                GetGenericTypeDefinition().Equals
                    (TouchSocketCoreUtility.nullableType));
        }

        /// <summary>
        /// 判断该类型是否为可空类型
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static bool IsNullableType(this PropertyInfo propertyInfo)
        {
            CustomAttributeData att = propertyInfo.CustomAttributes.FirstOrDefault(a => a.AttributeType.FullName == "System.Runtime.CompilerServices.NullableAttribute");
            if (att != null)
            {
                return true;
            }

            return (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.
                GetGenericTypeDefinition().Equals
                    (TouchSocketCoreUtility.nullableType));
        }

        /// <summary>
        /// 判断该类型是否为可空类型
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <returns></returns>
        public static bool IsNullableType(this FieldInfo fieldInfo)
        {
            CustomAttributeData att = fieldInfo.CustomAttributes.FirstOrDefault(a => a.AttributeType.FullName == "System.Runtime.CompilerServices.NullableAttribute");
            if (att != null)
            {
                return true;
            }

            return (fieldInfo.FieldType.IsGenericType && fieldInfo.FieldType.
                GetGenericTypeDefinition().Equals
                    (TouchSocketCoreUtility.nullableType));
        }

        /// <summary>
        /// 判断该类型是否为值元组类型
        /// </summary>
        /// <param name="theType"></param>
        /// <returns></returns>
        public static bool IsValueTuple(this Type theType)
        {
            return theType.IsValueType &&
                   theType.IsGenericType &&
                   theType.FullName.StartsWith("System.ValueTuple");
        }

        #endregion Type扩展
    }
}