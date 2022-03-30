using GameSvr.CommandSystem;
using SystemModule;

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
            if (@Params == null)
            {
                return;
            }
            var sParam1 = string.Empty;
            if (@Params.Length > 0)
            {
                sParam1 = @Params.Length > 0 ? @Params[0] : "";
                if (string.IsNullOrEmpty(sParam1))
                {
                    PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.GameCommand.Name, M2Share.g_sGameCommandReloadGuildHelpMsg), MsgColor.Red, MsgType.Hint);
                    return;
                }
            }
            if (M2Share.nServerIndex != 0)
            {
                PlayObject.SysMsg(M2Share.g_sGameCommandReloadGuildOnMasterserver, MsgColor.Red, MsgType.Hint);
                return;
            }
            var Guild = M2Share.GuildManager.FindGuild(sParam1);
            if (Guild == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandReloadGuildNotFoundGuildMsg, sParam1), MsgColor.Red, MsgType.Hint);
                return;
            }
            Guild.LoadGuild();
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandReloadGuildSuccessMsg, sParam1), MsgColor.Red, MsgType.Hint);
            // UserEngine.SendServerGroupMsg(SS_207, nServerIndex, sParam1);
        }
    }
}