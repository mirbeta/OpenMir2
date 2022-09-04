using GameSvr.Actor;
using GameSvr.Event;
using GameSvr.Items;
using GameSvr.Maps;
using System.Collections;
using SystemModule;
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
            TUserItem UserItem;
            bool boRecalcAbilitys;
            bool boFind = false;
            try
            {
                if (!Ghost && !Death && !FixedHideMode && !StoneMode && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
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
                                        if (M2Share.g_Config.boHeroAttackTarget && Abil.Level < 22 || M2Share.g_Config.boHeroAttackTao && TargetCret.m_WAbil.MaxHP < 700 && Job == PlayJob.Taoist && TargetCret.Race != Grobal2.RC_PLAYOBJECT)
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
                            if (M2Share.g_Config.boHPAutoMoveMap)
                            {
                                if (m_WAbil.HP <= Math.Round(m_WAbil.MaxHP * 0.3) && HUtil32.GetTickCount() - m_dwHPToMapHomeTick > 15000) // 低血时回城或回守护点 
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
                            if (M2Share.g_Config.boAutoRepairItem)
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
                                        if (UseItems[nWhere].wIndex <= 0)
                                        {
                                            StdItem = M2Share.UserEngine.GetStdItem(m_UseItemNames[nWhere]);
                                            if (StdItem != null)
                                            {
                                                UserItem = new TUserItem();
                                                if (M2Share.UserEngine.CopyToUserItemFromName(m_UseItemNames[nWhere], ref UserItem))
                                                {
                                                    boRecalcAbilitys = true;
                                                    if (new ArrayList(new byte[] { 15, 19, 20, 21, 22, 23, 24, 26 }).Contains(StdItem.StdMode))
                                                    {
                                                        if (StdItem.Shape == 130 || StdItem.Shape == 131 || StdItem.Shape == 132)
                                                        {
                                                            //M2Share.UserEngine.GetUnknowItemValue(UserItem);
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
                                                    StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
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
                                                UserItem = new TUserItem();
                                                if (M2Share.UserEngine.CopyToUserItemFromName(m_BagItemNames[i], ref UserItem))
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
                                        if (UseItems[nWhere] != null && UseItems[nWhere].wIndex > 0)
                                        {
                                            StdItem = M2Share.UserEngine.GetStdItem(UseItems[nWhere].wIndex);
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
                            if (M2Share.g_Config.boRenewHealth) // 自动增加HP MP
                            {
                                if (HUtil32.GetTickCount() - m_dwAutoAddHealthTick > 5000)
                                {
                                    m_dwAutoAddHealthTick = HUtil32.GetTickCount();
                                    nPercent = m_WAbil.HP * 100 / m_WAbil.MaxHP;
                                    nValue = m_WAbil.MaxHP / 10;
                                    if (nPercent < M2Share.g_Config.nRenewPercent)
                                    {
                                        if (m_WAbil.HP + nValue >= m_WAbil.MaxHP)
                                        {
                                            m_WAbil.HP = m_WAbil.MaxHP;
                                        }
                                        else
                                        {
                                            m_WAbil.HP += (ushort)nValue;
                                        }
                                    }
                                    nValue = m_WAbil.MaxMP / 10;
                                    nPercent = m_WAbil.MP * 100 / m_WAbil.MaxMP;
                                    if (nPercent < M2Share.g_Config.nRenewPercent)
                                    {
                                        if (m_WAbil.MP + nValue >= m_WAbil.MaxMP)
                                        {
                                            m_WAbil.MP = m_WAbil.MaxMP;
                                        }
                                        else
                                        {
                                            m_WAbil.MP += (ushort)nValue;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (!Ghost && !Death && !FixedHideMode && !StoneMode && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
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
                Console.WriteLine(ex.StackTrace);
            }
            base.Run();
        }

        protected override bool IsProtectTarget(TBaseObject BaseObject)
        {
            return base.IsProtectTarget(BaseObject);
        }

        protected override bool IsAttackTarget(TBaseObject BaseObject)
        {
            return base.IsAttackTarget(BaseObject);
        }

        public override bool IsProperTarget(TBaseObject BaseObject)
        {
            bool result = false;
            if (BaseObject != null)
            {
                if (base.IsProperTarget(BaseObject))
                {
                    result = true;
                    if (BaseObject.Master != null)
                    {
                        if (BaseObject.Master == this || BaseObject.Master.IsRobot && !InFreePKArea)
                        {
                            result = false;
                        }
                    }
                    if (BaseObject.IsRobot && !InFreePKArea)// 假人不攻击假人,行会战除外
                    {
                        result = false;
                    }
                    switch (BaseObject.Race)
                    {
                        case Grobal2.RC_ARCHERGUARD:
                        case 55:// 不主动攻击练功师 弓箭手
                            if (BaseObject.TargetCret != this)
                            {
                                result = false;
                            }
                            break;
                        case 10:
                        case 11:
                        case 12: // 不攻击大刀卫士
                            result = false;
                            break;
                        case 110:
                        case 111:
                        case 158: // 沙巴克城门,沙巴克左城墙,宠物类
                            result = false;
                            break;
                    }
                }
                else
                {
                    if (AttatckMode == AttackMode.HAM_PKATTACK)// 红名模式，除红名目标外，受人攻击时才还击
                    {
                        if (BaseObject.Race == Grobal2.RC_PLAYOBJECT)
                        {
                            if (PKLevel() >= 2)
                            {
                                if (BaseObject.PKLevel() < 2)
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
                                if (BaseObject.PKLevel() >= 2)
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
                            if (BaseObject.Race == Grobal2.RC_PLAYOBJECT || BaseObject.Master != null)
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
                        if (BaseObject.Race == Grobal2.RC_PLAYOBJECT || BaseObject.Master != null)// 安全区不能打人物和英雄
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
                        if (BaseObject.IsRobot && (!InFreePKArea || BaseObject.PKLevel() < 2))// 假人不攻击假人,行会战除外
                        {
                            result = false;
                        }
                        switch (BaseObject.Race)
                        {
                            case Grobal2.RC_ARCHERGUARD:
                            case 55:// 不主动攻击练功师 弓箭手
                                if (BaseObject.TargetCret != this)
                                {
                                    result = false;
                                }
                                break;
                            case 10:
                            case 11:
                            case 12:// 不攻击大刀卫士
                                result = false;
                                break;
                            case 110:
                            case 111:
                            case 158:// 沙巴克城门,沙巴克左城墙,宠物类
                                result = false;
                                break;
                        }
                    }
                }
            }
            return result;
        }

        public override bool IsProperFriend(TBaseObject BaseObject)
        {
            return base.IsProperFriend(BaseObject);
        }

        public override void SearchViewRange()
        {
            int nIdx;
            MapCellInfo cellInfo;
            CellObject osObject = null;
            TBaseObject baseObject;
            MirEvent MapEvent;
            VisibleFlag nVisibleFlag;
            const string sExceptionMsg = "TAIPlayObject::SearchViewRange 1-{0} {1} {2} {3} {4}";
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
                var nStartX = CurrX - ViewRange;
                var nEndX = CurrX + ViewRange;
                var nStartY = CurrY - ViewRange;
                var nEndY = CurrY + ViewRange;
                var dwRunTick = HUtil32.GetTickCount();
                for (var n18 = nStartX; n18 <= nEndX; n18++)
                {
                    for (var n1C = nStartY; n1C <= nEndY; n1C++)
                    {
                        var cellsuccess = false;
                        cellInfo = Envir.GetCellInfo(n18, n1C, ref cellsuccess);
                        if (cellsuccess && cellInfo.ObjList != null)
                        {
                            nIdx = 0;
                            while (cellInfo.Count>0)
                            {
                                if (HUtil32.GetTickCount() - dwRunTick > 500)
                                {
                                    break;
                                }
                                if (cellInfo.ObjList != null && cellInfo.Count <= 0)
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
                                    osObject = cellInfo.ObjList[nIdx];
                                }
                                catch
                                {
                                    cellInfo.Remove(osObject);
                                    continue;
                                }
                                if (osObject != null)
                                {
                                    if (!osObject.ObjectDispose)
                                    {
                                        switch (osObject.CellType)
                                        {
                                            case CellType.MovingObject:
                                                if (HUtil32.GetTickCount() - osObject.AddTime >= 60000)
                                                {
                                                    osObject.ObjectDispose = true;
                                                    cellInfo.Remove(osObject);
                                                    if (cellInfo.Count <= 0)
                                                    {
                                                        cellInfo.Dispose();
                                                        break;
                                                    }
                                                    continue;
                                                }
                                                baseObject = M2Share.ActorMgr.Get(osObject.CellObjId);;
                                                if (baseObject != null)
                                                {
                                                    if (!baseObject.Ghost && !baseObject.FixedHideMode && !baseObject.ObMode)
                                                    {
                                                        if (Race < Grobal2.RC_ANIMAL || Master != null || CrazyMode || WantRefMsg || baseObject.Master != null && Math.Abs(baseObject.CurrX - CurrX) <= 3 && Math.Abs(baseObject.CurrY - CurrY) <= 3 || baseObject.Race == Grobal2.RC_PLAYOBJECT)
                                                        {
                                                            UpdateVisibleGay(baseObject);
                                                        }
                                                    }
                                                }
                                                break;
                                            case CellType.ItemObject:
                                                if (Race == Grobal2.RC_PLAYOBJECT)
                                                {
                                                    if (HUtil32.GetTickCount() - osObject.AddTime > M2Share.g_Config.dwClearDropOnFloorItemTime)
                                                    {
                                                        if (osObject.CellObjId > 0)
                                                        {
                                                            M2Share.CellObjectSystem.Dispose(osObject.CellObjId);
                                                        }
                                                        if (osObject != null)
                                                        {
                                                            osObject.ObjectDispose = true;
                                                        }
                                                        cellInfo.Remove(osObject);
                                                        if (cellInfo.Count <= 0)
                                                        {
                                                            cellInfo.Dispose();
                                                            break;
                                                        }
                                                        continue;
                                                    }
                                                    var MapItem = (MapItem)M2Share.CellObjectSystem.Get(osObject.CellObjId);
                                                    UpdateVisibleItem(n18, n1C, MapItem);
                                                    if (MapItem.OfBaseObject != 0 || MapItem.DropBaseObject != 0)
                                                    {
                                                        if (HUtil32.GetTickCount() - MapItem.CanPickUpTick > M2Share.g_Config.dwFloorItemCanPickUpTime)
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
                                                            if (MapItem.DropBaseObject >0)
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
                                            case CellType.EventObject:
                                                if (Race == Grobal2.RC_PLAYOBJECT)
                                                {
                                                    if (osObject.CellObjId < 0)
                                                    {
                                                        MapEvent = (MirEvent)M2Share.CellObjectSystem.Get(osObject.CellObjId);
                                                        UpdateVisibleEvent(n18, n1C, MapEvent);
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
                M2Share.LogSystem.Error(format(sExceptionMsg, new object[] { CharName, MapName, CurrX, CurrY }));
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
                                if (Race == Grobal2.RC_PLAYOBJECT)
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
                                if (Race == Grobal2.RC_PLAYOBJECT)
                                {
                                    baseObject = VisibleBaseObject.BaseObject;
                                    if (baseObject != null)
                                    {
                                        if (baseObject != this && !baseObject.Ghost && !Ghost)
                                        {
                                            if (baseObject.Death)
                                            {
                                                if (baseObject.m_boSkeleton)
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
                M2Share.LogSystem.Error(format(sExceptionMsg, new object[] { CharName, MapName, CurrX, CurrY }));
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
                                    SendMsg(this, Grobal2.RM_HIDEEVENT, 0, MapEvent.Id, MapEvent.m_nX, MapEvent.m_nY, "");
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
                                    SendMsg(this, Grobal2.RM_SHOWEVENT, MapEvent.EventType, MapEvent.Id, HUtil32.MakeLong(MapEvent.m_nX, MapEvent.m_nEventParam), MapEvent.m_nY, "");
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
                M2Share.LogSystem.Error(CharName + ',' + MapName + ',' + CurrX + ',' + CurrY + ',' + " SearchViewRange");
                KickException();
            }
        }

        public override void Struck(TBaseObject hiter)
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
                    if (hiter.Race == Grobal2.RC_PLAYOBJECT || hiter.Master != null && hiter.GetMaster().Race == Grobal2.RC_PLAYOBJECT)
                    {
                        if (TargetCret != null && (TargetCret.Race == Grobal2.RC_PLAYOBJECT || TargetCret.Master != null && TargetCret.GetMaster().Race == Grobal2.RC_PLAYOBJECT))
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
                if (hiter.Race == Grobal2.RC_PLAYOBJECT && !hiter.IsRobot && TargetCret == hiter)
                {
                    if (M2Share.RandomNumber.Random(8) == 0 && m_AISayMsgList.Count > 0)
                    {
                        if (HUtil32.GetTickCount() >= m_dwDisableSayMsgTick)
                        {
                            m_boDisableSayMsg = false;
                        }
                        var boDisableSayMsg = m_boDisableSayMsg;
                        //g_DenySayMsgList.Lock;
                        //if (g_DenySayMsgList.GetIndex(m_sCharName) >= 0)
                        //{
                        //    boDisableSayMsg = true;
                        //}
                        //g_DenySayMsgList.UnLock;
                        if (!boDisableSayMsg)
                        {
                            SendRefMsg(Grobal2.RM_HEAR, 0, M2Share.g_Config.btHearMsgFColor, M2Share.g_Config.btHearMsgBColor, 0, CharName + ':' + m_AISayMsgList[M2Share.RandomNumber.Random(m_AISayMsgList.Count)]);
                        }
                    }
                }
            }
            if (Animal)
            {
                m_nMeatQuality = (ushort)(m_nMeatQuality - M2Share.RandomNumber.Random(300));
                if (m_nMeatQuality < 0)
                {
                    m_nMeatQuality = 0;
                }
            }
            AttackTick = (ushort)(AttackTick + (150 - HUtil32._MIN(130, Abil.Level * 4)));
        }

        protected override void SearchTarget()
        {
            if ((TargetCret == null || HUtil32.GetTickCount() - m_dwSearchTargetTick > 1000) && m_boAIStart)
            {
                m_dwSearchTargetTick = HUtil32.GetTickCount();
                if (TargetCret == null || !(TargetCret != null && TargetCret.Race == Grobal2.RC_PLAYOBJECT) || TargetCret.Master != null && TargetCret.Master.Race == Grobal2.RC_PLAYOBJECT || (HUtil32.GetTickCount() - StruckTick) > 15000)
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