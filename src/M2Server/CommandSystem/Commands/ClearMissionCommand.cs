using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 清除指定玩家的任务标志。
    /// </summary>
    [GameCommand("ClearMission", "清除指定玩家的任务标志", 10)]
    public class ClearMissionCommand : BaseCommond
    {
        [DefaultCommand]
        public void ClearMission(string[] @Params, TPlayObject PlayObject)
        {
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            if (sHumanName == "")
            {
                PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " 人物名称)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sHumanName[0] == '?')
            {
                PlayObject.SysMsg("此命令用于清除人物的任务标志。", TMsgColor.c_Blue, TMsgType.t_Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format("{0}不在线，或在其它服务器上！！", sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject.SysMsg(string.Format("{0}的任务标志已经全部清零。", sHumanName), TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}