using OpenMir2;
using OpenMir2.Packets.ClientPackets;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 造指定物品(支持权限分配，小于最大权限受允许、禁止制造列表限制)
    /// 要求权限默认等级：10
    /// </summary>
    [Command("Make", desc: "制造指定物品(支持权限分配，小于最大权限受允许、禁止制造列表限制)", help: CommandHelp.GamecommandMakeHelpMsg, minUserLevel: 10)]
    public class MakeItemCommond : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }

            string sItemName = @params.Length > 0 ? @params[0] : ""; //物品名称
            int nCount = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 1; //数量
            string sParam = @params.Length > 2 ? @params[2] : ""; //可选参数（持久力）
            if (string.IsNullOrEmpty(sItemName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (nCount <= 0)
            {
                nCount = 1;
            }

            if (nCount > 10)
            {
                nCount = 10;
            }

            if (PlayerActor.Permission < Command.PermissionMax)
            {
                if (!SystemShare.CanMakeItem(sItemName))
                {
                    PlayerActor.SysMsg(CommandHelp.GamecommandMakeItemNameOrPerMissionNot, MsgColor.Red, MsgType.Hint);
                    return;
                }
                if (SystemShare.CastleMgr.InCastleWarArea(PlayerActor) != null) // 攻城区域，禁止使用此功能
                {
                    PlayerActor.SysMsg(CommandHelp.GamecommandMakeInCastleWarRange, MsgColor.Red, MsgType.Hint);
                    return;
                }
                if (!PlayerActor.InSafeZone())
                {
                    PlayerActor.SysMsg(CommandHelp.GamecommandMakeInSafeZoneRange, MsgColor.Red, MsgType.Hint);
                    return;
                }
                nCount = 1;
            }
            for (int i = 0; i < nCount; i++)
            {
                if (PlayerActor.ItemList.Count >= Grobal2.MaxBagItem)
                {
                    return;
                }

                UserItem userItem = null;
                if (SystemShare.EquipmentSystem.CopyToUserItemFromName(sItemName, ref userItem))
                {
                    OpenMir2.Data.StdItem stdItem = SystemShare.EquipmentSystem.GetStdItem(userItem.Index);
                    if (stdItem.Price >= 15000 && !SystemShare.Config.TestServer && PlayerActor.Permission < 5)
                    {
                        return;
                    }
                    if (SystemShare.RandomNumber.Random(SystemShare.Config.MakeRandomAddValue) == 0)
                    {
                        // ModuleShare.ItemSystem.RandomUpgradeItem(stdItem, userItem);
                    }
                    if (PlayerActor.Permission >= Command.PermissionMax)
                    {
                        userItem.MakeIndex = SystemShare.GetItemNumberEx(); // 制造的物品另行取得物品ID
                    }
                    PlayerActor.ItemList.Add(userItem);
                    PlayerActor.SendAddItem(userItem);
                    if (PlayerActor.Permission >= 6)
                    {
                        LogService.Warn("[制造物品] " + PlayerActor.ChrName + " " + sItemName + "(" + userItem.MakeIndex + ")");
                    }
                    if (stdItem.NeedIdentify == 1)
                    {
                        /*M2Share.EventSource.AddEventLog(5, PlayerActor.MapName + "\09" + PlayerActor.CurrX +
                           "\09" + PlayerActor.CurrY + "\09" + PlayerActor.ChrName + "\09" + stdItem.Name + "\09" + userItem.MakeIndex + "\09" + "(" +
                           HUtil32.LoByte(stdItem.DC) + "/" + HUtil32.HiByte(stdItem.DC) + ")" + "(" + HUtil32.LoByte(stdItem.MC) + "/" + HUtil32.HiByte(stdItem.MC) + ")" + "(" +
                           HUtil32.LoByte(stdItem.SC) + "/" + HUtil32.HiByte(stdItem.SC) + ")" + "(" + HUtil32.LoByte(stdItem.AC) + "/" +
                           HUtil32.HiByte(stdItem.AC) + ")" + "(" + HUtil32.LoByte(stdItem.MAC) + "/" + HUtil32.HiByte(stdItem.MAC) + ")" + userItem.Desc[0]
                           + "/" + userItem.Desc[1] + "/" + userItem.Desc[2] + "/" + userItem.Desc[3] + "/" + userItem.Desc[4] + "/" + userItem.Desc[5] + "/" + userItem.Desc[6]
                           + "/" + userItem.Desc[7] + "/" + userItem.Desc[8] + "/" + userItem.Desc[14] + "\09" + PlayerActor.ChrName);*/
                    }
                }
                else
                {
                    userItem = null;
                    PlayerActor.SysMsg(string.Format(CommandHelp.GamecommandMakeItemNameNotFound, sItemName), MsgColor.Red, MsgType.Hint);
                    break;
                }
            }
        }
    }
}