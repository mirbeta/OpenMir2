using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 调整当前玩家攻击模式
    /// </summary>
    [Command("AttackMode", "调整当前玩家攻击模式", 0)]
    public class ChangeAttackModeCommand : Command
    {
        [ExecuteCommand]
        public static void ChangeAttackMode(PlayObject PlayObject)
        {
            if (PlayObject.AttatckMode >= AttackMode.HAM_PKATTACK)
            {
                PlayObject.AttatckMode = 0;
            }
            else
            {
                if (PlayObject.AttatckMode < AttackMode.HAM_PKATTACK)
                {
                    PlayObject.AttatckMode++;
                }
                else
                {
                    PlayObject.AttatckMode = AttackMode.HAM_ALL;
                }
            }
            if (PlayObject.AttatckMode < AttackMode.HAM_PKATTACK)
            {
                PlayObject.AttatckMode++;
            }
            else
            {
                PlayObject.AttatckMode = AttackMode.HAM_ALL;
            }
            switch (PlayObject.AttatckMode)
            {
                case AttackMode.HAM_ALL:// [攻击模式: 全体攻击]
                    PlayObject.SysMsg(Settings.AttackModeOfAll, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_PEACE: // [攻击模式: 和平攻击]
                    PlayObject.SysMsg(Settings.AttackModeOfPeaceful, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_DEAR:// [攻击模式: 和平攻击]
                    PlayObject.SysMsg(Settings.AttackModeOfDear, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_MASTER:// [攻击模式: 和平攻击]
                    PlayObject.SysMsg(Settings.AttackModeOfMaster, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_GROUP:// [攻击模式: 编组攻击]
                    PlayObject.SysMsg(Settings.AttackModeOfGroup, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_GUILD:// [攻击模式: 行会攻击]
                    PlayObject.SysMsg(Settings.AttackModeOfGuild, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_PKATTACK:// [攻击模式: 红名攻击]
                    PlayObject.SysMsg(Settings.AttackModeOfRedWhite, MsgColor.Green, MsgType.Hint);
                    break;
            }
            PlayObject.SendDefMessage(Grobal2.SM_ATTACKMODE, (byte)PlayObject.AttatckMode, 0, 0, 0, "");
        }
    }
}