using SystemModule;
using SystemModule;
using SystemModule.Enums;

namespace CommandSystem {
    /// <summary>
    /// 查看禁言列表中的内容(支持权限分配)
    /// </summary>
    [Command("ShutupList", "查看禁言列表中的内容", 10)]
    public class ShutupListCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor) {
            HUtil32.EnterCriticalSection(ModuleShare.DenySayMsgList);
            try {
                var nCount = ModuleShare.DenySayMsgList.Count;
                if (ModuleShare.DenySayMsgList.Count <= 0) {
                    PlayerActor.SysMsg(CommandHelp.GameCommandShutupListIsNullMsg, MsgColor.Green, MsgType.Hint);
                }
                if (nCount > 0) {
                    //foreach (var item in Settings.g_DenySayMsgList)
                    //{
                    //    PlayerActor.SysMsg(Settings.g_DenySayMsgList[item.Key] + ' ' + (((Settings.g_DenySayMsgList[item.Key]) - HUtil32.GetTickCount()) / 60000).ToString()
                    //        , MsgColor.c_Green, MsgType.t_Hint);
                    //}

                    //for (int i = 0; i < Settings.g_DenySayMsgList.Count; i++)
                    //{
                    //this.SysMsg(Settings.g_DenySayMsgList[i] + ' ' + ((((uint)Settings.g_DenySayMsgList[i]) - HUtil32.GetTickCount()) / 60000).ToString(), MsgColor.c_Green, MsgType.t_Hint);
                    //}
                }
            }
            finally {
                HUtil32.LeaveCriticalSection(ModuleShare.DenySayMsgList);
            }
        }
    }
}