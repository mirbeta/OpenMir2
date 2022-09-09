using BotSvr.Maps;
using BotSvr.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using SystemModule;
using SystemModule.Packet.ClientPackets;

namespace BotSvr.Scenes.Scene
{
    public class PlayScene : SceneBase
    {
        private int m_dwMoveTime = 0;
        private readonly int _mNDefXx = 0;
        private readonly int _mNDefYy = 0;
        public IList<TActor> m_ActorList = null;
        public ProcMagic ProcMagic = null;

        public PlayScene(RobotClient robotClient) : base(SceneType.stPlayGame, robotClient)
        {
            ProcMagic = new ProcMagic();
            ProcMagic.NTargetX = -1;
            m_ActorList = new List<TActor>();
            m_dwMoveTime = HUtil32.GetTickCount();
        }

        public override void OpenScene()
        {
            robotClient.SocketEvents();
        }

        public override void CloseScene()
        {

        }

        public override void OpeningScene()
        {

        }

        public void CleanObjects()
        {
            //int i;
            //for (i = MActorList.Count - 1; i >= 0; i--)
            //{
            //    // (TActor(m_ActorList[i]) <> g_MySelf.m_SlaveObject)
            //    if ((((TActor)(MActorList[i])) != MShare.g_MySelf) && (((TActor)(MActorList[i])) != MShare.g_MySelf.m_HeroObject) && !PlayScn.IsMySlaveObject(((TActor)(MActorList[i]))))
            //    {
            //        // BLUE
            //        ((TActor)(MActorList[i])).Free;
            //        MActorList.RemoveAt(i);
            //    }
            //}
            //MShare.g_TargetCret = null;
            //MShare.g_FocusCret = null;
            //MShare.g_MagicTarget = null;
            //for (i = 0; i < MEffectList.Count; i++)
            //{
            //    ((TMagicEff)(MEffectList[i])).Free;
            //}
            //MEffectList.Clear();
        }

        private void ClearDropItemA()
        {
            TDropItem dropItem;
            for (var i = MShare.g_DropedItemList.Count - 1; i >= 0; i--)
            {
                dropItem = MShare.g_DropedItemList[i];
                if (dropItem == null)
                {
                    MShare.g_DropedItemList.RemoveAt(i);
                    // Continue;
                    break;
                }
                if ((Math.Abs(dropItem.X - MShare.g_MySelf.m_nCurrX) > 20) || (Math.Abs(dropItem.Y - MShare.g_MySelf.m_nCurrY) > 20))
                {
                    dropItem = null;
                    MShare.g_DropedItemList.RemoveAt(i);
                    break;
                }
            }
        }

        public void BeginScene()
        {
            if (MShare.g_MySelf == null)
            {
                return;
            }
            var movetick = false;
            var tick = HUtil32.GetTickCount();
            if (MShare.g_boSpeedRate)
            {
                if (tick - m_dwMoveTime >= (95 - MShare.g_MoveSpeedRate / 2))
                {
                    m_dwMoveTime = tick;
                    movetick = true;
                }
            }
            else
            {
                if (tick - m_dwMoveTime >= 95)
                {
                    m_dwMoveTime = tick;
                    movetick = true;
                }
            }
            var i = 0;
            while (true)
            {
                if (i >= m_ActorList.Count)
                {
                    break;
                }
                var actor = m_ActorList[i];
                if (actor.m_boDeath && MShare.g_gcGeneral[8] && !actor.m_boItemExplore && (actor.m_btRace != 0) && actor.IsIdle())
                {
                    i++;
                    continue;
                }
                if (movetick)
                {
                    actor.m_boLockEndFrame = false;
                }
                if (!actor.m_boLockEndFrame)
                {
                    actor.ProcMsg();   //处理角色的消息
                    if (movetick)
                    {
                        if (actor.Move())
                        {
                            i++;
                            continue;
                        }
                    }
                    actor.Run();
                    if (actor != MShare.g_MySelf)
                    {
                        actor.ProcHurryMsg();
                    }
                }
                if (actor == MShare.g_MySelf)
                {
                    actor.ProcHurryMsg();
                }
                if (actor.m_nWaitForRecogId != 0)
                {
                    if (actor.IsIdle())
                    {
                        ClFunc.DelChangeFace(actor.m_nWaitForRecogId);
                        NewActor(actor.m_nWaitForRecogId, actor.m_nCurrX, actor.m_nCurrY, actor.m_btDir, actor.m_nWaitForFeature, actor.m_nWaitForStatus);
                        actor.m_nWaitForRecogId = 0;
                        actor.m_boDelActor = true;
                    }
                }
                if (actor.m_boDelActor || (Math.Abs(MShare.g_MySelf.m_nCurrX - actor.m_nCurrX) > 16) || (Math.Abs(MShare.g_MySelf.m_nCurrY - actor.m_nCurrY) > 16))
                {
                    MShare.g_FreeActorList.Add(actor);
                    m_ActorList.RemoveAt(i);
                    if (MShare.g_TargetCret == actor)
                    {
                        MShare.g_TargetCret = null;
                    }
                    if (MShare.g_FocusCret == actor)
                    {
                        MShare.g_FocusCret = null;
                    }
                    if (MShare.g_MagicLockActor == actor)
                    {
                        MShare.g_MagicLockActor = null;
                    }
                    if (MShare.g_MagicTarget == actor)
                    {
                        MShare.g_MagicTarget = null;
                    }
                }
                else
                {
                    i++;
                }
            }
            robotClient.Map.UpdateMapPos(MShare.g_MySelf.m_nRx, MShare.g_MySelf.m_nRy);
        }

        public void PlaySurface(object sender)
        {

        }

        public void MagicSurface(object sender)
        {

        }

        public void NewMagic(TActor aowner, int magid, int magnumb, int cx, int cy, int tx, int ty, int targetCode, MagicType mtype, bool recusion, int anitime, ref bool boFly, int maglv, int poison)
        {

        }

        public void DelMagic(int magid)
        {
            //for (var i = 0; i < MEffectList.Count; i++)
            //{
            //    if (((TMagicEff)MEffectList[i]).ServerMagicId == magid)
            //    {
            //        ((TMagicEff)MEffectList[i]).Free;
            //        MEffectList.RemoveAt(i);
            //        break;
            //    }
            //}
        }

        public TActor GetCharacter(int x, int y, int wantsel, ref int nowsel, bool liveonly)
        {
            int ccy = 0;
            int dx = 0;
            int dy = 0;
            TActor result = null;
            nowsel = -1;
            for (var k = ccy + 8; k >= ccy - 1; k--)
            {
                for (var i = m_ActorList.Count - 1; i >= 0; i--)
                {
                    if (m_ActorList[i] != MShare.g_MySelf)
                    {
                        TActor a = m_ActorList[i];
                        if ((!liveonly || !a.m_boDeath) && a.m_boHoldPlace && a.m_boVisible)
                        {
                            if (a.m_nCurrY == k)
                            {
                                dx = (a.m_nRx - robotClient.Map.m_ClientRect.Left) * Grobal2.UNITX + _mNDefXx + a.m_nPx + a.m_nShiftX;
                                dy = (a.m_nRy - robotClient.Map.m_ClientRect.Top - 1) * Grobal2.UNITY + _mNDefYy + a.m_nPy + a.m_nShiftY;
                                if (a.CheckSelect(x - dx, y - dy))
                                {
                                    result = a;
                                    nowsel++;
                                    if (nowsel >= wantsel)
                                    {
                                        return result;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        // 取得鼠标所指坐标的角色
        public TActor GetAttackFocusCharacter(int x, int y, int wantsel, ref int nowsel, bool liveonly)
        {
            int ccy = 0;
            int dx = 0;
            int dy = 0;
            int centx = 0;
            int centy = 0;
            TActor a;
            TActor result = GetCharacter(x, y, wantsel, ref nowsel, liveonly);
            if (result == null)
            {
                nowsel = -1;
                for (var k = ccy + 8; k >= ccy - 1; k--)
                {
                    for (var i = m_ActorList.Count - 1; i >= 0; i--)
                    {
                        if (m_ActorList[i] != MShare.g_MySelf)
                        {
                            a = m_ActorList[i];
                            if ((!liveonly || !a.m_boDeath) && a.m_boHoldPlace && a.m_boVisible)
                            {
                                if (a.m_nCurrY == k)
                                {
                                    dx = (a.m_nRx - robotClient.Map.m_ClientRect.Left) * Grobal2.UNITX + _mNDefXx + a.m_nPx + a.m_nShiftX;
                                    dy = (a.m_nRy - robotClient.Map.m_ClientRect.Top - 1) * Grobal2.UNITY + _mNDefYy + a.m_nPy + a.m_nShiftY;
                                    if (a.CharWidth() > 40)
                                    {
                                        centx = (a.CharWidth() - 40) / 2;
                                    }
                                    else
                                    {
                                        centx = 0;
                                    }
                                    if (a.CharHeight() > 70)
                                    {
                                        centy = (a.CharHeight() - 70) / 2;
                                    }
                                    else
                                    {
                                        centy = 0;
                                    }
                                    if ((x - dx >= centx) && (x - dx <= a.CharWidth() - centx) && (y - dy >= centy) && (y - dy <= a.CharHeight() - centy))
                                    {
                                        result = a;
                                        nowsel++;
                                        if (nowsel >= wantsel)
                                        {
                                            return result;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public bool IsSelectMyself(int x, int y)
        {
            int ccy = 0;
            bool result = false;
            for (var k = ccy + 2; k >= ccy - 1; k--)
            {
                if (MShare.g_MySelf.m_nCurrY == k)
                {
                    int dx = (MShare.g_MySelf.m_nRx - robotClient.Map.m_ClientRect.Left) * Grobal2.UNITX + _mNDefXx + MShare.g_MySelf.m_nPx + MShare.g_MySelf.m_nShiftX;
                    int dy = (MShare.g_MySelf.m_nRy - robotClient.Map.m_ClientRect.Top - 1) * Grobal2.UNITY + _mNDefYy + MShare.g_MySelf.m_nPy + MShare.g_MySelf.m_nShiftY;
                    if (MShare.g_MySelf.CheckSelect(x - dx, y - dy))
                    {
                        result = true;
                        return result;
                    }
                }
            }
            return result;
        }

        public TDropItem GetDropItems(int x, int y, ref string inames)
        {
            int ccx = 0;
            int ccy = 0;
            TDropItem result = null;
            inames = "";
            for (var i = 0; i < MShare.g_DropedItemList.Count; i++)
            {
                TDropItem dropItem = MShare.g_DropedItemList[i];
                if ((dropItem.X == ccx) && (dropItem.Y == ccy))
                {
                    if (result == null)
                    {
                        result = dropItem;
                    }
                    inames = inames + dropItem.Name + "\\";
                }
            }
            return result;
        }

        public void GetXyDropItemsList(int nX, int nY, ref ArrayList itemList)
        {
            for (var i = 0; i < MShare.g_DropedItemList.Count; i++)
            {
                TDropItem dropItem = MShare.g_DropedItemList[i];
                if ((dropItem.X == nX) && (dropItem.Y == nY))
                {
                    itemList.Add(dropItem);
                }
            }
        }

        public TDropItem GetXyDropItems(int nX, int nY)
        {
            TDropItem result = null;
            for (var i = 0; i < MShare.g_DropedItemList.Count; i++)
            {
                TDropItem dropItem = MShare.g_DropedItemList[i];
                if ((dropItem.X == nX) && (dropItem.Y == nY))
                {
                    result = dropItem;
                    if (MShare.g_boPickUpAll || dropItem.boPickUp)
                    {
                        break;
                    }
                }
            }
            return result;
        }

        public bool CanRun(int sX, int sY, int ex, int ey)
        {
            bool result;
            byte ndir = ClFunc.GetNextDirection(sX, sY, ex, ey);
            int rx = sX;
            int ry = sY;
            ClFunc.GetNextPosXY(ndir, ref rx, ref ry);
            if (CanWalkEx(rx, ry) && CanWalkEx(ex, ey))
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public bool CanWalkEx(int mx, int my)
        {
            bool result = false;
            if (robotClient.Map.CanMove(mx, my))
            {
                result = !CrashManEx(mx, my);
            }
            return result;
        }

        private bool CrashManEx(int mx, int my)
        {
            bool result = false;
            for (var i = 0; i < m_ActorList.Count; i++)
            {
                TActor actor = m_ActorList[i];
                if (actor == MShare.g_MySelf)
                {
                    continue;
                }
                if (actor.m_boVisible && actor.m_boHoldPlace && (!actor.m_boDeath) && (actor.m_nCurrX == mx) && (actor.m_nCurrY == my))
                {
                    if ((MShare.g_MySelf.m_nTagX == 0) && (MShare.g_MySelf.m_nTagY == 0))
                    {
                        if ((actor.m_btRace == Grobal2.RCC_USERHUMAN) && (MShare.g_boCanRunHuman || MShare.g_boCanRunSafeZone))
                        {
                            continue;
                        }
                        if ((actor.m_btRace == Grobal2.RCC_MERCHANT) && MShare.g_boCanRunNpc)
                        {
                            continue;
                        }
                        if ((actor.m_btRace > Grobal2.RCC_USERHUMAN) && (actor.m_btRace != Grobal2.RCC_MERCHANT) && (MShare.g_boCanRunMon || MShare.g_boCanRunSafeZone))
                        {
                            continue;
                        }
                    }
                    result = true;
                    break;
                }
            }
            return result;
        }

        public bool CanWalk(int mx, int my)
        {
            bool result = false;
            if (robotClient.Map.CanMove(mx, my))
            {
                result = !CrashMan(mx, my);
            }
            return result;
        }

        public bool CrashMan(int mx, int my)
        {
            bool result = false;
            for (var i = 0; i < m_ActorList.Count; i++)
            {
                TActor actor = m_ActorList[i];
                if (actor == null || actor == MShare.g_MySelf)
                {
                    continue;
                }
                if (actor.m_boVisible && actor.m_boHoldPlace && (!actor.m_boDeath) && (actor.m_nCurrX == mx) && (actor.m_nCurrY == my))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public bool CanFly(int mx, int my)
        {
            return robotClient.Map.CanFly(mx, my);
        }

        public TActor FindActor(int id)
        {
            TActor result = null;
            if (id == 0)
            {
                return result;
            }
            for (var i = 0; i < m_ActorList.Count; i++)
            {
                if (m_ActorList[i].m_nRecogId == id)
                {
                    result = m_ActorList[i];
                    break;
                }
            }
            return result;
        }

        public TActor FindActor(string sName)
        {
            TActor result = null;
            for (var i = 0; i < m_ActorList.Count; i++)
            {
                TActor actor = m_ActorList[i];
                if (string.Compare(actor.m_sUserName, sName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = actor;
                    break;
                }
            }
            return result;
        }

        public TActor FindActorXY(int x, int y)
        {
            TActor result = null;
            for (var i = 0; i < m_ActorList.Count; i++)
            {
                TActor a = m_ActorList[i];
                if ((a.m_nCurrX == x) && (a.m_nCurrY == y))
                {
                    result = a;
                    if (!result.m_boDeath && result.m_boVisible && result.m_boHoldPlace)
                    {
                        break;
                    }
                }
            }
            return result;
        }

        public bool IsValidActor(TActor actor)
        {
            bool result = false;
            for (var i = 0; i < m_ActorList.Count; i++)
            {
                if (m_ActorList[i] == actor)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public byte RACEfeature(int cfeature)
        {
            return (byte)cfeature;
        }

        public TActor NewActor(int chrid, int cx, int cy, ushort cdir, int cfeature, int cstate)
        {
            TActor actor;
            TActor result = null;
            for (var i = 0; i < m_ActorList.Count; i++)
            {
                if (m_ActorList[i].m_nRecogId == chrid)
                {
                    result = m_ActorList[i];
                    return result;
                }
            }
            if (ClFunc.IsChangingFace(chrid))
            {
                return result;
            }
            switch (RACEfeature(cfeature))
            {
                case 0:
                    actor = new THumActor(robotClient);
                    break;
                case 9:
                    actor = new TSoccerBall(robotClient);
                    break;
                case 13:
                    actor = new TKillingHerb(robotClient);
                    break;
                case 14:
                    actor = new TSkeletonOma(robotClient);
                    break;
                case 15:
                    actor = new TDualAxeOma(robotClient);
                    break;
                case 16:
                    actor = new TGasKuDeGi(robotClient);
                    break;
                case 17:
                    actor = new TCatMon(robotClient);
                    break;
                case 18:
                    actor = new THuSuABi(robotClient);
                    break;
                case 19:
                    actor = new TCatMon(robotClient);
                    break;
                case 20:
                    actor = new TFireCowFaceMon(robotClient);
                    break;
                case 21:
                    actor = new TCowFaceKing(robotClient);
                    break;
                case 22:
                    actor = new TDualAxeOma(robotClient);
                    break;
                case 23:
                    actor = new TWhiteSkeleton(robotClient);
                    break;
                case 24:
                    actor = new TSuperiorGuard(robotClient);
                    break;
                case 25:
                    actor = new TKingOfSculpureKingMon(robotClient);
                    break;
                case 26:
                    actor = new TKingOfSculpureKingMon(robotClient);
                    break;
                case 27:
                    actor = new TSnowMon(robotClient);
                    break;
                case 28:
                    actor = new TSnowMon(robotClient);
                    break;
                case 29:
                    actor = new TSnowMon(robotClient);
                    break;
                case 30:
                    actor = new TCatMon(robotClient);
                    break;
                case 31:
                    actor = new TCatMon(robotClient);
                    break;
                case 32:
                    actor = new TScorpionMon(robotClient);
                    break;
                case 33:
                    actor = new TCentipedeKingMon(robotClient);
                    break;
                case 34:
                    actor = new TBigHeartMon(robotClient);
                    break;
                case 35:
                    actor = new TSpiderHouseMon(robotClient);
                    break;
                case 36:
                    actor = new TExplosionSpider(robotClient);
                    break;
                case 37:
                    actor = new TFlyingSpider(robotClient);
                    break;
                case 38:
                    actor = new TSnowMon(robotClient);
                    break;
                case 39:
                    actor = new TSnowMon(robotClient);
                    break;
                case 40:
                    actor = new TZombiLighting(robotClient);
                    break;
                case 41:
                    actor = new TZombiDigOut(robotClient);
                    break;
                case 42:
                    actor = new TZombiZilkin(robotClient);
                    break;
                case 43:
                    actor = new TBeeQueen(robotClient);
                    break;
                case 44:
                    actor = new TSnowMon(robotClient);
                    break;
                case 45:
                    actor = new TArcherMon(robotClient);
                    break;
                case 46:
                    actor = new TSnowMon(robotClient);
                    break;
                case 47:
                    actor = new TSculptureMon(robotClient);
                    break;
                case 48:
                    actor = new TSculptureMon(robotClient);
                    break;
                case 49:
                    actor = new TSculptureKingMon(robotClient);
                    break;
                case 50:
                    actor = new TNpcActor(robotClient);
                    break;
                case 51:
                    actor = new TSnowMon(robotClient);
                    break;
                case 52:
                    actor = new TGasKuDeGi(robotClient);
                    break;
                case 53:
                    actor = new TGasKuDeGi(robotClient);
                    break;
                case 54:
                    actor = new TSmallElfMonster(robotClient);
                    break;
                case 55:
                    actor = new TWarriorElfMonster(robotClient);
                    break;
                case 56:
                    actor = new TAngel(robotClient);
                    break;
                case 57:
                    actor = new TDualAxeOma(robotClient);
                    break;
                case 58:
                    actor = new TDualAxeOma(robotClient);
                    break;
                case 60:
                    actor = new TElectronicScolpionMon(robotClient);
                    break;
                case 61:
                    actor = new TBossPigMon(robotClient);
                    break;
                case 62:
                    actor = new TKingOfSculpureKingMon(robotClient);
                    break;
                case 63:
                    actor = new TSkeletonKingMon(robotClient);
                    break;
                case 64:
                    actor = new TGasKuDeGi(robotClient);
                    break;
                case 65:
                    actor = new TSamuraiMon(robotClient);
                    break;
                case 66:
                    actor = new TSkeletonSoldierMon(robotClient);
                    break;
                case 67:
                    actor = new TSkeletonSoldierMon(robotClient);
                    break;
                case 68:
                    actor = new TSkeletonSoldierMon(robotClient);
                    break;
                case 69:
                    actor = new TSkeletonArcherMon(robotClient);
                    break;
                case 70:
                    actor = new TBanyaGuardMon(robotClient);
                    break;
                case 71:
                    actor = new TBanyaGuardMon(robotClient);
                    break;
                case 72:
                    actor = new TBanyaGuardMon(robotClient);
                    break;
                case 73:
                    actor = new TPBOMA1Mon(robotClient);
                    break;
                case 74:
                    actor = new TCatMon(robotClient);
                    break;
                case 75:
                    actor = new TStoneMonster(robotClient);
                    break;
                case 76:
                    actor = new TSuperiorGuard(robotClient);
                    break;
                case 77:
                    actor = new TStoneMonster(robotClient);
                    break;
                case 78:
                    actor = new TBanyaGuardMon(robotClient);
                    break;
                case 79:
                    actor = new TPBOMA6Mon(robotClient);
                    break;
                case 80:
                    actor = new TMineMon(robotClient);
                    break;
                case 81:
                    actor = new TAngel(robotClient);
                    break;
                case 83:
                    actor = new TFireDragon(robotClient);
                    break;
                case 84:
                    actor = new TDragonStatue(robotClient);
                    break;
                case 87:
                    actor = new TDragonStatue(robotClient);
                    break;
                case 90:
                    actor = new TDragonBody(robotClient);
                    break;
                case 91:
                    actor = new TWhiteSkeleton(robotClient);
                    break;
                case 92:
                    actor = new TWhiteSkeleton(robotClient);
                    break;
                case 93:
                    actor = new TWhiteSkeleton(robotClient);
                    break;
                case 94:
                    actor = new TWarriorElfMonster(robotClient);
                    break;
                case 95:
                    actor = new TWarriorElfMonster(robotClient);
                    break;
                case 98:
                    actor = new TWallStructure(robotClient);
                    break;
                case 99:
                    actor = new TCastleDoor(robotClient);
                    break;
                case 101:
                    actor = new TBanyaGuardMon(robotClient);
                    break;
                case 102:
                    actor = new TKhazardMon(robotClient);
                    break;
                case 103:
                    actor = new TFrostTiger(robotClient);
                    break;
                case 104:
                    actor = new TRedThunderZuma(robotClient);
                    break;
                case 105:
                    actor = new TCrystalSpider(robotClient);
                    break;
                case 106:
                    actor = new TYimoogi(robotClient);
                    break;
                case 109:
                    actor = new TBlackFox(robotClient);
                    break;
                case 110:
                    actor = new TGreenCrystalSpider(robotClient);
                    break;
                case 111:
                    actor = new TBanyaGuardMon(robotClient);
                    break;
                case 113:
                    actor = new TBanyaGuardMon(robotClient);
                    break;
                case 114:
                    actor = new TBanyaGuardMon(robotClient);
                    break;
                case 115:
                    actor = new TBanyaGuardMon(robotClient);
                    break;
                case 117:
                case 118:
                case 119:
                    actor = new TBanyaGuardMon(robotClient);
                    break;
                case 120:
                    actor = new TFireDragon(robotClient);
                    break;
                case 121:
                    actor = new TTiger(robotClient);
                    break;
                case 122:
                    actor = new TDragon(robotClient);
                    break;
                case 123:
                    actor = new TGhostShipMonster(robotClient);
                    break;
                default:
                    actor = new TActor(robotClient);
                    break;
            }
            actor.m_nRecogId = chrid;
            actor.m_nCurrX = cx;
            actor.m_nCurrY = cy;
            actor.m_nRx = actor.m_nCurrX;
            actor.m_nRy = actor.m_nCurrY;
            actor.m_btDir = (byte)cdir;
            actor.m_nFeature = cfeature;
            if (MShare.g_boOpenAutoPlay && MShare.g_gcAss[6])
            {
                // actor.m_btAFilter = MShare.g_APMobList.IndexOf(actor.m_sUserName) >= 0;
            }
            actor.m_btRace = RACEfeature(cfeature);
            actor.m_Action = null;
            actor.m_nState = cstate;
            m_ActorList.Add(actor);
            result = actor;
            return result;
        }

        public void ActorDied(TActor actor)
        {
            for (var i = 0; i < m_ActorList.Count; i++)
            {
                if (m_ActorList[i] == actor)
                {
                    m_ActorList.RemoveAt(i);
                    break;
                }
            }
            bool flag = false;
            for (var i = 0; i < m_ActorList.Count; i++)
            {
                if (!m_ActorList[i].m_boDeath)
                {
                    m_ActorList.Insert(i, actor);
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                m_ActorList.Add(actor);
            }
        }

        public void SetActorDrawLevel(TActor actor, int level)
        {
            if (level == 0)
            {
                for (var i = 0; i < m_ActorList.Count; i++)
                {
                    if (m_ActorList[i] == actor)
                    {
                        m_ActorList.RemoveAt(i);
                        m_ActorList.Insert(0, actor);
                        break;
                    }
                }
            }
        }

        // 清除所有角色
        public void ClearActors()
        {
            m_ActorList.Clear();
            MShare.g_MySelf = null;
            MShare.g_TargetCret = null;
            MShare.g_FocusCret = null;
            MShare.g_MagicTarget = null;
        }

        public TActor DeleteActor(int id, bool boDeath)
        {
            TActor result = null;
            int i = 0;
            while (true)
            {
                if (i >= m_ActorList.Count)
                {
                    break;
                }
                if (m_ActorList[i].m_nRecogId == id)
                {
                    if (MShare.g_TargetCret == m_ActorList[i])
                    {
                        MShare.g_TargetCret = null;
                    }
                    if (MShare.g_FocusCret == m_ActorList[i])
                    {
                        MShare.g_FocusCret = null;
                    }
                    if (MShare.g_MagicTarget == m_ActorList[i])
                    {
                        MShare.g_MagicTarget = null;
                    }
                    if (IsMySlaveObject(m_ActorList[i]))
                    {
                        if (!boDeath)
                        {
                            break;
                        }
                    }
                    m_ActorList[i].m_dwDeleteTime = MShare.GetTickCount();
                    MShare.g_FreeActorList.Add(m_ActorList[i]);
                    m_ActorList.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
            return result;
        }

        public TActor DeleteActor(int id)
        {
            return DeleteActor(id, false);
        }

        public void DelActor(object actor)
        {
            for (var i = 0; i < m_ActorList.Count; i++)
            {
                if (m_ActorList[i] == actor)
                {
                    m_ActorList[i].m_dwDeleteTime = MShare.GetTickCount();
                    MShare.g_FreeActorList.Add(m_ActorList[i]);
                    m_ActorList.RemoveAt(i);
                    break;
                }
            }
        }

        public TActor ButchAnimal(int x, int y)
        {
            TActor result = null;
            for (var i = 0; i < m_ActorList.Count; i++)
            {
                TActor a = m_ActorList[i];
                if (a.m_boDeath)
                {
                    if ((Math.Abs(a.m_nCurrX - x) <= 1) && (Math.Abs(a.m_nCurrY - y) <= 1))
                    {
                        result = a;
                        break;
                    }
                }
            }
            return result;
        }

        public void SendMsg(int ident, int chrid, int x, int y, int cdir, int feature, int state, string str, int ipInfo = 0)
        {
            TActor actor;
            MessageBodyW mbw;
            switch (ident)
            {
                case Grobal2.SM_CHANGEMAP:
                case Grobal2.SM_NEWMAP:
                    ProcMagic.NTargetX = -1;
                    //robotClient.EventMan.ClearEvents();
                    robotClient.g_PathBusy = true;
                    try
                    {
                        if (robotClient.TimerAutoMove != null)
                        {
                            if (robotClient.TimerAutoMove.Enabled)
                            {
                                robotClient.TimerAutoMove.Enabled = false;
                                TPathMap.g_MapPath = new Point[0];
                                TPathMap.g_MapPath = null;
                                robotClient.DScreen.AddChatBoardString("地图跳转，停止自动移动", robotClient.GetRGB(5));
                            }
                            if (MShare.g_boOpenAutoPlay && robotClient.TimerAutoPlay.Enabled)
                            {
                                robotClient.TimerAutoPlay.Enabled = false;
                                MShare.g_gcAss[0] = false;
                                MShare.g_APMapPath = new Point[0];
                                MShare.g_APMapPath2 = new Point[0];
                                MShare.g_APStep = -1;
                                MShare.g_APLastPoint.X = -1;
                                robotClient.DScreen.AddChatBoardString("[挂机] 地图跳转，停止自动挂机", ConsoleColor.Red);
                            }
                        }
                        if (MShare.g_MySelf != null)
                        {
                            MShare.g_MySelf.m_nTagX = 0;
                            MShare.g_MySelf.m_nTagY = 0;
                        }
                        if (robotClient.Map.m_MapBuf != null)
                        {
                            //FreeMem(robotClient.Map.m_MapBuf);
                            robotClient.Map.m_MapBuf = null;
                        }
                        if (robotClient.Map.m_MapData != null)
                        {
                            robotClient.Map.m_MapData = new TCellParams[0, 0];
                            robotClient.Map.m_MapData = null;
                        }
                    }
                    finally
                    {
                        robotClient.g_PathBusy = false;
                    }
                    robotClient.Map.LoadMap(str, x, y);
                    if ((ident == Grobal2.SM_NEWMAP) && (MShare.g_MySelf != null))
                    {
                        MShare.g_MySelf.m_nCurrX = x;
                        MShare.g_MySelf.m_nCurrY = y;
                        MShare.g_MySelf.m_nRx = x;
                        MShare.g_MySelf.m_nRy = y;
                        DelActor(MShare.g_MySelf);
                    }
                    break;
                case Grobal2.SM_LOGON:
                    actor = FindActor(chrid);
                    if (actor == null)
                    {
                        actor = NewActor(chrid, x, y, HUtil32.LoByte(cdir), feature, state);
                        actor.m_nChrLight = HUtil32.HiByte(cdir);
                        cdir = HUtil32.LoByte(cdir);
                        actor.SendMsg(Grobal2.SM_TURN, x, y, cdir, feature, state, "", 0);
                    }
                    if (MShare.g_MySelf != null)
                    {
                        MShare.g_MySelf.m_SlaveObject.Clear();
                        MShare.g_MySelf = null;
                    }
                    MShare.g_MySelf = (THumActor)actor;
                    break;
                case Grobal2.SM_HIDE:
                    actor = FindActor(chrid);
                    if (actor != null)
                    {
                        if (actor.m_boDelActionAfterFinished)
                        {
                            return;
                        }
                        if (actor.m_nWaitForRecogId != 0)
                        {
                            return;
                        }
                        if (IsMySlaveObject(actor))
                        {
                            if ((cdir != 0) || actor.m_boDeath)
                            {
                                DeleteActor(chrid, true);
                            }
                            return;
                        }
                    }
                    DeleteActor(chrid);
                    break;
                default:
                    actor = FindActor(chrid);
                    if ((ident == Grobal2.SM_TURN) || (ident == Grobal2.SM_RUN) || (ident == Grobal2.SM_HORSERUN) || (ident == Grobal2.SM_WALK) || (ident == Grobal2.SM_BACKSTEP) || (ident == Grobal2.SM_DEATH) || (ident == Grobal2.SM_SKELETON) || (ident == Grobal2.SM_DIGUP) || (ident == Grobal2.SM_ALIVE))
                    {
                        if (actor == null)
                        {
                            actor = NewActor(chrid, x, y, HUtil32.LoByte(cdir), feature, state);
                        }
                        if (actor != null)
                        {
                            if (ipInfo != 0)
                            {
                                actor.m_nIPowerLvl = HUtil32.HiWord(ipInfo);
                            }
                            actor.m_nChrLight = HUtil32.HiByte(cdir);
                            cdir = HUtil32.LoByte(cdir);
                            if (ident == Grobal2.SM_SKELETON)
                            {
                                actor.m_boDeath = true;
                                actor.m_boSkeleton = true;
                            }
                            if (ident == Grobal2.SM_DEATH)
                            {
                                if (HUtil32.HiByte(cdir) != 0)
                                {
                                    actor.m_boItemExplore = true;
                                }
                            }
                        }
                    }
                    if (actor == null)
                    {
                        return;
                    }
                    switch (ident)
                    {
                        case Grobal2.SM_FEATURECHANGED:
                            actor.m_nFeature = feature;
                            actor.m_nFeatureEx = state;
                            if (!string.IsNullOrEmpty(str))
                            {
                                mbw = EDCode.DecodeBuffer<MessageBodyW>(str);
                                actor.m_btTitleIndex = (byte)HUtil32.LoWord(mbw.Param1);
                            }
                            else
                            {
                                actor.m_btTitleIndex = 0;
                            }
                            actor.FeatureChanged();
                            break;
                        case Grobal2.SM_CHARSTATUSCHANGED:
                            actor.m_nState = feature;
                            actor.m_nHitSpeed = (ushort)state;
                            break;
                        default:
                            if (ident == Grobal2.SM_TURN)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    actor.m_sUserName = str;
                                }
                            }
                            actor.SendMsg(ident, x, y, cdir, feature, state, "", 0);
                            break;
                    }
                    break;
            }
        }

        public static bool IsMySlaveObject(TActor atc)
        {
            var result = false;
            if (MShare.g_MySelf == null)
            {
                return result;
            }
            for (var i = 0; i < MShare.g_MySelf.m_SlaveObject.Count; i++)
            {
                if (atc == MShare.g_MySelf.m_SlaveObject[i])
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
