using GameSrv.GameCommand;
using GameSrv.Npc;
using GameSrv.Player;
using System.Text;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSrv
{
    internal class ProcessingBase
    {
        internal bool GetMovDataHumanInfoValue(NormNpc normNpc, PlayObject PlayObject, string sVariable, ref string sValue, ref int nValue, ref int nDataType)
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
                    sValue = PlayObject.ChrName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$KILLER":// 杀人者变量 
                    {
                        if (PlayObject.Death && (PlayObject.LastHiter != null))
                        {
                            if (PlayObject.LastHiter.Race == ActorRace.Play)
                            {
                                sValue = PlayObject.LastHiter.ChrName;
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
                        if (PlayObject.Death && (PlayObject.LastHiter != null))
                        {
                            if (PlayObject.LastHiter.Race != ActorRace.Play)
                            {
                                sValue = PlayObject.LastHiter.ChrName;
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
                    sValue = PlayObject.GetShowName();
                    nDataType = 0;
                    result = true;
                    return result;
                case "$SFNAME":// 师傅名 
                    sValue = PlayObject.MasterName;
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
                    sValue = PlayObject.UserAccount;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$IPADDR":// 登录IP
                    sValue = PlayObject.LoginIpAddr;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$X": // 人物X坐标
                    nValue = PlayObject.CurrX;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$Y": // 人物Y坐标
                    nValue = PlayObject.CurrY;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAP":
                    sValue = PlayObject.Envir.MapName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$GUILDNAME":
                    {
                        if (PlayObject.MyGuild != null)
                        {
                            sValue = PlayObject.MyGuild.GuildName;
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
                    sValue = PlayObject.GuildRankName;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$RELEVEL":
                    nValue = PlayObject.ReLevel;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$LEVEL":
                    nValue = PlayObject.Abil.Level;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$HP":
                    nValue = PlayObject.WAbil.HP;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXHP":
                    nValue = PlayObject.WAbil.MaxHP;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MP":
                    nValue = PlayObject.WAbil.MP;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXMP":
                    nValue = PlayObject.WAbil.MaxMP;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$AC":
                    nValue = HUtil32.LoWord(PlayObject.WAbil.AC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXAC":
                    nValue = HUtil32.HiWord(PlayObject.WAbil.AC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAC":
                    nValue = HUtil32.LoWord(PlayObject.WAbil.MAC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXMAC":
                    nValue = HUtil32.HiWord(PlayObject.WAbil.MAC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$DC":
                    nValue = HUtil32.LoWord(PlayObject.WAbil.DC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXDC":
                    nValue = HUtil32.HiWord(PlayObject.WAbil.DC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MC":
                    nValue = HUtil32.LoWord(PlayObject.WAbil.MC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXMC":
                    nValue = HUtil32.HiWord(PlayObject.WAbil.MC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$SC":
                    nValue = HUtil32.LoWord(PlayObject.WAbil.SC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXSC":
                    nValue = HUtil32.HiWord(PlayObject.WAbil.SC);
                    nDataType = 1;
                    result = true;
                    return result;
                case "$EXP":
                    nValue = PlayObject.Abil.Exp;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXEXP":
                    nValue = PlayObject.Abil.MaxExp;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$PKPOINT":
                    nValue = PlayObject.PkPoint;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$CREDITPOINT":
                    nValue = PlayObject.CreditPoint;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$HW":
                    nValue = PlayObject.WAbil.HandWeight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXHW":
                    nValue = PlayObject.WAbil.MaxHandWeight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$BW":
                    nValue = PlayObject.WAbil.Weight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXBW":
                    nValue = PlayObject.WAbil.MaxWeight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$WW":
                    nValue = PlayObject.WAbil.WearWeight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$MAXWW":
                    nValue = PlayObject.WAbil.MaxWearWeight;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$GOLDCOUNT":
                    nValue = PlayObject.Gold;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$GOLDCOUNTX":
                    nValue = PlayObject.GoldMax;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$GAMEGOLD":
                    nValue = PlayObject.GameGold;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$GAMEPOINT":
                    nValue = PlayObject.GamePoint;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$HUNGER":
                    nValue = PlayObject.GetMyStatus();
                    nDataType = 1;
                    result = true;
                    return result;
                case "$LOGINTIME":
                    sValue = DateTimeOffset.FromUnixTimeMilliseconds(PlayObject.LogonTime).ToString("yyyy-MM-dd HH:mm:ss");
                    nDataType = 0;
                    result = true;
                    return result;
                case "$LOGINLONG":
                    nValue = (HUtil32.GetTickCount() - PlayObject.LogonTick) / 60000;
                    nDataType = 1;
                    result = true;
                    return result;
                case "$DRESS":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Dress].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$WEAPON":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Weapon].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$RIGHTHAND":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.RighThand].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$HELMET":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Helmet].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$NECKLACE":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Necklace].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$RING_R":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Ringr].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$RING_L":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Ringl].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$ARMRING_R":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.ArmRingr].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$ARMRING_L":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.ArmRingl].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BUJUK":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Bujuk].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BELT":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Belt].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$BOOTS":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Boots].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$CHARM":
                    sValue = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Charm].Index);
                    nDataType = 0;
                    result = true;
                    return result;
                case "$IPLOCAL":
                    sValue = PlayObject.LoginIpLocal;
                    nDataType = 0;
                    result = true;
                    return result;
                case "$GUILDBUILDPOINT":
                    {
                        if (PlayObject.MyGuild != null)
                        {
                            //nValue = PlayObject.MyGuild.nBuildPoint;
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
                case "$GUILDAURAEPOINT":
                    {
                        if (PlayObject.MyGuild != null)
                        {
                            nValue = PlayObject.MyGuild.Aurae;
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
                case "$GUILDSTABILITYPOINT":
                    {
                        if (PlayObject.MyGuild != null)
                        {
                            nValue = PlayObject.MyGuild.Stability;
                        }
                        nDataType = 0;
                        result = true;
                        return result;
                    }
                case "$GUILDFLOURISHPOINT":
                    {
                        if (PlayObject.MyGuild != null)
                        {
                            nValue = PlayObject.MyGuild.Flourishing;
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
                if (PlayObject.DynamicVarMap.TryGetValue(sVarValue2, out DynamicVar))
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
                if (PlayObject.DynamicVarMap.TryGetValue(sVarValue2, out DynamicVar))
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

        internal static bool SetMovDataValNameValue(PlayObject PlayObject, string sVarName, string sValue, int nValue, int nDataType)
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
                            PlayObject.MNVal[n100] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 100, 199))
                        {
                            M2Share.Config.GlobalVal[n100 - 100] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 200, 299))
                        {
                            PlayObject.MDyVal[n100 - 200] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 300, 399))
                        {
                            PlayObject.MNMval[n100 - 300] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 400, 499))
                        {
                            M2Share.Config.GlobaDyMval[n100 - 400] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 500, 599))
                        {
                            PlayObject.MNInteger[n100 - 500] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 600, 699))
                        {
                            PlayObject.MSString[n100 - 600] = nValue.ToString();
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
                            PlayObject.MNVal[n100] = HUtil32.StrToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 100, 199))
                        {
                            M2Share.Config.GlobalVal[n100 - 100] = HUtil32.StrToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 200, 299))
                        {
                            PlayObject.MDyVal[n100 - 200] = HUtil32.StrToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 300, 399))
                        {
                            PlayObject.MNMval[n100 - 300] = HUtil32.StrToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 400, 499))
                        {
                            M2Share.Config.GlobaDyMval[n100 - 400] = HUtil32.StrToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 500, 599))
                        {
                            PlayObject.MNInteger[n100 - 500] = HUtil32.StrToInt(sValue, 0);
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 600, 699))
                        {
                            PlayObject.MSString[n100 - 600] = sValue;
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
                            PlayObject.MNVal[n100] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 100, 199))
                        {
                            M2Share.Config.GlobalVal[n100 - 100] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 200, 299))
                        {
                            PlayObject.MDyVal[n100 - 200] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 300, 399))
                        {
                            PlayObject.MNMval[n100 - 300] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 400, 499))
                        {
                            M2Share.Config.GlobaDyMval[n100 - 400] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 500, 599))
                        {
                            PlayObject.MNInteger[n100 - 500] = nValue;
                            result = true;
                        }
                        else if (HUtil32.RangeInDefined(n100, 600, 699))
                        {
                            PlayObject.MSString[n100 - 600] = sValue;
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

        internal static bool GetMovDataValNameValue(PlayObject PlayObject, string sVarName, ref string sValue, ref int nValue, ref int nDataType)
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
                    nValue = PlayObject.MNVal[n100];
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
                    nValue = PlayObject.MDyVal[n100 - 200];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 300, 399))
                {
                    nValue = PlayObject.MNMval[n100 - 300];
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
                    nValue = PlayObject.MNInteger[n100 - 500];
                    nDataType = 1;
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n100, 600, 699))
                {
                    sValue = PlayObject.MSString[n100 - 600];
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

        internal static bool GetMovDataDynamicVarValue(PlayObject PlayObject, string sVarType, string sVarName, ref string sValue, ref int nValue, ref int nDataType)
        {
            string sName = string.Empty;
            sValue = "";
            nValue = -1;
            nDataType = -1;
            Dictionary<string, DynamicVar> DynamicVarList = GetDynamicVarMap(PlayObject, sVarType, ref sName);
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

        internal static bool SetMovDataDynamicVarValue(PlayObject PlayObject, string sVarType, string sVarName, string sValue, int nValue, int nDataType)
        {
            string sName = string.Empty;
            bool boVarFound = false;
            Dictionary<string, DynamicVar> DynamicVarList = GetDynamicVarMap(PlayObject, sVarType, ref sName);
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

        internal static int GetMovDataType(QuestActionInfo QuestActionInfo)
        {
            string sParam1 = string.Empty;
            string sParam2 = string.Empty;
            string sParam3 = string.Empty;
            int result = -1;
            if (HUtil32.CompareLStr(QuestActionInfo.sParam1, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam1, "(", ")", ref sParam1);
            }
            else
            {
                sParam1 = QuestActionInfo.sParam1;
            }
            if (HUtil32.CompareLStr(QuestActionInfo.sParam2, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam2, "(", ")", ref sParam2);
            }
            else
            {
                sParam2 = QuestActionInfo.sParam2;
            }
            if (HUtil32.CompareLStr(QuestActionInfo.sParam3, "<$STR(", 6))
            {
                HUtil32.ArrestStringEx(QuestActionInfo.sParam3, "(", ")", ref sParam3);
            }
            else
            {
                sParam3 = QuestActionInfo.sParam3;
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

        public static Dictionary<string, DynamicVar> GetDynamicVarMap(PlayObject PlayObject, string sType, ref string sName)
        {
            Dictionary<string, DynamicVar> result = null;
            if (HUtil32.CompareLStr(sType, "HUMAN", 5))
            {
                result = PlayObject.DynamicVarMap;
                sName = PlayObject.ChrName;
            }
            else if (HUtil32.CompareLStr(sType, "GUILD", 5))
            {
                if (PlayObject.MyGuild == null)
                {
                    return null;
                }
                result = PlayObject.MyGuild.DynamicVarList;
                sName = PlayObject.MyGuild.GuildName;
            }
            else if (HUtil32.CompareLStr(sType, "GLOBAL", 6))
            {
                result = M2Share.DynamicVarList;
                sName = "GLOBAL";
            }
            else if (HUtil32.CompareLStr(sType, "Account", 7))
            {
                result = PlayObject.DynamicVarMap;
                sName = PlayObject.UserAccount;
            }
            return result;
        }

        /// <summary>
        /// 取文本变量
        /// </summary>
        /// <returns></returns>
        public VarInfo GetVarValue(PlayObject PlayObject, string sData, ref int nValue)
        {
            string sVar = string.Empty;
            string sValue = string.Empty;
            return GetVarValue(PlayObject, sData, ref sVar, ref sValue, ref nValue);
        }

        /// <summary>
        /// 取文本变量
        /// </summary>
        /// <returns></returns>
        public VarInfo GetVarValue(PlayObject PlayObject, string sData, ref string sValue)
        {
            string sVar = string.Empty;
            int nValue = 0;
            return GetVarValue(PlayObject, sData, ref sVar, ref sValue, ref nValue);
        }

        public bool GetValValue(PlayObject PlayObject, string sMsg, ref string sValue)
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
                    sValue = PlayObject.MSString[n01 - 600];
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
                    sValue = PlayObject.MServerStrVal[n01 - 1600];
                    result = true;
                }
            }
            return result;
        }

        public bool GetValValue(PlayObject PlayObject, string sMsg, ref int nValue)
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
                    nValue = PlayObject.MNVal[n01];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 100, 199))
                {
                    nValue = M2Share.Config.GlobalVal[n01 - 100];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 200, 299))
                {
                    nValue = PlayObject.MDyVal[n01 - 200];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 300, 399))
                {
                    nValue = PlayObject.MNMval[n01 - 300];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 400, 499))
                {
                    nValue = M2Share.Config.GlobaDyMval[n01 - 400];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 500, 599))
                {
                    nValue = PlayObject.MNInteger[n01 - 500];
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 600, 699))
                {
                    nValue = HUtil32.StrToInt(PlayObject.MSString[n01 - 600], 0);
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
                    nValue = HUtil32.StrToInt(PlayObject.MServerStrVal[n01 - 1600], 0);
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1700, 1799))
                {
                    nValue = PlayObject.MServerIntVal[n01 - 1700];
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 取文本变量
        /// </summary>
        /// <returns></returns>
        public VarInfo GetVarValue(PlayObject PlayObject, string sData, ref string sVar, ref string sValue, ref int nValue)
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
                result.VarType = GetValNameValue(PlayObject, sVar, ref sValue, ref nValue);
                result.VarAttr = VarAttr.aFixStr;
            }
            else if (HUtil32.CompareLStr(sName, "$HUMAN("))
            {
                sVar = '<' + sName + '>';
                result.VarType = GetDynamicValue(PlayObject, sVar, ref sValue, ref nValue);
                result.VarAttr = VarAttr.aDynamic;
            }
            else if (HUtil32.CompareLStr(sName, "$GUILD("))
            {
                sVar = '<' + sName + '>';
                result.VarType = GetDynamicValue(PlayObject, sVar, ref sValue, ref nValue);
                result.VarAttr = VarAttr.aDynamic;
            }
            else if (HUtil32.CompareLStr(sName, "$GLOBAL("))
            {
                sVar = '<' + sName + '>';
                result.VarType = GetDynamicValue(PlayObject, sVar, ref sValue, ref nValue);
                result.VarAttr = VarAttr.aDynamic;
            }
            else if (sName[0] == '$')
            {
                sName = sName.Substring(2 - 1, sName.Length - 1);
                n10 = M2Share.GetValNameNo(sName);
                if (n10 >= 0)
                {
                    sVar = "<$STR(" + sName + ")>";
                    result.VarType = GetValNameValue(PlayObject, sVar, ref sValue, ref nValue);
                    result.VarAttr = VarAttr.aFixStr;
                }
                else
                {
                    sVar = "<$" + sName + '>';
                    sValue = GetLineVariableText(PlayObject, sVar);
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
                    result.VarType = GetValNameValue(PlayObject, sVar, ref sValue, ref nValue);
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

        public VarType GetDynamicValue(PlayObject PlayObject, string sVar, ref string sValue, ref int nValue)
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
            Dictionary<string, DynamicVar> DynamicVarList = GeDynamicVarList(PlayObject, sVarType, ref sName);
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

        public VarType GetValNameValue(PlayObject PlayObject, string sVar, ref string sValue, ref int nValue)
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
                    nValue = PlayObject.MNVal[n01 - 1100];// P
                    sValue = nValue.ToString();
                    result = VarType.Integer;
                }
                else if (HUtil32.RangeInDefined(n01, 1110, 1119))
                {
                    nValue = PlayObject.MDyVal[n01 - 1110];// D
                    sValue = nValue.ToString();
                    result = VarType.Integer;
                }
                else if (HUtil32.RangeInDefined(n01, 1200, 1299))
                {
                    nValue = PlayObject.MNMval[n01 - 1200];// M
                    sValue = nValue.ToString();
                    result = VarType.Integer;
                }
                else if (HUtil32.RangeInDefined(n01, 1300, 1399))
                {
                    nValue = PlayObject.MNInteger[n01 - 1300];// N
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
                    sValue = PlayObject.MSString[n01 - 1400];// S
                    nValue = HUtil32.StrToInt(sValue, 0);
                    result = VarType.String;
                }
            }
            else if (sName != "" && char.ToUpper(sName[0]) == 'S')
            {
                if (PlayObject.m_StringList.ContainsKey(sName))
                {
                    sValue = PlayObject.m_StringList[sName];
                }
                else
                {
                    PlayObject.m_StringList.Add(sName, "");
                    sValue = "";
                }
                result = VarType.String;
            }
            else if (sName != "" && char.ToUpper(sName[0]) == 'N')
            {
                if (PlayObject.m_IntegerList.ContainsKey(sName))
                {
                    nValue = PlayObject.m_IntegerList[sName];
                }
                else
                {
                    nValue = 0;
                    PlayObject.m_IntegerList.Add(sName, nValue);
                }
                result = VarType.Integer;
            }
            return result;
        }

        public string GetLineVariableText(PlayObject PlayObject, string sMsg)
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
                    GetVariableText(PlayObject, ref sMsg, sText);
                }
                nC++;
                if (nC >= 101)
                {
                    break;
                }
            }
            return sMsg;
        }

        public virtual void GetVariableText(PlayObject PlayObject, ref string sMsg, string sVariable)
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
                GrobalVarProcessingSys.Handler(PlayObject, nIdx, sVariable, ref sMsg);
                return;
            }
            // 个人信息
            if (sVariable == "$CMD_ATTACKMODE")
            {
                sMsg = CombineStr(sMsg, "<$CMD_ATTACKMODE>", CommandMgr.GameCommands.AttackMode.CmdName);
                return;
            }
            if (sVariable == "$CMD_REST")
            {
                sMsg = CombineStr(sMsg, "<$CMD_REST>", CommandMgr.GameCommands.Rest.CmdName);
                return;
            }
            if (sVariable == "$CMD_UNLOCK")
            {
                sMsg = CombineStr(sMsg, "<$CMD_UNLOCK>", CommandMgr.GameCommands.Unlock.CmdName);
                return;
            }

            if (HUtil32.CompareLStr(sVariable, "$HUMAN("))
            {
                HUtil32.ArrestStringEx(sVariable, "(", ")", ref dynamicName);
                boFoundVar = false;
                if (PlayObject.DynamicVarMap.TryGetValue(dynamicName, out DynamicVar))
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
                if (PlayObject.MyGuild == null)
                {
                    return;
                }
                HUtil32.ArrestStringEx(sVariable, "(", ")", ref dynamicName);
                boFoundVar = false;
                if (PlayObject.MyGuild.DynamicVarList.TryGetValue(dynamicName, out DynamicVar))
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
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', PlayObject.MNVal[n18 - 1100].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 1110, 1119))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', PlayObject.MDyVal[n18 - 1110].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 1200, 1299))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', PlayObject.MNMval[n18 - 1200].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 1000, 1099))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', M2Share.Config.GlobaDyMval[n18 - 1000].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 1300, 1399))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', PlayObject.MNInteger[n18 - 1300].ToString());
                    }
                    else if (HUtil32.RangeInDefined(n18, 1400, 1499))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', PlayObject.MSString[n18 - 1400]);
                    }
                    else if (HUtil32.RangeInDefined(n18, 2000, 2499))
                    {
                        sMsg = CombineStr(sMsg, '<' + sVariable + '>', M2Share.Config.GlobalAVal[n18 - 2000]);
                    }
                }
                else if (dynamicName != "" && char.ToUpper(dynamicName[1]) == 'S')
                {
                    sMsg = CombineStr(sMsg, '<' + sVariable + '>', PlayObject.m_StringList.ContainsKey(dynamicName) ? PlayObject.m_StringList[dynamicName] : "");
                }
                else if (dynamicName != "" && char.ToUpper(dynamicName[1]) == 'N')
                {
                    sMsg = CombineStr(sMsg, '<' + sVariable + '>', PlayObject.m_IntegerList.ContainsKey(dynamicName) ? Convert.ToString(PlayObject.m_IntegerList[dynamicName]) : "-1");
                }
            }
        }

        public bool SetDynamicValue(PlayObject PlayObject, string sVar, string sValue, int nValue)
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
            var dynamicVarList = GeDynamicVarList(PlayObject, sVarType, ref sName);
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

        public bool SetValNameValue(PlayObject PlayObject, string sVar, string sValue, int nValue)
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
                    PlayObject.MNVal[n01 - 1100] = nValue;// P
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1110, 1119))
                {
                    PlayObject.MDyVal[n01 - 1110] = nValue;// D
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1200, 1299))
                {
                    PlayObject.MNMval[n01 - 1200] = nValue;// M
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1300, 1399))
                {
                    PlayObject.MNInteger[n01 - 1300] = nValue;// N
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 2000, 2499))
                {
                    M2Share.Config.GlobalAVal[n01 - 2000] = sValue;// A
                    result = true;
                }
                else if (HUtil32.RangeInDefined(n01, 1400, 1499))
                {
                    PlayObject.MSString[n01 - 1400] = sValue;// S
                    result = true;
                }
            }
            else if (sName != "" && char.ToUpper(sName[0]) == 'S')
            {
                if (PlayObject.m_StringList.ContainsKey(sName))
                {
                    PlayObject.m_StringList[sName] = sValue;
                    result = true;
                }
                else
                {
                    PlayObject.m_StringList.Add(sName, sValue);
                    result = true;
                }
            }
            else if (sName != "" && char.ToUpper(sName[0]) == 'N')
            {
                if (PlayObject.m_IntegerList.ContainsKey(sName))
                {
                    PlayObject.m_IntegerList[sName] = nValue;
                    result = true;
                }
                else
                {
                    PlayObject.m_IntegerList.Add(sName, nValue);
                    result = true;
                }
            }
            return result;
        }

        public Dictionary<string, DynamicVar> GeDynamicVarList(PlayObject PlayObject, string sType, ref string sName)
        {
            Dictionary<string, DynamicVar> result = null;
            if (HUtil32.CompareLStr(sType, "HUMAN"))
            {
                result = PlayObject.DynamicVarMap;
                sName = PlayObject.ChrName;
            }
            else if (HUtil32.CompareLStr(sType, "GUILD"))
            {
                if (PlayObject.MyGuild == null)
                {
                    return null;
                }
                result = PlayObject.MyGuild.DynamicVarList;
                sName = PlayObject.MyGuild.GuildName;
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
