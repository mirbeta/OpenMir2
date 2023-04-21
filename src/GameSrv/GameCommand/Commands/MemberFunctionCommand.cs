using GameSrv.Player;

namespace GameSrv.GameCommand.Commands {
    [Command("MemberFunction", "", help: "打开会员功能窗口")]
    public class MemberFunctionCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (GameShare.ManageNPC != null) {
                playObject.ScriptGotoCount = 0;
                GameShare.ManageNPC.GotoLable(playObject, "@Member", false);
            }
        }
    }
}