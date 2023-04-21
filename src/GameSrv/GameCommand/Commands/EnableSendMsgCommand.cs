using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 从禁言列表中删除指定玩家
    /// </summary>
    [Command("EnableSendMsg", "从禁言列表中删除指定玩家", "人物名称", 10)]
    public class EnableSendMsgCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            for (var i = GameShare.DisableSendMsgList.Count - 1; i >= 0; i--) {
                if (GameShare.DisableSendMsgList.Count <= 0) {
                    break;
                }
                //if ((sHumanName).CompareTo((Settings.g_DisableSendMsgList[i])) == 0)
                //{
                //    m_PlayObject = M2Share.WorldEngine.GePlayObject(sHumanName);
                //    if (m_PlayObject != null)
                //    {
                //        m_PlayObject.m_boFilterSendMsg = false;
                //    }
                //    Settings.g_DisableSendMsgList.RemoveAt(i);
                //    M2Share.SaveDisableSendMsgList();
                //    PlayObject.SysMsg(sHumanName + " 已从禁言列表中删除。", MsgColor.c_Green, MsgType.t_Hint);
                //    return;
                //}
            }
            playObject.SysMsg(sHumanName + " 没有被禁言!!!", MsgColor.Red, MsgType.Hint);
        }
    }
}