using System;
using SystemModule;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 造指定物品(支持权限分配，小于最大权限受允许、禁止制造列表限制)
    /// 要求权限默认等级：10
    /// </summary>
    [GameCommand("Make", desc: "制造指定物品(支持权限分配，小于最大权限受允许、禁止制造列表限制)", help: M2Share.g_sGamecommandMakeHelpMsg, minUserLevel: 10)]
    public class MakeItemCommond : BaseCommond
    {
        [DefaultCommand]
        public void CmdMakeItem(string[] Params, TPlayObject PlayObject)
        {
            if (Params == null) return;
            var sItemName = Params.Length > 0 ? Params[0] : ""; //物品名称
            var nCount = Params.Length > 1 ? Convert.ToInt32(Params[1]) : 1; //数量
            var sParam = Params.Length > 2 ? Params[2] : ""; //可选参数（持久力）
            if (string.IsNullOrEmpty(sItemName))
            {
                PlayObject.SysMsg(CommandAttribute.CommandHelp(), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (nCount <= 0) nCount = 1;
            if (nCount > 10) nCount = 10;
            if (PlayObject.m_btPermission < CommandAttribute.nPermissionMax)
            {
                if (!M2Share.CanMakeItem(sItemName))
                {
                    PlayObject.SysMsg(M2Share.g_sGamecommandMakeItemNameOrPerMissionNot, MsgColor.Red, MsgType.Hint);
                    return;
                }
                if (M2Share.CastleManager.InCastleWarArea(PlayObject) != null) // 攻城区域，禁止使用此功能
                {
                    PlayObject.SysMsg(M2Share.g_sGamecommandMakeInCastleWarRange, MsgColor.Red, MsgType.Hint);
                    return;
                }
                if (!PlayObject.InSafeZone())
                {
                    PlayObject.SysMsg(M2Share.g_sGamecommandMakeInSafeZoneRange, MsgColor.Red, MsgType.Hint);
                    return;
                }
                nCount = 1;
            }
            for (var i = 0; i < nCount; i++)
            {
                if (PlayObject.m_ItemList.Count >= Grobal2.MAXBAGITEM) return;
                TUserItem UserItem = null;
                if (M2Share.UserEngine.CopyToUserItemFromName(sItemName, ref UserItem))
                {
                    var StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    if (StdItem.Price >= 15000 && !M2Share.g_Config.boTestServer && PlayObject.m_btPermission < 5)
                    {
                        UserItem = null;
                    }
                    else
                    {
                        if (M2Share.RandomNumber.Random(M2Share.g_Config.nMakeRandomAddValue) == 0)
                        {
                            StdItem.RandomUpgradeItem(UserItem);
                        }
                    }
                    if (PlayObject.m_btPermission >= CommandAttribute.nPermissionMax)
                    {
                        UserItem.MakeIndex = M2Share.GetItemNumberEx(); // 制造的物品另行取得物品ID
                    }
                    PlayObject.m_ItemList.Add(UserItem);
                    PlayObject.SendAddItem(UserItem);
                    if (PlayObject.m_btPermission >= 6)
                    {
                        M2Share.MainOutMessage("[制造物品] " + PlayObject.m_sCharName + " " + sItemName + "(" + UserItem.MakeIndex + ")");
                    }
                    if (StdItem.NeedIdentify == 1)
                    {
                        M2Share.AddGameDataLog("5" + "\09" + PlayObject.m_sMapName + "\09" + PlayObject.m_nCurrX +
                           "\09" + PlayObject.m_nCurrY + "\09" + PlayObject.m_sCharName + "\09" + StdItem.Name + "\09" + UserItem.MakeIndex + "\09" + "(" +
                           HUtil32.LoWord(StdItem.Dc) + "/" + HUtil32.HiWord(StdItem.Dc) + ")" + "(" + HUtil32.LoWord(StdItem.Mc) + "/" + HUtil32.HiWord(StdItem.Mc) + ")" + "(" +
                           HUtil32.LoWord(StdItem.Sc) + "/" + HUtil32.HiWord(StdItem.Sc) + ")" + "(" + HUtil32.LoWord(StdItem.Ac) + "/" +
                           HUtil32.HiWord(StdItem.Ac) + ")" + "(" + HUtil32.LoWord(StdItem.Mac) + "/" + HUtil32.HiWord(StdItem.Mac) + ")" + UserItem.btValue[0]
                           + "/" + UserItem.btValue[1] + "/" + UserItem.btValue[2] + "/" + UserItem.btValue[3] + "/" + UserItem.btValue[4] + "/" + UserItem.btValue[5] + "/" + UserItem.btValue[6]
                           + "/" + UserItem.btValue[7] + "/" + UserItem.btValue[8] + "/" + UserItem.btValue[14] + "\09" + PlayObject.m_sCharName);
                    }
                }
                else
                {
                    UserItem = null;
                    PlayObject.SysMsg(string.Format(M2Share.g_sGamecommandMakeItemNameNotFound, sItemName), MsgColor.Red, MsgType.Hint);
                    break;
                }
            }
        }
    }
}