using System;
using System.Collections;
using System.Drawing;
using SystemModule;

namespace RobotSvr
{
    public class DrawScreen
    {
        private long _mDwFrameTime = 0;
        private long _mDwFrameCount = 0;
        // m_dwDrawFrameCount: LongWord;
        private ArrayList _mSysMsgList = null;
        private ArrayList _mSysMsgListEx = null;
        private ArrayList _mSysMsgListEx2 = null;
        public Scene CurrentScene = null;
        public ArrayList ChatStrs = null;
        public ArrayList ChatBks = null;
        public ArrayList MAdList = null;
        public ArrayList MAdList2 = null;
        public TGList MSmListCnt = null;
        public TGList MSmListCntFree = null;
        public TDxHintMgr MHint1 = null;
        public TDxHintMgr MHint2 = null;
        public TDxHintMgr MHint3 = null;

        public DrawScreen()
        {
            CurrentScene = null;
            _mDwFrameTime = MShare.GetTickCount();
            _mDwFrameCount = 0;
            _mSysMsgList = new ArrayList();
            _mSysMsgListEx = new ArrayList();
            _mSysMsgListEx2 = new ArrayList();
            MSmListCnt = new TGList();
            MSmListCntFree = new TGList();
            ChatStrs = new ArrayList();
            MAdList = new ArrayList();
            MAdList2 = new ArrayList();
            ChatBks = new ArrayList();
            MHint1 = new TDxHintMgr();
            MHint2 = new TDxHintMgr();
            MHint3 = new TDxHintMgr();
        }

        public void Initialize()
        {

        }

        public void Finalize()
        {

        }

        public void KeyPress(ref char key)
        {
            if (CurrentScene != null)
            {
                CurrentScene.KeyPress(ref key);
            }
        }

        public void KeyDown(ref short key, Keys shift)
        {
            if (CurrentScene != null)
            {
                CurrentScene.KeyDown(ref key, shift);
            }
        }

        public void MouseMove(Keys shift, int x, int y)
        {
            if (CurrentScene != null)
            {
                CurrentScene.MouseMove(shift, x, y);
            }
        }

        public void MouseDown(MouseButtons button, Keys shift, int x, int y)
        {
            if (CurrentScene != null)
            {
                CurrentScene.MouseDown(button, shift, x, y);
            }
        }

        public void ChangeScene(SceneType scenetype)
        {
            if (CurrentScene != null)
            {
                CurrentScene.CloseScene();
            }
            switch (scenetype)
            {
                case SceneType.stIntro:
                    CurrentScene = ClMain.IntroScene;
                    ClMain.IntroScene.m_dwStartTime = MShare.GetTickCount() + 2000;
                    break;
                case SceneType.stLogin:
                    CurrentScene = ClMain.LoginScene;
                    break;
                case SceneType.stSelectCountry:
                    break;
                case SceneType.stSelectChr:
                    CurrentScene = ClMain.SelectChrScene;
                    break;
                case SceneType.stNewChr:
                    break;
                case SceneType.stLoading:
                    break;
                case SceneType.stLoginNotice:
                    CurrentScene = ClMain.LoginNoticeScene;
                    break;
                case SceneType.stPlayGame:
                    CurrentScene = ClMain.g_PlayScene;
                    break;
            }
            if (CurrentScene != null)
            {
                CurrentScene.OpenScene();
            }
        }

        public void AddSysMsg(string msg)
        {
            if (_mSysMsgList.Count >= 10)
            {
                _mSysMsgList.Remove(0);
            }
            _mSysMsgList.Add(msg, ((MShare.GetTickCount()) as Object));
        }

        public void AddSysMsgBottom(string msg)
        {
            if (_mSysMsgListEx.Count >= 10)
            {
                _mSysMsgListEx.Remove(0);
            }
            _mSysMsgListEx.Add(msg, ((MShare.GetTickCount()) as Object));
        }

        public void AddSysMsgBottom2(string msg)
        {
            if (_mSysMsgListEx2.Count >= 10)
            {
                _mSysMsgListEx2.Remove(0);
            }
            _mSysMsgListEx2.Add(msg, ((MShare.GetTickCount()) as Object));
        }

        public void AddSysMsgCenter(string msg, Color fc, Color bc, int sec)
        {

        }

        public void AddSysMsgCenter(string msg, int fc, int bc, int sec)
        {
            int i;
            int n;
            int p;
            string s;
            TCenterMsg pm;
            TCenterMsg pmfree;
            if (msg == "")
            {
                return;
            }
            MSmListCnt.__Lock();
            try
            {
                if (MSmListCnt.Count >= 5)
                {
                    MSmListCnt.RemoveAt(0);
                    pm = MSmListCnt[0];
                    pm.dwCloseTick = MShare.GetTickCount();
                    MSmListCntFree.__Lock();
                    try
                    {
                        MSmListCntFree.Add(pm);
                    }
                    finally
                    {
                        MSmListCntFree.UnLock();
                    }
                }
            }
            finally
            {
                MSmListCnt.UnLock();
            }
            // 注释掉释放可解决发送公告导致客户端内核假死
            MSmListCntFree.__Lock();
            try
            {
                for (i = MSmListCntFree.Count - 1; i >= 0; i--)
                {
                    pmfree = MSmListCntFree[i];
                    if ((pmfree.dwCloseTick > 0) && (MShare.GetTickCount() - pmfree.dwCloseTick > 5 * 60 * 1000))
                    {
                        MSmListCntFree.RemoveAt(i);
                        Dispose(pmfree);
                    }
                }
            }
            finally
            {
                MSmListCntFree.UnLock();
            }
            i = 0;
            s = msg;
            n = msg.Length;
            while (true)
            {
                p = s.IndexOf("%");
                if (p > 0)
                {
                    if (p < n)
                    {
                        if ((msg[p + 1] == "t"))
                        {
                            msg[p + 1] = "d";
                        }
                        else
                        {
                            msg[p] = " ";
                            msg[p + 1] = " ";
                        }
                    }
                    else if (p == n)
                    {
                        msg[p] = " ";
                    }
                    s = msg.Substring(p + 1 - 1, n - p);
                }
                else
                {
                    break;
                }
                i++;
                if (i > 10)
                {
                    break;
                }
            }
            if (msg.Trim() != "")
            {
                pm = new TCenterMsg();
                pm.s = msg;
                pm.fc = fc;
                pm.bc = bc;
                pm.dwSec = HUtil32._MAX(1, sec);
                pm.dwNow = MShare.GetTickCount();
                pm.dwCloseTick = 0;
                MSmListCnt.__Lock();
                try
                {
                    MSmListCnt.Add(pm);
                }
                finally
                {
                    MSmListCnt.UnLock();
                }
            }
        }

        public void AddChatBoardString(string str, Color fcolor, Color bcolor)
        {

        }


        public void AddChatBoardString(string str, int fcolor, int bcolor)
        {
            int i;
            int len;
            int aline;
            string temp;
            len = str.Length;
            temp = "";
            i = 1;
            while (true)
            {
                if (i > len)
                {
                    break;
                }
                if (((byte)str[i]) >= 128)
                {
                    temp = temp + str[i];
                    i++;
                    if (i <= len)
                    {
                        temp = temp + str[i];
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    temp = temp + str[i];
                }
                aline = MShare.g_DXCanvas.TextWidth(temp);
                if (aline >= FrmDlg.DImageGridSay.Width)
                {
                    // 换行
                    ChatStrs.Add(temp, ((fcolor) as Object));
                    ChatBks.Add((bcolor as object));
                    str = str.Substring(i + 1 - 1, len - i);
                    temp = "";
                    break;
                }
                i++;
            }
            if (temp != "")
            {
                ChatStrs.Add(temp, ((fcolor) as Object));
                ChatBks.Add((bcolor as object));
                str = "";
            }
            if (ChatStrs.Count > 200)
            {
                ChatStrs.Remove(0);
                ChatBks.RemoveAt(0);
            }
            FrmDlg.DSayUpDown.MaxPosition = HUtil32._MAX(0, ChatStrs.Count - FrmDlg.DImageGridSay.Height / SAYLISTHEIGHT);// 是否少个锁定聊天
            FrmDlg.DSayUpDown.Position = FrmDlg.DSayUpDown.MaxPosition;
            if (str != "")
            {
                AddChatBoardString(" " + str, fcolor, bcolor);
            }
        }

        public int ShowHint(int x, int y, string str, Color color, bool drawup, bool drawLeft, bool bfh, bool lines, byte mgr, bool takeOn)
        {
            int result;
            switch (mgr)
            {
                case 1:
                    result = MHint1.ShowHint(x, y, str, color, drawup, drawLeft, lines, takeOn);
                    break;
                case 2:
                    result = MHint2.ShowHint(x, y, str, color, drawup, drawLeft, lines, takeOn);
                    break;
                case 3:
                    result = MHint3.ShowHint(x, y, str, color, drawup, drawLeft, lines, takeOn);
                    break;
                default:
                    result = MHint1.ShowHint(x, y, str, color, drawup, drawLeft, lines, takeOn);
                    result = result + MHint2.ShowHint(x, y, str, color, drawup, drawLeft, lines, takeOn);
                    result = result + MHint3.ShowHint(x, y, str, color, drawup, drawLeft, lines, takeOn);
                    break;
            }
            return result;
        }

        public void ClearChatBoard()
        {
            int i;
            _mSysMsgList.Clear();
            _mSysMsgListEx.Clear();
            _mSysMsgListEx2.Clear();
            // 注释掉释放可解决发送公告导致客户端内核假死 Development 2019-01-04
            for (i = 0; i < MSmListCnt.Count; i++)
            {
                // DisPose(PTCenterMsg(m_smListCnt[i]));
                MSmListCnt.Clear();
            }
            // //
            for (i = 0; i < MSmListCntFree.Count; i++)
            {
                // DisPose(PTCenterMsg(m_smListCntFree[i]));
                MSmListCntFree.Clear();
            }
            ChatStrs.Clear();
            ChatBks.Clear();
        }

        public void ClearHint()
        {
            //m_Hint1.ClearHint;
            //m_Hint2.ClearHint;
            //m_Hint3.ClearHint;
        }

        public void DrawHint()
        {
            //m_Hint1.DrawHint();
            //m_Hint2.DrawHint();
            //m_Hint3.DrawHint();
        }
    }
}

