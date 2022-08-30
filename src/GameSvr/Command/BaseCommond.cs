using GameSvr.Player;
using System.Reflection;
using SystemModule.Data;

namespace GameSvr.Command
{
    public class BaseCommond
    {
        public GameCommandAttribute GameCommand { get; private set; }

        private MethodInfo CommandMethod { get; set; }

        private int MethodParameterCount { get; set; }

        /// <summary>
        /// 注册命令
        /// </summary>
        public void Register(GameCommandAttribute attributes, MethodInfo method)
        {
            this.GameCommand = attributes;
            this.CommandMethod = method;
            this.MethodParameterCount = method.GetParameters().Length;
        }

        /// <summary>
        /// 处理命令
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="playObject"></param>
        /// <returns></returns>
        public virtual string Handle(string parameters, TPlayObject playObject = null)
        {
            if (playObject != null)
            {
#if DEBUG
                playObject.m_btPermission = 10;
                playObject.SysMsg("当前运行调试模式,权限等级：10", MsgColor.Red, MsgType.Hint);
#endif
                if (playObject.m_btPermission < this.GameCommand.nPermissionMin)// 检查用户是否有权限来调用命令。
                {
                    return GameCommandConst.g_sGameCommandPermissionTooLow;
                }
            }
            switch (MethodParameterCount)
            {
                case 2:
                    {
                        var @params = parameters.Split(' ');
                        return (string)CommandMethod.Invoke(this, new object[] { @params, playObject });
                    }
                case 1:
                    return (string)CommandMethod.Invoke(this, new object[] { playObject });
                default:
                    return (string)CommandMethod.Invoke(this, new object[] { null, playObject });
            }
        }

        /// <summary>
        /// 取可以使用的命令列表
        /// </summary>
        /// <param name="params"></param>
        /// <param name="PlayObject"></param>
        /// <returns></returns>
        [DefaultCommand]
        public virtual string Fallback(string[] @params = null, TPlayObject PlayObject = null)
        {
            return string.Empty;
        }
    }
}