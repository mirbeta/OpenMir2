using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 清除指定玩家PK值
    /// </summary>
    [Command("FreePenalty", "清除指定玩家PK值", "人物名称", 10)]
    public class FreePenaltyCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            if (!string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?')
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            m_PlayObject.PkPoint = 0;
            m_PlayObject.RefNameColor();
            m_PlayObject.SysMsg(CommandHelp.GameCommandFreePKHumanMsg, MsgColor.Green, MsgType.Hint);
            PlayObject.SysMsg(string.Format(CommandHelp.GameCommandFreePKMsg, sHumanName), MsgColor.Green, MsgType.Hint);
        }
    }
}