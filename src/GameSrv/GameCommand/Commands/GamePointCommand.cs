using System.Collections;
using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整指定玩家声望
    /// </summary>
    [Command("GamePoint", "调整指定玩家声望", CommandHelp.GameCommandGamePointHelpMsg, 10)]
    public class GamePointCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var ctr = '1';
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sCtr = @params.Length > 1 ? @params[1] : "";
            var nPoint = @params.Length > 2 ? Convert.ToUInt16(@params[2]) : 0;
            if (string.IsNullOrEmpty(sHumanName)) {
                return;
            }
            if (!string.IsNullOrEmpty(sCtr)) {
                ctr = sCtr[0];
            }
            if (string.IsNullOrEmpty(sHumanName) || !new ArrayList(new[] { '=', '+', '-' }).Contains(ctr) || nPoint < 0 || nPoint > 100000000
                || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?') {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mPlayObject = GameShare.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null) {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            switch (sCtr[1]) {
                case '=':
                    mPlayObject.GamePoint = (ushort)nPoint;
                    break;

                case '+':
                    mPlayObject.GamePoint += (ushort)nPoint;
                    break;

                case '-':
                    mPlayObject.GamePoint -= (ushort)nPoint;
                    break;
            }
            if (GameShare.GameLogGamePoint) {
                //M2Share.ItemEventSource.AddGameLog(string.Format(Settings.GameLogMsg1, M2Share.LOG_GAMEPOINT, m_PlayObject.MapName, m_PlayObject.CurrX, m_PlayObject.CurrY,
                //    m_PlayObject.m_sChrName, Settings.Config.sGamePointName, nPoint, sCtr[1], m_PlayObject.m_sChrName));
            }
            playObject.GameGoldChanged();
            mPlayObject.SysMsg(string.Format(CommandHelp.GameCommandGamePointHumanMsg, nPoint, mPlayObject.GamePoint), MsgColor.Green, MsgType.Hint);
            playObject.SysMsg(string.Format(CommandHelp.GameCommandGamePointGMMsg, sHumanName, nPoint, mPlayObject.GamePoint), MsgColor.Green, MsgType.Hint);
        }
    }
}