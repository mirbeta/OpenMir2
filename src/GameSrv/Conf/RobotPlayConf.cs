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

        public void LoadConfig(RobotPlayer botPlayer) {
            IList<string> tempList;
            UserItem userItem;
            StdItem stdItem;
            botPlayer.NoDropItem = ReadWriteBool("Info", "NoDropItem", true);// 是否掉包裹物品
            botPlayer.NoDropUseItem = ReadWriteBool("Info", "DropUseItem", true);// 是否掉装备
            botPlayer.MNDropUseItemRate = ReadWriteInteger("Info", "DropUseItemRate", 100);// 掉装备机率
            botPlayer.Job = (PlayJob)ReadWriteInteger("Info", "Job", 0);
            botPlayer.Gender = Enum.Parse<PlayGender>(ReadWriteString("Info", "Gender", "0"));
            botPlayer.Hair = (byte)ReadWriteInteger("Info", "Hair", 0);
            botPlayer.Abil.Level = (byte)ReadWriteInteger("Info", "Level", 1);
            botPlayer.Abil.MaxExp = BaseObject.GetLevelExp(botPlayer.Abil.Level);
            botPlayer.ProtectStatus = ReadWriteBool("Info", "ProtectStatus", false);// 是否守护模式
            var nAttatckMode = (byte)ReadWriteInteger("Info", "AttatckMode", 6);// 攻击模式
            if (nAttatckMode <= 6)
            {
                botPlayer.AttatckMode = (AttackMode)nAttatckMode;
            }
            var sLineText = ReadWriteString("Info", "UseSkill", "");
            if (!string.IsNullOrEmpty(sLineText)) {
                tempList = new List<string>();
                //HUtil32.ArrestStringEx(new char[] { '|', '\\', '/', ',' }, new object[] { }, sLineText, TempList);
                tempList = sLineText.Split(",").ToList();
                for (int i = 0; i < tempList.Count; i++)
                {
                    var sMagicName = tempList[i].Trim();
                    if (botPlayer.FindMagic(sMagicName) == null)
                    {
                        var magic = M2Share.WorldEngine.FindMagic(sMagicName);
                        if (magic != null) {
                            if (magic.Job == 99 || magic.Job == (byte)botPlayer.Job) {
                                var userMagic = new UserMagic();
                                userMagic.Magic = magic;
                                userMagic.MagIdx = magic.MagicId;
                                userMagic.Level = 3;
                                userMagic.Key = (char)0;
                                userMagic.TranPoint = magic.MaxTrain[3];
                                botPlayer.MagicList.Add(userMagic);
                            }
                        }
                    }
                }
            }
            sLineText = ReadWriteString("Info", "InitItems", "");
            if (!string.IsNullOrEmpty(sLineText)) {
                tempList = new List<string>();
                //ExtractStrings(new char[] { '|', '\\', '/', ',' }, new object[] { }, sLineText, TempList);
                tempList = sLineText.Split(",").ToList();
                for (int i = 0; i < tempList.Count; i++) {
                    var sItemName = tempList[i].Trim();
                    stdItem = M2Share.WorldEngine.GetStdItem(sItemName);
                    if (stdItem != null) {
                        userItem = new UserItem();
                        if (M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref userItem)) {
                            if (M2Share.StdModeMap.Contains(stdItem.StdMode)) {
                                if (stdItem.Shape == 130 || stdItem.Shape == 131 || stdItem.Shape == 132) {
                                    //M2Share.WorldEngine.GetUnknowItemValue(UserItem);
                                }
                            }
                            if (!botPlayer.AddItemToBag(userItem)) {
                                userItem = null;
                                break;
                            }
                            botPlayer.BagItemNames.Add(stdItem.Name);
                        }
                        else {
                            userItem = null;
                        }
                    }
                }
            }
            for (int i = 0; i < 9; i++)
            {
                var sSayMsg = ReadWriteString("MonSay", i.ToString(), "");
                if (!string.IsNullOrEmpty(sSayMsg)) {
                    botPlayer.MAiSayMsgList.Add(sSayMsg);
                }
                else {
                    break;
                }
            }
            botPlayer.UseItemNames[ItemLocation.Dress] = ReadWriteString("UseItems", "UseItems0", "布衣(男)"); // '衣服';
            botPlayer.UseItemNames[ItemLocation.Weapon] = ReadWriteString("UseItems", "UseItems1", "木剑"); // '武器';
            botPlayer.UseItemNames[ItemLocation.RighThand] = ReadWriteString("UseItems", "UseItems2", ""); // '照明物';
            botPlayer.UseItemNames[ItemLocation.Necklace] = ReadWriteString("UseItems", "UseItems3", ""); // '项链';
            botPlayer.UseItemNames[ItemLocation.Helmet] = ReadWriteString("UseItems", "UseItems4", ""); // '头盔';
            botPlayer.UseItemNames[ItemLocation.ArmRingl] = ReadWriteString("UseItems", "UseItems5", ""); // '左手镯';
            botPlayer.UseItemNames[ItemLocation.ArmRingr] = ReadWriteString("UseItems", "UseItems6", ""); // '右手镯';
            botPlayer.UseItemNames[ItemLocation.Ringl] = ReadWriteString("UseItems", "UseItems7", ""); // '左戒指';
            botPlayer.UseItemNames[ItemLocation.Ringr] = ReadWriteString("UseItems", "UseItems8", ""); // '右戒指';
            for (byte i = ItemLocation.Dress; i <= ItemLocation.Charm; i++) {
                if (!string.IsNullOrEmpty(botPlayer.UseItemNames[i])) {
                    stdItem = M2Share.WorldEngine.GetStdItem(botPlayer.UseItemNames[i]);
                    if (stdItem != null) {
                        userItem = new UserItem();
                        if (M2Share.WorldEngine.CopyToUserItemFromName(botPlayer.UseItemNames[i], ref userItem)) {
                            if (M2Share.StdModeMap.Contains(stdItem.StdMode)) {
                                if (stdItem.Shape == 130 || stdItem.Shape == 131 || stdItem.Shape == 132) {
                                    //M2Share.WorldEngine.GetUnknowItemValue(UserItem);
                                }
                            }
                        }
                        botPlayer.UseItems[i] = userItem;
                        userItem = null;
                    }
                }
            }
        }

    }
}