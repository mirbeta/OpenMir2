using GameSvr.CommandSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SystemModule;

namespace GameSvr
{
    public class BaseCommond
    {
        protected GameCommandAttribute Command { get; private set; }

        private readonly Dictionary<string, (CommandAttribute, MethodInfo)> _commands = new Dictionary<string, (CommandAttribute, MethodInfo)>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 注册命令
        /// </summary>
        /// <param name="attributes"></param>
        public void Register(GameCommandAttribute attributes)
        {
            this.Command = attributes;
            this.RegisterDefaultCommand();
            this.RegisterCommands();
        }

        private void RegisterCommands()
        {
            foreach (var method in this.GetType().GetMethods())
            {
                var attributes = method.GetCustomAttributes(typeof(CommandAttribute), true);
                if (attributes.Length == 0) continue;

                var attribute = (CommandAttribute)attributes[0];
                if (attribute is DefaultCommand) continue;

                if (_commands.ContainsKey(attribute.Name))
                {
                    M2Share.ErrorMessage($"命令名称重复: {attribute.Name}");
                }
                else
                {
                    _commands.Add(attribute.Name, (attribute, method));
                }
            }
        }

        private void RegisterDefaultCommand()
        {
            foreach (var method in this.GetType().GetMethods())
            {
                var attributes = method.GetCustomAttributes(typeof(DefaultCommand), true);
                if (attributes.Length == 0) continue;
                if (method.Name == "fallback") continue;
                this._commands.Add(method.Name, (new DefaultCommand(this.Command.nPermissionMin), method));
                return;
            }
            this._commands.Add("Fallback", (new DefaultCommand(this.Command.nPermissionMin), this.GetType().GetMethod("Fallback")));
        }

        /// <summary>
        /// 处理命令
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="playObject"></param>
        /// <returns></returns>
        public virtual string Handle(string command, string parameters, TPlayObject playObject = null)
        {
            if (playObject != null)
            {
#if DEBUG
                playObject.m_btPermission = 10;
                playObject.SysMsg("当前运行调试模式,权限等级：10", MsgColor.Red, MsgType.Hint);
#endif
                if (playObject.m_btPermission < this.Command.nPermissionMin)// 检查用户是否有权限来调用命令。
                {
                    return M2Share.g_sGameCommandPermissionTooLow; //权限不足
                }
            }
            string[] @params = null;
            CommandAttribute target = null;
            if (parameters == string.Empty)
            {
                target = this.GetDefaultSubcommand();
            }
            else
            {
                @params = parameters.Split(' ');
                target = this.GetSubcommand(command) ?? this.GetDefaultSubcommand();
                if (target != this.GetDefaultSubcommand())
                {
                    @params = @params.Skip(1).ToArray();
                }
            }
            string result;
            if (!_commands.ContainsKey(target.Name))
            {
                return string.Empty;
            }
            var targetCommand = _commands[target.Name].Item2;
            if (targetCommand == null)
            {
                return string.Empty;
            }
            var methodsParamsCount = targetCommand.GetParameters().Length;//查看命令目标所需要的参数个数
            if (methodsParamsCount == 2) //默认参数为当前对象，即：PlayObject
            {
                if (@params == null)
                {
                    return Command.CommandHelp;
                }
                if (@params.Length < methodsParamsCount - 1) //参数数量小于实际需要传递的数量
                {
                    return Command.CommandHelp;
                }
                result = (string)targetCommand.Invoke(this, new object[] { @params, playObject });
            }
            else if (methodsParamsCount == 1)
            {
                result = (string)targetCommand.Invoke(this, new object[] { playObject });
            }
            else
            {
                result = (string)targetCommand.Invoke(this, new object[] { null, playObject });
            }
            return result;
        }

        public string GetHelp(string command)
        {
            foreach (var pair in this._commands)
            {
                if (command != pair.Key) continue;
                return pair.Value.Item1.Help;
            }
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
            foreach (var pair in this._commands)
            {
                if (pair.Key.Trim() == string.Empty) continue;
                if (PlayObject != null && pair.Value.Item1.MinUserLevel > PlayObject.m_btPermission) continue;
                output += pair.Key + ", ";
            }
            return output.Substring(0, output.Length - 2) + ".";
        }

        private CommandAttribute GetDefaultSubcommand()
        {
            return this._commands["fallback"].Item1;
        }

        private CommandAttribute GetSubcommand(string name)
        {
            return this._commands.ContainsKey(name) ? _commands[name].Item1 : null;
        }
    }
}