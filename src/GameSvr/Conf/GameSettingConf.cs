using SystemModule.Common;

namespace GameSvr.Conf {
    public class GameSettingConf : ConfigFile {
        public GameSettingConf(string fileName) : base(fileName) {
            Load();
        }

        public void LoadConfig() {
            int nInteger = ReadWriteInteger("Config", "WhisperRecord", -1);  //游戏私聊
            if (nInteger <= -1) {
                WriteBool("Config", "WhisperRecord", M2Share.Config.ClientConf.boWhisperRecord);
            }

            nInteger = ReadWriteInteger("Config", "NoFog", -1);
            if (nInteger <= -1) WriteBool("Config", "NoFog", M2Share.Config.ClientConf.boNoFog);

            nInteger = ReadWriteInteger("Config", "StallSystem", -1);
            if (nInteger <= -1) WriteBool("Config", "StallSystem", M2Share.Config.ClientConf.boStallSystem);

            nInteger = ReadWriteInteger("Config", "SafeZoneStall", -1);
            if (nInteger <= -1) WriteBool("Config", "SafeZoneStall", Settings.boSafeZoneStall);

            nInteger = ReadWriteInteger("Config", "ShowHpBar", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowHpBar", M2Share.Config.ClientConf.boShowHpBar);

            nInteger = ReadWriteInteger("Config", "ShowHpNumber", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowHpNumber", M2Share.Config.ClientConf.boShowHpNumber);

            nInteger = ReadWriteInteger("Config", "NoStruck", -1);
            if (nInteger <= -1) WriteBool("Config", "NoStruck", M2Share.Config.ClientConf.boNoStruck);

            nInteger = ReadWriteInteger("Config", "FastMove", -1);
            if (nInteger <= -1) WriteBool("Config", "FastMove", M2Share.Config.ClientConf.boFastMove);

            nInteger = ReadWriteInteger("Config", "ShowFriend", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowFriend", M2Share.Config.ClientConf.boShowFriend);

            nInteger = ReadWriteInteger("Config", "ShowRelationship", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowRelationship", M2Share.Config.ClientConf.boShowRelationship);

            nInteger = ReadWriteInteger("Config", "ShowMail", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowMail", M2Share.Config.ClientConf.boShowMail);

            nInteger = ReadWriteInteger("Config", "ShowRecharging", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowRecharging", M2Share.Config.ClientConf.boShowRecharging);

            nInteger = ReadWriteInteger("Config", "ShowHelp", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowHelp", M2Share.Config.ClientConf.boShowHelp);

            nInteger = ReadWriteInteger("Config", "SecondCardSystem", -1);
            if (nInteger <= -1) WriteBool("Config", "SecondCardSystem", Settings.boSecondCardSystem);

            nInteger = ReadWriteInteger("Config", "ExpErienceLevel", -1);
            if (nInteger <= -1) WriteInteger("Config", "ExpErienceLevel", Settings.ExpErienceLevel);


            string sString = ReadWriteString("Config", "BadManHomeMap", "");
            if (string.IsNullOrEmpty(sString)) {
                WriteString("Config", "BadManHomeMap", "3");
            }
            nInteger = ReadWriteInteger("Config", "BadManStartX", -1);
            if (nInteger <= -1) WriteInteger("Config", "BadManStartX", Settings.BADMANSTARTX);
            nInteger = ReadWriteInteger("Config", "BadManStartY", -1);
            if (nInteger <= -1) WriteInteger("Config", "BadManStartY", Settings.BADMANSTARTY);

            sString = ReadWriteString("Config", "RECHARGINGMAP", ""); //充值地图
            if (string.IsNullOrEmpty(sString)) {
                WriteString("Config", "RECHARGINGMAP", "kaiqu");
            }

            M2Share.Config.ClientConf.boWhisperRecord = ReadWriteBool("Config", "WhisperRecord", M2Share.Config.ClientConf.boWhisperRecord);  //游戏私聊
            M2Share.Config.ClientConf.boNoFog = ReadWriteBool("Config", "NoFog", M2Share.Config.ClientConf.boNoFog);
            M2Share.Config.ClientConf.boStallSystem = ReadWriteBool("Config", "StallSystem", M2Share.Config.ClientConf.boStallSystem);
            Settings.boSafeZoneStall = ReadWriteBool("Config", "SafeZoneStall", Settings.boSafeZoneStall);
            M2Share.Config.ClientConf.boShowHpBar = ReadWriteBool("Config", "ShowHpBar", M2Share.Config.ClientConf.boShowHpBar);
            M2Share.Config.ClientConf.boShowHpNumber = ReadWriteBool("Config", "ShowHpNumber", M2Share.Config.ClientConf.boShowHpNumber);
            M2Share.Config.ClientConf.boNoStruck = ReadWriteBool("Config", "NoStruck", M2Share.Config.ClientConf.boNoStruck);
            M2Share.Config.ClientConf.boFastMove = ReadWriteBool("Config", "FastMove", M2Share.Config.ClientConf.boFastMove);

            M2Share.Config.ClientConf.boShowFriend = ReadWriteBool("Config", "ShowFriend", M2Share.Config.ClientConf.boShowFriend);
            M2Share.Config.ClientConf.boShowRelationship = ReadWriteBool("Config", "ShowRelationship", M2Share.Config.ClientConf.boShowRelationship);
            M2Share.Config.ClientConf.boShowMail = ReadWriteBool("Config", "ShowMail", M2Share.Config.ClientConf.boShowMail);
            M2Share.Config.ClientConf.boShowRecharging = ReadWriteBool("Config", "ShowRecharging", M2Share.Config.ClientConf.boShowRecharging);
            M2Share.Config.ClientConf.boShowHelp = ReadWriteBool("Config", "ShowHelp", M2Share.Config.ClientConf.boShowHelp);

            Settings.boSecondCardSystem = ReadWriteBool("Config", "SecondCardSystem", Settings.boSecondCardSystem);
            Settings.ExpErienceLevel = (byte)ReadWriteInteger("Config", "ExpErienceLevel", Settings.ExpErienceLevel);

            Settings.BADMANHOMEMAP = ReadWriteString("Config", "BadManHomeMap", Settings.BADMANHOMEMAP);
            Settings.BADMANSTARTX = (short)ReadWriteInteger("Config", "BadManStartX", Settings.BADMANSTARTX);
            Settings.BADMANSTARTY = (short)ReadWriteInteger("Config", "BadManStartY", Settings.BADMANSTARTY);
            M2Share.Config.ClientConf.boGamepath = ReadWriteBool("Config", "Gamepath", M2Share.Config.ClientConf.boGamepath);
            Settings.RECHARGINGMAP = ReadWriteString("Config", "RECHARGINGMAP", Settings.RECHARGINGMAP); //充值
        }
    }
}