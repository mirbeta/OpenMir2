using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 调整指定玩家属性的复位
    /// </summary>
    [Command("RestBonuPoint", "调整指定玩家属性的复位", "人物名称", 10)]
    public class RestBonuPointCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumName = @params.Length > 0 ? @params[0] : "";
            int nTotleUsePoint;
            if (string.IsNullOrEmpty(sHumName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mIPlayerActor = ModuleShare.WorldEngine.GetPlayObject(sHumName);
            if (mIPlayerActor != null)
            {
                nTotleUsePoint = mIPlayerActor.BonusAbil.DC + mIPlayerActor.BonusAbil.MC + mIPlayerActor.BonusAbil.SC + mIPlayerActor.BonusAbil.AC + mIPlayerActor.BonusAbil.MAC
                    + mIPlayerActor.BonusAbil.HP + mIPlayerActor.BonusAbil.MP + mIPlayerActor.BonusAbil.Hit + mIPlayerActor.BonusAbil.Speed + mIPlayerActor.BonusAbil.Reserved;
                mIPlayerActor.BonusPoint += nTotleUsePoint;
                mIPlayerActor.SendMsg(Messages.RM_ADJUST_BONUS, 0, 0, 0, 0);
                mIPlayerActor.HasLevelUp(0);
                mIPlayerActor.SysMsg("分配点数已复位!!!", MsgColor.Red, MsgType.Hint);
                PlayerActor.SysMsg(sHumName + " 的分配点数已复位.", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}