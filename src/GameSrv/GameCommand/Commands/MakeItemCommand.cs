using GameSrv.Items;
using GameSrv.Player;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.GameCommand.Commands
{
    /// <summary>
    /// 造指定物品(支持权限分配，小于最大权限受允许、禁止制造列表限制)
    /// 要求权限默认等级：10
    /// </summary>
    [Command("Make", desc: "制造指定物品(支持权限分配，小于最大权限受允许、禁止制造列表限制)", help: CommandHelp.GamecommandMakeHelpMsg, minUserLevel: 10)]
    public class MakeItemCommond : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject)
        {
            if (@params == null) return;
            var sItemName = @params.Length > 0 ? @params[0] : ""; //物品名称
            var nCount = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 1; //数量
            var sParam = @params.Length > 2 ? @params[2] : ""; //可选参数（持久力）
            if (string.IsNullOrEmpty(sItemName))
            {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (nCount <= 0) nCount = 1;
            if (nCount > 10) nCount = 10;
            if (playObject.Permission < Command.PermissionMax)
            {
                if (!M2Share.CanMakeItem(sItemName))
                {
                    playObject.SysMsg(CommandHelp.GamecommandMakeItemNameOrPerMissionNot, MsgColor.Red, MsgType.Hint);
                    return;
                }
                if (M2Share.CastleMgr.InCastleWarArea(playObject) != null) // 攻城区域，禁止使用此功能
                {
                    playObject.SysMsg(CommandHelp.GamecommandMakeInCastleWarRange, MsgColor.Red, MsgType.Hint);
                    return;
                }
                if (!playObject.InSafeZone())
                {
                    playObject.SysMsg(CommandHelp.GamecommandMakeInSafeZoneRange, MsgColor.Red, MsgType.Hint);
                    return;
                }
                nCount = 1;
            }
            for (var i = 0; i < nCount; i++)
            {
                if (playObject.ItemList.Count >= Grobal2.MaxBagItem) return;
                UserItem userItem = null;
                if (M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref userItem))
                {
                    var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                    if (stdItem.Price >= 15000 && !M2Share.Config.TestServer && playObject.Permission < 5)
                    {
                        return;
                    }
                    if (M2Share.RandomNumber.Random(M2Share.Config.MakeRandomAddValue) == 0)
                    {
                        ItemSystem.RandomUpgradeItem(stdItem, userItem);
                    }
                    if (playObject.Permission >= Command.PermissionMax)
                    {
                        userItem.MakeIndex = M2Share.GetItemNumberEx(); // 制造的物品另行取得物品ID
                    }
                    playObject.ItemList.Add(userItem);
                    playObject.SendAddItem(userItem);
                    if (playObject.Permission >= 6)
                    {
                        M2Share.Logger.Warn("[制造物品] " + playObject.ChrName + " " + sItemName + "(" + userItem.MakeIndex + ")");
                    }
                    if (stdItem.NeedIdentify == 1)
                    {
                        M2Share.EventSource.AddEventLog(5, playObject.MapName + "\09" + playObject.CurrX +
                           "\09" + playObject.CurrY + "\09" + playObject.ChrName + "\09" + stdItem.Name + "\09" + userItem.MakeIndex + "\09" + "(" +
                           HUtil32.LoByte(stdItem.DC) + "/" + HUtil32.HiByte(stdItem.DC) + ")" + "(" + HUtil32.LoByte(stdItem.MC) + "/" + HUtil32.HiByte(stdItem.MC) + ")" + "(" +
                           HUtil32.LoByte(stdItem.SC) + "/" + HUtil32.HiByte(stdItem.SC) + ")" + "(" + HUtil32.LoByte(stdItem.AC) + "/" +
                           HUtil32.HiByte(stdItem.AC) + ")" + "(" + HUtil32.LoByte(stdItem.MAC) + "/" + HUtil32.HiByte(stdItem.MAC) + ")" + userItem.Desc[0]
                           + "/" + userItem.Desc[1] + "/" + userItem.Desc[2] + "/" + userItem.Desc[3] + "/" + userItem.Desc[4] + "/" + userItem.Desc[5] + "/" + userItem.Desc[6]
                           + "/" + userItem.Desc[7] + "/" + userItem.Desc[8] + "/" + userItem.Desc[14] + "\09" + playObject.ChrName);
                    }
                }
                else
                {
                    userItem = null;
                    playObject.SysMsg(string.Format(CommandHelp.GamecommandMakeItemNameNotFound, sItemName), MsgColor.Red, MsgType.Hint);
                    break;
                }
            }
        }
    }
}