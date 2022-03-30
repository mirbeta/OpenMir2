using GameSvr.CommandSystem;
using System.Reflection;
using SystemModule;

namespace GameSvr
{
    public class BaseCommond
    {
        protected GameCommandAttribute GameCommand { get; private set; }

        protected MethodInfo CommandMethod { get; private set; }

        protected int MethodParameterCount { get; private set; }

        /// <summary>
        /// 注册命令
        /// </summary>
        /// <param name="attributes"></param>
        public void Register(GameCommandAttribute attributes, MethodInfo commond)
        {
            this.GameCommand = attributes;
            this.CommandMethod = commond;
            this.MethodParameterCount = CommandMethod.GetParameters().Length;
            this.RegisterDefaultCommand();
        }

        private void RegisterDefaultCommand()
        {
            foreach (var method in this.GetType().GetMethods())
            {
                var attributes = method.GetCustomAttributes(typeof(DefaultCommand), true);
                if (attributes.Length == 0) continue;
                if (method.Name == "fallback") continue;
                //this._commands.Add(method.Name, (new DefaultCommand(this.CommandInfo.nPermissionMin), method));
                return;
            }
            //this._commands.Add("Fallback", (new DefaultCommand(this.CommandInfo.nPermissionMin), this.GetType().GetMethod("Fallback")));
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
                    return M2Share.g_sGameCommandPermissionTooLow; //权限不足
                }
            }
            string[] @params = parameters.Split(' ');
            string result;
            if (CommandMethod == null)
            {
                return string.Empty;
            }
            if (MethodParameterCount == 2) //默认参数为当前对象，即：PlayObject
            {
                result = (string)CommandMethod.Invoke(this, new object[] { @params, playObject });
            }
            else if (MethodParameterCount == 1)
            {
                result = (string)CommandMethod.Invoke(this, new object[] { playObject });
            }
            else
            {
                result = (string)CommandMethod.Invoke(this, new object[] { null, playObject });
            }
            return result;
        }

        public string GetHelp(string command)
        {
            //foreach (var pair in this._commands)
            //{
            //    if (command != pair.Key) continue;
            //    return pair.Value.Item1.Help;
            //}
            return string.Empty;
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
            var output = "可用的命令: ";
            //foreach (var pair in this._commands)
            //{
            //    if (pair.Key.Trim() == string.Empty) continue;
            //    if (PlayObject != null && pair.Value.Item1.MinUserLevel > PlayObject.m_btPermission) continue;
            //    output += pair.Key + ", ";
            //}
            return output.Substring(0, output.Length - 2) + ".";
        }

        //private CommandAttribute GetDefaultSubcommand()
        //{
        //    return this._commands["Fallback"].Item1;
        //}

        //private CommandAttribute GetSubcommand(string name)
        //{
        //    return this._commands.ContainsKey(name) ? _commands[name].Item1 : null;
        //}
    }
}