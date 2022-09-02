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
            StdItem StdItem;
            if (nWhere >= 0 && nWhere <= 12 && nValueType >= 0 && nValueType <= 15 && nValue >= 0 && nValue <= 255)
            {
                if (PlayObject.m_UseItems[nWhere].wIndex > 0)
                {
                    StdItem = M2Share.UserEngine.GetStdItem(PlayObject.m_UseItems[nWhere].wIndex);
                    if (StdItem == null)
                    {
                        return;
                    }
                    PlayObject.m_UseItems[nWhere].btValue[9] = (byte)HUtil32._MIN(255, PlayObject.m_UseItems[nWhere].btValue[9] + 1);// 累积升级次数
                    if (nValueType == 16 && StdItem.Shape == 188)// 吸伤属性
                    {
                        PlayObject.m_UseItems[nWhere].btValue[20] = Convert.ToByte(nValue);
                        if (PlayObject.m_UseItems[nWhere].btValue[20] > 100)
                        {
                            PlayObject.m_UseItems[nWhere].btValue[20] = 100;
                        }
                    }
                    else if (nValueType > 13 && nValueType < 16)
                    {
                        nValue = HUtil32._MIN(65, nValue);
                        if (nValueType == 14)
                        {
                            PlayObject.m_UseItems[nWhere].Dura = Convert.ToUInt16(nValue * 1000);
                        }
                        if (nValueType == 15)
                        {
                            PlayObject.m_UseItems[nWhere].DuraMax = Convert.ToUInt16(nValue * 1000);
                        }
                    }
                    else
                    {
                        PlayObject.m_UseItems[nWhere].btValue[nValueType] = Convert.ToByte(nValue);
                    }
                    PlayObject.RecalcAbilitys();
                    PlayObject.SendUpdateItem(PlayObject.m_UseItems[nWhere]);
                    sShowMsg = PlayObject.m_UseItems[nWhere].wIndex.ToString() + '-' + PlayObject.m_UseItems[nWhere].MakeIndex + ' ' + PlayObject.m_UseItems[nWhere].Dura + '/'
                        + PlayObject.m_UseItems[nWhere].DuraMax + ' ' + PlayObject.m_UseItems[nWhere].btValue[0] + '/'
                        + PlayObject.m_UseItems[nWhere].btValue[1] + '/' + PlayObject.m_UseItems[nWhere].btValue[2] + '/'
                        + PlayObject.m_UseItems[nWhere].btValue[3] + '/' + PlayObject.m_UseItems[nWhere].btValue[4] + '/' + PlayObject.m_UseItems[nWhere].btValue[5]
                        + '/' + PlayObject.m_UseItems[nWhere].btValue[6] + '/' + PlayObject.m_UseItems[nWhere].btValue[7] + '/' + PlayObject.m_UseItems[nWhere].btValue[8]
                        + '/' + PlayObject.m_UseItems[nWhere].btValue[9] + '/' + PlayObject.m_UseItems[nWhere].btValue[ItemAttr.WeaponUpgrade] + '/' + PlayObject.m_UseItems[nWhere].btValue[11]
                        + '/' + PlayObject.m_UseItems[nWhere].btValue[12] + '/' + PlayObject.m_UseItems[nWhere].btValue[13];
                    PlayObject.SysMsg(sShowMsg, MsgColor.Blue, MsgType.Hint);
                    if (M2Share.g_Config.boShowMakeItemMsg)
                    {
                        M2Share.MainOutMessage("[物品调整] " + PlayObject.m_sCharName + '(' + StdItem.Name + " -> " + sShowMsg + ')');
                    }
                }
                else
                {
                    PlayObject.SysMsg(GameCommandConst.g_sGamecommandSuperMakeHelpMsg, MsgColor.Red, MsgType.Hint);
                }
            }
        }
    }
}