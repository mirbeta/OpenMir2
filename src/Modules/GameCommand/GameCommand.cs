using System.Reflection;
using SystemModule;
using SystemModule.Enums;

namespace CommandSystem
{
    public class GameCommand {
        public CommandAttribute Command { get; private set; }

        private MethodInfo CommandMethod { get; set; }

        private int MethodParameterCount { get; set; }

        /// <summary>
        /// 注册命令
        /// </summary>
        public void Register(CommandAttribute attributes, MethodInfo method) {
            this.Command = attributes;
            this.CommandMethod = method;
            this.MethodParameterCount = method.GetParameters().Length;
        }

        /// <summary>
        /// 处理命令
        /// </summary>
        /// <returns></returns>
        public virtual string Handle(string parameters, IPlayerActor PlayerActor = null) {
            if (IPlayerActor != null) {
#if DEBUG
                PlayerActor.Permission = 10;
                PlayerActor.SysMsg("当前运行调试模式,权限等级：10", MsgColor.Red, MsgType.Hint);
#endif
                if (PlayerActor.Permission < this.Command.PermissionMin)// 检查用户是否有权限来调用命令。
                {
                    return CommandHelp.GameCommandPermissionTooLow;
                }
            }
            switch (MethodParameterCount) {
                case 2: {
                        string[] @params = parameters.Split(' ');
                        return (string)CommandMethod.Invoke(this, new object[] { @params, IPlayerActor });
                    }
                case 1:
                    return (string)CommandMethod.Invoke(this, new object[] { IPlayerActor });
                default:
                    return (string)CommandMethod.Invoke(this, new object[] { null, IPlayerActor });
            }
        }

        /// <summary>
        /// 取可以使用的命令列表
        /// </summary>
        /// <param name="params"></param>
        /// <param name="IPlayerActor"></param>
        /// <returns></returns>
        [ExecuteCommand]
        public virtual string Fallback(string[] @params = null, IPlayerActor PlayerActor = null) {
            return string.Empty;
        }
    }
}