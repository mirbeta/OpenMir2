using SystemModule;
using SystemModule.Packets;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.Player
{
    public partial class PlayObject
    {
        const byte HeaderLen = 32;

        private ServerMessagePacket messageHead = new ServerMessagePacket
        {
            PacketCode = Grobal2.RUNGATECODE,
            Ident = Grobal2.GM_DATA
        };

        internal void SetSocketHead()
        {
            messageHead.Socket = SocketId;
            messageHead.SessionId = SocketIdx;
        }

        /// <summary>
        /// 动作消息 走路、跑步、战士攻击等
        /// </summary>
        /// <param name="sMsg"></param>
        private void SendSocket(string sMsg)
        {
            if (OffLineFlag)
            {
                return;
            }
            if (string.IsNullOrEmpty(sMsg))
                return;
            var msgBuff = HUtil32.GetBytes(sMsg);
            messageHead.PackLength = -msgBuff.Length;
            var actionData = BufferManager.GetBuffer(ServerMessagePacket.PacketSize + msgBuff.Length);
            MemoryCopy.BlockCopy(ServerPackSerializer.Serialize(messageHead), 0, actionData, 0, ServerMessagePacket.PacketSize);
            MemoryCopy.BlockCopy(msgBuff, 0, actionData, ServerMessagePacket.PacketSize, msgBuff.Length);
            M2Share.GateMgr.AddGateBuffer(GateIdx, actionData);
        }

        /// <summary>
        /// 发送普通命令消息（如：玩家升级、攻击目标等）
        /// </summary>
        /// <param name="defMsg"></param>
        private void SendSocket(ClientCommandPacket defMsg)
        {
            if (OffLineFlag && defMsg.Ident != Grobal2.SM_OUTOFCONNECTION)
            {
                return;
            }
            messageHead.PackLength = ClientCommandPacket.PackSize;
            var sendData = BufferManager.GetBuffer(HeaderLen);
            MemoryCopy.BlockCopy(ServerPackSerializer.Serialize(messageHead), 0, sendData, 0, ServerMessagePacket.PacketSize);
            MemoryCopy.BlockCopy(ServerPackSerializer.Serialize(defMsg), 0, sendData, ServerMessagePacket.PacketSize, ClientCommandPacket.PackSize);
            M2Share.GateMgr.AddGateBuffer(GateIdx, sendData);
        }

        internal virtual void SendSocket(ClientCommandPacket defMsg, string sMsg)
        {
            if (OffLineFlag && defMsg.Ident != Grobal2.SM_OUTOFCONNECTION)
            {
                return;
            }
            if (string.IsNullOrEmpty(sMsg))
            {
                return;
            }
            var bMsg = HUtil32.GetBytes(sMsg);
            var sendData = BufferManager.GetBuffer(HeaderLen + bMsg.Length);
            messageHead.PackLength = bMsg.Length + ClientCommandPacket.PackSize;
            MemoryCopy.BlockCopy(ServerPackSerializer.Serialize(messageHead), 0, sendData, 0, ServerMessagePacket.PacketSize);
            MemoryCopy.BlockCopy(ServerPackSerializer.Serialize(defMsg), 0, sendData, ServerMessagePacket.PacketSize, ClientCommandPacket.PackSize);
            MemoryCopy.BlockCopy(bMsg, 0, sendData, HeaderLen, bMsg.Length);
            M2Share.GateMgr.AddGateBuffer(GateIdx, sendData);
        }

        public void SendDefMessage(short wIdent, int nRecog, int nParam, int nTag, int nSeries, string sMsg)
        {
            ClientMsg = Grobal2.MakeDefaultMsg(wIdent, nRecog, nParam, nTag, nSeries);
            if (!string.IsNullOrEmpty(sMsg))
            {
                SendSocket(ClientMsg, EDCode.EncodeString(sMsg));
            }
            else
            {
                SendSocket(ClientMsg);
            }
        }
    }
}