using GameSvr.Player;
using System.Collections;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整指定玩家游戏币
    /// </summary>
    [Command("GameGold", "调整指定玩家游戏币", CommandHelp.GameCommandGameGoldHelpMsg, 10)]
    public class GameGoldCommand : Commond
    {
        [ExecuteCommand]
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
            var m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
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
                M2Share.AddGameDataLog(string.Format(CommandHelp.GameLogMsg1, Grobal2.LOG_GAMEGOLD, m_PlayObject.MapName, m_PlayObject.CurrX, m_PlayObject.CurrY,
                    m_PlayObject.ChrName, M2Share.Config.GameGoldName, nGold, sCtr[1], PlayObject.ChrName));
            }
            PlayObject.GameGoldChanged();
            m_PlayObject.SysMsg(string.Format(CommandHelp.GameCommandGameGoldHumanMsg, M2Share.Config.GameGoldName, nGold, m_PlayObject.m_nGameGold, M2Share.Config.GameGoldName), MsgColor.Green, MsgType.Hint);
            PlayObject.SysMsg(string.Format(CommandHelp.GameCommandGameGoldGMMsg, sHumanName, M2Share.Config.GameGoldName, nGold, m_PlayObject.m_nGameGold, M2Share.Config.GameGoldName), MsgColor.Green, MsgType.Hint);
        }
    }
}