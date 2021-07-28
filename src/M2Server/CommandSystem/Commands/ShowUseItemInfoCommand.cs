using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 显示物品信息
    /// </summary>
    [GameCommand("ShowUseItem", "显示物品信息", 10)]
    public class ShowUseItemInfoCommand : BaseCommond
    {
        [DefaultCommand]
        public void ShowUseItem(string[] @Params, TPlayObject PlayObject)
        {
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            TUserItem UserItem = null;
            if (sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, M2Share.g_sGameCommandShowUseItemInfoHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            for (var i = m_PlayObject.m_UseItems.GetLowerBound(0); i <= m_PlayObject.m_UseItems.GetUpperBound(0); i++)
            {
                UserItem = m_PlayObject.m_UseItems[i];
                if (UserItem.wIndex == 0)
                {
                    continue;
                }
                PlayObject.SysMsg(string.Format("%s[%s]IDX[%d]系列号[%d]持久[%d-%d]", M2Share.GetUseItemName(i), M2Share.UserEngine.GetStdItemName(UserItem.wIndex), UserItem.wIndex,
                    UserItem.MakeIndex, UserItem.Dura, UserItem.DuraMax), TMsgColor.c_Blue, TMsgType.t_Hint);
            }
        }
    }
}