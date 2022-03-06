using System;
using SystemModule;
using GameSvr.CommandSystem;

namespace GameSvr.Command
{
    /// <summary>
    /// 调整指定玩家性别
    /// </summary>
    [GameCommand("ChangeGender", "调整指定玩家性别", "人物名称 性别(男、女)", 10)]
    public class ChangeGenderCommand : BaseCommond
    {
        [DefaultCommand]
        public void ChangeGender(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var sSex = @Params.Length > 1 ? @Params[1] : "";
            var nSex = -1;
            if (sSex == "Man" || sSex == "男" || sSex == "0")
            {
                nSex = 0;
            }
            if (sSex == "WoMan" || sSex == "女" || sSex == "1")
            {
                nSex = 1;
            }
            if (string.IsNullOrEmpty(sHumanName) || nSex == -1)
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var playSex = PlayGender.Man;
            if (!Enum.TryParse(nSex.ToString(), out playSex))
            {
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                if (m_PlayObject.m_btGender != playSex)
                {
                    m_PlayObject.m_btGender = playSex;
                    m_PlayObject.FeatureChanged();
                    PlayObject.SysMsg(m_PlayObject.m_sCharName + " 的性别已改变。", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    PlayObject.SysMsg(m_PlayObject.m_sCharName + " 的性别未改变!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                PlayObject.SysMsg(sHumanName + "没有在线!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}