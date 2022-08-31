using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 显示沙巴克收入金币
    /// </summary>
    [GameCommand("ShowSbkGold", "显示沙巴克收入金币", 10)]
    public class ShowSbkGoldCommand : BaseCommond
    {
        [DefaultCommand]
        public void ShowSbkGold(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sCastleName = @Params.Length > 0 ? @Params[0] : "";
            var sCtr = @Params.Length > 1 ? @Params[1] : "";
            var sGold = @Params.Length > 2 ? @Params[2] : "";
            if (sCastleName != "" && sCastleName[0] == '?')
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.g_sGameCommandParamUnKnow, this.GameCommand.Name, ""), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (sCastleName == "")
            {
                IList<string> List = new List<string>();
                M2Share.CastleManager.GetCastleGoldInfo(List);
                for (var i = 0; i < List.Count; i++)
                {
                    PlayObject.SysMsg(List[i], MsgColor.Green, MsgType.Hint);
                }
                List = null;
                return;
            }
            var Castle = M2Share.CastleManager.Find(sCastleName);
            if (Castle == null)
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.g_sGameCommandSbkGoldCastleNotFoundMsg, sCastleName), MsgColor.Red, MsgType.Hint);
                return;
            }
            var Ctr = sCtr[1];
            var nGold = HUtil32.Str_ToInt(sGold, -1);
            if (!new List<char>(new char[] { '=', '-', '+' }).Contains(Ctr) || nGold < 0 || nGold > 100000000)
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.g_sGameCommandParamUnKnow, this.GameCommand.Name, GameCommandConst.g_sGameCommandSbkGoldHelpMsg), MsgColor.Red, MsgType.Hint);
                return;
            }
            switch (Ctr)
            {
                case '=':
                    Castle.m_nTotalGold = nGold;
                    break;
                case '-':
                    Castle.m_nTotalGold -= 1;
                    break;
                case '+':
                    Castle.m_nTotalGold += nGold;
                    break;
            }
            if (Castle.m_nTotalGold < 0)
            {
                Castle.m_nTotalGold = 0;
            }
        }
    }
}