using GameSrv.Player;

namespace GameSrv.GameCommand.Commands {
    [Command("MemberFunctionEx", "", help: "打开会员功能窗口")]
    public class MemberFunctionExCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (GameShare.FunctionNPC != null) {
                GameShare.FunctionNPC.GotoLable(playObject, "@Member", false);
            }
        }
    }
}