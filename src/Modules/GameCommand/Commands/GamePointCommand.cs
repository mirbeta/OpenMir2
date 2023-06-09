using System.Collections;
using SystemModule;
using SystemModule.Enums;

namespace CommandSystem {
    /// <summary>
    /// 调整指定玩家声望
    /// </summary>
    [Command("GamePoint", "调整指定玩家声望", CommandHelp.GameCommandGamePointHelpMsg, 10)]
    public class GamePointCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor) {
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
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor == null) {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            switch (sCtr[1]) {
                case '=':
                    mIPlayerActor.GamePoint = (ushort)nPoint;
                    break;

                case '+':
                    mIPlayerActor.GamePoint += (ushort)nPoint;
                    break;

                case '-':
                    mIPlayerActor.GamePoint -= (ushort)nPoint;
                    break;
            }
            if (SystemShare.GameLogGamePoint) {
                //M2Share.ItemEventSource.AddGameLog(string.Format(Settings.GameLogMsg1, M2Share.LOG_GAMEPOINT, m_IPlayerActor.MapName, m_IPlayerActor.CurrX, m_IPlayerActor.CurrY,
                //    m_IPlayerActor.m_sChrName, Settings.Config.sGamePointName, nPoint, sCtr[1], m_IPlayerActor.m_sChrName));
            }
            PlayerActor.GameGoldChanged();
            mIPlayerActor.SysMsg(string.Format(CommandHelp.GameCommandGamePointHumanMsg, nPoint, mIPlayerActor.GamePoint), MsgColor.Green, MsgType.Hint);
            PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandGamePointGMMsg, sHumanName, nPoint, mIPlayerActor.GamePoint), MsgColor.Green, MsgType.Hint);
        }
    }
}