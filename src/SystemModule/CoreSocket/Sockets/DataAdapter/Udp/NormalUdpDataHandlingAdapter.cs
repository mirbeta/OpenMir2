using System;
using System.Collections.Generic;
using System.Net;
using SystemModule.CoreSocket;

namespace SystemModule.CoreSocket
{
    /// <summary>
    /// 常规UDP数据处理适配器
    /// </summary>
    public class NormalUdpDataHandlingAdapter : UdpDataHandlingAdapter
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override bool CanSplicingSend => true;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override bool CanSendRequestInfo => false;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="remoteEndPoint"></param>
        /// <param name="byteBlock"></param>
        protected override void PreviewReceived(EndPoint remoteEndPoint, ByteBlock byteBlock)
        {
            GoReceived(remoteEndPoint, byteBlock, null);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        protected override void PreviewSend(EndPoint endPoint, byte[] buffer, int offset, int length)
        {
            GoSend(endPoint, buffer, offset, length);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="transferBytes"></param>
        protected override void PreviewSend(EndPoint endPoint, IList<ArraySegment<byte>> transferBytes)
        {
            int length = 0;
            foreach (ArraySegment<byte> item in transferBytes)
            {
                length += item.Count;
            }

            if (length > MaxPackageSize)
            {
                throw new OverlengthException("发送数据大于设定值，相同解析器可能无法收到有效数据，已终止发送");
            }

            using (ByteBlock byteBlock = new ByteBlock(length))
            {
                foreach (ArraySegment<byte> item in transferBytes)
                {
                    byteBlock.Write(item.Array, item.Offset, item.Count);
                }
                GoSend(endPoint, byteBlock.Buffer, 0, byteBlock.Len);
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="requestInfo"></param>
        protected override void PreviewSend(IRequestInfo requestInfo)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void Reset()
        {
        }
    }
}