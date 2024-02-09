using SystemModule;
using SystemModule.Enums;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 开始行会争霸赛
    /// </summary>
    [Command("StartContest", "开始行会争霸赛", 10)]
    public class StartContestCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            IPlayerActor mIPlayerActor;
            IPlayerActor IPlayerActorA;
            bool bo19;
            if (!PlayerActor.Envir.Flag.Fight3Zone)
            {
                PlayerActor.SysMsg("此命令不能在当前地图中使用!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayerActor.SysMsg("开始行会争霸赛。", MsgColor.Red, MsgType.Hint);
            IList<IPlayerActor> list10 = new List<IPlayerActor>();
            IList<IPlayerActor> list14 = new List<IPlayerActor>();
            IList<IGuild> guildList = new List<IGuild>();
            SystemShare.WorldEngine.GetMapRageHuman(PlayerActor.Envir, PlayerActor.CurrX, PlayerActor.CurrY, 1000, ref list10);
            for (var i = 0; i < list10.Count; i++)
            {
                mIPlayerActor = list10[i] as IPlayerActor;
                if (!mIPlayerActor.ObMode || !mIPlayerActor.AdminMode)
                {
                    mIPlayerActor.FightZoneDieCount = 0;
                    if (mIPlayerActor.MyGuild == null)
                    {
                        continue;
                    }
                    bo19 = false;
                    for (var j = 0; j < list14.Count; j++)
                    {
                        IPlayerActorA = list14[j];
                        if (mIPlayerActor.MyGuild == IPlayerActorA.MyGuild)
                        {
                            bo19 = true;
                        }
                    }
                    if (!bo19)
                    {
                        // guildList.Add(mIPlayerActor.MyGuild);
                    }
                }
            }
            PlayerActor.SysMsg("行会争霸赛已经开始。", MsgColor.Green, MsgType.Hint);
            SystemShare.WorldEngine.CryCry(Messages.RM_CRY, PlayerActor.Envir, PlayerActor.CurrX, PlayerActor.CurrY, 1000, SystemShare.Config.CryMsgFColor, SystemShare.Config.CryMsgBColor, "- 行会战争已爆发。");
            var s20 = "";
            //for (var i = 0; i < guildList.Count; i++)
            //{
            //    guild = guildList[i];
            //    guild.StartTeamFight();
            //    for (var ii = 0; ii < list10.Count; ii++)
            //    {
            //        mIPlayerActor = list10[i] as IPlayerActor;
            //        if (mIPlayerActor.MyGuild == guild)
            //        {
            //            guild.AddTeamFightMember(mIPlayerActor.ChrName);
            //        }
            //    }
            //    s20 = s20 + guild.GuildName + ' ';
            //}
            SystemShare.WorldEngine.CryCry(Messages.RM_CRY, PlayerActor.Envir, PlayerActor.CurrX, PlayerActor.CurrY, 1000, SystemShare.Config.CryMsgFColor, SystemShare.Config.CryMsgBColor, " -参加的门派:" + s20);
        }
    }
}