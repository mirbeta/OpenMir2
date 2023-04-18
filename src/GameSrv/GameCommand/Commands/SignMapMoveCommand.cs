using GameSrv.Player;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 回到上次死亡地点
    /// </summary>
    [Command("SignMapMove", "回到上次死亡地点", 10)]
    public class SignMapMoveCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            //try
            //{
            //    Envirnoment Envir = null;
            //    if ((PlayObject.m_btPermission >= this.Attributes.nPermissionMin) || M2Share.CanMoveMap(PlayObject.m_sLastMapName))
            //    {
            //        Envir = Settings.g_MapMgr.FindMap(PlayObject.m_sLastMapName);
            //        if (Envir != null)
            //        {
            //            if (Envir.CanWalk(PlayObject.m_nLastCurrX, PlayObject.m_nLastCurrY, true))
            //            {
            //                PlayObject.SpaceMove(PlayObject.m_sLastMapName, PlayObject.m_nLastCurrX, PlayObject.m_nLastCurrY, 0);
            //            }
            //            else
            //            {
            //                PlayObject.SysMsg(string.Format(Settings.GameCommandPositionMoveCanotMoveToMap1, PlayObject.m_sLastMapName, PlayObject.m_nLastCurrX,
            //                    PlayObject.m_nLastCurrY), MsgColor.c_Green, MsgType.t_Hint);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        PlayObject.SysMsg(string.Format(Settings.TheMapDisableMove, PlayObject.m_sLastMapName, Envir.sMapDesc), MsgColor.c_Red, MsgType.t_Hint);
            //    }
            //}
            //catch (Exception E)
            //{
            //    M2Share.Logger.Error("[Exceptioin] PlayObject.CmdPositionMove");
            //    M2Share.Logger.Error(e.Message);
            //}
        }
    }
}