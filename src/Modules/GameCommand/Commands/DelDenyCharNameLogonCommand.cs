using SystemModule;
using SystemModule.Enums;

namespace CommandSystem.Commands
{
    [Command("DelDenyChrNameLogon", "", "人物名称", 10)]
    public class DelDenyChrNameLogonCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sChrName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sChrName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var boDelete = false;
            try
            {
                for (var i = 0; i < SystemShare.DenyChrNameList.Count; i++)
                {
                    //if ((sChrName).CompareTo((M2Share.g_DenyChrNameList[i])) == 0)
                    //{
                    //    //if (((int)M2Share.g_DenyChrNameList[i]) != 0)
                    //    //{
                    //    //    M2Share.SaveDenyChrNameList();
                    //    //}
                    //    M2Share.g_DenyChrNameList.RemoveAt(i);
                    //    PlayerActor.SysMsg(sChrName + "已从禁止登录人物列表中删除。", MsgColor.c_Green, MsgType.t_Hint);
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
                PlayerActor.SysMsg(sChrName + "没有被禁止登录。", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}