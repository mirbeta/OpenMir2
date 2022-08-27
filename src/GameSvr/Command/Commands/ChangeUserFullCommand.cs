using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
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
            if (sUserCount == "" || nCount < 1 || sUserCount != "")
            {
                PlayObject.SysMsg("设置服务器最高上线人数。", MsgColor.Red, MsgType.Hint);
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            M2Share.g_Config.nUserFull = nCount;
            PlayObject.SysMsg($"服务器上线人数限制: {nCount}", MsgColor.Green, MsgType.Hint);
        }
    }
}