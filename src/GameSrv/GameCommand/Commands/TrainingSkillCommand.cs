using GameSrv.Player;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整指定玩家技能等级
    /// </summary>
    [Command("TrainingSkill", "调整指定玩家技能等级", "人物名称  技能名称 修炼等级(0-3)", 10)]
    public class TrainingSkillCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            string sSkillName = @params.Length > 1 ? @params[1] : "";
            int nLevel = @params.Length > 2 ? HUtil32.StrToInt(@params[2], 0) : 0;
            UserMagic userMagic;
            if (string.IsNullOrEmpty(sHumanName) || string.IsNullOrEmpty(sSkillName) || nLevel <= 0) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            nLevel = HUtil32._MIN(3, nLevel);
            PlayObject mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null) {
                playObject.SysMsg($"{sHumanName}不在线，或在其它服务器上!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            for (int i = 0; i < mPlayObject.MagicList.Count; i++) {
                userMagic = mPlayObject.MagicList[i];
                //if (string.Compare(UserMagic.MagicInfo.GetMagicName(), sSkillName, true) == 0)
                //{
                //    UserMagic.btLevel = (byte)nLevel;
                //    m_PlayObject.SendMsg(m_PlayObject, Messages.RM_MAGIC_LVEXP, 0, UserMagic.MagicInfo.wMagicId, UserMagic.btLevel, UserMagic.nTranPoint, "");
                //    m_PlayObject.SysMsg(string.Format("{0}的修改炼等级为{1}", sSkillName, nLevel), TMsgColor.c_Green, TMsgType.t_Hint);
                //    PlayObject.SysMsg(string.Format("{0}的技能{1}修炼等级为{2}", sHumanName, sSkillName, nLevel), TMsgColor.c_Green, TMsgType.t_Hint);
                //    break;
                //}
            }
        }
    }
}