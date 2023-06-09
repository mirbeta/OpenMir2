using SystemModule;
using SystemModule;
using SystemModule.Enums;

namespace CommandSystem
{
    /// <summary>
    /// 调整沙巴克所属行会
    /// </summary>
    [Command("ChangeSabukLord", "调整沙巴克所属行会", "城堡名称 行会名称", 10)]
    public class ChangeSabukLordCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sCastleName = @params.Length > 0 ? @params[0] : "";
            var sGuildName = @params.Length > 1 ? @params[1] : "";
            var boFlag = @params.Length > 2 && bool.Parse(@params[2]);
            if (string.IsNullOrEmpty(sCastleName) || string.IsNullOrEmpty(sGuildName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var castle = ModuleShare.CastleMgr.Find(sCastleName);
            if (castle == null)
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandSbkGoldCastleNotFoundMsg, sCastleName), MsgColor.Red, MsgType.Hint);
                return;
            }
            var guild = ModuleShare.GuildMgr.FindGuild(sGuildName);
            if (guild != null)
            {
                //M2Share.EventSource.AddEventLog(27, castle.OwnGuild + "\09" + '0' + "\09" + '1' + "\09" + "sGuildName" + "\09" + PlayerActor.ChrName + "\09" + '0' + "\09" + '1' + "\09" + '0');
                castle.GetCastle(guild);
                if (boFlag)
                {
                    ModuleShare.WorldEngine.SendServerGroupMsg(Messages.SS_211, ModuleShare.ServerIndex, sGuildName);
                }
                PlayerActor.SysMsg(castle.sName + " 所属行会已经更改为 " + sGuildName, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayerActor.SysMsg("行会 " + sGuildName + "还没建立!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}