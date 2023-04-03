using System;

namespace SystemModule.CoreSocket
{
    /// <summary>
    /// 依赖属性数据验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DataValidationAttribute : Attribute
    {
    }
}