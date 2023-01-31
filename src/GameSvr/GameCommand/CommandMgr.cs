using GameSvr.Conf;
using GameSvr.Player;
using NLog;
using System.Reflection;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand
{
    public class CommandMgr
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly GameCmdConf CommandConf;
        public static readonly GameCommands GameCommands = new GameCommands();
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
            Dictionary<string, GameCmd> customCommandMap = RegisterCustomCommand();
            if (customCommandMap == null)
            {
                _logger.Info("读取自定义命令配置失败.");
                return;
            }
            RegisterCommandGroups(customCommandMap);
            _logger.Info("读取游戏命令配置完成...");
        }

        /// <summary>
        /// 注册自定义命令
        /// </summary>
        private Dictionary<string, GameCmd> RegisterCustomCommand()
        {
            FieldInfo[] commands = GameCommands.GetType().GetFields();
            if (commands.Length <= 0)
            {
                _logger.Info("获取游戏命令失败");
                return null;
            }
            GameCmd[] customCommandList = new GameCmd[commands.Length];
            for (int i = 0; i < customCommandList.Length; i++)
            {
                customCommandList[i] = (GameCmd)commands[i].GetValue(GameCommands);
            }
            Dictionary<string, GameCmd> customCommandMap = new Dictionary<string, GameCmd>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < commands.Length; i++)
            {
                Attribute attributes = commands[i].GetCustomAttribute(typeof(CommandHandleAttribute), true);
                if (attributes == null) continue;
                CommandHandleAttribute commandAttribute = (CommandHandleAttribute)attributes;
                GameCmd customCmd = customCommandList[i];
                if (customCmd == null || string.IsNullOrEmpty(customCmd.CmdName))
                {
                    continue;
                }
                CommandAttribute commandInfo = (CommandAttribute)commandAttribute.CommandHandle.GetCustomAttribute(typeof(CommandAttribute), true);
                if (commandInfo == null)
                {
                    continue;
                }
                if (customCommandMap.ContainsKey(commandInfo.Name))
                {
                    _logger.Warn($"游戏命令[{commandInfo.Name}]重复定义,请确认配置文件是否正确.");
                    continue;
                }
                customCommandMap.Add(commandInfo.Name, customCmd);
            }
            customCommandList = null;
            return customCommandMap;
        }

        /// <summary>
        /// 注册游戏命令
        /// </summary>
        private static void RegisterCommandGroups(IReadOnlyDictionary<string, GameCmd> customCommands)
        {
            List<Type> commands = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsSubclassOf(typeof(Command))).ToList();//只有继承Commond，才能添加到命令对象中
            if (!commands.Any())
            {
                return;
            }
            foreach (Type type in commands)
            {
                CommandAttribute[] attributes = (CommandAttribute[])type.GetCustomAttributes(typeof(CommandAttribute), true);
                if (attributes.Length == 0) continue;

                CommandAttribute groupAttribute = attributes[0];
                if (CommandMaps.ContainsKey(groupAttribute.Name))
                {
                    M2Share.Logger.Error($"重复游戏命令: {groupAttribute.Name}");
                }
                Command gameCommand = (Command)Activator.CreateInstance(type);
                if (gameCommand == null)
                {
                    return;
                }
                if (customCommands.TryGetValue(groupAttribute.Name, out GameCmd customCommand))
                {
                    groupAttribute.Command = groupAttribute.Command;
                    groupAttribute.Name = customCommand.CmdName;
                    groupAttribute.nPermissionMax = (byte)customCommand.PerMissionMax;
                    groupAttribute.nPermissionMin = (byte)customCommand.PerMissionMin;
                }
                MethodInfo methodInfo = null;
                foreach (MethodInfo method in gameCommand.GetType().GetMethods())
                {
                    Attribute methodAttributes = method.GetCustomAttribute(typeof(ExecuteCommand), true);
                    if (methodAttributes == null)
                    {
                        continue;
                    }
                    methodInfo = method;
                    break;
                }
                if (methodInfo == null)
                {
                    return;
                }
                gameCommand.Register(groupAttribute, methodInfo);
                CommandMaps.Add(groupAttribute.Name, gameCommand);
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

            if (!ExtractCommandAndParameters(line, out string commandName, out string parameters))
                return false;

            string output = string.Empty;
            bool found = false;

            if (CommandMaps.TryGetValue(commandName, out Command commond))
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
            bool found = false;

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
        private static bool ExtractCommandAndParameters(string line, out string command, out string parameters)
        {
            line = line.Trim();
            command = string.Empty;
            parameters = string.Empty;

            if (line == string.Empty)
                return false;

            if (line[0] != '@') // 检查命令首字母是否为指定的字符串
                return false;

            line = line[1..];
            command = line.Split(' ')[0]; // 取命令
            parameters = string.Empty;
            if (line.Contains(' ')) parameters = line[(line.IndexOf(' ') + 1)..].Trim(); // 取命令参数
            return true;
        }

        [Command("commands", "列出可用的命令")]
        public class CommandsCommandGroup : Command
        {
            public override string Fallback(string[] parameters = null, PlayObject PlayObject = null)
            {
                string output = "Available commands: ";
                List<Command> commandList = CommandMaps.Values.ToList();
                foreach (Command pair in commandList)
                {
                    if (PlayObject != null && pair.GameCommand.nPermissionMin > PlayObject.Permission) continue;
                    output += pair.GameCommand.Name + ", ";
                }
                output = output[..^2] + ".";
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
                string[] @params = parameters.Split(' ');
                string group = @params[0];
                string command = @params.Count() > 1 ? @params[1] : string.Empty;
                string output = $"Unknown command: {group} {command}";
                return output;
            }
        }
    }
}