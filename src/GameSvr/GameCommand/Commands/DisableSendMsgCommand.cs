using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [Command("DisableSendMsg", "", "人物名称", 10)]
    public class DisableSendMsgCommand : Command
    {
        [ExecuteCommand]
        public void DisableSendMsg(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            if (string.IsNullOrEmpty(sHumanName))
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
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