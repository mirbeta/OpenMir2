using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 退出行会
    /// </summary>
    [GameCommand("EndGuild", "退出行会", 0)]
    public class EndGuildCommand : BaseCommond
    {
        [DefaultCommand]
        public void EndGuild(PlayObject PlayObject)
        {
            if (PlayObject.MyGuild != null)
            {
                if (PlayObject.GuildRankNo > 1)
                {
                    if (PlayObject.MyGuild.IsMember(PlayObject.CharName) && PlayObject.MyGuild.DelMember(PlayObject.CharName))
                    {
                        M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.nServerIndex, PlayObject.MyGuild.sGuildName);
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