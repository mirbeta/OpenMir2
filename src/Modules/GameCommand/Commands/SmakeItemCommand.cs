using M2Server.Items;
using M2Server.Player;
using SystemModule;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 调整身上装备附加属性
    /// </summary>
    [Command("SmakeItem", "调整身上装备附加属性", 10)]
    public class SmakeItemCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var nWhere = @params.Length > 0 ? HUtil32.StrToInt(@params[0], 0) : 0;
            var nValueType = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0;//参数16为吸伤属性
            var nValue = @params.Length > 2 ? HUtil32.StrToInt(@params[2],0) : 0;
            string sShowMsg;
            if (nWhere >= 0 && nWhere <= 12 && nValueType >= 0 && nValueType <= 15 && nValue >= 0 && nValue <= 255) {
                if (playObject.UseItems[nWhere].Index > 0) {
                    var stdItem = ItemSystem.GetStdItem(playObject.UseItems[nWhere].Index);
                    if (stdItem == null) {
                        return;
                    }
                    playObject.UseItems[nWhere].Desc[9] = (byte)HUtil32._MIN(255, playObject.UseItems[nWhere].Desc[9] + 1);// 累积升级次数
                    if (nValueType == 16 && stdItem.Shape == 188)// 吸伤属性
                    {
                        playObject.UseItems[nWhere].Desc[20] = Convert.ToByte(nValue);
                        if (playObject.UseItems[nWhere].Desc[20] > 100) {
                            playObject.UseItems[nWhere].Desc[20] = 100;
                        }
                    }
                    else if (nValueType > 13 && nValueType < 16) {
                        nValue = HUtil32._MIN(65, nValue);
                        if (nValueType == 14) {
                            playObject.UseItems[nWhere].Dura = Convert.ToUInt16(nValue * 1000);
                        }
                        if (nValueType == 15) {
                            playObject.UseItems[nWhere].DuraMax = Convert.ToUInt16(nValue * 1000);
                        }
                    }
                    else {
                        playObject.UseItems[nWhere].Desc[nValueType] = Convert.ToByte(nValue);
                    }
                    playObject.RecalcAbilitys();
                    playObject.SendUpdateItem(playObject.UseItems[nWhere]);
                    sShowMsg = playObject.UseItems[nWhere].Index.ToString() + '-' + playObject.UseItems[nWhere].MakeIndex + ' ' + playObject.UseItems[nWhere].Dura + '/'
                        + playObject.UseItems[nWhere].DuraMax + ' ' + playObject.UseItems[nWhere].Desc[0] + '/'
                        + playObject.UseItems[nWhere].Desc[1] + '/' + playObject.UseItems[nWhere].Desc[2] + '/'
                        + playObject.UseItems[nWhere].Desc[3] + '/' + playObject.UseItems[nWhere].Desc[4] + '/' + playObject.UseItems[nWhere].Desc[5]
                        + '/' + playObject.UseItems[nWhere].Desc[6] + '/' + playObject.UseItems[nWhere].Desc[7] + '/' + playObject.UseItems[nWhere].Desc[8]
                        + '/' + playObject.UseItems[nWhere].Desc[9] + '/' + playObject.UseItems[nWhere].Desc[ItemAttr.WeaponUpgrade] + '/' + playObject.UseItems[nWhere].Desc[11]
                        + '/' + playObject.UseItems[nWhere].Desc[12] + '/' + playObject.UseItems[nWhere].Desc[13];
                    playObject.SysMsg(sShowMsg, MsgColor.Blue, MsgType.Hint);
                    if (M2Share.Config.ShowMakeItemMsg) {
                        M2Share.Logger.Warn("[物品调整] " + playObject.ChrName + '(' + stdItem.Name + " -> " + sShowMsg + ')');
                    }
                }
                else {
                    playObject.SysMsg(CommandHelp.GamecommandSuperMakeHelpMsg, MsgColor.Red, MsgType.Hint);
                }
            }
        }
    }
}