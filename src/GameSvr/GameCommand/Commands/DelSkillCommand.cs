using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 删除指定玩家技能
    /// </summary>
    [Command("DelSkill", "删除指定玩家技能", "人物名称 技能名称", 10)]
    public class DelSkillCommand : Command
    {
        [ExecuteCommand]
        public void DelSkill(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            string sSkillName = @Params.Length > 1 ? @Params[1] : "";
            string Herostr = @Params.Length > 2 ? @Params[2] : "";
            bool boDelAll;
            UserMagic UserMagic;
            if (string.IsNullOrEmpty(sHumanName) || (sSkillName == ""))
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
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
            var m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            for (var i = m_PlayObject.MagicList.Count - 1; i >= 0; i--)
            {
                if (m_PlayObject.MagicList.Count <= 0)
                {
                    break;
                }
                UserMagic = m_PlayObject.MagicList[i];
                if (UserMagic != null)
                {
                    if (boDelAll)
                    {
                        m_PlayObject.MagicList.RemoveAt(i);
                    }
                    else
                    {
                        if (string.Compare(UserMagic.Magic.Desc, sSkillName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            m_PlayObject.SendDelMagic(UserMagic);
                            m_PlayObject.MagicList.RemoveAt(i);
                            m_PlayObject.SysMsg($"技能{sSkillName}已删除。", MsgColor.Green, MsgType.Hint);
                            PlayObject.SysMsg($"{sHumanName}的技能{sSkillName}已删除。", MsgColor.Green, MsgType.Hint);
                            break;
                        }
                    }
                }
            }
            m_PlayObject.RecalcAbilitys();
        }
    }
}