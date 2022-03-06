using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 调整指定玩家属性的复位
    /// </summary>
    [GameCommand("RestBonuPoint", "调整指定玩家属性的复位", "人物名称", 10)]
    public class RestBonuPointCommand : BaseCommond
    {
        [DefaultCommand]
        public void RestBonuPoint(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumName = @Params.Length > 0 ? @Params[0] : "";
            int nTotleUsePoint;
            if (sHumName == "")
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (m_PlayObject != null)
            {
                nTotleUsePoint = m_PlayObject.m_BonusAbil.DC + m_PlayObject.m_BonusAbil.MC + m_PlayObject.m_BonusAbil.SC + m_PlayObject.m_BonusAbil.AC + m_PlayObject.m_BonusAbil.MAC
                    + m_PlayObject.m_BonusAbil.HP + m_PlayObject.m_BonusAbil.MP + m_PlayObject.m_BonusAbil.Hit + m_PlayObject.m_BonusAbil.Speed + m_PlayObject.m_BonusAbil.X2;
                m_PlayObject.m_nBonusPoint += nTotleUsePoint;
                m_PlayObject.SendMsg(m_PlayObject, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                m_PlayObject.HasLevelUp(0);
                m_PlayObject.SysMsg("分配点数已复位!!!", MsgColor.Red, MsgType.Hint);
                PlayObject.SysMsg(sHumName + " 的分配点数已复位.", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}