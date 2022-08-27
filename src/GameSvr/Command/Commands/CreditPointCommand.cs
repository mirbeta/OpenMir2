using GameSvr.Player;
using System.Collections;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整指定玩家声望点
    /// </summary>
    [GameCommand("CreditPoint", "调整指定玩家声望点", "人物名称 控制符(+,-,=) 声望点数(0-255)", 10)]
    public class CreditPointCommand : BaseCommond
    {
        [DefaultCommand]
        public void CreditPoint(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
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
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
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
                MsgColor.Green, MsgType.Hint);
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandCreditPointGMMsg, sHumanName, nPoint, m_PlayObject.m_btCreditPoint),
                MsgColor.Green, MsgType.Hint);
        }
    }
}