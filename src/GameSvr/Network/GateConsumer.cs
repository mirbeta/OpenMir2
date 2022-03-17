using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Packages;

namespace GameSvr
{
    /// <summary>
    /// 游戏网关消费者
    /// GameSvr->GameGate
    /// </summary>
    public class GateConsumer
    {
        private readonly ChannelReader<byte[]> _queue;
        private readonly int _identifier;
        private readonly TGateInfo _gate;

        public GateConsumer(TGateInfo gate,int identifier)
        {
            _queue = gate.Queue.Reader;
            _identifier = identifier;
            _gate = gate;
        }

        public TGateInfo Gate => _gate;
        
        public async Task ProcessGateData(CancellationToken cancellation)
        {
            Console.WriteLine($"GameGate Consumer ({_identifier}): Starting");
            while (await _queue.WaitToReadAsync(cancellation))
            {
                if (_queue.TryRead(out var buffer))
                {
                    SendGateBuffers(buffer);
                }
            }
        }
        
        private void SendGateBuffers(byte[] buffer)
        {
            const string sExceptionMsg = "[Exception] TRunSocket::SendGateBuffers -> SendBuff";
            var dwRunTick = HUtil32.GetTickCount();
            if (Gate.nSendChecked > 0)// 如果网关未回复状态消息，则不再发送数据
            {
                if ((HUtil32.GetTickCount() - Gate.dwSendCheckTick) > M2Share.g_dwSocCheckTimeOut) // 2 * 1000
                {
                    Gate.nSendChecked = 0;
                    Gate.nSendBlockCount = 0;
                }
                return;
            }
            try
            {
                var nSendBuffLen = buffer.Length; 

                /*var dBuffer = buffer[24..];
                var DefMsg = new TDefaultMessage(dBuffer);
                M2Share.MainOutMessage(string.Format("消息 Ident:{0}",DefMsg.Ident, DefMsg.Ident));
                M2Share.MainOutMessage(string.Format("数据大小 Block:{0} sMsg:{1} {2}",M2Share.g_Config.nCheckBlock, Gate.nSendBlockCount + nSendBuffLen, nSendBuffLen));
                */
                
                if (Gate.nSendChecked == 0 && Gate.nSendBlockCount + nSendBuffLen >= M2Share.g_Config.nCheckBlock * 10)
                {
                    if (_gate.nSendBlockCount == 0 && M2Share.g_Config.nCheckBlock * 10 <= nSendBuffLen)
                    {
                        return;
                    }
                    SendCheck(_gate.Socket, Grobal2.GM_RECEIVE_OK);
                    _gate.nSendChecked = 1;
                    _gate.dwSendCheckTick = HUtil32.GetTickCount();
                }
                var sendBuffer = new byte[buffer.Length - 4];
                Array.Copy(buffer, 4, sendBuffer, 0, sendBuffer.Length);
                nSendBuffLen = sendBuffer.Length;
                if (nSendBuffLen > 0)
                {
                    while (true)
                    {
                        if (M2Share.g_Config.nSendBlock <= nSendBuffLen)
                        {
                            if (_gate.Socket != null)
                            {
                                if (_gate.Socket.Connected)
                                {
                                    var sendBuff = new byte[M2Share.g_Config.nSendBlock];
                                    Array.Copy(sendBuffer, 0, sendBuff, 0, M2Share.g_Config.nSendBlock);
                                    _gate.Socket.Send(sendBuff, 0, sendBuff.Length, SocketFlags.None);
                                }
                                _gate.nSendCount++;
                                _gate.nSendBytesCount += M2Share.g_Config.nSendBlock;
                            }
                            _gate.nSendBlockCount += M2Share.g_Config.nSendBlock;
                            nSendBuffLen -= M2Share.g_Config.nSendBlock;
                            var tempBuff = new byte[nSendBuffLen];
                            Array.Copy(sendBuffer, M2Share.g_Config.nSendBlock, tempBuff, 0, nSendBuffLen);
                            sendBuffer = tempBuff;
                            continue;
                        }
                        if (_gate.Socket != null)
                        {
                            if (_gate.Socket.Connected)
                            {
                                _gate.Socket.Send(sendBuffer, 0, nSendBuffLen, SocketFlags.None);
                            }
                            _gate.nSendCount++;
                            _gate.nSendBytesCount += nSendBuffLen;
                            _gate.nSendBlockCount += nSendBuffLen;
                        }
                        nSendBuffLen = 0;
                        break;
                    }
                }
                if ((HUtil32.GetTickCount() - dwRunTick) > M2Share.g_dwSocLimit)
                {
                    return;
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.StackTrace, MessageType.Error);
            }
        }
        
        private void SendCheck(Socket Socket, int nIdent)
        {
            if (!Socket.Connected)
            {
                return;
            }
            var MsgHeader = new MessageHeader
            {
                dwCode = Grobal2.RUNGATECODE,
                nSocket = 0,
                wIdent = (ushort)nIdent,
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
