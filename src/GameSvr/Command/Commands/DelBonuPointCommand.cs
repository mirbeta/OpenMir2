using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 删除指定玩家属性点
    /// </summary>
    [GameCommand("DelBonuPoint", "删除指定玩家属性点", "人物名称", 10)]
    public class DelBonuPointCommand : BaseCommond
    {
        [DefaultCommand]
        public void DelBonuPoint(string[] @Params, TPlayObject PlayObject)
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
            var targerPlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (targerPlayObject != null)
            {
                targerPlayObject.m_nBonusPoint = 0;
                targerPlayObject.SendMsg(PlayObject, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                targerPlayObject.HasLevelUp(0);
                targerPlayObject.SysMsg("分配点数已清除!!!", MsgColor.Red, MsgType.Hint);
                PlayObject.SysMsg(sHumName + " 的分配点数已清除.", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}