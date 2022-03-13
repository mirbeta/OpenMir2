/*
 Navicat Premium Data Transfer

 Source Server         : 10.10.0.199
 Source Server Type    : MySQL
 Source Server Version : 50735
 Source Host           : 10.10.0.199:3306
 Source Schema         : Mir2

 Target Server Type    : MySQL
 Target Server Version : 50735
 File Encoding         : 65001

 Date: 13/03/2022 22:00:53
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for TBL_ACCOUNT
-- ----------------------------
DROP TABLE IF EXISTS `TBL_ACCOUNT`;
CREATE TABLE `TBL_ACCOUNT`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_LOGINID` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_PASSWORD` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_USERNAME` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_ACTIONTICK` int(5) NULL DEFAULT NULL,
  `FLD_ERRORCOUNT` int(5) NULL DEFAULT NULL,
  `FLD_SSNO` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_PHONE` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_QUIZ1` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_ANSWER1` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_EMAIL` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_QUIZ2` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_ANSWER2` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_BIRTHDAY` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_MOBILEPHONE` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_DELETED` tinyint(2) NULL DEFAULT NULL,
  `FLD_CREATEDATE` datetime NULL DEFAULT NULL,
  `FLD_LASTUPDATE` datetime NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '账号' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for TBL_BONUSABILITY
-- ----------------------------
DROP TABLE IF EXISTS `TBL_BONUSABILITY`;
CREATE TABLE `TBL_BONUSABILITY`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_CHARNAME` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_AC` int(11) NULL DEFAULT NULL,
  `FLD_MAC` int(11) NULL DEFAULT NULL,
  `FLD_DC` int(11) NULL DEFAULT NULL,
  `FLD_MC` int(11) NULL DEFAULT NULL,
  `FLD_SC` int(11) NULL DEFAULT NULL,
  `FLD_HP` int(11) NULL DEFAULT NULL,
  `FLD_MP` int(11) NULL DEFAULT NULL,
  `FLD_HIT` int(11) NULL DEFAULT NULL,
  `FLD_SPEED` int(11) NULL DEFAULT NULL,
  `FLD_RESERVED` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for TBL_CHARACTER
-- ----------------------------
DROP TABLE IF EXISTS `TBL_CHARACTER`;
CREATE TABLE `TBL_CHARACTER`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_ServerNum` int(11) NOT NULL DEFAULT 0 COMMENT '服务器编号',
  `FLD_LoginID` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '登陆账号',
  `FLD_CharName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '角色名称',
  `FLD_MapName` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '所在地图名称',
  `FLD_CX` int(11) NULL DEFAULT 0 COMMENT '所在地图坐标X',
  `FLD_CY` int(11) NULL DEFAULT 0 COMMENT '所在地图坐标Y',
  `FLD_Level` int(11) NULL DEFAULT 0 COMMENT '等级',
  `FLD_Dir` tinyint(2) NULL DEFAULT 0 COMMENT '所在方向',
  `FLD_Hair` tinyint(1) NULL DEFAULT 0 COMMENT '发型',
  `FLD_Sex` tinyint(1) NULL DEFAULT 0 COMMENT '性别（0:男 1:女）',
  `FLD_Job` tinyint(1) NULL DEFAULT 0 COMMENT '职业（0:战士 1:法师 2:道士）',
  `FLD_Gold` int(11) NULL DEFAULT 0 COMMENT '金币数',
  `FLD_GamePoint` int(11) NULL DEFAULT 0 COMMENT '金刚石',
  `FLD_HomeMap` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '0' COMMENT '回城地图',
  `FLD_HomeX` int(5) NULL DEFAULT 0 COMMENT '回城坐标X',
  `FLD_HomeY` int(5) NULL DEFAULT 0 COMMENT '回城坐标Y',
  `FLD_PkPoint` int(5) NULL DEFAULT 0 COMMENT 'PK值',
  `FLD_ReLevel` tinyint(2) NULL DEFAULT 0 COMMENT '转生次数',
  `FLD_AttatckMode` tinyint(2) NULL DEFAULT 0 COMMENT '攻击模式',
  `FLD_FightZoneDieCount` tinyint(3) NULL DEFAULT 0 COMMENT '行会战争死亡次数',
  `FLD_BodyLuck` double(10, 3) NULL DEFAULT 0.000 COMMENT '幸运值',
  `FLD_IncHealth` tinyint(3) NULL DEFAULT 0,
  `FLD_IncSpell` tinyint(3) NULL DEFAULT 0,
  `FLD_IncHealing` tinyint(3) NULL DEFAULT 0,
  `FLD_CreditPoint` tinyint(3) NULL DEFAULT 0 COMMENT '声望点数',
  `FLD_BonusPoint` int(5) NULL DEFAULT 0 COMMENT '奖励点数',
  `FLD_HungerStatus` int(5) NULL DEFAULT 0 COMMENT '状态',
  `FLD_PayMentPoint` int(5) NULL DEFAULT 0,
  `FLD_LockLogon` tinyint(1) NULL DEFAULT 0 COMMENT '是否锁定登陆',
  `FLD_MarryCount` int(5) NULL DEFAULT 0 COMMENT '结婚次数',
  `FLD_AllowGroup` tinyint(1) NULL DEFAULT 0 COMMENT '是否允许组队',
  `FLD_AllowGroupReCall` tinyint(1) NULL DEFAULT 0 COMMENT '是否允许组队传送',
  `FLD_GroupRcallTime` int(5) NULL DEFAULT 0 COMMENT '组队传送间隔',
  `FLD_AllowGuildReCall` tinyint(1) NULL DEFAULT 0 COMMENT '是否允许行会传送',
  `FLD_IsMaster` tinyint(1) NULL DEFAULT 0 COMMENT '是否收徒',
  `FLD_MasterName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '师傅名称',
  `FLD_DearName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '配偶名称',
  `FLD_StoragePwd` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '仓库密码',
  `FLD_Deleted` tinyint(1) NULL DEFAULT NULL COMMENT '是否删除',
  `FLD_CREATEDATE` datetime NULL DEFAULT NULL COMMENT '创建日期',
  `FLD_LASTUPDATE` datetime NULL DEFAULT NULL COMMENT '修改日期',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '玩家' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for TBL_CHARACTER_ABLITY
-- ----------------------------
DROP TABLE IF EXISTS `TBL_CHARACTER_ABLITY`;
CREATE TABLE `TBL_CHARACTER_ABLITY`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_CharId` bigint(20) NULL DEFAULT NULL,
  `FLD_CharName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '角色名称',
  `FLD_Level` int(11) NULL DEFAULT NULL COMMENT '当前等级',
  `FLD_Ac` int(11) NULL DEFAULT NULL COMMENT '攻击防御',
  `FLD_Mac` int(11) NULL DEFAULT NULL COMMENT '魔法防御',
  `FLD_Dc` int(11) NULL DEFAULT NULL COMMENT '物理攻击力',
  `FLD_Mc` int(11) NULL DEFAULT NULL COMMENT '魔法攻击力',
  `FLD_Sc` int(11) NULL DEFAULT NULL COMMENT '道术攻击力',
  `FLD_Hp` int(11) NULL DEFAULT NULL COMMENT '当前HP',
  `FLD_Mp` int(11) NULL DEFAULT NULL COMMENT '当前MP',
  `FLD_MaxHP` int(11) NULL DEFAULT NULL COMMENT '最大HP',
  `FLD_MAxMP` int(11) NULL DEFAULT NULL COMMENT '最大MP',
  `FLD_Exp` int(11) NULL DEFAULT NULL COMMENT '当前经验',
  `FLD_MaxExp` int(11) NULL DEFAULT NULL COMMENT '升级经验',
  `FLD_Weight` int(11) NULL DEFAULT NULL COMMENT '当前包裹重量',
  `FLD_MaxWeight` int(11) NULL DEFAULT NULL COMMENT '最大包裹重量',
  `FLD_WearWeight` int(11) NULL DEFAULT NULL COMMENT '当前腕力',
  `FLD_MaxWearWeight` int(11) NULL DEFAULT NULL COMMENT '最大腕力',
  `FLD_HandWeight` int(11) NULL DEFAULT NULL COMMENT '当前负重',
  `FLD_MaxHandWeight` int(11) NULL DEFAULT NULL COMMENT '最大负重',
  `FLD_ModifyTime` datetime NULL DEFAULT NULL COMMENT '最后修改时间',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for TBL_CHARACTER_MAGIC
-- ----------------------------
DROP TABLE IF EXISTS `TBL_CHARACTER_MAGIC`;
CREATE TABLE `TBL_CHARACTER_MAGIC`  (
  `FLD_CHARNAME` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_MAGICID` int(11) NULL DEFAULT NULL,
  `FLD_LEVEL` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_USEKEY` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_CURRTRAIN` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '技能' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for TBL_CHARACTER_NAKEDABILITY
-- ----------------------------
DROP TABLE IF EXISTS `TBL_CHARACTER_NAKEDABILITY`;
CREATE TABLE `TBL_CHARACTER_NAKEDABILITY`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_CharId` bigint(20) NULL DEFAULT NULL,
  `FLD_DC` int(255) NULL DEFAULT NULL,
  `FLD_MC` int(255) NULL DEFAULT NULL,
  `FLD_SC` int(255) NULL DEFAULT NULL,
  `FLD_AC` int(255) NULL DEFAULT NULL,
  `FLD_MAC` int(255) NULL DEFAULT NULL,
  `FLD_HP` int(11) NULL DEFAULT NULL,
  `FLD_MP` int(255) NULL DEFAULT NULL,
  `FLD_HIT` int(255) NULL DEFAULT NULL,
  `FLD_SPEED` int(255) NULL DEFAULT NULL,
  `FLD_X2` int(255) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for TBL_CHARACTER_STATUS
-- ----------------------------
DROP TABLE IF EXISTS `TBL_CHARACTER_STATUS`;
CREATE TABLE `TBL_CHARACTER_STATUS`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_CharId` bigint(20) NULL DEFAULT NULL,
  `FLD_CharName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_Status` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for TBL_GOLDSALES
-- ----------------------------
DROP TABLE IF EXISTS `TBL_GOLDSALES`;
CREATE TABLE `TBL_GOLDSALES`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `DealCharName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `BuyCharName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `SellDateTime` datetime NULL DEFAULT NULL,
  `State` tinyint(1) NULL DEFAULT NULL,
  `SellGold` int(11) NULL DEFAULT NULL,
  `UseItems` json NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '金币寄售' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for TBL_HUMINDEX
-- ----------------------------
DROP TABLE IF EXISTS `TBL_HUMINDEX`;
CREATE TABLE `TBL_HUMINDEX`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_Account` int(11) NULL DEFAULT NULL,
  `FLD_sCharName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_IsSelected` tinyint(1) NULL DEFAULT NULL,
  `FLD_IsDeleted` tinyint(1) NULL DEFAULT NULL,
  `FLD_ModDate` datetime NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for TBL_HUMRECORD
-- ----------------------------
DROP TABLE IF EXISTS `TBL_HUMRECORD`;
CREATE TABLE `TBL_HUMRECORD`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_Account` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_CharName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_SelectID` tinyint(2) NULL DEFAULT NULL,
  `FLD_IsDeleted` tinyint(1) NULL DEFAULT NULL,
  `FLD_CreateDate` datetime NULL DEFAULT NULL,
  `FLD_ModifyDate` datetime NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for TBL_ITEM
-- ----------------------------
DROP TABLE IF EXISTS `TBL_ITEM`;
CREATE TABLE `TBL_ITEM`  (
  `FLD_CHARID` bigint(20) NULL DEFAULT NULL COMMENT '角色ID',
  `FLD_CHARNAME` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '角色名称',
  `FLD_POSITION` int(10) NULL DEFAULT 0,
  `FLD_MAKEINDEX` int(20) NULL DEFAULT 0 COMMENT '物品编号',
  `FLD_STDINDEX` int(10) NULL DEFAULT 0,
  `FLD_DURA` int(10) NULL DEFAULT 0 COMMENT '当前持久',
  `FLD_DURAMAX` int(10) NULL DEFAULT 0 COMMENT '最大持久',
  `FLD_VALUE0` int(10) NULL DEFAULT 0,
  `FLD_VALUE1` int(10) NULL DEFAULT 0,
  `FLD_VALUE2` int(10) NULL DEFAULT 0,
  `FLD_VALUE3` int(10) NULL DEFAULT 0,
  `FLD_VALUE4` int(10) NULL DEFAULT 0,
  `FLD_VALUE5` int(10) NULL DEFAULT 0,
  `FLD_VALUE6` int(10) NULL DEFAULT 0,
  `FLD_VALUE7` int(10) NULL DEFAULT 0,
  `FLD_VALUE8` int(10) NULL DEFAULT 0,
  `FLD_VALUE9` int(10) NULL DEFAULT 0,
  `FLD_VALUE10` int(10) NULL DEFAULT 0,
  `FLD_VALUE11` int(10) NULL DEFAULT 0,
  `FLD_VALUE12` int(10) NULL DEFAULT 0,
  `FLD_VALUE13` int(10) NULL DEFAULT 0
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '包裹' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for TBL_Magics
-- ----------------------------
DROP TABLE IF EXISTS `TBL_Magics`;
CREATE TABLE `TBL_Magics`  (
  `Idx` int(11) NOT NULL AUTO_INCREMENT,
  `MagID` int(11) NULL DEFAULT NULL,
  `MagName` varchar(25) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `EffectType` int(11) NULL DEFAULT NULL,
  `Effect` int(11) NULL DEFAULT NULL,
  `Spell` int(11) NULL DEFAULT NULL,
  `Power` int(11) NULL DEFAULT NULL,
  `MaxPower` int(11) NULL DEFAULT NULL,
  `DefSpell` int(11) NULL DEFAULT NULL,
  `DefPower` int(11) NULL DEFAULT NULL,
  `DefMaxPower` int(11) NULL DEFAULT NULL,
  `Job` int(11) NULL DEFAULT NULL,
  `NeedL1` int(11) NULL DEFAULT NULL,
  `L1Train` int(11) NULL DEFAULT NULL,
  `NeedL2` int(11) NULL DEFAULT NULL,
  `L2Train` int(11) NULL DEFAULT NULL,
  `NeedL3` int(11) NULL DEFAULT NULL,
  `L3Train` int(11) NULL DEFAULT NULL,
  `Delay` int(11) NULL DEFAULT NULL,
  `Descr` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Idx`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 207 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for TBL_Monsters
-- ----------------------------
DROP TABLE IF EXISTS `TBL_Monsters`;
CREATE TABLE `TBL_Monsters`  (
  `Idx` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(24) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Race` int(11) NULL DEFAULT NULL,
  `RaceImg` int(11) NULL DEFAULT NULL,
  `Appr` int(11) NULL DEFAULT NULL,
  `Lvl` int(11) NULL DEFAULT NULL,
  `Undead` int(11) NULL DEFAULT NULL,
  `CoolEye` int(11) NULL DEFAULT NULL,
  `Exp` int(11) NULL DEFAULT NULL,
  `HP` int(11) NULL DEFAULT NULL,
  `MP` int(11) NULL DEFAULT NULL,
  `AC` int(11) NULL DEFAULT NULL,
  `MAC` int(11) NULL DEFAULT NULL,
  `DC` int(11) NULL DEFAULT NULL,
  `DCMAX` int(11) NULL DEFAULT NULL,
  `MC` int(11) NULL DEFAULT NULL,
  `SC` int(11) NULL DEFAULT NULL,
  `SPEED` int(11) NULL DEFAULT NULL,
  `HIT` int(11) NULL DEFAULT NULL,
  `WALK_SPD` int(11) NULL DEFAULT NULL,
  `WalkStep` int(11) NULL DEFAULT NULL,
  `WaLkWait` int(11) NULL DEFAULT NULL,
  `ATTACK_SPD` int(11) NULL DEFAULT NULL,
  `UnFireRate` int(11) NULL DEFAULT NULL,
  `UnParalysis` int(11) NULL DEFAULT NULL,
  `UnPosion` int(11) NULL DEFAULT NULL,
  `UnDragonRate` int(11) NULL DEFAULT NULL,
  `ViewRange` int(11) NULL DEFAULT NULL,
  `MaxDamage` int(11) NULL DEFAULT NULL,
  `DecDamageRate` int(11) NULL DEFAULT NULL,
  `Tc` int(11) NULL DEFAULT NULL,
  `HeartPower` int(11) NULL DEFAULT NULL,
  `HeartAC` int(11) NULL DEFAULT NULL,
  `DropMode` int(11) NULL DEFAULT NULL,
  `UnCutting` int(11) NULL DEFAULT NULL,
  `DecDamage` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`Idx`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 706 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for TBL_QUEST
-- ----------------------------
DROP TABLE IF EXISTS `TBL_QUEST`;
CREATE TABLE `TBL_QUEST`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_CHARNAME` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_QUESTOPENINDEX` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_QUESTFININDEX` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_QUEST` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '任务标志' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for TBL_StdItems
-- ----------------------------
DROP TABLE IF EXISTS `TBL_StdItems`;
CREATE TABLE `TBL_StdItems`  (
  `Idx` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Stdmode` int(11) NULL DEFAULT NULL,
  `Shape` int(11) NULL DEFAULT NULL,
  `Weight` int(11) NULL DEFAULT NULL,
  `Anicount` int(11) NULL DEFAULT NULL,
  `Source` int(11) NULL DEFAULT NULL,
  `Reserved` int(11) NULL DEFAULT NULL,
  `Looks` int(11) NULL DEFAULT NULL,
  `DuraMax` int(11) NULL DEFAULT NULL,
  `Ac` int(11) NULL DEFAULT NULL,
  `Ac2` int(11) NULL DEFAULT NULL,
  `Mac` int(11) NULL DEFAULT NULL,
  `Mac2` int(11) NULL DEFAULT NULL,
  `Dc` int(11) NULL DEFAULT NULL,
  `Dc2` int(11) NULL DEFAULT NULL,
  `Mc` int(11) NULL DEFAULT NULL,
  `Mc2` int(11) NULL DEFAULT NULL,
  `Sc` int(11) NULL DEFAULT NULL,
  `Sc2` int(11) NULL DEFAULT NULL,
  `Need` int(11) NULL DEFAULT NULL,
  `NeedLevel` int(11) NULL DEFAULT NULL,
  `Price` int(11) NULL DEFAULT NULL,
  `Stock` int(11) NULL DEFAULT NULL,
  `HP` int(11) NULL DEFAULT NULL,
  `MP` int(11) NULL DEFAULT NULL,
  `MX` int(11) NULL DEFAULT NULL,
  `Attach` int(11) NULL DEFAULT NULL,
  `Attach1` int(11) NULL DEFAULT NULL,
  `Heart` int(11) NULL DEFAULT NULL,
  `Heart2` int(11) NULL DEFAULT NULL,
  `Tc` int(11) NULL DEFAULT NULL,
  `Tc2` int(11) NULL DEFAULT NULL,
  `Job` int(11) NULL DEFAULT NULL,
  `Color` int(11) NULL DEFAULT NULL,
  `NH` int(11) NULL DEFAULT NULL,
  `Main` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`Idx`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1001 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for TBL_Storages
-- ----------------------------
DROP TABLE IF EXISTS `TBL_Storages`;
CREATE TABLE `TBL_Storages`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_CHARNAME` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_POSITION` int(10) NULL DEFAULT NULL,
  `FLD_MAKEINDEX` int(20) NULL DEFAULT NULL,
  `FLD_STDINDEX` int(10) NULL DEFAULT NULL,
  `FLD_DURA` int(10) NULL DEFAULT NULL,
  `FLD_DURAMAX` int(10) NULL DEFAULT NULL,
  `FLD_VALUE0` int(10) NULL DEFAULT NULL,
  `FLD_VALUE1` int(10) NULL DEFAULT NULL,
  `FLD_VALUE2` int(10) NULL DEFAULT NULL,
  `FLD_VALUE3` int(10) NULL DEFAULT NULL,
  `FLD_VALUE4` int(10) NULL DEFAULT NULL,
  `FLD_VALUE5` int(10) NULL DEFAULT NULL,
  `FLD_VALUE6` int(10) NULL DEFAULT NULL,
  `FLD_VALUE7` int(10) NULL DEFAULT NULL,
  `FLD_VALUE8` int(10) NULL DEFAULT NULL,
  `FLD_VALUE9` int(10) NULL DEFAULT NULL,
  `FLD_VALUE10` int(10) NULL DEFAULT NULL,
  `FLD_VALUE11` int(10) NULL DEFAULT NULL,
  `FLD_VALUE12` int(10) NULL DEFAULT NULL,
  `FLD_VALUE13` int(10) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '仓库物品' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for TBL_UpgradeInfo
-- ----------------------------
DROP TABLE IF EXISTS `TBL_UpgradeInfo`;
CREATE TABLE `TBL_UpgradeInfo`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `CharName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '玩家名称',
  `FLD_DC` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_SC` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_MC` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_Dura` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_BackTick` int(11) NULL DEFAULT NULL COMMENT '升级取回时间',
  `FLD_UpgradeTIme` datetime NULL DEFAULT NULL,
  `FLD_Status` tinyint(2) NULL DEFAULT NULL COMMENT '武器升级状态',
  `FLD_UserItem` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '武器对象',
  `FLD_CreateTIme` datetime NULL DEFAULT NULL COMMENT '创建时间',
  `FLD_ModifyTIme` datetime NULL DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '武器升级数据' ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
