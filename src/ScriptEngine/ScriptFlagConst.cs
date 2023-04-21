namespace ScriptEngine {
    public static class ScriptFlagConst {
        public const string sMarket_Def = "Market_Def";
        public const string sNpc_def = "Npc_def";
        public const string sMAN = "MAN";
        public const string sSUNRAISE = "SUNRAISE";
        public const string sDAY = "DAY";
        public const string sSUNSET = "SUNSET";
        public const string sNIGHT = "NIGHT";
        public const string sWarrior = "Warrior";
        public const string sWizard = "Wizard";
        public const string sTaos = "Taoist";
        public const string sSUN = "SUN";
        public const string sMON = "MON";
        public const string sTUE = "TUE";
        public const string sWED = "WED";
        public const string sTHU = "THU";
        public const string sFRI = "FRI";
        public const string sSAT = "SAT";
        /// <summary>
        /// 元宝寄售:出售物品
        /// </summary>
        public const string sDealYBme = "@dealybme";
        /// <summary>
        /// 元宝寄售
        /// </summary>        
        public const string sybdeal = "@ybdeal";
        /// <summary>
        /// 离线挂机留言
        /// </summary>
        public const string sOFFLINEMSG = "@@offlinemsg";
        public const string sSL_SENDMSG = "@@sendmsg";
        /// <summary>
        /// 特殊修理
        /// </summary>
        public const string SuperRepair = "@s_repair";
        public const string sSUPERREPAIROK = "~@s_repair";
        /// <summary>
        /// 特殊修理失败
        /// </summary>
        public const string Superrepairfail = "@fail_s_repair";
        public const string sREPAIR = "@repair";
        public const string sREPAIROK = "~@repair";
        public const string sBUY = "@buy";
        public const string sSELL = "@sell";
        public const string sMAKEDURG = "@makedrug";
        public const string sPRICES = "@prices";
        public const string sSTORAGE = "@storage";
        public const string sGETBACK = "@getback";
        public const string sUPGRADENOW = "@upgradenow";
        public const string sUPGRADEING = "~@upgradenow_ing";
        public const string sUPGRADEOK = "~@upgradenow_ok";
        public const string sUPGRADEFAIL = "~@upgradenow_fail";
        public const string sGETBACKUPGNOW = "@getbackupgnow";
        public const string sGETBACKUPGOK = "~@getbackupgnow_ok";
        public const string sGETBACKUPGFAIL = "~@getbackupgnow_fail";
        public const string sGETBACKUPGFULL = "~@getbackupgnow_bagfull";
        public const string sGETBACKUPGING = "~@getbackupgnow_ing";
        public const string sEXIT = "@exit";
        public const string sBACK = "@back";
        public const string sMAIN = "@main";
        public const string sFAILMAIN = "~@main";
        public const string sGETMASTER = "@@getmaster";
        public const string sGETMARRY = "@@getmarry";
        public const string UseItemName = "@@useitemname";
        public const string sBUILDGUILDNOW = "@@buildguildnow";
        public const string sSCL_GUILDWAR = "@@guildwar";
        public const string sDONATE = "@@donate";
        public const string sREQUESTCASTLEWAR = "@requestcastlewarnow";
        public const string sCASTLENAME = "@@castlename";
        public const string sWITHDRAWAL = "@@withdrawal";
        public const string sRECEIPTS = "@@receipts";
        public const string sOPENMAINDOOR = "@openmaindoor";
        public const string sCLOSEMAINDOOR = "@closemaindoor";
        public const string sREPAIRDOORNOW = "@repairdoornow";
        public const string sREPAIRWALLNOW1 = "@repairwallnow1";
        public const string sREPAIRWALLNOW2 = "@repairwallnow2";
        public const string sREPAIRWALLNOW3 = "@repairwallnow3";
        public const string sHIREARCHERNOW = "@hirearchernow";
        public const string sHIREGUARDNOW = "@hireguardnow";
        public const string sHIREGUARDOK = "@hireguardok";
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
        public const string SASSEMBLE = "@ASSEMBLE";
        public const string SMAGSELFFUNC = "@MAGSELFFUNC";
        public const string SMAGTAGFUNC = "@MAGTAGFUNC";
        public const string SMAGTAGFUNCEX = "@MAGTAGFUNCEX";
        public const string SMAGMONFUNC = "@MAGMONFUNC";
        /// <summary>
        /// 重置标签
        /// </summary>
        public const string RESETLABEL = "$RESETLABEL";
    }
}