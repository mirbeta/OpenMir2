using OpenMir2;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
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
            string sCastleName = @params.Length > 0 ? @params[0] : "";
            string sGuildName = @params.Length > 1 ? @params[1] : "";
            bool boFlag = @params.Length > 2 && bool.Parse(@params[2]);
            if (string.IsNullOrEmpty(sCastleName) || string.IsNullOrEmpty(sGuildName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            SystemModule.Castles.IUserCastle castle = SystemShare.CastleMgr.Find(sCastleName);
            if (castle == null)
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandSbkGoldCastleNotFoundMsg, sCastleName), MsgColor.Red, MsgType.Hint);
                return;
            }
            SystemModule.Castles.IGuild guild = SystemShare.GuildMgr.FindGuild(sGuildName);
            if (guild != null)
            {
                //M2Share.EventSource.AddEventLog(27, castle.OwnGuild + "\09" + '0' + "\09" + '1' + "\09" + "sGuildName" + "\09" + PlayerActor.ChrName + "\09" + '0' + "\09" + '1' + "\09" + '0');
                castle.GetCastle(guild);
                if (boFlag)
                {
                    SystemShare.WorldEngine.SendServerGroupMsg(Messages.SS_211, SystemShare.ServerIndex, sGuildName);
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