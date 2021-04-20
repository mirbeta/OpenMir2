using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace M2Server
{
    public class TGateInfo
    {
        public bool boUsed;
        public Socket Socket;
        public string SocketId;
        public string sAddr;
        public int nPort;
        public int n520;
        public IList<TGateUserInfo> UserList;
        public int nUserCount;
        public IntPtr Buffer;
        public int nBuffLen;
        public IList<IntPtr> BufferList;
        public bool boSendKeepAlive;
        public int nSendChecked;
        public int nSendBlockCount;
        public int dwStartTime;
        public int nSendMsgCount;
        public int nSendRemainCount;
        public int dwSendTick;
        public int nSendMsgBytes;
        public int nSendBytesCount;
        public int nSendedMsgCount;
        public int nSendCount;
        public int dwSendCheckTick;
    }
}