using System.Collections;
using SystemModule;
using SystemModule;
using SystemModule.Enums;

namespace CommandSystem {
    /// <summary>
    /// 调整指定玩家声望点
    /// </summary>
    [Command("CreditPoint", "调整指定玩家声望点", "人物名称 控制符(+,-,=) 声望点数(0-255)", 10)]
    public class CreditPointCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sCtr = @params.Length > 1 ? @params[1] : "";
            var nPoint = @params.Length > 2 ? HUtil32.StrToInt(@params[2], 0) : 0;
            var ctr = '1';
            int nCreditPoint;
            if (!string.IsNullOrEmpty(sCtr)) {
                ctr = sCtr[0];
            }
            if (string.IsNullOrEmpty(sHumanName) || !new ArrayList(new[] { '=', '+', '-' }).Contains(ctr) || nPoint < 0 || nPoint > int.MaxValue
                || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?') {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mIPlayerActor = ModuleShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor == null) {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            switch (sCtr[0]) {
                case '=':
                    if (nPoint >= 0) {
                        mIPlayerActor.CreditPoint = (byte)nPoint;
                    }
                    break;

                case '+':
                    nCreditPoint = mIPlayerActor.CreditPoint + nPoint;
                    if (nPoint >= 0) {
                        mIPlayerActor.CreditPoint = (byte)nCreditPoint;
                    }
                    break;

                case '-':
                    nCreditPoint = mIPlayerActor.CreditPoint - nPoint;
                    if (nPoint >= 0) {
                        mIPlayerActor.CreditPoint = (byte)nCreditPoint;
                    }
                    break;
            }
            mIPlayerActor.SysMsg(string.Format(CommandHelp.GameCommandCreditPointHumanMsg, nPoint, mIPlayerActor.CreditPoint), MsgColor.Green, MsgType.Hint);
            PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandCreditPointGMMsg, sHumanName, nPoint, mIPlayerActor.CreditPoint), MsgColor.Green, MsgType.Hint);
        }
    }
}