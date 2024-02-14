using OpenMir2;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 开始工程战役
    /// </summary>
    [Command("ForcedWallconquestWar", "开始攻城战役", "城堡名称", 10)]
    public class ForcedWallconquestWarCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            string sCastleName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sCastleName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            SystemModule.Castles.IUserCastle castle = SystemShare.CastleMgr.Find(sCastleName);
            if (castle != null)
            {
                castle.UnderWar = !castle.UnderWar;
                if (castle.UnderWar)
                {
                    castle.ShowOverMsg = false;
                    castle.WarDate = DateTime.Now;
                    castle.StartCastleWarTick = HUtil32.GetTickCount();
                    castle.StartWallconquestWar();
                    SystemShare.WorldEngine.SendServerGroupMsg(Messages.SS_212, SystemShare.ServerIndex, "");
                    string s20 = "[" + castle.sName + " 攻城战已经开始]";
                    SystemShare.WorldEngine.SendBroadCastMsg(s20, MsgType.System);
                    SystemShare.WorldEngine.SendServerGroupMsg(Messages.SS_204, SystemShare.ServerIndex, s20);
                    castle.MainDoorControl(true);
                }
                else
                {
                    castle.StopWallconquestWar();
                }
            }
            else
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandSbkGoldCastleNotFoundMsg, sCastleName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}