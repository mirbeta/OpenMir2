using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 查看禁言列表中的内容(支持权限分配)
    /// </summary>
    [Command("ShutupList", "查看禁言列表中的内容", 10)]
    public class ShutupListCommand : Command
    {
        [ExecuteCommand]
        public void ShutupList(PlayObject PlayObject)
        {
            HUtil32.EnterCriticalSection(M2Share.DenySayMsgList);
            try
            {
                var nCount = M2Share.DenySayMsgList.Count;
                if (M2Share.DenySayMsgList.Count <= 0)
                {
                    PlayObject.SysMsg(CommandHelp.GameCommandShutupListIsNullMsg, MsgColor.Green, MsgType.Hint);
                }
                if (nCount > 0)
                {
                    //foreach (var item in M2Share.g_DenySayMsgList)
                    //{
                    //    PlayObject.SysMsg(M2Share.g_DenySayMsgList[item.Key] + ' ' + (((M2Share.g_DenySayMsgList[item.Key]) - HUtil32.GetTickCount()) / 60000).ToString()
                    //        , TMsgColor.c_Green, TMsgType.t_Hint);
                    //}

                    //for (int i = 0; i < M2Share.g_DenySayMsgList.Count; i++)
                    //{
                    //this.SysMsg(M2Share.g_DenySayMsgList[i] + ' ' + ((((uint)M2Share.g_DenySayMsgList[i]) - HUtil32.GetTickCount()) / 60000).ToString(), TMsgColor.c_Green, TMsgType.t_Hint);
                    //}
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.DenySayMsgList);
            }
        }
    }
}