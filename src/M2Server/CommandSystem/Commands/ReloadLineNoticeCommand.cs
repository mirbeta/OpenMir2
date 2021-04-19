using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 重新加载游戏公告
    /// </summary>
    [GameCommand("ReloadLineNotice", "重新加载游戏公告", 10)]
    public class ReloadLineNoticeCommand : BaseCommond
    {
        [DefaultCommand]
        public void ReloadLineNotice(string[] @Params, TPlayObject PlayObject)
        {
            var nPermission = @Params.Length > 0 ? int.Parse(@Params[0]) : 0;
            var sParam1 = @Params.Length > 1 ? @Params[1] : "";

            if (PlayObject.m_btPermission < nPermission)
            {
                PlayObject.SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sParam1 != "" && sParam1[1] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (M2Share.LoadLineNotice(M2Share.g_Config.sNoticeDir + "LineNotice.txt"))
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