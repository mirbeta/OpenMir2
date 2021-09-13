using GameSvr.CommandSystem;
using System;
using SystemModule;

namespace GameSvr
{
    /// <summary>
    /// 调整指定玩家技能
    /// </summary>
    [GameCommand("TrainingMagic", "调整指定玩家技能", 10)]
    public class TrainingMagicCommand : BaseCommond
    {
        [DefaultCommand]
        public void TrainingMagic(string[] @Params, TPlayObject PlayObject)
        {
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var sSkillName = @Params.Length > 1 ? @Params[1] : "";
            var nLevel = @Params.Length > 2 ? Convert.ToInt32(@Params[2]) : 0;
            var HeroStr = @Params.Length > 3 ? @Params[3] : "";

            TMagic Magic;
            TUserMagic UserMagic = null;
            TPlayObject m_PlayObject;
            if (sHumanName != "" && sHumanName[0] == '?' || sHumanName == "" || sSkillName == "" || nLevel < 0 || !(nLevel >= 0 && nLevel <= 3))
            {
                PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " 人物名称  技能名称 修炼等级(0-3) hero", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            Magic = M2Share.UserEngine.FindMagic(sSkillName);
            if (Magic == null)
            {

                PlayObject.SysMsg(string.Format("%s 技能名称不正确！！！", sSkillName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (m_PlayObject.IsTrainingSkill(Magic.wMagicID))
            {

                PlayObject.SysMsg(string.Format("%s 技能已修炼过了！！！", sSkillName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            UserMagic = new TUserMagic();
            UserMagic.MagicInfo = Magic;
            UserMagic.wMagIdx = Magic.wMagicID;
            UserMagic.btLevel = (byte)nLevel;
            UserMagic.btKey = 0;
            UserMagic.nTranPoint = 0;
            m_PlayObject.m_MagicList.Add(UserMagic);
            m_PlayObject.SendAddMagic(UserMagic);
            m_PlayObject.RecalcAbilitys();
            PlayObject.SysMsg(string.Format("{0} 的 {1} 技能修炼成功！！！", sHumanName, sSkillName), TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}