using GameSrv.Player;
using System.Collections;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整指定玩家声望点
    /// </summary>
    [Command("CreditPoint", "调整指定玩家声望点", "人物名称 控制符(+,-,=) 声望点数(0-255)", 10)]
    public class CreditPointCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            string sCtr = @Params.Length > 1 ? @Params[1] : "";
            int nPoint = @Params.Length > 2 ? HUtil32.StrToInt(@Params[2], 0) : 0;
            char Ctr = '1';
            int nCreditPoint;
            if (!string.IsNullOrEmpty(sCtr)) {
                Ctr = sCtr[0];
            }
            if (string.IsNullOrEmpty(sHumanName) || !new ArrayList(new[] { '=', '+', '-' }).Contains(Ctr) || nPoint < 0 || nPoint > int.MaxValue
                || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?') {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null) {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            switch (sCtr[0]) {
                case '=':
                    if (nPoint >= 0) {
                        m_PlayObject.CreditPoint = (byte)nPoint;
                    }
                    break;

                case '+':
                    nCreditPoint = m_PlayObject.CreditPoint + nPoint;
                    if (nPoint >= 0) {
                        m_PlayObject.CreditPoint = (byte)nCreditPoint;
                    }
                    break;

                case '-':
                    nCreditPoint = m_PlayObject.CreditPoint - nPoint;
                    if (nPoint >= 0) {
                        m_PlayObject.CreditPoint = (byte)nCreditPoint;
                    }
                    break;
            }
            m_PlayObject.SysMsg(string.Format(CommandHelp.GameCommandCreditPointHumanMsg, nPoint, m_PlayObject.CreditPoint), MsgColor.Green, MsgType.Hint);
            PlayObject.SysMsg(string.Format(CommandHelp.GameCommandCreditPointGMMsg, sHumanName, nPoint, m_PlayObject.CreditPoint), MsgColor.Green, MsgType.Hint);
        }
    }
}