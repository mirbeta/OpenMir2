using M2Server.Player;

namespace M2Server.GameCommand.Commands {
    [Command("MemberFunction", "", help: "打开会员功能窗口")]
    public class MemberFunctionCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (M2Share.ManageNPC != null) {
                playObject.ScriptGotoCount = 0;
                M2Share.ManageNPC.GotoLable(playObject, "@Member", false);
            }
        }
    }
}