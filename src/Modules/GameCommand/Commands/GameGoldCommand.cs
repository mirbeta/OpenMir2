using System.Collections;
using OpenMir2;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 调整指定玩家游戏币
    /// </summary>
    [Command("GameGold", "调整指定玩家游戏币", CommandHelp.GameCommandGameGoldHelpMsg, 10)]
    public class GameGoldCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sCtr = @params.Length > 1 ? @params[1] : "";
            var nGold = @params.Length > 2 ? HUtil32.StrToInt(@params[2], 0) : 0;
            var ctr = '1';
            if (!string.IsNullOrEmpty(sCtr))
            {
                ctr = sCtr[0];
            }
            if (string.IsNullOrEmpty(sHumanName) || !new ArrayList(new[] { '=', '+', '-' }).Contains(ctr) || nGold < 0 || nGold > 200000000 || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?')
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor == null)
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            switch (sCtr[0])
            {
                case '=':
                    mIPlayerActor.GameGold = nGold;
                    break;
                case '+':
                    mIPlayerActor.GameGold += nGold;
                    break;
                case '-':
                    mIPlayerActor.GameGold -= nGold;
                    break;
            }
            if (SystemShare.GameLogGameGold)
            {
                // M2Share.EventSource.AddEventLog(Grobal2.LogGameGold, string.Format(CommandHelp.GameLogMsg1, mIPlayerActor.MapName, mIPlayerActor.CurrX, mIPlayerActor.CurrY,
                //     mIPlayerActor.ChrName, M2Share.Config.GameGoldName, nGold, sCtr[1], PlayerActor.ChrName));
            }
            PlayerActor.GameGoldChanged();
            mIPlayerActor.SysMsg(string.Format(CommandHelp.GameCommandGameGoldHumanMsg, SystemShare.Config.GameGoldName, nGold, mIPlayerActor.GameGold, SystemShare.Config.GameGoldName), MsgColor.Green, MsgType.Hint);
            PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandGameGoldGMMsg, sHumanName, SystemShare.Config.GameGoldName, nGold, mIPlayerActor.GameGold, SystemShare.Config.GameGoldName), MsgColor.Green, MsgType.Hint);
        }
    }
}