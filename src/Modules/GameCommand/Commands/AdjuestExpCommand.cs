using SystemModule;
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
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sExp = @params.Length > 1 ? @params[1] : "";
            if (string.IsNullOrEmpty(sHumanName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var dwExp = HUtil32.StrToInt(sExp, 0);
            var mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor != null)
            {
                var dwOExp = PlayerActor.Abil.Exp;
                mIPlayerActor.Abil.Exp = dwExp;
                mIPlayerActor.HasLevelUp((ushort)(mIPlayerActor.Abil.Level - 1));
                PlayerActor.SysMsg(sHumanName + " 经验调整完成。", MsgColor.Green, MsgType.Hint);
                if (SystemShare.Config.ShowMakeItemMsg)
                {
                    SystemShare.Logger.Warn("[经验调整] " + PlayerActor.ChrName + '(' + mIPlayerActor.ChrName + ' ' + dwOExp + " -> " + mIPlayerActor.Abil.Exp + ')');
                }
            }
            else
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}