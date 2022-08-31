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
                if (!m_boGhost && !m_boDeath && !m_boFixedHideMode && !m_boStoneMode && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
                {
                    if (HUtil32.GetTickCount() - m_dwWalkTick > m_nWalkSpeed)
                    {
                        m_dwWalkTick = HUtil32.GetTickCount();
                        if (m_TargetCret != null)
                        {
                            if (m_TargetCret.m_boDeath || m_TargetCret.m_boGhost || m_TargetCret.InSafeZone() || m_TargetCret.m_PEnvir != m_PEnvir || Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) > 11 || Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) > 11)
                            {
                                DelTargetCreat();
                            }
                        }
                        if (!m_boAIStart)
                        {
                            DelTargetCreat();
                        }
                        SearchTarget();
                        if (m_ManagedEnvir != m_PEnvir) // 所在地图不是挂机地图则清空目标
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
                                m_nProtectTargetX = m_nCurrX;// 守护坐标
                                m_nProtectTargetY = m_nCurrY;// 守护坐标
                            }
                            if (!m_boProtectOK && m_ManagedEnvir != null && m_TargetCret == null)
                            {
                                GotoProtect();
                                m_nGotoProtectXYCount++;
                                if (Math.Abs(m_nCurrX - m_nProtectTargetX) <= 3 && Math.Abs(m_nCurrY - m_nProtectTargetY) <= 3)
                                {
                                    Direction = (byte)M2Share.RandomNumber.Random(8);
                                    m_boProtectOK = true;
                                    m_nGotoProtectXYCount = 0;// 是向守护坐标的累计数
                                }
                                if (m_nGotoProtectXYCount > 20 && !m_boProtectOK)// 20次还没有走到守护坐标，则飞回坐标上
                                {
                                    if (Math.Abs(m_nCurrX - m_nProtectTargetX) > 13 || Math.Abs(m_nCurrY - m_nProtectTargetY) > 13)
                                    {
                                        SpaceMove(m_ManagedEnvir.SMapName, m_nProtectTargetX, m_nProtectTargetY, 1);
                                        Direction = (byte)M2Share.RandomNumber.Random(8);
                                        m_boProtectOK = true;
                                        m_nGotoProtectXYCount = 0;// 是向守护坐标的累计数
                                    }
                                }
                                base.Run();
                                return;
                            }
                        }
                        if (m_TargetCret != null)
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
                                    m_nTargetX = m_TargetCret.m_nCurrX;
                                    m_nTargetY = m_TargetCret.m_nCurrY;
                                    if (AllowUseMagic(12) && m_btJob == 0)
                                    {
                                        GetGotoXY(m_TargetCret, 2);
                                    }
                                    if (m_btJob > 0)
                                    {
                                        if (M2Share.g_Config.boHeroAttackTarget && m_Abil.Level < 22 || M2Share.g_Config.boHeroAttackTao && m_TargetCret.m_WAbil.MaxHP < 700 && m_btJob == PlayJob.Taoist && m_TargetCret.m_btRaceServer != Grobal2.RC_PLAYOBJECT)
                                        {
                                            // 道法22前是否物理攻击
                                            if (m_Master != null)
                                            {
                                                if (Math.Abs(m_Master.m_nCurrX - m_nCurrX) > 6 || Math.Abs(m_Master.m_nCurrY - m_nCurrY) > 6)
                                                {
                                                    base.Run();
                                                    return;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            GetGotoXY(m_TargetCret, 3); // 道法只走向目标3格范围
                                        }
                                    }
                                    GotoTargetXY(m_nTargetX, m_nTargetY, 0);
                                    base.Run();
                                    return;
                                }
                            }
                        }

                        if (m_boAI && !m_boGhost && !m_boDeath)
                        {
                            if (M2Share.g_Config.boHPAutoMoveMap)
                            {
                                if (m_WAbil.HP <= Math.Round(m_WAbil.MaxHP * 0.3) && HUtil32.GetTickCount() - m_dwHPToMapHomeTick > 15000) // 低血时回城或回守护点 
                                {
                                    m_dwHPToMapHomeTick = HUtil32.GetTickCount();
                                    DelTargetCreat();
                                    if (m_boProtectStatus) // 守护状态
                                    {
                                        SpaceMove(m_ManagedEnvir.SMapName, m_nProtectTargetX, m_nProtectTargetY, 1);// 地图移动
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
                                        if (m_UseItems[nWhere].wIndex <= 0)
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
                                                m_UseItems[nWhere] = UserItem;
                                                Dispose(UserItem);
                                            }
                                        }
                                    }
                                    if (m_BagItemNames.Count > 0)
                                    {
                                        for (var i = 0; i < m_BagItemNames.Count; i++)
                                        {
                                            for (var j = 0; j < m_ItemList.Count; j++)
                                            {
                                                UserItem = m_ItemList[j];
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
                                    for (nWhere = 0; nWhere <= m_UseItems.Length; nWhere++)
                                    {
                                        if (m_UseItems[nWhere] != null && m_UseItems[nWhere].wIndex > 0)
                                        {
                                            StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[nWhere].wIndex);
                                            if (StdItem != null)
                                            {
                                                if (m_UseItems[nWhere].DuraMax > m_UseItems[nWhere].Dura && StdItem.StdMode != 43)
                                                {
                                                    /*if (PlugOfCheckCanItem(3, StdItem.Name, false, 0, 0))
                                                    {
                                                        continue;
                                                    }*/
                                                    m_UseItems[nWhere].Dura = m_UseItems[nWhere].DuraMax;
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

                    if (!m_boGhost && !m_boDeath && !m_boFixedHideMode && !m_boStoneMode && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
                    {
                        if (m_boProtectStatus && m_TargetCret == null)// 守护状态
                        {
                            if (Math.Abs(m_nCurrX - m_nProtectTargetX) > 50 || Math.Abs(m_nCurrY - m_nProtectTargetY) > 50)
                            {
                                m_boProtectOK = false;
                            }
                        }
                        if (m_TargetCret == null)
                        {
                            if (m_Master != null)
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
                    if (BaseObject.m_Master != null)
                    {
                        if (BaseObject.m_Master == this || BaseObject.m_Master.m_boAI && !m_boInFreePKArea)
                        {
                            result = false;
                        }
                    }
                    if (BaseObject.m_boAI && !m_boInFreePKArea)// 假人不攻击假人,行会战除外
                    {
                        result = false;
                    }
                    switch (BaseObject.m_btRaceServer)
                    {
                        case Grobal2.RC_ARCHERGUARD:
                        case 55:// 不主动攻击练功师 弓箭手
                            if (BaseObject.m_TargetCret != this)
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
                    if (m_btAttatckMode == AttackMode.HAM_PKATTACK)// 红名模式，除红名目标外，受人攻击时才还击
                    {
                        if (BaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
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
                        if (m_boAI && !result)
                        {
                            if (BaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT || BaseObject.m_Master != null)
                            {
                                if (BaseObject.m_TargetCret != null)
                                {
                                    if (BaseObject.m_TargetCret == this)
                                    {
                                        result = true;
                                    }
                                }
                                if (BaseObject.m_LastHiter != null)
                                {
                                    if (BaseObject.m_LastHiter == this)
                                    {
                                        result = true;
                                    }
                                }
                                if (BaseObject.m_ExpHitter != null)
                                {
                                    if (BaseObject.m_LastHiter == this)
                                    {
                                        result = true;
                                    }
                                }
                            }
                        }
                        if (BaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT || BaseObject.m_Master != null)// 安全区不能打人物和英雄
                        {
                            if (BaseObject.InSafeZone() || InSafeZone())
                            {
                                result = false;
                            }
                        }
                        if (BaseObject.m_Master == this)
                        {
                            result = false;
                        }
                        if (BaseObject.m_boAI && (!m_boInFreePKArea || BaseObject.PKLevel() < 2))// 假人不攻击假人,行会战除外
                        {
                            result = false;
                        }
                        switch (BaseObject.m_btRaceServer)
                        {
                            case Grobal2.RC_ARCHERGUARD:
                            case 55:// 不主动攻击练功师 弓箭手
                                if (BaseObject.m_TargetCret != this)
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
            int nStartX;
            int nEndX;
            int nStartY;
            int nEndY;
            int n18;
            int n1C;
            int nIdx;
            MapCellinfo MapCellInfo;
            CellObject OSObject = null;
            TBaseObject BaseObject;
            MapItem MapItem;
            MirEvent MapEvent;
            TVisibleBaseObject VisibleBaseObject;
            VisibleMapItem VisibleMapItem;
            int nVisibleFlag;
            const string sExceptionMsg1 = "TAIPlayObject::SearchViewRange Code:{0}";
            const string sExceptionMsg2 = "TAIPlayObject::SearchViewRange 1-{0} {1} {2} {3} {4}";
            try
            {
                if (m_boGhost)
                {
                    return;
                }
                if (m_VisibleItems.Count > 0)
                {
                    for (var i = 0; i < m_VisibleItems.Count; i++)
                    {
                        m_VisibleItems[i].nVisibleFlag = 0;
                    }
                }
            }
            catch
            {
                M2Share.MainOutMessage(sExceptionMsg1);
                KickException();
            }
            try
            {
                nStartX = m_nCurrX - m_nViewRange;
                nEndX = m_nCurrX + m_nViewRange;
                nStartY = m_nCurrY - m_nViewRange;
                nEndY = m_nCurrY + m_nViewRange;
                var dwRunTick = HUtil32.GetTickCount();
                for (n18 = nStartX; n18 <= nEndX; n18++)
                {
                    for (n1C = nStartY; n1C <= nEndY; n1C++)
                    {
                        var mapCell = false;
                        MapCellInfo = m_PEnvir.GetMapCellInfo(n18, n1C, ref mapCell);
                        if (mapCell && MapCellInfo.ObjList != null)
                        {
                            nIdx = 0;
                            while (MapCellInfo.Count>0)
                            {
                                if (HUtil32.GetTickCount() - dwRunTick > 500)
                                {
                                    break;
                                }
                                if (MapCellInfo.ObjList != null && MapCellInfo.Count <= 0)
                                {
                                    MapCellInfo.Dispose();
                                    break;
                                }
                                if (MapCellInfo.ObjList == null)
                                {
                                    break;
                                }
                                if (MapCellInfo.Count <= nIdx)
                                {
                                    break;
                                }
                                try
                                {
                                    OSObject = MapCellInfo.ObjList[nIdx];
                                }
                                catch
                                {
                                    MapCellInfo.Remove(OSObject);
                                    continue;
                                }
                                if (OSObject != null)
                                {
                                    if (!OSObject.boObjectDisPose)
                                    {
                                        switch (OSObject.CellType)
                                        {
                                            case CellType.OS_MOVINGOBJECT:
                                                if (HUtil32.GetTickCount() - OSObject.dwAddTime >= 60000)
                                                {
                                                    OSObject.boObjectDisPose = true;
                                                    MapCellInfo.Remove(OSObject);
                                                    if (MapCellInfo.Count <= 0)
                                                    {
                                                        MapCellInfo.Dispose();
                                                        break;
                                                    }
                                                    continue;
                                                }
                                                BaseObject = (TBaseObject)M2Share.ObjectManager.Get(OSObject.CellObjId);;
                                                if (BaseObject != null)
                                                {
                                                    if (!BaseObject.m_boGhost && !BaseObject.m_boFixedHideMode && !BaseObject.m_boObMode)
                                                    {
                                                        if (m_btRaceServer < Grobal2.RC_ANIMAL || m_Master != null || m_boCrazyMode || m_boWantRefMsg || BaseObject.m_Master != null && Math.Abs(BaseObject.m_nCurrX - m_nCurrX) <= 3 && Math.Abs(BaseObject.m_nCurrY - m_nCurrY) <= 3 || BaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                                                        {
                                                            UpdateVisibleGay(BaseObject);
                                                        }
                                                    }
                                                }
                                                break;
                                            case CellType.OS_ITEMOBJECT:
                                                if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                                                {
                                                    if (HUtil32.GetTickCount() - OSObject.dwAddTime > M2Share.g_Config.dwClearDropOnFloorItemTime)
                                                    {
                                                        if (OSObject.CellObjId > 0)
                                                        {
                                                            M2Share.CellObjectSystem.Dispose(OSObject.CellObjId);
                                                        }
                                                        if (OSObject != null)
                                                        {
                                                            OSObject.boObjectDisPose = true;
                                                        }
                                                        MapCellInfo.Remove(OSObject);
                                                        if (MapCellInfo.Count <= 0)
                                                        {
                                                            MapCellInfo.Dispose();
                                                            break;
                                                        }
                                                        continue;
                                                    }
                                                    MapItem = (MapItem)M2Share.CellObjectSystem.Get(OSObject.CellObjId);
                                                    UpdateVisibleItem(n18, n1C, MapItem);
                                                    if (MapItem.OfBaseObject != null || MapItem.DropBaseObject != null)
                                                    {
                                                        if (HUtil32.GetTickCount() - MapItem.CanPickUpTick > M2Share.g_Config.dwFloorItemCanPickUpTime)
                                                        {
                                                            MapItem.OfBaseObject = null;
                                                            MapItem.DropBaseObject = null;
                                                        }
                                                        else
                                                        {
                                                            if (MapItem.OfBaseObject != null)
                                                            {
                                                                if (((TBaseObject)MapItem.OfBaseObject).m_boGhost)
                                                                {
                                                                    MapItem.OfBaseObject = null;
                                                                }
                                                            }
                                                            if (MapItem.DropBaseObject != null)
                                                            {
                                                                if (((TBaseObject)MapItem.DropBaseObject).m_boGhost)
                                                                {
                                                                    MapItem.DropBaseObject = null;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                break;
                                            case CellType.OS_EVENTOBJECT:
                                                if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                                                {
                                                    if (OSObject.CellObjId < 0)
                                                    {
                                                        MapEvent = (MirEvent)M2Share.CellObjectSystem.Get(OSObject.CellObjId);
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
                M2Share.MainOutMessage(format(sExceptionMsg2, new object[] { m_sCharName, m_sMapName, m_nCurrX, m_nCurrY }));
                KickException();
            }
            try
            {
                n18 = 0;
                while (true)
                {
                    try
                    {
                        if (m_VisibleActors.Count <= n18)
                        {
                            break;
                        }
                        try
                        {
                            VisibleBaseObject = m_VisibleActors[n18];
                            nVisibleFlag = VisibleBaseObject.nVisibleFlag;
                        }
                        catch
                        {
                            m_VisibleActors.RemoveAt(n18);
                            if (m_VisibleActors.Count > 0)
                            {
                                continue;
                            }
                            break;
                        }
                        switch (VisibleBaseObject.nVisibleFlag)
                        {
                            case 0:
                                if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                                {
                                    BaseObject = VisibleBaseObject.BaseObject;
                                    if (BaseObject != null)
                                    {
                                        if (!BaseObject.m_boFixedHideMode && !BaseObject.m_boGhost)
                                        {
                                            SendMsg(BaseObject, Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
                                        }
                                    }
                                }
                                m_VisibleActors.RemoveAt(n18);
                                if (VisibleBaseObject != null)
                                {
                                    Dispose(VisibleBaseObject);
                                }
                                continue;
                            case 2:
                                if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                                {
                                    BaseObject = VisibleBaseObject.BaseObject;
                                    if (BaseObject != null)
                                    {
                                        if (BaseObject != this && !BaseObject.m_boGhost && !m_boGhost)
                                        {
                                            if (BaseObject.m_boDeath)
                                            {
                                                if (BaseObject.m_boSkeleton)
                                                {
                                                    SendMsg(BaseObject, Grobal2.RM_SKELETON, BaseObject.Direction, BaseObject.m_nCurrX, BaseObject.m_nCurrY, 0, "");
                                                }
                                                else
                                                {
                                                    SendMsg(BaseObject, Grobal2.RM_DEATH, BaseObject.Direction, BaseObject.m_nCurrX, BaseObject.m_nCurrY, 0, "");
                                                }
                                            }
                                            else
                                            {
                                                if (BaseObject != null)
                                                {
                                                    SendMsg(BaseObject, Grobal2.RM_TURN, BaseObject.Direction, BaseObject.m_nCurrX, BaseObject.m_nCurrY, 0, BaseObject.GetShowName());
                                                }
                                            }
                                        }
                                    }
                                }
                                VisibleBaseObject.nVisibleFlag = 0;
                                break;
                            case 1:
                                VisibleBaseObject.nVisibleFlag = 0;
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
                M2Share.MainOutMessage(format(sExceptionMsg2, new object[] { m_sCharName, m_sMapName, m_nCurrX, m_nCurrY }));
                KickException();
            }
            try
            {
                var position = 0;
                while (true)
                {
                    try
                    {
                        if (m_VisibleItems.Count <= position)
                        {
                            break;
                        }
                        try
                        {
                            VisibleMapItem = m_VisibleItems[position];
                            nVisibleFlag = VisibleMapItem.nVisibleFlag;
                        }
                        catch
                        {
                            m_VisibleItems.RemoveAt(position);
                            if (m_VisibleItems.Count > 0)
                            {
                                continue;
                            }
                            break;
                        }
                        if (VisibleMapItem.nVisibleFlag == 0)
                        {
                            m_VisibleItems.RemoveAt(position);
                            VisibleMapItem = null;
                            if (m_VisibleItems.Count > 0)
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
                        if (m_VisibleEvents.Count <= position)
                        {
                            break;
                        }
                        try
                        {
                            MapEvent = m_VisibleEvents[position];
                            nVisibleFlag = MapEvent.VisibleFlag;
                        }
                        catch
                        {
                            m_VisibleEvents.RemoveAt(position);
                            if (m_VisibleEvents.Count > 0)
                            {
                                continue;
                            }
                            break;
                        }
                        if (MapEvent != null)
                        {
                            switch (MapEvent.VisibleFlag)
                            {
                                case 0:
                                    SendMsg(this, Grobal2.RM_HIDEEVENT, 0, MapEvent.Id, MapEvent.m_nX, MapEvent.m_nY, "");
                                    m_VisibleEvents.RemoveAt(position);
                                    if (m_VisibleEvents.Count > 0)
                                    {
                                        continue;
                                    }
                                    break;
                                case 1:
                                    MapEvent.VisibleFlag = 0;
                                    break;
                                case 2:
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
                M2Share.MainOutMessage(m_sCharName + ',' + m_sMapName + ',' + m_nCurrX.ToString() + ',' + m_nCurrY.ToString() + ',' + " SearchViewRange 3");
                KickException();
            }
        }

        public override void Struck(TBaseObject hiter)
        {
            m_dwStruckTick = HUtil32.GetTickCount();
            if (hiter != null)
            {
                if (m_TargetCret == null && IsProperTarget(hiter))
                {
                    SetTargetCreat(hiter);
                }
                else
                {
                    if (hiter.m_btRaceServer == Grobal2.RC_PLAYOBJECT || hiter.m_Master != null && hiter.GetMaster().m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                    {
                        if (m_TargetCret != null && (m_TargetCret.m_btRaceServer == Grobal2.RC_PLAYOBJECT || m_TargetCret.m_Master != null && m_TargetCret.GetMaster().m_btRaceServer == Grobal2.RC_PLAYOBJECT))
                        {
                            if (Struck_MINXY(m_TargetCret, hiter) == hiter || M2Share.RandomNumber.Random(6) == 0)
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
                        if (m_TargetCret != null && Struck_MINXY(m_TargetCret, hiter) == hiter || M2Share.RandomNumber.Random(6) == 0)
                        {
                            if (m_btJob > 0 || m_TargetCret != null && (HUtil32.GetTickCount() - m_dwTargetFocusTick) > 1000 * 3)
                            {
                                if (IsProperTarget(hiter))
                                {
                                    SetTargetCreat(hiter);
                                }
                            }
                        }
                    }
                }
                if (hiter.m_btRaceServer == Grobal2.RC_PLAYOBJECT && !hiter.m_boAI && m_TargetCret == hiter)
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
                            SendRefMsg(Grobal2.RM_HEAR, 0, M2Share.g_Config.btHearMsgFColor, M2Share.g_Config.btHearMsgBColor, 0, m_sCharName + ':' + m_AISayMsgList[M2Share.RandomNumber.Random(m_AISayMsgList.Count)]);
                        }
                    }
                }
            }
            if (m_boAnimal)
            {
                m_nMeatQuality = (ushort)(m_nMeatQuality - M2Share.RandomNumber.Random(300));
                if (m_nMeatQuality < 0)
                {
                    m_nMeatQuality = 0;
                }
            }
            m_dwHitTick = (ushort)(m_dwHitTick + (150 - HUtil32._MIN(130, m_Abil.Level * 4)));
        }

        protected override void SearchTarget()
        {
            if ((m_TargetCret == null || HUtil32.GetTickCount() - m_dwSearchTargetTick > 1000) && m_boAIStart)
            {
                m_dwSearchTargetTick = HUtil32.GetTickCount();
                if (m_TargetCret == null || !(m_TargetCret != null && m_TargetCret.m_btRaceServer == Grobal2.RC_PLAYOBJECT) || m_TargetCret.m_Master != null && m_TargetCret.m_Master.m_btRaceServer == Grobal2.RC_PLAYOBJECT || (HUtil32.GetTickCount() - m_dwStruckTick) > 15000)
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