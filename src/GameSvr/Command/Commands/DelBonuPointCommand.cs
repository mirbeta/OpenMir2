using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
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
                PlayObject.SysMsg(CommandAttribute.CommandHelp(), MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (m_PlayObject != null)
            {
                m_PlayObject.m_nBonusPoint = 0;
                m_PlayObject.SendMsg(PlayObject, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                m_PlayObject.HasLevelUp(0);
                m_PlayObject.SysMsg("分配点数已清除!!!", MsgColor.Red, MsgType.Hint);
                PlayObject.SysMsg(sHumName + " 的分配点数已清除.", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}