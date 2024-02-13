using OpenMir2.Common;

namespace SystemModule.Conf
{
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
                WriteBool("Config", "WhisperRecord", SystemShare.Config.ClientConf.boWhisperRecord);
            }

            nInteger = ReadWriteInteger("Config", "NoFog", -1);
            if (nInteger <= -1) WriteBool("Config", "NoFog", SystemShare.Config.ClientConf.boNoFog);

            nInteger = ReadWriteInteger("Config", "StallSystem", -1);
            if (nInteger <= -1) WriteBool("Config", "StallSystem", SystemShare.Config.ClientConf.boStallSystem);

            nInteger = ReadWriteInteger("Config", "SafeZoneStall", -1);
            if (nInteger <= -1) WriteBool("Config", "SafeZoneStall", MessageSettings.boSafeZoneStall);

            nInteger = ReadWriteInteger("Config", "ShowHpBar", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowHpBar", SystemShare.Config.ClientConf.boShowHpBar);

            nInteger = ReadWriteInteger("Config", "ShowHpNumber", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowHpNumber", SystemShare.Config.ClientConf.boShowHpNumber);

            nInteger = ReadWriteInteger("Config", "NoStruck", -1);
            if (nInteger <= -1) WriteBool("Config", "NoStruck", SystemShare.Config.ClientConf.boNoStruck);

            nInteger = ReadWriteInteger("Config", "FastMove", -1);
            if (nInteger <= -1) WriteBool("Config", "FastMove", SystemShare.Config.ClientConf.boFastMove);

            nInteger = ReadWriteInteger("Config", "ShowFriend", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowFriend", SystemShare.Config.ClientConf.boShowFriend);

            nInteger = ReadWriteInteger("Config", "ShowRelationship", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowRelationship", SystemShare.Config.ClientConf.boShowRelationship);

            nInteger = ReadWriteInteger("Config", "ShowMail", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowMail", SystemShare.Config.ClientConf.boShowMail);

            nInteger = ReadWriteInteger("Config", "ShowRecharging", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowRecharging", SystemShare.Config.ClientConf.boShowRecharging);

            nInteger = ReadWriteInteger("Config", "ShowHelp", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowHelp", SystemShare.Config.ClientConf.boShowHelp);

            nInteger = ReadWriteInteger("Config", "SecondCardSystem", -1);
            if (nInteger <= -1) WriteBool("Config", "SecondCardSystem", MessageSettings.boSecondCardSystem);

            nInteger = ReadWriteInteger("Config", "ExpErienceLevel", -1);
            if (nInteger <= -1) WriteInteger("Config", "ExpErienceLevel", MessageSettings.ExpErienceLevel);


            string sString = ReadWriteString("Config", "BadManHomeMap", "");
            if (string.IsNullOrEmpty(sString))
            {
                WriteString("Config", "BadManHomeMap", "3");
            }
            nInteger = ReadWriteInteger("Config", "BadManStartX", -1);
            if (nInteger <= -1) WriteInteger("Config", "BadManStartX", MessageSettings.BADMANSTARTX);
            nInteger = ReadWriteInteger("Config", "BadManStartY", -1);
            if (nInteger <= -1) WriteInteger("Config", "BadManStartY", MessageSettings.BADMANSTARTY);

            sString = ReadWriteString("Config", "RECHARGINGMAP", ""); //充值地图
            if (string.IsNullOrEmpty(sString))
            {
                WriteString("Config", "RECHARGINGMAP", "kaiqu");
            }

            SystemShare.Config.ClientConf.boWhisperRecord = ReadWriteBool("Config", "WhisperRecord", SystemShare.Config.ClientConf.boWhisperRecord);  //游戏私聊
            SystemShare.Config.ClientConf.boNoFog = ReadWriteBool("Config", "NoFog", SystemShare.Config.ClientConf.boNoFog);
            SystemShare.Config.ClientConf.boStallSystem = ReadWriteBool("Config", "StallSystem", SystemShare.Config.ClientConf.boStallSystem);
            MessageSettings.boSafeZoneStall = ReadWriteBool("Config", "SafeZoneStall", MessageSettings.boSafeZoneStall);
            SystemShare.Config.ClientConf.boShowHpBar = ReadWriteBool("Config", "ShowHpBar", SystemShare.Config.ClientConf.boShowHpBar);
            SystemShare.Config.ClientConf.boShowHpNumber = ReadWriteBool("Config", "ShowHpNumber", SystemShare.Config.ClientConf.boShowHpNumber);
            SystemShare.Config.ClientConf.boNoStruck = ReadWriteBool("Config", "NoStruck", SystemShare.Config.ClientConf.boNoStruck);
            SystemShare.Config.ClientConf.boFastMove = ReadWriteBool("Config", "FastMove", SystemShare.Config.ClientConf.boFastMove);

            SystemShare.Config.ClientConf.boShowFriend = ReadWriteBool("Config", "ShowFriend", SystemShare.Config.ClientConf.boShowFriend);
            SystemShare.Config.ClientConf.boShowRelationship = ReadWriteBool("Config", "ShowRelationship", SystemShare.Config.ClientConf.boShowRelationship);
            SystemShare.Config.ClientConf.boShowMail = ReadWriteBool("Config", "ShowMail", SystemShare.Config.ClientConf.boShowMail);
            SystemShare.Config.ClientConf.boShowRecharging = ReadWriteBool("Config", "ShowRecharging", SystemShare.Config.ClientConf.boShowRecharging);
            SystemShare.Config.ClientConf.boShowHelp = ReadWriteBool("Config", "ShowHelp", SystemShare.Config.ClientConf.boShowHelp);

            MessageSettings.boSecondCardSystem = ReadWriteBool("Config", "SecondCardSystem", MessageSettings.boSecondCardSystem);
            MessageSettings.ExpErienceLevel = ReadWriteByte("Config", "ExpErienceLevel", MessageSettings.ExpErienceLevel);

            MessageSettings.BADMANHOMEMAP = ReadWriteString("Config", "BadManHomeMap", MessageSettings.BADMANHOMEMAP);
            MessageSettings.BADMANSTARTX = ReadWriteInt16("Config", "BadManStartX", MessageSettings.BADMANSTARTX);
            MessageSettings.BADMANSTARTY = ReadWriteInt16("Config", "BadManStartY", MessageSettings.BADMANSTARTY);
            SystemShare.Config.ClientConf.boGamepath = ReadWriteBool("Config", "Gamepath", SystemShare.Config.ClientConf.boGamepath);
            MessageSettings.RECHARGINGMAP = ReadWriteString("Config", "RECHARGINGMAP", MessageSettings.RECHARGINGMAP); //充值
        }
    }
}