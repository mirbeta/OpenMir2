using SystemModule;
using System;
using System.Collections.Generic;
using GameSvr.CommandSystem;
using System.Collections;
using System.Linq;

namespace GameSvr
{
    /// <summary>
    /// 显示你屏幕上你近处所有怪与人的详细情况
    /// </summary>
    [GameCommand("MobLevel", "显示你屏幕上你近处所有怪与人的详细情况", 10)]
    public class MobLevelCommand : BaseCommond
    {
        [DefaultCommand]
        public void MobLevel(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params != null && @Params.Length > 0)
            {
                var sParam = @Params.Length > 0 ? @Params[0] : "";
                if (sParam != "" && sParam[0] == '?')
                {
                    PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow,  this.Attributes.Name, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
            }
            TBaseObject BaseObject;
            IList<TBaseObject> BaseObjectList = new List<TBaseObject>();
            PlayObject.m_PEnvir.GetRangeBaseObject(PlayObject.m_nCurrX, PlayObject.m_nCurrY, 2, true, BaseObjectList);
            for (var i = 0; i < BaseObjectList.Count; i++)
            {
                BaseObject = BaseObjectList[i];
                PlayObject.SysMsg(BaseObject.GeTBaseObjectInfo(), TMsgColor.c_Green, TMsgType.t_Hint);
            }
            BaseObjectList.Clear();
            BaseObjectList = null;
        }
    }
}