using GameSvr.Conf;
using GameSvr.Player;
using NLog;
using System.Reflection;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand
{
    public static class CommandMgr
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly GameCmdConf CommandConf;
        public static readonly GameCommands GameCommands = new();
        private static readonly Dictionary<string, GameCommand> CommandMaps = new(StringComparer.OrdinalIgnoreCase);

        static CommandMgr()
        {
            CommandConf = new GameCmdConf(Path.Combine(M2Share.BasePath, ConfConst.CommandFileName));
        }

        public static void RegisterCommand()
        {
            CommandConf.LoadConfig();
            var customCommandMap = RegisterCustomCommand();
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
        private static Dictionary<string, GameCmd> RegisterCustomCommand()
        {
            var commands = GameCommands.GetType().GetFields();
            if (commands.Length <= 0)
            {
                _logger.Info("获取游戏命令类型失败,请确认游戏命令是否注册.");
                return null;
            }
            var customCommandMap = new Dictionary<string, GameCmd>(StringComparer.OrdinalIgnoreCase);
            for (var i = 0; i < commands.Length; i++)
            {
                var customCmd = (GameCmd)commands[i].GetValue(GameCommands);
                if (customCmd == null || string.IsNullOrEmpty(customCmd.CmdName))
                {
                    continue;
                }
                var commandAttribute = (RegisterCommandAttribute)commands[i].GetCustomAttribute(typeof(RegisterCommandAttribute), true);
                var commandInfo = (CommandAttribute)commandAttribute?.HandleType.GetCustomAttribute(typeof(CommandAttribute), true);
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
            return customCommandMap;
        }

        /// <summary>
        /// 注册游戏命令
        /// </summary>
        private static void RegisterCommandGroups(IReadOnlyDictionary<string, GameCmd> customCommands)
        {
            var commands = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsSubclassOf(typeof(GameCommand))).ToList();//只有继承GameCommand，才添加到命令Map中
            if (!commands.Any())
            {
                return;
            }
            foreach (var command in commands)
            {
                var commandAttribute = (CommandAttribute)command.GetCustomAttribute(typeof(CommandAttribute), true);
                if (commandAttribute == null) continue;
                if (CommandMaps.ContainsKey(commandAttribute.Name))
                {
                    M2Share.Logger.Error($"重复游戏命令: {commandAttribute.Name}");
                    continue;
                }
                var gameCommand = (GameCommand)Activator.CreateInstance(command);
                if (gameCommand == null)
                {
                    continue;
                }
                if (customCommands.TryGetValue(commandAttribute.Name, out var customCommand))
                {
                    commandAttribute.Command = commandAttribute.Name;
                    commandAttribute.Name = customCommand.CmdName;
                    commandAttribute.PermissionMax = customCommand.PerMissionMax;
                    commandAttribute.PermissionMin = customCommand.PerMissionMin;
                }
                var executeMethod = gameCommand.GetType().GetMethod("Execute");
                if (executeMethod == null)
                {
                    M2Share.Logger.Error(customCommand != null ? $"游戏命令:{customCommand.CmdName}未注册命令执行方法." : $"游戏命令:{commandAttribute.Name}未注册命令执行方法.");
                    continue;
                }
                gameCommand.Register(commandAttribute, executeMethod);
                CommandMaps.Add(commandAttribute.Name, gameCommand);
            }
        }

        /// <summary>
        /// 执行游戏命令
        /// </summary>
        /// <param name="line">命令字符串</param>
        /// <param name="playObject">命令对象</param>
        /// <returns><see cref="bool"/></returns>
        public static bool Execute(string line, PlayObject playObject)
        {
            if (playObject == null)
                throw new ArgumentException("PlayObject");

            if (!ExtractCommandAndParameters(line, out string commandName, out string parameters))
                return false;

            string output = string.Empty;
            bool found = false;

            if (CommandMaps.TryGetValue(commandName, out GameCommand commond))
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

        public static void ExecCmd(string line)
        {
            string command;
            string parameters;
            bool found = false;

            if (!ExtractCommandAndParameters(line, out command, out parameters))
                return;
            string output;

            GameCommand commond;
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

            if (string.IsNullOrEmpty(line))
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
        public class CommandsCommandGroup : GameCommand
        {
            public void Execute()
            {

            }

            public override string Fallback(string[] parameters = null, PlayObject PlayObject = null)
            {
                string output = "Available commands: ";
                List<GameCommand> commandList = CommandMaps.Values.ToList();
                foreach (GameCommand pair in commandList)
                {
                    if (PlayObject != null && pair.Command.PermissionMin > PlayObject.Permission) continue;
                    output += pair.Command.Name + ", ";
                }
                output = output[..^2] + ".";
                return output + "\nType 'help <command>' to get help.";
            }
        }

        [Command("help", "帮助命令")]
        public class HelpCommandGroup : GameCommand
        {
            public void Execute()
            {

            }

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