using System;
using SystemModule.Extensions;

namespace SystemModule.CoreSocket;

/// <summary>
/// String类型数据转换器
/// </summary>
public class StringConverter : TouchSocketConverter<string>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public StringConverter()
    {
        Add(new StringToPrimitiveConverter());
        Add(new JsonStringToClassConverter());
    }
}

/// <summary>
/// String值转换为基础类型。
/// </summary>
public class StringToPrimitiveConverter : IConverter<string>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="targetType"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool TryConvertFrom(string source, Type targetType, out object target)
    {
        if (targetType.IsPrimitive || targetType == TouchSocketCoreUtility.stringType)
        {
            return StringExtension.TryParseToType(source, targetType, out target);
        }
        target = default;
        return false;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="target"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public bool TryConvertTo(object target, out string source)
    {
        if (target != null)
        {
            Type type = target.GetType();
            if (type.IsPrimitive || type == TouchSocketCoreUtility.stringType)
            {
                source = target.ToString();
                return true;
            }
        }

        source = null;
        return false;
    }
}

/// <summary>
/// Json字符串转到对应类
/// </summary>
public class JsonStringToClassConverter : IConverter<string>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="targetType"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool TryConvertFrom(string source, Type targetType, out object target)
    {
        try
        {
            target = System.Text.Json.JsonSerializer.Deserialize<Type>(source);
            return true;
        }
        catch
        {
            target = default;
            return false;
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="target"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public bool TryConvertTo(object target, out string source)
    {
        try
        {
            source = System.Text.Json.JsonSerializer.Serialize(target);
            return true;
        }
        catch (Exception)
        {
            source = null;
            return false;
        }
    }
}