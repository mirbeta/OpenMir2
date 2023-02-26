using GameSrv.Actor;
using GameSrv.Items;
using GameSrv.RobotPlay;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.Conf {
    public class RobotPlayConf : ConfigFile {
        private readonly string m_sFilePath = string.Empty;
        private readonly string m_sConfigListFileName = string.Empty;
        private readonly string m_sHeroConfigListFileName = string.Empty;

        public RobotPlayConf(string fileName) : base(fileName) {
            Load();
        }

        public void LoadConfig(RobotPlayObject playObject) {
            byte nAttatckMode;
            string sLineText;
            string sMagicName;
            string sItemName;
            string sSayMsg;
            IList<string> TempList;
            UserItem UserItem;
            MagicInfo Magic;
            UserMagic UserMagic;
            StdItem StdItem;
            playObject.NoDropItem = ReadWriteBool("Info", "NoDropItem", true);// 是否掉包裹物品
            playObject.NoDropUseItem = ReadWriteBool("Info", "DropUseItem", true);// 是否掉装备
            playObject.m_nDropUseItemRate = ReadWriteInteger("Info", "DropUseItemRate", 100);// 掉装备机率
            playObject.Job = (PlayJob)ReadWriteInteger("Info", "Job", 0);
            playObject.Gender = Enum.Parse<PlayGender>(ReadWriteString("Info", "Gender", "0"));
            playObject.Hair = (byte)ReadWriteInteger("Info", "Hair", 0);
            playObject.Abil.Level = (byte)ReadWriteInteger("Info", "Level", 1);
            playObject.Abil.MaxExp = BaseObject.GetLevelExp(playObject.Abil.Level);
            playObject.m_boProtectStatus = ReadWriteBool("Info", "ProtectStatus", false);// 是否守护模式
            nAttatckMode = (byte)ReadWriteInteger("Info", "AttatckMode", 6);// 攻击模式
            if (nAttatckMode >= 0 && nAttatckMode <= 6) {
                playObject.AttatckMode = (AttackMode)nAttatckMode;
            }
            sLineText = ReadWriteString("Info", "UseSkill", "");
            if (!string.IsNullOrEmpty(sLineText)) {
                TempList = new List<string>();
                try {
                    //HUtil32.ArrestStringEx(new char[] { '|', '\\', '/', ',' }, new object[] { }, sLineText, TempList);
                    TempList = sLineText.Split(",").ToList();
                    for (int i = 0; i < TempList.Count; i++) {
                        sMagicName = TempList[i].Trim();
                        if (playObject.FindMagic(sMagicName) == null) {
                            Magic = M2Share.WorldEngine.FindMagic(sMagicName);
                            if (Magic != null) {
                                if (Magic.Job == 99 || Magic.Job == (byte)playObject.Job) {
                                    UserMagic = new UserMagic();
                                    UserMagic.Magic = Magic;
                                    UserMagic.MagIdx = Magic.MagicId;
                                    UserMagic.Level = 3;
                                    UserMagic.Key = (char)0;
                                    UserMagic.TranPoint = Magic.MaxTrain[3];
                                    playObject.MagicList.Add(UserMagic);
                                }
                            }
                        }
                    }
                }
                finally {
                    //TempList.Free;
                }
            }
            sLineText = ReadWriteString("Info", "InitItems", "");
            if (!string.IsNullOrEmpty(sLineText)) {
                TempList = new List<string>();
                try {
                    //ExtractStrings(new char[] { '|', '\\', '/', ',' }, new object[] { }, sLineText, TempList);
                    TempList = sLineText.Split(",").ToList();
                    for (int i = 0; i < TempList.Count; i++) {
                        sItemName = TempList[i].Trim();
                        StdItem = M2Share.WorldEngine.GetStdItem(sItemName);
                        if (StdItem != null) {
                            UserItem = new UserItem();
                            if (M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref UserItem)) {
                                if (M2Share.StdModeMap.Contains(StdItem.StdMode)) {
                                    if (StdItem.Shape == 130 || StdItem.Shape == 131 || StdItem.Shape == 132) {
                                        //M2Share.WorldEngine.GetUnknowItemValue(UserItem);
                                    }
                                }
                                if (!playObject.AddItemToBag(UserItem)) {
                                    UserItem = null;
                                    break;
                                }
                                playObject.m_BagItemNames.Add(StdItem.Name);
                            }
                            else {
                                UserItem = null;
                            }
                        }
                    }
                }
                finally {
                    //TempList.Free;
                }
            }
            for (int i = 0; i < 9; i++) {
                sSayMsg = ReadWriteString("MonSay", i.ToString(), "");
                if (!string.IsNullOrEmpty(sSayMsg)) {
                    playObject.m_AISayMsgList.Add(sSayMsg);
                }
                else {
                    break;
                }
            }
            playObject.m_UseItemNames[ItemLocation.Dress] = ReadWriteString("UseItems", "UseItems0", "布衣(男)"); // '衣服';
            playObject.m_UseItemNames[ItemLocation.Weapon] = ReadWriteString("UseItems", "UseItems1", "木剑"); // '武器';
            playObject.m_UseItemNames[ItemLocation.RighThand] = ReadWriteString("UseItems", "UseItems2", ""); // '照明物';
            playObject.m_UseItemNames[ItemLocation.Necklace] = ReadWriteString("UseItems", "UseItems3", ""); // '项链';
            playObject.m_UseItemNames[ItemLocation.Helmet] = ReadWriteString("UseItems", "UseItems4", ""); // '头盔';
            playObject.m_UseItemNames[ItemLocation.ArmRingl] = ReadWriteString("UseItems", "UseItems5", ""); // '左手镯';
            playObject.m_UseItemNames[ItemLocation.ArmRingr] = ReadWriteString("UseItems", "UseItems6", ""); // '右手镯';
            playObject.m_UseItemNames[ItemLocation.Ringl] = ReadWriteString("UseItems", "UseItems7", ""); // '左戒指';
            playObject.m_UseItemNames[ItemLocation.Ringr] = ReadWriteString("UseItems", "UseItems8", ""); // '右戒指';
            for (byte i = ItemLocation.Dress; i <= ItemLocation.Charm; i++) {
                if (!string.IsNullOrEmpty(playObject.m_UseItemNames[i])) {
                    StdItem = M2Share.WorldEngine.GetStdItem(playObject.m_UseItemNames[i]);
                    if (StdItem != null) {
                        UserItem = new UserItem();
                        if (M2Share.WorldEngine.CopyToUserItemFromName(playObject.m_UseItemNames[i], ref UserItem)) {
                            if (M2Share.StdModeMap.Contains(StdItem.StdMode)) {
                                if (StdItem.Shape == 130 || StdItem.Shape == 131 || StdItem.Shape == 132) {
                                    //M2Share.WorldEngine.GetUnknowItemValue(UserItem);
                                }
                            }
                        }
                        playObject.UseItems[i] = UserItem;
                        UserItem = null;
                    }
                }
            }
        }

    }
}