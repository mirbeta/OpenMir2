using M2Server.Player;
using SystemModule;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 夫妻传送，将对方传送到自己身边，对方必须允许传送。
    /// </summary>
    [Command("DearRecall", "夫妻传送", "(夫妻传送，将对方传送到自己身边，对方必须允许传送。)")]
    public class DearRecallCommond : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (string.IsNullOrEmpty(playObject.DearName)) {
                playObject.SysMsg("你没有结婚!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (playObject.Envir.Flag.boNODEARRECALL) {
                playObject.SysMsg("本地图禁止夫妻传送!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (playObject.DearHuman == null) {
                if (playObject.Gender == 0) {
                    playObject.SysMsg("你的老婆不在线!!!", MsgColor.Red, MsgType.Hint);
                }
                else {
                    playObject.SysMsg("你的老公不在线!!!", MsgColor.Red, MsgType.Hint);
                }
                return;
            }
            if (HUtil32.GetTickCount() - playObject.DearRecallTick < 10000) {
                playObject.SysMsg("稍等会才能再次使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            playObject.DearRecallTick = HUtil32.GetTickCount();
            if (playObject.DearHuman.CanDearRecall) {
                playObject.RecallHuman(playObject.DearHuman.ChrName);
            }
            else {
                playObject.SysMsg(playObject.DearHuman.ChrName + " 不允许传送!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}