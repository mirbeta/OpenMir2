using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 师徒传送，师父可以将徒弟传送到自己身边，徒弟必须允许传送。
    /// </summary>
    [Command("MasterRecall", "师徒传送，师父可以将徒弟传送到自己身边，徒弟必须允许传送。", 0)]
    public class MasterRecallCommand : GameCommand
    {
        [ExecuteCommand]
        public static void MasterRecall(PlayObject PlayObject)
        {
            if (!PlayObject.IsMaster)
            {
                PlayObject.SysMsg("只能师父才能使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayObject.MasterList.Count == 0)
            {
                PlayObject.SysMsg("你的徒弟一个都不在线!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            //if (PlayObject.m_PEnvir.m_boNOMASTERRECALL)
            //{
            //    PlayObject.SysMsg("本地图禁止师徒传送!!!", TMsgColor.c_Red, TMsgType.t_Hint);
            //    return;
            //}
            if ((HUtil32.GetTickCount() - PlayObject.MasterRecallTick) < 10000)
            {
                PlayObject.SysMsg("稍等一会才能再次使用此功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            for (int i = 0; i < PlayObject.MasterList.Count; i++)
            {
                PlayObject MasterHuman = PlayObject.MasterList[i];
                if (MasterHuman.CanMasterRecall)
                {
                    PlayObject.RecallHuman(MasterHuman.ChrName);
                }
                else
                {
                    PlayObject.SysMsg(MasterHuman.ChrName + " 不允许传送!!!", MsgColor.Red, MsgType.Hint);
                }
            }
        }
    }
}