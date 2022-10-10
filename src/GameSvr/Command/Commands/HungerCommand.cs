using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整指定玩家能量值
    /// </summary>
    [Command("Hunger", "调整指定玩家能量值", "人物名称 能量值", 10)]
    public class HungerCommand : Commond
    {
        [ExecuteCommand]
        public void Hunger(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var nHungerPoint = @Params.Length > 1 ? Convert.ToInt32(@Params[1]) : -1;
            if (PlayObject.Permission < 6)
            {
                return;
            }
            if (string.IsNullOrEmpty(sHumanName) || nHungerPoint < 0)
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                m_PlayObject.HungerStatus = nHungerPoint;
                m_PlayObject.SendMsg(m_PlayObject, Grobal2.RM_MYSTATUS, 0, 0, 0, 0, "");
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