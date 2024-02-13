using CommandModule.Conf;
using OpenMir2;
using System.Reflection;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Conf;
using SystemModule.Enums;
using SystemModule.SubSystem;

namespace CommandModule
{
    public class GameCommandSystem : ICommandSystem
    {
        private readonly GameCmdConf CommandConf;
        private readonly GameCommands GameCommands = new GameCommands();
        private static readonly Dictionary<string, GameCommand> CommandMaps = new(StringComparer.OrdinalIgnoreCase);
        private readonly char[] CommandSplitLine = new[] { ' ', ':', ',', '\t' };

        public GameCommandSystem()
        {
            CommandConf = new GameCmdConf(Path.Combine(SystemShare.BasePath, ConfConst.CommandFileName));
        }

        public void RegisterCommand()
        {
            LogService.Info("读取游戏命令配置...");
            CommandConf.LoadConfig(GameCommands);
            Dictionary<string, GameCmd> customCommandMap = RegisterCustomCommand();
            if (customCommandMap == null)
            {
                LogService.Info("读取自定义命令配置失败.");
                return;
            }
            RegisterCommandGroups(customCommandMap);
            LogService.Info("读取游戏命令配置完成...");
        }

        /// <summary>
        /// 注册自定义命令
        /// </summary>
        private Dictionary<string, GameCmd> RegisterCustomCommand()
        {
            FieldInfo[] commands = GameCommands.GetType().GetFields();
            if (commands.Length <= 0)
            {
                LogService.Info("获取游戏命令类型失败,请确认游戏命令是否注册.");
                return null;
            }
            Dictionary<string, GameCmd> customCommandMap = new Dictionary<string, GameCmd>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < commands.Length; i++)
            {
                GameCmd customCmd = (GameCmd)commands[i].GetValue(GameCommands);
                if (customCmd == null || string.IsNullOrEmpty(customCmd.CmdName))
                {
                    continue;
                }
                RegisterCommandAttribute commandAttribute = (RegisterCommandAttribute)commands[i].GetCustomAttribute(typeof(RegisterCommandAttribute), true);
                CommandAttribute commandInfo = (CommandAttribute)commandAttribute?.HandleType.GetCustomAttribute(typeof(CommandAttribute), true);
                if (commandInfo == null)
                {
                    continue;
                }
                if (customCommandMap.ContainsKey(commandInfo.Name))
                {
                    LogService.Warn($"游戏命令[{commandInfo.Name}]重复定义,请确认配置文件是否正确.");
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
            Type[] commands = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsSubclassOf(typeof(GameCommand))).ToArray();//只有继承GameCommand，才添加到命令Map中
            if (commands.Length <= 0)
            {
                return;
            }
            for (int i = 0; i < commands.Length; i++)
            {
                CommandAttribute commandAttribute = (CommandAttribute)commands[i].GetCustomAttribute(typeof(CommandAttribute), true);
                if (commandAttribute == null)
                {
                    continue;
                }

                if (CommandMaps.ContainsKey(commandAttribute.Name))
                {
                    LogService.Error($"重复游戏命令: {commandAttribute.Name}");
                    continue;
                }
                GameCommand gameCommand = (GameCommand)Activator.CreateInstance(commands[i]);
                if (gameCommand == null)
                {
                    continue;
                }
                if (customCommands.TryGetValue(commandAttribute.Name, out GameCmd customCommand))
                {
                    commandAttribute.Command = commandAttribute.Name;
                    commandAttribute.Name = customCommand.CmdName;
                    commandAttribute.PermissionMax = customCommand.PerMissionMax;
                    commandAttribute.PermissionMin = customCommand.PerMissionMin;
                }
                MethodInfo executeMethod = gameCommand.GetType().GetMethod("Execute");
                if (executeMethod == null)
                {
                    LogService.Error(customCommand != null ? $"游戏命令:{customCommand.CmdName}未注册命令执行方法." : $"游戏命令:{commandAttribute.Name}未注册命令执行方法.");
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
            {
                throw new ArgumentException("IPlayerActor");
            }

            //string sCMD = string.Empty;
            //string sParam1 = string.Empty;
            //string sParam2 = string.Empty;
            //string sParam3 = string.Empty;
            //string sParam4 = string.Empty;
            //string sParam5 = string.Empty;
            //string sParam6 = string.Empty;
            //string sParam7 = string.Empty;
            //string sC= sC = line.AsSpan()[1..].ToString();
            //sC = HUtil32.GetValidStr3(sC, ref sCMD, CommandSplitLine);
            //if (!string.IsNullOrEmpty(sC))
            //{
            //    sC = HUtil32.GetValidStr3(sC, ref sParam1, CommandSplitLine);
            //}
            //if (!string.IsNullOrEmpty(sC))
            //{
            //    sC = HUtil32.GetValidStr3(sC, ref sParam2, CommandSplitLine);
            //}
            //if (!string.IsNullOrEmpty(sC))
            //{
            //    sC = HUtil32.GetValidStr3(sC, ref sParam3, CommandSplitLine);
            //}
            //if (!string.IsNullOrEmpty(sC))
            //{
            //    sC = HUtil32.GetValidStr3(sC, ref sParam4, CommandSplitLine);
            //}
            //if (!string.IsNullOrEmpty(sC))
            //{
            //    sC = HUtil32.GetValidStr3(sC, ref sParam5, CommandSplitLine);
            //}
            //if (!string.IsNullOrEmpty(sC))
            //{
            //    sC = HUtil32.GetValidStr3(sC, ref sParam6, CommandSplitLine);
            //}
            //if (!string.IsNullOrEmpty(sC))
            //{
            //    sC = HUtil32.GetValidStr3(sC, ref sParam7, CommandSplitLine);
            //}

            if (!ExtractCommandAndParameters(line, out string commandName, out string parameters))
            {
                return false;
            }

            string output = string.Empty;
            bool found = false;

            if (CommandMaps.TryGetValue(commandName, out GameCommand command))
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
            bool found = false;

            if (!ExtractCommandAndParameters(line, out string command, out string parameters))
            {
                return;
            }

            string output;

            if (CommandMaps.TryGetValue(command, out GameCommand commond))
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
            {
                return false;
            }

            if (line[0] != '@') // 检查命令首字母是否为指定的字符串
            {
                return false;
            }

            line = line[1..];
            command = line.Split(' ')[0]; // 取命令
            parameters = string.Empty;
            if (line.Contains(' '))
            {
                parameters = line[(line.IndexOf(' ') + 1)..].Trim(); // 取命令参数
            }

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
                string output = "Available commands: ";
                List<GameCommand> commandList = CommandMaps.Values.ToList();
                foreach (GameCommand pair in commandList)
                {
                    if (PlayerActor != null && pair.Command.PermissionMin > PlayerActor.Permission)
                    {
                        continue;
                    }

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
                {
                    return this.Fallback();
                }

                string[] @params = parameters.Split(' ');
                string group = @params[0];
                string command = @params.Count() > 1 ? @params[1] : string.Empty;
                string output = $"Unknown command: {group} {command}";
                return output;
            }
        }
    }
}