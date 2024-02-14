using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 调整当前玩家管理模式
    /// </summary>
    [Command("ChangeAdminMode", "进入/退出管理员模式(进入模式后不会受到任何角色攻击)", 10)]
    public class ChangeAdminModeCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            PlayerActor.AdminMode = !PlayerActor.AdminMode;
            PlayerActor.SysMsg(PlayerActor.AdminMode ? MessageSettings.GameMasterMode : MessageSettings.ReleaseGameMasterMode, MsgColor.Green, MsgType.Hint);
        }
    }
}