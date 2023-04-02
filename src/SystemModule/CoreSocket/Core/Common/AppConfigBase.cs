using System;
using System.Collections.Generic;
using System.IO;

namespace SystemModule.CoreSocket;

/// <summary>
/// 运行配置类
/// </summary>
public abstract class AppConfigBase
{
    private readonly string m_fullPath;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="fullPath"></param>
    public AppConfigBase(string fullPath)
    {
        if (string.IsNullOrEmpty(fullPath))
        {
            throw new ArgumentException($"“{nameof(fullPath)}”不能为 null 或空。", nameof(fullPath));
        }

        m_fullPath = fullPath;
    }

    /// <summary>
    /// 保存配置
    /// </summary>
    /// <param name="overwrite"></param>
    /// <param name="msg"></param>
    /// <returns></returns>
    public bool Save(bool overwrite, out string msg)
    {
        if (overwrite == false && File.Exists(m_fullPath))
        {
            msg = null;
            return true;
        }
        try
        {
            //File.WriteAllText(m_fullPath, this.ToJson());
            msg = null;
            return true;
        }
        catch (Exception ex)
        {
            msg = ex.Message;
            return false;
        }
    }

    /// <summary>
    /// 加载配置
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public bool Load(out string msg)
    {
        try
        {
            if (!File.Exists(m_fullPath))
            {
                Save(false, out _);
            }
            /*var obj = File.ReadAllText(m_fullPath).FromJson(GetType());
            var ps = GetType().GetProperties();
            foreach (var item in ps)
            {
                item.SetValue(this, item.GetValue(obj));
            }*/
            msg = null;
            return true;
        }
        catch (Exception ex)
        {
            msg = ex.Message;
            return false;
        }
    }

    /// <summary>
    /// 获取默认配置。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetDefault<T>() where T : AppConfigBase, new()
    {
        Type type = typeof(T);
        if (list.TryGetValue(type, out object value))
        {
            return (T)value;
        }
        T _default = ((T)Activator.CreateInstance(typeof(T)));
        _default.Load(out _);
        list.Add(type, _default);
        return _default;
    }

    private static readonly Dictionary<Type, object> list = new Dictionary<Type, object>();

    /// <summary>
    /// 获取默认配置，每次调用该方法时，都会重新加载配置。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetNewDefault<T>() where T : AppConfigBase, new()
    {
        T _default = ((T)Activator.CreateInstance(typeof(T)));
        _default.Load(out _);
        if (list.ContainsKey(_default.GetType()))
        {
            list[_default.GetType()] = _default;
        }
        else
        {
            list.Add(_default.GetType(), _default);
        }
        return _default;
    }
}