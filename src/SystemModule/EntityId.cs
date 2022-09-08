namespace SystemModule
{
    public class EntityId
    {
        /// <summary>
        /// 对象唯一ID
        /// </summary>
        public int ActorId;

        public EntityId()
        {
            ActorId = HUtil32.Sequence();
        }
    }
}