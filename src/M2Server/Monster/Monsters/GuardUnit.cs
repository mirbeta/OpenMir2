using M2Server.Npc;
using OpenMir2;
using OpenMir2.Enums;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Data;
using SystemModule.Enums;

namespace M2Server.Monster.Monsters
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

        public override void Struck(IActor hiter)
        {
            base.Struck(hiter);
            if (Castle != null)
            {
                CrimeforCastle = true;
                CrimeforCastleTime = HUtil32.GetTickCount();
            }
        }

        public override bool IsProperTarget(IActor baseObject)
        {
            if (Castle != null)
            {
                if (LastHiter == baseObject)
                {
                    return true;
                }
                if (Castle.UnderWar)
                {
                    return true;
                }
                if (baseObject.Race == ActorRace.Guard)
                {
                    GuardUnit guardObject = (GuardUnit)baseObject;
                    if (guardObject.CrimeforCastle)
                    {
                        if ((HUtil32.GetTickCount() - guardObject.CrimeforCastleTime) < (2 * 60 * 1000))
                        {
                            return true;
                        }
                        else
                        {
                            guardObject.CrimeforCastle = false;
                        }
                        if (guardObject.Castle != null)
                        {
                            guardObject.CrimeforCastle = false;
                            return false;
                        }
                    }
                }
                if (Castle.MasterGuild != null)
                {
                    if (baseObject.Master == null)
                    {
                        if (baseObject.Race == ActorRace.Play)
                        {
                            if (Castle.MasterGuild == ((IPlayerActor)baseObject).MyGuild || Castle.MasterGuild.IsAllyGuild(((IPlayerActor)baseObject).MyGuild))
                            {
                                if (LastHiter != baseObject)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (baseObject.Master.Race == ActorRace.Play)
                        {
                            if (Castle.MasterGuild == ((IPlayerActor)baseObject.Master).MyGuild || Castle.MasterGuild.IsAllyGuild(((IPlayerActor)baseObject.Master).MyGuild))
                            {
                                if (LastHiter != baseObject.Master && LastHiter != baseObject)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                if (baseObject.AdminMode || baseObject.StoneMode || baseObject.Race >= ActorRace.NPC && baseObject.Race < ActorRace.Animal || baseObject == this || baseObject.Castle == Castle)
                {
                    return false;
                }
            }
            if (LastHiter == baseObject)
            {
                return true;
            }
            if (baseObject.TargetCret != null && baseObject.TargetCret.Race == ActorRace.MonsterArcherguard)
            {
                return true;
            }
            if (baseObject.Race == ActorRace.Play)
            {
                if (baseObject.AdminMode) //只有玩家才有管理员模式
                {
                    return false;
                }
                if (((IPlayerActor)baseObject).PvpLevel() >= 2)
                {
                    return true;
                }
            }
            if (baseObject.StoneMode || baseObject == this)
            {
                return false;
            }
            return true;
        }

        public override void SearchViewRange()
        {
            const string sExceptionMsg = "[Exception] TBaseObject::SearchViewRange {0} {1} {2} {3} {4}";
            int n24 = 0;
            IsVisibleActive = false;// 先置为FALSE
            for (int i = 0; i < VisibleActors.Count; i++)
            {
                VisibleActors[i].VisibleFlag = VisibleFlag.Hidden;
            }
            short nStartX = (short)(CurrX - ViewRange);
            short nEndX = (short)(CurrX + ViewRange);
            short nStartY = (short)(CurrY - ViewRange);
            short nEndY = (short)(CurrY + ViewRange);
            try
            {
                for (short nX = nStartX; nX <= nEndX; nX++)
                {
                    for (short nY = nStartY; nY <= nEndY; nY++)
                    {
                        ref MapCellInfo cellInfo = ref Envir.GetCellInfo(nX, nY, out bool cellSuccess);
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
                                    IActor baseObject = SystemShare.ActorMgr.Get(cellObject.CellObjId);
                                    if (baseObject != null)
                                    {
                                        if (!baseObject.Death && !baseObject.Invisible)
                                        {
                                            if (CanPassiveAttack(baseObject))//守卫和护卫不搜索不主动攻击的怪物
                                            {
                                                continue;
                                            }
                                            if (!baseObject.Ghost && !baseObject.FixedHideMode && !baseObject.ObMode)
                                            {
                                                if (((Math.Abs(baseObject.CurrX - CurrX) <= 6) && (Math.Abs(baseObject.CurrY - CurrY) <= 6)))
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
                LogService.Error(Format(sExceptionMsg, n24, ChrName, MapName, CurrX, CurrY));
                LogService.Error(e.Message);
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
                    if (visibleBaseObject.VisibleFlag == VisibleFlag.Hidden)
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
                LogService.Error(Format(sExceptionMsg, n24, ChrName, MapName, CurrX, CurrY));
                KickException();
            }
        }

        /// <summary>
        /// 是否被动攻击怪物类型
        /// Race:小于52属于一些不会主动攻击角色的怪物类型
        /// 如：鹿 鸡 羊
        /// </summary>
        /// <returns></returns>
        private static bool CanPassiveAttack(IActor monsterObject)
        {
            return monsterObject.Race <= 52 || monsterObject.Race == 112;
        }
    }
}