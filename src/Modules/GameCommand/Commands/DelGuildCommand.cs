using OpenMir2;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 删除指定行会名称
    /// </summary>
    [Command("DelGuild", "删除指定行会名称", help: "行会名称", 10)]
    public class DelGuildCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            string sGuildName = @params.Length > 0 ? @params[0] : "";
            if (SystemShare.ServerIndex != 0)
            {
                PlayerActor.SysMsg("只能在主服务器上才可以使用此命令删除行会!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (string.IsNullOrEmpty(sGuildName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (SystemShare.GuildMgr.DelGuild(sGuildName))
            {
                SystemShare.WorldEngine.SendServerGroupMsg(Messages.SS_206, SystemShare.ServerIndex, sGuildName);
            }
            else
            {
                PlayerActor.SysMsg("没找到" + sGuildName + "这个行会!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}