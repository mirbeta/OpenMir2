using GameSrv.Castle;
using GameSrv.Player;
using GameSrv.World;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 开始工程战役
    /// </summary>
    [Command("ForcedWallconquestWar", "开始攻城战役", "城堡名称", 10)]
    public class ForcedWallconquestWarCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sCastleName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sCastleName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            UserCastle castle = M2Share.CastleMgr.Find(sCastleName);
            if (castle != null) {
                castle.UnderWar = !castle.UnderWar;
                if (castle.UnderWar) {
                    castle.ShowOverMsg = false;
                    castle.WarDate = DateTime.Now;
                    castle.StartCastleWarTick = HUtil32.GetTickCount();
                    castle.StartWallconquestWar();
                    WorldServer.SendServerGroupMsg(Messages.SS_212, M2Share.ServerIndex, "");
                    string s20 = "[" + castle.sName + " 攻城战已经开始]";
                    M2Share.WorldEngine.SendBroadCastMsg(s20, MsgType.System);
                    WorldServer.SendServerGroupMsg(Messages.SS_204, M2Share.ServerIndex, s20);
                    castle.MainDoorControl(true);
                }
                else {
                    castle.StopWallconquestWar();
                }
            }
            else {
                playObject.SysMsg(string.Format(CommandHelp.GameCommandSbkGoldCastleNotFoundMsg, sCastleName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}