using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 开始工程战役
    /// </summary>
    [Command("ForcedWallconquestWar", "开始攻城战役", "城堡名称", 10)]
    public class ForcedWallconquestWarCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (Params == null) {
                return;
            }
            string sCastleName = @Params.Length > 0 ? @Params[0] : "";
            if (string.IsNullOrEmpty(sCastleName)) {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            Castle.UserCastle Castle = M2Share.CastleMgr.Find(sCastleName);
            if (Castle != null) {
                Castle.UnderWar = !Castle.UnderWar;
                if (Castle.UnderWar) {
                    Castle.ShowOverMsg = false;
                    Castle.WarDate = DateTime.Now;
                    Castle.StartCastleWarTick = HUtil32.GetTickCount();
                    Castle.StartWallconquestWar();
                    World.WorldServer.SendServerGroupMsg(Messages.SS_212, M2Share.ServerIndex, "");
                    string s20 = "[" + Castle.sName + " 攻城战已经开始]";
                    M2Share.WorldEngine.SendBroadCastMsg(s20, MsgType.System);
                    World.WorldServer.SendServerGroupMsg(Messages.SS_204, M2Share.ServerIndex, s20);
                    Castle.MainDoorControl(true);
                }
                else {
                    Castle.StopWallconquestWar();
                }
            }
            else {
                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandSbkGoldCastleNotFoundMsg, sCastleName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}