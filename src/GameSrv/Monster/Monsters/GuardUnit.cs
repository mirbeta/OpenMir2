using GameSrv.Actor;
using GameSrv.Maps;
using GameSrv.Npc;
using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.Monster.Monsters
{
    /// <summary>
    /// 守卫类
    /// </summary>
    public class GuardUnit : NormNpc
    {
        public sbyte GuardDirection;
        public bool CrimeforCastle;
        public int CrimeforCastleTime = 0;

        public GuardUnit()
        {
            Race = ActorRace.Guard;
        }

        public override void Struck(BaseObject hiter)
        {
            base.Struck(hiter);
            if (Castle != null)
            {
                CrimeforCastle = true;
                CrimeforCastleTime = HUtil32.GetTickCount();
            }
        }

        public override bool IsProperTarget(BaseObject baseObject)
        {
            bool result = false;
            if (Castle != null)
            {
                if (LastHiter == baseObject)
                {
                    result = true;
                }
                if (Castle.UnderWar)
                {
                    result = true;
                }
                if (baseObject.Race == ActorRace.Guard)
                {
                    GuardUnit guardObject = (GuardUnit)baseObject;
                    if (guardObject.CrimeforCastle)
                    {
                        if ((HUtil32.GetTickCount() - guardObject.CrimeforCastleTime) < (2 * 60 * 1000))
                        {
                            result = true;
                        }
                        else
                        {
                            guardObject.CrimeforCastle = false;
                        }
                        if (guardObject.Castle != null)
                        {
                            guardObject.CrimeforCastle = false;
                            result = false;
                        }
                    }
                }
                if (Castle.MasterGuild != null)
                {
                    if (baseObject.Master == null)
                    {
                        if (baseObject.Race == ActorRace.Play)
                        {
                            if (Castle.MasterGuild == ((PlayObject)baseObject).MyGuild || Castle.MasterGuild.IsAllyGuild(((PlayObject)baseObject).MyGuild))
                            {
                                if (LastHiter != baseObject)
                                {
                                    result = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (baseObject.Master.Race == ActorRace.Play)
                        {
                            if (Castle.MasterGuild == ((PlayObject)baseObject.Master).MyGuild || Castle.MasterGuild.IsAllyGuild(((PlayObject)baseObject.Master).MyGuild))
                            {
                                if (LastHiter != baseObject.Master && LastHiter != baseObject)
                                {
                                    result = false;
                                }
                            }
                        }
                    }
                }
                if (baseObject.AdminMode || baseObject.StoneMode || baseObject.Race >= ActorRace.NPC && baseObject.Race < ActorRace.Animal || baseObject == this || baseObject.Castle == Castle)
                {
                    result = false;
                }
                return result;
            }
            if (LastHiter == baseObject)
            {
                return true;
            }
            if (baseObject.TargetCret != null && baseObject.TargetCret.Race == 112)
            {
                return true;
            }
            if (baseObject.Race == ActorRace.Play)
            {
                if (((PlayObject)baseObject).PvpLevel() >= 2)
                {
                    return true;
                }
            }
            if (baseObject.AdminMode || baseObject.StoneMode || baseObject == this)
            {
                return false;
            }
            return result;
        }

        public override void SearchViewRange()
        {
            const string sExceptionMsg = "[Exception] TBaseObject::SearchViewRange {0} {1} {2} {3} {4}";
            int n24 = 0;
            IsVisibleActive = false;// 先置为FALSE
            for (int i = 0; i < VisibleActors.Count; i++)
            {
                VisibleActors[i].VisibleFlag = 0;
            }
            short nStartX = (short)(CurrX - ViewRange);
            short nEndX = (short)(CurrX + ViewRange);
            short nStartY = (short)(CurrY - ViewRange);
            short nEndY = (short)(CurrY + ViewRange);
            try
            {
                for (short n18 = nStartX; n18 <= nEndX; n18++)
                {
                    for (short n1C = nStartY; n1C <= nEndY; n1C++)
                    {
                        ref MapCellInfo cellInfo = ref Envir.GetCellInfo(n18, n1C, out bool cellSuccess);
                        if (cellSuccess && cellInfo.IsAvailable)
                        {
                            n24 = 1;
                            for (int i = 0; i < cellInfo.ObjList.Count; i++)
                            {
                                CellObject cellObject = cellInfo.ObjList[i];
                                if (cellObject.ActorObject)
                                {
                                    if ((HUtil32.GetTickCount() - cellObject.AddTime) >= 60 * 1000)
                                    {
                                        cellInfo.Remove(cellObject);
                                        if (cellInfo.Count > 0)
                                        {
                                            continue;
                                        }
                                        cellInfo.Clear();
                                        break;
                                    }
                                    BaseObject baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                                    if (baseObject != null)
                                    {
                                        if (!baseObject.Death && !baseObject.Invisible)
                                        {
                                            if (IsPassiveAttack(baseObject))//守卫和护卫不搜索不主动攻击的怪物
                                            {
                                                continue;
                                            }
                                            if (baseObject.Race == 112)
                                            {
                                                continue;
                                            }
                                            if (!baseObject.Ghost && !baseObject.FixedHideMode && !baseObject.ObMode)
                                            {
                                                if ((Race < ActorRace.Animal) || (Master != null) || CrazyMode || NastyMode || WantRefMsg || ((baseObject.Master != null) && (Math.Abs(baseObject.CurrX - CurrX) <= 3) && (Math.Abs(baseObject.CurrY - CurrY) <= 3)) || (baseObject.Race == ActorRace.Play))
                                                {                                                    
                                                    UpdateVisibleGay(baseObject);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.Logger.Error(Format(sExceptionMsg, n24, ChrName, MapName, CurrX, CurrY));
                M2Share.Logger.Error(e.Message);
                KickException();
            }
            n24 = 2;
            try
            {
                int n18 = 0;
                while (true)
                {
                    if (VisibleActors.Count <= n18)
                    {
                        break;
                    }
                    VisibleBaseObject visibleBaseObject = VisibleActors[n18];
                    if (visibleBaseObject.VisibleFlag == VisibleFlag.Visible)
                    {
                        VisibleActors.RemoveAt(n18);
                        Dispose(visibleBaseObject);
                        continue;
                    }
                    n18++;
                }
            }
            catch
            {
                M2Share.Logger.Error(Format(sExceptionMsg, n24, ChrName, MapName, CurrX, CurrY));
                KickException();
            }
        }
        
        /// <summary>
        /// 是否被动攻击怪物类型
        /// Race:小于52属于一些不会主动攻击角色的怪物类型
        /// 如：鹿 鸡 羊
        /// </summary>
        /// <returns></returns>
        private static bool IsPassiveAttack(BaseObject monsterObject)
        {
            return monsterObject.Race <= 52;
        }
    }
}