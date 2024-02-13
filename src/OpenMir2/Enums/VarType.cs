namespace OpenMir2.Enums
{
    /// <summary>
    /// 脚本变量类型
    /// </summary>
    public enum VarType : byte
    {
        /// <summary>
        /// 空
        /// </summary>
        None,
        /// <summary>
        /// 整形
        /// </summary>
        Integer,
        /// <summary>
        /// 字符串
        /// </summary>
        String
    }

    public class VarInfo
    {
        public VarType VarType;
        public VarAttr VarAttr;
    }

    public enum VarAttr
    {
        aNone,
        aFixStr,
        aDynamic,
        aConst
    }
}