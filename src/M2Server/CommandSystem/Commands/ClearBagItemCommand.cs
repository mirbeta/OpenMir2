using SystemModule;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using M2Server.CommandSystem;

namespace M2Server
{
    [GameCommand("ClearBagItem", "清理包裹物品", 10)]
    public class ClearBagItemCommand : BaseCommond
    {
        [DefaultCommand]
        public unsafe void ClearBagItem(string[] @Params, TPlayObject PlayObject)
        {
            var sHumanName = @Params.Length > 0 ? Params[0] : "";
            TUserItem UserItem;
            IList<int> DelList = null;
            if ((sHumanName == "") || ((sHumanName != "") && (sHumanName[0] == '?')))
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, "人物名称"), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            try
            {
                if (m_PlayObject.m_ItemList.Count > 0)
                {
                    for (var i = m_PlayObject.m_ItemList.Count - 1; i >= 0; i--)
                    {
                        UserItem = m_PlayObject.m_ItemList[i];
                        if (DelList == null)
                        {
                            DelList = new List<int>();
                        }
                        DelList.Add(UserItem.MakeIndex);
                        //Dispose(UserItem);
                        UserItem = null;
                        m_PlayObject.m_ItemList.RemoveAt(i);
                    }
                    m_PlayObject.m_ItemList.Clear();
                }
                if (DelList != null)
                {
                    var ObjectId = HUtil32.Sequence();
                    M2Share.ObjectSystem.AddOhter(ObjectId, DelList);
                    m_PlayObject.SendMsg(m_PlayObject, grobal2.RM_SENDDELITEMLIST, 0, ObjectId, 0, 0, "");
                }
            }
            finally
            {
            }
        }
    }
}