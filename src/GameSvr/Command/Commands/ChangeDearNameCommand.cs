using System;
using SystemModule;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 调整指定玩家配偶名称
    /// </summary>
    [GameCommand("ChangeDearName", "调整指定玩家配偶名称", help: "人物名称 配偶名称(如果为 无 则清除)", 10)]
    public class ChangeDearNameCommand : BaseCommond
    {
        [DefaultCommand]
        public void ChangeDearName(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var sDearName = @Params.Length > 1 ? @Params[1] : "";
            if (string.IsNullOrEmpty(sHumanName) || sDearName == "")
            {
                PlayObject.SysMsg(CommandAttribute.CommandHelp(), MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                if (String.Compare(sDearName, "无", StringComparison.Ordinal) == 0)
                {
                    m_PlayObject.m_sDearName = "";
                    m_PlayObject.RefShowName();
                    PlayObject.SysMsg(sHumanName + " 的配偶名清除成功。", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    m_PlayObject.m_sDearName = sDearName;
                    m_PlayObject.RefShowName();
                    PlayObject.SysMsg(sHumanName + " 的配偶名更改成功。", MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}