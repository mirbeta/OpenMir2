using GameSvr.CommandSystem;
using System;
using System.Collections;
using SystemModule;

namespace GameSvr
{
    /// <summary>
    /// 调整指定玩家声望
    /// </summary>
    [GameCommand("GamePoint", "调整指定玩家声望", M2Share.g_sGameCommandGamePointHelpMsg, 10)]
    public class GamePointCommand : BaseCommond
    {
        [DefaultCommand]
        public void GamePoint(string[] @params, TPlayObject PlayObject)
        {
            if (@params == null)
            {
                return;
            }
            TPlayObject m_PlayObject;
            char Ctr = '1';
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            string sCtr = @params.Length > 1 ? @params[1] : "";
            var nPoint = @params.Length > 2 ? Convert.ToUInt16(@params[2]) : 0;
            if (string.IsNullOrEmpty(sHumanName))
            {
                return;
            }
            if (sCtr != "")
            {
                Ctr = sCtr[0];
            }
            if (string.IsNullOrEmpty(sHumanName) || !new ArrayList(new char[] { '=', '+', '-' }).Contains(Ctr) || nPoint < 0 || nPoint > 100000000
                || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?')
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            switch (sCtr[1])
            {
                case '=':
                    m_PlayObject.m_nGamePoint = (ushort)nPoint;
                    break;

                case '+':
                    m_PlayObject.m_nGamePoint += (ushort)nPoint;
                    break;

                case '-':
                    m_PlayObject.m_nGamePoint -= (ushort)nPoint;
                    break;
            }
            if (M2Share.g_boGameLogGamePoint)
            {
                //M2Share.AddGameDataLog(string.Format(M2Share.g_sGameLogMsg1, M2Share.LOG_GAMEPOINT, m_PlayObject.m_sMapName, m_PlayObject.m_nCurrX, m_PlayObject.m_nCurrY,
                //    m_PlayObject.m_sCharName, M2Share.g_Config.sGamePointName, nPoint, sCtr[1], m_PlayObject.m_sCharName));
            }
            PlayObject.GameGoldChanged();
            m_PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandGamePointHumanMsg, nPoint, m_PlayObject.m_nGamePoint), MsgColor.Green, MsgType.Hint);
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandGamePointGMMsg, sHumanName, nPoint, m_PlayObject.m_nGamePoint), MsgColor.Green, MsgType.Hint);
        }
    }
}