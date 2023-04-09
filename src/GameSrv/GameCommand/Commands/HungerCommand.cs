using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands
{
    /// <summary>
    /// 调整指定玩家能量值
    /// </summary>
    [Command("Hunger", "调整指定玩家能量值", "人物名称 能量值", 10)]
    public class HungerCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            int nHungerPoint = @Params.Length > 1 ? HUtil32.StrToInt(@Params[1], 0) : -1;
            if (PlayObject.Permission < 6)
            {
                return;
            }
            if (string.IsNullOrEmpty(sHumanName) || nHungerPoint < 0)
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                m_PlayObject.HungerStatus = nHungerPoint;
                m_PlayObject.SendMsg(PlayObject, Messages.RM_MYSTATUS, 0, 0, 0, 0, "");
                m_PlayObject.RefMyStatus();
                PlayObject.SysMsg(sHumanName + " 的能量值已改变。", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(sHumanName + "没有在线!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}