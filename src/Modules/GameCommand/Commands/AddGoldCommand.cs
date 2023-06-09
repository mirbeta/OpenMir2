using SystemModule;
using SystemModule.Enums;

namespace CommandSystem
{
    /// <summary>
    /// 调整指定玩家金币
    /// </summary>
    [Command("AddGold", "调整指定玩家金币", "人物名称  金币数量", 10)]
    public class AddGoldCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumName = @params.Length > 0 ? @params[0] : "";//玩家名称
            var nCount = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0;//金币数量
            var nServerIndex = 0;
            if (PlayerActor.Permission < 6)
            {
                return;
            }
            if (string.IsNullOrEmpty(sHumName) || nCount <= 0)
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mIPlayerActor = ModuleShare.WorldEngine.GetPlayObject(sHumName);
            if (mIPlayerActor != null)
            {
                if (mIPlayerActor.Gold + nCount < mIPlayerActor.GoldMax)
                {
                    mIPlayerActor.Gold += nCount;
                }
                else
                {
                    nCount = mIPlayerActor.GoldMax - mIPlayerActor.Gold;
                    mIPlayerActor.Gold = mIPlayerActor.GoldMax;
                }
                mIPlayerActor.GoldChanged();
                PlayerActor.SysMsg(sHumName + "的金币已增加" + nCount + ".", MsgColor.Green, MsgType.Hint);
                if (ModuleShare.GameLogGold)
                {
                    //M2Share.EventSource.AddEventLog(14, PlayerActor.MapName + "\09" + PlayerActor.CurrX + "\09" + PlayerActor.CurrY+ "\09" + PlayerActor.ChrName + "\09" + Grobal2.StringGoldName + "\09" + nCount + "\09" + "1" + "\09" + sHumName);
                }
            }
            else
            {
                if (ModuleShare.WorldEngine.FindOtherServerUser(sHumName, ref nServerIndex))
                {
                    PlayerActor.SysMsg(sHumName + " 现在" + nServerIndex + "号服务器上", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    //M2Share.FrontEngine.AddChangeGoldList(PlayerActor.ChrName, sHumName, nCount);
                    PlayerActor.SysMsg(sHumName + " 现在不在线，等其上线时金币将自动增加", MsgColor.Green, MsgType.Hint);
                }
            }
        }
    }
}