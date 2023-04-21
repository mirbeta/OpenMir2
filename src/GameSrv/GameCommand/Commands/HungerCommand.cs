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
        public void Execute(string[] @params, PlayObject playObject)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var nHungerPoint = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : -1;
            if (playObject.Permission < 6)
            {
                return;
            }
            if (string.IsNullOrEmpty(sHumanName) || nHungerPoint < 0)
            {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mPlayObject = GameShare.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject != null)
            {
                mPlayObject.HungerStatus = nHungerPoint;
                mPlayObject.SendMsg(playObject, Messages.RM_MYSTATUS, 0, 0, 0, 0);
                mPlayObject.RefMyStatus();
                playObject.SysMsg(sHumanName + " 的能量值已改变。", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(sHumanName + "没有在线!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}