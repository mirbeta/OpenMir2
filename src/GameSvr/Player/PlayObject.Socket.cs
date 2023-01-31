using SystemModule.Packets;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.Player
{
    public partial class PlayObject
    {
        private const byte HeaderLen = 32;

        private ServerMessage messageHead = new ServerMessage
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
            byte[] msgBuff = HUtil32.GetBytes(sMsg);
            messageHead.PackLength = -msgBuff.Length;
            byte[] actionData = new byte[ServerMessage.PacketSize + msgBuff.Length];
            MemoryCopy.BlockCopy(SerializerUtil.Serialize(messageHead), 0, actionData, 0, ServerMessage.PacketSize);
            MemoryCopy.BlockCopy(msgBuff, 0, actionData, ServerMessage.PacketSize, msgBuff.Length);
            M2Share.GateMgr.AddGateBuffer(GateIdx, actionData);
            Console.WriteLine($"发送动作消息 Len:{actionData.Length}");
        }

        /// <summary>
        /// 发送普通命令消息（如：玩家升级、攻击目标等）
        /// </summary>
        /// <param name="defMsg"></param>
        private void SendSocket(CommandPacket defMsg)
        {
            if (OffLineFlag && defMsg.Ident != Messages.SM_OUTOFCONNECTION)
            {
                return;
            }
            messageHead.PackLength = CommandPacket.Size;
            byte[] sendData = new byte[HeaderLen];
            MemoryCopy.BlockCopy(SerializerUtil.Serialize(messageHead), 0, sendData, 0, ServerMessage.PacketSize);
            MemoryCopy.BlockCopy(SerializerUtil.Serialize(defMsg), 0, sendData, ServerMessage.PacketSize, CommandPacket.Size);
            M2Share.GateMgr.AddGateBuffer(GateIdx, sendData);
            Console.WriteLine($"发送命令消息 Len:{sendData.Length} Ident:{defMsg.Ident}");
        }

        internal virtual void SendSocket(CommandPacket defMsg, string sMsg)
        {
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
            messageHead.PackLength = bMsg.Length + CommandPacket.Size;
            MemoryCopy.BlockCopy(SerializerUtil.Serialize(messageHead), 0, sendData, 0, ServerMessage.PacketSize);
            MemoryCopy.BlockCopy(SerializerUtil.Serialize(defMsg), 0, sendData, ServerMessage.PacketSize, CommandPacket.Size);
            MemoryCopy.BlockCopy(bMsg, 0, sendData, HeaderLen, bMsg.Length);
            M2Share.GateMgr.AddGateBuffer(GateIdx, sendData);
            Console.WriteLine($"发送文字消息 Len:{sendData.Length} Ident:{defMsg.Ident}");
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