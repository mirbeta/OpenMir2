using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 从禁言列表中删除指定玩家
    /// </summary>
    [Command("EnableSendMsg", "从禁言列表中删除指定玩家", "人物名称", 10)]
    public class EnableSendMsgCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject PlayObject) {
            if (@params == null) {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName)) {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            for (int i = M2Share.DisableSendMsgList.Count - 1; i >= 0; i--) {
                if (M2Share.DisableSendMsgList.Count <= 0) {
                    break;
                }
                //if ((sHumanName).CompareTo((Settings.g_DisableSendMsgList[i])) == 0)
                //{
                //    m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
                //    if (m_PlayObject != null)
                //    {
                //        m_PlayObject.m_boFilterSendMsg = false;
                //    }
                //    Settings.g_DisableSendMsgList.RemoveAt(i);
                //    M2Share.SaveDisableSendMsgList();
                //    PlayObject.SysMsg(sHumanName + " 已从禁言列表中删除。", TMsgColor.c_Green, TMsgType.t_Hint);
                //    return;
                //}
            }
            PlayObject.SysMsg(sHumanName + " 没有被禁言!!!", MsgColor.Red, MsgType.Hint);
        }
    }
}