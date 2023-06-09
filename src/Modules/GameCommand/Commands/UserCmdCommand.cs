using SystemModule;

namespace CommandSystem {
    /// <summary>
    /// 自定义命令
    /// </summary>
    [Command("UserCmd", "自定义命令", 10)]
    public class UserCmdCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor) {
            // string sLable = @Params.Length > 0 ? @Params[0] : "";
            // byte Flag = 0;
            // if (PlayerActor.SysMsgm_nUserCmdNPC == M2Share.g_FunctionNPC)
            // {
            //     M2Share.g_FunctionNPC.GotoLable(IPlayerActor, sLable, false);
            //     Flag = 8;
            // }
            // if (Flag != 8)
            // {
            //     M2Share.g_FunctionNPC.GotoLable(IPlayerActor, sLable, false);// 执行默认脚本  修复不能执行自定义脚本问题
            // }
            // PlayerActor.SysMsgm_nUserCmdNPC = null;
        }
    }
}