using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 调整服务器最高上线人数
    /// </summary>
    [GameCommand("ChangeUserFull", "调整服务器最高上限人数", "人数", 10)]
    public class ChangeUserFullCommand : BaseCommond
    {
        [DefaultCommand]
        public void ChangeUserFull(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sUserCount = @Params.Length > 0 ? @Params[0] : "";
            var nCount = HUtil32.Str_ToInt(sUserCount, -1);
            if (sUserCount == "" || nCount < 1 || sUserCount != "" && sUserCount[0] == '?')
            {
                PlayObject.SysMsg("设置服务器最高上线人数。", TMsgColor.c_Red, TMsgType.t_Hint);
                PlayObject.SysMsg("命令格式: @" + this.CommandAttribute.Name + " 人数", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            M2Share.g_Config.nUserFull = nCount;
            PlayObject.SysMsg($"服务器上线人数限制: {nCount}", TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}