using System;
using System.Net.Sockets;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Packages;

namespace GameSvr
{
    public class SendQueue
    {
        private readonly Channel<SendQueueData> _sendQueue = null;
        private readonly int _queueConsumerCount;

        public SendQueue(int consumerCount)
        {
            _sendQueue = Channel.CreateUnbounded<SendQueueData>();
            _queueConsumerCount = consumerCount;
        }

        public int GetQueueCount => _sendQueue.Reader.Count;

        public int QueueConsumerCount => _queueConsumerCount;

        /// <summary>
        /// 添加到发送队列
        /// </summary>
        public void AddToQueue(TGateInfo session, byte[] buffer)
        {
            _sendQueue.Writer.TryWrite(new SendQueueData()
            {
                Gate = session,
                Buffer = buffer
            });
        }

        /// <summary>
        /// 处理队列数据并发送到GameGate
        /// GameSvr -> GameGate
        /// </summary>
        public async Task ProcessSendQueue()
        {
            while (await _sendQueue.Reader.WaitToReadAsync())
            {
                if (_sendQueue.Reader.TryRead(out var queueData))
                {
                    var resp = await queueData.SendBuffers();
                    //if (resp != queueData.Buffer.Length)
                    //{
                    //    Console.WriteLine("向客户端发送数据包失败", 5);
                    //}
                }
            }
        }
    }

    public class SendQueueData
    {
        public TGateInfo Gate;
        public byte[] Buffer;

        public async Task<int> SendBuffers()
        {
            const string sExceptionMsg = "[Exception] TRunSocket::SendGateBuffers -> SendBuff";
            var dwRunTick = HUtil32.GetTickCount();
            var sendLen = 0;
            if (Gate.nSendChecked > 0)// 如果网关未回复状态消息，则不再发送数据
            {
                if ((HUtil32.GetTickCount() - Gate.dwSendCheckTick) > M2Share.g_dwSocCheckTimeOut) // 2 * 1000
                {
                    Gate.nSendChecked = 0;
                    Gate.nSendBlockCount = 0;
                }
                return sendLen;
            }
            try
            {
                var nSendBuffLen = Buffer.Length;
                if (Gate.nSendChecked == 0 && Gate.nSendBlockCount + nSendBuffLen >= M2Share.g_Config.nCheckBlock * 10)
                {
                    if (Gate.nSendBlockCount == 0 && M2Share.g_Config.nCheckBlock * 10 <= nSendBuffLen)
                    {
                        return sendLen;
                    }
                    SendCheck(Gate.Socket, Grobal2.GM_RECEIVE_OK);
                    Gate.nSendChecked = 1;
                    Gate.dwSendCheckTick = HUtil32.GetTickCount();
                }
                var sendBuffer = new byte[Buffer.Length - 4];
                Array.Copy(Buffer, 4, sendBuffer, 0, sendBuffer.Length);
                nSendBuffLen = sendBuffer.Length;
                if (nSendBuffLen > 0)
                {
                    while (true)
                    {
                        if (M2Share.g_Config.nSendBlock <= nSendBuffLen)
                        {
                            if (Gate.Socket != null)
                            {
                                if (Gate.Socket.Connected)
                                {
                                    var sendBuff = new byte[M2Share.g_Config.nSendBlock];
                                    Array.Copy(sendBuffer, 0, sendBuff, 0, M2Share.g_Config.nSendBlock);
                                    sendLen = await Gate.Socket.SendAsync(sendBuff, SocketFlags.None);
                                }
                                Gate.nSendCount++;
                                Gate.nSendBytesCount += M2Share.g_Config.nSendBlock;
                            }
                            Gate.nSendBlockCount += M2Share.g_Config.nSendBlock;
                            nSendBuffLen -= M2Share.g_Config.nSendBlock;
                            var tempBuff = new byte[nSendBuffLen];
                            Array.Copy(sendBuffer, M2Share.g_Config.nSendBlock, tempBuff, 0, nSendBuffLen);
                            sendBuffer = tempBuff;
                            continue;
                        }
                        if (Gate.Socket != null)
                        {
                            if (Gate.Socket.Connected)
                            {
                                //Gate.Socket.Send(sendBuffer, 0, nSendBuffLen, SocketFlags.None);
                                sendLen = await Gate.Socket.SendAsync(sendBuffer, SocketFlags.None);
                            }
                            Gate.nSendCount++;
                            Gate.nSendBytesCount += nSendBuffLen;
                            Gate.nSendBlockCount += nSendBuffLen;
                        }
                        nSendBuffLen = 0;
                        break;
                    }
                }
                if ((HUtil32.GetTickCount() - dwRunTick) > M2Share.g_dwSocLimit)
                {
                    return 0;
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.StackTrace, MessageType.Error);
            }
            return sendLen;
        }

        private void SendCheck(Socket Socket, ushort nIdent)
        {
            if (!Socket.Connected)
            {
                return;
            }
            var MsgHeader = new MessageHeader
            {
                dwCode = Grobal2.RUNGATECODE,
                nSocket = 0,
                wIdent = nIdent,
                nLength = 0
            };
            if (Socket.Connected)
            {
                var data = MsgHeader.GetPacket();
                Socket.Send(data, 0, data.Length, SocketFlags.None);
            }
        }
    }
}
