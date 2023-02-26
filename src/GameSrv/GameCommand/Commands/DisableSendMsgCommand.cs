using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("DisableSendMsg", "", "人物名称", 10)]
    public class DisableSendMsgCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            if (string.IsNullOrEmpty(sHumanName)) {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null) {
                m_PlayObject.FilterSendMsg = true;
            }
            M2Share.DisableSendMsgList.Add(sHumanName);
            M2Share.SaveDisableSendMsgList();
            PlayObject.SysMsg(sHumanName + " 已加入禁言列表。", MsgColor.Green, MsgType.Hint);
        }
    }
}