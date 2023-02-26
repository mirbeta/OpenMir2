using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整当前玩家属下状态
    /// </summary>
    [Command("Rest", "调整当前玩家属下状态", 0)]
    public class ChangeSalveStatusCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject) {
            PlayObject.SlaveRelax = !PlayObject.SlaveRelax;
            if (PlayObject.SlaveList.Count > 0) {
                if (PlayObject.SlaveRelax) {
                    PlayObject.SysMsg(Settings.PetRest, MsgColor.Green, MsgType.Hint);
                }
                else {
                    PlayObject.SysMsg(Settings.PetAttack, MsgColor.Green, MsgType.Hint);
                }
            }
        }
    }
}