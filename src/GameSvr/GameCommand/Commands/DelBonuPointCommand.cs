using GameSvr.Player;
using SystemModule;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 删除指定玩家属性点
    /// </summary>
    [Command("DelBonuPoint", "删除指定玩家属性点", "人物名称", 10)]
    public class DelBonuPointCommand : Command
    {
        [ExecuteCommand]
        public void DelBonuPoint(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumName = @Params.Length > 0 ? @Params[0] : "";
            if (sHumName == "")
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var targerPlayObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (targerPlayObject != null)
            {
                targerPlayObject.BonusPoint = 0;
                targerPlayObject.SendMsg(PlayObject, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                targerPlayObject.HasLevelUp(0);
                targerPlayObject.SysMsg("分配点数已清除!!!", MsgColor.Red, MsgType.Hint);
                PlayObject.SysMsg(sHumName + " 的分配点数已清除.", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}