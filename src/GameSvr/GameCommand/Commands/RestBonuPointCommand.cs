using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 调整指定玩家属性的复位
    /// </summary>
    [Command("RestBonuPoint", "调整指定玩家属性的复位", "人物名称", 10)]
    public class RestBonuPointCommand : Command
    {
        [ExecuteCommand]
        public void RestBonuPoint(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sHumName = @Params.Length > 0 ? @Params[0] : "";
            int nTotleUsePoint;
            if (sHumName == "")
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (m_PlayObject != null)
            {
                nTotleUsePoint = m_PlayObject.BonusAbil.DC + m_PlayObject.BonusAbil.MC + m_PlayObject.BonusAbil.SC + m_PlayObject.BonusAbil.AC + m_PlayObject.BonusAbil.MAC
                    + m_PlayObject.BonusAbil.HP + m_PlayObject.BonusAbil.MP + m_PlayObject.BonusAbil.Hit + m_PlayObject.BonusAbil.Speed + m_PlayObject.BonusAbil.Reserved;
                m_PlayObject.BonusPoint += nTotleUsePoint;
                m_PlayObject.SendMsg(m_PlayObject, Messages.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                m_PlayObject.HasLevelUp(0);
                m_PlayObject.SysMsg("分配点数已复位!!!", MsgColor.Red, MsgType.Hint);
                PlayObject.SysMsg(sHumName + " 的分配点数已复位.", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}