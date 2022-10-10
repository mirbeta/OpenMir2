using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整指定玩家技能
    /// </summary>
    [Command("TrainingMagic", "调整指定玩家技能", "人物名称  技能名称 修炼等级(0-3)", 10)]
    public class TrainingMagicCommand : Commond
    {
        [ExecuteCommand]
        public void TrainingMagic(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var sSkillName = @Params.Length > 1 ? @Params[1] : "";
            var nLevel = @Params.Length > 2 ? Convert.ToInt32(@Params[2]) : 0;
            if (!string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?' || string.IsNullOrEmpty(sHumanName) || sSkillName == "" || nLevel < 0 || !(nLevel >= 0 && nLevel <= 3))
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            var Magic = M2Share.WorldEngine.FindMagic(sSkillName);
            if (Magic == null)
            {

                PlayObject.SysMsg($"{sSkillName} 技能名称不正确!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (m_PlayObject.IsTrainingSkill(Magic.MagicId))
            {

                PlayObject.SysMsg($"{sSkillName} 技能已修炼过了!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            UserMagic UserMagic = new UserMagic();
            UserMagic.Magic = Magic;
            UserMagic.MagIdx = Magic.MagicId;
            UserMagic.Level = (byte)nLevel;
            UserMagic.Key = (char)0;
            UserMagic.TranPoint = 0;
            m_PlayObject.MagicList.Add(UserMagic);
            m_PlayObject.SendAddMagic(UserMagic);
            m_PlayObject.RecalcAbilitys();
            PlayObject.SysMsg($"{sHumanName} 的 {sSkillName} 技能修炼成功!!!", MsgColor.Green, MsgType.Hint);
        }
    }
}