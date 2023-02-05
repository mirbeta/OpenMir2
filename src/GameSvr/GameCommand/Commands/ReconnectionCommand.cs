using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 此命令用于改变客户端连接网关的
    /// </summary>
    [Command("Reconnection", "此命令用于改变客户端连接网关的IP及端口", " IP地址 端口", 10)]
    public class ReconnectionCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject PlayObject)
        {
            if (@params == null)
            {
                return;
            }
            string sIPaddr = @params.Length > 0 ? @params[0] : "";
            string sPort = @params.Length > 1 ? @params[1] : "";
            if (PlayObject.Permission < 10)
            {
                return;
            }
            if (sIPaddr != "" && sIPaddr[0] == '?')
            {
                PlayObject.SysMsg("此命令用于改变客户端连接网关的IP及端口。", MsgColor.Blue, MsgType.Hint);
                return;
            }
            if (sIPaddr == "" || sPort == "")
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (sIPaddr != "" && sPort != "")
            {
                PlayObject.SendMsg(PlayObject, Messages.RM_RECONNECTION, 0, 0, 0, 0, sIPaddr + '/' + sPort);
            }
        }
    }
}