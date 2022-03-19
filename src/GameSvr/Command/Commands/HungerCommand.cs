using GameSvr.CommandSystem;
using System;
using SystemModule;

namespace GameSvr
{
    /// <summary>
    /// 调整指定玩家能量值
    /// </summary>
    [GameCommand("Hunger", "调整指定玩家能量值", "人物名称 能量值", 10)]
    public class HungerCommand : BaseCommond
    {
        [DefaultCommand]
        public void Hunger(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var nHungerPoint = @Params.Length > 1 ? Convert.ToInt32(@Params[1]) : -1;
            TPlayObject m_PlayObject;
            if (PlayObject.m_btPermission < 6)
            {
                return;
            }
            if (string.IsNullOrEmpty(sHumanName) || nHungerPoint < 0)
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                m_PlayObject.m_nHungerStatus = nHungerPoint;
                m_PlayObject.SendMsg(m_PlayObject, Grobal2.RM_MYSTATUS, 0, 0, 0, 0, "");
                m_PlayObject.RefMyStatus();
                PlayObject.SysMsg(sHumanName + " 的能量值已改变。", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(sHumanName + "没有在线!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}