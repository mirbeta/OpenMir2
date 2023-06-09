using SystemModule;
using SystemModule.Packets.ClientPackets;
using SystemModule.Packets.ServerPackets;

namespace M2Server.Player
{
    public partial class PlayObject
    {
        private const byte HeaderLen = 32;

        public void SetSocket()
        {
            //messageHead.Socket = SocketId;
            //messageHead.SessionId = SocketIdx;
        }

        public void SendDefMessage(short wIdent, int nRecog, int nParam, int nTag, int nSeries)
        {
            if (IsRobot)
            {
                return;
            }
            ClientMsg = Messages.MakeMessage(wIdent, nRecog, nParam, nTag, nSeries);
            SendSocket(ClientMsg);
        }

        public void SendDefMessage(short wIdent, int nRecog, int nParam, int nTag, int nSeries, string sMsg)
        {
            if (IsRobot)
            {
                return;
            }
            ClientMsg = Messages.MakeMessage(wIdent, nRecog, nParam, nTag, nSeries);
            if (!string.IsNullOrEmpty(sMsg))
            {
                SendSocket(ClientMsg, EDCode.EncodeString(sMsg));
            }
            else
            {
                SendSocket(ClientMsg);
            }
        }

        /// <summary>
        /// 动作消息 走路、跑步、战士攻击等
        /// </summary>
        /// <param name="sMsg"></param>
        public void SendSocket(string sMsg)
        {
            if (IsRobot)
            {
                return;
            }
            if (OffLineFlag)
            {
                return;
            }
            byte[] msgBuff = HUtil32.GetBytes(sMsg);
            ServerMessage messageHead = new ServerMessage
            {
                PacketCode = Grobal2.PacketCode,
                Ident = Grobal2.GM_DATA,
                Socket = SocketId,
                SessionId = SocketIdx
            };
            messageHead.PackLength = -msgBuff.Length;
            byte[] actionData = new byte[ServerMessage.PacketSize + msgBuff.Length];
            MemoryCopy.BlockCopy(SerializerUtil.Serialize(messageHead), 0, actionData, 0, ServerMessage.PacketSize);
            MemoryCopy.BlockCopy(msgBuff, 0, actionData, ServerMessage.PacketSize, msgBuff.Length);
            // M2Share.SocketMgr.AddGateBuffer(GateIdx, actionData);
        }

        /// <summary>
        /// 发送普通命令消息（如：玩家升级、攻击目标等）
        /// </summary>
        /// <param name="defMsg"></param>
        private void SendSocket(CommandMessage defMsg)
        {
            if (IsRobot)
            {
                return;
            }
            if (OffLineFlag && defMsg.Ident != Messages.SM_OUTOFCONNECTION)
            {
                return;
            }
            ServerMessage messageHead = new ServerMessage
            {
                PacketCode = Grobal2.PacketCode,
                Ident = Grobal2.GM_DATA,
                Socket = SocketId,
                SessionId = SocketIdx
            };
            messageHead.PackLength = CommandMessage.Size;
            byte[] sendData = new byte[HeaderLen];
            MemoryCopy.BlockCopy(SerializerUtil.Serialize(messageHead), 0, sendData, 0, ServerMessage.PacketSize);
            MemoryCopy.BlockCopy(SerializerUtil.Serialize(defMsg), 0, sendData, ServerMessage.PacketSize, CommandMessage.Size);
            // M2Share.SocketMgr.AddGateBuffer(GateIdx, sendData);
        }

        internal virtual void SendSocket(CommandMessage defMsg, string sMsg)
        {
            if (IsRobot)
            {
                return;
            }
            if (OffLineFlag && defMsg.Ident != Messages.SM_OUTOFCONNECTION)
            {
                return;
            }
            if (string.IsNullOrEmpty(sMsg))
            {
                return;
            }
            byte[] bMsg = HUtil32.GetBytes(sMsg);
            byte[] sendData = new byte[HeaderLen + bMsg.Length];
            ServerMessage messageHead = new ServerMessage
            {
                PacketCode = Grobal2.PacketCode,
                Ident = Grobal2.GM_DATA,
                Socket = SocketId,
                SessionId = SocketIdx
            };
            messageHead.PackLength = bMsg.Length + CommandMessage.Size;
            MemoryCopy.BlockCopy(SerializerUtil.Serialize(messageHead), 0, sendData, 0, ServerMessage.PacketSize);
            MemoryCopy.BlockCopy(SerializerUtil.Serialize(defMsg), 0, sendData, ServerMessage.PacketSize, CommandMessage.Size);
            MemoryCopy.BlockCopy(bMsg, 0, sendData, HeaderLen, bMsg.Length);
            // M2Share.SocketMgr.AddGateBuffer(GateIdx, sendData);
        }
    }
}