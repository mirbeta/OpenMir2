using SystemModule;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 调整当前玩家攻击模式
    /// </summary>
    [GameCommand("ChangeAttackMode", "调整当前玩家攻击模式", 10)]
    public class ChangeAttackModeCommand : BaseCommond
    {
        [DefaultCommand]
        public void ChangeAttackMode(string[] @Params, TPlayObject PlayObject)
        {
            var nMode = @Params.Length > 0 ? int.Parse(@Params[0]) : 0;
            if ((nMode >= 0) && (nMode <= 4))
            {
                PlayObject.m_btAttatckMode = (byte)nMode;
            }
            else
            {
                if (PlayObject.m_btAttatckMode < M2Share.HAM_PKATTACK)
                {
                    PlayObject.m_btAttatckMode++;
                }
                else
                {
                    PlayObject.m_btAttatckMode = M2Share.HAM_ALL;
                }
            }
            switch (PlayObject.m_btAttatckMode)
            {
                case M2Share.HAM_ALL: // [攻击模式: 全体攻击]
                    PlayObject.SysMsg(M2Share.sAttackModeOfAll, TMsgColor.c_Green, TMsgType.t_Hint);
                    break;
                case M2Share.HAM_PEACE: // [攻击模式: 和平攻击]
                    PlayObject.SysMsg(M2Share.sAttackModeOfPeaceful, TMsgColor.c_Green, TMsgType.t_Hint);
                    break;
                case M2Share.HAM_DEAR: // [攻击模式: 和平攻击]
                    PlayObject.SysMsg(M2Share.sAttackModeOfDear, TMsgColor.c_Green, TMsgType.t_Hint);
                    break;
                case M2Share.HAM_MASTER:// [攻击模式: 和平攻击]
                    PlayObject.SysMsg(M2Share.sAttackModeOfMaster, TMsgColor.c_Green, TMsgType.t_Hint);
                    break;
                case M2Share.HAM_GROUP:// [攻击模式: 编组攻击]
                    PlayObject.SysMsg(M2Share.sAttackModeOfGroup, TMsgColor.c_Green, TMsgType.t_Hint);
                    break;
                case M2Share.HAM_GUILD: // [攻击模式: 行会攻击]
                    PlayObject.SysMsg(M2Share.sAttackModeOfGuild, TMsgColor.c_Green, TMsgType.t_Hint);
                    break;
                case M2Share.HAM_PKATTACK:// [攻击模式: 红名攻击]
                    PlayObject.SysMsg(M2Share.sAttackModeOfRedWhite, TMsgColor.c_Green, TMsgType.t_Hint);
                    break;
            }
            PlayObject.SendDefMessage(grobal2.SM_ATTACKMODE, PlayObject.m_btAttatckMode, 0, 0, 0, "");
        }
    }
}