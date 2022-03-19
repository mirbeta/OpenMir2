using GameSvr.CommandSystem;
using System.Collections;
using SystemModule;

namespace GameSvr
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
            var sCASTLENAME = @Params.Length > 0 ? @Params[0] : "";
            var sCtr = @Params.Length > 1 ? @Params[1] : "";
            var sGold = @Params.Length > 2 ? @Params[2] : "";
            char Ctr;
            int nGold;
            TUserCastle Castle;
            ArrayList List;
            if (sCASTLENAME != "" && sCASTLENAME[0] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Command.Name, ""), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (sCASTLENAME == "")
            {
                List = new ArrayList();
                M2Share.CastleManager.GetCastleGoldInfo(List);
                for (var i = 0; i < List.Count; i++)
                {
                    PlayObject.SysMsg(List[i] as string, MsgColor.Green, MsgType.Hint);
                }
                List = null;
                return;
            }
            Castle = M2Share.CastleManager.Find(sCASTLENAME);
            if (Castle == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandSbkGoldCastleNotFoundMsg, sCASTLENAME), MsgColor.Red, MsgType.Hint);
                return;
            }
            Ctr = sCtr[1];
            nGold = HUtil32.Str_ToInt(sGold, -1);
            if (!new ArrayList(new char[] { '=', '-', '+' }).Contains(Ctr) || nGold < 0 || nGold > 100000000)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Command.Name, M2Share.g_sGameCommandSbkGoldHelpMsg), MsgColor.Red, MsgType.Hint);
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