using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 退出行会
    /// </summary>
    [Command("EndGuild", "退出行会", 0)]
    public class EndGuildCommand : Commond
    {
        [ExecuteCommand]
        public void EndGuild(PlayObject PlayObject)
        {
            if (PlayObject.MyGuild != null)
            {
                if (PlayObject.GuildRankNo > 1)
                {
                    if (PlayObject.MyGuild.IsMember(PlayObject.ChrName) && PlayObject.MyGuild.DelMember(PlayObject.ChrName))
                    {
                        M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.ServerIndex, PlayObject.MyGuild.sGuildName);
                        PlayObject.MyGuild = null;
                        PlayObject.RefRankInfo(0, "");
                        PlayObject.RefShowName();
                        PlayObject.SysMsg("你已经退出行会。", MsgColor.Green, MsgType.Hint);
                    }
                }
                else
                {
                    PlayObject.SysMsg("行会掌门人不能这样退出行会!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                PlayObject.SysMsg("你都没加入行会!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}