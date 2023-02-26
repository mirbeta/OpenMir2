using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整当前玩家进入无敌模式
    /// </summary>
    [Command("ChangeSuperManMode", "进入/退出无敌模式(进入模式后人物不会死亡)", 10)]
    public class ChangeSuperManModeCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject) {
            bool boFlag = !PlayObject.SuperMan;
            PlayObject.SuperMan = boFlag;
            if (PlayObject.SuperMan) {
                PlayObject.SysMsg(Settings.SupermanMode, MsgColor.Green, MsgType.Hint);
            }
            else {
                PlayObject.SysMsg(Settings.ReleaseSupermanMode, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}