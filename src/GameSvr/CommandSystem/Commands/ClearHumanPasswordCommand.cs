using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 清楚指定玩家仓库密码
    /// </summary>
    [GameCommand("ClearHumanPassword", "清楚指定玩家仓库密码", 10)]
    public class ClearHumanPasswordCommand : BaseCommond
    {
        [DefaultCommand]
        public void ClearHumanPassword(string[] @Params, TPlayObject PlayObject)
        {
            var nPermission = @Params.Length > 0 ? Convert.ToInt32(@Params[0]) : 0;
            var sHumanName = @Params.Length > 1 ? @Params[1] : "";
            if (PlayObject.m_btPermission < nPermission)
            {
                return;
            }
            if (sHumanName == "" || sHumanName != "" && sHumanName[0] == '?')
            {
                PlayObject.SysMsg("清除玩家的仓库密码！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                PlayObject.SysMsg(string.Format("命令格式: @{0} 人物名称", this.Attributes.Name), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                return;
            }
            m_PlayObject.m_boPasswordLocked = false;
            m_PlayObject.m_boUnLockStoragePwd = false;
            m_PlayObject.m_sStoragePwd = "";
            m_PlayObject.SysMsg("你的保护密码已被清除！！！", TMsgColor.c_Green, TMsgType.t_Hint);
            PlayObject.SysMsg(string.Format("{0}的保护密码已被清除！！！", sHumanName), TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}