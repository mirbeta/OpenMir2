using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整指定玩家权限
    /// </summary>
    [GameCommand("SetPermission", "调整指定玩家权限", "人物名称 权限等级(0 - 10)", 10)]
    public class SetPermissionCommand : BaseCommond
    {
        [DefaultCommand]
        public void SetPermission(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var sPermission = @Params.Length > 1 ? @Params[1] : "";
            var nPerission = HUtil32.Str_ToInt(sPermission, 0);
            const string sOutFormatMsg = "[权限调整] {0} [{1} {2} -> {3}]";
            if (string.IsNullOrEmpty(sHumanName) || !(nPerission >= 0 && nPerission <= 10))
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (M2Share.Config.ShowMakeItemMsg)
            {
                M2Share.Log.Warn(string.Format(sOutFormatMsg, PlayObject.CharName, m_PlayObject.CharName, m_PlayObject.Permission, nPerission));
            }
            m_PlayObject.Permission = (byte)nPerission;
            PlayObject.SysMsg(sHumanName + " 当前权限为: " + m_PlayObject.Permission, MsgColor.Red, MsgType.Hint);
        }
    }
}