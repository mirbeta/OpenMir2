using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 清除游戏中指定玩家复制物品
    /// </summary>
    [GameCommand("ClearCopyItem", "清除游戏中指定玩家复制物品", 10)]
    public class ClearCopyItemCommand : BaseCommond
    {
        [DefaultCommand]
        public void ClearCopyItem(string[] @Params, TPlayObject PlayObject)
        {
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            TPlayObject TargerObject;
            TUserItem UserItem;
            TUserItem UserItem1;
            string s14;

            if (sHumanName == "")
            {
                PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " 人物名称", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }

            TargerObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (TargerObject == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, new string[] {sHumanName}),
                    TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }

            for (var i = TargerObject.m_ItemList.Count - 1; i >= 0; i--)
            {
                if (TargerObject.m_ItemList.Count <= 0)
                {
                    break;
                }

                UserItem = TargerObject.m_ItemList[i];
                s14 = M2Share.UserEngine.GetStdItemName(UserItem.wIndex);
                for (var II = i - 1; II >= 0; II--)
                {
                    UserItem1 = TargerObject.m_ItemList[II];
                    if (M2Share.UserEngine.GetStdItemName(UserItem1.wIndex) == s14 &&
                        UserItem.MakeIndex == UserItem1.MakeIndex)
                    {
                        PlayObject.m_ItemList.RemoveAt(II);
                        break;
                    }
                }
            }

            for (var i = TargerObject.m_StorageItemList.Count - 1; i >= 0; i--)
            {
                if (TargerObject.m_StorageItemList.Count <= 0)
                {
                    break;
                }

                UserItem = TargerObject.m_StorageItemList[i];
                s14 = M2Share.UserEngine.GetStdItemName(UserItem.wIndex);
                for (var II = i - 1; II >= 0; II--)
                {
                    UserItem1 = TargerObject.m_StorageItemList[II];
                    if (M2Share.UserEngine.GetStdItemName(UserItem1.wIndex) == s14 &&
                        UserItem.MakeIndex == UserItem1.MakeIndex)
                    {
                        PlayObject.m_StorageItemList.RemoveAt(II);
                        break;
                    }
                }
            }
        }
    }
}