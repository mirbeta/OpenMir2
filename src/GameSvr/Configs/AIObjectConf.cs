using GameSvr.Actor;
using GameSvr.Items;
using GameSvr.RobotPlay;
using System.Collections;
using SystemModule;
using SystemModule.Common;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Configs
{
    public class AIObjectConf : IniFile
    {
        private readonly string m_sFilePath = string.Empty;
        private readonly string m_sConfigListFileName = string.Empty;
        private readonly string m_sHeroConfigListFileName = string.Empty;

        public AIObjectConf(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadConfig(RobotPlayObject playObject)
        {
            byte nAttatckMode;
            string sLineText;
            string sMagicName;
            string sItemName;
            string sSayMsg;
            IList<string> TempList;
            TUserItem UserItem;
            TMagic Magic;
            TUserMagic UserMagic;
            StdItem StdItem;
            playObject.m_boNoDropItem = ReadBool("Info", "NoDropItem", true);// 是否掉包裹物品
            playObject.m_boNoDropUseItem = ReadBool("Info", "DropUseItem", true);// 是否掉装备
            playObject.m_nDropUseItemRate = ReadInteger("Info", "DropUseItemRate", 100);// 掉装备机率
            playObject.m_btJob = (PlayJob)ReadInteger("Info", "Job", 0);
            playObject.Gender = Enum.Parse<PlayGender>(ReadString("Info", "Gender", "0"));
            playObject.m_btHair = (byte)ReadInteger("Info", "Hair", 0);
            playObject.m_Abil.Level = (byte)ReadInteger("Info", "Level", 1);
            playObject.m_Abil.MaxExp = playObject.GetLevelExp(playObject.m_Abil.Level);
            playObject.m_boProtectStatus = ReadBool("Info", "ProtectStatus", false);// 是否守护模式
            nAttatckMode = (byte)ReadInteger("Info", "AttatckMode", 6);// 攻击模式
            if (nAttatckMode >= 0 && nAttatckMode <= 6)
            {
                playObject.m_btAttatckMode = (AttackMode)nAttatckMode;
            }
            sLineText = ReadString("Info", "UseSkill", "");
            if (sLineText != "")
            {
                TempList = new List<string>();
                try
                {
                    //HUtil32.ArrestStringEx(new char[] { '|', '\\', '/', ',' }, new object[] { }, sLineText, TempList);
                    TempList = sLineText.Split(",").ToList();
                    for (var i = 0; i < TempList.Count; i++)
                    {
                        sMagicName = TempList[i].Trim();
                        if (playObject.FindMagic(sMagicName) == null)
                        {
                            Magic = M2Share.UserEngine.FindMagic(sMagicName);
                            if (Magic != null)
                            {
                                if (Magic.btJob == 99 || Magic.btJob == (byte)playObject.m_btJob)
                                {
                                    UserMagic = new TUserMagic();
                                    UserMagic.MagicInfo = Magic;
                                    UserMagic.wMagIdx = Magic.wMagicID;
                                    UserMagic.btLevel = 3;
                                    UserMagic.btKey = 0;
                                    UserMagic.nTranPoint = Magic.MaxTrain[3];
                                    playObject.m_MagicList.Add(UserMagic);
                                }
                            }
                        }
                    }
                }
                finally
                {
                    //TempList.Free;
                }
            }
            sLineText = ReadString("Info", "InitItems", "");
            if (sLineText != "")
            {
                TempList = new List<string>();
                try
                {
                    //ExtractStrings(new char[] { '|', '\\', '/', ',' }, new object[] { }, sLineText, TempList);
                    TempList = sLineText.Split(",").ToList();
                    for (var i = 0; i < TempList.Count; i++)
                    {
                        sItemName = TempList[i].Trim();
                        StdItem = M2Share.UserEngine.GetStdItem(sItemName);
                        if (StdItem != null)
                        {
                            UserItem = new TUserItem();
                            if (M2Share.UserEngine.CopyToUserItemFromName(sItemName, ref UserItem))
                            {
                                if (new ArrayList(new byte[] { 15, 19, 20, 21, 22, 23, 24, 26 }).Contains(StdItem.StdMode))
                                {
                                    if (StdItem.Shape == 130 || StdItem.Shape == 131 || StdItem.Shape == 132)
                                    {
                                        //M2Share.UserEngine.GetUnknowItemValue(UserItem);
                                    }
                                }
                                if (!playObject.AddItemToBag(UserItem))
                                {
                                    UserItem = null;
                                    break;
                                }
                                playObject.m_BagItemNames.Add(StdItem.Name);
                            }
                            else
                            {
                                UserItem = null;
                            }
                        }
                    }
                }
                finally
                {
                    //TempList.Free;
                }
            }
            for (var i = 0; i < 9; i++)
            {
                sSayMsg = ReadString("MonSay", i.ToString(), "");
                if (sSayMsg != "")
                {
                    playObject.m_AISayMsgList.Add(sSayMsg);
                }
                else
                {
                    break;
                }
            }
            playObject.m_UseItemNames[Grobal2.U_DRESS] = ReadString("UseItems", "UseItems0", "布衣(男)"); // '衣服';
            playObject.m_UseItemNames[Grobal2.U_WEAPON] = ReadString("UseItems", "UseItems1", "木剑"); // '武器';
            playObject.m_UseItemNames[Grobal2.U_RIGHTHAND] = ReadString("UseItems", "UseItems2", ""); // '照明物';
            playObject.m_UseItemNames[Grobal2.U_NECKLACE] = ReadString("UseItems", "UseItems3", ""); // '项链';
            playObject.m_UseItemNames[Grobal2.U_HELMET] = ReadString("UseItems", "UseItems4", ""); // '头盔';
            playObject.m_UseItemNames[Grobal2.U_ARMRINGL] = ReadString("UseItems", "UseItems5", ""); // '左手镯';
            playObject.m_UseItemNames[Grobal2.U_ARMRINGR] = ReadString("UseItems", "UseItems6", ""); // '右手镯';
            playObject.m_UseItemNames[Grobal2.U_RINGL] = ReadString("UseItems", "UseItems7", ""); // '左戒指';
            playObject.m_UseItemNames[Grobal2.U_RINGR] = ReadString("UseItems", "UseItems8", ""); // '右戒指';
            for (var i = Grobal2.U_DRESS; i <= Grobal2.U_CHARM; i++)
            {
                if (!string.IsNullOrEmpty(playObject.m_UseItemNames[i]))
                {
                    StdItem = M2Share.UserEngine.GetStdItem(playObject.m_UseItemNames[i]);
                    if (StdItem != null)
                    {
                        UserItem = new TUserItem();
                        if (M2Share.UserEngine.CopyToUserItemFromName(playObject.m_UseItemNames[i], ref UserItem))
                        {
                            if (new ArrayList(new byte[] { 15, 19, 20, 21, 22, 23, 24, 26 }).Contains(StdItem.StdMode))
                            {
                                if (StdItem.Shape == 130 || StdItem.Shape == 131 || StdItem.Shape == 132)
                                {
                                    //M2Share.UserEngine.GetUnknowItemValue(UserItem);
                                }
                            }
                        }
                        playObject.m_UseItems[i] = UserItem;
                        UserItem = null;
                    }
                }
            }
        }

    }
}