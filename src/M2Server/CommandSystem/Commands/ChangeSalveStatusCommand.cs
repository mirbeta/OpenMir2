using SystemModule;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 调整当前玩家属下状态
    /// </summary>
    [GameCommand("ChangeSalveStatus", "调整当前玩家属下状态", 10)]
    public class ChangeSalveStatusCommand : BaseCommond
    {
        [DefaultCommand]
        public void ChangeSalveStatus(string[] @Params, TPlayObject PlayObject)
        {
            PlayObject.m_boSlaveRelax = !PlayObject.m_boSlaveRelax;
            if (PlayObject.m_SlaveList.Count > 0)
            {
                if (PlayObject.m_boSlaveRelax)
                {
                    PlayObject.SysMsg(M2Share.sPetRest, TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    PlayObject.SysMsg(M2Share.sPetAttack, TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
        }
    }
}