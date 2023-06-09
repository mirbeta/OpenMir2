namespace CommandSystem
{
    /// <summary>
    /// 命令定义
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute {
        /// <summary>
        /// 命令名
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// 命令名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 命令描述
        /// </summary>
        public string Desc { get; set; }

        public string Help { get; set; }

        /// <summary>
        /// 命令等级最小权限
        /// </summary>
        public byte PermissionMin { get; set; }

        /// <summary>
        /// 命令等级最大权限
        /// </summary>
        public byte PermissionMax { get; set; }

        public CommandAttribute(string name, string desc, byte minUserLevel = 0, byte maxUserLevel = 10) {
            this.Name = name;
            this.Desc = desc;
            this.PermissionMin = minUserLevel;
            this.PermissionMax = maxUserLevel;
        }

        public CommandAttribute(string name, string desc, string help, byte minUserLevel = 0, byte maxUserLevel = 10) {
            this.Name = name;
            this.Desc = desc;
            this.Help = help;
            this.PermissionMin = minUserLevel;
            this.PermissionMax = maxUserLevel;
        }

        public string CommandHelp => $"命令格式: @{Name} {Help}";
    }

    /// <summary>
    /// 命令执行入口定义
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandExecuteAttribute : Attribute {
        /// <summary>
        /// 命令名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 命令说明
        /// </summary>
        public string Desc { get; private set; }

        /// <summary>
        /// 命令帮助
        /// </summary>
        public string Help { get; private set; }

        /// <summary>
        /// 命令等级
        /// </summary>
        public byte MinUserLevel { get; private set; }

        public CommandExecuteAttribute(string command, string desc, string help, byte minUserLevel = 0) {
            this.Name = command;
            this.Desc = desc;
            this.Help = help;
            this.MinUserLevel = minUserLevel;
        }
    }

    /// <summary>
    /// 游戏入口点方法必须标识为DefaultCommand
    /// 例：
    /// [DefaultCommand]
    /// public void CmdTest(){}
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ExecuteCommand : CommandExecuteAttribute {
        public ExecuteCommand(byte minUserLevel = 0) : base("", "", "", minUserLevel) {

        }
    }

    /// <summary>
    /// 游戏命令注册实现Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class RegisterCommandAttribute : Attribute {
        public readonly Type HandleType;

        public RegisterCommandAttribute(Type commond) {
            HandleType = commond;
        }
    }
}