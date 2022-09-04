using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 删除指定行会名称
    /// </summary>
    [GameCommand("DelGuild", "删除指定行会名称", help: "行会名称", 10)]
    public class DelGuildCommand : BaseCommond
    {
        [DefaultCommand]
        public void DelGuild(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sGuildName = @Params.Length > 0 ? @Params[0] : "";
            if (M2Share.ServerIndex != 0)
            {
                PlayObject.SysMsg("只能在主服务器上才可以使用此命令删除行会!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (sGuildName == "")
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (M2Share.GuildMgr.DelGuild(sGuildName))
            {
                M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_206, M2Share.ServerIndex, sGuildName);
            }
            else
            {
                PlayObject.SysMsg("没找到" + sGuildName + "这个行会!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}