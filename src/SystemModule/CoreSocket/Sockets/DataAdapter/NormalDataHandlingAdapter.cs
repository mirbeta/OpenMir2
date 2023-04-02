using System;
using System.Collections.Generic;
using SystemModule.CoreSocket;

namespace SystemModule.CoreSocket
{
    /// <summary>
    /// 普通TCP数据处理器，该适配器不对数据做任何处理。
    /// </summary>
    public class NormalDataHandlingAdapter : DataHandlingAdapter
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override bool CanSplicingSend => false;
    
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override bool CanSendRequestInfo => false;
    
        /// <summary>
        /// 当接收到数据时处理数据
        /// </summary>
        /// <param name="byteBlock">数据流</param>
        protected override void PreviewReceived(ByteBlock byteBlock)
        {
            GoReceived(byteBlock, null);
        }
    
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="buffer">数据</param>
        /// <param name="offset">偏移</param>
        /// <param name="length">长度</param>
        protected override void PreviewSend(byte[] buffer, int offset, int length)
        {
            GoSend(buffer, offset, length);
        }
    
        protected override void PreviewSend(ReadOnlyMemory<byte> buffer, int offset, int length)
        {
            GoSend(buffer, offset, length);
        }
    
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="transferBytes"></param>
        protected override void PreviewSend(IList<ArraySegment<byte>> transferBytes)
        {
            throw new System.NotImplementedException();//因为设置了不支持拼接发送，所以该方法可以不实现。
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