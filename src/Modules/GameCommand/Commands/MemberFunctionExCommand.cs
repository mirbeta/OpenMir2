using SystemModule;

namespace CommandSystem {
    [Command("MemberFunctionEx", "", help: "打开会员功能窗口")]
    public class MemberFunctionExCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor) {
            if (ModuleShare.FunctionNPC != null) {
                ModuleShare.FunctionNPC.GotoLable(IPlayerActor, "@Member", false);
            }
        }
    }
}