using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    [GameCommand("ReloadAbuse", "无用", 10)]
    public class ReloadAbuseCommand : BaseCommond
    {
        [DefaultCommand]
        public void ReloadAbuse(string[] @Params, TPlayObject PlayObject)
        {
            var nPermission = @Params.Length > 0 ? int.Parse(@Params[0]) : 0;
            var sParam1 = @Params.Length > 1 ? @Params[1] : "";
            if (sParam1 != "" && sParam1[1] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
        }
    }
}