using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 重新读取行会
    /// </summary>
    [GameCommand("ReloadGuild", "重新读取行会", 10)]
    public class ReloadGuildCommand : BaseCommond
    {
        [DefaultCommand]
        public void ReloadGuild(string[] @Params, TPlayObject PlayObject)
        {
            var nPermission = @Params.Length > 0 ? int.Parse(@Params[0]) : 0;
            var sParam1 = @Params.Length > 1 ? @Params[1] : "";
            if (sParam1 == "" || sParam1 != "" && sParam1[1] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, M2Share.g_sGameCommandReloadGuildHelpMsg),
                    TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (M2Share.nServerIndex != 0)
            {
                PlayObject.SysMsg(M2Share.g_sGameCommandReloadGuildOnMasterserver, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var Guild = M2Share.GuildManager.FindGuild(sParam1);
            if (Guild == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandReloadGuildNotFoundGuildMsg, sParam1), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Guild.LoadGuild();
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandReloadGuildSuccessMsg, sParam1), TMsgColor.c_Red, TMsgType.t_Hint);
            // UserEngine.SendServerGroupMsg(SS_207, nServerIndex, sParam1);
        }
    }
}