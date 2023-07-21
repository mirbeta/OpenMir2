using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整指定玩家游戏币
    /// </summary>
    [Command("DelGold", "调整指定玩家游戏币", help: "人物名称 数量", 10)]
    public class DelGoldCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sHumName = @params.Length > 0 ? @params[0] : "";
            int nCount = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0;
            if (string.IsNullOrEmpty(sHumName) || nCount <= 0) {
                return;
            }
            PlayObject mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (string.IsNullOrEmpty(sHumName) || nCount <= 0) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (mPlayObject != null) {
                if (mPlayObject.Gold > nCount) {
                    mPlayObject.Gold -= nCount;
                }
                else {
                    nCount = mPlayObject.Gold;
                    mPlayObject.Gold = 0;
                }
                mPlayObject.GoldChanged();
                playObject.SysMsg(sHumName + "的金币已减少" + nCount + ".", MsgColor.Green, MsgType.Hint);
                if (M2Share.GameLogGold) {
                    M2Share.EventSource.AddEventLog(13, playObject.MapName + "\t" + playObject.CurrX + "\t" + playObject.CurrY + "\t"
                                                        + playObject.ChrName + "\t" + Grobal2.StringGoldName + "\t" + nCount + "\t" + "1" + "\t" + sHumName);
                }
            }
            else {
                int nServerIndex = 0;
                if (M2Share.WorldEngine.FindOtherServerUser(sHumName, ref nServerIndex)) {
                    playObject.SysMsg(sHumName + "现在" + nServerIndex + "号服务器上", MsgColor.Green, MsgType.Hint);
                }
                else {
                    M2Share.FrontEngine.AddChangeGoldList(playObject.ChrName, sHumName, -nCount);
                    playObject.SysMsg(sHumName + "现在不在线，等其上线时金币将自动减少", MsgColor.Green, MsgType.Hint);
                }
            }
        }
    }
}