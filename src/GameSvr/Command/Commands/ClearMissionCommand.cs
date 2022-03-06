using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 清除指定玩家的任务标志。
    /// </summary>
    [GameCommand("ClearMission", "清除指定玩家的任务标志", "人物名称", 10)]
    public class ClearMissionCommand : BaseCommond
    {
        [DefaultCommand]
        public void ClearMission(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            if (string.IsNullOrEmpty(sHumanName))
            {
                PlayObject.SysMsg(CommandAttribute.CommandHelp(), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (sHumanName[0] == '?')
            {
                PlayObject.SysMsg("此命令用于清除人物的任务标志。", MsgColor.Blue, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg($"{sHumanName}不在线，或在其它服务器上!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject.SysMsg($"{sHumanName}的任务标志已经全部清零。", MsgColor.Green, MsgType.Hint);
        }
    }
}