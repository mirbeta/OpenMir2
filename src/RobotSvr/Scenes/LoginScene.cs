using System;

namespace RobotSvr
{
    public class LoginScene : Scene
    {
        private int _mNCurFrame = 0;
        private int _mNMaxFrame = 0;
        private long _mDwStartTime = 0;
        private bool _mBoOpenFirst = false;
        private TUserEntry _mNewIdRetryUe = null;
        private TUserEntryAdd _mNewIdRetryAdd = null;
        public string MSLoginId = String.Empty;
        public string MSLoginPasswd = String.Empty;
        public long MEditIdHandle = 0;
        public object MEditIdPointer = null;
        public long MEditPassHandle = 0;
        public object MEditPassPointer = null;

        public LoginScene() : base(SceneType.StLogin)
        {
            object p;
            int nX;
            int nY;
            _mBoOpenFirst = false;
        }

        // 进入登录界面
        public override void OpenScene()
        {
            _mNCurFrame = 0;
            _mNMaxFrame = 10;
            MSLoginId = "";
            MSLoginPasswd = "";
            _mBoOpenFirst = true;
        }

        // 离开登录界面
        public override void CloseScene()
        {

        }

        public void PassWdFail()
        {
            FrmDlg.DEditLoginAccount.SetFocus;
        }

        public void HideLoginBox()
        {
            ChangeLoginState(LoginState.LsCloseAll);
        }

        public void OpenLoginDoor()
        {
            _mDwStartTime = MShare.GetTickCount();
            HideLoginBox();
        }

        public override void PlayScene()
        {
            if (_mBoOpenFirst)
            {
                _mBoOpenFirst = false;
            }
        }

        public void ChangeLoginState(LoginState state)
        {
            int i;
            TfrmMain wvar1 = ClMain.frmMain;
            switch (state)
            {
                case LoginState.LsLogin:
                    wvar1.FrmDlg.DWinNewAccount.Visible = false;
                    wvar1.FrmDlg.DWinChgPw.Visible = false;
                    wvar1.FrmDlg.DWinLogin.Visible = true;
                    if (wvar1.FrmDlg.DEditLoginAccount.Visible)
                    {
                        wvar1.FrmDlg.DEditLoginAccount.SetFocus;
                    }
                    break;
                case LoginState.LsNewidRetry:
                case LoginState.LsNewid:
                    if (wvar1.FrmDlg.DEditNewID.Visible && wvar1.FrmDlg.DEditNewID.Enabled)
                    {
                        wvar1.FrmDlg.DEditNewID.SetFocus;
                    }
                    else
                    {
                        if (wvar1.FrmDlg.DEditNewPassWordConfirm.Visible && wvar1.FrmDlg.DEditNewPassWordConfirm.Enabled)
                        {
                            wvar1.FrmDlg.DEditNewPassWordConfirm.SetFocus;
                        }
                    }
                    break;
                case LoginState.LsChgpw:
                    wvar1.FrmDlg.DWinNewAccount.Visible = false;
                    wvar1.FrmDlg.DWinChgPw.Visible = true;
                    wvar1.FrmDlg.DWinLogin.Visible = false;
                    if (wvar1.FrmDlg.DEditChgID.Visible)
                    {
                        wvar1.FrmDlg.DEditChgID.SetFocus;
                    }
                    break;
                case LoginState.LsCloseAll:
                    wvar1.FrmDlg.DWinNewAccount.Visible = false;
                    wvar1.FrmDlg.DWinChgPw.Visible = false;
                    wvar1.FrmDlg.DWinLogin.Visible = false;
                    break;
            }
        }

        public void NewClick()
        {
            FrmDlg.NewAccountTitle = "";
            ChangeLoginState(LoginState.LsNewid);
        }

        public void NewIdRetry(bool boupdate)
        {
            ChangeLoginState(LoginState.LsNewidRetry);
            FrmDlg.DEditNewID.Text = _mNewIdRetryUe.sAccount;
            FrmDlg.DEditNewPassWord.Text = _mNewIdRetryUe.sPassword;
            FrmDlg.DEditNewYourName.Text = _mNewIdRetryUe.sUserName;
            FrmDlg.DEditNewIDcard.Text = _mNewIdRetryUe.sSSNo;
            FrmDlg.DEditNewQuiz1.Text = _mNewIdRetryUe.sQuiz;
            FrmDlg.DEditNewAnswer1.Text = _mNewIdRetryUe.sAnswer;
            FrmDlg.DEditNewPhone.Text = _mNewIdRetryUe.sPhone;
            FrmDlg.DEditNewEMail.Text = _mNewIdRetryUe.sEMail;
            FrmDlg.DEditNewQuiz2.Text = _mNewIdRetryAdd.sQuiz2;
            FrmDlg.DEditNewAnswer2.Text = _mNewIdRetryAdd.sAnswer2;
            FrmDlg.DEditNewMobPhone.Text = _mNewIdRetryAdd.sMobilePhone;
            FrmDlg.DEditNewBirthDay.Text = _mNewIdRetryAdd.sBirthDay;
        }

        public void UpdateAccountInfos(TUserEntry ue)
        {
            _mNewIdRetryUe = ue;
            //@ Unsupported function or procedure: 'FillChar'
            FillChar(_mNewIdRetryAdd);            // m_boUpdateAccountMode := True;
            NewIdRetry(true);
            FrmDlg.NewAccountTitle = "(请填写帐号相关信息)";
        }

        public void OkClick()
        {
            char key;
            key = '\r';
            FrmDlg.DEditLoginPassWordKeyPress(FrmDlg.DEditLoginPassWord, key);
        }

        public void ChgPwClick()
        {
            ChangeLoginState(LoginState.LsChgpw);
        }

        private bool CheckUserEntrys()
        {
            bool result;
            result = true;
            return result;
        }

        public void NewAccountOk()
        {
            TUserEntry ue;
            TUserEntryAdd ua;
            if (CheckUserEntrys())
            {
                //@ Unsupported function or procedure: 'FillChar'
                FillChar(ue);                //@ Unsupported function or procedure: 'FillChar'
                FillChar(ua); ue.sAccount = FrmDlg.DEditNewID.Text.ToLower();
                ue.sPassword = FrmDlg.DEditNewPassWord.Text;
                ue.sUserName = FrmDlg.DEditNewYourName.Text;
                ue.sSSNo = FrmDlg.DEditNewIDcard.Text;
                ue.sQuiz = FrmDlg.DEditNewQuiz1.Text;
                ue.sAnswer = FrmDlg.DEditNewAnswer1.Text.Trim();
                ue.sPhone = FrmDlg.DEditNewPhone.Text;
                ue.sEMail = FrmDlg.DEditNewEMail.Text.Trim();
                ua.sQuiz2 = FrmDlg.DEditNewQuiz1.Text;
                ua.sAnswer2 = FrmDlg.DEditNewAnswer1.Text.Trim();
                ua.sBirthDay = FrmDlg.DEditNewBirthDay.Text;
                ua.sMobilePhone = FrmDlg.DEditNewMobPhone.Text;
                _mNewIdRetryUe = ue;
                _mNewIdRetryUe.sAccount = "";
                _mNewIdRetryUe.sPassword = "";
                _mNewIdRetryAdd = ua;
                // 发送注册帐号信息
                ClMain.frmMain.SendNewAccount(ue, ua);
                NewAccountClose();
            }
        }

        public void NewAccountClose()
        {
            // if not m_boUpdateAccountMode then
            ChangeLoginState(LoginState.LsLogin);
        }

        public void ChgpwOk()
        {
            string uid;
            string passwd;
            string newpasswd;
            if (FrmDlg.DEditChgNewPw.Text == FrmDlg.DEditChgNewPwRepeat.Text)
            {
                uid = FrmDlg.DEditChgID.Text;
                passwd = FrmDlg.DEditChgCurrentpw.Text;
                newpasswd = FrmDlg.DEditChgNewPw.Text;
                ClMain.frmMain.SendChgPw(uid, passwd, newpasswd);
                ChgpwCancel();
            }
            else
            {
                FrmDlg.DMessageDlg("二次输入的密码不匹配！！！。", new object[] { MessageBoxButtons.OK });
                FrmDlg.DEditChgNewPw.SetFocus;
            }
        }

        public void ChgpwCancel()
        {
            ChangeLoginState(LoginState.LsLogin);
        }

    }
}