using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [Command("DelDenyChrNameLogon", "", "人物名称", 10)]
    public class DelDenyChrNameLogonCommand : Command
    {
        [ExecuteCommand]
        public void DelDenyChrNameLogon(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sChrName = @Params.Length > 0 ? @Params[0] : "";
            if (sChrName == "")
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var boDelete = false;
            try
            {
                for (var i = 0; i < M2Share.g_DenyChrNameList.Count; i++)
                {
                    //if ((sChrName).CompareTo((M2Share.g_DenyChrNameList[i])) == 0)
                    //{
                    //    //if (((int)M2Share.g_DenyChrNameList[i]) != 0)
                    //    //{
                    //    //    M2Share.SaveDenyChrNameList();
                    //    //}
                    //    M2Share.g_DenyChrNameList.RemoveAt(i);
                    //    PlayObject.SysMsg(sChrName + "已从禁止登录人物列表中删除。", TMsgColor.c_Green, TMsgType.t_Hint);
                    //    boDelete = true;
                    //    break;
                    //}
                }
            }
            finally
            {
            }
            if (!boDelete)
            {
                PlayObject.SysMsg(sChrName + "没有被禁止登录。", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}