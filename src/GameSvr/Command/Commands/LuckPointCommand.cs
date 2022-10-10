using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整指定玩家幸运点
    /// </summary>
    [Command("LuckPoint", "查看指定玩家幸运点", CommandHelp.GameCommandLuckPointHelpMsg, 10)]
    public class LuckPointCommand : Commond
    {
        [ExecuteCommand]
        public void LuckPoint(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var sCtr = @Params.Length > 1 ? @Params[1] : "";
            var sPoint = @Params.Length > 2 ? @Params[2] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?')
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (sCtr == "")
            {
                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandLuckPointMsg, sHumanName, mPlayObject.BodyLuckLevel, mPlayObject.BodyLuck, mPlayObject.Luck), MsgColor.Green, MsgType.Hint);
                return;
            }
            var nPoint = HUtil32.StrToInt(sPoint, 0);
            var cMethod = sCtr[0];
            switch (cMethod)
            {
                case '=':
                    mPlayObject.Luck = nPoint;
                    break;
                case '-':
                    if (mPlayObject.Luck >= nPoint)
                    {
                        mPlayObject.Luck -= nPoint;
                    }
                    else
                    {
                        mPlayObject.Luck = 0;
                    }
                    break;
                case '+':
                    mPlayObject.Luck += nPoint;
                    break;
            }
        }
    }
}