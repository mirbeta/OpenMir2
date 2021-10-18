using SystemModule;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 从禁言列表中删除指定玩家
    /// </summary>
    [GameCommand("EnableSendMsg", "从禁言列表中删除指定玩家", "人物名称", 10)]
    public class EnableSendMsgCommand : BaseCommond
    {
        [DefaultCommand]
        public void EnableSendMsg(string[] @params, TPlayObject PlayObject)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName))
            {
                PlayObject.SysMsg(CommandAttribute.CommandHelp(), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            for (var i = M2Share.g_DisableSendMsgList.Count - 1; i >= 0; i--)
            {
                if (M2Share.g_DisableSendMsgList.Count <= 0)
                {
                    break;
                }
                //if ((sHumanName).CompareTo((M2Share.g_DisableSendMsgList[i])) == 0)
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
            PlayObject.SysMsg(sHumanName + " 没有被禁言!!!", TMsgColor.c_Red, TMsgType.t_Hint);
        }
    }
}