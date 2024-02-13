namespace SystemModule.SubSystem
{
    /// <summary>
    /// 服务端变量系统
    /// 用于管理玩家变量和系统脚本变量
    /// </summary>
    public interface IServerDynamicSystem
    {
        /// <summary>
        /// 获取整形变量
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="variableType"></param>
        /// <returns></returns>
        int[] GetIntVariable(int actorId, int variableId, VariableType variableType);

        /// <summary>
        /// 获取字符串变量
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="variableType"></param>
        /// <returns></returns>
        string[] GetStrVariable(int actorId, int variableId, VariableType variableType);

    }

    public enum VariableType
    {
        /// <summary>
        /// 变量P
        /// </summary>
        PlayerP,
        /// <summary>
        /// 变量M
        /// </summary>
        PlayerM,
        /// <summary>
        /// 变量N
        /// </summary>
        PlayerN,
        /// <summary>
        /// 变量D
        /// </summary>
        PlayerD,
        /// <summary>
        /// 变量S
        /// </summary>
        PlayerS,
        /// <summary>
        /// 服务器变量 W 0-20 个人服务器数值变量，不可保存，不可操作
        /// </summary>
        ServerW,
        /// <summary>
        /// E 0-20 个人服务器字符串变量，不可保存，不可操作
        /// </summary>
        ServerE
    }
}
