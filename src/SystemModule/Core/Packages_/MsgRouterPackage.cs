using SystemModule.ByteManager;

namespace SystemModule.Core.Packages_
{
    /// <summary>
    /// 可承载消息的路由包
    /// </summary>
    public class MsgRouterPackage : RouterPackage
    {
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <inheritdoc/>
        public override void PackageBody(ByteBlock byteBlock)
        {
            byteBlock.Write(Message);
        }

        /// <inheritdoc/>
        public override void UnpackageBody(ByteBlock byteBlock)
        {
            this.Message = byteBlock.ReadString();
        }
    }
}