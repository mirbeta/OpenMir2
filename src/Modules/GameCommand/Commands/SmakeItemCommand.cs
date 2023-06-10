using SystemModule;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace CommandModule.Commands
{
    /// <summary>
    /// 调整身上装备附加属性
    /// </summary>
    [Command("SmakeItem", "调整身上装备附加属性", 10)]
    public class SmakeItemCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var nWhere = @params.Length > 0 ? HUtil32.StrToInt(@params[0], 0) : 0;
            var nValueType = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0;//参数16为吸伤属性
            var nValue = @params.Length > 2 ? HUtil32.StrToInt(@params[2], 0) : 0;
            string sShowMsg;
            if (nWhere >= 0 && nWhere <= 12 && nValueType >= 0 && nValueType <= 15 && nValue >= 0 && nValue <= 255)
            {
                if (PlayerActor.UseItems[nWhere].Index > 0)
                {
                    var stdItem = SystemShare.ItemSystem.GetStdItem(PlayerActor.UseItems[nWhere].Index);
                    if (stdItem == null)
                    {
                        return;
                    }
                    PlayerActor.UseItems[nWhere].Desc[9] = (byte)HUtil32._MIN(255, PlayerActor.UseItems[nWhere].Desc[9] + 1);// 累积升级次数
                    if (nValueType == 16 && stdItem.Shape == 188)// 吸伤属性
                    {
                        PlayerActor.UseItems[nWhere].Desc[20] = Convert.ToByte(nValue);
                        if (PlayerActor.UseItems[nWhere].Desc[20] > 100)
                        {
                            PlayerActor.UseItems[nWhere].Desc[20] = 100;
                        }
                    }
                    else if (nValueType > 13 && nValueType < 16)
                    {
                        nValue = HUtil32._MIN(65, nValue);
                        if (nValueType == 14)
                        {
                            PlayerActor.UseItems[nWhere].Dura = Convert.ToUInt16(nValue * 1000);
                        }
                        if (nValueType == 15)
                        {
                            PlayerActor.UseItems[nWhere].DuraMax = Convert.ToUInt16(nValue * 1000);
                        }
                    }
                    else
                    {
                        PlayerActor.UseItems[nWhere].Desc[nValueType] = Convert.ToByte(nValue);
                    }
                    PlayerActor.RecalcAbilitys();
                    PlayerActor.SendUpdateItem(PlayerActor.UseItems[nWhere]);
                    sShowMsg = PlayerActor.UseItems[nWhere].Index.ToString() + '-' + PlayerActor.UseItems[nWhere].MakeIndex + ' ' + PlayerActor.UseItems[nWhere].Dura + '/'
                        + PlayerActor.UseItems[nWhere].DuraMax + ' ' + PlayerActor.UseItems[nWhere].Desc[0] + '/'
                        + PlayerActor.UseItems[nWhere].Desc[1] + '/' + PlayerActor.UseItems[nWhere].Desc[2] + '/'
                        + PlayerActor.UseItems[nWhere].Desc[3] + '/' + PlayerActor.UseItems[nWhere].Desc[4] + '/' + PlayerActor.UseItems[nWhere].Desc[5]
                        + '/' + PlayerActor.UseItems[nWhere].Desc[6] + '/' + PlayerActor.UseItems[nWhere].Desc[7] + '/' + PlayerActor.UseItems[nWhere].Desc[8]
                        + '/' + PlayerActor.UseItems[nWhere].Desc[9] + '/' + PlayerActor.UseItems[nWhere].Desc[ItemAttr.WeaponUpgrade] + '/' + PlayerActor.UseItems[nWhere].Desc[11]
                        + '/' + PlayerActor.UseItems[nWhere].Desc[12] + '/' + PlayerActor.UseItems[nWhere].Desc[13];
                    PlayerActor.SysMsg(sShowMsg, MsgColor.Blue, MsgType.Hint);
                    if (SystemShare.Config.ShowMakeItemMsg)
                    {
                        SystemShare.Logger.Warn("[物品调整] " + PlayerActor.ChrName + '(' + stdItem.Name + " -> " + sShowMsg + ')');
                    }
                }
                else
                {
                    PlayerActor.SysMsg(CommandHelp.GamecommandSuperMakeHelpMsg, MsgColor.Red, MsgType.Hint);
                }
            }
        }
    }
}