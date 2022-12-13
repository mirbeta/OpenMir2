using GameSvr.Player;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 造指定物品(支持权限分配，小于最大权限受允许、禁止制造列表限制)
    /// 要求权限默认等级：10
    /// </summary>
    [Command("Make", desc: "制造指定物品(支持权限分配，小于最大权限受允许、禁止制造列表限制)", help: CommandHelp.GamecommandMakeHelpMsg, minUserLevel: 10)]
    public class MakeItemCommond : Command
    {
        [ExecuteCommand]
        public void CmdMakeItem(string[] Params, PlayObject PlayObject)
        {
            if (Params == null) return;
            var sItemName = Params.Length > 0 ? Params[0] : ""; //物品名称
            var nCount = Params.Length > 1 ? Convert.ToInt32(Params[1]) : 1; //数量
            var sParam = Params.Length > 2 ? Params[2] : ""; //可选参数（持久力）
            if (string.IsNullOrEmpty(sItemName))
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (nCount <= 0) nCount = 1;
            if (nCount > 10) nCount = 10;
            if (PlayObject.Permission < GameCommand.nPermissionMax)
            {
                if (!M2Share.CanMakeItem(sItemName))
                {
                    PlayObject.SysMsg(CommandHelp.GamecommandMakeItemNameOrPerMissionNot, MsgColor.Red, MsgType.Hint);
                    return;
                }
                if (M2Share.CastleMgr.InCastleWarArea(PlayObject) != null) // 攻城区域，禁止使用此功能
                {
                    PlayObject.SysMsg(CommandHelp.GamecommandMakeInCastleWarRange, MsgColor.Red, MsgType.Hint);
                    return;
                }
                if (!PlayObject.InSafeZone())
                {
                    PlayObject.SysMsg(CommandHelp.GamecommandMakeInSafeZoneRange, MsgColor.Red, MsgType.Hint);
                    return;
                }
                nCount = 1;
            }
            for (var i = 0; i < nCount; i++)
            {
                if (PlayObject.ItemList.Count >= Grobal2.MAXBAGITEM) return;
                UserItem UserItem = null;
                if (M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref UserItem))
                {
                    var stdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                    if (stdItem.Price >= 15000 && !M2Share.Config.TestServer && PlayObject.Permission < 5)
                    {
                        return;
                    }
                    if (M2Share.RandomNumber.Random(M2Share.Config.MakeRandomAddValue) == 0)
                    {
                        stdItem.RandomUpgradeItem(UserItem);
                    }
                    if (PlayObject.Permission >= GameCommand.nPermissionMax)
                    {
                        UserItem.MakeIndex = M2Share.GetItemNumberEx(); // 制造的物品另行取得物品ID
                    }
                    PlayObject.ItemList.Add(UserItem);
                    PlayObject.SendAddItem(UserItem);
                    if (PlayObject.Permission >= 6)
                    {
                         M2Share.Log.Warn("[制造物品] " + PlayObject.ChrName + " " + sItemName + "(" + UserItem.MakeIndex + ")");
                    }
                    if (stdItem.NeedIdentify == 1)
                    {
                        M2Share.EventSource.AddEventLog(5, PlayObject.MapName + "\09" + PlayObject.CurrX +
                           "\09" + PlayObject.CurrY + "\09" + PlayObject.ChrName + "\09" + stdItem.Name + "\09" + UserItem.MakeIndex + "\09" + "(" +
                           HUtil32.LoByte(stdItem.DC) + "/" + HUtil32.HiByte(stdItem.DC) + ")" + "(" + HUtil32.LoByte(stdItem.MC) + "/" + HUtil32.HiByte(stdItem.MC) + ")" + "(" +
                           HUtil32.LoByte(stdItem.SC) + "/" + HUtil32.HiByte(stdItem.SC) + ")" + "(" + HUtil32.LoByte(stdItem.AC) + "/" +
                           HUtil32.HiByte(stdItem.AC) + ")" + "(" + HUtil32.LoByte(stdItem.MAC) + "/" + HUtil32.HiByte(stdItem.MAC) + ")" + UserItem.Desc[0]
                           + "/" + UserItem.Desc[1] + "/" + UserItem.Desc[2] + "/" + UserItem.Desc[3] + "/" + UserItem.Desc[4] + "/" + UserItem.Desc[5] + "/" + UserItem.Desc[6]
                           + "/" + UserItem.Desc[7] + "/" + UserItem.Desc[8] + "/" + UserItem.Desc[14] + "\09" + PlayObject.ChrName);
                    }
                }
                else
                {
                    UserItem = null;
                    PlayObject.SysMsg(string.Format(CommandHelp.GamecommandMakeItemNameNotFound, sItemName), MsgColor.Red, MsgType.Hint);
                    break;
                }
            }
        }
    }
}