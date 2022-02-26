using SystemModule;
using System;
using System.IO;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 重新加载游戏公告
    /// </summary>
    [GameCommand("ReloadLineNotice", "重新加载游戏公告", 10)]
    public class ReloadLineNoticeCommand : BaseCommond
    {
        [DefaultCommand]
        public void ReloadLineNotice(TPlayObject PlayObject)
        {
            if (M2Share.LoadLineNotice(Path.Combine(M2Share.sConfigPath,M2Share.g_Config.sNoticeDir, "LineNotice.txt")))
            {
                PlayObject.SysMsg(M2Share.g_sGameCommandReloadLineNoticeSuccessMsg, TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                PlayObject.SysMsg(M2Share.g_sGameCommandReloadLineNoticeFailMsg, TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }
    }
}