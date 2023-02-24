using GameSvr.Player;

namespace GameSvr.GameCommand.Commands {
    [Command("MemberFunction", "", help: "打开会员功能窗口", 0)]
    public class MemberFunctionCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject) {
            if (M2Share.ManageNPC != null) {
                PlayObject.ScriptGotoCount = 0;
                M2Share.ManageNPC.GotoLable(PlayObject, "@Member", false);
            }
        }
    }
}