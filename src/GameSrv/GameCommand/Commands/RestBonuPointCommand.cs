using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands
{
    /// <summary>
    /// 调整指定玩家属性的复位
    /// </summary>
    [Command("RestBonuPoint", "调整指定玩家属性的复位", "人物名称", 10)]
    public class RestBonuPointCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject)
        {
            if (@params == null)
            {
                return;
            }
            var sHumName = @params.Length > 0 ? @params[0] : "";
            int nTotleUsePoint;
            if (string.IsNullOrEmpty(sHumName))
            {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (mPlayObject != null)
            {
                nTotleUsePoint = mPlayObject.BonusAbil.DC + mPlayObject.BonusAbil.MC + mPlayObject.BonusAbil.SC + mPlayObject.BonusAbil.AC + mPlayObject.BonusAbil.MAC
                    + mPlayObject.BonusAbil.HP + mPlayObject.BonusAbil.MP + mPlayObject.BonusAbil.Hit + mPlayObject.BonusAbil.Speed + mPlayObject.BonusAbil.Reserved;
                mPlayObject.BonusPoint += nTotleUsePoint;
                mPlayObject.SendMsg(Messages.RM_ADJUST_BONUS, 0, 0, 0, 0);
                mPlayObject.HasLevelUp(0);
                mPlayObject.SysMsg("分配点数已复位!!!", MsgColor.Red, MsgType.Hint);
                playObject.SysMsg(sHumName + " 的分配点数已复位.", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}