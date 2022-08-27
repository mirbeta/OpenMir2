using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 开始工程战役
    /// </summary>
    [GameCommand("ForcedWallconquestWar", "开始攻城战役", "城堡名称", 10)]
    public class ForcedWallconquestWarCommand : BaseCommond
    {
        [DefaultCommand]
        public void ForcedWallconquestWar(string[] @Params, TPlayObject PlayObject)
        {
            if (Params == null)
            {
                return;
            }
            var sCASTLENAME = @Params.Length > 0 ? @Params[0] : "";
            if (sCASTLENAME == "")
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var Castle = M2Share.CastleManager.Find(sCASTLENAME);
            if (Castle != null)
            {
                Castle.m_boUnderWar = !Castle.m_boUnderWar;
                if (Castle.m_boUnderWar)
                {
                    Castle.m_boShowOverMsg = false;
                    Castle.m_WarDate = DateTime.Now;
                    Castle.m_dwStartCastleWarTick = HUtil32.GetTickCount();
                    Castle.StartWallconquestWar();
                    M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_212, M2Share.nServerIndex, "");
                    var s20 = "[" + Castle.m_sName + " 攻城战已经开始]";
                    M2Share.UserEngine.SendBroadCastMsg(s20, MsgType.System);
                    M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_204, M2Share.nServerIndex, s20);
                    Castle.MainDoorControl(true);
                }
                else
                {
                    Castle.StopWallconquestWar();
                }
            }
            else
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandSbkGoldCastleNotFoundMsg, sCASTLENAME), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}