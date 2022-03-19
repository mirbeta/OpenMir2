using GameSvr.CommandSystem;
using System.Collections.Generic;
using SystemModule;

namespace GameSvr
{
    /// <summary>
    /// 显示你屏幕上你近处所有怪与人的详细情况
    /// </summary>
    [GameCommand("MobLevel", "显示你屏幕上你近处所有怪与人的详细情况", 10)]
    public class MobLevelCommand : BaseCommond
    {
        [DefaultCommand]
        public void MobLevel(TPlayObject PlayObject)
        {
            TBaseObject BaseObject;
            IList<TBaseObject> BaseObjectList = new List<TBaseObject>();
            PlayObject.m_PEnvir.GetRangeBaseObject(PlayObject.m_nCurrX, PlayObject.m_nCurrY, 2, true, BaseObjectList);
            for (var i = 0; i < BaseObjectList.Count; i++)
            {
                BaseObject = BaseObjectList[i];
                PlayObject.SysMsg(BaseObject.GeTBaseObjectInfo(), MsgColor.Green, MsgType.Hint);
            }
            BaseObjectList.Clear();
            BaseObjectList = null;
        }
    }
}