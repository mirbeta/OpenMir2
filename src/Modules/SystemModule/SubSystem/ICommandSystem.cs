namespace SystemModule
{
    public interface ICommandSystem
    {
        /// <summary>
        /// 注册游戏命令
        /// </summary>
        void RegisterCommand();

        /// <summary>
        /// 执行游戏命令
        /// </summary>
        /// <param name="IPlayerActor">命令对象</param>
        /// <param name="line">命令字符串</param>
        /// <returns><see cref="bool"/></returns>
        public bool Execute(IPlayerActor PlayerActor, string line);

        public void ExecCmd(string line);
    }
}