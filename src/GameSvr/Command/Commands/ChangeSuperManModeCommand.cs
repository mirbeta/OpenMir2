using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整当前玩家进入无敌模式
    /// </summary>
    [GameCommand("ChangeSuperManMode", "进入/退出无敌模式(进入模式后人物不会死亡)", 10)]
    public class ChangeSuperManModeCommand : BaseCommond
    {
        [DefaultCommand]
        public void ChangeSuperManMode(PlayObject PlayObject)
        {
            var boFlag = !PlayObject.SuperMan;
            PlayObject.SuperMan = boFlag;
            if (PlayObject.SuperMan)
            {
                PlayObject.SysMsg(M2Share.sSupermanMode, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(M2Share.sReleaseSupermanMode, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}