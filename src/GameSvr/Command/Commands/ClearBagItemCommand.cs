using GameSvr.CommandSystem;
using System.Collections.Generic;
using SystemModule;

namespace GameSvr
{
    [GameCommand("ClearBagItem", "清理包裹物品", "人物名称", 10)]
    public class ClearBagItemCommand : BaseCommond
    {
        [DefaultCommand]
        public void ClearBagItem(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? Params[0] : "";
            TUserItem UserItem;
            IList<TDeleteItem> DelList = null;
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?')
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (m_PlayObject.m_ItemList.Count > 0)
            {
                for (var i = m_PlayObject.m_ItemList.Count - 1; i >= 0; i--)
                {
                    UserItem = m_PlayObject.m_ItemList[i];
                    if (DelList == null)
                    {
                        DelList = new List<TDeleteItem>();
                    }
                    DelList.Add(new TDeleteItem()
                    {
                        sItemName = M2Share.UserEngine.GetStdItemName(UserItem.wIndex),
                        MakeIndex = UserItem.MakeIndex
                    });
                    UserItem = null;
                    m_PlayObject.m_ItemList.RemoveAt(i);
                }
                m_PlayObject.m_ItemList.Clear();
            }
            if (DelList != null)
            {
                var ObjectId = HUtil32.Sequence();
                M2Share.ObjectManager.AddOhter(ObjectId, DelList);
                m_PlayObject.SendMsg(m_PlayObject, Grobal2.RM_SENDDELITEMLIST, 0, ObjectId, 0, 0, "");
            }
        }
    }
}