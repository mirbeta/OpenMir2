﻿using GameSvr.Conf;
using GameSvr.Player;
using System.Reflection;
using SystemModule.Data;

namespace GameSvr.GameCommand
{
    public class CommandMgr
    {
        private readonly GameCmdConf CommandConf;
        public static readonly GameCommands Commands = new GameCommands();
        private static readonly Dictionary<string, Command> CommandMaps = new Dictionary<string, Command>(StringComparer.OrdinalIgnoreCase);
        private static CommandMgr instance = null;
        private static readonly object locker = new object();
        
        private CommandMgr()
        {
            CommandConf = new GameCmdConf(Path.Combine(M2Share.BasePath, ConfConst.sCommandFileName));
        }

        public static CommandMgr GetInstance()
        {
            lock (locker)
            {
                if (instance == null)
                {
                    instance = new CommandMgr();
                }
            }
            return instance;
        }

        public void RegisterCommand()
        {
            CommandConf.LoadConfig();
            Dictionary<string, string> customCommandMap = RegisterCustomCommand();
            RegisterCommandGroups(customCommandMap);
        }

        /// <summary>
        /// 注册自定义命令
        /// </summary>
        private Dictionary<string, string> RegisterCustomCommand()
        {
            var CustomCommandList = new List<GameCmd>();
            foreach (var item in Commands.GetType().GetFields())
            {
                CustomCommandList.Add((GameCmd)item.GetValue(Commands));
            }
            var CustomCommandMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (CustomCommandList.Count <= 0)
            {
                return CustomCommandMap;
            }
            var cmdIndex = -1;
            foreach (var command in Commands.GetType().GetFields())
            {
                cmdIndex++;
                var attributes = (CustomCommandAttribute)command.GetCustomAttribute(typeof(CustomCommandAttribute), true);
                if (attributes == null) continue;
                if (cmdIndex > CustomCommandList.Count)
                {
                    break;
                }
                var customCmd = CustomCommandList[cmdIndex];
                if (customCmd == null || string.IsNullOrEmpty(customCmd.CommandName))
                {
                    continue;
                }
                var commandInfo = (CommandAttribute)attributes.CommandType.GetCustomAttribute(typeof(CommandAttribute), true);
                if (CustomCommandMap.ContainsKey(commandInfo.Name))
                {
                    M2Share.Log.Error($"重复定义游戏命令[{commandInfo.Name}]");
                    continue;
                }
                CustomCommandMap.Add(commandInfo.Name, customCmd.CommandName);
            }
            CustomCommandList.Clear();
            return CustomCommandMap;
        }

        /// <summary>
        /// 注册游戏命令
        /// </summary>
        private void RegisterCommandGroups(IReadOnlyDictionary<string, string> CustomCommands)
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!type.IsSubclassOf(typeof(Command))) continue;//只有继承BaseCommond，才能添加到命令对象中

                 var attributes = (CommandAttribute[])type.GetCustomAttributes(typeof(CommandAttribute), true);
                if (attributes.Length == 0) continue;

                var groupAttribute = attributes[0];
                if (CommandMaps.ContainsKey(groupAttribute.Name))
                {
                    M2Share.Log.Error($"重复游戏命令: {groupAttribute.Name}");
                }

                if (CustomCommands.TryGetValue(groupAttribute.Name, out string cmdName))
                {
                    groupAttribute.Command = groupAttribute.Command;
                    groupAttribute.Name = cmdName;
                }

                var commandGroup = (Command)Activator.CreateInstance(type);
                if (commandGroup == null)
                {
                    return;
                }
                MethodInfo methodInfo = null;
                foreach (var method in commandGroup.GetType().GetMethods())
                {
                    var methodAttributes = method.GetCustomAttribute(typeof(ExecuteCommand), true);
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

        /// <summary>
        /// 更新游戏命令
        /// </summary>
        public void UpdataRegisterCommandGroups(CommandAttribute OldCmd, string sCmd)
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
        public bool ExecCmd(string line, PlayObject playObject)
        {
            if (playObject == null)
                throw new ArgumentException("PlayObject");

            var output = string.Empty;
            string commandName;
            string parameters;
            var found = false;

            if (!ExtractCommandAndParameters(line, out commandName, out parameters))
                return found;

            Command commond;
            if (CommandMaps.TryGetValue(commandName, out commond))
            {
                output = commond.Handle(parameters, playObject);
                found = true;
            }

            if (!found)
            {
                output = $"未知命令: {commandName} {parameters}";
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
            string command;
            string parameters;
            var found = false;

            if (!ExtractCommandAndParameters(line, out command, out parameters))
                return;
            string output;

            Command commond;
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

        [Command("commands", "列出可用的命令")]
        public class CommandsCommandGroup : Command
        {
            public override string Fallback(string[] parameters = null, PlayObject PlayObject = null)
            {
                var output = "Available commands: ";
                var commandList = CommandMaps.Values.ToList();
                foreach (var pair in commandList)
                {
                    if (PlayObject != null && pair.GameCommand.nPermissionMin > PlayObject.Permission) continue;
                    output += pair.GameCommand.Name + ", ";
                }
                output = output.Substring(0, output.Length - 2) + ".";
                return output + "\nType 'help <command>' to get help.";
            }
        }

        [Command("help", "帮助命令")]
        public class HelpCommandGroup : Command
        {
            public override string Fallback(string[] parameters = null, PlayObject PlayObject = null)
            {
                return "usage: help <command>";
            }

            public override string Handle(string parameters, PlayObject PlayObject = null)
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