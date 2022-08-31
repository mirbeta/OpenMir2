namespace SystemModule
{
    public class EntityId
    {
        /// <summary>
        /// 对象唯一ID
        /// </summary>
        public int ObjectId;

        public EntityId()
        {
            ObjectId = HUtil32.Sequence();
        }
    }
}