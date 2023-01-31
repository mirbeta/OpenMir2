using GameSvr.Player;
using SystemModule;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 调整指定玩家属性点
    /// </summary>
    [Command("BonuPoint", "调整指定玩家属性点", "人物名称 属性点数(不输入为查看点数)", 10)]
    public class BonuPointCommand : Command
    {
        [ExecuteCommand]
        public void BonuPoint(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumName = @Params.Length > 0 ? @Params[0] : "";
            var nCount = @Params.Length > 1 ? int.Parse(@Params[1]) : 0;
            if (string.IsNullOrEmpty(sHumName))
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (nCount > 0)
            {
                m_PlayObject.BonusPoint = nCount;
                m_PlayObject.SendMsg(PlayObject, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                return;
            }
            var sMsg = string.Format("未分配点数:{0} 已分配点数:(DC:{1} MC:{2} SC:{3} AC:{4} MAC:{5} HP:{6} MP:{7} HIT:{8} SPEED:{9})", m_PlayObject.BonusPoint,
                m_PlayObject.BonusAbil.DC, m_PlayObject.BonusAbil.MC, m_PlayObject.BonusAbil.SC, m_PlayObject.BonusAbil.AC,
                m_PlayObject.BonusAbil.MAC, m_PlayObject.BonusAbil.HP, m_PlayObject.BonusAbil.MP, m_PlayObject.BonusAbil.Hit, m_PlayObject.BonusAbil.Speed);
            PlayObject.SysMsg(string.Format("{0}的属性点数为:{1}", sHumName, sMsg), MsgColor.Red, MsgType.Hint);
        }
    }
}