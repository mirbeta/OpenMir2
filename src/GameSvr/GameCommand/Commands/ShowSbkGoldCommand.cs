using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 显示沙巴克收入金币
    /// </summary>
    [Command("ShowSbkGold", "显示沙巴克收入金币", 10)]
    public class ShowSbkGoldCommand : Command
    {
        [ExecuteCommand]
        public void ShowSbkGold(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sCastleName = @Params.Length > 0 ? @Params[0] : "";
            string sCtr = @Params.Length > 1 ? @Params[1] : "";
            string sGold = @Params.Length > 2 ? @Params[2] : "";
            if (sCastleName != "" && sCastleName[0] == '?')
            {
                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandParamUnKnow, this.GameCommand.Name, ""), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (sCastleName == "")
            {
                IList<string> List = new List<string>();
                M2Share.CastleMgr.GetCastleGoldInfo(List);
                for (int i = 0; i < List.Count; i++)
                {
                    PlayObject.SysMsg(List[i], MsgColor.Green, MsgType.Hint);
                }

                return;
            }
            Castle.UserCastle Castle = M2Share.CastleMgr.Find(sCastleName);
            if (Castle == null)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandSbkGoldCastleNotFoundMsg, sCastleName), MsgColor.Red, MsgType.Hint);
                return;
            }
            char Ctr = sCtr[1];
            int nGold = HUtil32.StrToInt(sGold, -1);
            if (!new List<char>(new[] { '=', '-', '+' }).Contains(Ctr) || nGold < 0 || nGold > 100000000)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandParamUnKnow, this.GameCommand.Name, CommandHelp.GameCommandSbkGoldHelpMsg), MsgColor.Red, MsgType.Hint);
                return;
            }
            switch (Ctr)
            {
                case '=':
                    Castle.TotalGold = nGold;
                    break;
                case '-':
                    Castle.TotalGold -= 1;
                    break;
                case '+':
                    Castle.TotalGold += nGold;
                    break;
            }
            if (Castle.TotalGold < 0)
            {
                Castle.TotalGold = 0;
            }
        }
    }
}