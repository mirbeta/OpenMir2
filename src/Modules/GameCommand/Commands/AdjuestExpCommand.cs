using OpenMir2;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 调整指定玩家经验值
    /// </summary>
    [Command("AdjuestExp", "调整指定人物的经验值", "物名称 经验值", 10)]
    public class AdjuestExpCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            string sExp = @params.Length > 1 ? @params[1] : "";
            if (string.IsNullOrEmpty(sHumanName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            int dwExp = HUtil32.StrToInt(sExp, 0);
            IPlayerActor mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor != null)
            {
                int dwOExp = PlayerActor.Abil.Exp;
                mIPlayerActor.Abil.Exp = dwExp;
                mIPlayerActor.HasLevelUp((ushort)(mIPlayerActor.Abil.Level - 1));
                PlayerActor.SysMsg(sHumanName + " 经验调整完成。", MsgColor.Green, MsgType.Hint);
                if (SystemShare.Config.ShowMakeItemMsg)
                {
                    LogService.Warn("[经验调整] " + PlayerActor.ChrName + '(' + mIPlayerActor.ChrName + ' ' + dwOExp + " -> " + mIPlayerActor.Abil.Exp + ')');
                }
            }
            else
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}