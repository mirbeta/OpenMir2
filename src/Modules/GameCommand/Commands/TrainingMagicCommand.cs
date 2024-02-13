using OpenMir2;
using OpenMir2.Packets.ClientPackets;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 调整指定玩家技能
    /// </summary>
    [Command("TrainingMagic", "调整指定玩家技能", "人物名称  技能名称 修炼等级(0-3)", 10)]
    public class TrainingMagicCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sSkillName = @params.Length > 1 ? @params[1] : "";
            var nLevel = @params.Length > 2 ? HUtil32.StrToInt(@params[2], 0) : 0;
            if (!string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?' || string.IsNullOrEmpty(sHumanName) || string.IsNullOrEmpty(sSkillName) || nLevel < 0 || !(nLevel >= 0 && nLevel <= 3))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor == null)
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            var magic = SystemShare.WorldEngine.FindMagic(sSkillName);
            if (magic == null)
            {

                PlayerActor.SysMsg($"{sSkillName} 技能名称不正确!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (mIPlayerActor.IsTrainingSkill(magic.MagicId))
            {

                PlayerActor.SysMsg($"{sSkillName} 技能已修炼过了!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            var userMagic = new UserMagic();
            userMagic.Magic = magic;
            userMagic.MagIdx = magic.MagicId;
            userMagic.Level = (byte)nLevel;
            userMagic.Key = (char)0;
            userMagic.TranPoint = 0;
            mIPlayerActor.MagicList.Add(userMagic);
            mIPlayerActor.SendAddMagic(userMagic);
            mIPlayerActor.RecalcAbilitys();
            PlayerActor.SysMsg($"{sHumanName} 的 {sSkillName} 技能修炼成功!!!", MsgColor.Green, MsgType.Hint);
        }
    }
}