using SystemModule;

namespace GameSvr.CommandSystem.Commands
{
    /// <summary>
    /// 重新加载管理员列表
    /// </summary>
    [GameCommand("ReLoadAdmin", "重新加载管理员列表", 10)]
    public class ReLoadAdminCommand : BaseCommond
    {
        [DefaultCommand]
        public void ReLoadAdmin(string[] @params, TPlayObject playObject)
        {
            M2Share.LocalDB.LoadAdminList();
            M2Share.UserEngine.SendServerGroupMsg(213, M2Share.nServerIndex, "");
            playObject.SysMsg("管理员列表重新加载成功...", TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}