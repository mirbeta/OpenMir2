using GameSvr.CommandSystem;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameSvr
{
    public class BaseCommond
    {
        protected GameCommandAttribute Attributes { get; private set; }

        private readonly Dictionary<CommandAttribute, MethodInfo> _commands =
            new Dictionary<CommandAttribute, MethodInfo>();

        /// <summary>
        /// 注册命令
        /// </summary>
        /// <param name="attributes"></param>
        public void Register(GameCommandAttribute attributes)
        {
            this.Attributes = attributes;
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

                if (!this._commands.ContainsKey(attribute))
                {
                    this._commands.Add(attribute, method);
                }
                else
                {
                    M2Share.ErrorMessage($"命令名称重复: {attribute.Name}");
                }
            }
        }

        private void RegisterDefaultCommand()
        {
            foreach (var method in this.GetType().GetMethods())
            {
                var attributes = method.GetCustomAttributes(typeof(DefaultCommand), true);
                if (attributes.Length == 0) continue;
                if (method.Name.ToLower() == "fallback") continue;

                this._commands.Add(new DefaultCommand(this.Attributes.nPermissionMin), method);
                return;
            }

            this._commands.Add(new DefaultCommand(this.Attributes.nPermissionMin), this.GetType().GetMethod("Fallback"));
        }

        /// <summary>
        /// 处理命令
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="playObject"></param>
        /// <returns></returns>
        public virtual string Handle(string parameters, TPlayObject playObject = null)
        {
            // 检查用户是否有足够的权限来调用命令。
            playObject.m_btPermission = 10;
            if (playObject != null && this.Attributes.nPermissionMin > playObject.m_btPermission)
            {
                return M2Share.g_sGameCommandPermissionTooLow;//权限不足
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
                target = this.GetSubcommand(@params[0]) ?? this.GetDefaultSubcommand();

                if (target != this.GetDefaultSubcommand())
                    @params = @params.Skip(1).ToArray();
            }
            // 检查用户是否有足够的权限来调用命令
            if (playObject != null && target.MinUserLevel > playObject.m_btPermission)
            {
                return M2Share.g_sGameCommandPermissionTooLow;//权限不足
            }
            string result;
            var methodsParamsCount = this._commands[target].GetParameters().Length;//查看命令目标所需要的参数个数
            if (methodsParamsCount > 1) //默认参数为当前对象，即：PlayObject
            {
                result = (string)this._commands[target].Invoke(this, new object[] { @params, playObject });
            }
            else
            {
                result = (string)this._commands[target].Invoke(this, new object[] { null, playObject });
            }
            return result;
        }

        public string GetHelp(string command)
        {
            foreach (var pair in this._commands)
            {
                if (command != pair.Key.Name) continue;
                return pair.Key.Help;
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
                if (pair.Key.Name.Trim() == string.Empty) continue;
                if (PlayObject != null && pair.Key.MinUserLevel > PlayObject.m_btPermission) continue;
                output += pair.Key.Name + ", ";
            }

            return output.Substring(0, output.Length - 2) + ".";
        }

        protected CommandAttribute GetDefaultSubcommand()
        {
            return this._commands.Keys.First();
        }

        protected CommandAttribute GetSubcommand(string name)
        {
            return this._commands.Keys.FirstOrDefault(command => command.Name == name);
        }
    }
}