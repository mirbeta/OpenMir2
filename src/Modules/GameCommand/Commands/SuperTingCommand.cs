using OpenMir2;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 随机传送一个指定玩家和他身边的人
    /// </summary>
    [Command("SuperTing", "随机传送一个指定玩家和他身边的人", CommandHelp.GameCommandSuperTingHelpMsg, 10)]
    public class SuperTingCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            string sRange = @params.Length > 1 ? @params[1] : "";
            IPlayerActor moveHuman;
            IList<IPlayerActor> humanList;
            if (string.IsNullOrEmpty(sRange) || string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?')
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            int nRange = HUtil32._MAX(10, HUtil32.StrToInt(sRange, 2));
            IPlayerActor mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor != null)
            {
                humanList = new List<IPlayerActor>();
                SystemShare.WorldEngine.GetMapRageHuman(mIPlayerActor.Envir, mIPlayerActor.CurrX, mIPlayerActor.CurrY, nRange, ref humanList);
                for (int i = 0; i < humanList.Count; i++)
                {
                    moveHuman = humanList[i];
                    if (moveHuman != PlayerActor)
                    {
                        moveHuman.MapRandomMove(moveHuman.HomeMap, 0);
                    }
                }
            }
            else
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}