using GameSvr.Items;
using GameSvr.Player;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整身上装备附加属性
    /// </summary>
    [GameCommand("SmakeItem", "调整身上装备附加属性", 10)]
    public class SmakeItemCommand : BaseCommond
    {
        [DefaultCommand]
        public void SmakeItem(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var nWhere = @Params.Length > 0 ? int.Parse(@Params[0]) : 0;
            var nValueType = @Params.Length > 1 ? int.Parse(@Params[1]) : 0;//参数16为吸伤属性
            var nValue = @Params.Length > 2 ? int.Parse(@Params[2]) : 0;
            string sShowMsg;
            Equipment StdItem = null;
            if (nWhere >= 0 && nWhere <= 12 && nValueType >= 0 && nValueType <= 15 && nValue >= 0 && nValue <= 255)
            {
                if (PlayObject.UseItems[nWhere].wIndex > 0)
                {
                    StdItem = M2Share.WorldEngine.GetStdItem(PlayObject.UseItems[nWhere].wIndex);
                    if (StdItem == null)
                    {
                        return;
                    }
                    PlayObject.UseItems[nWhere].btValue[9] = (byte)HUtil32._MIN(255, PlayObject.UseItems[nWhere].btValue[9] + 1);// 累积升级次数
                    if (nValueType == 16 && StdItem.Shape == 188)// 吸伤属性
                    {
                        PlayObject.UseItems[nWhere].btValue[20] = Convert.ToByte(nValue);
                        if (PlayObject.UseItems[nWhere].btValue[20] > 100)
                        {
                            PlayObject.UseItems[nWhere].btValue[20] = 100;
                        }
                    }
                    else if (nValueType > 13 && nValueType < 16)
                    {
                        nValue = HUtil32._MIN(65, nValue);
                        if (nValueType == 14)
                        {
                            PlayObject.UseItems[nWhere].Dura = Convert.ToUInt16(nValue * 1000);
                        }
                        if (nValueType == 15)
                        {
                            PlayObject.UseItems[nWhere].DuraMax = Convert.ToUInt16(nValue * 1000);
                        }
                    }
                    else
                    {
                        PlayObject.UseItems[nWhere].btValue[nValueType] = Convert.ToByte(nValue);
                    }
                    PlayObject.RecalcAbilitys();
                    PlayObject.SendUpdateItem(PlayObject.UseItems[nWhere]);
                    sShowMsg = PlayObject.UseItems[nWhere].wIndex.ToString() + '-' + PlayObject.UseItems[nWhere].MakeIndex + ' ' + PlayObject.UseItems[nWhere].Dura + '/'
                        + PlayObject.UseItems[nWhere].DuraMax + ' ' + PlayObject.UseItems[nWhere].btValue[0] + '/'
                        + PlayObject.UseItems[nWhere].btValue[1] + '/' + PlayObject.UseItems[nWhere].btValue[2] + '/'
                        + PlayObject.UseItems[nWhere].btValue[3] + '/' + PlayObject.UseItems[nWhere].btValue[4] + '/' + PlayObject.UseItems[nWhere].btValue[5]
                        + '/' + PlayObject.UseItems[nWhere].btValue[6] + '/' + PlayObject.UseItems[nWhere].btValue[7] + '/' + PlayObject.UseItems[nWhere].btValue[8]
                        + '/' + PlayObject.UseItems[nWhere].btValue[9] + '/' + PlayObject.UseItems[nWhere].btValue[ItemAttr.WeaponUpgrade] + '/' + PlayObject.UseItems[nWhere].btValue[11]
                        + '/' + PlayObject.UseItems[nWhere].btValue[12] + '/' + PlayObject.UseItems[nWhere].btValue[13];
                    PlayObject.SysMsg(sShowMsg, MsgColor.Blue, MsgType.Hint);
                    if (M2Share.Config.ShowMakeItemMsg)
                    {
                        M2Share.Log.Warn("[物品调整] " + PlayObject.CharName + '(' + StdItem.Name + " -> " + sShowMsg + ')');
                    }
                }
                else
                {
                    PlayObject.SysMsg(GameCommandConst.GamecommandSuperMakeHelpMsg, MsgColor.Red, MsgType.Hint);
                }
            }
        }
    }
}