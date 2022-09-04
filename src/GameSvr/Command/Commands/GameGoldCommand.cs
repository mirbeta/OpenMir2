using GameSvr.Player;
using System.Collections;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整指定玩家游戏币
    /// </summary>
    [GameCommand("GameGold", "调整指定玩家游戏币", GameCommandConst.g_sGameCommandGameGoldHelpMsg, 10)]
    public class GameGoldCommand : BaseCommond
    {
        [DefaultCommand]
        public void GameGold(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var sCtr = @Params.Length > 1 ? @Params[1] : "";
            var nGold = @Params.Length > 2 ? int.Parse(@Params[2]) : 0;
            var Ctr = '1';
            if (sCtr != "")
            {
                Ctr = sCtr[0];
            }
            if (string.IsNullOrEmpty(sHumanName) || !new ArrayList(new char[] { '=', '+', '-' }).Contains(Ctr) || nGold < 0 || nGold > 200000000 || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?')
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.g_sNowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            switch (sCtr[0])
            {
                case '=':
                    m_PlayObject.m_nGameGold = nGold;
                    break;

                case '+':
                    m_PlayObject.m_nGameGold += nGold;
                    break;

                case '-':
                    m_PlayObject.m_nGameGold -= nGold;
                    break;
            }
            if (M2Share.g_boGameLogGameGold)
            {
                M2Share.AddGameDataLog(string.Format(GameCommandConst.g_sGameLogMsg1, Grobal2.LOG_GAMEGOLD, m_PlayObject.MapName, m_PlayObject.CurrX, m_PlayObject.CurrY,
                    m_PlayObject.CharName, M2Share.g_Config.sGameGoldName, nGold, sCtr[1], PlayObject.CharName));
            }
            PlayObject.GameGoldChanged();
            m_PlayObject.SysMsg(string.Format(GameCommandConst.g_sGameCommandGameGoldHumanMsg, M2Share.g_Config.sGameGoldName, nGold, m_PlayObject.m_nGameGold, M2Share.g_Config.sGameGoldName), MsgColor.Green, MsgType.Hint);
            PlayObject.SysMsg(string.Format(GameCommandConst.g_sGameCommandGameGoldGMMsg, sHumanName, M2Share.g_Config.sGameGoldName, nGold, m_PlayObject.m_nGameGold, M2Share.g_Config.sGameGoldName), MsgColor.Green, MsgType.Hint);
        }
    }
}