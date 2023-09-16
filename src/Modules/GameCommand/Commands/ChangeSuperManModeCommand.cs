using SystemModule;
using SystemModule.Enums;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 调整当前玩家进入无敌模式
    /// </summary>
    [Command("ChangeSuperManMode", "进入/退出无敌模式(进入模式后人物不会死亡)", 10)]
    public class ChangeSuperManModeCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            var boFlag = !PlayerActor.SuperMan;
            PlayerActor.SuperMan = boFlag;
            if (PlayerActor.SuperMan)
            {
                PlayerActor.SysMsg(MessageSettings.SupermanMode, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayerActor.SysMsg(MessageSettings.ReleaseSupermanMode, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}