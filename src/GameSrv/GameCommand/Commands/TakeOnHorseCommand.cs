using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("TakeOnHorse", "", 10)]
    public class TakeOnHorseCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (playObject.OnHorse) {
                return;
            }
            if (playObject.HorseType == 0) {
                playObject.SysMsg("骑马必须先戴上马牌!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            playObject.OnHorse = true;
            playObject.FeatureChanged();
            if (playObject.OnHorse) {
                M2Share.FunctionNPC.GotoLable(playObject, "@OnHorse", false);
            }
        }
    }
}