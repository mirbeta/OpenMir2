using GameSvr.Player;
using System.Collections;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 调整指定玩家游戏币
    /// </summary>
    [Command("GameGold", "调整指定玩家游戏币", CommandHelp.GameCommandGameGoldHelpMsg, 10)]
    public class GameGoldCommand : GameCommand
    {
        [ExecuteCommand]
        public void GameGold(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            string sCtr = @Params.Length > 1 ? @Params[1] : "";
            int nGold = @Params.Length > 2 ? int.Parse(@Params[2]) : 0;
            char Ctr = '1';
            if (sCtr != "")
            {
                Ctr = sCtr[0];
            }
            if (string.IsNullOrEmpty(sHumanName) || !new ArrayList(new[] { '=', '+', '-' }).Contains(Ctr) || nGold < 0 || nGold > 200000000 || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?')
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            switch (sCtr[0])
            {
                case '=':
                    m_PlayObject.GameGold = nGold;
                    break;
                case '+':
                    m_PlayObject.GameGold += nGold;
                    break;
                case '-':
                    m_PlayObject.GameGold -= nGold;
                    break;
            }
            if (M2Share.GameLogGameGold)
            {
                M2Share.EventSource.AddEventLog(Grobal2.LogGameGold, string.Format(CommandHelp.GameLogMsg1, m_PlayObject.MapName, m_PlayObject.CurrX, m_PlayObject.CurrY,
                    m_PlayObject.ChrName, M2Share.Config.GameGoldName, nGold, sCtr[1], PlayObject.ChrName));
            }
            PlayObject.GameGoldChanged();
            m_PlayObject.SysMsg(string.Format(CommandHelp.GameCommandGameGoldHumanMsg, M2Share.Config.GameGoldName, nGold, m_PlayObject.GameGold, M2Share.Config.GameGoldName), MsgColor.Green, MsgType.Hint);
            PlayObject.SysMsg(string.Format(CommandHelp.GameCommandGameGoldGMMsg, sHumanName, M2Share.Config.GameGoldName, nGold, m_PlayObject.GameGold, M2Share.Config.GameGoldName), MsgColor.Green, MsgType.Hint);
        }
    }
}