using GameSrv.Player;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整指定玩家技能
    /// </summary>
    [Command("TrainingMagic", "调整指定玩家技能", "人物名称  技能名称 修炼等级(0-3)", 10)]
    public class TrainingMagicCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            string sSkillName = @params.Length > 1 ? @params[1] : "";
            int nLevel = @params.Length > 2 ? HUtil32.StrToInt(@params[2],0) : 0;
            if (!string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?' || string.IsNullOrEmpty(sHumanName) || string.IsNullOrEmpty(sSkillName) || nLevel < 0 || !(nLevel >= 0 && nLevel <= 3)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null) {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            MagicInfo magic = M2Share.WorldEngine.FindMagic(sSkillName);
            if (magic == null) {

                playObject.SysMsg($"{sSkillName} 技能名称不正确!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (mPlayObject.IsTrainingSkill(magic.MagicId)) {

                playObject.SysMsg($"{sSkillName} 技能已修炼过了!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            UserMagic userMagic = new UserMagic();
            userMagic.Magic = magic;
            userMagic.MagIdx = magic.MagicId;
            userMagic.Level = (byte)nLevel;
            userMagic.Key = (char)0;
            userMagic.TranPoint = 0;
            mPlayObject.MagicList.Add(userMagic);
            mPlayObject.SendAddMagic(userMagic);
            mPlayObject.RecalcAbilitys();
            playObject.SysMsg($"{sHumanName} 的 {sSkillName} 技能修炼成功!!!", MsgColor.Green, MsgType.Hint);
        }
    }
}