using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 退出行会
    /// </summary>
    [Command("EndGuild", "退出行会")]
    public class EndGuildCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            if (PlayerActor.MyGuild != null)
            {
                if (PlayerActor.GuildRankNo > 1)
                {
                    if (PlayerActor.MyGuild.IsMember(PlayerActor.ChrName) && PlayerActor.MyGuild.DelMember(PlayerActor.ChrName))
                    {
                        SystemShare.WorldEngine.SendServerGroupMsg(Messages.SS_207, SystemShare.ServerIndex, PlayerActor.MyGuild.GuildName);
                        PlayerActor.MyGuild = null;
                        PlayerActor.RefRankInfo(0, "");
                        PlayerActor.RefShowName();
                        PlayerActor.SysMsg("你已经退出行会。", MsgColor.Green, MsgType.Hint);
                    }
                }
                else
                {
                    PlayerActor.SysMsg("行会掌门人不能这样退出行会!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                PlayerActor.SysMsg("你都没加入行会!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}