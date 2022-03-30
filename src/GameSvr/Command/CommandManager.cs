using System.Reflection;
using SystemModule;

namespace GameSvr.CommandSystem
{
    public class CommandManager
    {
        private static readonly Dictionary<string, BaseCommond> CommandMaps = new Dictionary<string, BaseCommond>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 自定义游戏命令列表
        /// </summary>
        private static readonly Dictionary<string, string> CustomCommands = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public CommandManager()
        {

        }

        public void RegisterCommand()
        {
            M2Share.CommandConf.LoadConfig();
            RegisterCommandGroups();
        }

        /// <summary>
        /// 注册游戏命令
        /// </summary>
        private void RegisterCommandGroups()
        {
            var cmdName = string.Empty;
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!type.IsSubclassOf(typeof(BaseCommond))) continue;//只有继承BaseCommond，才能添加到命令对象中

                var attributes = (GameCommandAttribute[])type.GetCustomAttributes(typeof(GameCommandAttribute), true);
                if (attributes.Length == 0) continue;

                var groupAttribute = attributes[0];
                if (CommandMaps.ContainsKey(groupAttribute.Name))
                {
                    M2Share.ErrorMessage($"重复游戏命令: {groupAttribute.Name}");
                }

                if (CustomCommands.TryGetValue(groupAttribute.Name, out cmdName))
                {
                    groupAttribute.Command = groupAttribute.Command;
                    groupAttribute.Name = cmdName;
                }

                var commandGroup = (BaseCommond)Activator.CreateInstance(type);
                if (commandGroup == null)
                {
                    return;
                }
                MethodInfo methodInfo = null;
                foreach (var method in commandGroup.GetType().GetMethods())
                {
                    var methodAttributes = method.GetCustomAttribute(typeof(DefaultCommand), true);
                    if (methodAttributes != null)
                    {
                        methodInfo = method;
                        break;
                    }
                }
                if (methodInfo == null)
                {
                    return;
                }
                commandGroup.Register(groupAttribute, methodInfo);
                CommandMaps.Add(groupAttribute.Name, commandGroup);
            }
        }

        public void RegisterCommand(string command, string commandName)
        {
            CustomCommands.Add(command, commandName);
        }

        /// <summary>
        /// 更新游戏命令
        /// </summary>
        /// <param name="OldCmd"></param>
        /// <param name="sCmd"></param>
        public void UpdataRegisterCommandGroups(GameCommandAttribute OldCmd, string sCmd)
        {
            if (CommandMaps.ContainsKey(OldCmd.Command))
            {
                //foreach (var pair in CommandGroups)
                //{
                //    if (pair.Key.Name != "make") continue;
                //    pair.Key.Name = "item";
                //}

                //var commandGroup = (CommandGroup)Activator.CreateInstance(typeof(MakeItmeCommand));
                //commandGroup.Register(NewCmd);
                //CommandGroups.Add(NewCmd, commandGroup);
                //CommandGroups.Remove(OldCmd);
            }
        }

        /// <summary>
        /// 执行游戏命令
        /// </summary>
        /// <param name="line">命令字符串</param>
        /// <param name="playObject">命令对象</param>
        /// <returns><see cref="bool"/></returns>
        public bool ExecCmd(string line, TPlayObject playObject)
        {
            var output = string.Empty;
            string command;
            string parameters;
            var found = false;

            if (playObject == null)
                throw new ArgumentException("PlayObject");
            if (!ExtractCommandAndParameters(line, out command, out parameters))
                return found;

            BaseCommond commond;
            if (CommandMaps.TryGetValue(command, out commond))
            {
                output = commond.Handle(parameters, playObject);
                found = true;
            }

            if (!found)
            {
                output = $"未知命令: {command} {parameters}";
            }

            //把返回结果给玩家
            if (!string.IsNullOrEmpty(output))
            {
                playObject.SysMsg(output, MsgColor.Red, MsgType.Hint);
            }
            return found;
        }

        public void ExecCmd(string line)
        {
            var output = string.Empty;
            string command;
            string parameters;
            var found = false;

            if (!ExtractCommandAndParameters(line, out command, out parameters))
                return;

            BaseCommond commond = null;
            if (CommandMaps.TryGetValue(command, out commond))
            {
                output = commond.Handle(parameters);
                found = true;
            }

            if (!found)
            {
                output = $"未知命令: {command} {parameters}";
            }
        }

        /// <summary>
        /// 构造游戏命令
        /// </summary>
        /// <param name="line">字符串</param>
        /// <param name="command">返回 命令名称</param>
        /// <param name="parameters">返回 命令参数</param>
        /// <returns>解析是否成功</returns>
        private bool ExtractCommandAndParameters(string line, out string command, out string parameters)
        {
            line = line.Trim();
            command = string.Empty;
            parameters = string.Empty;

            if (line == string.Empty)
                return false;

            if (line[0] != '@') // 检查命令首字母是否为指定的字符串
                return false;

            line = line.Substring(1);
            command = line.Split(' ')[0]; // 取命令
            parameters = string.Empty;
            if (line.Contains(' ')) parameters = line.Substring(line.IndexOf(' ') + 1).Trim(); // 取命令参数
            return true;
        }

        [GameCommand("commands", "列出可用的命令")]
        public class CommandsCommandGroup : BaseCommond
        {
            public override string Fallback(string[] parameters = null, TPlayObject PlayObject = null)
            {
                var output = "Available commands: ";
                var commandList = CommandMaps.Values.ToList();
                foreach (var pair in commandList)
                {
                    if (PlayObject != null && pair.GameCommand.nPermissionMin > PlayObject.m_btPermission) continue;
                    output += pair.GameCommand.Name + ", ";
                }
                output = output.Substring(0, output.Length - 2) + ".";
                return output + "\nType 'help <command>' to get help.";
            }
        }

        [GameCommand("help", "帮助命令")]
        public class HelpCommandGroup : BaseCommond
        {
            public override string Fallback(string[] parameters = null, TPlayObject PlayObject = null)
            {
                return "usage: help <command>";
            }

            public override string Handle(string parameters, TPlayObject PlayObject = null)
            {
                if (parameters == string.Empty)
                    return this.Fallback();
                var @params = parameters.Split(' ');
                var group = @params[0];
                var command = @params.Count() > 1 ? @params[1] : string.Empty;
                var output = $"Unknown command: {group} {command}";
                return output;
            }
        }
    }
}