using GameSvr.Items;
using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 给指定纯度的矿石
    /// </summary>
    [Command("GiveMine", "给指定纯度的矿石", "矿石名称 数量 持久", 10)]
    public class GiveMineCommand : Commond
    {
        [ExecuteCommand]
        public void GiveMine(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sMineName = @Params.Length > 0 ? @Params[0] : "";
            var nMineCount = @Params.Length > 0 ? int.Parse(@Params[1]) : 0;
            var nDura = @Params.Length > 0 ? int.Parse(@Params[2]) : 0;
            if (PlayObject.Permission < this.GameCommand.nPermissionMin)
            {
                PlayObject.SysMsg(CommandHelp.GameCommandPermissionTooLow, MsgColor.Red, MsgType.Hint);
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
                var UserItem = new UserItem();
                if (M2Share.WorldEngine.CopyToUserItemFromName(sMineName, ref UserItem))
                {
                    var StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                    if (StdItem != null && StdItem.StdMode == 43)
                    {
                        if (PlayObject.IsAddWeightAvailable(StdItem.Weight * nMineCount))
                        {
                            UserItem.Dura = Convert.ToUInt16(nDura * 1000);
                            if (UserItem.Dura > UserItem.DuraMax)
                            {
                                UserItem.Dura = UserItem.DuraMax;
                            }
                            PlayObject.ItemList.Add(UserItem);
                            PlayObject.SendAddItem(UserItem);
                            if (StdItem.NeedIdentify == 1)
                            {
                                M2Share.AddGameDataLog("5" + "\09" + PlayObject.MapName + "\09" + PlayObject.CurrX + "\09" + PlayObject.CurrY + "\09" +
                                                       PlayObject.ChrName + "\09" + StdItem.Name + "\09" + UserItem.MakeIndex + "\09" + UserItem.Dura + "/"
                                    + UserItem.DuraMax + "\09" + PlayObject.ChrName);
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