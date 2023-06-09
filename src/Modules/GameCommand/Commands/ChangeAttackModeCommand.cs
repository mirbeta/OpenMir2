using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 调整当前玩家攻击模式
    /// </summary>
    [Command("AttackMode", "调整当前玩家攻击模式")]
    public class ChangeAttackModeCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            if (PlayerActor.AttatckMode >= AttackMode.HAM_PKATTACK)
            {
                PlayerActor.AttatckMode = 0;
            }
            else
            {
                if (PlayerActor.AttatckMode < AttackMode.HAM_PKATTACK)
                {
                    PlayerActor.AttatckMode++;
                }
                else
                {
                    PlayerActor.AttatckMode = AttackMode.HAM_ALL;
                }
            }
            if (PlayerActor.AttatckMode < AttackMode.HAM_PKATTACK)
            {
                PlayerActor.AttatckMode++;
            }
            else
            {
                PlayerActor.AttatckMode = AttackMode.HAM_ALL;
            }
            switch (PlayerActor.AttatckMode)
            {
                case AttackMode.HAM_ALL:// [攻击模式: 全体攻击]
                    PlayerActor.SysMsg(Settings.AttackModeOfAll, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_PEACE: // [攻击模式: 和平攻击]
                    PlayerActor.SysMsg(Settings.AttackModeOfPeaceful, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_DEAR:// [攻击模式: 和平攻击]
                    PlayerActor.SysMsg(Settings.AttackModeOfDear, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_MASTER:// [攻击模式: 和平攻击]
                    PlayerActor.SysMsg(Settings.AttackModeOfMaster, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_GROUP:// [攻击模式: 编组攻击]
                    PlayerActor.SysMsg(Settings.AttackModeOfGroup, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_GUILD:// [攻击模式: 行会攻击]
                    PlayerActor.SysMsg(Settings.AttackModeOfGuild, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_PKATTACK:// [攻击模式: 红名攻击]
                    PlayerActor.SysMsg(Settings.AttackModeOfRedWhite, MsgColor.Green, MsgType.Hint);
                    break;
            }
            PlayerActor.SendDefMessage(Messages.SM_ATTACKMODE, (byte)PlayerActor.AttatckMode, 0, 0, 0);
        }
    }
}