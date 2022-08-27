using GameSvr.Actor;
using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 召唤指定怪物为宠物
    /// 格式:RECALLMOB 怪物名称 宝宝等级(最高为 7) 叛变时间(分钟) 是否自动变色（0、1）固定颜色（1-7）
    /// </summary>
    [GameCommand("RecallMob", "召唤指定怪物为宠物", "怪物名称 数量 等级(0-7) 叛变时间(分钟) 是否自动变色（0、1）固定颜色（1-7）", 10)]
    public class RecallMobCommand : BaseCommond
    {
        [DefaultCommand]
        public void RecallMob(string[] @Params, TPlayObject PlayObject)
        {
            var sMonName = @Params.Length > 0 ? @Params[0] : "";
            var nCount = @Params.Length > 1 ? Convert.ToInt32(@Params[1]) : 0;
            var nLevel = @Params.Length > 2 ? Convert.ToInt32(@Params[2]) : 0;
            var nTick = @Params.Length > 3 ? Convert.ToInt32(@Params[3]) : 86400000;
            var nAutoChangeColor = @Params.Length > 4 ? Convert.ToInt32(@Params[4]) : 0;
            var nFixColor = @Params.Length > 5 ? Convert.ToInt32(@Params[5]) : 0;

            short n10 = 0;
            short n14 = 0;
            TBaseObject mon;
            if (sMonName == "" || sMonName != "" && sMonName[0] == '?')
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (nLevel >= 10)
            {
                nLevel = 0;
            }
            if (nCount <= 0)
            {
                nCount = 1;
            }
            for (var i = 0; i < nCount; i++)
            {
                if (PlayObject.m_SlaveList.Count >= 20)
                {
                    break;
                }
                PlayObject.GetFrontPosition(ref n10, ref n14);
                mon = M2Share.UserEngine.RegenMonsterByName(PlayObject.m_PEnvir.SMapName, n10, n14, sMonName);
                if (mon != null)
                {
                    mon.m_Master = PlayObject;
                    mon.m_dwMasterRoyaltyTick = nTick;
                    mon.m_btSlaveMakeLevel = 3;
                    mon.m_btSlaveExpLevel = (byte)nLevel;
                    if (nAutoChangeColor == 1)
                    {
                        mon.m_boAutoChangeColor = true;
                    }
                    else if (nFixColor > 0)
                    {
                        mon.m_boFixColor = true;
                        mon.m_nFixColorIdx = nFixColor - 1;
                    }
                    mon.RecalcAbilitys();
                    mon.RefNameColor();
                    PlayObject.m_SlaveList.Add(mon);
                }
            }
        }
    }
}