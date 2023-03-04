using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using BotSrv.Data;
using BotSrv.Maps;
using BotSrv.Objects;
using BotSrv.Player;
using SystemModule;
using SystemModule.Enums;
using SystemModule.Packets.ServerPackets;

namespace BotSrv.Scenes.Scene
{
    public class PlayScene : SceneBase
    {
        private int m_dwMoveTime = 0;
        private readonly int _mNDefXx = 0;
        private readonly int _mNDefYy = 0;
        public IList<Actor> ActorList = null;
        public ProcMagic ProcMagic = null;

        public PlayScene(RobotPlayer robotClient) : base(SceneType.PlayGame, robotClient)
        {
            ProcMagic = new ProcMagic();
            ProcMagic.NTargetX = -1;
            ActorList = new List<Actor>();
            m_dwMoveTime = HUtil32.GetTickCount();
        }

        public override void OpenScene()
        {
            RobotClient.SocketEvents();
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
                if ((Math.Abs(dropItem.X - MShare.MySelf.CurrX) > 20) || (Math.Abs(dropItem.Y - MShare.MySelf.CurrY) > 20))
                {
                    MShare.g_DropedItemList.RemoveAt(i);
                    break;
                }
            }
        }

        public void BeginScene()
        {
            if (MShare.MySelf == null)
            {
                return;
            }
            var movetick = false;
            var tick = HUtil32.GetTickCount();
            if (MShare.SpeedRate)
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
                if (i >= ActorList.Count)
                {
                    break;
                }
                var actor = ActorList[i];
                if (actor.Death && MShare.g_gcGeneral[8] && !actor.m_boItemExplore && (actor.Race != 0) && actor.IsIdle())
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
                    if (actor != MShare.MySelf)
                    {
                        actor.ProcHurryMsg();
                    }
                }
                if (actor == MShare.MySelf)
                {
                    actor.ProcHurryMsg();
                }
                if (actor.m_nWaitForRecogId != 0)
                {
                    if (actor.IsIdle())
                    {
                        ClFunc.DelChangeFace(actor.m_nWaitForRecogId);
                        NewActor(actor.m_nWaitForRecogId, actor.CurrX, actor.CurrY, actor.m_btDir, actor.m_nWaitForFeature, actor.m_nWaitForStatus);
                        actor.m_nWaitForRecogId = 0;
                        actor.DelActor = true;
                    }
                }
                if (actor.DelActor || (Math.Abs(MShare.MySelf.CurrX - actor.CurrX) > 16) || (Math.Abs(MShare.MySelf.CurrY - actor.CurrY) > 16))
                {
                    MShare.g_FreeActorList.Add(actor);
                    ActorList.RemoveAt(i);
                    if (MShare.TargetCret == actor)
                    {
                        MShare.TargetCret = null;
                    }
                    if (MShare.FocusCret == actor)
                    {
                        MShare.FocusCret = null;
                    }
                    if (MShare.MagicLockActor == actor)
                    {
                        MShare.MagicLockActor = null;
                    }
                    if (MShare.MagicTarget == actor)
                    {
                        MShare.MagicTarget = null;
                    }
                }
                else
                {
                    i++;
                }
            }
            RobotClient.Map.UpdateMapPos(MShare.MySelf.m_nRx, MShare.MySelf.m_nRy);
        }

        public void PlaySurface(object sender)
        {

        }

        public void MagicSurface(object sender)
        {

        }

        public void NewMagic(Actor aowner, int magid, int magnumb, int cx, int cy, int tx, int ty, int targetCode, MagicType mtype, bool recusion, int anitime, ref bool boFly, int maglv, int poison)
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

        public Actor GetCharacter(int x, int y, int wantsel, ref int nowsel, bool liveonly)
        {
            int ccy = 0;
            Actor result = null;
            nowsel = -1;
            for (var k = ccy + 8; k >= ccy - 1; k--)
            {
                for (var i = ActorList.Count - 1; i >= 0; i--)
                {
                    if (ActorList[i] != MShare.MySelf)
                    {
                        Actor a = ActorList[i];
                        if ((!liveonly || !a.Death) && a.m_boHoldPlace && a.Visible)
                        {
                            if (a.CurrY == k)
                            {
                                int dx = (a.m_nRx - RobotClient.Map.m_ClientRect.Left) * BotConst.UNITX + _mNDefXx + a.m_nPx + a.m_nShiftX;
                                int dy = (a.m_nRy - RobotClient.Map.m_ClientRect.Top - 1) * BotConst.UNITY + _mNDefYy + a.m_nPy + a.m_nShiftY;
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
        public Actor GetAttackFocusCharacter(int x, int y, int wantsel, ref int nowsel, bool liveonly)
        {
            int ccy = 0;
            Actor a;
            Actor result = GetCharacter(x, y, wantsel, ref nowsel, liveonly);
            if (result == null)
            {
                nowsel = -1;
                for (var k = ccy + 8; k >= ccy - 1; k--)
                {
                    for (var i = ActorList.Count - 1; i >= 0; i--)
                    {
                        if (ActorList[i] != MShare.MySelf)
                        {
                            a = ActorList[i];
                            if ((!liveonly || !a.Death) && a.m_boHoldPlace && a.Visible)
                            {
                                if (a.CurrY == k)
                                {
                                    int dx = (a.m_nRx - RobotClient.Map.m_ClientRect.Left) * BotConst.UNITX + _mNDefXx + a.m_nPx + a.m_nShiftX;
                                    int dy = (a.m_nRy - RobotClient.Map.m_ClientRect.Top - 1) * BotConst.UNITY + _mNDefYy + a.m_nPy + a.m_nShiftY;
                                    int centx;
                                    if (a.CharWidth() > 40)
                                    {
                                        centx = (a.CharWidth() - 40) / 2;
                                    }
                                    else
                                    {
                                        centx = 0;
                                    }

                                    int centy;
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
                if (MShare.MySelf.CurrY == k)
                {
                    int dx = (MShare.MySelf.m_nRx - RobotClient.Map.m_ClientRect.Left) * BotConst.UNITX + _mNDefXx + MShare.MySelf.m_nPx + MShare.MySelf.m_nShiftX;
                    int dy = (MShare.MySelf.m_nRy - RobotClient.Map.m_ClientRect.Top - 1) * BotConst.UNITY + _mNDefYy + MShare.MySelf.m_nPy + MShare.MySelf.m_nShiftY;
                    if (MShare.MySelf.CheckSelect(x - dx, y - dy))
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
                    if (MShare.PickUpAll || dropItem.boPickUp)
                    {
                        break;
                    }
                }
            }
            return result;
        }

        public bool CanRun(short sX, short sY, int ex, int ey)
        {
            bool result;
            byte ndir = ClFunc.GetNextDirection(sX, sY, ex, ey);
            short rx = sX;
            short ry = sY;
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
            if (RobotClient.Map.CanMove(mx, my))
            {
                result = !CrashManEx(mx, my);
            }
            return result;
        }

        private bool CrashManEx(int mx, int my)
        {
            bool result = false;
            for (var i = 0; i < ActorList.Count; i++)
            {
                Actor actor = ActorList[i];
                if (actor == MShare.MySelf)
                {
                    continue;
                }
                if (actor.Visible && actor.m_boHoldPlace && (!actor.Death) && (actor.CurrX == mx) && (actor.CurrY == my))
                {
                    if ((MShare.MySelf.m_nTagX == 0) && (MShare.MySelf.m_nTagY == 0))
                    {
                        if ((actor.Race == ActorRace.Play) && (MShare.g_boCanRunHuman || MShare.g_boCanRunSafeZone))
                        {
                            continue;
                        }
                        if ((actor.Race == ActorRace.Merchant) && MShare.g_boCanRunNpc)
                        {
                            continue;
                        }
                        if ((actor.Race > ActorRace.Play) && (actor.Race != ActorRace.Merchant) && (MShare.g_boCanRunMon || MShare.g_boCanRunSafeZone))
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
            if (RobotClient.Map.CanMove(mx, my))
            {
                result = !CrashMan(mx, my);
            }
            return result;
        }

        public bool CrashMan(int mx, int my)
        {
            bool result = false;
            for (var i = 0; i < ActorList.Count; i++)
            {
                Actor actor = ActorList[i];
                if (actor == null || actor == MShare.MySelf)
                {
                    continue;
                }
                if (actor.Visible && actor.m_boHoldPlace && (!actor.Death) && (actor.CurrX == mx) && (actor.CurrY == my))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public bool CanFly(int mx, int my)
        {
            return RobotClient.Map.CanFly(mx, my);
        }

        public Actor FindActor(int id)
        {
            Actor result = null;
            if (id == 0)
            {
                return result;
            }
            for (var i = 0; i < ActorList.Count; i++)
            {
                if (ActorList[i].RecogId == id)
                {
                    result = ActorList[i];
                    break;
                }
            }
            return result;
        }

        public Actor FindActor(string sName)
        {
            Actor result = null;
            for (var i = 0; i < ActorList.Count; i++)
            {
                Actor actor = ActorList[i];
                if (string.Compare(actor.UserName, sName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = actor;
                    break;
                }
            }
            return result;
        }

        public Actor FindActorXY(int x, int y)
        {
            Actor result = null;
            for (var i = 0; i < ActorList.Count; i++)
            {
                Actor a = ActorList[i];
                if ((a.CurrX == x) && (a.CurrY == y))
                {
                    result = a;
                    if (!result.Death && result.Visible && result.m_boHoldPlace)
                    {
                        break;
                    }
                }
            }
            return result;
        }

        public bool IsValidActor(Actor actor)
        {
            bool result = false;
            for (var i = 0; i < ActorList.Count; i++)
            {
                if (ActorList[i] == actor)
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

        public Actor NewActor(int chrid, int cx, int cy, ushort cdir, int cfeature, int cstate)
        {
            Actor actor;
            Actor result = null;
            for (var i = 0; i < ActorList.Count; i++)
            {
                if (ActorList[i].RecogId == chrid)
                {
                    result = ActorList[i];
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
                    actor = new THumActor(RobotClient);
                    break;
                case 9:
                    actor = new TSoccerBall(RobotClient);
                    break;
                case 13:
                    actor = new TKillingHerb(RobotClient);
                    break;
                case 14:
                    actor = new TSkeletonOma(RobotClient);
                    break;
                case 15:
                    actor = new TDualAxeOma(RobotClient);
                    break;
                case 16:
                    actor = new TGasKuDeGi(RobotClient);
                    break;
                case 17:
                    actor = new TCatMon(RobotClient);
                    break;
                case 18:
                    actor = new THuSuABi(RobotClient);
                    break;
                case 19:
                    actor = new TCatMon(RobotClient);
                    break;
                case 20:
                    actor = new TFireCowFaceMon(RobotClient);
                    break;
                case 21:
                    actor = new TCowFaceKing(RobotClient);
                    break;
                case 22:
                    actor = new TDualAxeOma(RobotClient);
                    break;
                case 23:
                    actor = new TWhiteSkeleton(RobotClient);
                    break;
                case 24:
                    actor = new TSuperiorGuard(RobotClient);
                    break;
                case 25:
                    actor = new TKingOfSculpureKingMon(RobotClient);
                    break;
                case 26:
                    actor = new TKingOfSculpureKingMon(RobotClient);
                    break;
                case 27:
                    actor = new TSnowMon(RobotClient);
                    break;
                case 28:
                    actor = new TSnowMon(RobotClient);
                    break;
                case 29:
                    actor = new TSnowMon(RobotClient);
                    break;
                case 30:
                    actor = new TCatMon(RobotClient);
                    break;
                case 31:
                    actor = new TCatMon(RobotClient);
                    break;
                case 32:
                    actor = new TScorpionMon(RobotClient);
                    break;
                case 33:
                    actor = new TCentipedeKingMon(RobotClient);
                    break;
                case 34:
                    actor = new TBigHeartMon(RobotClient);
                    break;
                case 35:
                    actor = new TSpiderHouseMon(RobotClient);
                    break;
                case 36:
                    actor = new TExplosionSpider(RobotClient);
                    break;
                case 37:
                    actor = new TFlyingSpider(RobotClient);
                    break;
                case 38:
                    actor = new TSnowMon(RobotClient);
                    break;
                case 39:
                    actor = new TSnowMon(RobotClient);
                    break;
                case 40:
                    actor = new TZombiLighting(RobotClient);
                    break;
                case 41:
                    actor = new TZombiDigOut(RobotClient);
                    break;
                case 42:
                    actor = new TZombiZilkin(RobotClient);
                    break;
                case 43:
                    actor = new TBeeQueen(RobotClient);
                    break;
                case 44:
                    actor = new TSnowMon(RobotClient);
                    break;
                case 45:
                    actor = new TArcherMon(RobotClient);
                    break;
                case 46:
                    actor = new TSnowMon(RobotClient);
                    break;
                case 47:
                    actor = new TSculptureMon(RobotClient);
                    break;
                case 48:
                    actor = new TSculptureMon(RobotClient);
                    break;
                case 49:
                    actor = new TSculptureKingMon(RobotClient);
                    break;
                case 50:
                    actor = new TNpcActor(RobotClient);
                    break;
                case 51:
                    actor = new TSnowMon(RobotClient);
                    break;
                case 52:
                    actor = new TGasKuDeGi(RobotClient);
                    break;
                case 53:
                    actor = new TGasKuDeGi(RobotClient);
                    break;
                case 54:
                    actor = new TSmallElfMonster(RobotClient);
                    break;
                case 55:
                    actor = new TWarriorElfMonster(RobotClient);
                    break;
                case 56:
                    actor = new TAngel(RobotClient);
                    break;
                case 57:
                    actor = new TDualAxeOma(RobotClient);
                    break;
                case 58:
                    actor = new TDualAxeOma(RobotClient);
                    break;
                case 60:
                    actor = new TElectronicScolpionMon(RobotClient);
                    break;
                case 61:
                    actor = new TBossPigMon(RobotClient);
                    break;
                case 62:
                    actor = new TKingOfSculpureKingMon(RobotClient);
                    break;
                case 63:
                    actor = new TSkeletonKingMon(RobotClient);
                    break;
                case 64:
                    actor = new TGasKuDeGi(RobotClient);
                    break;
                case 65:
                    actor = new TSamuraiMon(RobotClient);
                    break;
                case 66:
                    actor = new TSkeletonSoldierMon(RobotClient);
                    break;
                case 67:
                    actor = new TSkeletonSoldierMon(RobotClient);
                    break;
                case 68:
                    actor = new TSkeletonSoldierMon(RobotClient);
                    break;
                case 69:
                    actor = new TSkeletonArcherMon(RobotClient);
                    break;
                case 70:
                    actor = new TBanyaGuardMon(RobotClient);
                    break;
                case 71:
                    actor = new TBanyaGuardMon(RobotClient);
                    break;
                case 72:
                    actor = new TBanyaGuardMon(RobotClient);
                    break;
                case 73:
                    actor = new TPBOMA1Mon(RobotClient);
                    break;
                case 74:
                    actor = new TCatMon(RobotClient);
                    break;
                case 75:
                    actor = new TStoneMonster(RobotClient);
                    break;
                case 76:
                    actor = new TSuperiorGuard(RobotClient);
                    break;
                case 77:
                    actor = new TStoneMonster(RobotClient);
                    break;
                case 78:
                    actor = new TBanyaGuardMon(RobotClient);
                    break;
                case 79:
                    actor = new TPBOMA6Mon(RobotClient);
                    break;
                case 80:
                    actor = new TMineMon(RobotClient);
                    break;
                case 81:
                    actor = new TAngel(RobotClient);
                    break;
                case 83:
                    actor = new TFireDragon(RobotClient);
                    break;
                case 84:
                    actor = new TDragonStatue(RobotClient);
                    break;
                case 87:
                    actor = new TDragonStatue(RobotClient);
                    break;
                case 90:
                    actor = new TDragonBody(RobotClient);
                    break;
                case 91:
                    actor = new TWhiteSkeleton(RobotClient);
                    break;
                case 92:
                    actor = new TWhiteSkeleton(RobotClient);
                    break;
                case 93:
                    actor = new TWhiteSkeleton(RobotClient);
                    break;
                case 94:
                    actor = new TWarriorElfMonster(RobotClient);
                    break;
                case 95:
                    actor = new TWarriorElfMonster(RobotClient);
                    break;
                case 98:
                    actor = new TWallStructure(RobotClient);
                    break;
                case 99:
                    actor = new TCastleDoor(RobotClient);
                    break;
                case 101:
                    actor = new TBanyaGuardMon(RobotClient);
                    break;
                case 102:
                    actor = new TKhazardMon(RobotClient);
                    break;
                case 103:
                    actor = new TFrostTiger(RobotClient);
                    break;
                case 104:
                    actor = new TRedThunderZuma(RobotClient);
                    break;
                case 105:
                    actor = new TCrystalSpider(RobotClient);
                    break;
                case 106:
                    actor = new TYimoogi(RobotClient);
                    break;
                case 109:
                    actor = new TBlackFox(RobotClient);
                    break;
                case 110:
                    actor = new TGreenCrystalSpider(RobotClient);
                    break;
                case 111:
                    actor = new TBanyaGuardMon(RobotClient);
                    break;
                case 113:
                    actor = new TBanyaGuardMon(RobotClient);
                    break;
                case 114:
                    actor = new TBanyaGuardMon(RobotClient);
                    break;
                case 115:
                    actor = new TBanyaGuardMon(RobotClient);
                    break;
                case 117:
                case 118:
                case 119:
                    actor = new TBanyaGuardMon(RobotClient);
                    break;
                case 120:
                    actor = new TFireDragon(RobotClient);
                    break;
                case 121:
                    actor = new TTiger(RobotClient);
                    break;
                case 122:
                    actor = new TDragon(RobotClient);
                    break;
                case 123:
                    actor = new TGhostShipMonster(RobotClient);
                    break;
                default:
                    actor = new Actor(RobotClient);
                    break;
            }
            actor.RecogId = chrid;
            actor.CurrX = (short)(ushort)(short)cx;
            actor.CurrY = (short)(ushort)(short)cy;
            actor.m_nRx = actor.CurrX;
            actor.m_nRy = actor.CurrY;
            actor.m_btDir = (byte)cdir;
            actor.m_nFeature = cfeature;
            if (MShare.OpenAutoPlay && MShare.g_gcAss[6])
            {
                // actor.m_btAFilter = MShare.g_APMobList.IndexOf(actor.m_sUserName) >= 0;
            }
            actor.Race = RACEfeature(cfeature);
            actor.m_Action = null;
            actor.m_nState = cstate;
            ActorList.Add(actor);
            result = actor;
            return result;
        }

        public void ActorDied(Actor actor)
        {
            for (var i = 0; i < ActorList.Count; i++)
            {
                if (ActorList[i] == actor)
                {
                    ActorList.RemoveAt(i);
                    break;
                }
            }
            bool flag = false;
            for (var i = 0; i < ActorList.Count; i++)
            {
                if (!ActorList[i].Death)
                {
                    ActorList.Insert(i, actor);
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                ActorList.Add(actor);
            }
        }

        public void SetActorDrawLevel(Actor actor, int level)
        {
            if (level == 0)
            {
                for (var i = 0; i < ActorList.Count; i++)
                {
                    if (ActorList[i] == actor)
                    {
                        ActorList.RemoveAt(i);
                        ActorList.Insert(0, actor);
                        break;
                    }
                }
            }
        }

        // 清除所有角色
        public void ClearActors()
        {
            ActorList.Clear();
            MShare.MySelf = null;
            MShare.TargetCret = null;
            MShare.FocusCret = null;
            MShare.MagicTarget = null;
        }

        public Actor DeleteActor(int id, bool boDeath)
        {
            Actor result = null;
            int i = 0;
            while (true)
            {
                if (i >= ActorList.Count)
                {
                    break;
                }
                if (ActorList[i].RecogId == id)
                {
                    if (MShare.TargetCret == ActorList[i])
                    {
                        MShare.TargetCret = null;
                    }
                    if (MShare.FocusCret == ActorList[i])
                    {
                        MShare.FocusCret = null;
                    }
                    if (MShare.MagicTarget == ActorList[i])
                    {
                        MShare.MagicTarget = null;
                    }
                    if (IsMySlaveObject(ActorList[i]))
                    {
                        if (!boDeath)
                        {
                            break;
                        }
                    }
                    ActorList[i].m_dwDeleteTime = MShare.GetTickCount();
                    MShare.g_FreeActorList.Add(ActorList[i]);
                    ActorList.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
            return result;
        }

        public Actor DeleteActor(int id)
        {
            return DeleteActor(id, false);
        }

        public void DelActor(object actor)
        {
            for (var i = 0; i < ActorList.Count; i++)
            {
                if (ActorList[i] == actor)
                {
                    ActorList[i].m_dwDeleteTime = MShare.GetTickCount();
                    MShare.g_FreeActorList.Add(ActorList[i]);
                    ActorList.RemoveAt(i);
                    break;
                }
            }
        }

        public Actor ButchAnimal(int x, int y)
        {
            Actor result = null;
            for (var i = 0; i < ActorList.Count; i++)
            {
                Actor a = ActorList[i];
                if (a.Death)
                {
                    if ((Math.Abs(a.CurrX - x) <= 1) && (Math.Abs(a.CurrY - y) <= 1))
                    {
                        result = a;
                        break;
                    }
                }
            }
            return result;
        }

        public void SendMsg(int ident, int chrid, ushort x, ushort y, byte cdir, int feature, int state, string str, int ipInfo = 0)
        {
            Actor actor;
            MessageBodyW mbw;
            switch (ident)
            {
                case Messages.SM_CHANGEMAP:
                case Messages.SM_NEWMAP:
                    ProcMagic.NTargetX = -1;
                    //robotClient.EventMan.ClearEvents();
                    RobotClient.g_PathBusy = true;
                    try
                    {
                        if (RobotClient.TimerAutoMove != null)
                        {
                            if (RobotClient.TimerAutoMove.Enabled)
                            {
                                RobotClient.TimerAutoMove.Enabled = false;
                                PathMap.g_MapPath = new Point[0];
                                PathMap.g_MapPath = null;
                                ScreenManager.AddChatBoardString("地图跳转，停止自动移动");
                            }
                            if (MShare.OpenAutoPlay && RobotClient.TimerAutoPlay.Enabled)
                            {
                                RobotClient.TimerAutoPlay.Enabled = false;
                                MShare.g_gcAss[0] = false;
                                MShare.MapPath = new Point[0];
                                MShare.g_APMapPath2 = new Point[0];
                                MShare.AutoStep = -1;
                                MShare.AutoLastPoint.X = -1;
                                ScreenManager.AddChatBoardString("[挂机] 地图跳转，停止自动挂机");
                            }
                        }
                        if (MShare.MySelf != null)
                        {
                            MShare.MySelf.m_nTagX = 0;
                            MShare.MySelf.m_nTagY = 0;
                        }
                        if (RobotClient.Map.m_MapBuf != null)
                        {
                            //FreeMem(robotClient.Map.m_MapBuf);
                            RobotClient.Map.m_MapBuf = null;
                        }
                        if (RobotClient.Map.m_MapData != null)
                        {
                            RobotClient.Map.m_MapData = new CellParams[0, 0];
                            RobotClient.Map.m_MapData = null;
                        }
                    }
                    finally
                    {
                        RobotClient.g_PathBusy = false;
                    }
                    RobotClient.Map.LoadMap(str, x, y);
                    if ((ident == Messages.SM_NEWMAP) && (MShare.MySelf != null))
                    {
                        MShare.MySelf.CurrX = (short)x;
                        MShare.MySelf.CurrY = (short)y;
                        MShare.MySelf.m_nRx = (short)x;
                        MShare.MySelf.m_nRy = (short)y;
                        DelActor(MShare.MySelf);
                    }
                    break;
                case Messages.SM_LOGON:
                    actor = FindActor(chrid);
                    if (actor == null)
                    {
                        actor = NewActor(chrid, x, y, HUtil32.LoByte(cdir), feature, state);
                        actor.m_nChrLight = HUtil32.HiByte(cdir);
                        cdir = HUtil32.LoByte(cdir);
                        actor.SendMsg(Messages.SM_TURN, x, y, cdir, feature, state, "", 0);
                    }
                    if (MShare.MySelf != null)
                    {
                        MShare.MySelf.m_SlaveObject.Clear();
                        MShare.MySelf = null;
                    }
                    MShare.MySelf = (THumActor)actor;
                    break;
                case Messages.SM_HIDE:
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
                            if ((cdir != 0) || actor.Death)
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
                    if ((ident == Messages.SM_TURN) || (ident == Messages.SM_RUN) || (ident == Messages.SM_HORSERUN) || (ident == Messages.SM_WALK) || (ident == Messages.SM_BACKSTEP) || (ident == Messages.SM_DEATH) || (ident == Messages.SM_SKELETON) || (ident == Messages.SM_DIGUP) || (ident == Messages.SM_ALIVE))
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
                            if (ident == Messages.SM_SKELETON)
                            {
                                actor.Death = true;
                                actor.m_boSkeleton = true;
                            }
                            if (ident == Messages.SM_DEATH)
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
                        case Messages.SM_FEATURECHANGED:
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
                        case Messages.SM_CHARSTATUSCHANGED:
                            actor.m_nState = feature;
                            actor.HitSpeed = (ushort)state;
                            break;
                        default:
                            if (ident == Messages.SM_TURN)
                            {
                                if (!string.IsNullOrEmpty(str))
                                {
                                    actor.UserName = str;
                                }
                            }
                            actor.SendMsg(ident, x, y, cdir, feature, state, "", 0);
                            break;
                    }
                    break;
            }
        }

        public static bool IsMySlaveObject(Actor atc)
        {
            var result = false;
            if (MShare.MySelf == null)
            {
                return result;
            }
            for (var i = 0; i < MShare.MySelf.m_SlaveObject.Count; i++)
            {
                if (atc == MShare.MySelf.m_SlaveObject[i])
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}
