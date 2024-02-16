using OpenMir2.Common;
using OpenMir2.Data;
using OpenMir2.Enums;
using OpenMir2.Packets.ClientPackets;
using RobotSystem.Conf;
using SystemModule;

namespace RobotSystem.Services
{
    public partial class RobotPlayer
    {
        public void LoadConfig()
        {
            string sFileName = GetRandomConfigFileName(ChrName, 0);
            if (string.IsNullOrEmpty(sFileName) || !File.Exists(sFileName))
            {
                if (!string.IsNullOrEmpty(ConfigFileName) && File.Exists(ConfigFileName))
                {
                    sFileName = ConfigFileName;
                }
            }
            RobotPlayerConf conf = new RobotPlayerConf(sFileName);
            IList<string> tempList;
            UserItem userItem;
            StdItem stdItem;
            Resurrection = conf.ReadWriteBool("Info", "Resurrection", false);// 是否掉包裹物品
            NoDropItem = conf.ReadWriteBool("Info", "NoDropItem", true);// 是否掉包裹物品
            NoDropUseItem = conf.ReadWriteBool("Info", "DropUseItem", true);// 是否掉装备
            DropUseItemRate = conf.ReadWriteInteger("Info", "DropUseItemRate", 100);// 掉装备机率
            Job = (PlayerJob)conf.ReadWriteInteger("Info", "Job", 0);
            Gender = Enum.Parse<PlayerGender>(conf.ReadWriteString("Info", "Gender", "0"));
            Hair = (byte)conf.ReadWriteInteger("Info", "Hair", 0);
            Abil.Level = (byte)conf.ReadWriteInteger("Info", "Level", 1);
            Abil.MaxExp = GetLevelExp(Abil.Level);
            ProtectStatus = conf.ReadWriteBool("Info", "ProtectStatus", false);// 是否守护模式
            byte nAttatckMode = (byte)conf.ReadWriteInteger("Info", "AttatckMode", 6);// 攻击模式
            if (nAttatckMode <= 6)
            {
                AttatckMode = (AttackMode)nAttatckMode;
            }
            string sLineText = conf.ReadWriteString("Info", "UseSkill", "");
            if (!string.IsNullOrEmpty(sLineText))
            {
                tempList = new List<string>();
                //HUtil32.ArrestStringEx(new char[] { '|', '\\', '/', ',' }, new object[] { }, sLineText, TempList);
                tempList = sLineText.Split(",").ToList();
                for (int i = 0; i < tempList.Count; i++)
                {
                    string sMagicName = tempList[i].Trim();
                    if (FindMagic(sMagicName) == null)
                    {
                        MagicInfo magic = SystemShare.WorldEngine.FindMagic(sMagicName);
                        if (magic != null)
                        {
                            if (magic.Job == 99 || magic.Job == (byte)Job)
                            {
                                UserMagic userMagic = new UserMagic();
                                userMagic.Magic = magic;
                                userMagic.MagIdx = magic.MagicId;
                                userMagic.Level = 3;
                                userMagic.Key = (char)0;
                                userMagic.TranPoint = magic.MaxTrain[3];
                                MagicList.Add(userMagic);
                            }
                        }
                    }
                }
            }
            sLineText = conf.ReadWriteString("Info", "InitItems", "");
            if (!string.IsNullOrEmpty(sLineText))
            {
                tempList = new List<string>();
                //ExtractStrings(new char[] { '|', '\\', '/', ',' }, new object[] { }, sLineText, TempList);
                tempList = sLineText.Split(",").ToList();
                for (int i = 0; i < tempList.Count; i++)
                {
                    string sItemName = tempList[i].Trim();
                    stdItem = SystemShare.EquipmentSystem.GetStdItem(sItemName);
                    if (stdItem != null)
                    {
                        userItem = new UserItem();
                        if (SystemShare.EquipmentSystem.CopyToUserItemFromName(sItemName, ref userItem))
                        {
                            if (SystemShare.StdModeMap.Contains(stdItem.StdMode))
                            {
                                if (stdItem.Shape == 130 || stdItem.Shape == 131 || stdItem.Shape == 132)
                                {
                                    //SystemShare.WorldEngine.GetUnknowItemValue(UserItem);
                                }
                            }
                            if (!AddItemToBag(userItem))
                            {
                                userItem = null;
                                break;
                            }
                            BagItemNames.Add(stdItem.Name);
                        }
                        else
                        {
                            userItem = null;
                        }
                    }
                }
            }
            for (int i = 0; i < 9; i++)
            {
                string sSayMsg = conf.ReadWriteString("MonSay", i.ToString(), "");
                if (!string.IsNullOrEmpty(sSayMsg))
                {
                    AiSayMsgList.Add(sSayMsg);
                }
                else
                {
                    break;
                }
            }
            UseItemNames[ItemLocation.Dress] = conf.ReadWriteString("UseItems", "UseItems0", "布衣(男)"); // '衣服';
            UseItemNames[ItemLocation.Weapon] = conf.ReadWriteString("UseItems", "UseItems1", "木剑"); // '武器';
            UseItemNames[ItemLocation.RighThand] = conf.ReadWriteString("UseItems", "UseItems2", ""); // '照明物';
            UseItemNames[ItemLocation.Necklace] = conf.ReadWriteString("UseItems", "UseItems3", ""); // '项链';
            UseItemNames[ItemLocation.Helmet] = conf.ReadWriteString("UseItems", "UseItems4", ""); // '头盔';
            UseItemNames[ItemLocation.ArmRingl] = conf.ReadWriteString("UseItems", "UseItems5", ""); // '左手镯';
            UseItemNames[ItemLocation.ArmRingr] = conf.ReadWriteString("UseItems", "UseItems6", ""); // '右手镯';
            UseItemNames[ItemLocation.Ringl] = conf.ReadWriteString("UseItems", "UseItems7", ""); // '左戒指';
            UseItemNames[ItemLocation.Ringr] = conf.ReadWriteString("UseItems", "UseItems8", ""); // '右戒指';
            for (byte i = ItemLocation.Dress; i <= ItemLocation.Charm; i++)
            {
                if (!string.IsNullOrEmpty(UseItemNames[i]))
                {
                    stdItem = SystemShare.EquipmentSystem.GetStdItem(UseItemNames[i]);
                    if (stdItem != null)
                    {
                        userItem = new UserItem();
                        if (SystemShare.EquipmentSystem.CopyToUserItemFromName(UseItemNames[i], ref userItem))
                        {
                            if (SystemShare.StdModeMap.Contains(stdItem.StdMode))
                            {
                                if (stdItem.Shape == 130 || stdItem.Shape == 131 || stdItem.Shape == 132)
                                {
                                    //SystemShare.WorldEngine.GetUnknowItemValue(UserItem);
                                }
                            }
                        }
                        UseItems[i] = userItem;
                        userItem = null;
                    }
                }
            }
            conf = null;
        }

        /// <summary>
        /// 取随机配置
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="nType"></param>
        /// <returns></returns>
        private string GetRandomConfigFileName(string sName, byte nType)
        {
            int nIndex;
            string str;
            StringList loadList;
            if (!Directory.Exists(FilePath + "RobotIni"))
            {
                Directory.CreateDirectory(FilePath + "RobotIni");
            }
            string sFileName = Path.Combine(FilePath, "RobotIni", sName + ".txt");
            if (File.Exists(sFileName))
            {
                return sFileName;
            }
            string result = Path.Combine(FilePath, "RobotIni", "默认.txt");
            switch (nType)
            {
                case 0:
                    if (!string.IsNullOrEmpty(ConfigListFileName) && File.Exists(ConfigListFileName))
                    {
                        loadList = new StringList();
                        loadList.LoadFromFile(ConfigListFileName);
                        nIndex = SystemShare.RandomNumber.Random(loadList.Count);
                        if (nIndex >= 0 && nIndex < loadList.Count)
                        {
                            str = loadList[nIndex];
                            if (!string.IsNullOrEmpty(str))
                            {
                                if (str[1] == '\\')
                                {
                                    str = str.AsSpan()[1..].ToString();
                                }
                                if (str[2] == '\\')
                                {
                                    str = str.AsSpan()[2..].ToString();
                                }
                                if (str[3] == '\\')
                                {
                                    str = str.AsSpan()[3..].ToString();
                                }
                            }
                            result = FilePath + str;
                        }
                    }
                    break;
                case 1:
                    if (!string.IsNullOrEmpty(HeroConfigListFileName) && File.Exists(HeroConfigListFileName))
                    {
                        loadList = new StringList();
                        loadList.LoadFromFile(HeroConfigListFileName);
                        nIndex = SystemShare.RandomNumber.Random(loadList.Count);
                        if (nIndex >= 0 && nIndex < loadList.Count)
                        {
                            str = loadList[nIndex];
                            if (!string.IsNullOrEmpty(str))
                            {
                                if (str[1] == '\\')
                                {
                                    str = str[1..];
                                }
                                if (str[2] == '\\')
                                {
                                    str = str[2..];
                                }
                                if (str[3] == '\\')
                                {
                                    str = str[3..];
                                }
                            }
                            result = FilePath + str;
                        }
                    }
                    break;
            }
            return result;
        }
    }
}