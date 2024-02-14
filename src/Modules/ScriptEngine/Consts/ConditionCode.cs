namespace ScriptSystem.Consts
{
    /// <summary>
    /// 条件检测编码定义
    /// </summary>
    public enum ConditionCode : short
    {
        [ScriptDefName("CHECK")]
        CHECK = 1,
        [ScriptDefName("RANDOM")]
        RANDOM,
        [ScriptDefName("GENDER")]
        GENDER,
        [ScriptDefName("DAYTIME")]
        DAYTIME,
        [ScriptDefName("CHECKOPEN")]
        CHECKOPEN,
        [ScriptDefName("CHECKUNIT")]
        CHECKUNIT,
        [ScriptDefName("CHECKLEVEL")]
        CHECKLEVEL,
        [ScriptDefName("CHECKJOB")]
        CHECKJOB,
        [ScriptDefName("CHECKBBCOUNT")]
        CHECKBBCOUNT,
        [ScriptDefName("CHECKITEM")]
        CHECKITEM,
        [ScriptDefName("CHECKITEMW")]
        CHECKITEMW,
        [ScriptDefName("CHECKGOLD")]
        CHECKGOLD,
        [ScriptDefName("ISTAKEITEM")]
        ISTAKEITEM,
        [ScriptDefName("CHECKDURA")]
        CHECKDURA,
        [ScriptDefName("CHECKDURAEVA")]
        CHECKDURAEVA,
        [ScriptDefName("DAYOFWEEK")]
        DAYOFWEEK,
        [ScriptDefName("HOUR")]
        HOUR,
        [ScriptDefName("MIN")]
        MIN,
        [ScriptDefName("CHECKPKPOINT")]
        CHECKPKPOINT,
        [ScriptDefName("CHECKLUCKYPOINT")]
        CHECKLUCKYPOINT,
        [ScriptDefName("CHECKMONMAP")]
        CHECKMONMAP,
        [ScriptDefName("CHECKMONAREA")]
        CHECKMONAREA,
        [ScriptDefName("CHECKHUM")]
        CHECKHUM,
        [ScriptDefName("CHECKBAGGAGE")]
        CHECKBAGGAGE,
        [ScriptDefName("EQUAL")]
        EQUAL,
        [ScriptDefName("LARGE")]
        LARGE,
        [ScriptDefName("SMALL")]
        SMALL,
        [ScriptDefName("CHECKMAGIC")]
        CHECKMAGIC,
        [ScriptDefName("CHKMAGICLEVEL")]
        CHKMAGICLEVEL,
        [ScriptDefName("CHECKMONRECALL")]
        CHECKMONRECALL,
        [ScriptDefName("CHECKHORSE")]
        CHECKHORSE,
        [ScriptDefName("CHECKRIDING")]
        CHECKRIDING,
        [ScriptDefName("STARTDAILYQUEST")]
        STARTDAILYQUEST,
        [ScriptDefName("CHECKDAILYQUEST")]
        CHECKDAILYQUEST,
        [ScriptDefName("RANDOMEX")]
        RANDOMEX,
        [ScriptDefName("CHECKNAMELIST")]
        CHECKNAMELIST,
        [ScriptDefName("CHECKWEAPONLEVEL")]
        CHECKWEAPONLEVEL,
        [ScriptDefName("CHECKWEAPONATOM")]
        CHECKWEAPONATOM,
        [ScriptDefName("CHECKREFINEWEAPON")]
        CHECKREFINEWEAPON,
        [ScriptDefName("CHECKWEAPONMCTYPE")]
        CHECKWEAPONMCTYPE,
        [ScriptDefName("CHECKREFINEITEM")]
        CHECKREFINEITEM,
        [ScriptDefName("HASWEAPONATOM")]
        HASWEAPONATOM,
        [ScriptDefName("ISGUILDMASTER")]
        ISGUILDMASTER,
        [ScriptDefName("CANPROPOSECASTLEWAR")]
        CANPROPOSECASTLEWAR,
        [ScriptDefName("CANHAVESHOOTER")]
        CANHAVESHOOTER,
        [ScriptDefName("CHECKFAME")]
        CHECKFAME,
        [ScriptDefName("ISONCASTLEWAR")]
        ISONCASTLEWAR,
        [ScriptDefName("ISONREADYCASTLEWAR")]
        ISONREADYCASTLEWAR,
        [ScriptDefName("ISCASTLEGUILD")]
        ISCASTLEGUILD,
        /// <summary>
        /// 是否为攻城方
        /// </summary>
        [ScriptDefName("ISATTACKGUILD")]
        ISATTACKGUILD,
        /// <summary>
        /// 是否为守城方
        /// </summary>
        [ScriptDefName("ISDEFENSEGUILD")]
        ISDEFENSEGUILD,
        [ScriptDefName("CHECKSHOOTER")]
        CHECKSHOOTER,
        [ScriptDefName("CHECKSAVEDSHOOTER")]
        CHECKSAVEDSHOOTER,
        /// <summary>
        /// 是否加入行会
        /// </summary>
        [ScriptDefName("HAVEGUILD")]
        HASGUILD,
        /// <summary>
        /// 检查城门
        /// </summary>
        [ScriptDefName("CHECKCASTLEDOOR")]
        CHECKCASTLEDOOR,
        /// <summary>
        /// 城门是否打开
        /// </summary>
        [ScriptDefName("CHECKCASTLEDOOROPEN")]
        CHECKCASTLEDOOROPEN,
        [ScriptDefName("CHECKPOS")]
        CHECKPOS,
        [ScriptDefName("CANCHARGESHOOTER")]
        CANCHARGESHOOTER,
        /// <summary>
        /// 是否为攻城方联盟行会
        /// </summary>
        [ScriptDefName("ISATTACKALLYGUILD")]
        ISATTACKALLYGUILD,
        /// <summary>
        /// 是否为守城方联盟行会
        /// </summary>
        [ScriptDefName("ISDEFENSEALLYGUILD")]
        ISDEFENSEALLYGUILD,
        [ScriptDefName("TESTTEAM")]
        TESTTEAM,
        [ScriptDefName("ISSYSOP")]
        ISSYSOP,
        [ScriptDefName("ISADMIN")]
        ISADMIN,
        [ScriptDefName("CHECKBONUS")]
        CHECKBONUS,
        [ScriptDefName("CHECKMARRIAGE")]
        CHECKMARRIAGE,
        [ScriptDefName("CHECKMARRIAGERING")]
        CHECKMARRIAGERING,
        [ScriptDefName("CHECKGMETERM")]
        CHECKGMETERM,
        [ScriptDefName("CHECKOPENGME")]
        CHECKOPENGME,
        [ScriptDefName("CHECKENTERGMEMAP")]
        CHECKENTERGMEMAP,
        [ScriptDefName("CHECKSERVER")]
        CHECKSERVER,
        [ScriptDefName("ELARGE")]
        ELARGE,
        [ScriptDefName("ESMALL")]
        ESMALL,
        [ScriptDefName("CHECKGROUPCOUNT")]
        CHECKGROUPCOUNT,
        [ScriptDefName("CHECKACCESSORY")]
        CHECKACCESSORY,
        [ScriptDefName("ONERROR")]
        ONERROR,
        [ScriptDefName("CHECKARMOR")]
        CHECKARMOR,
        [ScriptDefName("CHECKACCOUNTLIST")]
        CHECKACCOUNTLIST,
        [ScriptDefName("CHECKIPLIST")]
        CHECKIPLIST,
        [ScriptDefName("CHECKCREDITPOINT")]
        CHECKCREDITPOINT,
        [ScriptDefName("CHECKPOSEDIR")]
        CHECKPOSEDIR,
        [ScriptDefName("CHECKPOSELEVEL")]
        CHECKPOSELEVEL,
        [ScriptDefName("CHECKPOSEGENDER")]
        CHECKPOSEGENDER,
        [ScriptDefName("CHECKLEVELEX")]
        CHECKLEVELEX,
        [ScriptDefName("CHECKBONUSPOINT")]
        CHECKBONUSPOINT,
        [ScriptDefName("CHECKMARRY")]
        CHECKMARRY,
        [ScriptDefName("CHECKPOSEMARRY")]
        CHECKPOSEMARRY,
        [ScriptDefName("CHECKMARRYCOUNT")]
        CHECKMARRYCOUNT,
        [ScriptDefName("CHECKMASTER")]
        CHECKMASTER,
        [ScriptDefName("HAVEMASTER")]
        HAVEMASTER,
        [ScriptDefName("CHECKPOSEMASTER")]
        CHECKPOSEMASTER,
        [ScriptDefName("POSEHAVEMASTER")]
        POSEHAVEMASTER,
        [ScriptDefName("CHECKPOSEISMASTER")]
        CHECKISMASTER,
        [ScriptDefName("CHECKISMASTER")]
        CHECKPOSEISMASTER,
        [ScriptDefName("CHECKNAMEIPLIST")]
        CHECKNAMEIPLIST,
        [ScriptDefName("CHECKACCOUNTIPLIST")]
        CHECKACCOUNTIPLIST,
        [ScriptDefName("CHECKSLAVECOUNT")]
        CHECKSLAVECOUNT,
        [ScriptDefName("ISCASTLEMASTER")]
        CHECKCASTLEMASTER,
        [ScriptDefName("ISNEWHUMAN")]
        ISNEWHUMAN,
        [ScriptDefName("CHECKMEMBERTYPE")]
        CHECKMEMBERTYPE,
        [ScriptDefName("CHECKMEMBERLEVEL")]
        CHECKMEMBERLEVEL,
        [ScriptDefName("CHECKGAMEGOLD")]
        CHECKGAMEGOLD,
        [ScriptDefName("CHECKGAMEPOINT")]
        CHECKGAMEPOINT,
        [ScriptDefName("CHECKNAMELISTPOSITION")]
        CHECKNAMELISTPOSITION,
        [ScriptDefName("CHECKGUILDLIST")]
        CHECKGUILDLIST,
        [ScriptDefName("CHECKRENEWLEVEL")]
        CHECKRENEWLEVEL,
        [ScriptDefName("CHECKSLAVELEVEL")]
        CHECKSLAVELEVEL,
        [ScriptDefName("CHECKSLAVENAME")]
        CHECKSLAVENAME,
        [ScriptDefName("CHECKOFGUILD")]
        CHECKOFGUILD,
        [ScriptDefName("CHECKPAYMENT")]
        CHECKPAYMENT,
        [ScriptDefName("CHECKUSEITEM")]
        CHECKUSEITEM,
        [ScriptDefName("CHECKBAGSIZE")]
        CHECKBAGSIZE,
        [ScriptDefName("CHECKLISTCOUNT")]
        CHECKLISTCOUNT,
        [ScriptDefName("CHECKDC")]
        CHECKDC,
        [ScriptDefName("CHECKMC")]
        CHECKMC,
        [ScriptDefName("CHECKSC")]
        CHECKSC,
        [ScriptDefName("CHECKHP")]
        CHECKHP,
        [ScriptDefName("CHECKMP")]
        CHECKMP,
        [ScriptDefName("CHECKITEMTYPE")]
        CHECKITEMTYPE,
        [ScriptDefName("CHECKEXP")]
        CHECKEXP,
        [ScriptDefName("CHECKCASTLEGOLD")]
        CHECKCASTLEGOLD,
        [ScriptDefName("PASSWORDERRORCOUNT")]
        PASSWORDERRORCOUNT,
        [ScriptDefName("ISLOCKPASSWORD")]
        ISLOCKPASSWORD,
        [ScriptDefName("ISLOCKSTORAGE")]
        ISLOCKSTORAGE,
        [ScriptDefName("CHECKGUILDBUILDPOINT")]
        CHECKBUILDPOINT,
        [ScriptDefName("CHECKGUILDAURAEPOINT")]
        CHECKAURAEPOINT,
        [ScriptDefName("CHECKGUILDSTABILITYPOINT")]
        CHECKSTABILITYPOINT,
        [ScriptDefName("CHECKGUILDFLOURISHPOINT")]
        CHECKFLOURISHPOINT,
        /// <summary>
        /// 贡献度
        /// </summary>
        [ScriptDefName("CHECKCONTRIBUTION")]
        CHECKCONTRIBUTION,
        /// <summary>
        /// 检查一个区域中有多少怪
        /// </summary>
        [ScriptDefName("CHECKRANGEMONCOUNT")]
        CHECKRANGEMONCOUNT,
        [ScriptDefName("CHECKITEMADDVALUE")]
        CHECKITEMADDVALUE,
        [ScriptDefName("CHECKINMAPRANGE")]
        CHECKINMAPRANGE,
        [ScriptDefName("CASTLECHANGEDAY")]
        CASTLECHANGEDAY,
        [ScriptDefName("CASTLEWARAY")]
        CASTLEWARDAY,
        [ScriptDefName("ONLINELONGMIN")]
        ONLINELONGMIN,
        [ScriptDefName("CHECKGUILDCHIEFITEMCOUNT")]
        CHECKGUILDCHIEFITEMCOUNT,
        [ScriptDefName("CHECKNAMEDATELIST")]
        CHECKNAMEDATELIST,
        [ScriptDefName("CHECKMAPHUMANCOUNT")]
        CHECKMAPHUMANCOUNT,
        [ScriptDefName("CHECKMAPMONCOUNT")]
        CHECKMAPMONCOUNT,
        [ScriptDefName("CHECKVAR")]
        CHECKVAR,
        [ScriptDefName("CHECKSERVERNAME")]
        CHECKSERVERNAME,
        [ScriptDefName("CHECKMAPNAME")]
        CHECKMAPNAME,
        [ScriptDefName("INSAFEZONE")]
        INSAFEZONE,
        [ScriptDefName("CHECKSKILL")]
        CHECKSKILL,
        [ScriptDefName("CHECKUSERDATE")]
        CHECKUSERDATE,
        [ScriptDefName("CHECKCONTAINSTEXT")]
        CHECKCONTAINSTEXT,
        [ScriptDefName("COMPARETEXT")]
        COMPARETEXT,
        [ScriptDefName("CHECKTEXTLIST")]
        CHECKTEXTLIST,
        [ScriptDefName("ISGROUPMASTER")]
        ISGROUPMASTER,
        [ScriptDefName("CHECKCONTAINSTEXTLIST")]
        CHECKCONTAINSTEXTLIST,
        [ScriptDefName("CHECKONLINE")]
        CHECKONLINE,
        [ScriptDefName("CHECKTEXTLENGTH")]
        CHECKTEXTLENGTH,
        [ScriptDefName("ISDUPMODE")]
        ISDUPMODE,
        [ScriptDefName("ISOFFLINEMODE")]
        ISOFFLINEMODE,
        [ScriptDefName("CHECKSTATIONTIME")]
        CHECKSTATIONTIME,
        [ScriptDefName("CHECKSIGNMAP")]
        CHECKSIGNMAP,
        [ScriptDefName("CHECKGUILDMEMBERMAXLIMIT")]
        CHECKGUILDMEMBERMAXLIMIT,
        [ScriptDefName("CHECKGUILDNAMEDATELIST")]
        CHECKGUILDNAMEDATELIST,
        [ScriptDefName("CHECKRANGEROUPCOUNT")]
        CHECKRANGEROUPCOUNT,
        [ScriptDefName("CHECKONLINEPLAYCOUNT")]
        CHECKONLINEPLAYCOUNT,
        [ScriptDefName("CHECKITEMLIMIT")]
        CHECKITEMLIMIT,
        [ScriptDefName("CHECKITEMLIMITCOUNT")]
        CHECKITEMLIMITCOUNT,
        [ScriptDefName("CHECKMEMORYITEM")]
        CHECKMEMORYITEM,
        [ScriptDefName("CHECKUPGRADEITEMS")]
        CHECKUPGRADEITEMS,
        [ScriptDefName("CHECKIPUTTEM")]
        CHECKIPUTTEM,
        [ScriptDefName("CHECKDEATH")]
        CHECKDEATH,
        [ScriptDefName("ISUNDERWAR")]
        ISUNDERWAR,
        [ScriptDefName("CHECKPKPOINTEX")]
        CHECKPKPOINTEX,
        [ScriptDefName("CHECKUSEITEMBIND")]
        CHECKUSEITEMBIND,
        [ScriptDefName("CHECKITEMBIND")]
        CHECKITEMBIND,
        [ScriptDefName("CHECKITEMNEWADDVALUE")]
        CHECKITEMNEWADDVALUE,
        [ScriptDefName("ISSPACELOCK")]
        ISSPACELOCK,
        [ScriptDefName("CHECKRANGEMAPMAGICEVENTCOUNT")]
        CHECKRANGEMAPMAGICEVENTCOUNT,
        [ScriptDefName("CHECKITEMNEWADDVALUECOUNT")]
        CHECKITEMNEWADDVALUECOUNT,
        [ScriptDefName("GETDUELMAP")]
        GETDUELMAP,
        [ScriptDefName("CHECKMAPDUELING")]
        CHECKMAPDUELING,
        [ScriptDefName("CHECKHUMDUELING")]
        CHECKHUMDUELING,
        [ScriptDefName("CHECKCANUSEITEM")]
        CHECKCANUSEITEM,
        [ScriptDefName("CHECKINCURRRECT")]
        CHECKINCURRRECT,
        [ScriptDefName("CHECKGUILDMEMBER")]
        CHECKGUILDMEMBER,
        [ScriptDefName("INDEXOF")]
        INDEXOF,
        [ScriptDefName("CHECKMASKED")]
        CHECKMASKED,
        [ScriptDefName("CHECKUSEITEMSTARSLEVEL")]
        CHECKUSEITEMSTARSLEVEL,
        [ScriptDefName("CHECKBAGITEMSTARSLEVEL")]
        CHECKBAGITEMSTARSLEVEL,
        [ScriptDefName("CHECKHEROGROUP")]
        CHECKHEROGROUP,
        [ScriptDefName("CHECKPUTITEMTYPE")]
        CHECKPUTITEMTYPE,
        [ScriptDefName("CHECKSLAVERANGE")]
        CHECKSLAVERANGE,
        [ScriptDefName("ISAI")]
        ISAI,
        [ScriptDefName("CHECKBAGITEMINLIST")]
        CHECKBAGITEMINLIST,
        [ScriptDefName("INCASTLEWARAREA")]
        INCASTLEWARAREA,
        /// <summary>
        /// 检测地图命令
        /// </summary>
        [ScriptDefName("ISONMAP")]
        ISONMAP,
        /// <summary>
        // 检测当前人是否在MAP地图上
        /// </summary>
        [ScriptDefName("CHECKISONMAP")]
        CHECKISONMAP,
        [ScriptDefName("REVIVESLAVES")]
        REVIVESLAVE,
        [ScriptDefName("CHECKMAGICLVL")]
        CHECKMAGICLVL,
        [ScriptDefName("CHECKGROUPCLASS")]
        CHECKGROUPCLASS,
        [ScriptDefName("ISHIGH")]
        ISHIGH,
        /// <summary>
        /// 检查人物死亡被指定怪物杀死
        /// </summary>
        [ScriptDefName("CHECKDIEMON")]
        CHECKDIEMON,
        /// <summary>
        /// 检查杀死怪物
        /// </summary>
        [ScriptDefName("CHECKKILLPLAYMON")]
        CHECKKILLPLAYMON,
        /// <summary>
        /// 检测输入的验证码是否正确
        /// </summary>
        [ScriptDefName("CHECKRANDOMNO")]
        CHECKRANDOMNO,
        /// <summary>
        /// 是否被人杀
        /// </summary>
        [ScriptDefName("KILLBYHUM")]
        KILLBYHUM,
        /// <summary>
        /// 是否被怪杀
        /// </summary>
        [ScriptDefName("KILLBYMON")]
        KILLBYMON,
        /// <summary>
        /// 检测人物是否在安全区
        /// </summary>
        [ScriptDefName("CHECKINSAFEZONE")]
        CHECKINSAFEZONE,
        /// <summary>
        /// 检查放入装备指定的属性点
        /// </summary>
        [ScriptDefName("CHECKDLGITEMADDVALUE")]
        CHECKDLGITEMADDVALUE,
        /// <summary>
        /// 检查放入装备的类型
        /// </summary>
        [ScriptDefName("CHECKDLGITEMTYPE")]
        CHECKDLGITEMTYPE,
        /// <summary>
        /// 检查放入装备名称特征字符
        /// </summary>
        [ScriptDefName("CHECKDLGITEMNAME")]
        CHECKDLGITEMNAME,
        /// <summary>
        /// 检查已杀死怪物
        /// </summary>
        [ScriptDefName("CHECKDEATHPLAYMON")]
        SCHECKDEATHPLAYMON,
        /// <summary>
        /// 检测人物杀死怪物的名字
        /// </summary>
        [ScriptDefName("CHECKKILLMONNAME")]
        CHECKKILLMONNAME,
        /// <summary>
        /// 检测地图地图
        /// </summary>
        [ScriptDefName("CHECKMAP")]
        CHECKMAP
    }
}