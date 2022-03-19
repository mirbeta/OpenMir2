using GameSvr.CommandSystem;
using SystemModule;

namespace GameSvr
{
    [GameCommand("DisableSendMsg", "", "人物名称", 10)]
    public class DisableSendMsgCommand : BaseCommond
    {
        [DefaultCommand]
        public void DisableSendMsg(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            if (string.IsNullOrEmpty(sHumanName))
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                m_PlayObject.m_boFilterSendMsg = true;
            }
            M2Share.g_DisableSendMsgList.Add(sHumanName);
            M2Share.SaveDisableSendMsgList();
            PlayObject.SysMsg(sHumanName + " 已加入禁言列表。", MsgColor.Green, MsgType.Hint);
        }
    }
}