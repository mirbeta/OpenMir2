using SystemModule;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 从禁言列表中删除指定玩家
    /// </summary>
    [GameCommand("EnableSendMsg", "从禁言列表中删除指定玩家", 10)]
    public class EnableSendMsgCommand : BaseCommond
    {
        [DefaultCommand]
        public void EnableSendMsg(string[] @params, TPlayObject PlayObject)
        {
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (sHumanName == "")
            {
                PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " 人物名称", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            for (var i = M2Share.g_DisableSendMsgList.Count - 1; i >= 0; i--)
            {
                if (M2Share.g_DisableSendMsgList.Count <= 0)
                {
                    break;
                }
                //if ((sHumanName).ToLower().CompareTo((M2Share.g_DisableSendMsgList[i]).ToLower()) == 0)
                //{
                //    m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
                //    if (m_PlayObject != null)
                //    {
                //        m_PlayObject.m_boFilterSendMsg = false;
                //    }
                //    M2Share.g_DisableSendMsgList.RemoveAt(i);
                //    M2Share.SaveDisableSendMsgList();
                //    PlayObject.SysMsg(sHumanName + " 已从禁言列表中删除。", TMsgColor.c_Green, TMsgType.t_Hint);
                //    return;
                //}
            }
            PlayObject.SysMsg(sHumanName + " 没有被禁言！！！", TMsgColor.c_Red, TMsgType.t_Hint);
        }
    }
}