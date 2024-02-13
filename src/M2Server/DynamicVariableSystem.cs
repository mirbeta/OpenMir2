using SystemModule.SubSystem;

namespace M2Server
{
    internal class DynamicVariableSystem : IServerDynamicSystem
    {
        /// <summary>
        /// 玩家的变量P
        /// </summary>
        private int[] MNVal { get; set; }

        /// <summary>
        /// 玩家的变量M
        /// </summary>
        private int[] MNMval { get; set; }

        /// <summary>
        /// 玩家的变量D
        /// </summary>
        private int[] MDyVal { get; set; }

        /// <summary>
        /// 玩家的变量
        /// </summary>
        private string[] MNSval { get; set; }

        /// <summary>
        /// 人物变量  N
        /// </summary>
        private int[] MNInteger { get; set; }

        /// <summary>
        /// 人物变量  S
        /// </summary>
        private string[] MSString { get; set; }

        /// <summary>
        /// 服务器变量 W 0-20 个人服务器数值变量，不可保存，不可操作
        /// </summary>
        private string[] ServerStrVal { get; set; }

        /// <summary>
        /// E 0-20 个人服务器字符串变量，不可保存，不可操作
        /// </summary>
        private int[] ServerIntVal { get; set; }

        public int[] GetIntVariable(int actorId, int variableId, VariableType variableType)
        {
            throw new NotImplementedException();
        }

        public string[] GetStrVariable(int actorId, int variableId, VariableType variableType)
        {
            throw new NotImplementedException();
        }
    }
}