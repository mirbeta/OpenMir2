using M2Server.Player;
using M2Server;
using SystemModule;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 调整当前玩家攻击模式
    /// </summary>
    [Command("AttackMode", "调整当前玩家攻击模式")]
    public class ChangeAttackModeCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (playObject.AttatckMode >= AttackMode.HAM_PKATTACK) {
                playObject.AttatckMode = 0;
            }
            else {
                if (playObject.AttatckMode < AttackMode.HAM_PKATTACK) {
                    playObject.AttatckMode++;
                }
                else {
                    playObject.AttatckMode = AttackMode.HAM_ALL;
                }
            }
            if (playObject.AttatckMode < AttackMode.HAM_PKATTACK) {
                playObject.AttatckMode++;
            }
            else {
                playObject.AttatckMode = AttackMode.HAM_ALL;
            }
            switch (playObject.AttatckMode) {
                case AttackMode.HAM_ALL:// [攻击模式: 全体攻击]
                    playObject.SysMsg(Settings.AttackModeOfAll, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_PEACE: // [攻击模式: 和平攻击]
                    playObject.SysMsg(Settings.AttackModeOfPeaceful, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_DEAR:// [攻击模式: 和平攻击]
                    playObject.SysMsg(Settings.AttackModeOfDear, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_MASTER:// [攻击模式: 和平攻击]
                    playObject.SysMsg(Settings.AttackModeOfMaster, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_GROUP:// [攻击模式: 编组攻击]
                    playObject.SysMsg(Settings.AttackModeOfGroup, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_GUILD:// [攻击模式: 行会攻击]
                    playObject.SysMsg(Settings.AttackModeOfGuild, MsgColor.Green, MsgType.Hint);
                    break;
                case AttackMode.HAM_PKATTACK:// [攻击模式: 红名攻击]
                    playObject.SysMsg(Settings.AttackModeOfRedWhite, MsgColor.Green, MsgType.Hint);
                    break;
            }
            playObject.SendDefMessage(Messages.SM_ATTACKMODE, (byte)playObject.AttatckMode, 0, 0, 0);
        }
    }
}