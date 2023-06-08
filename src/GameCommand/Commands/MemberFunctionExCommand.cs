using M2Server.Player;

namespace M2Server.GameCommand.Commands {
    [Command("MemberFunctionEx", "", help: "打开会员功能窗口")]
    public class MemberFunctionExCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (M2Share.FunctionNPC != null) {
                M2Share.FunctionNPC.GotoLable(playObject, "@Member", false);
            }
        }
    }
}