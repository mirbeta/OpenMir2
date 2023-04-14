using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 删除指定玩家属性点
    /// </summary>
    [Command("DelBonuPoint", "删除指定玩家属性点", "人物名称", 10)]
    public class DelBonuPointCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var targerPlayObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (targerPlayObject != null) {
                targerPlayObject.BonusPoint = 0;
                targerPlayObject.SendMsg(Messages.RM_ADJUST_BONUS, 0, 0, 0, 0);
                targerPlayObject.HasLevelUp(0);
                targerPlayObject.SysMsg("分配点数已清除!!!", MsgColor.Red, MsgType.Hint);
                playObject.SysMsg(sHumName + " 的分配点数已清除.", MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}