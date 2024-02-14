using SystemModule.Actors;
using SystemModule.Enums;

namespace SystemModule.Data
{
    /// <summary>
    /// 可见的精灵
    /// </summary>
    public class VisibleBaseObject
    {
        public IActor BaseObject;
        public VisibleFlag VisibleFlag;
    }
}