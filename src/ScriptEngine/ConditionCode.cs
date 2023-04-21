using GameSrv.Script;

namespace GameSrv
{
    /// <summary>
    /// 条件检测编码定义
    /// </summary>
    public enum ConditionCode : short
    {
        [ScriptCode("CHECK")]
        CHECK,
        [ScriptCode("RANDOM")]
        RANDOM,
        [ScriptCode("GENDER")]
        GENDER,
        [ScriptCode("DAYTIME")]
        DAYTIME,
        [ScriptCode("CHECKOPEN")]
        CHECKOPEN,
        [ScriptCode("CHECKUNIT")]
        CHECKUNIT,
        [ScriptCode("CHECKLEVEL")]
        CHECKLEVEL,
        [ScriptCode("CHECKJOB")]
        CHECKJOB,
        [ScriptCode("CHECKBBCOUNT")]
        CHECKBBCOUNT,
        [ScriptCode("CHECKITEM")]
        CHECKITEM,
        [ScriptCode("CHECKITEMW")]
        CHECKITEMW,
        [ScriptCode("CHECKGOLD")]
        CHECKGOLD,
        [ScriptCode("ISTAKEITEM")]
        ISTAKEITEM,
        [ScriptCode("CHECKDURA")]
        CHECKDURA,
        [ScriptCode("CHECKDURAEVA")]
        CHECKDURAEVA,
        [ScriptCode("DAYOFWEEK")]
        DAYOFWEEK,
        [ScriptCode("HOUR")]
        HOUR,
        [ScriptCode("MIN")]
        MIN,
        [ScriptCode("CHECKPKPOINT")]
        CHECKPKPOINT,
        [ScriptCode("CHECKLUCKYPOINT")]
        CHECKLUCKYPOINT,
        [ScriptCode("CHECKMONMAP")]
        CHECKMONMAP,
        [ScriptCode("CHECKMONAREA")]
        CHECKMONAREA,
        [ScriptCode("CHECKHUM")]
        CHECKHUM,
        [ScriptCode("CHECKBAGGAGE")]
        CHECKBAGGAGE,
        [ScriptCode("EQUAL")]
        EQUAL,
        [ScriptCode("LARGE")]
        LARGE,
        [ScriptCode("SMALL")]
        SMALL,
        [ScriptCode("CHECKMAGIC")]
        CHECKMAGIC,
        [ScriptCode("CHKMAGICLEVEL")]
        CHKMAGICLEVEL,
        [ScriptCode("CHECKMONRECALL")]
        CHECKMONRECALL,
        [ScriptCode("CHECKHORSE")]
        CHECKHORSE,
        [ScriptCode("CHECKRIDING")]
        CHECKRIDING,
        [ScriptCode("STARTDAILYQUEST")]
        STARTDAILYQUEST,
        [ScriptCode("CHECKDAILYQUEST")]
        CHECKDAILYQUEST,
        [ScriptCode("RANDOMEX")]
        RANDOMEX,
        [ScriptCode("CHECKNAMELIST")]
        CHECKNAMELIST,
        [ScriptCode("CHECKWEAPONLEVEL")]
        CHECKWEAPONLEVEL,
        [ScriptCode("CHECKWEAPONATOM")]
        CHECKWEAPONATOM,
        [ScriptCode("CHECKREFINEWEAPON")]
        CHECKREFINEWEAPON,
        [ScriptCode("CHECKWEAPONMCTYPE")]
        CHECKWEAPONMCTYPE,
        [ScriptCode("CHECKREFINEITEM")]
        CHECKREFINEITEM,
        [ScriptCode("HASWEAPONATOM")]
        HASWEAPONATOM,
        [ScriptCode("ISGUILDMASTER")]
        ISGUILDMASTER,
        [ScriptCode("CANPROPOSECASTLEWAR")]
        CANPROPOSECASTLEWAR,
        [ScriptCode("CANHAVESHOOTER")]
        CANHAVESHOOTER,
        [ScriptCode("CHECKFAME")]
        CHECKFAME,
        [ScriptCode("ISONCASTLEWAR")]
        ISONCASTLEWAR,
        [ScriptCode("ISONREADYCASTLEWAR")]
        ISONREADYCASTLEWAR,
        [ScriptCode("ISCASTLEGUILD")]
        ISCASTLEGUILD,
        /// <summary>
        /// 是否为攻城方
        /// </summary>
        [ScriptCode("ISATTACKGUILD")]
        ISATTACKGUILD,
        /// <summary>
        /// 是否为守城方
        /// </summary>
        [ScriptCode("ISDEFENSEGUILD")]
        ISDEFENSEGUILD,
        [ScriptCode("CHECKSHOOTER")]
        CHECKSHOOTER,
        [ScriptCode("CHECKSAVEDSHOOTER")]
        CHECKSAVEDSHOOTER,
        /// <summary>
        /// 是否加入行会
        /// </summary>
        [ScriptCode("HAVEGUILD")]
        HASGUILD,
        /// <summary>
        /// 检查城门
        /// </summary>
        [ScriptCode("CHECKCASTLEDOOR")]
        CHECKCASTLEDOOR,
        /// <summary>
        /// 城门是否打开
        /// </summary>
        [ScriptCode("CHECKCASTLEDOOROPEN")]
        CHECKCASTLEDOOROPEN,
        [ScriptCode("CHECKPOS")]
        CHECKPOS,
        [ScriptCode("CANCHARGESHOOTER")]
        CANCHARGESHOOTER,
        /// <summary>
        /// 是否为攻城方联盟行会
        /// </summary>
        [ScriptCode("ISATTACKALLYGUILD")]
        ISATTACKALLYGUILD,
        /// <summary>
        /// 是否为守城方联盟行会
        /// </summary>
        [ScriptCode("ISDEFENSEALLYGUILD")]
        ISDEFENSEALLYGUILD,
        [ScriptCode("TESTTEAM")]
        TESTTEAM,
        [ScriptCode("ISSYSOP")]
        ISSYSOP,
        [ScriptCode("ISADMIN")]
        ISADMIN,
        [ScriptCode("CHECKBONUS")]
        CHECKBONUS,
        [ScriptCode("CHECKMARRIAGE")]
        CHECKMARRIAGE,
        [ScriptCode("CHECKMARRIAGERING")]
        CHECKMARRIAGERING,
        [ScriptCode("CHECKGMETERM")]
        CHECKGMETERM,
        [ScriptCode("CHECKOPENGME")]
        CHECKOPENGME,
        [ScriptCode("CHECKENTERGMEMAP")]
        CHECKENTERGMEMAP,
        [ScriptCode("CHECKSERVER")]
        CHECKSERVER,
        [ScriptCode("ELARGE")]
        ELARGE,
        [ScriptCode("ESMALL")]
        ESMALL,
        [ScriptCode("CHECKGROUPCOUNT")]
        CHECKGROUPCOUNT,
        [ScriptCode("CHECKACCESSORY")]
        CHECKACCESSORY,
        [ScriptCode("ONERROR")]
        ONERROR,
        [ScriptCode("CHECKARMOR")]
        CHECKARMOR,
        [ScriptCode("CHECKACCOUNTLIST")]
        CHECKACCOUNTLIST,
        [ScriptCode("CHECKIPLIST")]
        CHECKIPLIST,
        [ScriptCode("CHECKCREDITPOINT")]
        CHECKCREDITPOINT,
        [ScriptCode("CHECKPOSEDIR")]
        CHECKPOSEDIR,
        [ScriptCode("CHECKPOSELEVEL")]
        CHECKPOSELEVEL,
        [ScriptCode("CHECKPOSEGENDER")]
        CHECKPOSEGENDER,
        [ScriptCode("CHECKLEVELEX")]
        CHECKLEVELEX,
        [ScriptCode("CHECKBONUSPOINT")]
        CHECKBONUSPOINT,
        [ScriptCode("CHECKMARRY")]
        CHECKMARRY,
        [ScriptCode("CHECKPOSEMARRY")]
        CHECKPOSEMARRY,
        [ScriptCode("CHECKMARRYCOUNT")]
        CHECKMARRYCOUNT,
        [ScriptCode("CHECKMASTER")]
        CHECKMASTER,
        [ScriptCode("HAVEMASTER")]
        HAVEMASTER,
        [ScriptCode("CHECKPOSEMASTER")]
        CHECKPOSEMASTER,
        [ScriptCode("POSEHAVEMASTER")]
        POSEHAVEMASTER,
        [ScriptCode("CHECKPOSEISMASTER")]
        CHECKISMASTER,
        [ScriptCode("CHECKISMASTER")]
        CHECKPOSEISMASTER,
        [ScriptCode("CHECKNAMEIPLIST")]
        CHECKNAMEIPLIST,
        [ScriptCode("CHECKACCOUNTIPLIST")]
        CHECKACCOUNTIPLIST,
        [ScriptCode("CHECKSLAVECOUNT")]
        CHECKSLAVECOUNT,
        [ScriptCode("ISCASTLEMASTER")]
        CHECKCASTLEMASTER,
        [ScriptCode("ISNEWHUMAN")]
        ISNEWHUMAN,
        [ScriptCode("CHECKMEMBERTYPE")]
        CHECKMEMBERTYPE,
        [ScriptCode("CHECKMEMBERLEVEL")]
        CHECKMEMBERLEVEL,
        [ScriptCode("CHECKGAMEGOLD")]
        CHECKGAMEGOLD,
        [ScriptCode("CHECKGAMEPOINT")]
        CHECKGAMEPOINT,
        [ScriptCode("CHECKNAMELISTPOSITION")]
        CHECKNAMELISTPOSITION,
        [ScriptCode("CHECKGUILDLIST")]
        CHECKGUILDLIST,
        [ScriptCode("CHECKRENEWLEVEL")]
        CHECKRENEWLEVEL,
        [ScriptCode("CHECKSLAVELEVEL")]
        CHECKSLAVELEVEL,
        [ScriptCode("CHECKSLAVENAME")]
        CHECKSLAVENAME,
        [ScriptCode("CHECKOFGUILD")]
        CHECKOFGUILD,
        [ScriptCode("CHECKPAYMENT")]
        CHECKPAYMENT,
        [ScriptCode("CHECKUSEITEM")]
        CHECKUSEITEM,
        [ScriptCode("CHECKBAGSIZE")]
        CHECKBAGSIZE,
        [ScriptCode("CHECKLISTCOUNT")]
        CHECKLISTCOUNT,
        [ScriptCode("CHECKDC")]
        CHECKDC,
        [ScriptCode("CHECKMC")]
        CHECKMC,
        [ScriptCode("CHECKSC")]
        CHECKSC,
        [ScriptCode("CHECKHP")]
        CHECKHP,
        [ScriptCode("CHECKMP")]
        CHECKMP,
        [ScriptCode("CHECKITEMTYPE")]
        CHECKITEMTYPE,
        [ScriptCode("CHECKEXP")]
        CHECKEXP,
        [ScriptCode("CHECKCASTLEGOLD")]
        CHECKCASTLEGOLD,
        [ScriptCode("PASSWORDERRORCOUNT")]
        PASSWORDERRORCOUNT,
        [ScriptCode("ISLOCKPASSWORD")]
        ISLOCKPASSWORD,
        [ScriptCode("ISLOCKSTORAGE")]
        ISLOCKSTORAGE,
        [ScriptCode("CHECKGUILDBUILDPOINT")]
        CHECKBUILDPOINT,
        [ScriptCode("CHECKGUILDAURAEPOINT")]
        CHECKAURAEPOINT,
        [ScriptCode("CHECKGUILDSTABILITYPOINT")]
        CHECKSTABILITYPOINT,
        [ScriptCode("CHECKGUILDFLOURISHPOINT")]
        CHECKFLOURISHPOINT,
        /// <summary>
        /// 贡献度
        /// </summary>
        [ScriptCode("CHECKCONTRIBUTION")]
        CHECKCONTRIBUTION,
        /// <summary>
        /// 检查一个区域中有多少怪
        /// </summary>
        [ScriptCode("CHECKRANGEMONCOUNT")]
        CHECKRANGEMONCOUNT,
        [ScriptCode("CHECKITEMADDVALUE")]
        CHECKITEMADDVALUE,
        [ScriptCode("CHECKINMAPRANGE")]
        CHECKINMAPRANGE,
        [ScriptCode("CASTLECHANGEDAY")]
        CASTLECHANGEDAY,
        [ScriptCode("CASTLEWARAY")]
        CASTLEWARDAY,
        [ScriptCode("ONLINELONGMIN")]
        ONLINELONGMIN,
        [ScriptCode("CHECKGUILDCHIEFITEMCOUNT")]
        CHECKGUILDCHIEFITEMCOUNT,
        [ScriptCode("CHECKNAMEDATELIST")]
        CHECKNAMEDATELIST,
        [ScriptCode("CHECKMAPHUMANCOUNT")]
        CHECKMAPHUMANCOUNT,
        [ScriptCode("CHECKMAPMONCOUNT")]
        CHECKMAPMONCOUNT,
        [ScriptCode("CHECKVAR")]
        CHECKVAR,
        [ScriptCode("CHECKSERVERNAME")]
        CHECKSERVERNAME,
        [ScriptCode("CHECKMAPNAME")]
        CHECKMAPNAME,
        [ScriptCode("INSAFEZONE")]
        INSAFEZONE,
        [ScriptCode("CHECKSKILL")]
        CHECKSKILL,
        [ScriptCode("CHECKUSERDATE")]
        CHECKUSERDATE,
        [ScriptCode("CHECKCONTAINSTEXT")]
        CHECKCONTAINSTEXT,
        [ScriptCode("COMPARETEXT")]
        COMPARETEXT,
        [ScriptCode("CHECKTEXTLIST")]
        CHECKTEXTLIST,
        [ScriptCode("ISGROUPMASTER")]
        ISGROUPMASTER,
        [ScriptCode("CHECKCONTAINSTEXTLIST")]
        CHECKCONTAINSTEXTLIST,
        [ScriptCode("CHECKONLINE")]
        CHECKONLINE,
        [ScriptCode("CHECKTEXTLENGTH")]
        CHECKTEXTLENGTH,
        [ScriptCode("ISDUPMODE")]
        ISDUPMODE,
        [ScriptCode("ISOFFLINEMODE")]
        ISOFFLINEMODE,
        [ScriptCode("CHECKSTATIONTIME")]
        CHECKSTATIONTIME,
        [ScriptCode("CHECKSIGNMAP")]
        CHECKSIGNMAP,
        [ScriptCode("CHECKGUILDMEMBERMAXLIMIT")]
        CHECKGUILDMEMBERMAXLIMIT,
        [ScriptCode("CHECKGUILDNAMEDATELIST")]
        CHECKGUILDNAMEDATELIST,
        [ScriptCode("CHECKRANGEROUPCOUNT")]
        CHECKRANGEROUPCOUNT,
        [ScriptCode("CHECKONLINEPLAYCOUNT")]
        CHECKONLINEPLAYCOUNT,
        [ScriptCode("CHECKITEMLIMIT")]
        CHECKITEMLIMIT,
        [ScriptCode("CHECKITEMLIMITCOUNT")]
        CHECKITEMLIMITCOUNT,
        [ScriptCode("CHECKMEMORYITEM")]
        CHECKMEMORYITEM,
        [ScriptCode("CHECKUPGRADEITEMS")]
        CHECKUPGRADEITEMS,
        [ScriptCode("CHECKIPUTTEM")]
        CHECKIPUTTEM,
        [ScriptCode("CHECKDEATH")]
        CHECKDEATH,
        [ScriptCode("ISUNDERWAR")]
        ISUNDERWAR,
        [ScriptCode("CHECKPKPOINTEX")]
        CHECKPKPOINTEX,
        [ScriptCode("CHECKUSEITEMBIND")]
        CHECKUSEITEMBIND,
        [ScriptCode("CHECKITEMBIND")]
        CHECKITEMBIND,
        [ScriptCode("CHECKITEMNEWADDVALUE")]
        CHECKITEMNEWADDVALUE,
        [ScriptCode("ISSPACELOCK")]
        ISSPACELOCK,
        [ScriptCode("CHECKRANGEMAPMAGICEVENTCOUNT")]
        CHECKRANGEMAPMAGICEVENTCOUNT,
        [ScriptCode("CHECKITEMNEWADDVALUECOUNT")]
        CHECKITEMNEWADDVALUECOUNT,
        [ScriptCode("GETDUELMAP")]
        GETDUELMAP,
        [ScriptCode("CHECKMAPDUELING")]
        CHECKMAPDUELING,
        [ScriptCode("CHECKHUMDUELING")]
        CHECKHUMDUELING,
        [ScriptCode("CHECKCANUSEITEM")]
        CHECKCANUSEITEM,
        [ScriptCode("CHECKINCURRRECT")]
        CHECKINCURRRECT,
        [ScriptCode("CHECKGUILDMEMBER")]
        CHECKGUILDMEMBER,
        [ScriptCode("INDEXOF")]
        INDEXOF,
        [ScriptCode("CHECKMASKED")]
        CHECKMASKED,
        [ScriptCode("CHECKUSEITEMSTARSLEVEL")]
        CHECKUSEITEMSTARSLEVEL,
        [ScriptCode("CHECKBAGITEMSTARSLEVEL")]
        CHECKBAGITEMSTARSLEVEL,
        [ScriptCode("CHECKHEROGROUP")]
        CHECKHEROGROUP,
        [ScriptCode("CHECKPUTITEMTYPE")]
        CHECKPUTITEMTYPE,
        [ScriptCode("CHECKSLAVERANGE")]
        CHECKSLAVERANGE,
        [ScriptCode("ISAI")]
        ISAI,
        [ScriptCode("CHECKBAGITEMINLIST")]
        CHECKBAGITEMINLIST,
        [ScriptCode("INCASTLEWARAREA")]
        INCASTLEWARAREA,
        /// <summary>
        /// 检测地图命令
        /// </summary>
        [ScriptCode("ISONMAP")]
        ISONMAP,
        /// <summary>
        // 检测当前人是否在MAP地图上
        /// </summary>
        [ScriptCode("CHECKISONMAP")]
        CHECKISONMAP,
        [ScriptCode("REVIVESLAVES")]
        REVIVESLAVE,
        [ScriptCode("CHECKMAGICLVL")]
        CHECKMAGICLVL,
        [ScriptCode("CHECKGROUPCLASS")]
        CHECKGROUPCLASS,
        [ScriptCode("ISHIGH")]
        ISHIGH,
        /// <summary>
        /// 检查人物死亡被指定怪物杀死
        /// </summary>
        [ScriptCode("CHECKDIEMON")]
        CHECKDIEMON,
        /// <summary>
        /// 检查杀死怪物
        /// </summary>
        [ScriptCode("CHECKKILLPLAYMON")]
        CHECKKILLPLAYMON,
        /// <summary>
        /// 检测输入的验证码是否正确
        /// </summary>
        [ScriptCode("CHECKRANDOMNO")]
        CHECKRANDOMNO,
        /// <summary>
        /// 是否被人杀
        /// </summary>
        [ScriptCode("KILLBYHUM")]
        KILLBYHUM,
        /// <summary>
        /// 是否被怪杀
        /// </summary>
        [ScriptCode("KILLBYMON")]
        KILLBYMON,
        /// <summary>
        /// 检测人物是否在安全区
        /// </summary>
        [ScriptCode("CHECKINSAFEZONE")]
        CHECKINSAFEZONE,
        /// <summary>
        /// 检查放入装备指定的属性点
        /// </summary>
        [ScriptCode("CHECKDLGITEMADDVALUE")]
        CHECKDLGITEMADDVALUE,
        /// <summary>
        /// 检查放入装备的类型
        /// </summary>
        [ScriptCode("CHECKDLGITEMTYPE")]
        CHECKDLGITEMTYPE,
        /// <summary>
        /// 检查放入装备名称特征字符
        /// </summary>
        [ScriptCode("CHECKDLGITEMNAME")]
        CHECKDLGITEMNAME,
        /// <summary>
        /// 检查已杀死怪物
        /// </summary>
        [ScriptCode("CHECKDEATHPLAYMON")]
        SCHECKDEATHPLAYMON,
        [ScriptCode("CHECKKILLMONNAME")]
        CHECKKILLMONNAME,
        [ScriptCode("CHECKMAP")]
        CHECKMAP
    }
}