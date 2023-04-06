using SystemModule.Packets.ClientPackets;

namespace SystemModule
{
    public static class Messages
    {
        public const byte DefBlockSize = 16;

        //摆摊
        public const int CM_SENDSELL = 9050;
        public const int SM_SENDSELL = 9051;
        public const int RM_SENDSELL = 9052;
        public const int CM_MYSHOPEXIT = 9053;
        public const int SM_MYSHOPEXIT = 9054;
        public const int CM_MYSHOPHAM = 9055;
        public const int SM_MYSHOPHAM = 9056;
        public const int CM_HAMSHOPBUY = 9057;
        public const int SM_HAMSHOPBUYA = 9058;
        public const int SM_HAMSHOPBUYB = 9059;
        public const int SM_HAMSHOPTYPE = 9060;
        public const int CM_OPHAMSHOP = 9061;
        public const int SM_OPHAMSHOP = 9062;
        public const int CM_CHACKITEM = 9063;
        public const int SM_CHACKITEM = 9064;

        public const int CM_QUERYUSERSTATE = 82;
        public const int CM_QUERYUSERNAME = 80;
        public const int CM_QUERYBAGITEMS = 81;
        public const int CM_QUERYCHR = 100;
        public const int CM_NEWCHR = 101;
        public const int CM_DELCHR = 102;
        public const int CM_SELCHR = 103;
        /// <summary>
        /// 玩家选择服务器
        /// </summary>
        public const int CM_SELECTSERVER = 104;
        /// <summary>
        /// 开门
        /// </summary>
        public const int CM_OPENDOOR = 1002;
        public const int CM_SOFTCLOSE = 1009;
        public const int CM_DROPITEM = 1000;
        /// <summary>
        /// 拾取物品
        /// </summary>
        public const int CM_PICKUP = 1001;
        public const int CM_TAKEONITEM = 1003;
        public const int CM_TAKEOFFITEM = 1004;
        public const int CM_1005 = 1005;
        /// <summary>
        /// 使用武物品
        /// </summary>
        public const int CM_EAT = 1006;
        /// <summary>
        /// 挖物品
        /// </summary>
        public const int CM_BUTCH = 1007;
        public const int CM_MAGICKEYCHANGE = 1008;
        /// <summary>
        /// 点击NPC
        /// </summary>
        public const int CM_CLICKNPC = 1010;
        public const int CM_MERCHANTDLGSELECT = 1011;
        public const int CM_MERCHANTQUERYSELLPRICE = 1012;
        public const int CM_USERSELLITEM = 1013;
        public const int CM_USERBUYITEM = 1014;
        public const int CM_USERGETDETAILITEM = 1015;
        public const int CM_DROPGOLD = 1016;
        public const int CM_TEST = 1017;
        /// <summary>
        /// 检测客户是否有下载好的client
        /// </summary>
        public const int CM_LOGINNOTICEOK = 1018;
        public const int CM_GROUPMODE = 1019;
        public const int CM_CREATEGROUP = 1020;
        public const int CM_ADDGROUPMEMBER = 1021;
        public const int CM_DELGROUPMEMBER = 1022;
        public const int CM_USERREPAIRITEM = 1023;
        public const int CM_MERCHANTQUERYREPAIRCOST = 1024;
        /// <summary>
        /// 发起交易
        /// </summary>
        public const int CM_DEALTRY = 1025;
        public const int CM_DEALADDITEM = 1026;
        public const int CM_DEALDELITEM = 1027;
        public const int CM_DEALCANCEL = 1028;
        public const int CM_DEALCHGGOLD = 1029;
        public const int CM_DEALEND = 1030;
        public const int CM_USERSTORAGEITEM = 1031;
        public const int CM_USERTAKEBACKSTORAGEITEM = 1032;
        public const int CM_WANTMINIMAP = 1033;
        public const int CM_USERMAKEDRUGITEM = 1034;
        public const int CM_OPENGUILDDLG = 1035;
        public const int CM_GUILDHOME = 1036;
        public const int CM_GUILDMEMBERLIST = 1037;
        public const int CM_GUILDADDMEMBER = 1038;
        public const int CM_GUILDDELMEMBER = 1039;
        public const int CM_GUILDUPDATENOTICE = 1040;
        public const int CM_GUILDUPDATERANKINFO = 1041;
        public const int CM_1042 = 1042;
        public const int CM_ADJUST_BONUS = 1043;
        public const int CM_GUILDALLY = 1044;
        public const int CM_GUILDBREAKALLY = 1045;
        public const int CM_SPEEDHACKUSER = 10430;
        public const int CM_PROTOCOL = 2000;
        public const int CM_IDPASSWORD = 2001;
        /// <summary>
        /// 创建账号
        /// </summary>
        public const int CM_ADDNEWUSER = 2002;
        /// <summary>
        /// 修改密码
        /// </summary>
        public const int CM_CHANGEPASSWORD = 2003;
        /// <summary>
        /// 更新账号信息
        /// </summary>
        public const int CM_UPDATEUSER = 2004;
        public const int CM_THROW = 3005;
        public const int CM_TURN = 3010;
        /// <summary>
        /// 走路
        /// </summary>
        public const int CM_WALK = 3011;
        /// <summary>
        /// 蹲下
        /// </summary>
        public const int CM_SITDOWN = 3012;
        /// <summary>
        /// 跑步
        /// </summary>
        public const int CM_RUN = 3013;
        /// <summary>
        /// 攻击
        /// </summary>
        public const int CM_HIT = 3014;
        public const int CM_HEAVYHIT = 3015;
        public const int CM_BIGHIT = 3016;
        public const int CM_SPELL = 3017;
        public const int CM_POWERHIT = 3018;
        public const int CM_LONGHIT = 3019;
        public const int CM_WIDEHIT = 3024;
        public const int CM_FIREHIT = 3025;
        /// <summary>
        /// 玩家说话
        /// </summary>
        public const int CM_SAY = 3030;
        public const int CM_SPEEDHACKMSG = 3500;
        public const int SM_41 = 4;
        public const int SM_THROW = 5;
        public const int SM_RUSH = 6;
        public const int SM_RUSHKUNG = 7;
        /// <summary>
        /// 烈火
        /// </summary>
        public const int SM_FIREHIT = 8;
        public const int SM_BACKSTEP = 9;
        /// <summary>
        /// 转向
        /// </summary>
        public const int SM_TURN = 10;
        /// <summary>
        /// 走
        /// </summary>
        public const int SM_WALK = 11;
        public const int SM_SITDOWN = 12;
        public const int SM_RUN = 13;
        /// <summary>
        /// 砍
        /// </summary>
        public const int SM_HIT = 14;
        public const int SM_HEAVYHIT = 15;
        public const int SM_BIGHIT = 16;
        /// <summary>
        /// 使用魔法
        /// </summary>
        public const int SM_SPELL = 17;
        /// <summary>
        /// 刺杀
        /// </summary>
        public const int SM_POWERHIT = 18;
        public const int SM_LONGHIT = 19;
        public const int SM_DIGUP = 20;
        public const int SM_DIGDOWN = 21;
        public const int SM_FLYAXE = 22;
        public const int SM_LIGHTING = 23;
        public const int SM_WIDEHIT = 24;
        public const int SM_CRSHIT = 25;
        public const int SM_TWINHIT = 26;
        public const int SM_ALIVE = 27;
        public const int SM_MOVEFAIL = 28;
        public const int SM_HIDE = 29;
        public const int SM_DISAPPEAR = 30;
        /// <summary>
        /// 弯腰
        /// </summary>
        public const int SM_STRUCK = 31;
        public const int SM_DEATH = 32;
        public const int SM_SKELETON = 33;
        public const int SM_NOWDEATH = 34;
        public const int SM_HEAR = 40;
        public const int SM_FEATURECHANGED = 41;
        public const int SM_USERNAME = 42;
        public const int SM_43 = 43;
        public const int SM_WINEXP = 44;
        public const int SM_LEVELUP = 45;
        public const int SM_DAYCHANGING = 46;
        public const int SM_LOGON = 50;
        public const int SM_NEWMAP = 51;
        public const int SM_ABILITY = 52;
        public const int SM_HEALTHSPELLCHANGED = 53;
        public const int SM_MAPDESCRIPTION = 54;
        public const int SM_SPELL2 = 117;
        public const int SM_HWID = 113;
        public const int SM_SYSMESSAGE = 100;
        public const int SM_GROUPMESSAGE = 101;
        public const int SM_CRY = 102;
        public const int SM_WHISPER = 103;
        public const int SM_GUILDMESSAGE = 104;
        public const int SM_ADDITEM = 200;
        public const int SM_BAGITEMS = 201;
        public const int SM_DELITEM = 202;
        public const int SM_UPDATEITEM = 203;
        public const int SM_ADDMAGIC = 210;
        public const int SM_SENDMYMAGIC = 211;
        public const int SM_DELMAGIC = 212;
        public const int SM_ATTACKMODE = 213;
        public const int SM_QUERYVALUE = 215;

        public const int CM_CHECKTIME = 15999;
        /// <summary>
        /// 攻击模式成功
        /// </summary>
        public const int SM_CERTIFICATION_SUCCESS = 500;
        /// <summary>
        /// 攻击模式失败
        /// </summary>
        public const int SM_CERTIFICATION_FAIL = 501;
        /// <summary>
        /// 账号不存在
        /// </summary>
        public const int SM_ID_NOTFOUND = 502;
        public const int SM_PASSWD_FAIL = 503;
        public const int SM_NEWID_SUCCESS = 504;
        public const int SM_NEWID_FAIL = 505;
        public const int SM_CHGPASSWD_SUCCESS = 506;
        public const int SM_CHGPASSWD_FAIL = 507;
        /// <summary>
        /// 查询人物
        /// </summary>
        public const int SM_QUERYCHR = 520;
        /// <summary>
        /// 创建人物成功
        /// </summary>
        public const int SM_NEWCHR_SUCCESS = 521;
        /// <summary>
        /// 创建人物失败
        /// </summary>
        public const int SM_NEWCHR_FAIL = 522;
        /// <summary>
        /// 删除人物成功
        /// </summary>
        public const int SM_DELCHR_SUCCESS = 523;
        /// <summary>
        /// 删除人物失败
        /// </summary>
        public const int SM_DELCHR_FAIL = 524;
        public const int SM_STARTPLAY = 525;
        public const int SM_STARTFAIL = 526;
        public const int SM_QUERYCHR_FAIL = 527;
        public const int SM_OUTOFCONNECTION = 528;
        public const int SM_PASSOK_SELECTSERVER = 529;
        public const int SM_SELECTSERVER_OK = 530;
        public const int SM_NEEDUPDATE_ACCOUNT = 531;
        public const int SM_UPDATEID_SUCCESS = 532;
        public const int SM_UPDATEID_FAIL = 533;
        public const int SM_DROPITEM_SUCCESS = 600;
        public const int SM_DROPITEM_FAIL = 601;
        public const int SM_ITEMSHOW = 610;
        public const int SM_ITEMHIDE = 611;
        public const int SM_OPENDOOR_OK = 612;
        public const int SM_OPENDOOR_LOCK = 613;
        public const int SM_CLOSEDOOR = 614;
        public const int SM_TAKEON_OK = 615;
        public const int SM_TAKEON_FAIL = 616;
        public const int SM_TAKEOFF_OK = 619;
        public const int SM_TAKEOFF_FAIL = 620;
        public const int SM_SENDUSEITEMS = 621;
        public const int SM_WEIGHTCHANGED = 622;
        public const int SM_QUERYITEMDLG = 623;
        public const int SM_ITEMDLGSELECT = 624;

        public const int SM_CLEAROBJECTS = 633;
        public const int SM_CHANGEMAP = 634;
        public const int SM_EAT_OK = 635;
        public const int SM_EAT_FAIL = 636;
        public const int SM_BUTCH = 637;
        public const int SM_MAGICFIRE = 638;
        public const int SM_MAGICFIRE_FAIL = 639;
        public const int SM_MAGIC_LVEXP = 640;
        public const int SM_DURACHANGE = 642;
        public const int SM_MERCHANTSAY = 643;
        public const int SM_MOVEMESSAGE = 99;
        public const int SM_MERCHANTDLGCLOSE = 644;
        public const int SM_SENDGOODSLIST = 645;
        public const int SM_SENDUSERSELL = 646;
        public const int SM_SENDBUYPRICE = 647;
        public const int SM_USERSELLITEM_OK = 648;
        public const int SM_USERSELLITEM_FAIL = 649;
        public const int SM_BUYITEM_SUCCESS = 650;
        public const int SM_BUYITEM_FAIL = 651;
        public const int SM_SENDDETAILGOODSLIST = 652;
        public const int SM_GOLDCHANGED = 653;
        public const int SM_CHANGELIGHT = 654;
        public const int SM_LAMPCHANGEDURA = 655;
        public const int SM_CHANGENAMECOLOR = 656;
        public const int SM_CHARSTATUSCHANGED = 657;
        public const int SM_SENDNOTICE = 658;
        public const int SM_GROUPMODECHANGED = 659;
        public const int SM_CREATEGROUP_OK = 660;
        public const int SM_CREATEGROUP_FAIL = 661;
        public const int SM_GROUPADDMEM_OK = 662;
        public const int SM_GROUPDELMEM_OK = 663;
        public const int SM_GROUPADDMEM_FAIL = 664;
        public const int SM_GROUPDELMEM_FAIL = 665;
        public const int SM_GROUPCANCEL = 666;
        public const int SM_GROUPMEMBERS = 667;
        public const int SM_SENDUSERREPAIR = 668;
        public const int SM_USERREPAIRITEM_OK = 669;
        public const int SM_USERREPAIRITEM_FAIL = 670;
        public const int SM_SENDREPAIRCOST = 671;
        public const int SM_DEALMENU = 673;
        public const int SM_DEALTRY_FAIL = 674;
        public const int SM_DEALADDITEM_OK = 675;
        public const int SM_DEALADDITEM_FAIL = 676;
        public const int SM_DEALDELITEM_OK = 677;
        public const int SM_DEALDELITEM_FAIL = 678;
        public const int SM_DEALCANCEL = 681;
        public const int SM_DEALREMOTEADDITEM = 682;
        public const int SM_DEALREMOTEDELITEM = 683;
        public const int SM_DEALCHGGOLD_OK = 684;
        public const int SM_DEALCHGGOLD_FAIL = 685;
        public const int SM_DEALREMOTECHGGOLD = 686;
        public const int SM_DEALSUCCESS = 687;
        public const int SM_SENDUSERSTORAGEITEM = 700;
        public const int SM_STORAGE_OK = 701;
        public const int SM_STORAGE_FULL = 702;
        public const int SM_STORAGE_FAIL = 703;
        public const int SM_SAVEITEMLIST = 704;
        public const int SM_TAKEBACKSTORAGEITEM_OK = 705;
        public const int SM_TAKEBACKSTORAGEITEM_FAIL = 706;
        public const int SM_TAKEBACKSTORAGEITEM_FULLBAG = 707;
        public const int SM_AREASTATE = 766;
        public const int SM_MYSTATUS = 708;
        public const int SM_DELITEMS = 709;
        public const int SM_READMINIMAP_OK = 710;
        public const int SM_READMINIMAP_FAIL = 711;
        public const int SM_SENDUSERMAKEDRUGITEMLIST = 712;
        public const int SM_MAKEDRUG_SUCCESS = 713;
        public const int SM_MAKEDRUG_FAIL = 714;
        public const int SM_716 = 716;
        public const int SM_CHANGEGUILDNAME = 750;
        public const int SM_SENDUSERSTATE = 751;
        public const int SM_SUBABILITY = 752;
        public const int SM_OPENGUILDDLG = 753;
        public const int SM_OPENGUILDDLG_FAIL = 754;
        public const int SM_SENDGUILDMEMBERLIST = 756;
        public const int SM_GUILDADDMEMBER_OK = 757;
        public const int SM_GUILDADDMEMBER_FAIL = 758;
        public const int SM_GUILDDELMEMBER_OK = 759;
        public const int SM_GUILDDELMEMBER_FAIL = 760;
        public const int SM_GUILDRANKUPDATE_FAIL = 761;
        public const int SM_BUILDGUILD_OK = 762;
        public const int SM_BUILDGUILD_FAIL = 763;
        public const int SM_DONATE_OK = 764;
        public const int SM_DONATE_FAIL = 765;
        public const int SM_MENU_OK = 767;
        public const int SM_GUILDMAKEALLY_OK = 768;
        public const int SM_GUILDMAKEALLY_FAIL = 769;
        public const int SM_GUILDBREAKALLY_OK = 770;
        public const int SM_GUILDBREAKALLY_FAIL = 771;
        public const int SM_DLGMSG = 772;
        public const int SM_SPACEMOVE_HIDE = 800;
        public const int SM_SPACEMOVE_SHOW = 801;
        public const int SM_RECONNECT = 802;
        public const int SM_GHOST = 803;
        public const int SM_SHOWEVENT = 804;
        public const int SM_HIDEEVENT = 805;
        public const int SM_SPACEMOVE_HIDE2 = 806;
        public const int SM_SPACEMOVE_SHOW2 = 807;
        public const int SM_TIMECHECK_MSG = 810;
        public const int SM_ADJUST_BONUS = 811;
        public const int SM_OPENHEALTH = 1100;
        public const int SM_CLOSEHEALTH = 1101;
        public const int SM_CHANGEFACE = 1104;
        public const int SM_BREAKWEAPON = 1102;
        public const int SM_INSTANCEHEALGUAGE = 1103;
        public const int SM_VERSION_FAIL = 1106;
        public const int SM_ITEMUPDATE = 1500;
        public const int SM_MONSTERSAY = 1501;
        public const int SM_EXCHGTAKEON_OK = 65023;
        public const int SM_EXCHGTAKEON_FAIL = 65024;
        public const int SM_TEST = 65037;
        public const int SM_ACTION_MIN = 65070;
        public const int SM_ACTION_MAX = 65071;
        public const int SM_ACTION2_MIN = 65072;
        public const int SM_ACTION2_MAX = 65073;
        public const int CM_SERVERREGINFO = 65074;
        public const int CM_GETGAMELIST = 5001;
        public const int SM_SENDGAMELIST = 5002;
        public const int CM_CHECKCLIENT_RES = 41905;
        public const int SM_SMUGGLE_SUCESS = 41901;
        public const int CM_SMUGGLE = 41902;
        public const int CM_SMUGGLE_SUCESS = 41903;
        public const int SM_SMUGGLE = 41900; // 夹带数据
        /// <summary>
        /// 找回密码
        /// </summary>
        public const int CM_GETBACKPASSWORD = 5003;
        /// <summary>
        /// 找回密码成功
        /// </summary>
        public const int SM_GETBACKPASSWD_SUCCESS = 5005;
        /// <summary>
        /// 找回密码失败
        /// </summary>
        public const int SM_GETBACKPASSWD_FAIL = 5006;
        /// <summary>
        /// 发送服务器配置信息
        /// </summary>
        public const int SM_SERVERCONFIG = 11029;
        public const int SM_GAMEGOLDNAME = 5008;
        public const int SM_PASSWORD = 5009;
        public const int SM_HORSERUN = 5010;
        public const int UNKNOWMSG = 199;
        public const int SS_OPENSESSION = 100;
        public const int SS_CLOSESESSION = 101;
        public const int SS_KEEPALIVE = 104;
        public const int SS_KICKUSER = 111;
        public const int SS_SERVERLOAD = 113;
        public const int ISM_ACCOUNTEXPIRED = 114;
        /// <summary>
        /// 查询账号剩余游戏时间
        /// </summary>
        public const int ISM_QUERYACCOUNTEXPIRETIME = 115;
        public const int ISM_QUERYPLAYTIME = 116;
        public const int SS_200 = 200;
        public const int SS_201 = 201;
        public const int SS_202 = 202;
        public const int SS_203 = 203;
        public const int SS_204 = 204;
        public const int SS_205 = 205;
        public const int SS_206 = 206;
        public const int SS_207 = 207;
        public const int SS_208 = 208;
        public const int SS_209 = 209;
        public const int SS_210 = 210;
        public const int SS_211 = 211;
        public const int SS_212 = 212;
        public const int SS_213 = 213;
        public const int SS_214 = 214;
        public const int SS_WHISPER = 299;
        /// <summary>
        /// 同步服务器信息
        /// </summary>
        public const int SS_SERVERINFO = 103;
        /// <summary>
        /// 客户端退出游戏
        /// </summary>
        public const int SS_SOFTOUTSESSION = 102;
        /// <summary>
        /// 减少或更新账号游戏时间
        /// </summary>
        public const int ISM_GAMETIMEOFTIMECARDUSER = 112;
        public const int ISM_CHECKTIMEACCOUNT = 116;
        public const int SS_LOGINCOST = 30002;
        public const int DBR_FAIL = 2000;
        public const int DB_LOADHUMANRCD = 100;
        public const int DB_SAVEHUMANRCD = 101;
        public const int DB_SAVEHUMANRCDEX = 102;
        public const int DBR_LOADHUMANRCD = 1100;
        public const int DBR_SAVEHUMANRCD = 1102;

        /// <summary>
        /// 读取拍卖行数据
        /// </summary>
        public const byte DB_LOADMARKET = 100;
        /// <summary>
        /// 保存拍卖行数据
        /// </summary>
        public const byte DB_SAVEMARKET = 101;
        /// <summary>
        /// 搜索拍卖行数据
        /// </summary>
        public const byte DB_SEARCHMARKET = 102;
        /// <summary>
        /// 搜索拍卖行数据成功
        /// </summary>
        public const byte DB_SEARCHMARKETSUCCESS = 103;
        /// <summary>
        /// 搜索拍卖行数据失败
        /// </summary>
        public const byte DB_SRARCHMARKETFAIL = 104;
        /// <summary>
        /// 读取拍卖行数据成功
        /// </summary>
        public const byte DB_LOADMARKETSUCCESS = 105;
        /// <summary>
        /// 读取拍卖行数据失败
        /// </summary>
        public const byte DB_LOADMARKETFAIL = 106;
        /// <summary>
        /// 保存拍卖行数据成功
        /// </summary>
        public const byte DB_SAVEMARKETSUCCESS = 107;
        /// <summary>
        /// 保存拍卖行数据失败
        /// </summary>
        public const byte DB_SAVEMARKETFAIL = 108;
        /// <summary>
        /// 获取用户拍卖行数据
        /// </summary>
        public const byte DB_LOADUSERMARKET = 109;
        /// <summary>
        /// 获取用户拍卖行数据成功
        /// </summary>
        public const byte DB_LOADUSERMARKETSUCCESS = 110;
        /// <summary>
        /// 获取用户拍卖行数据失败
        /// </summary>
        public const byte DB_LOADUSERMARKETFAIL = 111;

        public const int SM_RUNGATELOGOUT = 599;
        public const int SM_PLAYERCONFIG = 560;
        public const int GM_TEST = 20;
        public const int GM_STOP = 21;
        public const int CM_42HIT = 42;

        public const int CM_QUERYVAL = 1065;
        public const int CM_PASSWORD = 2001;
        public const int CM_CHGPASSWORD = 2002;
        public const int CM_SETPASSWORD = 2004;
        public const int CM_HORSERUN = 3035;
        public const int CM_CRSHIT = 3036;
        public const int CM_3037 = 3037;
        public const int CM_TWINHIT = 3038;
        public const int CM_QUERYUSERSET = 3040;
        public const int SM_PLAYDICE = 8001;
        public const int SM_PASSWORDSTATUS = 8002;
        public const int SM_NEEDPASSWORD = 8003;
        public const int SM_GETREGINFO = 8004;

        public const int RM_TURN = 10001;
        public const int RM_WALK = 10002;
        public const int RM_HORSERUN = 50003;
        public const int RM_RUN = 10003;
        public const int RM_HIT = 10004;
        public const int RM_BIGHIT = 10006;
        public const int RM_HEAVYHIT = 10007;
        public const int RM_SPELL = 10008;
        public const int RM_SPELL2 = 10009;
        public const int RM_MOVEFAIL = 10010;
        public const int RM_LONGHIT = 10011;
        public const int RM_WIDEHIT = 10012;
        public const int RM_FIREHIT = 10014;
        public const int RM_CRSHIT = 10015;
        public const int RM_DEATH = 10021;
        public const int RM_SKELETON = 10024;
        /// <summary>
        /// 发送登录消息
        /// </summary>
        public const int RM_LOGON = 10050;
        public const int RM_ABILITY = 10051;
        public const int RM_HEALTHSPELLCHANGED = 10052;
        public const int RM_DAYCHANGING = 10053;
        public const int RM_REFMESSAGE = 10101;
        public const int RM_WEIGHTCHANGED = 10115;
        public const int RM_FEATURECHANGED = 10116;
        public const int RM_BUTCH = 10119;
        public const int RM_MAGICFIRE = 10120;
        public const int RM_MAGICFIREFAIL = 10121;
        public const int RM_SENDMYMAGIC = 10122;
        public const int RM_MAGIC_LVEXP = 10123;
        public const int RM_DURACHANGE = 10125;
        public const int RM_MERCHANTDLGCLOSE = 10127;
        public const int RM_SENDGOODSLIST = 10128;
        public const int RM_SENDUSERSELL = 10129;
        public const int RM_SENDBUYPRICE = 10130;
        public const int RM_USERSELLITEM_OK = 10131;
        public const int RM_USERSELLITEM_FAIL = 10132;
        public const int RM_BUYITEM_SUCCESS = 10133;
        public const int RM_BUYITEM_FAIL = 10134;
        public const int RM_SENDDETAILGOODSLIST = 10135;
        public const int RM_GOLDCHANGED = 10136;
        public const int RM_CHANGELIGHT = 10137;
        public const int RM_LAMPCHANGEDURA = 10138;
        public const int RM_CHARSTATUSCHANGED = 10139;
        public const int RM_GROUPCANCEL = 10140;
        public const int RM_SENDUSERREPAIR = 10141;
        public const int RM_SENDUSERSREPAIR = 50142;
        public const int RM_SENDREPAIRCOST = 10142;
        public const int RM_USERREPAIRITEM_OK = 10143;
        public const int RM_USERREPAIRITEM_FAIL = 10144;
        public const int RM_USERSTORAGEITEM = 10146;
        public const int RM_USERGETBACKITEM = 10147;
        public const int RM_SENDDELITEMLIST = 10148;
        public const int RM_USERMAKEDRUGITEMLIST = 10149;
        public const int RM_MAKEDRUG_SUCCESS = 10150;
        public const int RM_MAKEDRUG_FAIL = 10151;
        public const int RM_ALIVE = 10153;
        public const int RM_RANDOMSPACEMOVE = 10155;
        public const int RM_DIGUP = 10200;
        public const int RM_DIGDOWN = 10201;
        public const int RM_FLYAXE = 10202;
        public const int RM_LIGHTING = 10204;
        public const int RM_10205 = 10205;
        public const int RM_CHANGEGUILDNAME = 10301;
        public const int RM_SUBABILITY = 10302;
        public const int RM_BUILDGUILD_OK = 10303;
        public const int RM_BUILDGUILD_FAIL = 10304;
        public const int RM_DONATE_OK = 10305;
        public const int RM_DONATE_FAIL = 10306;
        public const int RM_MENU_OK = 10309;
        public const int RM_RECONNECTION = 10332;
        public const int RM_HIDEEVENT = 10333;
        public const int RM_SHOWEVENT = 10334;
        public const int RM_10401 = 10401;
        public const int RM_OPENHEALTH = 10410;
        public const int RM_CLOSEHEALTH = 10411;
        /// <summary>
        /// 升级武器失败
        /// </summary>
        public const int RM_BREAKWEAPON = 10413;
        public const int RM_10414 = 10414;
        public const int RM_CHANGEFACE = 10415;
        public const int RM_PASSWORD = 10416;
        public const int RM_PLAYDICE = 10500;
        public const int RM_HEAR = 11001;
        public const int RM_WHISPER = 11002;
        public const int RM_CRY = 11003;
        public const int RM_SYSMESSAGE = 11004;
        public const int RM_GROUPMESSAGE = 11005;
        public const int RM_SYSMESSAGE2 = 11006;
        public const int RM_GUILDMESSAGE = 11007;
        public const int RM_SYSMESSAGE3 = 11008;
        public const int RM_MERCHANTSAY = 11009;
        public const int RM_ZEN_BEE = 8020;
        public const int RM_DELAYMAGIC = 8021;
        public const int RM_STRUCK = 8018;
        public const int RM_MAGSTRUCK_MINE = 8030;
        public const int RM_MAGHEALING = 8034;
        public const int RM_POISON = 8037;
        public const int RM_DOOPENHEALTH = 8040;
        public const int RM_SPACEMOVE_FIRE2 = 8042;
        public const int RM_DELAYPUSHED = 8043;
        public const int RM_MAGSTRUCK = 8044;
        public const int RM_TRANSPARENT = 8045;
        public const int RM_DOOROPEN = 8046;
        public const int RM_DOORCLOSE = 8047;
        public const int RM_DISAPPEAR = 8061;
        public const int RM_SPACEMOVE_FIRE = 8062;
        public const int RM_SENDUSEITEMS = 8074;
        public const int RM_WINEXP = 8075;
        public const int RM_ADJUST_BONUS = 8078;
        public const int RM_ITEMSHOW = 8082;
        public const int RM_GAMEGOLDCHANGED = 8084;
        public const int RM_ITEMHIDE = 8085;
        public const int RM_LEVELUP = 8086;
        public const int RM_CHANGENAMECOLOR = 8090;
        /// <summary>
        /// 往后退（野蛮冲转 抗拒火环）
        /// </summary>
        public const int RM_PUSH = 8092;
        public const int RM_CLEAROBJECTS = 8097;
        public const int RM_CHANGEMAP = 8098;
        public const int RM_SPACEMOVE_SHOW2 = 8099;
        public const int RM_SPACEMOVE_SHOW = 8100;
        public const int RM_USERNAME = 8101;
        public const int RM_MYSTATUS = 8102;
        public const int RM_STRUCK_MAG = 8103;
        public const int RM_RUSH = 8104;
        public const int RM_RUSHKUNG = 8105;
        public const int RM_PASSWORDSTATUS = 8106;
        public const int RM_POWERHIT = 8107;
        public const int RM_41 = 9041;
        public const int RM_TWINHIT = 9042;
        public const int RM_43 = 9043;
        public const int RM_MOVEMESSAGE = 10099;

        public const int ISM_GROUPSERVERHEART = 100;
        public const int ISM_USERSERVERCHANGE = 200;
        public const int ISM_USERLOGON = 201;
        public const int ISM_USERLOGOUT = 202;
        public const int ISM_WHISPER = 203;
        public const int ISM_SYSOPMSG = 204;
        public const int ISM_ADDGUILD = 205;
        public const int ISM_DELGUILD = 206;
        public const int ISM_RELOADGUILD = 207;
        public const int ISM_GUILDMSG = 208;
        public const int ISM_CHATPROHIBITION = 209;
        public const int ISM_CHATPROHIBITIONCANCEL = 210;
        public const int ISM_CHANGECASTLEOWNER = 211;
        public const int ISM_RELOADCASTLEINFO = 212;
        public const int ISM_RELOADADMIN = 213;

        public const int ISM_FRIEND_INFO = 214;
        public const int ISM_FRIEND_DELETE = 215;
        public const int ISM_FRIEND_OPEN = 216;
        public const int ISM_FRIEND_CLOSE = 217;
        public const int ISM_FRIEND_RESULT = 218;
        public const int ISM_TAG_SEND = 219;
        public const int ISM_TAG_RESULT = 220;
        public const int ISM_USER_INFO = 221;
        public const int ISM_CHANGESERVERRECIEVEOK = 222;
        public const int ISM_RELOADCHATLOG = 223;
        public const int ISM_MARKETOPEN = 224;
        public const int ISM_MARKETCLOSE = 225;
        public const int ISM_LM_DELETE = 226;
        public const int ISM_RELOADMAKEITEMLIST = 227;
        public const int ISM_GUILDMEMBER_RECALL = 228;
        public const int ISM_RELOADGUILDAGIT = 229;
        public const int ISM_LM_WHISPER = 230;
        public const int ISM_GMWHISPER = 231;
        public const int ISM_LM_LOGIN = 232;
        public const int ISM_LM_LOGOUT = 233;
        public const int ISM_REQUEST_RECALL = 234;
        public const int ISM_RECALL = 235;
        public const int ISM_LM_LOGIN_REPLY = 236;
        public const int ISM_LM_KILLED_MSG = 237;
        public const int ISM_REQUEST_LOVERRECALL = 238;
        public const int ISM_STANDARDTICKREQ = 239;
        public const int ISM_STANDARDTICK = 240;
        public const int ISM_GUILDWAR = 241;
        /// <summary>
        /// 发送跨服组队消息
        /// </summary>
        public const int ISM_GRUOPMESSAGE = 242;

        // ==============================元宝寄售系统==========================
        public const int RM_SENDDEALOFFFORM = 23000;
        // 打开出售物品窗口
        public const int SM_SENDDEALOFFFORM = 23001;
        // 打开出售物品窗口
        public const int CM_SELLOFFADDITEM = 23002;
        // 客户端往出售物品窗口里加物品
        public const int SM_SELLOFFADDITEM_OK = 23003;
        // 客户端往出售物品窗口里加物品 成功
        public const int RM_SELLOFFADDITEM_OK = 23004;
        public const int SM_SellOffADDITEM_FAIL = 23005;
        // 客户端往出售物品窗口里加物品 失败
        public const int RM_SellOffADDITEM_FAIL = 23006;
        public const int CM_SELLOFFDELITEM = 23007;
        // 客户端删除出售物品窗里的物品
        public const int SM_SELLOFFDELITEM_OK = 23008;
        // 客户端删除出售物品窗里的物品 成功
        public const int RM_SELLOFFDELITEM_OK = 23009;
        public const int SM_SELLOFFDELITEM_FAIL = 23010;
        // 客户端删除出售物品窗里的物品 失败
        public const int RM_SELLOFFDELITEM_FAIL = 23011;
        public const int CM_SELLOFFCANCEL = 23012;
        // 客户端取消元宝寄售
        public const int RM_SELLOFFCANCEL = 23013;
        // 元宝寄售取消出售
        public const int SM_SellOffCANCEL = 23014;
        // 元宝寄售取消出售
        public const int CM_SELLOFFEND = 23015;
        // 客户端元宝寄售结束
        public const int SM_SELLOFFEND_OK = 23016;
        // 客户端元宝寄售结束 成功
        public const int RM_SELLOFFEND_OK = 23017;
        public const int SM_SELLOFFEND_FAIL = 23018;
        // 客户端元宝寄售结束 失败
        public const int RM_SELLOFFEND_FAIL = 23019;
        public const int RM_QUERYYBSELL = 23020;
        // 查询正在出售的物品
        public const int SM_QUERYYBSELL = 23021;
        // 查询正在出售的物品
        public const int RM_QUERYYBDEAL = 23022;
        // 查询可以的购买物品
        public const int SM_QUERYYBDEAL = 23023;
        // 查询可以的购买物品
        public const int CM_CANCELSELLOFFITEMING = 23024;
        // 取消正在寄售的物品 (出售人)
        // SM_CANCELSELLOFFITEMING_OK =23018;//取消正在寄售的物品 成功
        public const int CM_SELLOFFBUYCANCEL = 23025;
        // 取消寄售 物品购买 (购买人)
        public const int CM_SELLOFFBUY = 23026;
        // 确定购买寄售物品 
        public const int SM_SELLOFFBUY_OK = 23027;
        // 购买成功
        public const int RM_SELLOFFBUY_OK = 23028;

        public const int RM_MARKET_LIST = 11015;
        public const int RM_MARKET_RESULT = 11016;

        /// <summary>
        /// 更新自身视野对象
        /// </summary>
        public const int RM_UPDATEVIEWRANGE = 60000;
        /// <summary>
        /// 玩家杀死怪物触发消息
        /// </summary>
        public const int RM_PLAYERKILLMONSTER = 60001;

        public static CommandMessage MakeMessage(int msg, int recog, int param, int tag, int series)
        {
            CommandMessage result = new CommandMessage
            {
                Ident = (ushort)msg,
                Param = (ushort)param,
                Tag = (ushort)tag,
                Series = (ushort)series,
                Recog = recog
            };
            return result;
        }
    }
}