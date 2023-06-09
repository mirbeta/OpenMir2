using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 调整指定玩家权限
    /// </summary>
    [Command("SetPermission", "调整指定玩家权限", "人物名称 权限等级(0 - 10)", 10)]
    public class SetPermissionCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sPermission = @params.Length > 1 ? @params[1] : "";
            var nPerission = HUtil32.StrToInt(sPermission, 0);
            const string sOutFormatMsg = "[权限调整] {0} [{1} {2} -> {3}]";
            if (string.IsNullOrEmpty(sHumanName) || !(nPerission >= 0 && nPerission <= 10))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mIPlayerActor = ModuleShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor == null)
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (ModuleShare.Config.ShowMakeItemMsg)
            {
                ModuleShare.Logger.Warn(string.Format(sOutFormatMsg, PlayerActor.ChrName, mIPlayerActor.ChrName, mIPlayerActor.Permission, nPerission));
            }
            mIPlayerActor.Permission = (byte)nPerission;
            PlayerActor.SysMsg(sHumanName + " 当前权限为: " + mIPlayerActor.Permission, MsgColor.Red, MsgType.Hint);
        }
    }
}