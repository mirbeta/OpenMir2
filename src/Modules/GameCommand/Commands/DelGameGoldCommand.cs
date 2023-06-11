using SystemModule;
using SystemModule.Enums;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 调整指定玩家游戏币
    /// </summary>
    [Command("DelGameGold", "调整指定玩家游戏币", help: "人物名称 数量", 10)]
    public class DelGameGoldCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumName = @params.Length > 0 ? @params[0] : ""; //玩家名称
            var nPoint = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0; //数量
            if (string.IsNullOrEmpty(sHumName) || nPoint <= 0)
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumName);
            if (mIPlayerActor != null)
            {
                if (mIPlayerActor.GameGold > nPoint)
                {
                    mIPlayerActor.GameGold -= nPoint;
                }
                else
                {
                    nPoint = mIPlayerActor.GameGold;
                    mIPlayerActor.GameGold = 0;
                }
                mIPlayerActor.GoldChanged();
                PlayerActor.SysMsg(sHumName + "的游戏点已减少" + nPoint + '.', MsgColor.Green, MsgType.Hint);
                mIPlayerActor.SysMsg("游戏点已减少" + nPoint + '.', MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}