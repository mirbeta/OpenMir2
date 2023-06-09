using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 重新加载管理员列表
    /// </summary>
    [Command("LoadAdmin", "重新加载管理员列表", 10)]
    public class LoadAdminCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            if (PlayerActor.Permission < 6)
            {
                return;
            }
            //LocalDB.GetInstance().LoadAdminList();
            // WorldEngine.SendServerGroupMsg(213, nServerIndex, '');
            PlayerActor.SysMsg("管理员列表重新加载成功...", MsgColor.Green, MsgType.Hint);
        }
    }
}