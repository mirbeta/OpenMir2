using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 重新加载管理员列表
    /// </summary>
    [Command("LoadAdmin", "重新加载管理员列表", 10)]
    public class LoadAdminCommand : Command
    {
        [ExecuteCommand]
        public static void LoadAdmin(PlayObject PlayObject)
        {
            if (PlayObject.Permission < 6)
            {
                return;
            }
            //LocalDB.GetInstance().LoadAdminList();
            // UserEngine.SendServerGroupMsg(213, nServerIndex, '');
            PlayObject.SysMsg("管理员列表重新加载成功...", MsgColor.Green, MsgType.Hint);
        }
    }
}