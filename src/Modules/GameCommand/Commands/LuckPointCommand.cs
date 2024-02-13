using OpenMir2;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 调整指定玩家幸运点
    /// </summary>
    [Command("LuckPoint", "查看指定玩家幸运点", CommandHelp.GameCommandLuckPointHelpMsg, 10)]
    public class LuckPointCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            string sCtr = @params.Length > 1 ? @params[1] : "";
            string sPoint = @params.Length > 2 ? @params[2] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?')
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            IPlayerActor mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor == null)
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (string.IsNullOrEmpty(sCtr))
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandLuckPointMsg, sHumanName, mIPlayerActor.BodyLuckLevel, mIPlayerActor.BodyLuck, mIPlayerActor.Luck), MsgColor.Green, MsgType.Hint);
                return;
            }
            byte nPoint = (byte)HUtil32.StrToInt(sPoint, 0);
            char cMethod = sCtr[0];
            switch (cMethod)
            {
                case '=':
                    mIPlayerActor.Luck = nPoint;
                    break;
                case '-':
                    if (mIPlayerActor.Luck >= nPoint)
                    {
                        mIPlayerActor.Luck -= nPoint;
                    }
                    else
                    {
                        mIPlayerActor.Luck = 0;
                    }
                    break;
                case '+':
                    mIPlayerActor.Luck += nPoint;
                    break;
            }
        }
    }
}