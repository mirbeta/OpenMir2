using System;
using SystemModule;
using SystemModule.Packages;

namespace SelGate
{
    public class TSessionObj
    {
        //public _tagUserObj m_pUserOBJ = null;
        //public _tagIOCPCommObj m_pOverlapRecv = null;
        //public _tagIOCPCommObj m_pOverlapSend = null;
        //public TIOCPWriter m_tIOCPSender = null;
        //public TClientThread m_tLastGameSvr = null;
        private UserSession userSession;
        public bool m_fKickFlag = false;
        public byte m_fHandleLogin = 0;
        public long m_dwSessionID = 0;
        public int m_nSvrListIdx = 0;
        public int m_nSvrObject = 0;
        public short m_wRandKey = 0;
        public long m_dwClientTimeOutTick = 0;
        public int m_status = 0;
        private readonly TConfigMgr configMgr;
        private readonly int sessionId;

        public TSessionObj()
        {
            sessionId = HUtil32.Sequence();
            m_fKickFlag = false;
            m_nSvrObject = 0;
            m_dwClientTimeOutTick = HUtil32.GetTickCount();
            m_fHandleLogin = 0;
            m_nSvrListIdx = 0;
            m_status = 0;
        }

        public void SendDefMessage(ushort wIdent, int nRecog, ushort nParam, ushort nTag, ushort nSeries, string sMsg)
        {
            //int iLen;
            //TCmdPack Cmd;
            //byte[] TempBuf = new byte[2048];
            //byte[] SendBuf = new byte[2048];
            //string sAnsiCharMsg = sMsg;
            //if ((m_tLastGameSvr == null) || !m_tLastGameSvr.Active)
            //{
            //    return;
            //}
            //Cmd = new TCmdPack();
            //Cmd.Recog = nRecog;
            //Cmd.Ident = wIdent;
            //Cmd.Param = nParam;
            //Cmd.Tag = nTag;
            //Cmd.Param = nSeries;
            //SendBuf[0] = (byte)'#';
            //TempBuf = Cmd.GetPacket(6);//Move(Cmd, TempBuf[1], sizeof(TCmdPack));
            //// 把Cmd的内容，复制到TempBuf中
            //if (sAnsiCharMsg != "")
            //{
            //    Move(sAnsiCharMsg[1], TempBuf[sizeof(TCmdPack) + 1], sAnsiCharMsg.Length);
            //    // 加密编码TempBuf中的内容，保存到SendBuf中去
            //    iLen = Misc.EncodeBuf(((int)TempBuf[1]), TCmdPack.PackSize + sAnsiCharMsg.Length, ((int)SendBuf[1]));
            //}
            //else
            //{
            //    iLen = Misc.EncodeBuf(((int)TempBuf[1]), TCmdPack.PackSize, ((int)SendBuf[1]));
            //}
            //SendBuf[iLen + 1] = (byte)'!';
            //m_tIOCPSender.SendData(m_pOverlapSend, SendBuf[0], iLen + 2);
        }

        // 发送创建角色，删除角色，恢复角色，创建名字等功能
        public void ProcessCltData(byte[] Addr, int Len, ref bool Succeed, bool fCDPacket)
        {
            int i;
            byte[] nABuf;
            byte[] nBBuf;
            int nBuffer;
            int nNewIDCode;
            int nRand;
            int nDeCodeLen;
            int nEnCodeLen;
            TCmdPack Cmd;
            TCmdPack CltCmd;
            var pszBuf = new byte[1024];
            if (m_fKickFlag)
            {
                // m_fKickFlag为真，表示连接进来的用户已被踢掉，直接反转m_fKickFlag标志，退出
                m_fKickFlag = false;
                Succeed = false;
                return;
            }
            if (Len > configMgr.m_nNomClientPacketSize)
            {
                //if (g_pLogMgr.CheckLevel(4))
                //{
                //    g_pLogMgr.Add("数据包超长: " + (Len).ToString());
                //}
                KickUser(userSession.nIPAddr);
                Succeed = false;
                return;
            }
            //((byte)Addr + Len) = 0;
            var sData = HUtil32.GetString(Addr, 0, Addr.Length);
            if ((Len >= 5) && configMgr.m_fDefenceCCPacket)
            {
                if (sData.IndexOf("HTTP/") > -1)
                {
                    // StrPos返回第二个参数在第一个参数中第一次出现的位置指针
                    //if (g_pLogMgr.CheckLevel(6))
                    //{
                    //    g_pLogMgr.Add("CC Attack, Kick: " + m_pUserOBJ.pszIPAddr);
                    //}
                    KickUser(userSession.nIPAddr);// Kick即英文踢的意思
                    Succeed = false;
                    return;
                }
            }
            if ((Len >= 1))
            {
                if (sData.IndexOf("$")>-1)
                {
                    //if (g_pLogMgr.CheckLevel(6))
                    //{
                    //    g_pLogMgr.Add("$ Attack, Kick: " + m_pUserOBJ.pszIPAddr);
                    //}
                    KickUser(userSession.nIPAddr);
                    Succeed = false;
                    return;
                }
            }
            if (ClientSession.gDeny)
            {
                KickUser(userSession.nIPAddr);
                Succeed = false;
                return;
            }
            // 到这里，m_pOverlapRecv.ABuffer和m_pOverlapRecv.BBuffer，应该已经被赋过值了
            nABuf = Addr;//m_pOverlapRecv.ABuffer;
            // 这时的nABuf是整数，作为内存地址来使用
            nBBuf = Addr;//m_pOverlapRecv.BBuffer;
            nDeCodeLen = Misc.DecodeBuf(Addr, Len, ref nABuf);
            // 把参数Addr的值，进行解密，数据的长度是Len，解密后存到nABuf中
            // nDeCodeLen是实际解密出的长度
            CltCmd = new TCmdPack(nABuf, 12);
            // 把nABuf地址位置中的内容，转换成PCmdPack
            if (m_fHandleLogin == 0)
            {
                switch (CltCmd.Cmd)
                {
                    //case Grobal2.CM_QUERYSELCHARCODE:
                    case Grobal2.CM_QUERYCHR:
                    case Grobal2.CM_NEWCHR:
                    case Grobal2.CM_DELCHR:
                    case Grobal2.CM_SELCHR:
                    //case Grobal2.CM_QUERYDELCHR:
                    //case Grobal2.CM_GETBACKDELCHR:
                        // 所有角色选项操作数据包
                        m_dwClientTimeOutTick = HUtil32.GetTickCount();
                        nEnCodeLen = Misc.EncodeBuf(nABuf, nDeCodeLen, nBBuf);
                        // StrFmt函数 必须保证格式化的字符串是AnsiString 否则消息错误.
                        pszBuf[0] = (byte)'%';
                        pszBuf[1] = (byte)'>';
                        pszBuf[2] = (byte)'#';
                        pszBuf[3] = (byte)'1';
                        Array.Copy(nBBuf, 0, pszBuf, 4, nBBuf.Length);
                        pszBuf[pszBuf.Length - 1] = (byte)'$';
                        //StrFmt(pszBuf[1], ">%d/#1%s!$", new object[] { m_pUserOBJ._SendObj.Socket, nBBuf });
                        userSession.SendBuffer(pszBuf);
                        break;
                    default:
                        //if (g_pLogMgr.CheckLevel(4))
                        //{
                        //    g_pLogMgr.Add(string.Format("错误的数据包索引: {0}", CltCmd.Cmd));
                        //}
                        KickUser(userSession.nIPAddr);
                        Succeed = false;
                        break;
                }
            }
        }

        public void ProcessSvrData(byte[] Addr, int Len)
        {
            if (m_fKickFlag)
            {
                m_fKickFlag = false;
                //SHSocket.FreeSocket(ref m_pOverlapSend.Socket);
                return;
            }
            //m_tIOCPSender.SendData(m_pOverlapSend, Addr, Len);
        }

        /// <summary>
        /// 用户链接
        /// </summary>
        public void UserEnter()
        {
            ClientSession.EnterCount++;
            m_fHandleLogin = 0;
            var szSenfBuf = "%" + string.Format("K{0}/{1}/{2}$", new object[] { userSession.SocketId, userSession.IPAddr, userSession.LocalIPAddr });
            m_tLastGameSvr.SendBuffer(szSenfBuf[1], szSenfBuf.Length);//发送服务器IP地址到DBServer
        }

        /// <summary>
        /// 用户离开 删除session并删除所有消息
        /// </summary>
        public void UserLeave()
        {
            string szSenfBuf;
            int nCode;
            //TDynPacket DynPacket;
            nCode = 0;
            try
            {
                //nCode = 1;
                //m_fHandleLogin = 0;
                //szSenfBuf = "%" + string.Format("L%d$", new object[] { m_pUserOBJ._SendObj.Socket });
                //m_tLastGameSvr.SendBuffer(szSenfBuf[1], szSenfBuf.Length);
                //nCode = 2;
                //IPAddrFilter.DeleteConnectOfIP(this.m_pUserOBJ.nIPAddr);
                //nCode = 3;
                //EnterCriticalSection(((_tagSendQueueNode)(m_pOverlapSend)).QueueLock);
                //try
                //{
                //    while (true)
                //    {
                //        if (((_tagSendQueueNode)(m_pOverlapSend)).DynSendList.Count == 0)
                //        {
                //            break;
                //        }
                //        DynPacket = m_tIOCPSender.SendQueue.GetDynPacket(m_pOverlapSend);
                //        if (DynPacket == null)
                //        {
                //            break;
                //        }
                //        FreeMem(DynPacket.Buf);
                //        Dispose(DynPacket);
                //        ((_tagSendQueueNode)(m_pOverlapSend)).DynSendList.Delete(0);
                //    }
                //}
                //finally
                //{
                //    LeaveCriticalSection(((_tagSendQueueNode)(m_pOverlapSend)).QueueLock);
                //}
                //nCode = 4;
                //if (g_ProcMsgThread != null)
                //{
                //    g_ProcMsgThread.DelSession(this);
                //}
            }
            catch (Exception M)
            {
                //g_pLogMgr.Add(Format("TSessionObj.UserLeave: %d %s", new object[] { nCode, M.Message }));
            }
        }

        public void ReCreate()
        {
            m_fKickFlag = false;
            m_nSvrObject = 0;
            m_fHandleLogin = 0;
            m_dwClientTimeOutTick = HUtil32.GetTickCount();
            m_status = 0;
        }

        public void KickUser(int remoteIP)
        {

        }
    }
}

namespace SelGate
{
    public class ClientSession
    {
        public static TSessionObj g_pFillUserObj = null;
        /// <summary>
        /// 会话对象列表
        /// </summary>
        public static TSessionObj[] g_UserList = new TSessionObj[1000];
        public static double lastqueryTick = 0;
        public static double starttick = 0;
        public static int EnterCount = 0;
        //public static HWND mainhwnd = null;
        public static bool gDeny = false;
        public static int n4ErrCount = 0;

        public static void FillUserList()
        {
            //if (g_pFillUserObj == null)
            //{
            //    g_pFillUserObj = new TSessionObj();
            //}
            //g_pFillUserObj.m_tLastGameSvr = null;
            //for (var i = 0; i < 1000; i++)
            //{
            //    g_UserList[i] = g_pFillUserObj;
            //}
        }

        public static void CleanupUserList()
        {
            
        }

        public void initialization()
        {
            FillUserList();
            gDeny = false;
            starttick = HUtil32.GetTickCount();
            lastqueryTick = starttick;
            EnterCount = 0;
            n4ErrCount = 0;
        }

        public void finalization()
        {
            CleanupUserList();
        }

    }
}

