using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 调整指定玩家属性点
    /// </summary>
    [GameCommand("BonuPoint", "调整指定玩家属性点", 10)]
    public class BonuPointCommand : BaseCommond
    {
        [DefaultCommand]
        public void BonuPoint(string[] @Params, TPlayObject PlayObject)
        {
            var sHumName = @Params.Length > 0 ? @Params[0] : "";
            var nCount = @Params.Length > 1 ? int.Parse(@Params[1]) : 0;
            string sMsg;
            if (sHumName == "")
            {
                PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " 人物名称 属性点数(不输入为查看点数)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (nCount > 0)
            {
                m_PlayObject.m_nBonusPoint = nCount;
                m_PlayObject.SendMsg(PlayObject, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                return;
            }
            sMsg = string.Format("未分配点数:%d 已分配点数:(DC:%d MC:%d SC:%d AC:%d MAC:%d HP:%d MP:%d HIT:%d SPEED:%d)", m_PlayObject.m_nBonusPoint,
                m_PlayObject.m_BonusAbil.DC, m_PlayObject.m_BonusAbil.MC, m_PlayObject.m_BonusAbil.SC, m_PlayObject.m_BonusAbil.AC,
                m_PlayObject.m_BonusAbil.MAC, m_PlayObject.m_BonusAbil.HP, m_PlayObject.m_BonusAbil.MP, m_PlayObject.m_BonusAbil.Hit, m_PlayObject.m_BonusAbil.Speed);
            PlayObject.SysMsg(string.Format("%s的属性点数为:%s", sHumName, sMsg), TMsgColor.c_Red, TMsgType.t_Hint);
        }
    }
}