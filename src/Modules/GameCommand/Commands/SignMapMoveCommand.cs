using SystemModule;

namespace CommandModule.Commands
{
    /// <summary>
    /// 回到上次死亡地点
    /// </summary>
    [Command("SignMapMove", "回到上次死亡地点", 10)]
    public class SignMapMoveCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            //try
            //{
            //    Envirnoment Envir = null;
            //    if ((PlayerActor.SysMsgm_btPermission >= this.Attributes.nPermissionMin) || M2Share.CanMoveMap(PlayerActor.SysMsgm_sLastMapName))
            //    {
            //        Envir = Settings.g_MapMgr.FindMap(PlayerActor.SysMsgm_sLastMapName);
            //        if (Envir != null)
            //        {
            //            if (Envir.CanWalk(PlayerActor.SysMsgm_nLastCurrX, PlayerActor.SysMsgm_nLastCurrY, true))
            //            {
            //                PlayerActor.SpaceMove(PlayerActor.SysMsgm_sLastMapName, PlayerActor.SysMsgm_nLastCurrX, PlayerActor.SysMsgm_nLastCurrY, 0);
            //            }
            //            else
            //            {
            //                PlayerActor.SysMsg(string.Format(Settings.GameCommandPositionMoveCanotMoveToMap1, PlayerActor.SysMsgm_sLastMapName, PlayerActor.SysMsgm_nLastCurrX,
            //                    PlayerActor.SysMsgm_nLastCurrY), MsgColor.c_Green, MsgType.t_Hint);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        PlayerActor.SysMsg(string.Format(Settings.TheMapDisableMove, PlayerActor.SysMsgm_sLastMapName, Envir.sMapDesc), MsgColor.c_Red, MsgType.t_Hint);
            //    }
            //}
            //catch (Exception E)
            //{
            //    M2Share.Logger.Error("[Exceptioin] PlayerActor.SysMsgCmdPositionMove");
            //    M2Share.Logger.Error(e.Message);
            //}
        }
    }
}