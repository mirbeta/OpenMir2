using SystemModule;
using SystemModule;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace CommandSystem {
    /// <summary>
    /// 调整指定玩家技能等级
    /// </summary>
    [Command("TrainingSkill", "调整指定玩家技能等级", "人物名称  技能名称 修炼等级(0-3)", 10)]
    public class TrainingSkillCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sSkillName = @params.Length > 1 ? @params[1] : "";
            var nLevel = @params.Length > 2 ? HUtil32.StrToInt(@params[2], 0) : 0;
            UserMagic userMagic;
            if (string.IsNullOrEmpty(sHumanName) || string.IsNullOrEmpty(sSkillName) || nLevel <= 0) {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            nLevel = HUtil32._MIN(3, nLevel);
            var mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor == null) {
                PlayerActor.SysMsg($"{sHumanName}不在线，或在其它服务器上!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            for (var i = 0; i < mIPlayerActor.MagicList.Count; i++) {
                userMagic = mIPlayerActor.MagicList[i];
                //if (string.Compare(UserMagic.MagicInfo.GetMagicName(), sSkillName, true) == 0)
                //{
                //    UserMagic.btLevel = (byte)nLevel;
                //    m_IPlayerActor.SendMsg(m_IPlayerActor, Messages.RM_MAGIC_LVEXP, 0, UserMagic.MagicInfo.wMagicId, UserMagic.btLevel, UserMagic.nTranPoint, "");
                //    m_IPlayerActor.SysMsg(string.Format("{0}的修改炼等级为{1}", sSkillName, nLevel), MsgColor.c_Green, MsgType.t_Hint);
                //    PlayerActor.SysMsg(string.Format("{0}的技能{1}修炼等级为{2}", sHumanName, sSkillName, nLevel), MsgColor.c_Green, MsgType.t_Hint);
                //    break;
                //}
            }
        }
    }
}