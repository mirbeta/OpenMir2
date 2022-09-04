using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整当前玩家攻击模式
    /// </summary>
    [GameCommand("AttackMode", "调整当前玩家攻击模式", 0)]
    public class ChangeAttackModeCommand : BaseCommond
    {
        [DefaultCommand]
        public void ChangeAttackMode(PlayObject PlayObject)
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
                    PlayObject.SysMsg(M2Share.sAttackModeOfAll, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_PEACE: // [攻击模式: 和平攻击]
                    PlayObject.SysMsg(M2Share.sAttackModeOfPeaceful, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_DEAR:// [攻击模式: 和平攻击]
                    PlayObject.SysMsg(M2Share.sAttackModeOfDear, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_MASTER:// [攻击模式: 和平攻击]
                    PlayObject.SysMsg(M2Share.sAttackModeOfMaster, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_GROUP:// [攻击模式: 编组攻击]
                    PlayObject.SysMsg(M2Share.sAttackModeOfGroup, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_GUILD:// [攻击模式: 行会攻击]
                    PlayObject.SysMsg(M2Share.sAttackModeOfGuild, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_PKATTACK:// [攻击模式: 红名攻击]
                    PlayObject.SysMsg(M2Share.sAttackModeOfRedWhite, MsgColor.Green, MsgType.Hint);
                    break;
            }
            PlayObject.SendDefMessage(Grobal2.SM_ATTACKMODE, (byte)PlayObject.AttatckMode, 0, 0, 0, "");
        }
    }
}