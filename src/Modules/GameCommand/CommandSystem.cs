using NLog;
using System.Reflection;
using SystemModule;
using SystemModule.Enums;

namespace CommandSystem
{
    public class GameCommandSystem : ICommandSystem
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly GameCmdConf CommandConf;
        private readonly GameCommands GameCommands = new GameCommands();
        private static readonly Dictionary<string, GameCommand> CommandMaps = new(StringComparer.OrdinalIgnoreCase);

        public GameCommandSystem()
        {
            CommandConf = new GameCmdConf(Path.Combine(SystemShare.BasePath, ConfConst.CommandFileName));
        }

        public void RegisterCommand()
        {
            Logger.Info("读取游戏命令配置...");
            CommandConf.LoadConfig();
            var customCommandMap = RegisterCustomCommand();
            if (customCommandMap == null)
            {
                Logger.Info("读取自定义命令配置失败.");
                return;
            }
            RegisterCommandGroups(customCommandMap);
            Logger.Info("读取游戏命令配置完成...");
        }

        /// <summary>
        /// 注册自定义命令
        /// </summary>
        private Dictionary<string, GameCmd> RegisterCustomCommand()
        {
            var commands = GameCommands.GetType().GetFields();
            if (commands.Length <= 0)
            {
                Logger.Info("获取游戏命令类型失败,请确认游戏命令是否注册.");
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
                    Logger.Warn($"游戏命令[{commandInfo.Name}]重复定义,请确认配置文件是否正确.");
                    continue;
                }
                customCommandMap.Add(commandInfo.Name, customCmd);
            }
            return customCommandMap;
        }

        /// <summary>
        /// 注册游戏命令
        /// </summary>
        private void RegisterCommandGroups(IReadOnlyDictionary<string, GameCmd> customCommands)
        {
            var commands = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsSubclassOf(typeof(GameCommand))).ToArray();//只有继承GameCommand，才添加到命令Map中
            if (commands.Length <= 0)
            {
                return;
            }
            for (var i = 0; i < commands.Length; i++)
            {
                var commandAttribute = (CommandAttribute)commands[i].GetCustomAttribute(typeof(CommandAttribute), true);
                if (commandAttribute == null) continue;
                if (CommandMaps.ContainsKey(commandAttribute.Name))
                {
                    SystemShare.Logger.Error($"重复游戏命令: {commandAttribute.Name}");
                    continue;
                }
                var gameCommand = (GameCommand)Activator.CreateInstance(commands[i]);
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
                    SystemShare.Logger.Error(customCommand != null ? $"游戏命令:{customCommand.CmdName}未注册命令执行方法." : $"游戏命令:{commandAttribute.Name}未注册命令执行方法.");
                    continue;
                }
                gameCommand.Register(commandAttribute, executeMethod);
                CommandMaps.Add(commandAttribute.Name, gameCommand);
            }
        }

        /// <summary>
        /// 执行游戏命令
        /// </summary>
        /// <param name="IPlayerActor">命令对象</param>
        /// <param name="line">命令字符串</param>
        /// <returns><see cref="bool"/></returns>
        public bool Execute(IPlayerActor PlayerActor, string line)
        {
            if (PlayerActor == null)
                throw new ArgumentException("IPlayerActor");

            if (!ExtractCommandAndParameters(line, out var commandName, out var parameters))
                return false;

            var output = string.Empty;
            var found = false;

            if (CommandMaps.TryGetValue(commandName, out var command))
            {
                output = command.Handle(parameters, PlayerActor);
                found = true;
            }

            if (!found)
            {
                output = $"未知命令: {commandName} {parameters}";
            }

            //把返回结果给玩家
            if (!string.IsNullOrEmpty(output))
            {
                PlayerActor.SysMsg(output, MsgColor.Red, MsgType.Hint);
            }
            return found;
        }

        public void ExecCmd(string line)
        {
            var found = false;

            if (!ExtractCommandAndParameters(line, out var command, out var parameters))
                return;
            string output;

            if (CommandMaps.TryGetValue(command, out var commond))
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

            public override string Fallback(string[] parameters = null, IPlayerActor PlayerActor = null)
            {
                var output = "Available commands: ";
                var commandList = CommandMaps.Values.ToList();
                foreach (var pair in commandList)
                {
                    if (PlayerActor != null && pair.Command.PermissionMin > PlayerActor.Permission) continue;
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

            public override string Fallback(string[] parameters = null, IPlayerActor PlayerActor = null)
            {
                return "usage: help <command>";
            }

            public override string Handle(string parameters, IPlayerActor PlayerActor = null)
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