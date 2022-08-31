using GameSvr.Items;
using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 给指定纯度的矿石
    /// </summary>
    [GameCommand("GiveMine", "给指定纯度的矿石", "矿石名称 数量 持久", 10)]
    public class GiveMineCommand : BaseCommond
    {
        [DefaultCommand]
        public void GiveMine(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sMineName = @Params.Length > 0 ? @Params[0] : "";
            var nMineCount = @Params.Length > 0 ? int.Parse(@Params[1]) : 0;
            var nDura = @Params.Length > 0 ? int.Parse(@Params[2]) : 0;
            if (PlayObject.m_btPermission < this.GameCommand.nPermissionMin)
            {
                PlayObject.SysMsg(GameCommandConst.g_sGameCommandPermissionTooLow, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (sMineName == "" || sMineName != "" && sMineName[0] == '?' || nMineCount <= 0)
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (nDura <= 0)
            {
                nDura = M2Share.RandomNumber.Random(18) + 3;
            }
            // 如纯度不填,则随机给纯度
            for (var i = 0; i < nMineCount; i++)
            {
                var UserItem = new TUserItem();
                if (M2Share.UserEngine.CopyToUserItemFromName(sMineName, ref UserItem))
                {
                    var StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    if (StdItem != null && StdItem.StdMode == 43)
                    {
                        if (PlayObject.IsAddWeightAvailable(StdItem.Weight * nMineCount))
                        {
                            UserItem.Dura = Convert.ToUInt16(nDura * 1000);
                            if (UserItem.Dura > UserItem.DuraMax)
                            {
                                UserItem.Dura = UserItem.DuraMax;
                            }
                            PlayObject.m_ItemList.Add(UserItem);
                            PlayObject.SendAddItem(UserItem);
                            if (StdItem.NeedIdentify == 1)
                            {
                                M2Share.AddGameDataLog("5" + "\09" + PlayObject.m_sMapName + "\09" + PlayObject.m_nCurrX + "\09" + PlayObject.m_nCurrY + "\09" +
                                                       PlayObject.m_sCharName + "\09" + StdItem.Name + "\09" + UserItem.MakeIndex + "\09" + UserItem.Dura + "/"
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