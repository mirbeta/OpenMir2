using System;
using System.Collections;
using System.Drawing;
using SystemModule;

namespace RobotSvr;

public class PlayScene : Scene
{
    public bool MBoPlayChange = false;
    public long MDwPlayChangeTick = 0;
    private long _mDwMoveTime = 0;
    private long _mDwAniTime = 0;
    private int _mNAniCount = 0;
    private int _mNDefXx = 0;
    private int _mNDefYy = 0;
    private Timer _mMainSoundTimer = null;
    public TextBox MemoLog = null;
    public ArrayList MActorList = null;
    public ArrayList MEffectList = null;
    public ArrayList MFlyList = null;
    public long MDwBlinkTime = 0;
    public bool MBoViewBlink = false;
    public ProcMagic ProcMagic = null;

    public PlayScene()
    {
        MMapSurface = null;
        MObjSurface = null;
        MMagSurface = null;
        ProcMagic.NTargetX = -1;
        MActorList = new ArrayList();
        MEffectList = new ArrayList();
        MFlyList = new ArrayList();
        MDwBlinkTime = MShare.GetTickCount();
        MBoViewBlink = false;
        MemoLog = new TextBox(ClMain.frmMain.Owner);
        MemoLog.Parent = ClMain.frmMain;
        MemoLog.BorderStyle = System.Windows.Forms.FormBorderStyle.None;
        MemoLog.Visible = false;
        MemoLog.Ctl3D = true;
        MemoLog.Left = 0;
        MemoLog.Top = 250;
        MemoLog.Width = 300;
        MemoLog.Height = 150;
        _mDwMoveTime = MShare.GetTickCount();
        _mDwAniTime = MShare.GetTickCount();
        _mNAniCount = 0;
        // m_nMoveStepCount := 0;
        _mMainSoundTimer = new Timer(ClMain.frmMain.Owner);
        _mMainSoundTimer.onTimer = SoundOnTimer;
        _mMainSoundTimer.Interval = 1;
        _mMainSoundTimer.Enabled = false;
    }


    ~PlayScene()
    {
        MActorList.Free;
        MEffectList.Free;
        MFlyList.Free;
        base.Destroy();
    }

    private void SoundOnTimer(Object sender)
    {
        SoundUtil.Units.SoundUtil.g_SndMgr.PlaySound(SoundUtil.Units.SoundUtil.s_main_theme);
        _mMainSoundTimer.Interval = 46 * 1000;
    }

    public bool Initialize()
    {
        bool result;
        result = false;
        // 更新
        MMapSurface = new TDXRenderTargetTexture(HGECanvas.Units.HGECanvas.g_DXCanvas);
        MMapSurface.Size = new Point(MShare.g_FScreenWidth + Grobal2.UNITX * 10 + 30, MShare.g_FScreenHeight + Grobal2.UNITY * 10 + 30);
        MMapSurface.Active = true;
        if (!MMapSurface.Active)
        {
            return result;
        }
        MObjSurface = new TDXRenderTargetTexture(HGECanvas.Units.HGECanvas.g_DXCanvas);
        MObjSurface.Size = new Point(MShare.g_FScreenWidth + Grobal2.UNITX * 8 + 30, MShare.g_FScreenHeight + Grobal2.UNITY * 8 + 30);
        MObjSurface.Active = true;
        if (!MObjSurface.Active)
        {
            return result;
        }
        MMagSurface = new TDXRenderTargetTexture(HGECanvas.Units.HGECanvas.g_DXCanvas);
        MMagSurface.Size = new Point(MShare.g_FScreenWidth, MShare.g_FScreenHeight);
        MMagSurface.Active = true;
        if (!MMagSurface.Active)
        {
            return result;
        }
        result = true;
        return result;
    }

    public override void Finalize()
    {
        if (MMapSurface != null)
        {
            MMapSurface.Free;
        }
        if (MObjSurface != null)
        {
            MObjSurface.Free;
        }
        if (MMagSurface != null)
        {
            MMagSurface.Free;
        }
        MMapSurface = null;
        MObjSurface = null;
        MMagSurface = null;
    }

    public void Recovered()
    {
        if (MMapSurface != null)
        {
            MMapSurface.Recovered;
        }
        if (MObjSurface != null)
        {
            MObjSurface.Recovered;
        }
        if (MMagSurface != null)
        {
            MMagSurface.Recovered;
        }
    }

    public void Lost()
    {
        if (MMapSurface != null)
        {
            MMapSurface.Lost;
        }
        if (MObjSurface != null)
        {
            MObjSurface.Lost;
        }
        if (MMagSurface != null)
        {
            MMagSurface.Lost;
        }
    }

    public bool CanDrawTileMap()
    {
        bool result;
        result = false;
        TMap wvar1 = ClMain.Map;
        if ((wvar1.m_ClientRect.Left == wvar1.m_OldClientRect.Left) && (wvar1.m_ClientRect.Top == wvar1.m_OldClientRect.Top))
        {
            return result;
        }
        if (!MShare.g_boDrawTileMap)
        {
            return result;
        }
        result = true;
        return result;
    }

    public override void OpenScene()
    {
        // 进入游戏场景
        ClMain.HGE.Gfx_Restore(MShare.g_FScreenWidth, MShare.g_FScreenHeight, 32);
        FrmDlg.DEditChat.Visible = false;
        // 迷你地图
        FrmDlg.DWinMiniMap.Visible = true;
        FrmDlg.ViewBottomBox(true);
    }

    public override void CloseScene()
    {
        // 关闭游戏场景
        SoundUtil.Units.SoundUtil.g_SndMgr.SilenceSound();
        FrmDlg.DEditChat.Visible = false;
        FrmDlg.ViewBottomBox(false);
    }

    public override void OpeningScene()
    {
    }

    public void CleanObjects()
    {
        int i;
        for (i = MActorList.Count - 1; i >= 0; i--)
        {
            // (TActor(m_ActorList[i]) <> g_MySelf.m_SlaveObject)
            if ((((TActor)(MActorList[i])) != MShare.g_MySelf) && (((TActor)(MActorList[i])) != MShare.g_MySelf.m_HeroObject) && !Units.PlayScn.IsMySlaveObject(((TActor)(MActorList[i]))))
            {
                // BLUE
                ((TActor)(MActorList[i])).Free;
                MActorList.RemoveAt(i);
            }
        }
        MShare.g_TargetCret = null;
        MShare.g_FocusCret = null;
        MShare.g_MagicTarget = null;
        for (i = 0; i < MEffectList.Count; i++)
        {
            ((TMagicEff)(MEffectList[i])).Free;
        }
        MEffectList.Clear();
    }

    public void DrawTileMap()
    {
        
    }

    private void ClearDropItemA()
    {
        int i;
        TDropItem dropItem;
        for (i = MShare.g_DropedItemList.Count - 1; i >= 0; i--)
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
                //@ Unsupported function or procedure: 'Dispose'
                Dispose(dropItem);
                MShare.g_DropedItemList.RemoveAt(i);
                break;
            }
        }
    }

    public void BeginScene()
    {
        int i;
        int k;
        bool movetick;
        TClEvent evn;
        TActor actor;
        TMagicEff meff;
        long tick;
        tick = MShare.GetTickCount();
        if ((MShare.g_MySelf == null))
        {
            return;
        }
        MShare.g_boDoFastFadeOut = false;
        movetick = false;
        if (MShare.g_boSpeedRate)
        {
            // move speed
            if (tick - _mDwMoveTime >= ((long)95 - MShare.g_MoveSpeedRate / 2))
            {
                // move speed
                _mDwMoveTime = tick;
                movetick = true;
            }
        }
        else
        {
            if (tick - _mDwMoveTime >= 95)
            {
                _mDwMoveTime = tick;
                movetick = true;
            }
        }
        if (tick - _mDwAniTime >= 150)
        {
            // 活动素材
            _mDwAniTime = tick;
            _mNAniCount++;
            if (_mNAniCount > 100000)
            {
                _mNAniCount = 0;
            }
        }
        try
        {
            i = 0;
            while (true)
            {
                // DYNAMIC MODE
                if (i >= MActorList.Count)
                {
                    break;
                }
                actor = MActorList[i];
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
                    actor.ProcMsg();
                    if (movetick)
                    {
                        if (actor.Move())
                        {
                            MBoPlayChange = MBoPlayChange;
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
                // dogz....
                if (actor.m_nWaitForRecogId != 0)
                {
                    if (actor.IsIdle())
                    {
                        ClFunc.Units.ClFunc.DelChangeFace(actor.m_nWaitForRecogId);
                        NewActor(actor.m_nWaitForRecogId, actor.m_nCurrX, actor.m_nCurrY, actor.m_btDir, actor.m_nWaitForFeature, actor.m_nWaitForStatus);
                        actor.m_nWaitForRecogId = 0;
                        actor.m_boDelActor = true;
                    }
                }
                if (actor.m_boDelActor)
                {
                    MShare.g_FreeActorList.Add(actor);
                    MActorList.RemoveAt(i);
                    if (MShare.g_TargetCret == actor)
                    {
                        MShare.g_TargetCret = null;
                    }
                    if (MShare.g_FocusCret == actor)
                    {
                        MShare.g_FocusCret = null;
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
        }
        catch (Exception e)
        {
            ClMain.DebugOutStr("101 " + e.Message);
        }
        MBoPlayChange = MBoPlayChange || (MShare.GetTickCount() > MDwPlayChangeTick);
        try
        {
            // STATIC MODE
            i = 0;
            while (true)
            {
                if (i >= MEffectList.Count)
                {
                    break;
                }
                meff = MEffectList[i];
                if (meff.m_boActive)
                {
                    if (!meff.Run())
                    {
                        meff.Free;
                        // 1003
                        MEffectList.RemoveAt(i);
                        continue;
                    }
                }
                i++;
            }
            i = 0;
            while (true)
            {
                if (i >= MFlyList.Count)
                {
                    break;
                }
                meff = MFlyList[i];
                if (meff.m_boActive)
                {
                    if (!meff.Run())
                    {
                        meff.Free;
                        MFlyList.RemoveAt(i);
                        continue;
                    }
                }
                i++;
            }
            ClMain.EventMan.Execute();
            if ((MShare.g_RareBoxWindow != null))
            {
                if (MShare.g_RareBoxWindow.m_boActive)
                {
                    if (!MShare.g_RareBoxWindow.Run())
                    {
                        MShare.g_RareBoxWindow.m_boActive = false;
                    }
                }
            }
        }
        catch
        {
            ClMain.DebugOutStr("102");
        }
        try
        {
            ClearDropItemA();
            // 释放事件的地方
            if (ClMain.EventMan.EventList.Count > 0)
            {
                for (k = 0; k < ClMain.EventMan.EventList.Count; k++)
                {
                    evn = ((TClEvent)(ClMain.EventMan.EventList[k]));
                    if ((Math.Abs(evn.m_nX - MShare.g_MySelf.m_nCurrX) > 16) || (Math.Abs(evn.m_nY - MShare.g_MySelf.m_nCurrY) > 16))
                    {
                        evn.Free;
                        ClMain.EventMan.EventList.RemoveAt(k);
                        break;
                    }
                }
            }
        }
        catch
        {
            ClMain.DebugOutStr("103");
        }
        try
        {
            // with Map.m_ClientRect do
            // begin
            // Left := g_MySelf.m_nRx - g_TileMapOffSetX;
            // Right := g_MySelf.m_nRx + g_TileMapOffSetX;
            // Top := g_MySelf.m_nRy - g_TileMapOffSetY;
            // Bottom := g_MySelf.m_nRy + g_TileMapOffSetY;
            // end;
            Rectangle wvar1 = ClMain.Map.m_ClientRect;
            switch (MShare.g_FScreenMode)
            {
                case 0:
                    // 分辨率修改
                    // 地图的范围是玩家为中心，例如左右各9
                    // 800x600模式
                    wvar1.Left = MShare.g_MySelf.m_nRx - 10;
                    wvar1.Top = MShare.g_MySelf.m_nRy - 12;
                    wvar1.Right = MShare.g_MySelf.m_nRx + 10;
                    wvar1.Bottom = MShare.g_MySelf.m_nRy + 12;
                    break;
                case 1:
                    // 1024 X 768
                    wvar1.Left = MShare.g_MySelf.m_nRx - 13;
                    wvar1.Top = MShare.g_MySelf.m_nRy - 15;
                    wvar1.Right = MShare.g_MySelf.m_nRx + 13;
                    wvar1.Bottom = MShare.g_MySelf.m_nRy + 13;
                    break;
                case 2:
                    // 1280 X 800
                    wvar1.Left = MShare.g_MySelf.m_nRx - 13 - 2;
                    // 左
                    wvar1.Top = MShare.g_MySelf.m_nRy - 15;
                    // 上
                    wvar1.Right = MShare.g_MySelf.m_nRx + 13 + 3;
                    // 右
                    wvar1.Bottom = MShare.g_MySelf.m_nRy + 13 + 3;
                    break;
                case 3:
                    // 下
                    // 1280 X 1024
                    wvar1.Left = MShare.g_MySelf.m_nRx - 13 - 2;
                    wvar1.Top = MShare.g_MySelf.m_nRy - 15 - 4;
                    wvar1.Right = MShare.g_MySelf.m_nRx + 13 + 3;
                    wvar1.Bottom = MShare.g_MySelf.m_nRy + 13 + 4;
                    break;
                case 4:
                    // 1366 X 768
                    wvar1.Left = MShare.g_MySelf.m_nRx - 13 - 3;
                    wvar1.Top = MShare.g_MySelf.m_nRy - 15;
                    wvar1.Right = MShare.g_MySelf.m_nRx + 13 + 4;
                    wvar1.Bottom = MShare.g_MySelf.m_nRy + 13;
                    break;
                case 5:
                    // 1440 X 900
                    wvar1.Left = MShare.g_MySelf.m_nRx - 13 - 4;
                    wvar1.Top = MShare.g_MySelf.m_nRy - 15 - 1;
                    wvar1.Right = MShare.g_MySelf.m_nRx + 13 + 5;
                    wvar1.Bottom = MShare.g_MySelf.m_nRy + 13 + 2;
                    break;
                case 6:
                    // 1600 X 900
                    wvar1.Left = MShare.g_MySelf.m_nRx - 13 - 6;
                    wvar1.Top = MShare.g_MySelf.m_nRy - 15 - 1;
                    wvar1.Right = MShare.g_MySelf.m_nRx + 13 + 6;
                    wvar1.Bottom = MShare.g_MySelf.m_nRy + 13 + 2;
                    break;
                case 7:
                    // 1680 X 1050
                    wvar1.Left = MShare.g_MySelf.m_nRx - 13 - 7;
                    wvar1.Top = MShare.g_MySelf.m_nRy - 15 - 4;
                    wvar1.Right = MShare.g_MySelf.m_nRx + 13 + 5;
                    wvar1.Bottom = MShare.g_MySelf.m_nRy + 13 + 5;
                    break;
                case 8:
                    // 1920 X 1080
                    wvar1.Left = MShare.g_MySelf.m_nRx - 13 - 9;
                    wvar1.Top = MShare.g_MySelf.m_nRy - 15 - 5;
                    wvar1.Right = MShare.g_MySelf.m_nRx + 13 + 9;
                    wvar1.Bottom = MShare.g_MySelf.m_nRy + 13 + 6;
                    break;
                default:
                    // 9: begin  //1024 X 700
                    // Left := g_MySelf.m_nRx - 13;
                    // Top := g_MySelf.m_nRy - 14;
                    // Right := g_MySelf.m_nRx + 13;
                    // Bottom := g_MySelf.m_nRy + 13;
                    // end;
                    // 默认
                    wvar1.Left = MShare.g_MySelf.m_nRx - 13;
                    wvar1.Top = MShare.g_MySelf.m_nRy - 15;
                    wvar1.Right = MShare.g_MySelf.m_nRx + 13;
                    wvar1.Bottom = MShare.g_MySelf.m_nRy + 13;
                    break;
            }
            ClMain.Map.UpdateMapPos(MShare.g_MySelf.m_nRx, MShare.g_MySelf.m_nRy);
        }
        catch (Exception e)
        {
            ClMain.DebugOutStr("104 " + e.Message);
        }
    }

    public override void PlayScene()
    {

    }

    public void PlaySurface(Object sender)
    {
       
    }

    public void MagicSurface(Object sender)
    {
        int k;
        TMagicEff meff;
        MMagSurface.Draw(Units.PlayScn.SOFFX, Units.PlayScn.SOFFY, MObjSurface.ClientRect, MObjSurface, false);
        for (k = 0; k < MEffectList.Count; k++)
        {
            meff = ((TMagicEff)(MEffectList[k]));
            meff.DrawEff(MObjSurface);
        }
    }

    public void NewMagic(TActor aowner, int magid, int magnumb, int cx, int cy, int tx, int ty, int targetCode, MagicType mtype, bool recusion, int anitime, ref bool boFly, int maglv, int poison)
    {
       
    }

    public void DelMagic(int magid)
    {
        int i;
        for (i = 0; i < MEffectList.Count; i++)
        {
            if (((TMagicEff)(MEffectList[i])).ServerMagicId == magid)
            {
                ((TMagicEff)(MEffectList[i])).Free;
                MEffectList.RemoveAt(i);
                break;
            }
        }
    }

    public TMagicEff NewFlyObject(TActor aowner, int cx, int cy, int tx, int ty, int targetCode, TMagicType mtype)
    {
        TMagicEff result;
        int scx;
        int scy;
        int sctx;
        int scty;
        TMagicEff meff;
        ScreenXYfromMcxy(cx, cy, ref scx, ref scy);
        ScreenXYfromMcxy(tx, ty, ref sctx, ref scty);
        switch (mtype)
        {
            case magiceff.TMagicType.mtFlyArrow:
                meff = new TFlyingArrow(1, 1, scx, scy, sctx, scty, mtype, true, 0);
                break;
            case magiceff.TMagicType.mtFlyBug:
                meff = new TFlyingFireBall(1, 1, scx, scy, sctx, scty, mtype, true, 0);
                break;
            case magiceff.TMagicType.mtFireBall:
                meff = new TFlyingBug(1, 1, scx, scy, sctx, scty, mtype, true, 0);
                break;
            default:
                meff = new TFlyingAxe(1, 1, scx, scy, sctx, scty, mtype, true, 0);
                break;
        }
        meff.TargetRx = tx;
        meff.TargetRy = ty;
        meff.TargetActor = FindActor(targetCode);
        meff.MagOwner = aowner;
        MFlyList.Add(meff);
        result = meff;
        return result;
    }

    // procedure TPlayScene.ScreenXYfromMCXY(cx, cy: Integer; var sX, sY: Integer);
    // begin
    // if g_MySelf = nil then Exit;
    // sX := (cx - g_MySelf.m_nRx) * UNITX - g_MySelf.m_nShiftX + SCREENWIDTH div 2;
    // sY := (cy - g_MySelf.m_nRy) * UNITY - g_MySelf.m_nShiftY + ShiftYOffset;
    // end;
    // procedure TPlayScene.CXYfromMouseXY(mx, my: Integer; var ccx, ccy: Integer);
    // begin
    // if g_MySelf = nil then Exit;
    // ccx := Round((mx - SCREENWIDTH div 2 + g_MySelf.m_nShiftX) / UNITX) + g_MySelf.m_nRx;
    // ccy := Round((my - ShiftYOffset + g_MySelf.m_nShiftY) / UNITY) + g_MySelf.m_nRy;
    // end;
    public void ScreenXYfromMcxy(int cx, int cy, ref int sx, ref int sy)
    {
        if (MShare.g_MySelf == null)
        {
            return;
        }
        switch (MShare.g_FScreenWidth)
        {
            case 800:
                // {$IF Var_Interface = Var_Mir2}
                // 宽度 计算方式 : (屏幕宽度高- 低)  div 2 + 364
                // + 12
                sx = (cx - MShare.g_MySelf.m_nRx) * Grobal2.UNITX + 364 - 8 + Grobal2.UNITX / 2 - MShare.g_MySelf.m_nShiftX;
                break;
            case 1024:
                sx = (cx - MShare.g_MySelf.m_nRx) * Grobal2.UNITX + 476 - 4 + Grobal2.UNITX / 2 - MShare.g_MySelf.m_nShiftX;
                break;
            case 1280:
                // + 12
                sx = (cx - MShare.g_MySelf.m_nRx) * Grobal2.UNITX + 604 - 8 + Grobal2.UNITX / 2 - MShare.g_MySelf.m_nShiftX;
                break;
            case 1366:
                // + 16
                sx = (cx - MShare.g_MySelf.m_nRx) * Grobal2.UNITX + 647 - 10 + Grobal2.UNITX / 2 - MShare.g_MySelf.m_nShiftX;
                break;
            case 1440:
                sx = (cx - MShare.g_MySelf.m_nRx) * Grobal2.UNITX + 684 - 12 + Grobal2.UNITX / 2 - MShare.g_MySelf.m_nShiftX;
                break;
            case 1600:
                sx = (cx - MShare.g_MySelf.m_nRx) * Grobal2.UNITX + 764 - 10 + Grobal2.UNITX / 2 - MShare.g_MySelf.m_nShiftX;
                break;
            case 1680:
                sx = (cx - MShare.g_MySelf.m_nRx) * Grobal2.UNITX + 804 - 8 + Grobal2.UNITX / 2 - MShare.g_MySelf.m_nShiftX;
                break;
            case 1920:
                sx = (cx - MShare.g_MySelf.m_nRx) * Grobal2.UNITX + 924 - 16 + Grobal2.UNITX / 2 - MShare.g_MySelf.m_nShiftX;
                break;
            default:
                // 2560: sx := (cx - g_MySelf.m_nRx) * UNITX + 1244 - 20  + UNITX div 2 - g_MySelf.m_nShiftX;
                // + 12
                sx = (cx - MShare.g_MySelf.m_nRx) * Grobal2.UNITX + 364 - 8 + Grobal2.UNITX / 2 - MShare.g_MySelf.m_nShiftX;
                break;
        }
        switch (MShare.g_FScreenHeight)
        {
            case 600:
                // 高度 计算方式 : (屏幕高度高- 低)  div 2 + 224 + 12
                sy = (cy - MShare.g_MySelf.m_nRy) * Grobal2.UNITY + 224 + Grobal2.UNITY / 2 - MShare.g_MySelf.m_nShiftY;
                break;
            case 768:
                // 700: sy := (cy - g_MySelf.m_nRy) * UNITY + 286 + UNITY div 2 - g_MySelf.m_nShiftY;
                sy = (cy - MShare.g_MySelf.m_nRy) * Grobal2.UNITY + 320 + 2 + Grobal2.UNITY / 2 - MShare.g_MySelf.m_nShiftY;
                break;
            case 800:
                sy = (cy - MShare.g_MySelf.m_nRy) * Grobal2.UNITY + 336 - 14 + Grobal2.UNITY / 2 - MShare.g_MySelf.m_nShiftY;
                break;
            case 900:
                sy = (cy - MShare.g_MySelf.m_nRy) * Grobal2.UNITY + 386 - 32 + Grobal2.UNITY / 2 - MShare.g_MySelf.m_nShiftY;
                break;
            case 1024:
                sy = (cy - MShare.g_MySelf.m_nRy) * Grobal2.UNITY + 448 + 2 + Grobal2.UNITY / 2 - MShare.g_MySelf.m_nShiftY;
                break;
            case 1050:
                sy = (cy - MShare.g_MySelf.m_nRy) * Grobal2.UNITY + 461 - 12 + Grobal2.UNITY / 2 - MShare.g_MySelf.m_nShiftY;
                break;
            case 1080:
                sy = (cy - MShare.g_MySelf.m_nRy) * Grobal2.UNITY + 476 + 6 + Grobal2.UNITY / 2 - MShare.g_MySelf.m_nShiftY;
                break;
            default:
                // 1200: sy := (cy - g_MySelf.m_nRy) * UNITY + 536 + UNITY div 2 - g_MySelf.m_nShiftY;
                // 1600: sy := (cy - g_MySelf.m_nRy) * UNITY + 736 + UNITY div 2 - g_MySelf.m_nShiftY;
                sy = (cy - MShare.g_MySelf.m_nRy) * Grobal2.UNITY + 224 + Grobal2.UNITY / 2 - MShare.g_MySelf.m_nShiftY;
                break;
        }
        // {$ELSE}
        // case g_FScreenWidth of     //宽度 计算方式 : (屏幕宽度高- 低)  div 2 + 364
        // 800: sx := (cx - g_MySelf.m_nRx) * UNITX + 364 + 12 + UNITX div 2 - g_MySelf.m_nShiftX;
        // 1024: sx := (cx - g_MySelf.m_nRx) * UNITX + 476 - 4  + UNITX div 2 - g_MySelf.m_nShiftX;
        // 1280: sx := (cx - g_MySelf.m_nRx) * UNITX + 604 + 12 + UNITX div 2 - g_MySelf.m_nShiftX;
        // 1366: sx := (cx - g_MySelf.m_nRx) * UNITX + 647 + 16 + UNITX div 2 - g_MySelf.m_nShiftX;
        // 1440: sx := (cx - g_MySelf.m_nRx) * UNITX + 684 - 20 + UNITX div 2 - g_MySelf.m_nShiftX;
        // 1600: sx := (cx - g_MySelf.m_nRx) * UNITX + 764 - 4  + UNITX div 2 - g_MySelf.m_nShiftX;
        // 1680: sx := (cx - g_MySelf.m_nRx) * UNITX + 804 + 4  + UNITX div 2 - g_MySelf.m_nShiftX;
        // 1920: sx := (cx - g_MySelf.m_nRx) * UNITX + 924 - 20 + UNITX div 2 - g_MySelf.m_nShiftX;
        // //2560: sx := (cx - g_MySelf.m_nRx) * UNITX + 1244 - 20  + UNITX div 2 - g_MySelf.m_nShiftX;
        // else
        // sx := (cx - g_MySelf.m_nRx) * UNITX + 364 + 12 + UNITX div 2 - g_MySelf.m_nShiftX;
        // end;
        // case g_FScreenHeight of  //高度 计算方式 : (屏幕高度高- 低)  div 2 + 288 + 12
        // 600: sy := (cy - g_MySelf.m_nRy) * UNITY + 288 + UNITY div 2 - g_MySelf.m_nShiftY;
        // //700: sy := (cy - g_MySelf.m_nRy) * UNITY + 350 + 4 + UNITY div 2 - g_MySelf.m_nShiftY;
        // 768: sy := (cy - g_MySelf.m_nRy) * UNITY + 384 + 2 + UNITY div 2 - g_MySelf.m_nShiftY;
        // 800: sy := (cy - g_MySelf.m_nRy) * UNITY + 400 - 14 + UNITY div 2 - g_MySelf.m_nShiftY;
        // 900: sy := (cy - g_MySelf.m_nRy) * UNITY + 450 - 32 + UNITY div 2 - g_MySelf.m_nShiftY;
        // 1024: sy := (cy - g_MySelf.m_nRy) * UNITY + 512 + 2 + UNITY div 2 - g_MySelf.m_nShiftY;
        // 1050: sy := (cy - g_MySelf.m_nRy) * UNITY + 525 - 12 + UNITY div 2 - g_MySelf.m_nShiftY;
        // 1080: sy := (cy - g_MySelf.m_nRy) * UNITY + 540 + 6 + UNITY div 2 - g_MySelf.m_nShiftY;
        // //    1200: sy := (cy - g_MySelf.m_nRy) * UNITY + 600 + UNITY div 2 - g_MySelf.m_nShiftY;
        // //    1600: sy := (cy - g_MySelf.m_nRy) * UNITY + 800 + UNITY div 2 - g_MySelf.m_nShiftY;
        // else
        // sy := (cy - g_MySelf.m_nRy) * UNITY + 288 + UNITY div 2 - g_MySelf.m_nShiftY;
        // end;
        // {$IFEND}

    }

    // 屏幕座标 mx, my转换成ccx, ccy地图座标
    public void CxYfromMouseXy(int mx, int my, ref int ccx, ref int ccy)
    {
        if (MShare.g_MySelf == null)
        {
            return;
        }
        switch (MShare.g_FScreenWidth)
        {
            case 800:
                // {$IF Var_Interface = Var_Mir2}
                // 宽度 计算方式 : (屏幕宽度高- 低)  div 2 + 364
                // - 12
                ccx = Math.Round((mx - 364 + 8 + MShare.g_MySelf.m_nShiftX - Grobal2.UNITX / 2) / Grobal2.UNITX) + MShare.g_MySelf.m_nRx;
                break;
            case 1024:
                ccx = Math.Round((mx - 476 + 4 + MShare.g_MySelf.m_nShiftX - Grobal2.UNITX / 2) / Grobal2.UNITX) + MShare.g_MySelf.m_nRx;
                break;
            case 1280:
                // - 12
                ccx = Math.Round((mx - 604 + 8 + MShare.g_MySelf.m_nShiftX - Grobal2.UNITX / 2) / Grobal2.UNITX) + MShare.g_MySelf.m_nRx;
                break;
            case 1366:
                // - 16
                ccx = Math.Round((mx - 657 + 10 + MShare.g_MySelf.m_nShiftX - Grobal2.UNITX / 2) / Grobal2.UNITX) + MShare.g_MySelf.m_nRx;
                break;
            case 1440:
                ccx = Math.Round((mx - 684 + 12 + MShare.g_MySelf.m_nShiftX - Grobal2.UNITX / 2) / Grobal2.UNITX) + MShare.g_MySelf.m_nRx;
                break;
            case 1600:
                ccx = Math.Round((mx - 764 + 10 + MShare.g_MySelf.m_nShiftX - Grobal2.UNITX / 2) / Grobal2.UNITX) + MShare.g_MySelf.m_nRx;
                break;
            case 1680:
                ccx = Math.Round((mx - 804 + 8 + MShare.g_MySelf.m_nShiftX - Grobal2.UNITX / 2) / Grobal2.UNITX) + MShare.g_MySelf.m_nRx;
                break;
            case 1920:
                ccx = Math.Round((mx - 924 + 16 + MShare.g_MySelf.m_nShiftX - Grobal2.UNITX / 2) / Grobal2.UNITX) + MShare.g_MySelf.m_nRx;
                break;
            default:
                // 2560: ccx := Round((mx - 1244 + 20 + g_MySelf.m_nShiftX - UNITX div 2) / UNITX) + g_MySelf.m_nRx;
                // - 12
                ccx = Math.Round((mx - 364 + 8 + MShare.g_MySelf.m_nShiftX - Grobal2.UNITX / 2) / Grobal2.UNITX) + MShare.g_MySelf.m_nRx;
                break;
        }
        switch (MShare.g_FScreenHeight)
        {
            case 600:
                // 高度 计算方式 : (屏幕高度高- 低)  div 2 + 224 + 12
                ccy = Math.Round((my - 224 + MShare.g_MySelf.m_nShiftY - Grobal2.UNITY / 2) / Grobal2.UNITY) + MShare.g_MySelf.m_nRy;
                break;
            case 768:
                // 700: ccy := Round((my - 286 + g_MySelf.m_nShiftY - UNITY div 2) / UNITY) + g_MySelf.m_nRy;
                ccy = Math.Round((my - 320 - 2 + MShare.g_MySelf.m_nShiftY - Grobal2.UNITY / 2) / Grobal2.UNITY) + MShare.g_MySelf.m_nRy;
                break;
            case 800:
                ccy = Math.Round((my - 336 + 14 + MShare.g_MySelf.m_nShiftY - Grobal2.UNITY / 2) / Grobal2.UNITY) + MShare.g_MySelf.m_nRy;
                break;
            case 900:
                ccy = Math.Round((my - 386 + 32 + MShare.g_MySelf.m_nShiftY - Grobal2.UNITY / 2) / Grobal2.UNITY) + MShare.g_MySelf.m_nRy;
                break;
            case 1024:
                ccy = Math.Round((my - 448 - 2 + MShare.g_MySelf.m_nShiftY - Grobal2.UNITY / 2) / Grobal2.UNITY) + MShare.g_MySelf.m_nRy;
                break;
            case 1050:
                ccy = Math.Round((my - 461 + 12 + MShare.g_MySelf.m_nShiftY - Grobal2.UNITY / 2) / Grobal2.UNITY) + MShare.g_MySelf.m_nRy;
                break;
            case 1080:
                ccy = Math.Round((my - 476 - 6 + MShare.g_MySelf.m_nShiftY - Grobal2.UNITY / 2) / Grobal2.UNITY) + MShare.g_MySelf.m_nRy;
                break;
            default:
                // 1200: ccy := Round((my - 636 + g_MySelf.m_nShiftY - UNITY div 2) / UNITY) + g_MySelf.m_nRy;
                // 1600: ccy := Round((my - 736 + g_MySelf.m_nShiftY - UNITY div 2) / UNITY) + g_MySelf.m_nRy;
                ccy = Math.Round((my - 224 + MShare.g_MySelf.m_nShiftY - Grobal2.UNITY / 2) / Grobal2.UNITY) + MShare.g_MySelf.m_nRy;
                break;
        }
        // {$ELSE}
        // case g_FScreenWidth of     //宽度 计算方式 : (屏幕宽度高- 低)  div 2 + 364
        // 800: ccx := Round((mx - 364 - 12 + g_MySelf.m_nShiftX - UNITX div 2) / UNITX) + g_MySelf.m_nRx;
        // 1024: ccx := Round((mx - 476 + 4  + g_MySelf.m_nShiftX - UNITX div 2) / UNITX) + g_MySelf.m_nRx;
        // 1280: ccx := Round((mx - 604 - 12 + g_MySelf.m_nShiftX - UNITX div 2) / UNITX) + g_MySelf.m_nRx;
        // 1366: ccx := Round((mx - 657 - 16 + g_MySelf.m_nShiftX - UNITX div 2) / UNITX) + g_MySelf.m_nRx;
        // 1440: ccx := Round((mx - 684 + 20 + g_MySelf.m_nShiftX - UNITX div 2) / UNITX) + g_MySelf.m_nRx;
        // 1600: ccx := Round((mx - 764 + 4  + g_MySelf.m_nShiftX - UNITX div 2) / UNITX) + g_MySelf.m_nRx;
        // 1680: ccx := Round((mx - 804 - 4  + g_MySelf.m_nShiftX - UNITX div 2) / UNITX) + g_MySelf.m_nRx;
        // 1920: ccx := Round((mx - 924 + 20 + g_MySelf.m_nShiftX - UNITX div 2) / UNITX) + g_MySelf.m_nRx;
        // //    2560: ccx := Round((mx - 1244 + 20 + g_MySelf.m_nShiftX - UNITX div 2) / UNITX) + g_MySelf.m_nRx;
        // else
        // ccx := Round((mx - 364 - 12 + g_MySelf.m_nShiftX - UNITX div 2) / UNITX) + g_MySelf.m_nRx;
        // end;
        // case g_FScreenHeight of  //高度 计算方式 : (屏幕高度高- 低)  div 2 - 288
        // 600: ccy := Round((my - 288 + g_MySelf.m_nShiftY - UNITY div 2) / UNITY) + g_MySelf.m_nRy;
        // //700: ccy := Round((my - 350 - 4 + g_MySelf.m_nShiftY - UNITY div 2) / UNITY) + g_MySelf.m_nRy;
        // 768: ccy := Round((my - 384 - 2  + g_MySelf.m_nShiftY - UNITY div 2) / UNITY) + g_MySelf.m_nRy;
        // 800: ccy := Round((my - 400 + 14 + g_MySelf.m_nShiftY - UNITY div 2) / UNITY) + g_MySelf.m_nRy;
        // 900: ccy := Round((my - 450 + 32 + g_MySelf.m_nShiftY - UNITY div 2) / UNITY) + g_MySelf.m_nRy;
        // 1024: ccy := Round((my - 512 - 2  + g_MySelf.m_nShiftY - UNITY div 2) / UNITY) + g_MySelf.m_nRy;
        // 1050: ccy := Round((my - 525 + 12 + g_MySelf.m_nShiftY - UNITY div 2) / UNITY) + g_MySelf.m_nRy;
        // 1080: ccy := Round((my - 540 - 6  + g_MySelf.m_nShiftY - UNITY div 2) / UNITY) + g_MySelf.m_nRy;
        // //    1200: ccy := Round((my - 600 + g_MySelf.m_nShiftY - UNITY div 2) / UNITY) + g_MySelf.m_nRy;
        // //    1600: ccy := Round((my - 800 + g_MySelf.m_nShiftY - UNITY div 2) / UNITY) + g_MySelf.m_nRy;
        // else
        // ccy := Round((my - 288 + g_MySelf.m_nShiftY - UNITY div 2) / UNITY) + g_MySelf.m_nRy;
        // end;
        // {$IFEND}

    }

    public TActor GetCharacter(int x, int y, int wantsel, ref int nowsel, bool liveonly)
    {
        TActor result;
        int k;
        int i;
        int ccx;
        int ccy;
        int dx;
        int dy;
        TActor a;
        result = null;
        nowsel = -1;
        CxYfromMouseXy(x, y, ref ccx, ref ccy);
        for (k = ccy + 8; k >= ccy - 1; k--)
        {
            for (i = MActorList.Count - 1; i >= 0; i--)
            {
                if (((TActor)(MActorList[i])) != MShare.g_MySelf)
                {
                    a = ((TActor)(MActorList[i]));
                    if ((!liveonly || !a.m_boDeath) && (a.m_boHoldPlace) && (a.m_boVisible))
                    {
                        if (a.m_nCurrY == k)
                        {
                            dx = (a.m_nRx - ClMain.Map.m_ClientRect.Left) * Grobal2.UNITX + _mNDefXx + a.m_nPx + a.m_nShiftX;
                            dy = (a.m_nRy - ClMain.Map.m_ClientRect.Top - 1) * Grobal2.UNITY + _mNDefYy + a.m_nPy + a.m_nShiftY;
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
        TActor result;
        int k;
        int i;
        int ccx;
        int ccy;
        int dx;
        int dy;
        int centx;
        int centy;
        TActor a;
        result = GetCharacter(x, y, wantsel, ref nowsel, liveonly);
        if (result == null)
        {
            nowsel = -1;
            CxYfromMouseXy(x, y, ref ccx, ref ccy);
            for (k = ccy + 8; k >= ccy - 1; k--)
            {
                for (i = MActorList.Count - 1; i >= 0; i--)
                {
                    if (((TActor)(MActorList[i])) != MShare.g_MySelf)
                    {
                        a = ((TActor)(MActorList[i]));
                        if ((!liveonly || !a.m_boDeath) && (a.m_boHoldPlace) && (a.m_boVisible))
                        {
                            if (a.m_nCurrY == k)
                            {
                                dx = (a.m_nRx - ClMain.Map.m_ClientRect.Left) * Grobal2.UNITX + _mNDefXx + a.m_nPx + a.m_nShiftX;
                                dy = (a.m_nRy - ClMain.Map.m_ClientRect.Top - 1) * Grobal2.UNITY + _mNDefYy + a.m_nPy + a.m_nShiftY;
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
        bool result;
        int k;
        int ccx;
        int ccy;
        int dx;
        int dy;
        result = false;
        CxYfromMouseXy(x, y, ref ccx, ref ccy);
        for (k = ccy + 2; k >= ccy - 1; k--)
        {
            if (MShare.g_MySelf.m_nCurrY == k)
            {
                dx = (MShare.g_MySelf.m_nRx - ClMain.Map.m_ClientRect.Left) * Grobal2.UNITX + _mNDefXx + MShare.g_MySelf.m_nPx + MShare.g_MySelf.m_nShiftX;
                dy = (MShare.g_MySelf.m_nRy - ClMain.Map.m_ClientRect.Top - 1) * Grobal2.UNITY + _mNDefYy + MShare.g_MySelf.m_nPy + MShare.g_MySelf.m_nShiftY;
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
        TDropItem result;
        int i;
        int ccx;
        int ccy;
        int ssx;
        int ssy;
        TDropItem dropItem;
        result = null;
        CxYfromMouseXy(x, y, ref ccx, ref ccy);
        ScreenXYfromMcxy(ccx, ccy, ref ssx, ref ssy);
        inames = "";
        for (i = 0; i < MShare.g_DropedItemList.Count; i++)
        {
            dropItem = ((TDropItem)(MShare.g_DropedItemList[i]));
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
        int i;
        TDropItem dropItem;
        for (i = 0; i < MShare.g_DropedItemList.Count; i++)
        {
            dropItem = MShare.g_DropedItemList[i];
            if ((dropItem.X == nX) && (dropItem.Y == nY))
            {
                itemList.Add(dropItem);
            }
        }
    }

    public TDropItem GetXyDropItems(int nX, int nY)
    {
        TDropItem result;
        int i;
        TDropItem dropItem;
        result = null;
        for (i = 0; i < MShare.g_DropedItemList.Count; i++)
        {
            dropItem = MShare.g_DropedItemList[i];
            if ((dropItem.X == nX) && (dropItem.Y == nY))
            {
                result = dropItem;
                // if not g_gcGeneral[7] or DropItem.boShowName then
                // Break;
                // not g_gcGeneral[7] or
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
        int ndir;
        int rx;
        int ry;
        ndir = ClFunc.Units.ClFunc.GetNextDirection(sX, sY, ex, ey);
        rx = sX;
        ry = sY;
        ClFunc.Units.ClFunc.GetNextPosXY(ndir, ref rx, ref ry);
        // if Map.CanMove(rx, ry) and Map.CanMove(ex, ey) then
        // Result := True
        // else begin
        // Result := False;
        // end;
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
        bool result;
        result = false;
        if (ClMain.Map.CanMove(mx, my))
        {
            result = !CrashManEx(mx, my);
        }
        return result;
    }

    private bool CrashManEx(int mx, int my)
    {
        bool result;
        int i;
        TActor actor;
        result = false;
        for (i = 0; i < MActorList.Count; i++)
        {
            actor = ((TActor)(MActorList[i]));
            if (actor == MShare.g_MySelf)
            {
                continue;
            }
            if ((actor.m_boVisible) && (actor.m_boHoldPlace) && (!actor.m_boDeath) && (actor.m_nCurrX == mx) && (actor.m_nCurrY == my))
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
                    if (((actor.m_btRace > Grobal2.RCC_USERHUMAN) && (actor.m_btRace != Grobal2.RCC_MERCHANT)) && (MShare.g_boCanRunMon || MShare.g_boCanRunSafeZone))
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
        bool result;
        result = false;
        if (ClMain.Map.CanMove(mx, my))
        {
            result = !CrashMan(mx, my);
        }
        return result;
    }

    public bool CrashMan(int mx, int my)
    {
        bool result;
        int i;
        TActor a;
        result = false;
        for (i = 0; i < MActorList.Count; i++)
        {
            a = ((TActor)(MActorList[i]));
            if (a == MShare.g_MySelf)
            {
                continue;
            }
            if ((a.m_boVisible) && (a.m_boHoldPlace) && (!a.m_boDeath) && (a.m_nCurrX == mx) && (a.m_nCurrY == my))
            {
                result = true;
                break;
            }
        }
        return result;
    }

    // function TPlayScene.CrashManPath(mx, my: Integer): Boolean;
    // var
    // i                         : Integer;
    // a                         : TActor;
    // begin
    // Result := False;
    // for i := 0 to m_ActorList.count - 1 do begin
    // a := TActor(m_ActorList[i]);
    // if a = g_MySelf then Continue;
    // if (a.m_boVisible) and (a.m_boHoldPlace) and (not a.m_boDeath) and (a.m_nCurrX = mx) and (a.m_nCurrY = my) then begin
    // Result := True;
    // Break;
    // end;
    // end;
    // end;
    public bool CanFly(int mx, int my)
    {
        bool result;
        result = ClMain.Map.CanFly(mx, my);
        return result;
    }

    // ------------------------ Actor ------------------------
    public TActor FindActor(int id)
    {
        TActor result;
        int i;
        result = null;
        if (id == 0)
        {
            return result;
        }
        for (i = 0; i < MActorList.Count; i++)
        {
            if (((TActor)(MActorList[i])).m_nRecogId == id)
            {
                result = ((TActor)(MActorList[i]));
                break;
            }
        }
        return result;
    }

    public TActor FindActor(string sName)
    {
        TActor result;
        int i;
        TActor actor;
        result = null;
        for (i = 0; i < MActorList.Count; i++)
        {
            actor = ((TActor)(MActorList[i]));
            if ((actor.m_sUserName).ToLower().CompareTo((sName).ToLower()) == 0)
            {
                result = actor;
                break;
            }
        }
        return result;
    }

    public TActor FindActorXy(int x, int y)
    {
        TActor result;
        int i;
        TActor a;
        result = null;
        for (i = 0; i < MActorList.Count; i++)
        {
            a = ((TActor)(MActorList[i]));
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
        bool result;
        int i;
        result = false;
        for (i = 0; i < MActorList.Count; i++)
        {
            if (((TActor)(MActorList[i])) == actor)
            {
                result = true;
                break;
            }
        }
        return result;
    }

    public TActor NewActor(int chrid, short cx, short cy, short cdir, int cfeature, int cstate)
    {
        TActor result;
        // race, hair, dress, weapon
        int i;
        TActor actor;
        result = null;
        for (i = 0; i < MActorList.Count; i++)
        {
            if (((TActor)(MActorList[i])).m_nRecogId == chrid)
            {
                result = ((TActor)(MActorList[i]));
                // if is my hero then ???
                return result;
            }
        }
        if (ClFunc.Units.ClFunc.IsChangingFace(chrid))
        {
            return result;
        }
        switch (Grobal2.RACEfeature(cfeature))
        {
            case 0:
                actor = new THumActor();
                break;
            case 9:
                // 人物
                actor = new TSoccerBall();
                break;
            case 13:
                // 足球
                actor = new TKillingHerb();
                break;
            case 14:
                // 食人花
                actor = new TSkeletonOma();
                break;
            case 15:
                // 骷髅
                actor = new TDualAxeOma();
                break;
            case 16:
                // 掷斧骷髅
                actor = new TGasKuDeGi();
                break;
            case 17:
                // 洞蛆
                actor = new TCatMon();
                break;
            case 18:
                // 钩爪猫
                actor = new THuSuABi();
                break;
            case 19:
                // 稻草人
                actor = new TCatMon();
                break;
            case 20:
                // 沃玛战士
                actor = new TFireCowFaceMon();
                break;
            case 21:
                // 火焰沃玛
                actor = new TCowFaceKing();
                break;
            case 22:
                // 沃玛教主
                actor = new TDualAxeOma();
                break;
            case 23:
                // 黑暗战士
                actor = new TWhiteSkeleton();
                break;
            case 24:
                // 变异骷髅
                actor = new TSuperiorGuard();
                break;
            case 25:
                // 带刀卫士
                actor = new TKingOfSculpureKingMon();
                break;
            case 26:
                actor = new TKingOfSculpureKingMon();
                break;
            case 27:
                actor = new TSnowMon();
                break;
            case 28:
                actor = new TSnowMon();
                break;
            case 29:
                actor = new TSnowMon();
                break;
            case 30:
                actor = new TCatMon();
                break;
            case 31:
                // 朝俺窿
                actor = new TCatMon();
                break;
            case 32:
                // 角蝇
                actor = new TScorpionMon();
                break;
            case 33:
                // 蝎子
                actor = new TCentipedeKingMon();
                break;
            case 34:
                // 触龙神
                actor = new TBigHeartMon();
                break;
            case 35:
                // 赤月恶魔
                actor = new TSpiderHouseMon();
                break;
            case 36:
                // 幻影蜘蛛
                actor = new TExplosionSpider();
                break;
            case 37:
                // 月魔蜘蛛
                actor = new TFlyingSpider();
                break;
            case 38:
                actor = new TSnowMon();
                break;
            case 39:
                actor = new TSnowMon();
                break;
            case 40:
                actor = new TZombiLighting();
                break;
            case 41:
                // 僵尸1
                actor = new TZombiDigOut();
                break;
            case 42:
                // 僵尸2
                actor = new TZombiZilkin();
                break;
            case 43:
                // 僵尸3
                actor = new TBeeQueen();
                break;
            case 44:
                // 角蝇巢
                actor = new TSnowMon();
                break;
            case 45:
                actor = new TArcherMon();
                break;
            case 46:
                // 弓箭手
                actor = new TSnowMon();
                break;
            case 47:
                actor = new TSculptureMon();
                break;
            case 48:
                // 祖玛雕像
                actor = new TSculptureMon();
                break;
            case 49:
                actor = new TSculptureKingMon();
                break;
            case 50:
                // 祖玛教主
                actor = new TNpcActor();
                break;
            case 51:
                actor = new TSnowMon();
                break;
            case 52:
                actor = new TGasKuDeGi();
                break;
            case 53:
                // 楔蛾
                actor = new TGasKuDeGi();
                break;
            case 54:
                // 粪虫
                actor = new TSmallElfMonster();
                break;
            case 55:
                // 神兽
                actor = new TWarriorElfMonster();
                break;
            case 56:
                // 神兽1
                actor = new TAngel();
                break;
            case 57:
                actor = new TDualAxeOma();
                break;
            case 58:
                // 1234
                actor = new TDualAxeOma();
                break;
            case 60:
                // 1234
                actor = new TElectronicScolpionMon();
                break;
            case 61:
                actor = new TBossPigMon();
                break;
            case 62:
                actor = new TKingOfSculpureKingMon();
                break;
            case 63:
                actor = new TSkeletonKingMon();
                break;
            case 64:
                actor = new TGasKuDeGi();
                break;
            case 65:
                actor = new TSamuraiMon();
                break;
            case 66:
                actor = new TSkeletonSoldierMon();
                break;
            case 67:
                actor = new TSkeletonSoldierMon();
                break;
            case 68:
                actor = new TSkeletonSoldierMon();
                break;
            case 69:
                actor = new TSkeletonArcherMon();
                break;
            case 70:
                actor = new TBanyaGuardMon();
                break;
            case 71:
                actor = new TBanyaGuardMon();
                break;
            case 72:
                actor = new TBanyaGuardMon();
                break;
            case 73:
                actor = new TPBOMA1Mon();
                break;
            case 74:
                actor = new TCatMon();
                break;
            case 75:
                actor = new TStoneMonster();
                break;
            case 76:
                actor = new TSuperiorGuard();
                break;
            case 77:
                actor = new TStoneMonster();
                break;
            case 78:
                actor = new TBanyaGuardMon();
                break;
            case 79:
                actor = new TPBOMA6Mon();
                break;
            case 80:
                actor = new TMineMon();
                break;
            case 81:
                actor = new TAngel();
                break;
            case 83:
                actor = new TFireDragon();
                break;
            case 84:
                actor = new TDragonStatue();
                break;
            case 87:
                actor = new TDragonStatue();
                break;
            case 90:
                actor = new TDragonBody();
                break;
            case 91:
                // 龙
                actor = new TWhiteSkeleton();
                break;
            case 92:
                // 变异骷髅
                actor = new TWhiteSkeleton();
                break;
            case 93:
                // 变异骷髅
                actor = new TWhiteSkeleton();
                break;
            case 94:
                // 变异骷髅
                actor = new TWarriorElfMonster();
                break;
            case 95:
                // 神兽1
                actor = new TWarriorElfMonster();
                break;
            case 98:
                // 神兽1
                actor = new TWallStructure();
                break;
            case 99:
                // LeftWall
                actor = new TCastleDoor();
                break;
            case 101:
                // MainDoor
                actor = new TBanyaGuardMon();
                break;
            case 102:
                actor = new TKhazardMon();
                break;
            case 103:
                actor = new TFrostTiger();
                break;
            case 104:
                actor = new TRedThunderZuma();
                break;
            case 105:
                actor = new TCrystalSpider();
                break;
            case 106:
                actor = new TYimoogi();
                break;
            case 109:
                actor = new TBlackFox();
                break;
            case 110:
                actor = new TGreenCrystalSpider();
                break;
            case 111:
                actor = new TBanyaGuardMon();
                break;
            case 113:
                // TSpiderKing.Create;
                actor = new TBanyaGuardMon();
                break;
            case 114:
                actor = new TBanyaGuardMon();
                break;
            case 115:
                actor = new TBanyaGuardMon();
                break;
            case 117:
            case 118:
            case 119:
                actor = new TBanyaGuardMon();
                break;
            case 120:
                actor = new TFireDragon();
                break;
            case 121:
                actor = new TTiger();
                break;
            case 122:
                actor = new TDragon();
                break;
            case 123:
                actor = new TGhostShipMonster();
                break;
            default:
                actor = new TActor();
                break;
        }
        actor.m_nRecogId = chrid;
        actor.m_nCurrX = cx;
        actor.m_nCurrY = cy;
        actor.m_nRx = actor.m_nCurrX;
        actor.m_nRy = actor.m_nCurrY;
        actor.m_btDir = cdir;
        actor.m_nFeature = cfeature;
        if (MShare.g_boOpenAutoPlay && MShare.g_gcAss[6])
        {
            actor.m_btAFilter = MShare.g_APMobList.IndexOf(actor.m_sUserName) >= 0;
        }
        actor.m_btRace = Grobal2.RACEfeature(cfeature);
        actor.m_btHair = Grobal2.HAIRfeature(cfeature);
        actor.m_btDress = Grobal2.DRESSfeature(cfeature);
        actor.m_btWeapon = Grobal2.WEAPONfeature(cfeature);
        actor.m_wAppearance = Grobal2.APPRfeature(cfeature);
        // if (m_btRace = 50) and (m_wAppearance in [54..48]) then
        // m_boVisible := False;
        actor.m_Action = null;
        if (actor.m_btRace == 0)
        {
            actor.m_btSex = actor.m_btDress % 2;
            if (actor.m_btDress >= 24 && actor.m_btDress <= 27)
            {
                actor.m_btDress = 18 + actor.m_btSex;
            }
        }
        else
        {
            actor.m_btSex = 0;
        }
        actor.m_nState = cstate;
        actor.m_SayingArr[0] = "";
        MActorList.Add(actor);
        result = actor;
        return result;
    }

    public void ActorDied(Object actor)
    {
        int i;
        bool flag;
        for (i = 0; i < MActorList.Count; i++)
        {
            if (MActorList[i] == actor)
            {
                MActorList.RemoveAt(i);
                break;
            }
        }
        flag = false;
        for (i = 0; i < MActorList.Count; i++)
        {
            if (!((TActor)(MActorList[i])).m_boDeath)
            {
                MActorList.Insert(i, actor);
                flag = true;
                break;
            }
        }
        if (!flag)
        {
            MActorList.Add(actor);
        }
    }

    public void SetActorDrawLevel(Object actor, int level)
    {
        int i;
        if (level == 0)
        {
            for (i = 0; i < MActorList.Count; i++)
            {
                if (MActorList[i] == actor)
                {
                    MActorList.RemoveAt(i);
                    MActorList.Insert(0, actor);
                    break;
                }
            }
        }
    }

    // (*procedure TPlayScene.ClearActors;
    // var
    // i                         : Integer;
    // begin
    // for i := 0 to g_FreeActorList.Count - 1 do
    // TActor(g_FreeActorList[i]).Free;
    // g_FreeActorList.Clear;
    // 
    // for i := 0 to m_ActorList.Count - 1 do
    // TActor(m_ActorList[i]).Free;
    // m_ActorList.Clear;
    // 
    // if g_MySelf  <> nil then begin
    // g_MySelf.m_HeroObject := nil;
    // g_MySelf.m_SlaveObject.Clear;       // := nil;
    // g_MySelf := nil;
    // end;
    // 
    // g_TargetCret := nil;
    // g_FocusCret := nil;
    // g_MagicTarget := nil;
    // for i := 0 to m_EffectList.Count - 1 do
    // TMagicEff(m_EffectList[i]).Free;
    // m_EffectList.Clear;
    // DScreen.ClearHint;
    // end;*)
    // 清除所有角色
    public void ClearActors()
    {
        int i;
        try
        {
            if (MActorList.Count > 0)
            {
                for (i = 0; i < MActorList.Count; i++)
                {
                    ((TActor)(MActorList[i])).Free;
                }
                MActorList.Clear();
            }
            MShare.g_MySelf = null;
            MShare.g_TargetCret = null;
            MShare.g_FocusCret = null;
            MShare.g_MagicTarget = null;
            // 清除魔法效果对象
            if (MEffectList.Count > 0)
            {
                for (i = 0; i < MEffectList.Count; i++)
                {
                    ((TMagicEff)(MEffectList[i])).Free;
                }
                MEffectList.Clear();
            }
        }
        catch
        {
            ClMain.DebugOutStr("TPlayScene.ClearActors");
        }
    }

    public TActor DeleteActor(int id, bool boDeath)
    {
        TActor result;
        int i;
        result = null;
        i = 0;
        while (true)
        {
            if (i >= MActorList.Count)
            {
                break;
            }
            if (((TActor)(MActorList[i])).m_nRecogId == id)
            {
                if (MShare.g_TargetCret == ((TActor)(MActorList[i])))
                {
                    MShare.g_TargetCret = null;
                }
                if (MShare.g_FocusCret == ((TActor)(MActorList[i])))
                {
                    MShare.g_FocusCret = null;
                }
                if (MShare.g_MagicTarget == ((TActor)(MActorList[i])))
                {
                    MShare.g_MagicTarget = null;
                }
                if ((((TActor)(MActorList[i])) == MShare.g_MySelf.m_HeroObject))
                {
                    // TActor(m_ActorList[i]).m_boNotShowHealth := True;
                    if (!boDeath)
                    {
                        break;
                    }
                }
                if (Units.PlayScn.IsMySlaveObject(((TActor)(MActorList[i]))))
                {
                    if (!boDeath)
                    {
                        break;
                    }
                }
                // if (TActor(m_ActorList[i]) = g_MySelf.m_SlaveObject) then begin
                // //TActor(m_ActorList[i]).m_boNotShowHealth := True;
                // if not boDeath then
                // Break;
                // end;
                ((TActor)(MActorList[i])).m_dwDeleteTime = MShare.GetTickCount();
                MShare.g_FreeActorList.Add(MActorList[i]);
                MActorList.RemoveAt(i);
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

    public void DelActor(Object actor)
    {
        int i;
        for (i = 0; i < MActorList.Count; i++)
        {
            if (MActorList[i] == actor)
            {
                ((TActor)(MActorList[i])).m_dwDeleteTime = MShare.GetTickCount();
                MShare.g_FreeActorList.Add(MActorList[i]);
                MActorList.RemoveAt(i);
                break;
            }
        }
    }

    public TActor ButchAnimal(int x, int y)
    {
        TActor result;
        int i;
        TActor a;
        result = null;
        for (i = 0; i < MActorList.Count; i++)
        {
            a = ((TActor)(MActorList[i]));
            // and (a.m_btRace <> 0)
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

    public void SendMsg(int ident, int chrid, int x, int y, int cdir, int feature, int state, string str, int ipInfo)
    {
        TActor actor;
        TMagicEff meff;
        TMessageBodyW mbw;
        switch (ident)
        {
            case Grobal2.SM_CHANGEMAP:
            case Grobal2.SM_NEWMAP:
                ProcMagic.NTargetX = -1;
                ClMain.EventMan.ClearEvents();
                ClMain.g_PathBusy = true;
                try
                {
                    if (ClMain.frmMain.TimerAutoMove.Enabled)
                    {
                        ClMain.frmMain.TimerAutoMove.Enabled = false;
                        MapUnit.Units.MapUnit.g_MapPath = new Point[0];
                        MapUnit.Units.MapUnit.g_MapPath = null;
                        ClMain.DScreen.AddChatBoardString("地图跳转，停止自动移动", ClMain.GetRGB(5), System.Drawing.Color.White);
                    }
                    if (MShare.g_boOpenAutoPlay && ClMain.frmMain.TimerAutoPlay.Enabled)
                    {
                        ClMain.frmMain.TimerAutoPlay.Enabled = false;
                        MShare.g_gcAss[0] = false;
                        MShare.g_APMapPath = new Point[0];
                        MShare.g_APMapPath2 = new Point[0];
                        MShare.g_APStep = -1;
                        MShare.g_APLastPoint.X = -1;
                        ClMain.DScreen.AddChatBoardString("[挂机] 地图跳转，停止自动挂机", System.Drawing.Color.Red, System.Drawing.Color.White);
                    }
                    if ((MShare.g_MySelf != null))
                    {
                        MShare.g_MySelf.m_nTagX = 0;
                        MShare.g_MySelf.m_nTagY = 0;
                    }
                    if (ClMain.Map.m_MapBuf != null)
                    {
                        //@ Unsupported function or procedure: 'FreeMem'
                        FreeMem(ClMain.Map.m_MapBuf);
                        ClMain.Map.m_MapBuf = null;
                    }
                    if (ClMain.Map.m_MapData.Length > 0)
                    {
                        ClMain.Map.m_MapData = new TCellParams[0];
                        ClMain.Map.m_MapData = null;
                    }
                }
                finally
                {
                    ClMain.g_PathBusy = false;
                }
                ClMain.Map.LoadMap(str, x, y);
                if ((ident == Grobal2.SM_NEWMAP) && (MShare.g_MySelf != null))
                {
                    MShare.g_MySelf.m_nCurrX = x;
                    MShare.g_MySelf.m_nCurrY = y;
                    MShare.g_MySelf.m_nRx = x;
                    MShare.g_MySelf.m_nRy = y;
                    DelActor(MShare.g_MySelf);
                }
                if (frmDlg.DWGameConfig.Visible && (frmDlg.DWGameConfig.tag == 5))
                {
                    MShare.g_nApMiniMap = true;
                    ClMain.frmMain.SendWantMiniMap();
                }
                if (MShare.g_boViewMiniMap)
                {
                    MShare.g_nMiniMapIndex = -1;
                    ClMain.frmMain.SendWantMiniMap();
                }
                break;
            case Grobal2.SM_LOGON:
                actor = FindActor(chrid);
                if (actor == null)
                {
                    actor = NewActor(chrid, x, y, Lobyte(cdir), feature, state);
                    actor.m_nChrLight = Hibyte(cdir);
                    cdir = Lobyte(cdir);
                    actor.SendMsg(Grobal2.SM_TURN, x, y, cdir, feature, state, "", 0);
                }
                if (MShare.g_MySelf != null)
                {
                    if (MShare.g_MySelf.m_HeroObject != null)
                    {
                        MShare.g_MySelf.m_HeroObject = null;
                    }
                    MShare.g_MySelf.m_SlaveObject.Clear();
                    MShare.g_MySelf = null;
                }
                MShare.g_MySelf = ((THumActor)(actor));
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
                    if (actor == MShare.g_MySelf.m_HeroObject)
                    {
                        if (!actor.m_boDeath)
                        {
                            return;
                        }
                        DeleteActor(chrid, true);
                        return;
                    }
                    if (Units.PlayScn.IsMySlaveObject(actor))
                    {
                        // if (Actor = g_MySelf.m_SlaveObject) then begin
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
                        actor = NewActor(chrid, x, y, Lobyte(cdir), feature, state);
                    }
                    if (actor != null)
                    {
                        if (ipInfo != 0)
                        {
                            // Actor.m_nIPower := LoWord(IPInfo);
                            //@ Unsupported function or procedure: 'HiWord'
                            actor.m_nIPowerLvl = HiWord(ipInfo);
                        }
                        //@ Unsupported function or procedure: 'Hibyte'
                        actor.m_nChrLight = Hibyte(cdir);
                        cdir = Lobyte(cdir);
                        if (ident == Grobal2.SM_SKELETON)
                        {
                            actor.m_boDeath = true;
                            actor.m_boSkeleton = true;
                        }
                        if (ident == Grobal2.SM_DEATH)
                        {
                            //@ Unsupported function or procedure: 'Hibyte'
                            if (Hibyte(cdir) != 0)
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
                        if (str != "")
                        {
                            EDcode.Units.EDcode.DecodeBuffer(str, mbw);
                            actor.m_btTitleIndex = LoWord(mbw.param1);
                        }
                        else
                        {
                            actor.m_btTitleIndex = 0;
                        }
                        actor.FeatureChanged();
                        break;
                    case Grobal2.SM_APPRCHANGED:
                        break;
                    case Grobal2.SM_CHARSTATUSCHANGED:
                        actor.m_nState = feature;
                        actor.m_nHitSpeed = state;
                        if (str == "1")
                        {
                            meff = new TCharEffect(1110, 10, actor);
                            meff.NextFrameTime = 80;
                            meff.ImgLib = WMFile.Units.WMFile.g_WMagic2Images;
                            ClMain.g_PlayScene.m_EffectList.Add(meff);
                            // PlaySoundName('wav\M1-2.wav');
                        }
                        break;
                    default:
                        if (ident == Grobal2.SM_TURN)
                        {
                            if (str != "")
                            {
                                actor.m_sUserName = str;
                                actor.m_sUserNameOffSet = HGECanvas.Units.HGECanvas.g_DXCanvas.TextWidth(actor.m_sUserName) / 2;
                            }
                        }
                        actor.SendMsg(ident, x, y, cdir, feature, state, "", 0);
                        break;
                }
                break;
        }
    }

}