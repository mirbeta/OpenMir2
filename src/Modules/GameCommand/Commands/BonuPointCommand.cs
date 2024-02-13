using OpenMir2;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 调整指定玩家属性点
    /// </summary>
    [Command("BonuPoint", "调整指定玩家属性点", "人物名称 属性点数(不输入为查看点数)", 10)]
    public class BonuPointCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumName = @params.Length > 0 ? @params[0] : "";
            var nCount = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0;
            if (string.IsNullOrEmpty(sHumName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumName);
            if (mIPlayerActor == null)
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (nCount > 0)
            {
                mIPlayerActor.BonusPoint = nCount;
                mIPlayerActor.SendMsg(Messages.RM_ADJUST_BONUS, 0, 0, 0, 0);
                return;
            }
            var sMsg = string.Format("未分配点数:{0} 已分配点数:(DC:{1} MC:{2} SC:{3} AC:{4} MAC:{5} HP:{6} MP:{7} HIT:{8} SPEED:{9})", mIPlayerActor.BonusPoint,
                mIPlayerActor.BonusAbil.DC, mIPlayerActor.BonusAbil.MC, mIPlayerActor.BonusAbil.SC, mIPlayerActor.BonusAbil.AC,
                mIPlayerActor.BonusAbil.MAC, mIPlayerActor.BonusAbil.HP, mIPlayerActor.BonusAbil.MP, mIPlayerActor.BonusAbil.Hit, mIPlayerActor.BonusAbil.Speed);
            PlayerActor.SysMsg(string.Format("{0}的属性点数为:{1}", sHumName, sMsg), MsgColor.Red, MsgType.Hint);
        }
    }
}