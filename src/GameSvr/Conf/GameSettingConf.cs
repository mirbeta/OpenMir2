using SystemModule.Common;

namespace GameSvr.Conf
{
    public class GameSettingConf : IniFile
    {
        public GameSettingConf(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadConfig()
        {
            var nInteger = ReadInteger("Config", "WhisperRecord", -1);  //游戏私聊
            if (nInteger <= -1)
            {
                WriteBool("Config", "WhisperRecord", M2Share.Config.ClientConf.boWhisperRecord);
            }

            nInteger = ReadInteger("Config", "NoFog", -1);
            if (nInteger <= -1) WriteBool("Config", "NoFog", M2Share.Config.ClientConf.boNoFog);

            nInteger = ReadInteger("Config", "StallSystem", -1);
            if (nInteger <= -1) WriteBool("Config", "StallSystem", M2Share.Config.ClientConf.boStallSystem);

            nInteger = ReadInteger("Config", "SafeZoneStall", -1);
            if (nInteger <= -1) WriteBool("Config", "SafeZoneStall", M2Share.boSafeZoneStall);

            nInteger = ReadInteger("Config", "ShowHpBar", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowHpBar", M2Share.Config.ClientConf.boShowHpBar);

            nInteger = ReadInteger("Config", "ShowHpNumber", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowHpNumber", M2Share.Config.ClientConf.boShowHpNumber);

            nInteger = ReadInteger("Config", "NoStruck", -1);
            if (nInteger <= -1) WriteBool("Config", "NoStruck", M2Share.Config.ClientConf.boNoStruck);

            nInteger = ReadInteger("Config", "FastMove", -1);
            if (nInteger <= -1) WriteBool("Config", "FastMove", M2Share.Config.ClientConf.boFastMove);

            nInteger = ReadInteger("Config", "ShowFriend", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowFriend", M2Share.Config.ClientConf.boShowFriend);

            nInteger = ReadInteger("Config", "ShowRelationship", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowRelationship", M2Share.Config.ClientConf.boShowRelationship);

            nInteger = ReadInteger("Config", "ShowMail", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowMail", M2Share.Config.ClientConf.boShowMail);

            nInteger = ReadInteger("Config", "ShowRecharging", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowRecharging", M2Share.Config.ClientConf.boShowRecharging);

            nInteger = ReadInteger("Config", "ShowHelp", -1);
            if (nInteger <= -1) WriteBool("Config", "ShowHelp", M2Share.Config.ClientConf.boShowHelp);

            nInteger = ReadInteger("Config", "SecondCardSystem", -1);
            if (nInteger <= -1) WriteBool("Config", "SecondCardSystem", M2Share.boSecondCardSystem);

            nInteger = ReadInteger("Config", "ExpErienceLevel", -1);
            if (nInteger <= -1) WriteInteger("Config", "ExpErienceLevel", M2Share.g_nExpErienceLevel);


            var sString = ReadString("Config", "BadManHomeMap", "");
            if (string.IsNullOrEmpty(sString))
            {
                WriteString("Config", "BadManHomeMap", "3");
            }
            nInteger = ReadInteger("Config", "BadManStartX", -1);
            if (nInteger <= -1) WriteInteger("Config", "BadManStartX", M2Share.BADMANSTARTX);
            nInteger = ReadInteger("Config", "BadManStartY", -1);
            if (nInteger <= -1) WriteInteger("Config", "BadManStartY", M2Share.BADMANSTARTY);

            sString = ReadString("Config", "RECHARGINGMAP", ""); //充值地图
            if (string.IsNullOrEmpty(sString))
            {
                WriteString("Config", "RECHARGINGMAP", "kaiqu");
            }

            M2Share.Config.ClientConf.boWhisperRecord = ReadBool("Config", "WhisperRecord", M2Share.Config.ClientConf.boWhisperRecord);  //游戏私聊
            M2Share.Config.ClientConf.boNoFog = ReadBool("Config", "NoFog", M2Share.Config.ClientConf.boNoFog);
            M2Share.Config.ClientConf.boStallSystem = ReadBool("Config", "StallSystem", M2Share.Config.ClientConf.boStallSystem);
            M2Share.boSafeZoneStall = ReadBool("Config", "SafeZoneStall", M2Share.boSafeZoneStall);
            M2Share.Config.ClientConf.boShowHpBar = ReadBool("Config", "ShowHpBar", M2Share.Config.ClientConf.boShowHpBar);
            M2Share.Config.ClientConf.boShowHpNumber = ReadBool("Config", "ShowHpNumber", M2Share.Config.ClientConf.boShowHpNumber);
            M2Share.Config.ClientConf.boNoStruck = ReadBool("Config", "NoStruck", M2Share.Config.ClientConf.boNoStruck);
            M2Share.Config.ClientConf.boFastMove = ReadBool("Config", "FastMove", M2Share.Config.ClientConf.boFastMove);

            M2Share.Config.ClientConf.boShowFriend = ReadBool("Config", "ShowFriend", M2Share.Config.ClientConf.boShowFriend);
            M2Share.Config.ClientConf.boShowRelationship = ReadBool("Config", "ShowRelationship", M2Share.Config.ClientConf.boShowRelationship);
            M2Share.Config.ClientConf.boShowMail = ReadBool("Config", "ShowMail", M2Share.Config.ClientConf.boShowMail);
            M2Share.Config.ClientConf.boShowRecharging = ReadBool("Config", "ShowRecharging", M2Share.Config.ClientConf.boShowRecharging);
            M2Share.Config.ClientConf.boShowHelp = ReadBool("Config", "ShowHelp", M2Share.Config.ClientConf.boShowHelp);

            M2Share.boSecondCardSystem = ReadBool("Config", "SecondCardSystem", M2Share.boSecondCardSystem);
            M2Share.g_nExpErienceLevel = (byte)ReadInteger("Config", "ExpErienceLevel", M2Share.g_nExpErienceLevel);

            M2Share.BADMANHOMEMAP = ReadString("Config", "BadManHomeMap", M2Share.BADMANHOMEMAP);
            M2Share.BADMANSTARTX = (short)ReadInteger("Config", "BadManStartX", M2Share.BADMANSTARTX);
            M2Share.BADMANSTARTY = (short)ReadInteger("Config", "BadManStartY", M2Share.BADMANSTARTY);
            M2Share.Config.ClientConf.boGamepath = ReadBool("Config", "Gamepath", M2Share.Config.ClientConf.boGamepath);
            M2Share.RECHARGINGMAP = ReadString("Config", "RECHARGINGMAP", M2Share.RECHARGINGMAP); //充值
        }
    }
}