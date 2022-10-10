using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整沙巴克所属行会
    /// </summary>
    [Command("ChangeSabukLord", "调整沙巴克所属行会", "城堡名称 行会名称", 10)]
    public class ChangeSabukLordCommand : Commond
    {
        [ExecuteCommand]
        public void ChangeSabukLord(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sCastleName = @Params.Length > 0 ? @Params[0] : "";
            var sGuildName = @Params.Length > 1 ? @Params[1] : "";
            var boFlag = @Params.Length > 2 && bool.Parse(@Params[2]);
            if (sCastleName == "" || sGuildName == "")
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var Castle = M2Share.CastleMgr.Find(sCastleName);
            if (Castle == null)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandSbkGoldCastleNotFoundMsg, sCastleName), MsgColor.Red, MsgType.Hint);
                return;
            }
            var Guild = M2Share.GuildMgr.FindGuild(sGuildName);
            if (Guild != null)
            {
                M2Share.AddGameDataLog("27" + "\09" + Castle.OwnGuild + "\09" + '0' + "\09" + '1' + "\09" + "sGuildName" + "\09" + PlayObject.ChrName + "\09" + '0' + "\09" + '1' + "\09" + '0');
                Castle.GetCastle(Guild);
                if (boFlag)
                {
                    M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_211, M2Share.ServerIndex, sGuildName);
                }
                PlayObject.SysMsg(Castle.sName + " 所属行会已经更改为 " + sGuildName, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg("行会 " + sGuildName + "还没建立!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}