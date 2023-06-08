using M2Server.Castle;
using M2Server.Player;
using SystemModule;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 显示沙巴克收入金币
    /// </summary>
    [Command("ShowSbkGold", "显示沙巴克收入金币", 10)]
    public class ShowSbkGoldCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sCastleName = @params.Length > 0 ? @params[0] : "";
            var sCtr = @params.Length > 1 ? @params[1] : "";
            var sGold = @params.Length > 2 ? @params[2] : "";
            if (!string.IsNullOrEmpty(sCastleName) && sCastleName[0] == '?') {
                playObject.SysMsg(string.Format(CommandHelp.GameCommandParamUnKnow, this.Command.Name, ""), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (string.IsNullOrEmpty(sCastleName)) {
                IList<string> list = new List<string>();
                M2Share.CastleMgr.GetCastleGoldInfo(list);
                for (var i = 0; i < list.Count; i++) {
                    playObject.SysMsg(list[i], MsgColor.Green, MsgType.Hint);
                }
                return;
            }
            var castle = M2Share.CastleMgr.Find(sCastleName);
            if (castle == null) {
                playObject.SysMsg(string.Format(CommandHelp.GameCommandSbkGoldCastleNotFoundMsg, sCastleName), MsgColor.Red, MsgType.Hint);
                return;
            }
            var ctr = sCtr[1];
            var nGold = HUtil32.StrToInt(sGold, -1);
            if (!new List<char>(new[] { '=', '-', '+' }).Contains(ctr) || nGold < 0 || nGold > 100000000) {
                playObject.SysMsg(string.Format(CommandHelp.GameCommandParamUnKnow, this.Command.Name, CommandHelp.GameCommandSbkGoldHelpMsg), MsgColor.Red, MsgType.Hint);
                return;
            }
            switch (ctr) {
                case '=':
                    castle.TotalGold = nGold;
                    break;
                case '-':
                    castle.TotalGold -= 1;
                    break;
                case '+':
                    castle.TotalGold += nGold;
                    break;
            }
            if (castle.TotalGold < 0) {
                castle.TotalGold = 0;
            }
        }
    }
}