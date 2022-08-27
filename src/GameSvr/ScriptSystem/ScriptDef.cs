namespace GameSvr.ScriptSystem
{
    public class SctiptDef
    {
        /// <summary>
        /// 人物下线触发
        /// </summary>
        public const string SPLAYOFFLINE = "@PLAYOFFLINE";

        /// <summary>
        /// 人物下线触发编号
        /// </summary>
        public const int NPLAYOFFLINE = 0;

        public const string SMARRYERROR = "@MARRYERROR";
        public const int NMARRYERROR = 3;
        public const string SMASTERERROR = "@MASTERERROR";
        public const int NMASTERERROR = 4;
        public const string SMARRYCHECKDIR = "@MARRYCHECKDIR";
        public const int NMARRYCHECKDIR = 5;
        public const string SHUMANTYPEERR = "@HUMANTYPEERR";
        public const int NHUMANTYPEERR = 6;
        public const string SSTARTMARRY = "@STARTMARRY";
        public const int NSTARTMARRY = 7;
        public const string SMARRYSEXERR = "@MARRYSEXERR";
        public const int NMARRYSEXERR = 8;
        public const string SMARRYDIRERR = "@MARRYDIRERR";
        public const int NMARRYDIRERR = 9;
        public const string SWATEMARRY = "@WATEMARRY";
        public const int NWATEMARRY = 10;
        public const string SREVMARRY = "@REVMARRY";
        public const int NREVMARRY = 11;
        public const string SENDMARRY = "@ENDMARRY";
        public const int NENDMARRY = 12;
        public const string SENDMARRYFAIL = "@ENDMARRYFAIL";
        public const int NENDMARRYFAIL = 13;
        public const string SMASTERCHECKDIR = "@MASTERCHECKDIR";
        public const int NMASTERCHECKDIR = 14;
        public const string SSTARTGETMASTER = "@STARTGETMASTER";
        public const int NSTARTGETMASTER = 15;
        public const string SMASTERDIRERR = "@MASTERDIRERR";
        public const int NMASTERDIRERR = 16;
        public const string SWATEMASTER = "@WATEMASTER";
        public const int NWATEMASTER = 17;
        public const string SREVMASTER = "@REVMASTER";
        public const int NREVMASTER = 18;
        public const string SENDMASTER = "@ENDMASTER";
        public const int NENDMASTER = 19;
        public const string SSTARTMASTER = "@STARTMASTER";
        public const int NSTARTMASTER = 20;
        public const string SENDMASTERFAIL = "@ENDMASTERFAIL";
        public const int NENDMASTERFAIL = 21;
        public const string SEXEMARRYFAIL = "@EXEMARRYFAIL";
        public const int NEXEMARRYFAIL = 22;
        public const string SUNMARRYCHECKDIR = "@UNMARRYCHECKDIR";
        public const int NUNMARRYCHECKDIR = 23;
        public const string SUNMARRYTYPEERR = "@UNMARRYTYPEERR";
        public const int NUNMARRYTYPEERR = 24;
        public const string SSTARTUNMARRY = "@STARTUNMARRY";
        public const int NSTARTUNMARRY = 25;
        public const string SUNMARRYEND = "@UNMARRYEND";
        public const int NUNMARRYEND = 26;
        public const string SWATEUNMARRY = "@WATEUNMARRY";
        public const int NWATEUNMARRY = 27;
        public const string SEXEMASTERFAIL = "@EXEMASTERFAIL";
        public const int NEXEMASTERFAIL = 28;
        public const string SUNMASTERCHECKDIR = "@UNMASTERCHECKDIR";
        public const int NUNMASTERCHECKDIR = 29;
        public const string SUNMASTERTYPEERR = "@UNMASTERTYPEERR";
        public const int NUNMASTERTYPEERR = 30;
        public const string SUNISMASTER = "@UNISMASTER";
        public const int NUNISMASTER = 31;
        public const string SUNMASTERERROR = "@UNMASTERERROR";
        public const int NUNMASTERERROR = 32;
        public const string SSTARTUNMASTER = "@STARTUNMASTER";
        public const int NSTARTUNMASTER = 33;
        public const string SWATEUNMASTER = "@WATEUNMASTER";
        public const int NWATEUNMASTER = 34;
        public const string SUNMASTEREND = "@UNMASTEREND";
        public const int NUNMASTEREND = 35;
        public const string SREVUNMASTER = "@REVUNMASTER";
        public const int NREVUNMASTER = 36;

        /// <summary>
        /// 请求攻城成功
        /// </summary>
        public const string SSUPREQUEST_OK = "~@REQUEST_OK";

        /// <summary>
        /// 请求攻城成功编号
        /// </summary>
        public const int NSUPREQUEST_OK = 37;

        public const string SMEMBER = "@MEMBER";
        public const int NMEMBER = 38;

        /// <summary>
        /// 人物小退触发
        /// </summary>
        public const string SPLAYRECONNECTION = "@PLAYRECONNECTION";

        /// <summary>
        /// 人物小退触发编号
        /// </summary>
        public const int NPLAYRECONNECTION = 39;

        public const string SLOGIN = "@LOGIN";
        public const int NLOGIN = 40;
        public const string SPLAYDIE = "@PLAYDIE";
        public const int NPLAYDIE = 41;

        /// <summary>
        /// 杀人触发标签
        /// </summary>
        public const string SKILLPLAY = "@KILLPLAY";

        /// <summary>
        /// 杀人触发编号
        /// </summary>
        public const int NKILLPLAY = 42;

        /// <summary>
        /// 升级触发标签
        /// </summary>
        public const string SPLAYLEVELUP = "@PLAYLEVELUP";

        /// <summary>
        /// 升级触发编号
        /// </summary>
        public const int NPLAYLEVELUP = 43;

        public const string SSTDMODEFUNC = "@STDMODEFUNC";
        public const int NSTDMODEFUNC = 44;
        public const string SPLAYLEVELUPEX = "@PLAYLEVELUPEX";
        public const int NPLAYLEVELUPEX = 45;
        public const string SKILLMONSTER = "@KILLMONSTER";
        public const int NKILLMONSTER = 46;
        public const string SUSERCMD = "@USERCMD";
        public const int NUSERCMD = 47;
        public const string SCREATEECTYPE_IN = "@CREATEECTYPE_IN";
        public const int NCREATEECTYPE_IN = 48;
        public const string SCREATEECTYPE_OK = "@CREATEECTYPE_OK";
        public const int NCREATEECTYPE_OK = 49;
        public const string SCREATEECTYPE_FAIL = "@CREATEECTYPE_FAIL";
        public const int NCREATEECTYPE_FAIL = 50;
        public const string SCLEARMISSION = "@CLEARMISSION";
        public const int NCLEARMISSION = 51;
        public const string SRESUME = "@RESUME";
        public const int NRESUME = 52;
        public const string SGETLARGESSGOLD_OK = "@GETLARGESSGOLD_OK";
        public const int NGETLARGESSGOLD_OK = 53;
        public const string SGETLARGESSGOLD_FAIL = "@GETLARGESSGOLD_FAIL";
        public const int NGETLARGESSGOLD_FAIL = 54;
        public const string SGETLARGESSGOLD_ERROR = "@GETLARGESSGOLD_ERROR";
        public const int NGETLARGESSGOLD_ERROR = 55;
        public const string SMASTERISPRENTICE = "@MASTERISPRENTICE";
        public const int NMASTERISPRENTICE = 56;
        public const string SMASTERISFULL = "@MASTERISFULL";
        public const int NMASTERISFULL = 57;
        public const string SGROUPCREATE = "@GROUPCREATE";
        public const int NGROUPCREATE = 58;
        public const string SSTARTGROUP = "@STARTGROUP";
        public const int NSTARTGROUP = 59;
        public const string SJOINGROUP = "@JOINGROUP";
        public const int NJOINGROUP = 60;
        public const string SSPEEDCLOSE = "@SPEEDCLOSE";
        public const int NSPEEDCLOSE = 61;

        /// <summary>
        /// 武器升级成功
        /// </summary>
        public const string SUPGRADENOW_OK = "~@UPGRADENOW_OK";

        /// <summary>
        /// 武器升级成功编号
        /// </summary>
        public const int NUPGRADENOW_OK = 62;

        /// <summary>
        /// 武器正在升级
        /// </summary>
        public const string SUPGRADENOW_ING = "~@UPGRADENOW_ING";

        /// <summary>
        /// 武器正在升级编号
        /// </summary>
        public const int NUPGRADENOW_ING = 63;

        /// <summary>
        /// 武器升级失败
        /// </summary>
        public const string SUPGRADENOW_FAIL = "~@UPGRADENOW_FAIL";

        /// <summary>
        /// 武器升级失败编号
        /// </summary>
        public const int NUPGRADENOW_FAIL = 64;

        /// <summary>
        /// 取回武器升级成功
        /// </summary>
        public const string SGETBACKUPGNOW_OK = "~@GETBACKUPGNOW_OK";

        /// <summary>
        /// 取回武器升级成功编号
        /// </summary>
        public const int NGETBACKUPGNOW_OK = 65;

        /// <summary>
        /// 正在取回武器
        /// </summary>
        public const string SGETBACKUPGNOW_ING = "~@GETBACKUPGNOW_ING";

        /// <summary>
        /// 正在取回武器编号
        /// </summary>
        public const int NGETBACKUPGNOW_ING = 66;

        /// <summary>
        /// 取回武器失败
        /// </summary>
        public const string SGETBACKUPGNOW_FAIL = "~@GETBACKUPGNOW_FAIL";

        /// <summary>
        /// 取回武器失败编号
        /// </summary>
        public const int NGETBACKUPGNOW_FAIL = 67;

        /// <summary>
        /// 取回武器升级包裹满
        /// </summary>
        public const string SGETBACKUPGNOW_BAGFULL = "~@getbackupgnow_bagfull";

        /// <summary>
        /// 取回武器升级包裹满编号
        /// </summary>
        public const int NGETBACKUPGNOW_BAGFULL = 68;

        public const string STAKEONITEMS = "@TAKEONITEM";
        public const int NTAKEONITEMS = 69;
        public const string STAKEOFFITEMS = "@TAKEOFFITEM";
        public const int NTAKEOFFITEMS = 70;
        public const string SPLAYREVIVE = "@PLAYREVIVE";
        public const int NPLAYREVIVE = 71;
        public const string SMOVEABILITY_OK = "@MOVEABILITY_OK";
        public const int NMOVEABILITY_OK = 72;
        public const string SMOVEABILITY_FAIL = "@MOVEABILITY_FAIL";
        public const int NMOVEABILITY_FAIL = 73;
        public const string SASSEMBLEALL = "@ASSEMBLEALL";
        public const int NASSEMBLEALL = 74;
        public const string SASSEMBLEWEAPON = "@ASSEMBLEWEAPON";
        public const int NASSEMBLEWEAPON = 75;
        public const string SASSEMBLEDRESS = "@ASSEMBLEDRESS";
        public const int NASSEMBLEDRESS = 76;
        public const string SASSEMBLEHELMET = "@ASSEMBLEHELMET";
        public const int NASSEMBLEHELMET = 77;
        public const string SASSEMBLENECKLACE = "@ASSEMBLENECKLACE";
        public const int NASSEMBLENECKLACE = 78;
        public const string SASSEMBLERING = "@ASSEMBLERING";
        public const int NASSEMBLERING = 79;
        public const string SASSEMBLEARMRING = "@ASSEMBLEARMRING";
        public const int NASSEMBLEARMRING = 80;
        public const string SASSEMBLEBELT = "@ASSEMBLEBELT";
        public const int NASSEMBLEBELT = 81;
        public const string SASSEMBLEBOOT = "@ASSEMBLEBOOT";
        public const int NASSEMBLEBOOT = 82;
        public const string SASSEMBLEFAIL = "@ASSEMBLEFAIL";
        public const int NASSEMBLEFAIL = 83;

        // By John 增加英雄创建脚本定义
        public const string SCREATEHEROFAILEX = "@CREATEHEROFAILEX";

        public const int NCREATEHEROFAILEX = 84;
        public const string SLOGOUTHEROFIRST = "@LOGOUTHEROFIRST";
        public const int NLOGOUTHEROFIRST = 85;
        public const string SNOTHAVEHERO = "@NOTHAVEHERO";
        public const int NNOTHAVEHERO = 86;
        public const string SHERONAMEFILTER = "@HERONAMEFILTER";
        public const int NHERONAMEFILTER = 87;
        public const string SHAVEHERO = "@HAVEHERO";
        public const int NHAVEHERO = 88;
        public const string SCREATEHEROOK = "@CREATEHEROOK";
        public const int NCREATEHEROOK = 89;
        public const string SHERONAMEEXISTS = "@HERONAMEEXISTS";
        public const int NHERONAMEEXISTS = 90;
        public const string SDELETEHEROOK = "@DELETEHEROOK";
        public const int NDELETEHEROOK = 91;
        public const string SDELETEHEROFAIL = "@DELETEHEROFAIL";
        public const int NDELETEHEROFAIL = 92;
        public const string SHEROOVERCHRCOUNT = "@HEROOVERCHRCOUNT";
        public const int NHEROOVERCHRCOUNT = 93;
        public const string SASSEMBLE = "@ASSEMBLE";
        public const string SMAGSELFFUNC = "@MAGSELFFUNC";
        public const string SMAGTAGFUNC = "@MAGTAGFUNC";
        public const string SMAGTAGFUNCEX = "@MAGTAGFUNCEX";
        public const string SMAGMONFUNC = "@MAGMONFUNC";
        public const string sVAR_SERVERNAME = "$SERVERNAME";
        public const string tVAR_SERVERNAME = "<$1>";
        public const int nVAR_SERVERNAME = 1;
        public const string sVAR_SERVERIP = "$SERVERIP";
        public const string tVAR_SERVERIP = "<$2>";
        public const int nVAR_SERVERIP = 2;
        public const string sVAR_WEBSITE = "$WEBSITE";
        public const string tVAR_WEBSITE = "<$3>";
        public const int nVAR_WEBSITE = 3;
        public const string sVAR_BBSSITE = "$BBSSITE";
        public const string tVAR_BBSSITE = "<$4>";
        public const int nVAR_BBSSITE = 4;
        public const string sVAR_CLIENTDOWNLOAD = "$CLIENTDOWNLOAD";
        public const string tVAR_CLIENTDOWNLOAD = "<$5>";
        public const int nVAR_CLIENTDOWNLOAD = 5;
        public const string sVAR_QQ = "$QQ";
        public const string tVAR_QQ = "<$6>";
        public const int nVAR_QQ = 6;
        public const string sVAR_PHONE = "$PHONE";
        public const string tVAR_PHONE = "<$7>";
        public const int nVAR_PHONE = 7;
        public const string sVAR_BANKACCOUNT0 = "$BANKACCOUNT0";
        public const string tVAR_BANKACCOUNT0 = "<$8>";
        public const int nVAR_BANKACCOUNT0 = 8;
        public const string sVAR_BANKACCOUNT1 = "$BANKACCOUNT1";
        public const string tVAR_BANKACCOUNT1 = "<$9>";
        public const int nVAR_BANKACCOUNT1 = 9;
        public const string sVAR_BANKACCOUNT2 = "$BANKACCOUNT2";
        public const string tVAR_BANKACCOUNT2 = "<$10>";
        public const int nVAR_BANKACCOUNT2 = 10;
        public const string sVAR_BANKACCOUNT3 = "$BANKACCOUNT3";
        public const string tVAR_BANKACCOUNT3 = "<$11>";
        public const int nVAR_BANKACCOUNT3 = 11;
        public const string sVAR_BANKACCOUNT4 = "$BANKACCOUNT4";
        public const string tVAR_BANKACCOUNT4 = "<$12>";
        public const int nVAR_BANKACCOUNT4 = 12;
        public const string sVAR_BANKACCOUNT5 = "$BANKACCOUNT5";
        public const string tVAR_BANKACCOUNT5 = "<$13>";
        public const int nVAR_BANKACCOUNT5 = 13;
        public const string sVAR_BANKACCOUNT6 = "$BANKACCOUNT6";
        public const string tVAR_BANKACCOUNT6 = "<$14>";
        public const int nVAR_BANKACCOUNT6 = 14;
        public const string sVAR_BANKACCOUNT7 = "$BANKACCOUNT7";
        public const string tVAR_BANKACCOUNT7 = "<$15>";
        public const int nVAR_BANKACCOUNT7 = 15;
        public const string sVAR_BANKACCOUNT8 = "$BANKACCOUNT8";
        public const string tVAR_BANKACCOUNT8 = "<$16>";
        public const int nVAR_BANKACCOUNT8 = 16;
        public const string sVAR_BANKACCOUNT9 = "$BANKACCOUNT9";
        public const string tVAR_BANKACCOUNT9 = "<$17>";
        public const int nVAR_BANKACCOUNT9 = 17;
        public const string sVAR_GAMEGOLDNAME = "$GAMEGOLDNAME";
        public const string tVAR_GAMEGOLDNAME = "<$18>";
        public const int nVAR_GAMEGOLDNAME = 18;
        public const string sVAR_GAMEPOINTNAME = "$GAMEPOINTNAME";
        public const string tVAR_GAMEPOINTNAME = "<$19>";
        public const int nVAR_GAMEPOINTNAME = 19;
        public const string sVAR_USERCOUNT = "$USERCOUNT";
        public const string tVAR_USERCOUNT = "<$20>";
        public const int nVAR_USERCOUNT = 20;
        public const string sVAR_DATETIME = "$DATETIME";
        public const string tVAR_DATETIME = "<$21>";
        public const int nVAR_DATETIME = 21;
        public const string sVAR_USERNAME = "$USERNAME";
        public const string tVAR_USERNAME = "<$22>";
        public const int nVAR_USERNAME = 22;
        public const string sVAR_MAPNAME = "$MAPNAME";
        public const string tVAR_MAPNAME = "<$23>";
        public const int nVAR_MAPNAME = 23;
        public const string sVAR_GUILDNAME = "$GUILDNAME";
        public const string tVAR_GUILDNAME = "<$24>";
        public const int nVAR_GUILDNAME = 24;
        public const string sVAR_RANKNAME = "$RANKNAME";
        public const string tVAR_RANKNAME = "<$25>";
        public const int nVAR_RANKNAME = 25;
        public const string sVAR_LEVEL = "$LEVEL";
        public const string tVAR_LEVEL = "<$26>";
        public const int nVAR_LEVEL = 26;
        public const string sVAR_HP = "$HP";
        public const string tVAR_HP = "<$27>";
        public const int nVAR_HP = 27;
        public const string sVAR_MAXHP = "$MAXHP";
        public const string tVAR_MAXHP = "<$28>";
        public const int nVAR_MAXHP = 28;
        public const string sVAR_MP = "$MP";
        public const string tVAR_MP = "<$29>";
        public const int nVAR_MP = 29;
        public const string sVAR_MAXMP = "$MAXMP";
        public const string tVAR_MAXMP = "<$30>";
        public const int nVAR_MAXMP = 30;
        public const string sVAR_AC = "$AC";
        public const string tVAR_AC = "<$31>";
        public const int nVAR_AC = 31;
        public const string sVAR_MAXAC = "$MAXAC";
        public const string tVAR_MAXAC = "<$32>";
        public const int nVAR_MAXAC = 32;
        public const string sVAR_MAC = "$MAC";
        public const string tVAR_MAC = "<$33>";
        public const int nVAR_MAC = 33;
        public const string sVAR_MAXMAC = "$MAXMAC";
        public const string tVAR_MAXMAC = "<$34>";
        public const int nVAR_MAXMAC = 34;
        public const string sVAR_DC = "$DC";
        public const string tVAR_DC = "<$35>";
        public const int nVAR_DC = 35;
        public const string sVAR_MAXDC = "$MAXDC";
        public const string tVAR_MAXDC = "<$36>";
        public const int nVAR_MAXDC = 36;
        public const string sVAR_MC = "$MC";
        public const string tVAR_MC = "<$37>";
        public const int nVAR_MC = 37;
        public const string sVAR_MAXMC = "$MAXMC";
        public const string tVAR_MAXMC = "<$38>";
        public const int nVAR_MAXMC = 38;
        public const string sVAR_SC = "$SC";
        public const string tVAR_SC = "<$39>";
        public const int nVAR_SC = 39;
        public const string sVAR_MAXSC = "$MAXSC";
        public const string tVAR_MAXSC = "<$40>";
        public const int nVAR_MAXSC = 40;
        public const string sVAR_EXP = "$EXP";
        public const string tVAR_EXP = "<$41>";
        public const int nVAR_EXP = 41;
        public const string sVAR_MAXEXP = "$MAXEXP";
        public const string tVAR_MAXEXP = "<$42>";
        public const int nVAR_MAXEXP = 42;
        public const string sVAR_PKPOINT = "$PKPOINT";
        public const string tVAR_PKPOINT = "<$43>";
        public const int nVAR_PKPOINT = 43;
        public const string sVAR_CREDITPOINT = "$CREDITPOINT";
        public const string tVAR_CREDITPOINT = "<$44>";
        public const int nVAR_CREDITPOINT = 44;
        public const string sVAR_GOLDCOUNT = "$GOLDCOUNT";
        public const string tVAR_GOLDCOUNT = "<$45>";
        public const int nVAR_GOLDCOUNT = 45;
        public const string sVAR_GAMEGOLD = "$GAMEGOLD";
        public const string tVAR_GAMEGOLD = "<$46>";
        public const int nVAR_GAMEGOLD = 46;
        public const string sVAR_GAMEPOINT = "$GAMEPOINT";
        public const string tVAR_GAMEPOINT = "<$47>";
        public const int nVAR_GAMEPOINT = 47;
        public const string sVAR_LOGINTIME = "$LOGINTIME";
        public const string tVAR_LOGINTIME = "<$48>";
        public const int nVAR_LOGINTIME = 48;
        public const string sVAR_LOGINLONG = "$LOGINLONG";
        public const string tVAR_LOGINLONG = "<$49>";
        public const int nVAR_LOGINLONG = 49;
        public const string sVAR_DRESS = "$DRESS";
        public const string tVAR_DRESS = "<$50>";
        public const int nVAR_DRESS = 50;
        public const string sVAR_WEAPON = "$WEAPON";
        public const string tVAR_WEAPON = "<$51>";
        public const int nVAR_WEAPON = 51;
        public const string sVAR_RIGHTHAND = "$RIGHTHAND";
        public const string tVAR_RIGHTHAND = "<$52>";
        public const int nVAR_RIGHTHAND = 52;
        public const string sVAR_HELMET = "$HELMET";
        public const string tVAR_HELMET = "<$53>";
        public const int nVAR_HELMET = 53;
        public const string sVAR_NECKLACE = "$NECKLACE";
        public const string tVAR_NECKLACE = "<$54>";
        public const int nVAR_NECKLACE = 54;
        public const string sVAR_RING_R = "$RING_R";
        public const string tVAR_RING_R = "<$55>";
        public const int nVAR_RING_R = 55;
        public const string sVAR_RING_L = "$RING_L";
        public const string tVAR_RING_L = "<$56>";
        public const int nVAR_RING_L = 56;
        public const string sVAR_ARMRING_R = "$ARMRING_R";
        public const string tVAR_ARMRING_R = "<$57>";
        public const int nVAR_ARMRING_R = 57;
        public const string sVAR_ARMRING_L = "$ARMRING_L";
        public const string tVAR_ARMRING_L = "<$58>";
        public const int nVAR_ARMRING_L = 58;
        public const string sVAR_BUJUK = "$BUJUK";
        public const string tVAR_BUJUK = "<$59>";
        public const int nVAR_BUJUK = 59;
        public const string sVAR_BELT = "$BELT";
        public const string tVAR_BELT = "<$60>";
        public const int nVAR_BELT = 60;
        public const string sVAR_BOOTS = "$BOOTS";
        public const string tVAR_BOOTS = "<$61>";
        public const int nVAR_BOOTS = 61;
        public const string sVAR_CHARM = "$CHARM";
        public const string tVAR_CHARM = "<$62>";
        public const int nVAR_CHARM = 62;
        public const string sVAR_HOUSE = "$HOUSE";
        public const string tVAR_HOUSE = "<$63>";
        public const int nVAR_HOUSE = 63;
        public const string sVAR_CIMELIA = "$CIMELIA";
        public const string tVAR_CIMELIA = "<$64>";
        public const int nVAR_CIMELIA = 64;
        public const string sVAR_IPADDR = "$IPADDR";
        public const string tVAR_IPADDR = "<$65>";
        public const int nVAR_IPADDR = 65;
        public const string sVAR_IPLOCAL = "$IPLOCAL";
        public const string tVAR_IPLOCAL = "<$66>";
        public const int nVAR_IPLOCAL = 66;
        public const string sVAR_GUILDBUILDPOINT = "$GUILDBUILDPOINT";
        public const string tVAR_GUILDBUILDPOINT = "<$67>";
        public const int nVAR_GUILDBUILDPOINT = 67;
        public const string sVAR_GUILDAURAEPOINT = "$GUILDAURAEPOINT";
        public const string tVAR_GUILDAURAEPOINT = "<$68>";
        public const int nVAR_GUILDAURAEPOINT = 68;
        public const string sVAR_GUILDSTABILITYPOINT = "$GUILDSTABILITYPOINT";
        public const string tVAR_GUILDSTABILITYPOINT = "<$69>";
        public const int nVAR_GUILDSTABILITYPOINT = 69;
        public const string sVAR_GUILDFLOURISHPOINT = "$GUILDFLOURISHPOINT";
        public const string tVAR_GUILDFLOURISHPOINT = "<$70>";
        public const int nVAR_GUILDFLOURISHPOINT = 70;
        public const string sVAR_GUILDMONEYCOUNT = "$GUILDMONEYCOUNT";
        public const string tVAR_GUILDMONEYCOUNT = "<$71>";
        public const int nVAR_GUILDMONEYCOUNT = 71;
        public const string sVAR_REQUESTCASTLEWARITEM = "$REQUESTCASTLEWARITEM";
        public const string tVAR_REQUESTCASTLEWARITEM = "<$72>";
        public const int nVAR_REQUESTCASTLEWARITEM = 72;
        public const string sVAR_REQUESTCASTLEWARDAY = "$REQUESTCASTLEWARDAY";
        public const string tVAR_REQUESTCASTLEWARDAY = "<$73>";
        public const int nVAR_REQUESTCASTLEWARDAY = 73;
        public const string sVAR_REQUESTBUILDGUILDITEM = "$REQUESTBUILDGUILDITEM";
        public const string tVAR_REQUESTBUILDGUILDITEM = "<$74>";
        public const int nVAR_REQUESTBUILDGUILDITEM = 74;
        public const string sVAR_OWNERGUILD = "$OWNERGUILD";
        public const string tVAR_OWNERGUILD = "<$75>";
        public const int nVAR_OWNERGUILD = 75;
        public const string sVAR_CASTLENAME = "$CASTLENAME";
        public const string tVAR_CASTLENAME = "<$76>";
        public const int nVAR_CASTLENAME = 76;
        public const string sVAR_LORD = "$LORD";
        public const string tVAR_LORD = "<$77>";
        public const int nVAR_LORD = 77;
        public const string sVAR_GUILDWARFEE = "$GUILDWARFEE";
        public const string tVAR_GUILDWARFEE = "<$78>";
        public const int nVAR_GUILDWARFEE = 78;
        public const string sVAR_BUILDGUILDFEE = "$BUILDGUILDFEE";
        public const string tVAR_BUILDGUILDFEE = "<$79>";
        public const int nVAR_BUILDGUILDFEE = 79;
        public const string sVAR_CASTLEWARDATE = "$CASTLEWARDATE";
        public const string tVAR_CASTLEWARDATE = "<$80>";
        public const int nVAR_CASTLEWARDATE = 80;
        public const string sVAR_LISTOFWAR = "$LISTOFWAR";
        public const string tVAR_LISTOFWAR = "<$81>";
        public const int nVAR_LISTOFWAR = 81;
        public const string sVAR_CASTLECHANGEDATE = "$CASTLECHANGEDATE";
        public const string tVAR_CASTLECHANGEDATE = "<$82>";
        public const int nVAR_CASTLECHANGEDATE = 82;
        public const string sVAR_CASTLEWARLASTDATE = "$CASTLEWARLASTDATE";
        public const string tVAR_CASTLEWARLASTDATE = "<$83>";
        public const int nVAR_CASTLEWARLASTDATE = 83;
        public const string sVAR_CASTLEGETDAYS = "$CASTLEGETDAYS";
        public const string tVAR_CASTLEGETDAYS = "<$84>";
        public const int nVAR_CASTLEGETDAYS = 84;
        public const string sVAR_CMD_DATE = "$CMD_DATE";
        public const string tVAR_CMD_DATE = "<$85>";
        public const int nVAR_CMD_DATE = 85;
        public const string sVAR_CMD_PRVMSG = "$CMD_PRVMSG";
        public const string tVAR_CMD_PRVMSG = "<$86>";
        public const int nVAR_CMD_PRVMSG = 86;
        public const string sVAR_CMD_ALLOWMSG = "$CMD_ALLOWMSG";
        public const string tVAR_CMD_ALLOWMSG = "<$87>";
        public const int nVAR_CMD_ALLOWMSG = 87;
        public const string sVAR_CMD_LETSHOUT = "$CMD_LETSHOUT";
        public const string tVAR_CMD_LETSHOUT = "<$88>";
        public const int nVAR_CMD_LETSHOUT = 88;
        public const string sVAR_CMD_LETTRADE = "$CMD_LETTRADE";
        public const string tVAR_CMD_LETTRADE = "<$89>";
        public const int nVAR_CMD_LETTRADE = 89;
        public const string sVAR_CMD_LETGuild = "$CMD_LETGuild";
        public const string tVAR_CMD_LETGuild = "<$90>";
        public const int nVAR_CMD_LETGuild = 90;
        public const string sVAR_CMD_ENDGUILD = "$CMD_ENDGUILD";
        public const string tVAR_CMD_ENDGUILD = "<$91>";
        public const int nVAR_CMD_ENDGUILD = 91;
        public const string sVAR_CMD_BANGUILDCHAT = "$CMD_BANGUILDCHAT";
        public const string tVAR_CMD_BANGUILDCHAT = "<$92>";
        public const int nVAR_CMD_BANGUILDCHAT = 92;
        public const string sVAR_CMD_AUTHALLY = "$CMD_AUTHALLY";
        public const string tVAR_CMD_AUTHALLY = "<$93>";
        public const int nVAR_CMD_AUTHALLY = 93;
        public const string sVAR_CMD_AUTH = "$CMD_AUTH";
        public const string tVAR_CMD_AUTH = "<$94>";
        public const int nVAR_CMD_AUTH = 94;
        public const string sVAR_CMD_AUTHCANCEL = "$CMD_AUTHCANCEL";
        public const string tVAR_CMD_AUTHCANCEL = "<$95>";
        public const int nVAR_CMD_AUTHCANCEL = 95;
        public const string sVAR_CMD_USERMOVE = "$CMD_USERMOVE";
        public const string tVAR_CMD_USERMOVE = "<$96>";
        public const int nVAR_CMD_USERMOVE = 96;
        public const string sVAR_CMD_SEARCHING = "$CMD_SEARCHING";
        public const string tVAR_CMD_SEARCHING = "<$97>";
        public const int nVAR_CMD_SEARCHING = 97;
        public const string sVAR_CMD_ALLOWGROUPCALL = "$CMD_ALLOWGROUPCALL";
        public const string tVAR_CMD_ALLOWGROUPCALL = "<$98>";
        public const int nVAR_CMD_ALLOWGROUPCALL = 98;
        public const string sVAR_CMD_GROUPRECALLL = "$CMD_GROUPRECALLL";
        public const string tVAR_CMD_GROUPRECALLL = "<$99>";
        public const int nVAR_CMD_GROUPRECALLL = 99;
        public const string sVAR_CMD_ALLOWGUILDRECALL = "$CMD_ALLOWGUILDRECALL";
        public const string tVAR_CMD_ALLOWGUILDRECALL = "<$100>";
        public const int nVAR_CMD_ALLOWGUILDRECALL = 100;
        public const string sVAR_CMD_GUILDRECALLL = "$CMD_GUILDRECALLL";
        public const string tVAR_CMD_GUILDRECALLL = "<$101>";
        public const int nVAR_CMD_GUILDRECALLL = 101;
        public const string sVAR_CMD_DEAR = "$CMD_DEAR";
        public const string tVAR_CMD_DEAR = "<$102>";
        public const int nVAR_CMD_DEAR = 102;
        public const string sVAR_CMD_ALLOWDEARRCALL = "$CMD_ALLOWDEARRCALL";
        public const string tVAR_CMD_ALLOWDEARRCALL = "<$103>";
        public const int nVAR_CMD_ALLOWDEARRCALL = 103;
        public const string sVAR_CMD_DEARRECALL = "$CMD_DEARRECALL";
        public const string tVAR_CMD_DEARRECALL = "<$104>";
        public const int nVAR_CMD_DEARRECALL = 104;
        public const string sVAR_CMD_MASTER = "$CMD_MASTER";
        public const string tVAR_CMD_MASTER = "<$105>";
        public const int nVAR_CMD_MASTER = 105;
        public const string sVAR_CMD_ALLOWMASTERRECALL = "$CMD_ALLOWMASTERRECALL";
        public const string tVAR_CMD_ALLOWMASTERRECALL = "<$106>";
        public const int nVAR_CMD_ALLOWMASTERRECALL = 106;
        public const string sVAR_CMD_MASTERECALL = "$CMD_MASTERECALL";
        public const string tVAR_CMD_MASTERECALL = "<$107>";
        public const int nVAR_CMD_MASTERECALL = 107;
        public const string sVAR_CMD_TAKEONHORSE = "$CMD_TAKEONHORSE";
        public const string tVAR_CMD_TAKEONHORSE = "<$108>";
        public const int nVAR_CMD_TAKEONHORSE = 108;
        public const string sVAR_CMD_TAKEOFHORSE = "$CMD_TAKEOFHORSE";
        public const string tVAR_CMD_TAKEOFHORSE = "<$109>";
        public const int nVAR_CMD_TAKEOFHORSE = 109;
        public const string sVAR_CMD_ALLSYSMSG = "$CMD_ALLSYSMSG";
        public const string tVAR_CMD_ALLSYSMSG = "<$110>";
        public const int nVAR_CMD_ALLSYSMSG = 110;
        public const string sVAR_CMD_MEMBERFUNCTION = "$CMD_MEMBERFUNCTION";
        public const string tVAR_CMD_MEMBERFUNCTION = "<$111>";
        public const int nVAR_CMD_MEMBERFUNCTION = 111;
        public const string sVAR_CMD_MEMBERFUNCTIONEX = "$CMD_MEMBERFUNCTIONEX";
        public const string tVAR_CMD_MEMBERFUNCTIONEX = "<$112>";
        public const int nVAR_CMD_MEMBERFUNCTIONEX = 112;
        public const string sVAR_CASTLEGOLD = "$CASTLEGOLD";
        public const string tVAR_CASTLEGOLD = "<$113>";
        public const int nVAR_CASTLEGOLD = 113;
        public const string sVAR_TODAYINCOME = "$TODAYINCOME";
        public const string tVAR_TODAYINCOME = "<$114>";
        public const int nVAR_TODAYINCOME = 114;
        public const string sVAR_CASTLEDOORSTATE = "$CASTLEDOORSTATE";
        public const string tVAR_CASTLEDOORSTATE = "<$115>";
        public const int nVAR_CASTLEDOORSTATE = 115;
        public const string sVAR_REPAIRDOORGOLD = "$REPAIRDOORGOLD";
        public const string tVAR_REPAIRDOORGOLD = "<$116>";
        public const int nVAR_REPAIRDOORGOLD = 116;
        public const string sVAR_REPAIRWALLGOLD = "$REPAIRWALLGOLD";
        public const string tVAR_REPAIRWALLGOLD = "<$117>";
        public const int nVAR_REPAIRWALLGOLD = 117;
        public const string sVAR_GUARDFEE = "$GUARDFEE";
        public const string tVAR_GUARDFEE = "<$118>";
        public const int nVAR_GUARDFEE = 118;
        public const string sVAR_ARCHERFEE = "$ARCHERFEE";
        public const string tVAR_ARCHERFEE = "<$119>";
        public const int nVAR_ARCHERFEE = 119;
        public const string sVAR_GUARDRULE = "$GUARDRULE";
        public const string tVAR_GUARDRULE = "<$120>";
        public const int nVAR_GUARDRULE = 120;
        public const string sVAR_HUMAN = "$HUMAN(";
        public const string tVAR_HUMAN = "<$122/{0}>";
        public const int nVAR_HUMAN = 122;
        public const string sVAR_GUILD = "$GUILD(";
        public const string tVAR_GUILD = "<$123/{0}>";
        public const int nVAR_GUILD = 123;
        public const string sVAR_GLOBAL = "$GLOBAL(";
        public const string tVAR_GLOBAL = "<$124/{0}>";
        public const int nVAR_GLOBAL = 124;
        public const string sVAR_STR = "$STR(";
        public const string tVAR_STR = "<$125/{0}>";
        public const int nVAR_STR = 125;
        public const string sVAR_STORAGE2STATE = "$STORAGE2STATE";
        public const string tVAR_STORAGE2STATE = "<$126>";
        public const int nVAR_STORAGE2STATE = 126;
        public const string sVAR_STORAGE3STATE = "$STORAGE3STATE";
        public const string tVAR_STORAGE3STATE = "<$127>";
        public const int nVAR_STORAGE3STATE = 127;
        public const string sVAR_STORAGE4STATE = "$STORAGE4STATE";
        public const string tVAR_STORAGE4STATE = "<$128>";
        public const int nVAR_STORAGE4STATE = 128;
        public const string sVAR_STORAGE5STATE = "$STORAGE5STATE";
        public const string tVAR_STORAGE5STATE = "<$129>";
        public const int nVAR_STORAGE5STATE = 129;
        public const string sVAR_SELFNAME = "$SELFNAME";
        public const string tVAR_SELFNAME = "<$130>";
        public const int nVAR_SELFNAME = 130;
        public const string sVAR_POSENAME = "$POSENAME";
        public const string tVAR_POSENAME = "<$131>";
        public const int nVAR_POSENAME = 131;
        public const string sVAR_GAMEDIAMOND = "$GAMEDIAMOND";
        public const string tVAR_GAMEDIAMOND = "<$132>";
        public const int nVAR_GAMEDIAMOND = 132;
        public const string sVAR_GAMEGIRD = "$GAMEGIRD";
        public const string tVAR_GAMEGIRD = "<$133>";
        public const int nVAR_GAMEGIRD = 133;
        public const string sVAR_MISSIONARITHMOMETER = "$MISSIONARITHMOMETER(";
        public const string tVAR_MISSIONARITHMOMETER = "<$134/{0}>";
        public const int nVAR_MISSIONARITHMOMETER = 134;
        public const string sVAR_CMD_ALLOWFIREND = "$CMD_ALLOWFIREND";
        public const string tVAR_CMD_ALLOWFIREND = "<$135>";
        public const int nVAR_CMD_ALLOWFIREND = 135;
        public const string sVAR_EFFIGYSTATE = "$EFFIGYSTATE";
        public const string tVAR_EFFIGYSTATE = "<$136>";
        public const int nVAR_EFFIGYSTATE = 136;
        public const string sVAR_EFFIGYOFFSET = "$EFFIGYOFFSET";
        public const string tVAR_EFFIGYOFFSET = "<$137>";
        public const int nVAR_EFFIGYOFFSET = 137;
        public const string sVAR_YEAR = "$YEAR";
        public const string tVAR_YEAR = "<$138>";
        public const int nVAR_YEAR = 138;
        public const string sVAR_MONTH = "$MONTH";
        public const string tVAR_MONTH = "<$139>";
        public const int nVAR_MONTH = 139;
        public const string sVAR_DAY = "$DAY";
        public const string tVAR_DAY = "<$140>";
        public const int nVAR_DAY = 140;
        public const string sVAR_HOUR = "$HOUR";
        public const string tVAR_HOUR = "<$141>";
        public const int nVAR_HOUR = 141;
        public const string sVAR_MINUTE = "$MINUTE";
        public const string tVAR_MINUTE = "<$142>";
        public const int nVAR_MINUTE = 142;
        public const string sVAR_SEC = "$SEC";
        public const string tVAR_SEC = "<$143>";
        public const int nVAR_SEC = 143;
        public const string sVAR_MAP = "$MAP";
        public const string tVAR_MAP = "<$144>";
        public const int nVAR_MAP = 144;
        public const string sVAR_X = "$X";
        public const string tVAR_X = "<$145>";
        public const int nVAR_X = 145;
        public const string sVAR_Y = "$Y";
        public const string tVAR_Y = "<$146>";
        public const int nVAR_Y = 146;
        public const string sVAR_UNMASTER_FORCE = "$UNMASTER_FORCE";
        public const string tVAR_UNMASTER_FORCE = "<$147>";
        public const int nVAR_UNMASTER_FORCE = 147;
        public const string sVAR_TEAM = "$TEAM";
        public const string tVAR_TEAM = "<$148/{0}>";
        public const int nVAR_TEAM = 148;
        public const string sVAR_USERGOLDCOUNT = "$USERGOLDCOUNT";
        public const string tVAR_USERGOLDCOUNT = "<$149>";
        public const int nVAR_USERGOLDCOUNT = 149;
        public const string sVAR_MAXGOLDCOUNT = "$MAXGOLDCOUNT";
        public const string tVAR_MAXGOLDCOUNT = "<$150>";
        public const int nVAR_MAXGOLDCOUNT = 150;
        public const string sVAR_STORAGEGOLDCOUNT = "$STORAGEGOLDCOUNT";
        public const string tVAR_STORAGEGOLDCOUNT = "<$151>";
        public const int nVAR_STORAGEGOLDCOUNT = 151;
        public const string sVAR_BINDGOLDCOUNT = "$BINDGOLDCOUNT";
        public const string tVAR_BINDGOLDCOUNT = "<$152>";
        public const int nVAR_BINDGOLDCOUNT = 152;
        public const string sVAR_UPGRADEWEAPONFEE = "$UPGRADEWEAPONFEE";
        public const string tVAR_UPGRADEWEAPONFEE = "<$153>";
        public const int nVAR_UPGRADEWEAPONFEE = 153;
        public const string sVAR_USERWEAPON = "$USERWEAPON";
        public const string tVAR_USERWEAPON = "<$154>";
        public const int nVAR_USERWEAPON = 154;
        public const string sVAR_CMD_STARTQUEST = "$CMD_STARTQUEST";
        public const string tVAR_CMD_STARTQUEST = "<$155>";
        public const int nVAR_CMD_STARTQUEST = 155;
        public const string sVAR_FBMAPNAME = "$FBMAPNAME";
        public const string tVAR_FBMAPNAME = "<$156>";
        public const int nVAR_FBMAPNAME = 156;
        public const string sVAR_FBMAP = "$FBMAP";
        public const string tVAR_FBMAP = "<$157>";
        public const int nVAR_FBMAP = 157;
        public const string sVAR_ACCOUNT = "$ACCOUNT";
        public const string tVAR_ACCOUNT = "<$158>";
        public const int nVAR_ACCOUNT = 158;
        public const string sVAR_ASSEMBLEITEMNAME = "$ASSEMBLEITEMNAME";
        public const string tVAR_ASSEMBLEITEMNAME = "<$159>";
        public const int nVAR_ASSEMBLEITEMNAME = 159;

        /// <summary>
        /// 重置标签
        /// </summary>
        public const string RESETLABEL = "$RESETLABEL";
    }
}