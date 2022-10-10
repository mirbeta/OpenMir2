using GameSvr.Player;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 回到上次死亡地点
    /// </summary>
    [Command("SignMapMove", "回到上次死亡地点", 10)]
    public class SignMapMoveCommand : Command
    {
        [ExecuteCommand]
        public void SignMapMove(PlayObject PlayObject)
        {
            //try
            //{
            //    Envirnoment Envir = null;
            //    if ((PlayObject.m_btPermission >= this.Attributes.nPermissionMin) || M2Share.CanMoveMap(PlayObject.m_sLastMapName))
            //    {
            //        Envir = M2Share.g_MapManager.FindMap(PlayObject.m_sLastMapName);
            //        if (Envir != null)
            //        {
            //            if (Envir.CanWalk(PlayObject.m_nLastCurrX, PlayObject.m_nLastCurrY, true))
            //            {
            //                PlayObject.SpaceMove(PlayObject.m_sLastMapName, PlayObject.m_nLastCurrX, PlayObject.m_nLastCurrY, 0);
            //            }
            //            else
            //            {
            //                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandPositionMoveCanotMoveToMap1, PlayObject.m_sLastMapName, PlayObject.m_nLastCurrX,
            //                    PlayObject.m_nLastCurrY), TMsgColor.c_Green, TMsgType.t_Hint);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        PlayObject.SysMsg(string.Format(M2Share.g_sTheMapDisableMove, PlayObject.m_sLastMapName, Envir.sMapDesc), TMsgColor.c_Red, TMsgType.t_Hint);
            //    }
            //}
            //catch (Exception E)
            //{
            //    M2Share.MainOutMessage("[Exceptioin] TPlayObject.CmdPositionMove");
            //    M2Share.MainOutMessage(e.Message, MessageType.Error);
            //}
        }
    }
}