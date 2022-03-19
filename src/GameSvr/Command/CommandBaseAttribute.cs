using System;

namespace GameSvr.CommandSystem
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GameCommandAttribute : Attribute
    {
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

        /// <summary>
        /// 命令帮助
        /// </summary>
        public string Help { get; set; }

        /// <summary>
        /// 命令等级最小权限
        /// </summary>
        public byte nPermissionMin { get; private set; }

        /// <summary>
        /// 命令等级最大权限
        /// </summary>
        public byte nPermissionMax { get; private set; }

        public GameCommandAttribute(string name, string desc, byte minUserLevel = 0, byte maxUserLevel = 10)
        {
            this.Name = name;
            this.Desc = desc;
            this.nPermissionMin = minUserLevel;
            this.nPermissionMax = maxUserLevel;
        }

        public GameCommandAttribute(string name, string desc, string help, byte minUserLevel = 0, byte maxUserLevel = 10)
        {
            this.Name = name;
            this.Desc = desc;
            this.Help = help;
            this.nPermissionMin = minUserLevel;
            this.nPermissionMax = maxUserLevel;
        }

        public string CommandHelp => $"命令格式: @{Name} {Help}";
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
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

        public CommandAttribute(string command, string desc, string help, byte minUserLevel = 0)
        {
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
    public class DefaultCommand : CommandAttribute
    {
        public DefaultCommand(byte minUserLevel = 0) : base("", "", "", minUserLevel)
        {
        }
    }
}