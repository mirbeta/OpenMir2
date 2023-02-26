using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 查看禁言列表中的内容(支持权限分配)
    /// </summary>
    [Command("ShutupList", "查看禁言列表中的内容", 10)]
    public class ShutupListCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject) {
            HUtil32.EnterCriticalSection(M2Share.DenySayMsgList);
            try {
                int nCount = M2Share.DenySayMsgList.Count;
                if (M2Share.DenySayMsgList.Count <= 0) {
                    PlayObject.SysMsg(CommandHelp.GameCommandShutupListIsNullMsg, MsgColor.Green, MsgType.Hint);
                }
                if (nCount > 0) {
                    //foreach (var item in Settings.g_DenySayMsgList)
                    //{
                    //    PlayObject.SysMsg(Settings.g_DenySayMsgList[item.Key] + ' ' + (((Settings.g_DenySayMsgList[item.Key]) - HUtil32.GetTickCount()) / 60000).ToString()
                    //        , TMsgColor.c_Green, TMsgType.t_Hint);
                    //}

                    //for (int i = 0; i < Settings.g_DenySayMsgList.Count; i++)
                    //{
                    //this.SysMsg(Settings.g_DenySayMsgList[i] + ' ' + ((((uint)Settings.g_DenySayMsgList[i]) - HUtil32.GetTickCount()) / 60000).ToString(), TMsgColor.c_Green, TMsgType.t_Hint);
                    //}
                }
            }
            finally {
                HUtil32.LeaveCriticalSection(M2Share.DenySayMsgList);
            }
        }
    }
}