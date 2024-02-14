using OpenMir2;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 调整指定玩家游戏币
    /// </summary>
    [Command("AddGameGold", "调整指定玩家游戏币", "人物名称  金币数量", 10)]
    public class AddGameGoldCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            string sHumName = @params.Length > 0 ? @params[0] : "";
            int nPoint = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0;
            if (PlayerActor.Permission < 6)
            {
                return;
            }
            if (string.IsNullOrEmpty(sHumName) || nPoint <= 0)
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            IPlayerActor mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumName);
            if (mIPlayerActor != null)
            {
                if (mIPlayerActor.GameGold + nPoint < 2000000)
                {
                    mIPlayerActor.GameGold += nPoint;
                }
                else
                {
                    nPoint = 2000000 - mIPlayerActor.GameGold;
                    mIPlayerActor.GameGold = 2000000;
                }
                mIPlayerActor.GoldChanged();
                PlayerActor.SysMsg(sHumName + "的游戏点已增加" + nPoint + '.', MsgColor.Green, MsgType.Hint);
                mIPlayerActor.SysMsg("游戏点已增加" + nPoint + '.', MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}