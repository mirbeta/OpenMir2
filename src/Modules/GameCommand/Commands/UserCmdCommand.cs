using SystemModule;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 自定义命令
    /// </summary>
    [Command("UserCmd", "自定义命令", 10)]
    public class UserCmdCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            string sLable = @params.Length > 0 ? @params[0] : "";
            /*if (PlayerActor.UserCmdNPC == SystemShare.FunctionNPC)
            {
                SystemShare.FunctionNPC.GotoLable(PlayerActor, sLable, false);
                Flag = 8;
            }
            if (Flag != 8)
            {
                SystemShare.FunctionNPC.GotoLable(PlayerActor, sLable, false);// 执行默认脚本  修复不能执行自定义脚本问题
            }
            SystemShare.UserCmdNPC = null;*/
        }
    }
}