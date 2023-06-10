using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 从禁言列表中删除指定玩家
    /// </summary>
    [Command("EnableSendMsg", "从禁言列表中删除指定玩家", "人物名称", 10)]
    public class EnableSendMsgCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            for (var i = SystemShare.DisableSendMsgList.Count - 1; i >= 0; i--)
            {
                if (SystemShare.DisableSendMsgList.Count <= 0)
                {
                    break;
                }
                //if ((sHumanName).CompareTo((Settings.g_DisableSendMsgList[i])) == 0)
                //{
                //    m_IPlayerActor = M2Share.WorldEngine.GeIPlayerActor(sHumanName);
                //    if (m_IPlayerActor != null)
                //    {
                //        m_IPlayerActor.m_boFilterSendMsg = false;
                //    }
                //    Settings.g_DisableSendMsgList.RemoveAt(i);
                //    M2Share.SaveDisableSendMsgList();
                //    PlayerActor.SysMsg(sHumanName + " 已从禁言列表中删除。", MsgColor.c_Green, MsgType.t_Hint);
                //    return;
                //}
            }
            PlayerActor.SysMsg(sHumanName + " 没有被禁言!!!", MsgColor.Red, MsgType.Hint);
        }
    }
}