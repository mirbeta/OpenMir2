using GameSvr.Actor;
using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 刷指定怪物
    /// </summary>
    [Command("Mob", "刷指定怪物", "怪物名称 数量 等级(0-7)", 10)]
    public class MobCommand : Command
    {
        [ExecuteCommand]
        public void Mob(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            short nX = 0;
            short nY = 0;
            var sMonName = Params.Length > 0 ? @Params[0] : "";//名称
            var nCount = Params.Length > 1 ? Convert.ToInt32(@Params[1]) : 1;//数量
            var nLevel = Params.Length > 2 ? Convert.ToByte(@Params[2]) : (byte)0;//怪物等级
            if (string.IsNullOrEmpty(sMonName))
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (nCount <= 0)
            {
                nCount = 1;
            }
            if (!(nLevel <= 10))
            {
                nLevel = 0;
            }
            nCount = (byte)HUtil32._MIN(64, nCount);
            PlayObject.GetFrontPosition(ref nX, ref nY);//刷在当前X，Y坐标
            for (var i = 0; i < nCount; i++)
            {
                BaseObject Monster = M2Share.WorldEngine.RegenMonsterByName(PlayObject.Envir.MapName, nX, nY, sMonName);
                if (Monster != null)
                {
                    Monster.SlaveMakeLevel = nLevel;
                    Monster.SlaveExpLevel = nLevel;
                    Monster.RecalcAbilitys();
                    Monster.RefNameColor();
                }
                else
                {
                    PlayObject.SysMsg(CommandHelp.GameCommandMobMsg, MsgColor.Red, MsgType.Hint);
                    break;
                }
            }
        }
    }
}