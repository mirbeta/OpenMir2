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
        public void ForcedWallconquestWar(string[] @Params, PlayObject PlayObject)
        {
            if (Params == null)
            {
                return;
            }
            var sCastleName = @Params.Length > 0 ? @Params[0] : "";
            if (sCastleName == "")
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var Castle = M2Share.CastleMgr.Find(sCastleName);
            if (Castle != null)
            {
                Castle.UnderWar = !Castle.UnderWar;
                if (Castle.UnderWar)
                {
                    Castle.ShowOverMsg = false;
                    Castle.m_WarDate = DateTime.Now;
                    Castle.StartCastleWarTick = HUtil32.GetTickCount();
                    Castle.StartWallconquestWar();
                    M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_212, M2Share.ServerIndex, "");
                    var s20 = "[" + Castle.sName + " 攻城战已经开始]";
                    M2Share.WorldEngine.SendBroadCastMsg(s20, MsgType.System);
                    M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_204, M2Share.ServerIndex, s20);
                    Castle.MainDoorControl(true);
                }
                else
                {
                    Castle.StopWallconquestWar();
                }
            }
            else
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.GameCommandSbkGoldCastleNotFoundMsg, sCastleName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}