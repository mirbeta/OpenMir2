using GameSrv.Player;
using GameSrv.World;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 删除指定行会名称
    /// </summary>
    [Command("DelGuild", "删除指定行会名称", help: "行会名称", 10)]
    public class DelGuildCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sGuildName = @params.Length > 0 ? @params[0] : "";
            if (M2Share.ServerIndex != 0) {
                playObject.SysMsg("只能在主服务器上才可以使用此命令删除行会!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (string.IsNullOrEmpty(sGuildName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (M2Share.GuildMgr.DelGuild(sGuildName)) {
                WorldServer.SendServerGroupMsg(Messages.SS_206, M2Share.ServerIndex, sGuildName);
            }
            else {
                playObject.SysMsg("没找到" + sGuildName + "这个行会!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}