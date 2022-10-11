using GameSvr.Actor;
using GameSvr.Event;
using GameSvr.Items;
using GameSvr.Maps;
using SystemModule;
using SystemModule.Consts;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.RobotPlay
{
    public partial class RobotPlayObject
    {
        public override void Run()
        {
            int nWhere;
            int nPercent;
            int nValue;
            StdItem StdItem;
            UserItem UserItem;
            bool boRecalcAbilitys;
            bool boFind = false;
            try
            {
                if (!Ghost && !Death && !FixedHideMode && !StoneMode && StatusArr[StatuStateConst.POISON_STONE] == 0)
                {
                    if (HUtil32.GetTickCount() - WalkTick > WalkSpeed)
                    {
                        WalkTick = HUtil32.GetTickCount();
                        if (TargetCret != null)
                        {
                            if (TargetCret.Death || TargetCret.Ghost || TargetCret.InSafeZone() || TargetCret.Envir != Envir || Math.Abs(CurrX - TargetCret.CurrX) > 11 || Math.Abs(CurrY - TargetCret.CurrY) > 11)
                            {
                                DelTargetCreat();
                            }
                        }
                        if (!m_boAIStart)
                        {
                            DelTargetCreat();
                        }
                        SearchTarget();
                        if (m_ManagedEnvir != Envir) // 所在地图不是挂机地图则清空目标
                        {
                            DelTargetCreat();
                        }
                        if (Thinking())
                        {
                            base.Run();
                            return;
                        }
                        if (m_boProtectStatus) // 守护状态
                        {
                            if (m_nProtectTargetX == 0 || m_nProtectTargetY == 0)// 取守护坐标
                            {
                                m_nProtectTargetX = CurrX;// 守护坐标
                                m_nProtectTargetY = CurrY;// 守护坐标
                            }
                            if (!m_boProtectOK && m_ManagedEnvir != null && TargetCret == null)
                            {
                                GotoProtect();
                                m_nGotoProtectXYCount++;
                                if (Math.Abs(CurrX - m_nProtectTargetX) <= 3 && Math.Abs(CurrY - m_nProtectTargetY) <= 3)
                                {
                                    Direction = (byte)M2Share.RandomNumber.Random(8);
                                    m_boProtectOK = true;
                                    m_nGotoProtectXYCount = 0;// 是向守护坐标的累计数
                                }
                                if (m_nGotoProtectXYCount > 20 && !m_boProtectOK)// 20次还没有走到守护坐标，则飞回坐标上
                                {
                                    if (Math.Abs(CurrX - m_nProtectTargetX) > 13 || Math.Abs(CurrY - m_nProtectTargetY) > 13)
                                    {
                                        SpaceMove(m_ManagedEnvir.MapName, m_nProtectTargetX, m_nProtectTargetY, 1);
                                        Direction = (byte)M2Share.RandomNumber.Random(8);
                                        m_boProtectOK = true;
                                        m_nGotoProtectXYCount = 0;// 是向守护坐标的累计数
                                    }
                                }
                                base.Run();
                                return;
                            }
                        }
                        if (TargetCret != null)
                        {
                            if (AttackTarget())// 攻击
                            {
                                base.Run();
                                return;
                            }
                            else if (IsNeedAvoid()) // 自动躲避
                            {
                                m_dwActionTick = HUtil32.GetTickCount() - 10;
                                AutoAvoid();
                                base.Run();
                                return;
                            }
                            else
                            {
                                if (IsNeedGotoXY())// 是否走向目标
                                {
                                    m_dwActionTick = HUtil32.GetTickCount();
                                    TargetX = TargetCret.CurrX;
                                    TargetY = TargetCret.CurrY;
                                    if (AllowUseMagic(12) && Job == 0)
                                    {
                                        GetGotoXY(TargetCret, 2);
                                    }
                                    if (Job > 0)
                                    {
                                        if (M2Share.Config.boHeroAttackTarget && Abil.Level < 22 || M2Share.Config.boHeroAttackTao && TargetCret.WAbil.MaxHP < 700 && Job == PlayJob.Taoist && TargetCret.Race != ActorRace.Play)
                                        {
                                            // 道法22前是否物理攻击
                                            if (Master != null)
                                            {
                                                if (Math.Abs(Master.CurrX - CurrX) > 6 || Math.Abs(Master.CurrY - CurrY) > 6)
                                                {
                                                    base.Run();
                                                    return;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            GetGotoXY(TargetCret, 3); // 道法只走向目标3格范围
                                        }
                                    }
                                    GotoTargetXY(TargetX, TargetY, 0);
                                    base.Run();
                                    return;
                                }
                            }
                        }

                        if (IsRobot && !Ghost && !Death)
                        {
                            if (M2Share.Config.boHPAutoMoveMap)
                            {
                                if (WAbil.HP <= Math.Round(WAbil.MaxHP * 0.3) && HUtil32.GetTickCount() - m_dwHPToMapHomeTick > 15000) // 低血时回城或回守护点 
                                {
                                    m_dwHPToMapHomeTick = HUtil32.GetTickCount();
                                    DelTargetCreat();
                                    if (m_boProtectStatus) // 守护状态
                                    {
                                        SpaceMove(m_ManagedEnvir.MapName, m_nProtectTargetX, m_nProtectTargetY, 1);// 地图移动
                                        Direction = M2Share.RandomNumber.RandomByte(8);
                                        m_boProtectOK = true;
                                        m_nGotoProtectXYCount = 0; // 是向守护坐标的累计数 20090203
                                    }
                                    else
                                    {
                                        MoveToHome(); // 不是守护状态，直接回城
                                    }
                                }
                            }
                            if (M2Share.Config.boAutoRepairItem)
                            {
                                if (HUtil32.GetTickCount() - m_dwAutoRepairItemTick > 15000)
                                {
                                    m_dwAutoRepairItemTick = HUtil32.GetTickCount();
                                    boRecalcAbilitys = false;
                                    for (nWhere = 0; nWhere < m_UseItemNames.Length; nWhere++)
                                    {
                                        if (string.IsNullOrEmpty(m_UseItemNames[nWhere]))
                                        {
                                            continue;
                                        }
                                        if (UseItems[nWhere].Index <= 0)
                                        {
                                            StdItem = M2Share.WorldEngine.GetStdItem(m_UseItemNames[nWhere]);
                                            if (StdItem != null)
                                            {
                                                UserItem = new UserItem();
                                                if (M2Share.WorldEngine.CopyToUserItemFromName(m_UseItemNames[nWhere], ref UserItem))
                                                {
                                                    boRecalcAbilitys = true;
                                                    if (M2Share.StdModeMap.Contains(StdItem.StdMode))
                                                    {
                                                        if (StdItem.Shape == 130 || StdItem.Shape == 131 || StdItem.Shape == 132)
                                                        {
                                                            //M2Share.WorldEngine.GetUnknowItemValue(UserItem);
                                                        }
                                                    }
                                                }
                                                UseItems[nWhere] = UserItem;
                                                Dispose(UserItem);
                                            }
                                        }
                                    }
                                    if (m_BagItemNames.Count > 0)
                                    {
                                        for (var i = 0; i < m_BagItemNames.Count; i++)
                                        {
                                            for (var j = 0; j < ItemList.Count; j++)
                                            {
                                                UserItem = ItemList[j];
                                                if (UserItem != null)
                                                {
                                                    StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                                                    if (StdItem != null)
                                                    {
                                                        boFind = false;
                                                        if (StdItem.Name.CompareTo(m_BagItemNames[i]) == 0)
                                                        {
                                                            boFind = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            if (!boFind)
                                            {
                                                UserItem = new UserItem();
                                                if (M2Share.WorldEngine.CopyToUserItemFromName(m_BagItemNames[i], ref UserItem))
                                                {
                                                    if (!AddItemToBag(UserItem))
                                                    {
                                                        Dispose(UserItem);
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    Dispose(UserItem);
                                                }
                                            }
                                        }
                                    }
                                    for (nWhere = 0; nWhere <= UseItems.Length; nWhere++)
                                    {
                                        if (UseItems[nWhere] != null && UseItems[nWhere].Index > 0)
                                        {
                                            StdItem = M2Share.WorldEngine.GetStdItem(UseItems[nWhere].Index);
                                            if (StdItem != null)
                                            {
                                                if (UseItems[nWhere].DuraMax > UseItems[nWhere].Dura && StdItem.StdMode != 43)
                                                {
                                                    /*if (PlugOfCheckCanItem(3, StdItem.Name, false, 0, 0))
                                                    {
                                                        continue;
                                                    }*/
                                                    UseItems[nWhere].Dura = UseItems[nWhere].DuraMax;
                                                }
                                            }
                                        }
                                    }
                                    if (boRecalcAbilitys)
                                    {
                                        RecalcAbilitys();
                                    }
                                }
                            }
                            if (M2Share.Config.boRenewHealth) // 自动增加HP MP
                            {
                                if (HUtil32.GetTickCount() - m_dwAutoAddHealthTick > 5000)
                                {
                                    m_dwAutoAddHealthTick = HUtil32.GetTickCount();
                                    nPercent = WAbil.HP * 100 / WAbil.MaxHP;
                                    nValue = WAbil.MaxHP / 10;
                                    if (nPercent < M2Share.Config.nRenewPercent)
                                    {
                                        if (WAbil.HP + nValue >= WAbil.MaxHP)
                                        {
                                            WAbil.HP = WAbil.MaxHP;
                                        }
                                        else
                                        {
                                            WAbil.HP += (ushort)nValue;
                                        }
                                    }
                                    nValue = WAbil.MaxMP / 10;
                                    nPercent = WAbil.MP * 100 / WAbil.MaxMP;
                                    if (nPercent < M2Share.Config.nRenewPercent)
                                    {
                                        if (WAbil.MP + nValue >= WAbil.MaxMP)
                                        {
                                            WAbil.MP = WAbil.MaxMP;
                                        }
                                        else
                                        {
                                            WAbil.MP += (ushort)nValue;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (!Ghost && !Death && !FixedHideMode && !StoneMode && StatusArr[StatuStateConst.POISON_STONE] == 0)
                    {
                        if (m_boProtectStatus && TargetCret == null)// 守护状态
                        {
                            if (Math.Abs(CurrX - m_nProtectTargetX) > 50 || Math.Abs(CurrY - m_nProtectTargetY) > 50)
                            {
                                m_boProtectOK = false;
                            }
                        }
                        if (TargetCret == null)
                        {
                            if (Master != null)
                            {
                                FollowMaster();
                            }
                            else
                            {
                                Wondering();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                M2Share.Log.Error(ex.StackTrace);
            }
            base.Run();
        }

        protected override bool IsProtectTarget(BaseObject BaseObject)
        {
            return base.IsProtectTarget(BaseObject);
        }

        protected override bool IsAttackTarget(BaseObject BaseObject)
        {
            return base.IsAttackTarget(BaseObject);
        }

        public override bool IsProperTarget(BaseObject BaseObject)
        {
            bool result = false;
            if (BaseObject != null)
            {
                if (base.IsProperTarget(BaseObject))
                {
                    result = true;
                    if (BaseObject.Master != null)
                    {
                        if (BaseObject.Master == this || BaseObject.Master.IsRobot && !InFreePkArea)
                        {
                            result = false;
                        }
                    }
                    if (BaseObject.IsRobot && !InFreePkArea)// 假人不攻击假人,行会战除外
                    {
                        result = false;
                    }
                    switch (BaseObject.Race)
                    {
                        case ActorRace.ArcherGuard:
                        case ActorRace.Exercise:// 不主动攻击练功师 弓箭手
                            if (BaseObject.TargetCret != this)
                            {
                                result = false;
                            }
                            break;
                        case ActorRace.NPC:
                        case ActorRace.Guard:
                        case 12: // 不攻击大刀卫士
                            result = false;
                            break;
                        case ActorRace.SabukDoor:
                        case ActorRace.SabukWall:
                        case 158: // 沙巴克城门,沙巴克左城墙,宠物类
                            result = false;
                            break;
                    }
                }
                else
                {
                    if (AttatckMode == AttackMode.HAM_PKATTACK)// 红名模式，除红名目标外，受人攻击时才还击
                    {
                        if (BaseObject.Race == ActorRace.Play)
                        {
                            if (PvpLevel() >= 2)
                            {
                                if (BaseObject.PvpLevel() < 2)
                                {
                                    result = true;
                                }
                                else
                                {
                                    result = false;
                                }
                            }
                            else
                            {
                                if (BaseObject.PvpLevel() >= 2)
                                {
                                    result = true;
                                }
                                else
                                {
                                    result = false;
                                }
                            }
                        }
                        if (IsRobot && !result)
                        {
                            if (BaseObject.Race == ActorRace.Play || BaseObject.Master != null)
                            {
                                if (BaseObject.TargetCret != null)
                                {
                                    if (BaseObject.TargetCret == this)
                                    {
                                        result = true;
                                    }
                                }
                                if (BaseObject.LastHiter != null)
                                {
                                    if (BaseObject.LastHiter == this)
                                    {
                                        result = true;
                                    }
                                }
                                if (BaseObject.ExpHitter != null)
                                {
                                    if (BaseObject.LastHiter == this)
                                    {
                                        result = true;
                                    }
                                }
                            }
                        }
                        if (BaseObject.Race == ActorRace.Play || BaseObject.Master != null)// 安全区不能打人物和英雄
                        {
                            if (BaseObject.InSafeZone() || InSafeZone())
                            {
                                result = false;
                            }
                        }
                        if (BaseObject.Master == this)
                        {
                            result = false;
                        }
                        if (BaseObject.IsRobot && (!InFreePkArea || BaseObject.PvpLevel() < 2))// 假人不攻击假人,行会战除外
                        {
                            result = false;
                        }
                        switch (BaseObject.Race)
                        {
                            case ActorRace.ArcherGuard:
                            case ActorRace.Exercise:// 不主动攻击练功师 弓箭手
                                if (BaseObject.TargetCret != this)
                                {
                                    result = false;
                                }
                                break;
                            case ActorRace.NPC:
                            case ActorRace.Guard:
                            case 12:// 不攻击大刀卫士
                                result = false;
                                break;
                            case ActorRace.SabukDoor:
                            case ActorRace.SabukWall:
                            case 158:// 沙巴克城门,沙巴克左城墙,宠物类
                                result = false;
                                break;
                        }
                    }
                }
            }
            return result;
        }

        public override bool IsProperFriend(BaseObject BaseObject)
        {
            return base.IsProperFriend(BaseObject);
        }

        public override void SearchViewRange()
        {
            int nIdx;
            MapCellInfo cellInfo;
            CellObject cellObject = null;
            BaseObject baseObject;
            EventInfo MapEvent;
            VisibleFlag nVisibleFlag;
            const string sExceptionMsg = "RobotPlayObject::SearchViewRange 1-{0} {1} {2} {3} {4}";
            if (Ghost)
            {
                return;
            }
            if (VisibleItems.Count > 0)
            {
                for (var i = 0; i < VisibleItems.Count; i++)
                {
                    VisibleItems[i].VisibleFlag = 0;
                }
            }
            try
            {
                var nStartX = (short)(CurrX - ViewRange);
                var nEndX = (short)(CurrX + ViewRange);
                var nStartY = (short)(CurrY - ViewRange);
                var nEndY = (short)(CurrY + ViewRange);
                var dwRunTick = HUtil32.GetTickCount();
                for (var nX = nStartX; nX <= nEndX; nX++)
                {
                    for (var nY = nStartY; nY <= nEndY; nY++)
                    {
                        var cellSuccess = false;
                        cellInfo = Envir.GetCellInfo(nX, nY, ref cellSuccess);
                        if (cellSuccess && cellInfo.IsAvailable)
                        {
                            nIdx = 0;
                            while (cellInfo.Count > 0)
                            {
                                if (HUtil32.GetTickCount() - dwRunTick > 500)
                                {
                                    break;
                                }
                                if (cellInfo.IsAvailable && cellInfo.Count <= 0)
                                {
                                    cellInfo.Dispose();
                                    break;
                                }
                                if (cellInfo.ObjList == null)
                                {
                                    break;
                                }
                                if (cellInfo.Count <= nIdx)
                                {
                                    break;
                                }
                                try
                                {
                                    cellObject = cellInfo.ObjList[nIdx];
                                }
                                catch
                                {
                                    cellInfo.Remove(cellObject);
                                    continue;
                                }
                                if (cellObject != null)
                                {
                                    if (!cellObject.Dispose)
                                    {
                                        switch (cellObject.CellType)
                                        {
                                            case CellType.Play:
                                            case CellType.Monster:
                                                if (HUtil32.GetTickCount() - cellObject.AddTime >= 60000)
                                                {
                                                    cellObject.Dispose = true;
                                                    cellInfo.Remove(cellObject);
                                                    if (cellInfo.Count <= 0)
                                                    {
                                                        cellInfo.Dispose();
                                                        break;
                                                    }
                                                    continue;
                                                }
                                                baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId); ;
                                                if (baseObject != null)
                                                {
                                                    if (!baseObject.Ghost && !baseObject.FixedHideMode && !baseObject.ObMode)
                                                    {
                                                        if (Race < ActorRace.Animal || Master != null || CrazyMode || WantRefMsg || baseObject.Master != null && Math.Abs(baseObject.CurrX - CurrX) <= 3 && Math.Abs(baseObject.CurrY - CurrY) <= 3 || baseObject.Race == ActorRace.Play)
                                                        {
                                                            UpdateVisibleGay(baseObject);
                                                        }
                                                    }
                                                }
                                                break;
                                            case CellType.Item:
                                                if (Race == ActorRace.Play)
                                                {
                                                    if (HUtil32.GetTickCount() - cellObject.AddTime > M2Share.Config.ClearDropOnFloorItemTime)
                                                    {
                                                        if (cellObject.CellObjId > 0)
                                                        {
                                                            M2Share.CellObjectSystem.Dispose(cellObject.CellObjId);
                                                        }
                                                        if (cellObject != null)
                                                        {
                                                            cellObject.Dispose = true;
                                                        }
                                                        cellInfo.Remove(cellObject);
                                                        if (cellInfo.Count <= 0)
                                                        {
                                                            cellInfo.Dispose();
                                                            break;
                                                        }
                                                        continue;
                                                    }
                                                    var MapItem = (MapItem)M2Share.CellObjectSystem.Get(cellObject.CellObjId);
                                                    UpdateVisibleItem(nX, nY, MapItem);
                                                    if (MapItem.OfBaseObject != 0 || MapItem.DropBaseObject != 0)
                                                    {
                                                        if (HUtil32.GetTickCount() - MapItem.CanPickUpTick > M2Share.Config.FloorItemCanPickUpTime)
                                                        {
                                                            MapItem.OfBaseObject = 0;
                                                            MapItem.DropBaseObject = 0;
                                                        }
                                                        else
                                                        {
                                                            if (MapItem.OfBaseObject > 0)
                                                            {
                                                                if (M2Share.ActorMgr.Get(MapItem.OfBaseObject).Ghost)
                                                                {
                                                                    MapItem.OfBaseObject = 0;
                                                                }
                                                            }
                                                            if (MapItem.DropBaseObject > 0)
                                                            {
                                                                if (M2Share.ActorMgr.Get(MapItem.DropBaseObject).Ghost)
                                                                {
                                                                    MapItem.DropBaseObject = 0;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                break;
                                            case CellType.Event:
                                                if (Race == ActorRace.Play)
                                                {
                                                    if (cellObject.CellObjId < 0)
                                                    {
                                                        MapEvent = (EventInfo)M2Share.CellObjectSystem.Get(cellObject.CellObjId);
                                                        UpdateVisibleEvent(nX, nY, MapEvent);
                                                    }
                                                }
                                                break;
                                        }
                                    }
                                }
                                nIdx++;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                M2Share.Log.Error(Format(sExceptionMsg, new object[] { ChrName, MapName, CurrX, CurrY }));
                KickException();
            }
            try
            {
                var n18 = 0;
                while (true)
                {
                    try
                    {
                        if (VisibleActors.Count <= n18)
                        {
                            break;
                        }
                        VisibleBaseObject VisibleBaseObject;
                        try
                        {
                            VisibleBaseObject = VisibleActors[n18];
                            nVisibleFlag = VisibleBaseObject.VisibleFlag;
                        }
                        catch
                        {
                            VisibleActors.RemoveAt(n18);
                            if (VisibleActors.Count > 0)
                            {
                                continue;
                            }
                            break;
                        }
                        switch (VisibleBaseObject.VisibleFlag)
                        {
                            case VisibleFlag.Visible:
                                if (Race == ActorRace.Play)
                                {
                                    baseObject = VisibleBaseObject.BaseObject;
                                    if (baseObject != null)
                                    {
                                        if (!baseObject.FixedHideMode && !baseObject.Ghost)
                                        {
                                            SendMsg(baseObject, Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
                                        }
                                    }
                                }
                                VisibleActors.RemoveAt(n18);
                                if (VisibleBaseObject != null)
                                {
                                    Dispose(VisibleBaseObject);
                                }
                                continue;
                            case VisibleFlag.Hidden:
                                if (Race == ActorRace.Play)
                                {
                                    baseObject = VisibleBaseObject.BaseObject;
                                    if (baseObject != null)
                                    {
                                        if (baseObject != this && !baseObject.Ghost && !Ghost)
                                        {
                                            if (baseObject.Death)
                                            {
                                                if (baseObject.Skeleton)
                                                {
                                                    SendMsg(baseObject, Grobal2.RM_SKELETON, baseObject.Direction, baseObject.CurrX, baseObject.CurrY, 0, "");
                                                }
                                                else
                                                {
                                                    SendMsg(baseObject, Grobal2.RM_DEATH, baseObject.Direction, baseObject.CurrX, baseObject.CurrY, 0, "");
                                                }
                                            }
                                            else
                                            {
                                                if (baseObject != null)
                                                {
                                                    SendMsg(baseObject, Grobal2.RM_TURN, baseObject.Direction, baseObject.CurrX, baseObject.CurrY, 0, baseObject.GetShowName());
                                                }
                                            }
                                        }
                                    }
                                }
                                VisibleBaseObject.VisibleFlag = 0;
                                break;
                            case VisibleFlag.Invisible:
                                VisibleBaseObject.VisibleFlag = 0;
                                break;
                        }
                    }
                    catch
                    {
                        break;
                    }
                    n18++;
                }
            }
            catch (Exception)
            {
                M2Share.Log.Error(Format(sExceptionMsg, new object[] { ChrName, MapName, CurrX, CurrY }));
                KickException();
            }
            try
            {
                var position = 0;
                while (true)
                {
                    try
                    {
                        if (VisibleItems.Count <= position)
                        {
                            break;
                        }

                        VisibleMapItem VisibleMapItem;
                        try
                        {
                            VisibleMapItem = VisibleItems[position];
                            nVisibleFlag = VisibleMapItem.VisibleFlag;
                        }
                        catch
                        {
                            VisibleItems.RemoveAt(position);
                            if (VisibleItems.Count > 0)
                            {
                                continue;
                            }
                            break;
                        }
                        if (VisibleMapItem.VisibleFlag == 0)
                        {
                            VisibleItems.RemoveAt(position);
                            VisibleMapItem = null;
                            if (VisibleItems.Count > 0)
                            {
                                continue;
                            }
                            break;
                        }
                    }
                    catch
                    {
                        break;
                    }
                    position++;
                }
                position = 0;
                while (true)
                {
                    try
                    {
                        if (VisibleEvents.Count <= position)
                        {
                            break;
                        }
                        try
                        {
                            MapEvent = VisibleEvents[position];
                            nVisibleFlag = MapEvent.VisibleFlag;
                        }
                        catch
                        {
                            VisibleEvents.RemoveAt(position);
                            if (VisibleEvents.Count > 0)
                            {
                                continue;
                            }
                            break;
                        }
                        if (MapEvent != null)
                        {
                            switch (MapEvent.VisibleFlag)
                            {
                                case VisibleFlag.Visible:
                                    SendMsg(this, Grobal2.RM_HIDEEVENT, 0, MapEvent.Id, MapEvent.nX, MapEvent.nY, "");
                                    VisibleEvents.RemoveAt(position);
                                    if (VisibleEvents.Count > 0)
                                    {
                                        continue;
                                    }
                                    break;
                                case VisibleFlag.Invisible:
                                    MapEvent.VisibleFlag = 0;
                                    break;
                                case VisibleFlag.Hidden:
                                    SendMsg(this, Grobal2.RM_SHOWEVENT, MapEvent.EventType, MapEvent.Id, HUtil32.MakeLong(MapEvent.nX, MapEvent.EventParam), MapEvent.nY, "");
                                    MapEvent.VisibleFlag = 0;
                                    break;
                            }
                        }
                    }
                    catch
                    {
                        break;
                    }
                    position++;
                }
            }
            catch
            {
                M2Share.Log.Error(ChrName + ',' + MapName + ',' + CurrX + ',' + CurrY + ',' + " SearchViewRange");
                KickException();
            }
        }

        public override void Struck(BaseObject hiter)
        {
            StruckTick = HUtil32.GetTickCount();
            if (hiter != null)
            {
                if (TargetCret == null && IsProperTarget(hiter))
                {
                    SetTargetCreat(hiter);
                }
                else
                {
                    if (hiter.Race == ActorRace.Play || hiter.Master != null && hiter.GetMaster().Race == ActorRace.Play)
                    {
                        if (TargetCret != null && (TargetCret.Race == ActorRace.Play || TargetCret.Master != null && TargetCret.GetMaster().Race == ActorRace.Play))
                        {
                            if (Struck_MINXY(TargetCret, hiter) == hiter || M2Share.RandomNumber.Random(6) == 0)
                            {
                                SetTargetCreat(hiter);
                            }
                        }
                        else
                        {
                            SetTargetCreat(hiter);
                        }
                    }
                    else
                    {
                        if (TargetCret != null && Struck_MINXY(TargetCret, hiter) == hiter || M2Share.RandomNumber.Random(6) == 0)
                        {
                            if (Job > 0 || TargetCret != null && (HUtil32.GetTickCount() - TargetFocusTick) > 1000 * 3)
                            {
                                if (IsProperTarget(hiter))
                                {
                                    SetTargetCreat(hiter);
                                }
                            }
                        }
                    }
                }
                if (hiter.Race == ActorRace.Play && !hiter.IsRobot && TargetCret == hiter)
                {
                    if (M2Share.RandomNumber.Random(8) == 0 && m_AISayMsgList.Count > 0)
                    {
                        if (HUtil32.GetTickCount() >= m_dwDisableSayMsgTick)
                        {
                            m_boDisableSayMsg = false;
                        }
                        var boDisableSayMsg = m_boDisableSayMsg;
                        //g_DenySayMsgList.Lock;
                        //if (g_DenySayMsgList.GetIndex(m_sChrName) >= 0)
                        //{
                        //    boDisableSayMsg = true;
                        //}
                        //g_DenySayMsgList.UnLock;
                        if (!boDisableSayMsg)
                        {
                            SendRefMsg(Grobal2.RM_HEAR, 0, M2Share.Config.btHearMsgFColor, M2Share.Config.btHearMsgBColor, 0, ChrName + ':' + m_AISayMsgList[M2Share.RandomNumber.Random(m_AISayMsgList.Count)]);
                        }
                    }
                }
            }
            if (Animal)
            {
                MeatQuality = (ushort)(MeatQuality - M2Share.RandomNumber.Random(300));
                if (MeatQuality < 0)
                {
                    MeatQuality = 0;
                }
            }
            AttackTick = (ushort)(AttackTick + (150 - HUtil32._MIN(130, Abil.Level * 4)));
        }

        protected override void SearchTarget()
        {
            if ((TargetCret == null || HUtil32.GetTickCount() - m_dwSearchTargetTick > 1000) && m_boAIStart)
            {
                m_dwSearchTargetTick = HUtil32.GetTickCount();
                if (TargetCret == null || !(TargetCret != null && TargetCret.Race == ActorRace.Play) || TargetCret.Master != null && TargetCret.Master.Race == ActorRace.Play || (HUtil32.GetTickCount() - StruckTick) > 15000)
                {
                    base.SearchTarget();
                }
            }
        }

        public override void Die()
        {
            if (m_boAIStart)
            {
                m_boAIStart = false;
            }
            base.Die();
        }
    }
}