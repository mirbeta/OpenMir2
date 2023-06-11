using SystemModule;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 删除指定玩家技能
    /// </summary>
    [Command("DelSkill", "删除指定玩家技能", "人物名称 技能名称", 10)]
    public class DelSkillCommand : GameCommand
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
            var herostr = @params.Length > 2 ? @params[2] : "";
            bool boDelAll;
            UserMagic userMagic;
            if (string.IsNullOrEmpty(sHumanName) || (string.IsNullOrEmpty(sSkillName)))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (string.Compare(sSkillName, "All", StringComparison.OrdinalIgnoreCase) == 0)
            {
                boDelAll = true;
            }
            else
            {
                boDelAll = false;
            }
            var mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor == null)
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            for (var i = mIPlayerActor.MagicList.Count - 1; i >= 0; i--)
            {
                if (mIPlayerActor.MagicList.Count <= 0)
                {
                    break;
                }
                userMagic = mIPlayerActor.MagicList[i];
                if (userMagic != null)
                {
                    if (boDelAll)
                    {
                        mIPlayerActor.MagicList.RemoveAt(i);
                    }
                    else
                    {
                        if (string.Compare(userMagic.Magic.Desc, sSkillName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            mIPlayerActor.SendDelMagic(userMagic);
                            mIPlayerActor.MagicList.RemoveAt(i);
                            mIPlayerActor.SysMsg($"技能{sSkillName}已删除。", MsgColor.Green, MsgType.Hint);
                            PlayerActor.SysMsg($"{sHumanName}的技能{sSkillName}已删除。", MsgColor.Green, MsgType.Hint);
                            break;
                        }
                    }
                }
            }
            mIPlayerActor.RecalcAbilitys();
        }
    }
}