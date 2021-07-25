using System;
using SystemModule;
using M2Server;
using M2Server.CommandSystem;

namespace M2Servers
{
    /// <summary>
    /// 给指定纯度的矿石
    /// 命令格式:GIVEMINE 矿名称 数量 纯度
    /// </summary>
    [GameCommand("GiveMine", "给指定纯度的矿石", 10)]
    public class GiveMineCommand : BaseCommond
    {
        [DefaultCommand]
        public void GiveMine(string[] @Params, TPlayObject PlayObject)
        {
            var sMINEName= @Params.Length > 0 ? @Params[0] : "";
            var nMineCount= @Params.Length > 0 ? int.Parse(@Params[1]) : 0;
            var nDura= @Params.Length > 0 ? int.Parse(@Params[2]) : 0;
            TUserItem UserItem = null;
            TItem StdItem;
            if (PlayObject.m_btPermission < this.Attributes.nPermissionMin)
            {
                PlayObject.SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (sMINEName == "" || sMINEName != "" && sMINEName[0] == '?' || nMineCount <= 0)
            {
                //PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, M2Share.g_sGameCommandGIVEMINEHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (nDura <= 0)
            {
                nDura = M2Share.RandomNumber.Random(18) + 3;
            }
            // 如纯度不填,则随机给纯度
            for (var i = 0; i < nMineCount; i++)
            {
                UserItem = new TUserItem();
                if (M2Share.UserEngine.CopyToUserItemFromName(sMINEName, ref UserItem))
                {
                    StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    if (StdItem != null && StdItem.StdMode == 43)
                    {
                        if (PlayObject.IsAddWeightAvailable(StdItem.Weight * nMineCount))
                        {
                            UserItem.Dura = Convert.ToUInt16(nDura * 1000);
                            if (UserItem.Dura > UserItem.DuraMax)
                            {
                                UserItem.Dura = UserItem.DuraMax;
                            }
                            PlayObject. m_ItemList.Add(UserItem);
                            PlayObject.  SendAddItem(UserItem);
                            if (StdItem.NeedIdentify == 1)
                            {
                                M2Share.AddGameDataLog("5" + "\09" + PlayObject.m_sMapName + "\09" + PlayObject.m_nCurrX + "\09" + PlayObject.m_nCurrY + "\09" +
                                                       PlayObject.  m_sCharName + "\09" + StdItem.Name + "\09" + UserItem.MakeIndex + "\09" + UserItem.Dura + "/"
                                    + UserItem.DuraMax + "\09" + PlayObject.m_sCharName);
                            }
                        }
                    }
                }
                else
                {
                    UserItem = null;
                    break;
                }
            }
        }
    }
}