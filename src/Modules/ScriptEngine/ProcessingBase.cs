using M2Server;
using M2Server.Items;
using M2Server.Npc;
using M2Server.Player;
using ScriptEngine.Consts;
using ScriptEngine.Processings;
using System.Text;
using SystemModule;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Enums;

namespace ScriptEngine
{
    public class ProcessingBase
    {
        internal bool GetMovDataHumanInfoValue(NormNpc normNpc, PlayObject playObject, string sVariable, ref string sValue, ref int nValue, ref int nDataType)
        {
            string s10 = string.Empty;
            string sVarValue2 = string.Empty;
            DynamicVar DynamicVar;
            sValue = "";
            nValue = -1;
            nDataType = -1;
            bool result = false;
            if (string.IsNullOrEmpty(sVariable))
            {
                return false;
            }
            string sMsg = sVariable;
            HUtil32.ArrestStringEx(sMsg, "<", ">", ref s10);
            if (string.IsNullOrEmpty(s10))
            {
                return false;
            }
            sVariable = s10;
            //全局信息
            switch (sVariable)
            {
                case "$SERVERNAME":
                    sValue = M2Share.Config.ServerName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$SERVERIP":
                    sValue = M2Share.Config.ServerIPaddr;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$WEBSITE":
                    sValue = M2Share.Config.sWebSite;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BBSSITE":
                    sValue = M2Share.Config.sBbsSite;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$CLIENTDOWNLOAD":
                    sValue = M2Share.Config.sClientDownload;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$QQ":
                    sValue = M2Share.Config.sQQ;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$PHONE":
                    sValue = M2Share.Config.sPhone;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT0":
                    sValue = M2Share.Config.sBankAccount0;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT1":
                    sValue = M2Share.Config.sBankAccount1;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT2":
                    sValue = M2Share.Config.sBankAccount2;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT3":
                    sValue = M2Share.Config.sBankAccount3;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT4":
                    sValue = M2Share.Config.sBankAccount4;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT5":
                    sValue = M2Share.Config.sBankAccount5;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT6":
                    sValue = M2Share.Config.sBankAccount6;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT7":
                    sValue = M2Share.Config.sBankAccount7;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT8":
                    sValue = M2Share.Config.sBankAccount8;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BANKACCOUNT9":
                    sValue = M2Share.Config.sBankAccount9;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$GAMEGOLDNAME":
                    sValue = M2Share.Config.GameGoldName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$GAMEPOINTNAME":
                    sValue = M2Share.Config.GamePointName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$USERCOUNT":
                    sValue = M2Share.WorldEngine.PlayObjectCount.ToString();
                    nDataType = 0;
                    result = true;
                    return result;
                case "$MACRUNTIME":
                    sValue = (HUtil32.GetTickCount() / 86400000).ToString();// (24 * 60 * 60 * 1000)
                    nDataType = 0;
                    result = true;
                    return result;
                case "$SERVERRUNTIME":
                    sValue = DateTimeOffset.FromUnixTimeMilliseconds(M2Share.StartTime).ToString("YYYY-MM-DD HH:mm:ss");
                    nDataType = 0;
                    result = true;
                    return result;
                case "$DATETIME":
                    sValue = DateTime.Now.ToString("YYYY-MM-DD HH:mm:ss");
                    nDataType = 0;
                    result = true;
                    return result;
                case "$DATE":
                    sValue = DateTime.Now.ToString("yyyy-MM-dd");
                    nDataType = 0;
                    result = true;
                    return result;
                case "$CASTLEWARDATE":// 申请攻城的日期
                    {
                        if (normNpc.Castle == null)
                        {
                            normNpc.Castle = M2Share.CastleMgr.GetCastle(0);
                        }
                        if (normNpc.Castle != null)
                        {
                            if (!normNpc.Castle.UnderWar)
                            {
                                sValue = normNpc.Castle.GetWarDate();
                                if (!string.IsNullOrEmpty(sValue))
                                {
                                    sMsg = ReplaceVariableText(sMsg, "<$CASTLEWARDATE>", sValue);
                                }
                            }
                        }
                        else
                        {
                            sValue = "????";
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
                case "$USERNAME":// 个人信息
                    sValue = playObject.ChrName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$KILLER":// 杀人者变量 
                    {
                        if (playObject.Death && (playObject.LastHiter != null))
                        {
                            if (playObject.LastHiter.Race == ActorRace.Play)
                            {
                                sValue = playObject.LastHiter.ChrName;
                            }
                        }
                        else
                        {
                            sValue = "未知";
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
                case "$MONKILLER":// 杀人的怪物变量 
                    {
                        if (playObject.Death && (playObject.LastHiter != null))
                        {
                            if (playObject.LastHiter.Race != ActorRace.Play)
                            {
                                sValue = playObject.LastHiter.ChrName;
                            }
                        }
                        else
                        {
                            sValue = "未知";
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
                case "$USERALLNAME":// 全名 
                    sValue = playObject.GetShowName();
                    nDataType = 0;
                    result = true;
                    return result;
                case "$SFNAME":// 师傅名 
                    sValue = playObject.MasterName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$STATSERVERTIME":// 显示M2启动时间
                    DateTimeOffset.FromUnixTimeMilliseconds(M2Share.StartTime).ToString("YYYY-MM-DD HH:mm:ss");
                    nDataType = 0;
                    result = true;
                    return result;
                case "$RUNDATETIME":// 开区间隔时间 显示为XX小时。
                    var ts = DateTimeOffset.Now - DateTimeOffset.FromUnixTimeMilliseconds(M2Share.StartTime);
                    sValue = $"服务器运行:[{ts.Days}天{ts.Hours}小时{ts.Minutes}分{ts.Seconds}秒]";
                    nDataType = 0;
                    result = true;
                    return result;
                case "$RANDOMNO":// 随机值变量
                    nValue = M2Share.RandomNumber.Random(int.MaxValue);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$USERID":// 登录账号
                    sValue = playObject.UserAccount;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$IPADDR":// 登录IP
                    sValue = playObject.LoginIpAddr;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$X": // 人物X坐标
                    nValue = playObject.CurrX;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$Y": // 人物Y坐标
                    nValue = playObject.CurrY;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAP":
                    sValue = playObject.Envir.MapName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$GUILDNAME":
                    {
                        if (playObject.MyGuild != null)
                        {
                            sValue = playObject.MyGuild.GuildName;
                        }
                        else
                        {
                            sValue = "无";
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
                case "$RANKNAME":
                    sValue = playObject.GuildRankName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$RELEVEL":
                    nValue = playObject.ReLevel;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$LEVEL":
                    nValue = playObject.Abil.Level;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$HP":
                    nValue = playObject.WAbil.HP;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXHP":
                    nValue = playObject.WAbil.MaxHP;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MP":
                    nValue = playObject.WAbil.MP;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXMP":
                    nValue = playObject.WAbil.MaxMP;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$AC":
                    nValue = HUtil32.LoWord(playObject.WAbil.AC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXAC":
                    nValue = HUtil32.HiWord(playObject.WAbil.AC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAC":
                    nValue = HUtil32.LoWord(playObject.WAbil.MAC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXMAC":
                    nValue = HUtil32.HiWord(playObject.WAbil.MAC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$DC":
                    nValue = HUtil32.LoWord(playObject.WAbil.DC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXDC":
                    nValue = HUtil32.HiWord(playObject.WAbil.DC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MC":
                    nValue = HUtil32.LoWord(playObject.WAbil.MC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXMC":
                    nValue = HUtil32.HiWord(playObject.WAbil.MC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$SC":
                    nValue = HUtil32.LoWord(playObject.WAbil.SC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXSC":
                    nValue = HUtil32.HiWord(playObject.WAbil.SC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$EXP":
                    nValue = playObject.Abil.Exp;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXEXP":
                    nValue = playObject.Abil.MaxExp;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$PKPOINT":
                    nValue = playObject.PkPoint;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$CREDITPOINT":
                    nValue = playObject.CreditPoint;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$HW":
                    nValue = playObject.WAbil.HandWeight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXHW":
                    nValue = playObject.WAbil.MaxHandWeight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$BW":
                    nValue = playObject.WAbil.Weight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXBW":
                    nValue = playObject.WAbil.MaxWeight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$WW":
                    nValue = playObject.WAbil.WearWeight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXWW":
                    nValue = playObject.WAbil.MaxWearWeight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$GOLDCOUNT":
                    nValue = playObject.Gold;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$GOLDCOUNTX":
                    nValue = playObject.GoldMax;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$GAMEGOLD":
                    nValue = playObject.GameGold;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$GAMEPOINT":
                    nValue = playObject.GamePoint;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$HUNGER":
                    nValue = playObject.GetMyStatus();
                    nDataType = 1;
                    result = true;
                    return result;
                case "$LOGINTIME":
                    sValue = DateTimeOffset.FromUnixTimeMilliseconds(playObject.LogonTime).ToString("yyyy-MM-dd HH:mm:ss");
                    nDataType = 0;
                    result = true;
                    return result;
                case "$LOGINLONG":
                    nValue = (HUtil32.GetTickCount() - playObject.LogonTick) / 60000;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$DRESS":
                    sValue = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.Dress].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$WEAPON":
                    sValue = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.Weapon].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$RIGHTHAND":
                    sValue = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.RighThand].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$HELMET":
                    sValue = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.Helmet].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$NECKLACE":
                    sValue = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.Necklace].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$RING_R":
                    sValue = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.Ringr].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$RING_L":
                    sValue = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.Ringl].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$ARMRING_R":
                    sValue = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.ArmRingr].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$ARMRING_L":
                    sValue = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.ArmRingl].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BUJUK":
                    sValue = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.Bujuk].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BELT":
                    sValue = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.Belt].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BOOTS":
                    sValue = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.Boots].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$CHARM":
                    sValue = ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.Charm].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$IPLOCAL":
                    sValue = playObject.LoginIpLocal;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$GUILDBUILDPOINT":
                    {
                        if (playObject.MyGuild != null)
                        {
                            //nValue = playObject.MyGuild.nBuildPoint;
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
                case "$GUILDAURAEPOINT":
                    {
                        if (playObject.MyGuild != null)
                        {
                            nValue = playObject.MyGuild.Aurae;
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
                case "$GUILDSTABILITYPOINT":
                    {
                        if (playObject.MyGuild != null)
                        {
                            nValue = playObject.MyGuild.Stability;
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
                case "$GUILDFLOURISHPOINT":
                    {
                        if (playObject.MyGuild != null)
                        {
                            nValue = playObject.MyGuild.Flourishing;
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
            }
            if (HUtil32.CompareLStr(sVariable, "$GLOBAL", 6))//  全局变量
            {
                HUtil32.ArrestStringEx(sVariable, "(", ")", ref sVarValue2);
                if (M2Share.DynamicVarList.TryGetValue(sVarValue2, out DynamicVar))
                {
                    switch (DynamicVar.VarType)
                    {
                        case VarType.Integer:
                            nValue = DynamicVar.nInternet;
                            nDataType = 1;
                            result = true;
                            return result;
                        case VarType.String:
                            sValue = DynamicVar.sString;
                            nDataType = 0;
                            result = true;
                            return result;
                    }
                }
            }
            if (HUtil32.CompareLStr(sVariable, "$HUMAN", 6))//  人物变量
            {
                HUtil32.ArrestStringEx(sVariable, "(", ")", ref sVarValue2);
                if (playObject.DynamicVarMap.TryGetValue(sVarValue2, out DynamicVar))
                {
                    switch (DynamicVar.VarType)
                    {
                        case VarType.Integer:
                            nValue = DynamicVar.nInternet;
                            nDataType = 1;
                            result = true;
                            return result;
                        case VarType.String:
                            sValue = DynamicVar.sString;
                            nDataType = 0;
                            result = true;
                            return result;
                    }
                }
            }
            if (HUtil32.CompareLStr(sVariable, "$ACCOUNT", 8)) //  人物变量
            {
                HUtil32.ArrestStringEx(sVariable, "(", ")", ref sVarValue2);
                if (playObject.DynamicVarMap.TryGetValue(sVarValue2, out DynamicVar))
                {
                    switch (DynamicVar.VarType)
                    {
                        case VarType.Integer:
                            nValue = DynamicVar.nInternet;
                            nDataType = 1;
                            result = true;
                            return result;
                        case VarType.String:
                            sValue = DynamicVar.sString;
                            nDataType = 0;
                            result = true;
                            return result;
                    }
                }
            }
            return result;
        }

        internal static bool SetMovDataValNameValue(PlayObject playObject, string sVarName, string sValue, int nValue, int nDataType)
        {
            bool result = false;
            int n100 = M2Share.GetValNameNo(sVarName);
            if (n100 >= 0)
            {
                switch (nDataType)
                {
                    case 1:
                        if (HUtil32.RangeInDefined(n100, 0, 99))
                        {
                            playObject.MNVal[n100] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 100, 199))
                        {
                            M2Share.Config.GlobalVal[n100 - 100] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 200, 299))
                        {
                            playObject.MDyVal[n100 - 200] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 300, 399))
                        {
                            playObject.MNMval[n100 - 300] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 400, 499))
                        {
                            M2Share.Config.GlobaDyMval[n100 - 400] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 500, 599))
                        {
                            playObject.MNInteger[n100 - 500] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 600, 699))
                        {
                            playObject.MSString[n100 - 600] = nValue.ToString();
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 700, 799))
                        {
                            M2Share.Config.GlobalAVal[n100 - 700] = nValue.ToString();
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 800, 1199)) // G变量
                        {
                            M2Share.Config.GlobalVal[n100 - 700] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 1200, 1599)) // A变量
                        {
                            M2Share.Config.GlobalAVal[n100 - 1100] = nValue.ToString();
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                    case 0:
                        if (HUtil32.RangeInDefined(n100, 0, 99))
                        {
                            playObject.MNVal[n100] = HUtil32.StrToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 100, 199))
                        {
                            M2Share.Config.GlobalVal[n100 - 100] = HUtil32.StrToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 200, 299))
                        {
                            playObject.MDyVal[n100 - 200] = HUtil32.StrToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 300, 399))
                        {
                            playObject.MNMval[n100 - 300] = HUtil32.StrToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 400, 499))
                        {
                            M2Share.Config.GlobaDyMval[n100 - 400] = HUtil32.StrToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 500, 599))
                        {
                            playObject.MNInteger[n100 - 500] = HUtil32.StrToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 600, 699))
                        {
                            playObject.MSString[n100 - 600] = sValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 700, 799))
                        {
                            M2Share.Config.GlobalAVal[n100 - 700] = sValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 800, 1199)) // G变量
                        {
                            M2Share.Config.GlobalVal[n100 - 700] = HUtil32.StrToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 1200, 1599)) // A变量
                        {
                            M2Share.Config.GlobalAVal[n100 - 1100] = sValue;
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                    case 3:
                        if (HUtil32.RangeInDefined(n100, 0, 99))
                        {
                            playObject.MNVal[n100] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 100, 199))
                        {
                            M2Share.Config.GlobalVal[n100 - 100] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 200, 299))
                        {
                            playObject.MDyVal[n100 - 200] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 300, 399))
                        {
                            playObject.MNMval[n100 - 300] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 400, 499))
                        {
                            M2Share.Config.GlobaDyMval[n100 - 400] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 500, 599))
                        {
                            playObject.MNInteger[n100 - 500] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 600, 699))
                        {
                            playObject.MSString[n100 - 600] = sValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 700, 799))
                        {
                            M2Share.Config.GlobalAVal[n100 - 700] = sValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 800, 1199)) // G变量
                        {
                            M2Share.Config.GlobalVal[n100 - 700] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 1200, 1599)) // A变量
                        {
                            M2Share.Config.GlobalAVal[n100 - 1100] = sValue;
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        internal static bool GetMovDataValNameValue(PlayObject playObject, string sVarName, ref string sValue, ref int nValue, ref int nDataType)
        {
            bool result = false;
            nValue = -1;
            sValue = "";
            nDataType = -1;
            int n100 = M2Share.GetValNameNo(sVarName);
            if (n100 >= 0)
            {
                if (HUtil32.RangeInDefined(n100, 0, 99))
                {
                    nValue = playObject.MNVal[n100];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 100, 199))
                {
                    nValue = M2Share.Config.GlobalVal[n100 - 100];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 200, 299))
                {
                    nValue = playObject.MDyVal[n100 - 200];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 300, 399))
                {
                    nValue = playObject.MNMval[n100 - 300];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 400, 499))
                {
                    nValue = M2Share.Config.GlobaDyMval[n100 - 400];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 500, 599))
                {
                    nValue = playObject.MNInteger[n100 - 500];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 600, 699))
                {
                    sValue = playObject.MSString[n100 - 600];
                    nDataType = 0;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 700, 799))
                {
                    sValue = M2Share.Config.GlobalAVal[n100 - 700];
                    nDataType = 0;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 800, 1199))//G变量
                {
                    nValue = M2Share.Config.GlobalVal[n100 - 700];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 1200, 1599))//A变量
                {
                    sValue = M2Share.Config.GlobalAVal[n100 - 1100];
                    nDataType = 0;
                    result = true;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        internal static bool GetMovDataDynamicVarValue(PlayObject playObject, string sVarType, string sVarName, ref string sValue, ref int nValue, ref int nDataType)
        {
            string sName = string.Empty;
            sValue = "";
            nValue = -1;
            nDataType = -1;
            Dictionary<string, DynamicVar> DynamicVarList = GetDynamicVarMap(playObject, sVarType, ref sName);
            if (DynamicVarList == null)
            {
                return false;
            }
            if (DynamicVarList.TryGetValue(sVarName, out DynamicVar DynamicVar))
            {
                switch (DynamicVar.VarType)
                {
                    case VarType.Integer:
                        nValue = DynamicVar.nInternet;
                        nDataType = 1;
                        break;
                    case VarType.String:
                        sValue = DynamicVar.sString;
                        nDataType = 0;
                        break;
                }
                return true;
            }
            return false;
        }

        internal static bool SetMovDataDynamicVarValue(PlayObject playObject, string sVarType, string sVarName, string sValue, int nValue, int nDataType)
        {
            string sName = string.Empty;
            bool boVarFound = false;
            Dictionary<string, DynamicVar> DynamicVarList = GetDynamicVarMap(playObject, sVarType, ref sName);
            if (DynamicVarList == null)
            {
                return false;
            }
            if (DynamicVarList.TryGetValue(sVarName, out DynamicVar DynamicVar))
            {
                if (nDataType == 1 && DynamicVar.VarType == VarType.Integer)
                {
                    DynamicVar.nInternet = nValue;
                    boVarFound = true;
                }
                else if (DynamicVar.VarType == VarType.String)
                {
                    DynamicVar.sString = sValue;
                    boVarFound = true;
                }
            }
            return boVarFound;
        }

        internal static int GetMovDataType(QuestActionInfo questActionInfo)
        {
            string sParam1 = string.Empty;
            string sParam2 = string.Empty;
            string sParam3 = string.Empty;
            int result = -1;
            if (HUtil32.CompareLStr(questActionInfo.sParam1, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(questActionInfo.sParam1, "(", ")", ref sParam1);
            }
            else
            {
                sParam1 = questActionInfo.sParam1;
            }
            if (HUtil32.CompareLStr(questActionInfo.sParam2, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(questActionInfo.sParam2, "(", ")", ref sParam2);
            }
            else
            {
                sParam2 = questActionInfo.sParam2;
            }
            if (HUtil32.CompareLStr(questActionInfo.sParam3, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(questActionInfo.sParam3, "(", ")", ref sParam3);
            }
            else
            {
                sParam3 = questActionInfo.sParam3;
            }
            if (HUtil32.IsVarNumber(sParam1))
            {
                if ((!string.IsNullOrEmpty(sParam3)) && (sParam3[0] == '<') && (sParam3[^1] == '>'))
                {
                    result = 0;
                }
                else if ((!string.IsNullOrEmpty(sParam3)) && (M2Share.GetValNameNo(sParam3) >= 0))
                {
                    result = 1;
                }
                else if ((!string.IsNullOrEmpty(sParam3)) && HUtil32.IsStringNumber(sParam3))
                {
                    result = 2;
                }
                else
                {
                    result = 3;
                }
                return result;
            }
            int n01 = M2Share.GetValNameNo(sParam1);
            if (n01 >= 0)
            {
                if (((!string.IsNullOrEmpty(sParam2))) && (sParam2[0] == '<') && (sParam2[^1] == '>'))
                {
                    result = 4;
                }
                else if (((!string.IsNullOrEmpty(sParam2))) && (M2Share.GetValNameNo(sParam2) >= 0))
                {
                    result = 5;
                }
                else if (((!string.IsNullOrEmpty(sParam2))) && HUtil32.IsVarNumber(sParam2))
                {
                    result = 6;
                }
                else
                {
                    result = 7;
                }
                return result;
            }
            return result;
        }

        public static Dictionary<string, DynamicVar> GetDynamicVarMap(PlayObject playObject, string sType, ref string sName)
        {
            Dictionary<string, DynamicVar> result = null;
            if (HUtil32.CompareLStr(sType, "HUMAN", 5))
            {
                result = playObject.DynamicVarMap;
                sName = playObject.ChrName;
            }
            else if (HUtil32.CompareLStr(sType, "GUILD", 5))
            {
                if (playObject.MyGuild == null)
                {
                    return null;
                }
                result = playObject.MyGuild.DynamicVarList;
                sName = playObject.MyGuild.GuildName;
            }
            else if (HUtil32.CompareLStr(sType, "GLOBAL", 6))
            {
                result = M2Share.DynamicVarList;
                sName = "GLOBAL";
            }
            else if (HUtil32.CompareLStr(sType, "Account", 7))
            {
                result = playObject.DynamicVarMap;
                sName = playObject.UserAccount;
            }
            return result;
        }

        /// <summary>
        /// 取文本变量
        /// </summary>
        /// <returns></returns>
        public VarInfo GetVarValue(PlayObject playObject, string sData, ref int nValue)
        {
            string sVar = string.Empty;
            string sValue = string.Empty;
            return GetVarValue(playObject, sData, ref sVar, ref sValue, ref nValue);
        }

        /// <summary>
        /// 取文本变量
        /// </summary>
        /// <returns></returns>
        public VarInfo GetVarValue(PlayObject playObject, string sData, ref string sValue)
        {
            string sVar = string.Empty;
            int nValue = 0;
            return GetVarValue(playObject, sData, ref sVar, ref sValue, ref nValue);
        }

        public bool GetValValue(PlayObject playObject, string sMsg, ref string sValue)
        {
            if (string.IsNullOrEmpty(sMsg))
            {
                return false;
            }
            bool result = false;
            var n01 = M2Share.GetValNameNo(sMsg);
            if (n01 >= 0)
            {
                if (HUtil32.RangeInDefined(n01, 600, 699))
                {
                    sValue = playObject.MSString[n01 - 600];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 700, 799))
                {
                    sValue = M2Share.Config.GlobalAVal[n01 - 700];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1200, 1599))
                {
                    sValue = M2Share.Config.GlobalAVal[n01 - 1100];// A变量(100-499)
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1600, 1699))
                {
                    sValue = playObject.MServerStrVal[n01 - 1600];
                    result = true;
                }
            }
            return result;
        }

        public bool GetValValue(PlayObject playObject, string sMsg, ref int nValue)
        {
            bool result = false;
            if (string.IsNullOrEmpty(sMsg))
            {
                return false;
            }
            int n01 = M2Share.GetValNameNo(sMsg);
            if (n01 >= 0)
            {
                if (HUtil32.RangeInDefined(n01, 0, 99))
                {
                    nValue = playObject.MNVal[n01];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 100, 199))
                {
                    nValue = M2Share.Config.GlobalVal[n01 - 100];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 200, 299))
                {
                    nValue = playObject.MDyVal[n01 - 200];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 300, 399))
                {
                    nValue = playObject.MNMval[n01 - 300];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 400, 499))
                {
                    nValue = M2Share.Config.GlobaDyMval[n01 - 400];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 500, 599))
                {
                    nValue = playObject.MNInteger[n01 - 500];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 600, 699))
                {
                    nValue = HUtil32.StrToInt(playObject.MSString[n01 - 600], 0);
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 700, 799))
                {
                    nValue = HUtil32.StrToInt(M2Share.Config.GlobalAVal[n01 - 700], 0);
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 800, 1199))
                {
                    nValue = M2Share.Config.GlobalVal[n01 - 700];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1200, 1599))
                {
                    nValue = HUtil32.StrToInt(M2Share.Config.GlobalAVal[n01 - 1100], 0);
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1600, 1699))
                {
                    nValue = HUtil32.StrToInt(playObject.MServerStrVal[n01 - 1600], 0);
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1700, 1799))
                {
                    nValue = playObject.MServerIntVal[n01 - 1700];
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 取文本变量
        /// </summary>
        /// <returns></returns>
        public VarInfo GetVarValue(PlayObject playObject, string sData, ref string sVar, ref string sValue, ref int nValue)
        {
            long n10;
            sVar = sData;
            sValue = sData;
            var result = new VarInfo {VarType = VarType.None, VarAttr = VarAttr.aNone};
            if (sData == "")
            {
                return result;
            }
            var sVarName = sData;
            var sName = sData;
            if (sData[0] == '<' && sData[sData.Length - 1] == '>')// <$STR(S0)>
            {
                sData = HUtil32.ArrestStringEx(sData, "<", ">", ref sName);
            }
            if (HUtil32.CompareLStr(sName, "$STR("))// $STR(S0)
            {
                sVar = '<' + sName + '>';
                result.VarType = GetValNameValue(playObject, sVar, ref sValue, ref nValue);
                result.VarAttr = VarAttr.aFixStr;
            }
            else if (HUtil32.CompareLStr(sName, "$HUMAN("))
            {
                sVar = '<' + sName + '>';
                result.VarType = GetDynamicValue(playObject, sVar, ref sValue, ref nValue);
                result.VarAttr = VarAttr.aDynamic;
            }
            else if (HUtil32.CompareLStr(sName, "$GUILD("))
            {
                sVar = '<' + sName + '>';
                result.VarType = GetDynamicValue(playObject, sVar, ref sValue, ref nValue);
                result.VarAttr = VarAttr.aDynamic;
            }
            else if (HUtil32.CompareLStr(sName, "$GLOBAL("))
            {
                sVar = '<' + sName + '>';
                result.VarType = GetDynamicValue(playObject, sVar, ref sValue, ref nValue);
                result.VarAttr = VarAttr.aDynamic;
            }
            else if (sName[0] == '$')
            {
                sName = sName.Substring(2 - 1, sName.Length - 1);
                n10 = M2Share.GetValNameNo(sName);
                if (n10 >= 0)
                {
                    sVar = "<$STR(" + sName + ")>";
                    result.VarType = GetValNameValue(playObject, sVar, ref sValue, ref nValue);
                    result.VarAttr = VarAttr.aFixStr;
                }
                else
                {
                    sVar = "<$" + sName + '>';
                    sValue = GetLineVariableText(playObject, sVar);
                    if (string.Compare(sValue, sVar, StringComparison.Ordinal) == 0)
                    {
                        sValue = sVarName;
                        nValue = HUtil32.StrToInt(sVarName, 0);
                    }
                    else
                    {
                        result.VarType = VarType.String;
                        nValue = HUtil32.StrToInt(sValue, 0);
                        if (HUtil32.IsStringNumber(sValue))
                        {
                            result.VarType = VarType.Integer;
                        }
                    }
                    result.VarAttr = VarAttr.aConst;
                }
            }
            else
            {
                n10 = M2Share.GetValNameNo(sName);
                if (n10 >= 0)
                {
                    sVar = "<$STR(" + sName + ")>";
                    result.VarType = GetValNameValue(playObject, sVar, ref sValue, ref nValue);
                    result.VarAttr = VarAttr.aFixStr;
                }
                else
                {
                    result.VarType = VarType.String;
                    nValue = HUtil32.StrToInt(sValue, 0);
                    if (HUtil32.IsStringNumber(sValue))
                    {
                        result.VarType = VarType.Integer;
                    }
                    result.VarAttr = VarAttr.aConst;
                }
            }
            return result;
        }

        public VarType GetDynamicValue(PlayObject playObject, string sVar, ref string sValue, ref int nValue)
        {
            string sVarName = "";
            string sVarType = "";
            string sData = sVar;
            string sName = "";
            var result = VarType.None;
            if (sData[0] == '<' && sData[sData.Length - 1] == '>')
            {
                sData = HUtil32.ArrestStringEx(sData, "<", ">", ref sName);
            }
            if (HUtil32.CompareLStr(sName, "$HUMAN("))
            {
                sData = HUtil32.ArrestStringEx(sName, "(", ")", ref sVarName);
                sVarType = "HUMAN";
            }
            else if (HUtil32.CompareLStr(sName, "$GUILD("))
            {
                sData = HUtil32.ArrestStringEx(sName, "(", ")", ref sVarName);
                sVarType = "GUILD";
            }
            else if (HUtil32.CompareLStr(sName, "$GLOBAL("))
            {
                sData = HUtil32.ArrestStringEx(sName, "(", ")", ref sVarName);
                sVarType = "GLOBAL";
            }
            if (sVarName == "" || sVarType == "")
            {
                return result;
            }
            Dictionary<string, DynamicVar> DynamicVarList = GeDynamicVarList(playObject, sVarType, ref sName);
            if (DynamicVarList == null)
            {
                return result;
            }
            if (DynamicVarList.TryGetValue(sVarName, out DynamicVar DynamicVar))
            {
                if (string.Compare(DynamicVar.sName, sVarName, StringComparison.Ordinal) == 0)
                {
                    switch (DynamicVar.VarType)
                    {
                        case VarType.Integer:
                            nValue = DynamicVar.nInternet;
                            sValue = nValue.ToString();
                            result = VarType.Integer;
                            break;

                        case VarType.String:
                            sValue = DynamicVar.sString;
                            nValue = HUtil32.StrToInt(sValue, 0);
                            result = VarType.String;
                            break;
                    }
                }
            }
            return result;
        }

        public VarType GetValNameValue(PlayObject playObject, string sVar, ref string sValue, ref int nValue)
        {
            var result = VarType.None;
            var sName = string.Empty;
            if (sVar == "")
            {
                return result;
            }
            var sData = sVar;
            if (sData[0] == '<' && sData[^1] == '>')
            {
                sData = HUtil32.ArrestStringEx(sData, "<", ">", ref sName); // <$STR(S0)>
            }
            if (HUtil32.CompareLStr(sName, "$STR("))
            {
                sData = HUtil32.ArrestStringEx(sName, "(", ")", ref sName); // $STR(S0)
            }
            if (sName[0] == '$')
            {
                sName = sName.Substring(1, sName.Length - 1);// $S0
            }
            var n01 = M2Share.GetValNameNo(sName);
            if (n01 >= 0)
            {
                if (HUtil32.RangeInDefined(n01, 0, 99))
                {
                    nValue = M2Share.Config.GlobalVal[n01];// G
                    sValue = nValue.ToString();
                    result = VarType.Integer;
                }
                else if (HUtil32.RangeInDefined(n01, 1000, 1099))
                {
                    nValue = M2Share.Config.GlobaDyMval[n01 - 1000];// I
                    sValue = nValue.ToString();
                    result = VarType.Integer;
                }
                else if (HUtil32.RangeInDefined(n01, 1100, 1109))
                {
                    nValue = playObject.MNVal[n01 - 1100];// P
                    sValue = nValue.ToString();
                    result = VarType.Integer;
                }
                else if (HUtil32.RangeInDefined(n01, 1110, 1119))
                {
                    nValue = playObject.MDyVal[n01 - 1110];// D
                    sValue = nValue.ToString();
                    result = VarType.Integer;
                }
                else if (HUtil32.RangeInDefined(n01, 1200, 1299))
                {
                    nValue = playObject.MNMval[n01 - 1200];// M
                    sValue = nValue.ToString();
                    result = VarType.Integer;
                }
                else if (HUtil32.RangeInDefined(n01, 1300, 1399))
                {
                    nValue = playObject.MNInteger[n01 - 1300];// N
                    sValue = nValue.ToString();
                    result = VarType.Integer;
                }
                else if (HUtil32.RangeInDefined(n01, 2000, 2499))
                {
                    sValue = M2Share.Config.GlobalAVal[n01 - 2000];// A
                    nValue = HUtil32.StrToInt(sValue, 0);
                    result = VarType.String;
                }
                else if (HUtil32.RangeInDefined(n01, 1400, 1499))
                {
                    sValue = playObject.MSString[n01 - 1400];// S
                    nValue = HUtil32.StrToInt(sValue, 0);
                    result = VarType.String;
                }
            }
            else if (sName != "" && char.ToUpper(sName[0]) == 'S')
            {
                if (playObject.m_StringList.ContainsKey(sName))
                {
                    sValue = playObject.m_StringList[sName];
                }
                else
                {
                    playObject.m_StringList.Add(sName, "");
                    sValue = "";
                }
                result = VarType.String;
            }
            else if (sName != "" && char.ToUpper(sName[0]) == 'N')
            {
                if (playObject.m_IntegerList.ContainsKey(sName))
                {
                    nValue = playObject.m_IntegerList[sName];
                }
                else
                {
                    nValue = 0;
                    playObject.m_IntegerList.Add(sName, nValue);
                }
                result = VarType.Integer;
            }
            return result;
        }

        public string GetLineVariableText(PlayObject playObject, string sMsg)
        {
            var nC = 0;
            var sText = string.Empty;
            var tempstr = sMsg;
            while (true)
            {
                if (tempstr.IndexOf('>') <= 0)
                {
                    break;
                }
                tempstr = HUtil32.ArrestStringEx(tempstr, "<", ">", ref sText);
                if (!string.IsNullOrEmpty(sText) && sText[0] == '$')
                {
                    GetVariableText(playObject, ref sMsg, sText);
                }
                nC++;
                if (nC >= 101)
                {
                    break;
                }
            }
            return sMsg;
        }

        public virtual void GetVariableText(PlayObject playObject, ref string sMsg, string sVariable)
        {
            string dynamicName = string.Empty;
            DynamicVar DynamicVar;
            bool boFoundVar;
            if (HUtil32.IsStringNumber(sVariable))//检查发送字符串是否有数字
            {
                string sIdx = sVariable.Substring(1, sVariable.Length - 1);
                int nIdx = HUtil32.StrToInt(sIdx, -1);
                if (nIdx == -1)
                {
                    return;
                }
                GrobalVarProcessingSys.Handler(playObject, nIdx, sVariable, ref sMsg);
                return;
            }

            // 个人信息
            if (sVariable == "$CMD_ATTACKMODE")
            {
              //  sMsg = CombineStr(sMsg, "<$CMD_ATTACKMODE>", CommandMgr.GameCommands.AttackMode.CmdName);
                return;
            }
            if (sVariable == "$CMD_REST")
            {
               // sMsg = CombineStr(sMsg, "<$CMD_REST>", CommandMgr.GameCommands.Rest.CmdName);
                return;
            }
            if (sVariable == "$CMD_UNLOCK")
            {
               // sMsg = CombineStr(sMsg, "<$CMD_UNLOCK>", CommandMgr.GameCommands.Unlock.CmdName);
                return;
            }

            if (HUtil32.CompareLStr(sVariable, "$HUMAN("))
            {
                HUtil32.ArrestStringEx(sVariable, "(", ")", ref dynamicName);
                boFoundVar = false;
                if (playObject.DynamicVarMap.TryGetValue(dynamicName, out DynamicVar))
                {
                    if (string.Compare(DynamicVar.sName, dynamicName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        switch (DynamicVar.VarType)
                        {
                            case VarType.Integer:
                                sMsg = CombineStr(sMsg, '<' + sVariable + '>', DynamicVar.nInternet.ToString());
                                boFoundVar = true;
                                break;

                            case VarType.String:
                                sMsg = CombineStr(sMsg, '<' + sVariable + '>', DynamicVar.sString);
                                boFoundVar = true;
                                break;
                        }
                    }
                }
                if (!boFoundVar)
                {
                    sMsg = "??";
                }
                return;
            }
            if (HUtil32.CompareLStr(sVariable, "$GUILD("))
            {
                if (playObject.MyGuild == null)
                {
                    return;
                }
                HUtil32.ArrestStringEx(sVariable, "(", ")", ref dynamicName);
                boFoundVar = false;
                if (playObject.MyGuild.DynamicVarList.TryGetValue(dynamicName, out DynamicVar))
                {
                    if (string.Compare(DynamicVar.sName, dynamicName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        switch (DynamicVar.VarType)
                        {
                            case VarType.Integer:
                                sMsg = CombineStr(sMsg, '<' + sVariable + '>', DynamicVar.nInternet);
                                boFoundVar = true;
                                break;

                            case VarType.String:
                                sMsg = CombineStr(sMsg, '<' + sVariable + '>', DynamicVar.sString);
                                boFoundVar = true;
                                break;
                        }
                    }
                }
                if (!boFoundVar)
                {
                    sMsg = "??";
                }
                return;
            }
            if (HUtil32.CompareLStr(sVariable, "$GLOBAL("))
            {
                HUtil32.ArrestStringEx(sVariable, "(", ")", ref dynamicName);
                boFoundVar = false;
                if (M2Share.DynamicVarList.TryGetValue(dynamicName, out DynamicVar))
                {
                    if (string.Compare(DynamicVar.sName, dynamicName, StringComparison.Ordinal) == 0)
                    {
                        switch (DynamicVar.VarType)
                        {
                            case VarType.Integer:
                                sMsg = CombineStr(sMsg, '<' + sVariable + '>', DynamicVar.nInternet.ToString());
                                boFoundVar = true;
                                break;
                            case VarType.String:
                                sMsg = CombineStr(sMsg, '<' + sVariable + '>', DynamicVar.sString);
                                boFoundVar = true;
                                break;
                        }
                    }
                }
                if (!boFoundVar)
                {
                    sMsg = "??";
                }
                return;
            }
            if (HUtil32.CompareLStr(sVariable, "$STR("))
            {
                HUtil32.ArrestStringEx(sVariable, "(", ")", ref dynamicName);
                int n18 = M2Share.GetValNameNo(dynamicName);
                if (n18 >= 0)
                {
                    if (HUtil32.RangeInDefined(n18, 0, 499))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', M2Share.Config.GlobalVal[n18].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 1100, 1109))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', playObject.MNVal[n18 - 1100].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 1110, 1119))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', playObject.MDyVal[n18 - 1110].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 1200, 1299))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', playObject.MNMval[n18 - 1200].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 1000, 1099))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', M2Share.Config.GlobaDyMval[n18 - 1000].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 1300, 1399))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', playObject.MNInteger[n18 - 1300].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 1400, 1499))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', playObject.MSString[n18 - 1400]);
                    }
                    else if (HUtil32.RangeInDefined(n18, 2000, 2499))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', M2Share.Config.GlobalAVal[n18 - 2000]);
                    }
                }
                else if (dynamicName != "" && char.ToUpper(dynamicName[1]) == 'S')
                {
                    sMsg = CombineStr(sMsg, '<' + sVariable + '>', playObject.m_StringList.ContainsKey(dynamicName) ? playObject.m_StringList[dynamicName] : "");
                }
                else if (dynamicName != "" && char.ToUpper(dynamicName[1]) == 'N')
                {
                    sMsg = CombineStr(sMsg, '<' + sVariable + '>', playObject.m_IntegerList.ContainsKey(dynamicName) ? Convert.ToString(playObject.m_IntegerList[dynamicName]) : "-1");
                }
            }
        }

        public bool SetDynamicValue(PlayObject playObject, string sVar, string sValue, int nValue)
        {
            var result = false;
            var sVarName = "";
            var sVarType = "";
            var sName = "";
            var sData = sVar;
            if (sData[0] == '<' && sData[sData.Length - 1] == '>')
            {
                sData = HUtil32.ArrestStringEx(sData, "<", ">", ref sName);
            }
            if (HUtil32.CompareLStr(sName, "$HUMAN("))
            {
                sData = HUtil32.ArrestStringEx(sName, "(", ")", ref sVarName);
                sVarType = "HUMAN";
            }
            else if (HUtil32.CompareLStr(sName, "$GUILD("))
            {
                sData = HUtil32.ArrestStringEx(sName, "(", ")", ref sVarName);
                sVarType = "GUILD";
            }
            else if (HUtil32.CompareLStr(sName, "$GLOBAL("))
            {
                sData = HUtil32.ArrestStringEx(sName, "(", ")", ref sVarName);
                sVarType = "GLOBAL";
            }
            if (sVarName == "" || sVarType == "")
            {
                return false;
            }
            var dynamicVarList = GeDynamicVarList(playObject, sVarType, ref sName);
            if (dynamicVarList == null)
            {
                return false;
            }
            if (dynamicVarList.TryGetValue(sVarName, out var dynamicVar))
            {
                switch (dynamicVar.VarType)
                {
                    case VarType.Integer:
                        dynamicVar.nInternet = nValue;
                        break;

                    case VarType.String:
                        dynamicVar.sString = sValue;
                        break;
                }
                result = true;
            }
            return result;
        }

        public bool SetValNameValue(PlayObject playObject, string sVar, string sValue, int nValue)
        {
            var sName = string.Empty;
            var result = false;
            if (sVar == "")
            {
                return false;
            }
            var sData = sVar;
            if (sData[0] == '<' && sData[sData.Length - 1] == '>') // <$STR(S0)>
            {
                sData = HUtil32.ArrestStringEx(sData, "<", ">", ref sName);
            }
            if (HUtil32.CompareLStr(sName, "$STR("))// $STR(S0)
            {
                sData = HUtil32.ArrestStringEx(sName, "(", ")", ref sName);
            }
            if (sName[0] == '$')// $S0
            {
                sName = sName.Substring(1, sName.Length - 1);
            }
            var n01 = M2Share.GetValNameNo(sName);
            if (n01 >= 0)
            {
                if (HUtil32.RangeInDefined(n01, 0, 499))
                {
                    M2Share.Config.GlobalVal[n01] = nValue;// G
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1000, 1099))
                {
                    M2Share.Config.GlobaDyMval[n01 - 1000] = nValue;// I
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1100, 1109))
                {
                    playObject.MNVal[n01 - 1100] = nValue;// P
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1110, 1119))
                {
                    playObject.MDyVal[n01 - 1110] = nValue;// D
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1200, 1299))
                {
                    playObject.MNMval[n01 - 1200] = nValue;// M
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1300, 1399))
                {
                    playObject.MNInteger[n01 - 1300] = nValue;// N
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 2000, 2499))
                {
                    M2Share.Config.GlobalAVal[n01 - 2000] = sValue;// A
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1400, 1499))
                {
                    playObject.MSString[n01 - 1400] = sValue;// S
                    result = true;
                }
            }
            else if (sName != "" && char.ToUpper(sName[0]) == 'S')
            {
                if (playObject.m_StringList.ContainsKey(sName))
                {
                    playObject.m_StringList[sName] = sValue;
                    result = true;
                }
                else
                {
                    playObject.m_StringList.Add(sName, sValue);
                    result = true;
                }
            }
            else if (sName != "" && char.ToUpper(sName[0]) == 'N')
            {
                if (playObject.m_IntegerList.ContainsKey(sName))
                {
                    playObject.m_IntegerList[sName] = nValue;
                    result = true;
                }
                else
                {
                    playObject.m_IntegerList.Add(sName, nValue);
                    result = true;
                }
            }
            return result;
        }

        public Dictionary<string, DynamicVar> GeDynamicVarList(PlayObject playObject, string sType, ref string sName)
        {
            Dictionary<string, DynamicVar> result = null;
            if (HUtil32.CompareLStr(sType, "HUMAN"))
            {
                result = playObject.DynamicVarMap;
                sName = playObject.ChrName;
            }
            else if (HUtil32.CompareLStr(sType, "GUILD"))
            {
                if (playObject.MyGuild == null)
                {
                    return null;
                }
                result = playObject.MyGuild.DynamicVarList;
                sName = playObject.MyGuild.GuildName;
            }
            else if (HUtil32.CompareLStr(sType, "GLOBAL"))
            {
                result = M2Share.DynamicVarList;
                sName = "GLOBAL";
            }
            return result;
        }

        /// <summary>
        /// 合并字符串
        /// </summary>
        /// <returns></returns>
        public string CombineStr(string sMsg, string variable, object variableVal)
        {
            string result;
            var n10 = sMsg.IndexOf(variable, StringComparison.Ordinal);
            if (n10 > -1)
            {
                var s14 = sMsg.Substring(1 - 1, n10);
                var s18 = sMsg.Substring(variable.Length + n10, sMsg.Length - (variable.Length + n10));
                result = s14 + Convert.ToString(variableVal) + s18;
            }
            else
            {
                result = sMsg;
            }
            return result;
        }

        public bool GotoLableCheckStringList(string sHumName, string sListFileName)
        {
            bool result = false;
            StringList LoadList;
            sListFileName = M2Share.GetEnvirFilePath(sListFileName);
            if (File.Exists(sListFileName))
            {
                LoadList = new StringList();
                try
                {
                    LoadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    M2Share.Logger.Error("loading fail.... => " + sListFileName);
                }
                for (int i = 0; i < LoadList.Count; i++)
                {
                    if (string.Compare(LoadList[i].Trim(), sHumName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = true;
                        break;
                    }
                }
                LoadList = null;
            }
            else
            {
                M2Share.Logger.Error("file not found => " + sListFileName);
            }
            return result;
        }

        protected static string ReplaceVariableText(string sMsg, string sStr, string sText)
        {
            int n10 = sMsg.IndexOf(sStr, StringComparison.OrdinalIgnoreCase);
            if (n10 > -1)
            {
                ReadOnlySpan<char> s18 = sMsg.AsSpan()[(sStr.Length + n10)..sMsg.Length];
                StringBuilder builder = new StringBuilder();
                builder.Append(sMsg[..n10]);
                builder.Append(sText);
                builder.Append(s18);
                return builder.ToString();
            }
            return sMsg;
        }
    }
}
