using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 重新加载管理员列表
    /// </summary>
    [GameCommand("LoadAdmin", "重新加载管理员列表", 10)]
    public class LoadAdminCommand : BaseCommond
    {
        [DefaultCommand]
        public void LoadAdmin(PlayObject PlayObject)
        {
            if (PlayObject.m_btPermission < 6)
            {
                return;
            }
            //LocalDB.GetInstance().LoadAdminList();
            // UserEngine.SendServerGroupMsg(213, nServerIndex, '');
            PlayObject.SysMsg("管理员列表重新加载成功...", MsgColor.Green, MsgType.Hint);
        }
    }
}