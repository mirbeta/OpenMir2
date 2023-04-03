using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Text;
using SystemModule;
using SystemModule.ByteManager;
using SystemModule.CoreSocket;
using SystemModule.Packets.ServerPackets;

namespace GameGate.Services.Tests
{
    [TestClass()]
    public class ClientThreadTests
    {
        private int bodyLength = 0;
        private bool beCached;
        private ByteBlock buffBlock = new ByteBlock();

        [TestMethod()]
        public void ProcessPacketTest()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var testData = new byte[10240 * 100];
            var offset = 0;
            for (int i = 0; i < 10000; i++)
            {
                var messageHead = new ServerMessage()
                {
                    PacketCode = Grobal2.PacketCode,
                    Ident = Grobal2.GM_DATA,
                    Socket = 0,
                    SessionId = 0,
                    SessionIndex = 0,
                    PackLength = 0
                };

                var strLen = HUtil32.GetBytes(RandomNumber.GetInstance().GenerateRandomNumber(20));
                messageHead.PackLength = strLen.Length;
                var data = new byte[20 + strLen.Length];

                var headData = SerializerUtil.Serialize(messageHead);
                Array.Copy(headData, 0, data, 0, headData.Length);
                Array.Copy(strLen, 0, data, headData.Length, strLen.Length);

                Array.Copy(data, 0, testData, offset, data.Length);
                offset += data.Length;
            }

            ProcessPacket(testData, offset);

        }

        public void ProcessPacket(byte[] data, int dataLen)
        {
            ReadOnlySequence<byte> readSequence = new ReadOnlySequence<byte>(data, 0, dataLen);
            var sequenceReader = new SequenceReader<byte>(readSequence);
            while (sequenceReader.Remaining > 0) //表示整个序列还剩几个数据，也就是“已读索引”之后有几个数据
            {
                if (dataLen > 400)
                {
                    Console.WriteLine("123");
                }
                var sourceSpan = sequenceReader.CurrentSpan.Slice((int)sequenceReader.Consumed, ServerMessage.PacketSize);
                if (!MemoryMarshal.TryRead(sourceSpan, out ServerMessage message))
                {
                    return;
                }
                if (message.PacketCode != Grobal2.PacketCode)
                {
                    return;
                }
                if (message.PackLength < 0)
                {
                    bodyLength = -message.PackLength;
                }
                else
                {
                    bodyLength = message.PackLength;
                }
                if (sequenceReader.Remaining < bodyLength)  //body不满足解析，开始缓存，然后保存对象
                {
                    beCached = true;
                    buffBlock.Write(sequenceReader.UnreadSpan.ToArray(), 0, (int)sequenceReader.Remaining);
                    return;
                }
                else
                {
                    var serverPacket = sequenceReader.CurrentSpan.Slice(ServerMessage.PacketSize, bodyLength);
                    //ProcessServerPacket(message, serverPacket);
                    sequenceReader.Advance(20 + bodyLength);
                    bodyLength = 0;
                }
            }
        }
    }
}