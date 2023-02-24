using GameSvr.Player;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands {
    /// <summary>
    /// 删除指定玩家属性点
    /// </summary>
    [Command("DelBonuPoint", "删除指定玩家属性点", "人物名称", 10)]
    public class DelBonuPointCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sHumName = @Params.Length > 0 ? @Params[0] : "";
            if (sHumName == "") {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject targerPlayObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (targerPlayObject != null) {
                targerPlayObject.BonusPoint = 0;
                targerPlayObject.SendMsg(PlayObject, Messages.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                targerPlayObject.HasLevelUp(0);
                targerPlayObject.SysMsg("分配点数已清除!!!", MsgColor.Red, MsgType.Hint);
                PlayObject.SysMsg(sHumName + " 的分配点数已清除.", MsgColor.Green, MsgType.Hint);
            }
            else {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}