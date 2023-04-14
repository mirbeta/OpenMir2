using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands
{
    /// <summary>
    /// 此命令用于改变客户端连接网关的
    /// </summary>
    [Command("Reconnection", "此命令用于改变客户端连接网关的IP及端口", " IP地址 端口", 10)]
    public class ReconnectionCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject)
        {
            if (@params == null)
            {
                return;
            }
            string sIPaddr = @params.Length > 0 ? @params[0] : "";
            string sPort = @params.Length > 1 ? @params[1] : "";
            if (playObject.Permission < 10)
            {
                return;
            }
            if (!string.IsNullOrEmpty(sIPaddr) && sIPaddr[0] == '?')
            {
                playObject.SysMsg("此命令用于改变客户端连接网关的IP及端口。", MsgColor.Blue, MsgType.Hint);
                return;
            }
            if (string.IsNullOrEmpty(sIPaddr) || string.IsNullOrEmpty(sPort))
            {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (!string.IsNullOrEmpty(sIPaddr) && !string.IsNullOrEmpty(sPort))
            {
                playObject.SendMsg(playObject,Messages.RM_RECONNECTION, 0, 0, 0, 0, sIPaddr + '/' + sPort);
            }
        }
    }
}