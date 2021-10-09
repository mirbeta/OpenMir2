using SystemModule;
using GameSvr.CommandSystem;

namespace GameSvr.Command
{
    /// <summary>
    /// 此命令用于改变客户端连接网关的
    /// </summary>
    [GameCommand("Reconnection", "此命令用于改变客户端连接网关的IP及端口", 10)]
    public class ReconnectionCommand : BaseCommond
    {
        [DefaultCommand]
        public void Reconnection(string[] @params, TPlayObject PlayObject)
        {
            var sIPaddr = @params.Length > 0 ? @params[0] : "";
            var sPort = @params.Length > 1 ? @params[1] : "";
            if (PlayObject.m_btPermission < 10)
            {
                return;
            }
            if (sIPaddr != "" && sIPaddr[0] == '?')
            {
                PlayObject.SysMsg("此命令用于改变客户端连接网关的IP及端口。", TMsgColor.c_Blue, TMsgType.t_Hint);
                return;
            }
            if (sIPaddr == "" || sPort == "")
            {
                PlayObject.SysMsg("命令格式: @" + this.CommandAttribute.Name + " IP地址 端口", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sIPaddr != "" && sPort != "")
            {
                PlayObject.SendMsg(PlayObject, Grobal2.RM_RECONNECTION, 0, 0, 0, 0, sIPaddr + '/' + sPort);
            }
        }
    }
}