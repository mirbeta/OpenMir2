using M2Server.Player;
using SystemModule;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands
{
    /// <summary>
    /// 调整指定玩家金币
    /// </summary>
    [Command("AddGold", "调整指定玩家金币", "人物名称  金币数量", 10)]
    public class AddGoldCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject)
        {
            if (@params == null)
            {
                return;
            }
            var sHumName = @params.Length > 0 ? @params[0] : "";//玩家名称
            var nCount = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0;//金币数量
            var nServerIndex = 0;
            if (playObject.Permission < 6)
            {
                return;
            }
            if (string.IsNullOrEmpty(sHumName) || nCount <= 0)
            {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (mPlayObject != null)
            {
                if (mPlayObject.Gold + nCount < mPlayObject.GoldMax)
                {
                    mPlayObject.Gold += nCount;
                }
                else
                {
                    nCount = mPlayObject.GoldMax - mPlayObject.Gold;
                    mPlayObject.Gold = mPlayObject.GoldMax;
                }
                mPlayObject.GoldChanged();
                playObject.SysMsg(sHumName + "的金币已增加" + nCount + ".", MsgColor.Green, MsgType.Hint);
                if (M2Share.GameLogGold)
                {
                    //M2Share.EventSource.AddEventLog(14, playObject.MapName + "\09" + playObject.CurrX + "\09" + playObject.CurrY+ "\09" + playObject.ChrName + "\09" + Grobal2.StringGoldName + "\09" + nCount + "\09" + "1" + "\09" + sHumName);
                }
            }
            else
            {
                if (M2Share.WorldEngine.FindOtherServerUser(sHumName, ref nServerIndex))
                {
                    playObject.SysMsg(sHumName + " 现在" + nServerIndex + "号服务器上", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    M2Share.FrontEngine.AddChangeGoldList(playObject.ChrName, sHumName, nCount);
                    playObject.SysMsg(sHumName + " 现在不在线，等其上线时金币将自动增加", MsgColor.Green, MsgType.Hint);
                }
            }
        }
    }
}