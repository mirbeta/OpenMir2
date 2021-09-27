using SystemModule;
using System;
using System.Collections;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 调整指定玩家声望点
    /// </summary>
    [GameCommand("CreditPoint", "调整指定玩家声望点", 10)]
    public class CreditPointCommand : BaseCommond
    {
        [DefaultCommand]
        public void CreditPoint(string[] @Params, TPlayObject PlayObject)
        {
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var sCtr = @Params.Length > 1 ? @Params[1] : "";
            var nPoint = @Params.Length > 2 ? int.Parse(@Params[2]) : 0;
            var Ctr = '1';
            int nCreditPoint;
            if (sCtr != "")
            {
                Ctr = sCtr[0];
            }
            if (string.IsNullOrEmpty(sHumanName) || !new ArrayList(new char[] { '=', '+', '-' }).Contains(Ctr) || nPoint < 0 || nPoint > int.MaxValue
                || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, M2Share.g_sGameCommandCreditPointHelpMsg),
                    TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            switch (sCtr[0])
            {
                case '=':
                    if (nPoint >= 0)
                    {
                        m_PlayObject.m_btCreditPoint = (byte)nPoint;
                    }
                    break;

                case '+':
                    nCreditPoint = m_PlayObject.m_btCreditPoint + nPoint;
                    if (nPoint >= 0)
                    {
                        m_PlayObject.m_btCreditPoint = (byte)nCreditPoint;
                    }
                    break;

                case '-':
                    nCreditPoint = m_PlayObject.m_btCreditPoint - nPoint;
                    if (nPoint >= 0)
                    {
                        m_PlayObject.m_btCreditPoint = (byte)nCreditPoint;
                    }
                    break;
            }
            m_PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandCreditPointHumanMsg, nPoint, m_PlayObject.m_btCreditPoint),
                TMsgColor.c_Green, TMsgType.t_Hint);
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandCreditPointGMMsg, sHumanName, nPoint, m_PlayObject.m_btCreditPoint),
                TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}