using System;

namespace RobotSvr;

public class SelectChrScene: Scene
{
    private Timer _soundTimer = null;
    private bool _createChrMode = false;
    public int NewIndex = 0;
    public SelChar[] ChrArr;
    // -------------------- TSelectChrScene ------------------------
    //Constructor  Create()
    public SelectChrScene() : base(SceneType.StSelectChr)
    {
        _createChrMode = false;
        //@ Unsupported function or procedure: 'FillChar'
        FillChar(ChrArr);            ChrArr[0].FreezeState = true;
        ChrArr[1].FreezeState = true;
        NewIndex = 0;
        _soundTimer = new Timer(ClMain.frmMain.Owner);
        _soundTimer.onTimer = SoundOnTimer;
        _soundTimer.Interval = 1;
        _soundTimer.Enabled = false;
    }
    //@ Destructor  Destroy()
    ~SelectChrScene()
    {
        base.Destroy();
    }
    public override void OpenScene()
    {
        // 进入人物选择场景
                ClMain.HGE.Gfx_Restore(MShare.g_FScreenWidth, MShare.g_FScreenHeight, 32);
                                FrmDlg.DWinSelectChr.Visible = true;
        _soundTimer.Enabled = true;
        _soundTimer.Interval = 100;
    }

    public override void CloseScene()
    {
        // 离开人物选择场景
        SoundUtil.Units.SoundUtil.g_SndMgr.SilenceSound();
                                FrmDlg.DWinSelectChr.Visible = false;
        _soundTimer.Enabled = false;
    }

    // EdChrName: TEdit;
    private void SoundOnTimer(Object sender)
    {
        SoundUtil.Units.SoundUtil.g_SndMgr.PlayBKGSound(SoundUtil.Units.SoundUtil.bmg_select);
        _soundTimer.Enabled = false;
    }

    public void SelChrSelect1Click()
    {
        if ((!ChrArr[0].Selected) && (ChrArr[0].Valid) && ChrArr[0].FreezeState)
        {
            ClMain.frmMain.SelectChr(ChrArr[0].UserChr.Name);
            // 2004/05/17
            ChrArr[0].Selected = true;
            ChrArr[1].Selected = false;
            ChrArr[0].Unfreezing = true;
            ChrArr[0].AniIndex = 0;
            ChrArr[0].DarkLevel = 0;
            ChrArr[0].EffIndex = 0;
            ChrArr[0].StartTime = MShare.GetTickCount();
            ChrArr[0].Moretime = MShare.GetTickCount();
            ChrArr[0].Startefftime = MShare.GetTickCount();
            SoundUtil.Units.SoundUtil.g_SndMgr.PlaySound(SoundUtil.Units.SoundUtil.s_meltstone);
        }
    }

    public void SelChrSelect2Click()
    {
        if ((!ChrArr[1].Selected) && (ChrArr[1].Valid) && ChrArr[1].FreezeState)
        {
            ClMain.frmMain.SelectChr(ChrArr[1].UserChr.Name);
            // 2004/05/17
            ChrArr[1].Selected = true;
            ChrArr[0].Selected = false;
            ChrArr[1].Unfreezing = true;
            ChrArr[1].AniIndex = 0;
            ChrArr[1].DarkLevel = 0;
            ChrArr[1].EffIndex = 0;
            ChrArr[1].StartTime = MShare.GetTickCount();
            ChrArr[1].Moretime = MShare.GetTickCount();
            ChrArr[1].Startefftime = MShare.GetTickCount();
            SoundUtil.Units.SoundUtil.g_SndMgr.PlaySound(SoundUtil.Units.SoundUtil.s_meltstone);
        }
    }

    public void SelChrStartClick()
    {
        // 开始
        string chrname;
        chrname = "";
        if (ChrArr[0].Valid && ChrArr[0].Selected)
        {
            chrname = ChrArr[0].UserChr.Name;
        }
        if (ChrArr[1].Valid && ChrArr[1].Selected)
        {
            chrname = ChrArr[1].UserChr.Name;
        }
        if (chrname != "")
        {
            if (!MShare.g_boDoFadeOut && !MShare.g_boDoFadeIn)
            {
                MShare.g_boDoFastFadeOut = true;
                MShare.g_nFadeIndex = 29;
            }
            ClMain.frmMain.SendSelChr(chrname);
        }
        else
        {
                                    FrmDlg.DMessageDlg("开始游戏前你应该先创建一个新角色！\\点击<创建角色>按钮创建一个游戏角色。", new object[] {MessageBoxButtons.OK});
        }
    }

    public void SelChrNewChrClick()
    {
        if (!ChrArr[0].Valid || !ChrArr[1].Valid)
        {
            if (!ChrArr[0].Valid)
            {
                MakeNewChar(0);
            }
            else
            {
                MakeNewChar(1);
            }
        }
        else
        {
                                    FrmDlg.DMessageDlg("一个帐号最多只能创建 2 个游戏角色！", new object[] {MessageBoxButtons.OK});
        }
    }

    public void SelChrEraseChrClick()
    {
        int n;
        n = 0;
        if (ChrArr[0].Valid && ChrArr[0].Selected)
        {
            n = 0;
        }
        if (ChrArr[1].Valid && ChrArr[1].Selected)
        {
            n = 1;
        }
        if ((ChrArr[n].Valid) && (!ChrArr[n].FreezeState) && (ChrArr[n].UserChr.Name != ""))
        {
                                    if (System.Windows.Forms.DialogResult.Yes == FrmDlg.DMessageDlg("\"" + ChrArr[n].UserChr.Name + "\" 是否确认删除此游戏角色？", new object[] {MessageBoxButtons.YesNo, MessageBoxButtons.YesNo}))
            {
                ClMain.frmMain.SendDelChr(ChrArr[n].UserChr.Name);
            }
        }
    }

    public void SelChrCreditsClick()
    {
        // [失败] 没有找到被删除的角色。
        // [失败] 客户端版本错误。
        // [失败] 你没有这个角色。
        // [失败] 角色已被删除。
        // [失败] 角色数据读取失败，请稍候再试。
        // [失败] 你选择的服务器用户满员。
        // if not ChrArr[0].Valid or not ChrArr[1].Valid then begin
        // if not ChrArr[0].Valid then
        // MakeNewChar(0)
        // else
        // MakeNewChar(1);
        // end else
        // FrmDlg.DMessageDlg('一个帐号最多只能创建二个游戏角色！', [mbOk]);

    }

    public void SelChrExitClick()
    {
        ClMain.frmMain.Close();
    }

    public void ClearChrs()
    {
        //@ Unsupported function or procedure: 'FillChar'
        FillChar(ChrArr);            ChrArr[0].FreezeState = false;
        ChrArr[1].FreezeState = true;
        ChrArr[0].Selected = true;
        ChrArr[1].Selected = false;
        ChrArr[0].UserChr.Name = "";
        ChrArr[1].UserChr.Name = "";
    }

    public void AddChr(string uname, int job, int hair, int level, int sex)
    {
        int n;
        if (!ChrArr[0].Valid)
        {
            n = 0;
        }
        else if (!ChrArr[1].Valid)
        {
            n = 1;
        }
        else
        {
            return;
        }
        ChrArr[n].UserChr.Name = uname;
        ChrArr[n].UserChr.Job = job;
        ChrArr[n].UserChr.hair = hair;
        ChrArr[n].UserChr.Level = level;
        ChrArr[n].UserChr.Sex = sex;
        ChrArr[n].Valid = true;
    }

    private void MakeNewChar(int index)
    {
        _createChrMode = true;
        NewIndex = index;
        if (index == 0)
        {
                                                FrmDlg.DCreateChr.Left = 415;
                                                FrmDlg.DCreateChr.Top = 15;
        }
        else
        {
                                                FrmDlg.DCreateChr.Left = 75;
                                                FrmDlg.DCreateChr.Top = 15;
        }
                                FrmDlg.DCreateChr.Visible = true;
        ChrArr[NewIndex].Valid = true;
        ChrArr[NewIndex].FreezeState = false;
                                FrmDlg.DEditChrName.SetFocus;
        SelectChr(NewIndex);
        //@ Unsupported function or procedure: 'FillChar'
        FillChar(ChrArr[NewIndex].UserChr);        }

    public void SelectChr(int index)
    {
        ChrArr[index].Selected = true;
        ChrArr[index].DarkLevel = 30;
        ChrArr[index].StartTime = MShare.GetTickCount();
        ChrArr[index].Moretime = MShare.GetTickCount();
        if (index == 0)
        {
            ChrArr[1].Selected = false;
        }
        else
        {
            ChrArr[0].Selected = false;
        }
    }

    public void SelChrNewClose()
    {
        ChrArr[NewIndex].Valid = false;
        _createChrMode = false;
                                FrmDlg.DCreateChr.Visible = false;
        ChrArr[NewIndex].Selected = true;
        ChrArr[NewIndex].FreezeState = false;
    }

    public void SelChrNewOk()
    {
        string chrname;
        string shair;
        string sjob;
        string ssex;
                                chrname = FrmDlg.DEditChrName.Text.Trim();
        if (chrname != "")
        {
            ChrArr[NewIndex].Valid = false;
            _createChrMode = false;
                                                FrmDlg.DCreateChr.Visible = false;
            ChrArr[NewIndex].Selected = true;
            ChrArr[NewIndex].FreezeState = false;
            shair = (1 + (new System.Random(5)).Next()).ToString();
            sjob = (ChrArr[NewIndex].UserChr.Job).ToString();
            ssex = (ChrArr[NewIndex].UserChr.Sex).ToString();
            ClMain.frmMain.SendNewChr(ClMain.frmMain.LoginID, chrname, shair, sjob, ssex);
        }
    }

    public void SelChrNewJob(int job)
    {
        if ((job >= 0 && job<= 2) && (ChrArr[NewIndex].UserChr.Job != job))
        {
            ChrArr[NewIndex].UserChr.Job = job;
            SelectChr(NewIndex);
        }
    }

    public void SelChrNewm_btSex(int sex)
    {
        if (sex != ChrArr[NewIndex].UserChr.Sex)
        {
            ChrArr[NewIndex].UserChr.Sex = sex;
            SelectChr(NewIndex);
        }
    }

    public void SelChrNewPrevHair()
    {
    }

    public void SelChrNewNextHair()
    {
    }

    public override void PlayScene(TDirectDrawSurface mSurface)
    {
        int n;
        int bx;
        int by;
        int fx;
        int fy;
        int img;
        int ex;
        int ey;
        // 选择人物时显示的效果光位置
        TDirectDrawSurface d;
        TDirectDrawSurface e;
        TDirectDrawSurface dd;
        string svname;
        if (MShare.g_boOpenAutoPlay && (MShare.g_nAPReLogon == 2))
        {
            // 0613
            if (MShare.GetTickCount() - MShare.g_nAPReLogonWaitTick > MShare.g_nAPReLogonWaitTime)
            {
                MShare.g_nAPReLogonWaitTick = MShare.GetTickCount();
                MShare.g_nAPReLogon = 3;
                if (!MShare.g_boDoFadeOut && !MShare.g_boDoFadeIn)
                {
                    MShare.g_boDoFastFadeOut = true;
                    MShare.g_nFadeIndex = 29;
                }
                // frmMain.SendSelChr(frmMain.m_sCharName);
            }
        }
        bx = 0;
        by = 0;
        fx = 0;
        fy = 0;
        // 选择人物背景
        d = WMFile.Units.WMFile.g_WMain3Images.Images[400];
        if (d != null)
        {
                                                            mSurface.Draw((MShare.g_FScreenWidth - d.Width) / 2, (MShare.g_FScreenHeight - d.Height) / 2, d.ClientRect, d, true);
        }
        // Tips.dat
        for (n = 0; n <= 1; n ++ )
        {
            if (ChrArr[n].Valid)
            {
                // 90
                ex = (MShare.g_FScreenWidth - 800) / 2 + 90;
                // 60-2
                ey = (MShare.g_FScreenHeight - 600) / 2 + 60 - 2;
                switch(ChrArr[n].UserChr.Job)
                {
                    case 0:
                        if (ChrArr[n].UserChr.Sex == 0)
                        {
                            // 71
                            bx = (MShare.g_FScreenWidth - 800) / 2 + 71;
                            // 75-23
                            by = (MShare.g_FScreenHeight - 600) / 2 + 75 - 23;
                            fx = bx;
                            fy = by;
                        }
                        else
                        {
                            // 65
                            bx = (MShare.g_FScreenWidth - 800) / 2 + 65;
                            by = (MShare.g_FScreenHeight - 600) / 2 + 75 - 2 - 18;
                            fx = bx - 28 + 28;
                            fy = by - 16 + 16;
                        }
                        break;
                    case 1:
                        if (ChrArr[n].UserChr.Sex == 0)
                        {
                            // 77
                            bx = (MShare.g_FScreenWidth - 800) / 2 + 77;
                            by = (MShare.g_FScreenHeight - 600) / 2 + 75 - 29;
                            fx = bx;
                            fy = by;
                        }
                        else
                        {
                            bx = (MShare.g_FScreenWidth - 800) / 2 + 141 + 30;
                            by = (MShare.g_FScreenHeight - 600) / 2 + 85 + 14 - 2;
                            fx = bx - 30;
                            fy = by - 14;
                        }
                        break;
                    case 2:
                        if (ChrArr[n].UserChr.Sex == 0)
                        {
                            bx = (MShare.g_FScreenWidth - 800) / 2 + 85;
                            by = (MShare.g_FScreenHeight - 600) / 2 + 75 - 12;
                            fx = bx;
                            fy = by;
                        }
                        else
                        {
                            bx = (MShare.g_FScreenWidth - 800) / 2 + 141 + 23;
                            by = (MShare.g_FScreenHeight - 600) / 2 + 85 + 20 - 2;
                            fx = bx - 23;
                            fy = by - 20;
                        }
                        break;
                }
                if (n == 1)
                {
                    ex = (MShare.g_FScreenWidth - 800) / 2 + 430;
                    ey = (MShare.g_FScreenHeight - 600) / 2 + 60;
                    bx = bx + 340;
                    by = by + 2;
                    fx = fx + 340;
                    fy = fy + 2;
                }
                if (ChrArr[n].Unfreezing)
                {
                    img = 140 - 80 + ChrArr[n].UserChr.Job * 40 + ChrArr[n].UserChr.Sex * 120;
                    d = WMFile.Units.WMFile.g_WChrSel.Images[img + ChrArr[n].AniIndex];
                    e = WMFile.Units.WMFile.g_WChrSel.Images[4 + ChrArr[n].EffIndex];
                    if (d != null)
                    {
                                                                        mSurface.Draw(bx, by, d.ClientRect, d, true);
                    }
                    if (e != null)
                    {
                        cliUtil.Units.cliUtil.DrawBlend(mSurface, ex, ey, e, 1);
                    }
                    if (MShare.GetTickCount() - ChrArr[n].StartTime > 110)
                    {
                        ChrArr[n].StartTime = MShare.GetTickCount();
                        ChrArr[n].AniIndex = ChrArr[n].AniIndex + 1;
                    }
                    if (MShare.GetTickCount() - ChrArr[n].Startefftime > 110)
                    {
                        ChrArr[n].Startefftime = MShare.GetTickCount();
                        ChrArr[n].EffIndex = ChrArr[n].EffIndex + 1;
                    }
                    if (ChrArr[n].AniIndex > Units.IntroScn.FREEZEFRAME - 1)
                    {
                        ChrArr[n].Unfreezing = false;
                        ChrArr[n].FreezeState = false;
                        ChrArr[n].AniIndex = 0;
                    }
                }
                else if (!ChrArr[n].Selected && (!ChrArr[n].FreezeState && !ChrArr[n].Freezing))
                {
                    ChrArr[n].Freezing = true;
                    ChrArr[n].AniIndex = 0;
                    ChrArr[n].StartTime = MShare.GetTickCount();
                }
                if (ChrArr[n].Freezing)
                {
                    img = 140 - 80 + ChrArr[n].UserChr.Job * 40 + ChrArr[n].UserChr.Sex * 120;
                    d = WMFile.Units.WMFile.g_WChrSel.Images[img + Units.IntroScn.FREEZEFRAME - ChrArr[n].AniIndex - 1];
                    if (d != null)
                    {
                                                                        mSurface.Draw(bx, by, d.ClientRect, d, true);
                    }
                    if (MShare.GetTickCount() - ChrArr[n].StartTime > 110)
                    {
                        ChrArr[n].StartTime = MShare.GetTickCount();
                        ChrArr[n].AniIndex = ChrArr[n].AniIndex + 1;
                    }
                    if (ChrArr[n].AniIndex > Units.IntroScn.FREEZEFRAME - 1)
                    {
                        ChrArr[n].Freezing = false;
                        ChrArr[n].FreezeState = true;
                        ChrArr[n].AniIndex = 0;
                    }
                }
                if (!ChrArr[n].Unfreezing && !ChrArr[n].Freezing)
                {
                    if (!ChrArr[n].FreezeState)
                    {
                        img = 120 - 80 + ChrArr[n].UserChr.Job * 40 + ChrArr[n].AniIndex + ChrArr[n].UserChr.Sex * 120;
                        d = WMFile.Units.WMFile.g_WChrSel.Images[img];
                        if (d != null)
                        {
                            // if ChrArr[n].DarkLevel > 0 then begin
                            // //              dd := TDirectDrawSurface.Create(frmMain.DXDraw.DDraw);
                            // //              dd.SetSize(d.Width, d.Height);
                            // //              dd.Draw(0, 0, d.ClientRect, d, False);
                            // MakeDark( 30 - ChrArr[n].DarkLevel);
                            // //              MSurface.Draw(fx, fy, dd.ClientRect, dd, True);
                            // //              dd.free;
                            // end else
                                                                                    mSurface.Draw(fx, fy, d.ClientRect, d, true);
                        }
                    }
                    else
                    {
                        img = 140 - 80 + ChrArr[n].UserChr.Job * 40 + ChrArr[n].UserChr.Sex * 120;
                        d = WMFile.Units.WMFile.g_WChrSel.Images[img];
                        if (d != null)
                        {
                                                                                    mSurface.Draw(bx, by, d.ClientRect, d, true);
                        }
                    }
                    if (ChrArr[n].Selected)
                    {
                        if (MShare.GetTickCount() - ChrArr[n].StartTime > 230)
                        {
                            ChrArr[n].StartTime = MShare.GetTickCount();
                            ChrArr[n].AniIndex = ChrArr[n].AniIndex + 1;
                            if (ChrArr[n].AniIndex > Units.IntroScn.SELECTEDFRAME - 1)
                            {
                                ChrArr[n].AniIndex = 0;
                            }
                        }
                        if (MShare.GetTickCount() - ChrArr[n].Moretime > 25)
                        {
                            ChrArr[n].Moretime = MShare.GetTickCount();
                            if (ChrArr[n].DarkLevel > 0)
                            {
                                ChrArr[n].DarkLevel = ChrArr[n].DarkLevel - 1;
                            }
                        }
                    }
                }
            }
        }
    }

}