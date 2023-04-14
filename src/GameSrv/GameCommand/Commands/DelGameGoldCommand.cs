using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整指定玩家游戏币
    /// </summary>
    [Command("DelGameGold", "调整指定玩家游戏币", help: "人物名称 数量", 10)]
    public class DelGameGoldCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sHumName = @params.Length > 0 ? @params[0] : ""; //玩家名称
            int nPoint = @params.Length > 1 ? HUtil32.StrToInt(@params[1],0) : 0; //数量
            if (string.IsNullOrEmpty(sHumName) || nPoint <= 0) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (mPlayObject != null) {
                if (mPlayObject.GameGold > nPoint) {
                    mPlayObject.GameGold -= nPoint;
                }
                else {
                    nPoint = mPlayObject.GameGold;
                    mPlayObject.GameGold = 0;
                }
                mPlayObject.GoldChanged();
                playObject.SysMsg(sHumName + "的游戏点已减少" + nPoint + '.', MsgColor.Green, MsgType.Hint);
                mPlayObject.SysMsg("游戏点已减少" + nPoint + '.', MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}