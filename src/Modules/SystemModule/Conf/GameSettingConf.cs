using SystemModule.Common;

namespace SystemModule{
    public class GameSettingConf : ConfigFile
    {
        public GameSettingConf(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadConfig()
        {
            int nInteger = ReadWriteInteger("Config", "WhisperRecord", -1);  //游戏私聊
            if (nInteger <= -1)
            {
                WriteBool("Config", "WhisperRecord", ModuleShare.Config.ClientConf.boWhisperRecord);
            }

            nInteger = ReadWriteInteger("Config", "NoFog", -1);
            if (nInteger <= -1) WriteBool("Config", "NoFog", ModuleShare.Config.ClientConf.boNoFog);

            nInteger = ReadWriteInteger("Config", "StallSystem", -1);
            if (nInteger <= -1) WriteBool("Config", "StallSystem", ModuleShare.Config.ClientConf.boStallSystem);

            nInteger = ReadWriteInteger("Config", "SafeZoneStall", -1);
            if (nInteger <= -1) WriteBool("Config", "SafeZoneStall", Settings.boSafeZoneStall);

            nInteger = ReadWriteInteger("Config", "ShowHpBar", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowHpBar", ModuleShare.Config.ClientConf.boShowHpBar);

            nInteger = ReadWriteInteger("Config", "ShowHpNumber", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowHpNumber", ModuleShare.Config.ClientConf.boShowHpNumber);

            nInteger = ReadWriteInteger("Config", "NoStruck", -1);
            if (nInteger <= -1) WriteBool("Config", "NoStruck", ModuleShare.Config.ClientConf.boNoStruck);

            nInteger = ReadWriteInteger("Config", "FastMove", -1);
            if (nInteger <= -1) WriteBool("Config", "FastMove", ModuleShare.Config.ClientConf.boFastMove);

            nInteger = ReadWriteInteger("Config", "ShowFriend", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowFriend", ModuleShare.Config.ClientConf.boShowFriend);

            nInteger = ReadWriteInteger("Config", "ShowRelationship", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowRelationship", ModuleShare.Config.ClientConf.boShowRelationship);

            nInteger = ReadWriteInteger("Config", "ShowMail", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowMail", ModuleShare.Config.ClientConf.boShowMail);

            nInteger = ReadWriteInteger("Config", "ShowRecharging", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowRecharging", ModuleShare.Config.ClientConf.boShowRecharging);

            nInteger = ReadWriteInteger("Config", "ShowHelp", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowHelp", ModuleShare.Config.ClientConf.boShowHelp);

            nInteger = ReadWriteInteger("Config", "SecondCardSystem", -1);
            if (nInteger <= -1) WriteBool("Config", "SecondCardSystem", Settings.boSecondCardSystem);

            nInteger = ReadWriteInteger("Config", "ExpErienceLevel", -1);
            if (nInteger <= -1) WriteInteger("Config", "ExpErienceLevel", Settings.ExpErienceLevel);


            string sString = ReadWriteString("Config", "BadManHomeMap", "");
            if (string.IsNullOrEmpty(sString))
            {
                WriteString("Config", "BadManHomeMap", "3");
            }
            nInteger = ReadWriteInteger("Config", "BadManStartX", -1);
            if (nInteger <= -1) WriteInteger("Config", "BadManStartX", Settings.BADMANSTARTX);
            nInteger = ReadWriteInteger("Config", "BadManStartY", -1);
            if (nInteger <= -1) WriteInteger("Config", "BadManStartY", Settings.BADMANSTARTY);

            sString = ReadWriteString("Config", "RECHARGINGMAP", ""); //充值地图
            if (string.IsNullOrEmpty(sString))
            {
                WriteString("Config", "RECHARGINGMAP", "kaiqu");
            }

            ModuleShare.Config.ClientConf.boWhisperRecord = ReadWriteBool("Config", "WhisperRecord", ModuleShare.Config.ClientConf.boWhisperRecord);  //游戏私聊
            ModuleShare.Config.ClientConf.boNoFog = ReadWriteBool("Config", "NoFog", ModuleShare.Config.ClientConf.boNoFog);
            ModuleShare.Config.ClientConf.boStallSystem = ReadWriteBool("Config", "StallSystem", ModuleShare.Config.ClientConf.boStallSystem);
            Settings.boSafeZoneStall = ReadWriteBool("Config", "SafeZoneStall", Settings.boSafeZoneStall);
            ModuleShare.Config.ClientConf.boShowHpBar = ReadWriteBool("Config", "ShowHpBar", ModuleShare.Config.ClientConf.boShowHpBar);
            ModuleShare.Config.ClientConf.boShowHpNumber = ReadWriteBool("Config", "ShowHpNumber", ModuleShare.Config.ClientConf.boShowHpNumber);
            ModuleShare.Config.ClientConf.boNoStruck = ReadWriteBool("Config", "NoStruck", ModuleShare.Config.ClientConf.boNoStruck);
            ModuleShare.Config.ClientConf.boFastMove = ReadWriteBool("Config", "FastMove", ModuleShare.Config.ClientConf.boFastMove);

            ModuleShare.Config.ClientConf.boShowFriend = ReadWriteBool("Config", "ShowFriend", ModuleShare.Config.ClientConf.boShowFriend);
            ModuleShare.Config.ClientConf.boShowRelationship = ReadWriteBool("Config", "ShowRelationship", ModuleShare.Config.ClientConf.boShowRelationship);
            ModuleShare.Config.ClientConf.boShowMail = ReadWriteBool("Config", "ShowMail", ModuleShare.Config.ClientConf.boShowMail);
            ModuleShare.Config.ClientConf.boShowRecharging = ReadWriteBool("Config", "ShowRecharging", ModuleShare.Config.ClientConf.boShowRecharging);
            ModuleShare.Config.ClientConf.boShowHelp = ReadWriteBool("Config", "ShowHelp", ModuleShare.Config.ClientConf.boShowHelp);

            Settings.boSecondCardSystem = ReadWriteBool("Config", "SecondCardSystem", Settings.boSecondCardSystem);
            Settings.ExpErienceLevel = ReadWriteByte("Config", "ExpErienceLevel", Settings.ExpErienceLevel);

            Settings.BADMANHOMEMAP = ReadWriteString("Config", "BadManHomeMap", Settings.BADMANHOMEMAP);
            Settings.BADMANSTARTX = ReadWriteInt16("Config", "BadManStartX", Settings.BADMANSTARTX);
            Settings.BADMANSTARTY = ReadWriteInt16("Config", "BadManStartY", Settings.BADMANSTARTY);
            ModuleShare.Config.ClientConf.boGamepath = ReadWriteBool("Config", "Gamepath", ModuleShare.Config.ClientConf.boGamepath);
            Settings.RECHARGINGMAP = ReadWriteString("Config", "RECHARGINGMAP", Settings.RECHARGINGMAP); //充值
        }
    }
}