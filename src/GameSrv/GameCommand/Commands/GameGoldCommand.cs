using System.Collections;
using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整指定玩家游戏币
    /// </summary>
    [Command("GameGold", "调整指定玩家游戏币", CommandHelp.GameCommandGameGoldHelpMsg, 10)]
    public class GameGoldCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sCtr = @params.Length > 1 ? @params[1] : "";
            var nGold = @params.Length > 2 ? HUtil32.StrToInt(@params[2], 0) : 0;
            var ctr = '1';
            if (!string.IsNullOrEmpty(sCtr)) {
                ctr = sCtr[0];
            }
            if (string.IsNullOrEmpty(sHumanName) || !new ArrayList(new[] { '=', '+', '-' }).Contains(ctr) || nGold < 0 || nGold > 200000000 || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?') {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null) {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            switch (sCtr[0]) {
                case '=':
                    mPlayObject.GameGold = nGold;
                    break;
                case '+':
                    mPlayObject.GameGold += nGold;
                    break;
                case '-':
                    mPlayObject.GameGold -= nGold;
                    break;
            }
            if (M2Share.GameLogGameGold) {
                M2Share.EventSource.AddEventLog(Grobal2.LogGameGold, string.Format(CommandHelp.GameLogMsg1, mPlayObject.MapName, mPlayObject.CurrX, mPlayObject.CurrY,
                    mPlayObject.ChrName, M2Share.Config.GameGoldName, nGold, sCtr[1], playObject.ChrName));
            }
            playObject.GameGoldChanged();
            mPlayObject.SysMsg(string.Format(CommandHelp.GameCommandGameGoldHumanMsg, M2Share.Config.GameGoldName, nGold, mPlayObject.GameGold, M2Share.Config.GameGoldName), MsgColor.Green, MsgType.Hint);
            playObject.SysMsg(string.Format(CommandHelp.GameCommandGameGoldGMMsg, sHumanName, M2Share.Config.GameGoldName, nGold, mPlayObject.GameGold, M2Share.Config.GameGoldName), MsgColor.Green, MsgType.Hint);
        }
    }
}