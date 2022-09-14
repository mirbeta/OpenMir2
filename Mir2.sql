/*
 Navicat Premium Data Transfer

 Source Server         : 10.10.0.199
 Source Server Type    : MySQL
 Source Server Version : 50737 (5.7.37-log)
 Source Host           : 10.10.0.199:3306
 Source Schema         : mir2

 Target Server Type    : MySQL
 Target Server Version : 50737 (5.7.37-log)
 File Encoding         : 65001

 Date: 14/09/2022 09:56:19
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for tbl_account
-- ----------------------------
DROP TABLE IF EXISTS `tbl_account`;
CREATE TABLE `tbl_account`  (
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
) ENGINE = InnoDB AUTO_INCREMENT = 13149 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '账号' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of tbl_account
-- ----------------------------
INSERT INTO `tbl_account` VALUES (1, 'admin', '123123', '123123', 20284187, 0, '650101-1455111', '\0\0\0\0\0\0\0\0\0\0\0\0\0\0', 'admin', 'admin', '\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0', 'admin', 'admin', '1978/01/01', '\0\0\0\0\0\0\0\0\0\0\0\0\0', 0, '2022-03-17 13:33:10', '2022-09-06 21:12:08');

-- ----------------------------
-- Table structure for tbl_bonusability
-- ----------------------------
DROP TABLE IF EXISTS `tbl_bonusability`;
CREATE TABLE `tbl_bonusability`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_PLAYERID` int(50) NULL DEFAULT NULL,
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
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of tbl_bonusability
-- ----------------------------

-- ----------------------------
-- Table structure for tbl_character
-- ----------------------------
DROP TABLE IF EXISTS `tbl_character`;
CREATE TABLE `tbl_character`  (
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
) ENGINE = InnoDB AUTO_INCREMENT = 6222 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '玩家' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of tbl_character
-- ----------------------------
INSERT INTO `tbl_character` VALUES (1, 1, '1', 'gm01', '3', 283, 349, 50, 0, 1, 0, 0, 891095, 0, '3', 330, 330, 0, 0, 0, 0, -244.658, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, 0, '2022-08-03 01:33:16', '2022-09-13 23:40:10');
INSERT INTO `tbl_character` VALUES (6221, 1, 'admin', '深山老毒', NULL, 0, 0, 0, 0, 1, 0, 2, 0, 0, NULL, 0, 0, 0, 0, 0, 0, 0.000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, 0, '2022-09-13 13:28:09', '2022-09-13 13:28:09');

-- ----------------------------
-- Table structure for tbl_character_ablity
-- ----------------------------
DROP TABLE IF EXISTS `tbl_character_ablity`;
CREATE TABLE `tbl_character_ablity`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_PlayerId` bigint(20) NOT NULL COMMENT '角色ID',
  `FLD_Level` tinyint(1) UNSIGNED NOT NULL COMMENT '当前等级',
  `FLD_Ac` smallint(11) NOT NULL COMMENT '攻击防御',
  `FLD_Mac` smallint(11) NOT NULL COMMENT '魔法防御',
  `FLD_Dc` smallint(11) NOT NULL COMMENT '物理攻击力',
  `FLD_Mc` smallint(11) NOT NULL COMMENT '魔法攻击力',
  `FLD_Sc` smallint(11) NOT NULL COMMENT '道术攻击力',
  `FLD_Hp` smallint(11) NOT NULL COMMENT '当前HP',
  `FLD_Mp` smallint(11) NOT NULL COMMENT '当前MP',
  `FLD_MaxHP` smallint(11) NOT NULL COMMENT '最大HP',
  `FLD_MAxMP` smallint(11) NOT NULL COMMENT '最大MP',
  `FLD_Exp` int(11) NOT NULL COMMENT '当前经验',
  `FLD_MaxExp` int(11) NOT NULL COMMENT '升级经验',
  `FLD_Weight` smallint(11) NOT NULL COMMENT '当前包裹重量',
  `FLD_MaxWeight` smallint(11) NOT NULL COMMENT '最大包裹重量',
  `FLD_WearWeight` tinyint(1) UNSIGNED NOT NULL COMMENT '当前腕力',
  `FLD_MaxWearWeight` tinyint(1) UNSIGNED NOT NULL COMMENT '最大腕力',
  `FLD_HandWeight` tinyint(1) UNSIGNED NOT NULL COMMENT '当前负重',
  `FLD_MaxHandWeight` tinyint(1) UNSIGNED NOT NULL COMMENT '最大负重',
  `FLD_ModifyTime` datetime NULL DEFAULT NULL COMMENT '最后修改时间',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6222 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of tbl_character_ablity
-- ----------------------------
INSERT INTO `tbl_character_ablity` VALUES (1, 1, 50, 50, 0, 0, 0, 0, 939, 186, 939, 186, 2671, 200000000, 2, 850, 0, 115, 7, 162, '2022-09-13 23:40:10');
INSERT INTO `tbl_character_ablity` VALUES (6221, 6221, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL);

-- ----------------------------
-- Table structure for tbl_character_magic
-- ----------------------------
DROP TABLE IF EXISTS `tbl_character_magic`;
CREATE TABLE `tbl_character_magic`  (
  `FLD_PLAYERID` int(11) NOT NULL,
  `FLD_MAGICID` int(11) NOT NULL,
  `FLD_LEVEL` int(11) NOT NULL,
  `FLD_USEKEY` char(2) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `FLD_CURRTRAIN` int(11) NOT NULL
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '技能' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of tbl_character_magic
-- ----------------------------
INSERT INTO `tbl_character_magic` VALUES (1, 26, 0, '83', 19);

-- ----------------------------
-- Table structure for tbl_character_nakedability
-- ----------------------------
DROP TABLE IF EXISTS `tbl_character_nakedability`;
CREATE TABLE `tbl_character_nakedability`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_PLAYERID` bigint(20) NULL DEFAULT NULL,
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
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of tbl_character_nakedability
-- ----------------------------

-- ----------------------------
-- Table structure for tbl_character_status
-- ----------------------------
DROP TABLE IF EXISTS `tbl_character_status`;
CREATE TABLE `tbl_character_status`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_PlayerId` bigint(20) NULL DEFAULT NULL,
  `FLD_CharName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_Status` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 36808 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of tbl_character_status
-- ----------------------------
INSERT INTO `tbl_character_status` VALUES (36807, 1, 'gm01', '0/0/0/0/0/0/0/0/0/0/0/0');

-- ----------------------------
-- Table structure for tbl_goldsales
-- ----------------------------
DROP TABLE IF EXISTS `tbl_goldsales`;
CREATE TABLE `tbl_goldsales`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `DealCharName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `BuyCharName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `SellDateTime` datetime NULL DEFAULT NULL,
  `State` tinyint(1) NULL DEFAULT NULL,
  `SellGold` int(11) NULL DEFAULT NULL,
  `UseItems` json NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '金币寄售' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of tbl_goldsales
-- ----------------------------

-- ----------------------------
-- Table structure for tbl_humrecord
-- ----------------------------
DROP TABLE IF EXISTS `tbl_humrecord`;
CREATE TABLE `tbl_humrecord`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_Account` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_CharName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_SelectID` tinyint(2) NULL DEFAULT NULL,
  `FLD_IsDeleted` tinyint(1) NULL DEFAULT NULL,
  `FLD_CreateDate` datetime NULL DEFAULT NULL,
  `FLD_ModifyDate` datetime NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6222 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of tbl_humrecord
-- ----------------------------
INSERT INTO `tbl_humrecord` VALUES (1, 'admin', 'gm01', 1, 0, '2022-08-03 01:33:16', '2022-09-06 21:18:50');
INSERT INTO `tbl_humrecord` VALUES (6221, 'admin', '深山老毒', 0, 0, '2022-09-13 13:28:09', '2022-09-13 13:28:09');

-- ----------------------------
-- Table structure for tbl_item
-- ----------------------------
DROP TABLE IF EXISTS `tbl_item`;
CREATE TABLE `tbl_item`  (
  `FLD_PLAYERID` bigint(20) NULL DEFAULT NULL COMMENT '角色ID',
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
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '包裹' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of tbl_item
-- ----------------------------
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 0, 1073741873, 115, 34931, 35000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 1073741874, 223, 31977, 32000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 1073741874, 223, 31977, 32000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 0, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 3968, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 3968, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 0, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 3992, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 3992, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 0, 469286, 97, 5000, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 4000, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 3954, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 0, 469286, 97, 4939, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 1073741874, 223, 31968, 32000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 1073741874, 223, 31968, 32000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 4919, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 3954, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 0, 1073741875, 115, 34695, 35000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 1073741874, 223, 31671, 32000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 1073741874, 223, 31671, 32000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469286, 97, 4919, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 3954, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 1073741877, 630, 7000, 7000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 1073741878, 630, 7000, 7000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 1073741879, 631, 7000, 7000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 1073741880, 631, 7000, 7000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 1073741881, 628, 7000, 7000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 1073741882, 629, 7000, 7000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 1073741883, 334, 6000, 6000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 1073741884, 334, 6000, 6000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 0, 1073741875, 115, 33892, 35000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 1073741874, 223, 31338, 32000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 1073741874, 223, 31338, 32000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 1073741873, 368, 35, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 3957, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 1073741875, 223, 32000, 32000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 0, 469286, 97, 4358, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 1073741876, 225, 32885, 33000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 1073741876, 225, 32885, 33000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 1073741873, 368, 35, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469285, 207, 3957, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 1073741875, 223, 32000, 32000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 0, 469286, 97, 4358, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 1073741876, 225, 32885, 33000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 1073741876, 225, 32885, 33000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 465264, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 465244, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 465260, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 465256, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 0, 469286, 97, 4396, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 3899, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 6, 467291, 322, 5530, 7000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 6, 467291, 322, 5530, 7000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473223, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473195, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 0, 469286, 97, 4876, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 3951, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 3951, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473223, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473195, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 1670, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 1670, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473223, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473195, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 1434, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 1434, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473223, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473195, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 752, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 752, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473223, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473195, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 71, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 71, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473223, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473195, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473195, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473223, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473195, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473195, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473223, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473195, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473195, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473223, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473195, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473195, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473223, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473195, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473195, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473223, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473195, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 2, 469283, 48, 7273, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 2, 469283, 48, 7273, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473223, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473195, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 2, 469283, 48, 5903, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 2, 469283, 48, 5903, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473223, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 473195, 8, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 2, 469283, 48, 4451, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 2, 469283, 48, 4451, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 0, 469286, 97, 1325, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 3609, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 3609, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 3335, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 3335, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 3290, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 3290, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469284, 3, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', -1, 469283, 48, 8000, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 2556, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_item` VALUES (1, 'gm01', 1, 469285, 207, 2556, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

-- ----------------------------
-- Table structure for tbl_magics
-- ----------------------------
DROP TABLE IF EXISTS `tbl_magics`;
CREATE TABLE `tbl_magics`  (
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
) ENGINE = InnoDB AUTO_INCREMENT = 207 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of tbl_magics
-- ----------------------------
INSERT INTO `tbl_magics` VALUES (1, 1, '白日门火球术', 1, 1, 4, 8, 8, 1, 2, 2, 1, 7, 200, 9, 300, 11, 500, 60, '英雄');
INSERT INTO `tbl_magics` VALUES (2, 2, '白日门治愈术', 2, 2, 7, 14, 20, 0, 0, 0, 2, 7, 200, 9, 300, 11, 500, 40, '英雄');
INSERT INTO `tbl_magics` VALUES (3, 3, '白日门剑术', 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 200, 9, 300, 11, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (4, 4, '白日门战法', 0, 0, 0, 0, 0, 0, 0, 0, 2, 9, 200, 13, 300, 19, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (5, 5, '白日门大火球', 1, 3, 3, 6, 6, 5, 10, 10, 1, 19, 200, 23, 300, 25, 500, 60, '英雄');
INSERT INTO `tbl_magics` VALUES (6, 6, '白日门施毒术', 2, 4, 4, 0, 0, 0, 0, 0, 2, 14, 200, 17, 300, 20, 500, 40, '英雄');
INSERT INTO `tbl_magics` VALUES (7, 7, '白日门攻杀', 0, 5, 0, 0, 0, 0, 0, 0, 0, 19, 200, 22, 300, 24, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (8, 8, '白日门抗拒', 4, 6, 8, 1, 1, 0, 0, 0, 1, 12, 200, 15, 300, 19, 500, 30, '英雄');
INSERT INTO `tbl_magics` VALUES (9, 9, '白日门地狱火', 5, 7, 10, 14, 14, 10, 6, 6, 1, 16, 200, 21, 300, 26, 500, 60, '英雄');
INSERT INTO `tbl_magics` VALUES (10, 10, '白日门疾光', 6, 8, 25, 12, 12, 20, 12, 12, 1, 26, 200, 29, 300, 32, 500, 100, '英雄');
INSERT INTO `tbl_magics` VALUES (11, 11, '白日门雷电术', 7, 9, 12, 14, 28, 6, 10, 10, 1, 17, 200, 20, 300, 23, 500, 100, '英雄');
INSERT INTO `tbl_magics` VALUES (12, 12, '白日门刺杀', 0, 13, 0, 0, 0, 0, 0, 0, 0, 25, 200, 27, 300, 29, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (13, 13, '白日门火符', 8, 10, 5, 15, 28, 2, 10, 10, 2, 18, 200, 21, 300, 24, 500, 60, '英雄');
INSERT INTO `tbl_magics` VALUES (14, 14, '白日门幽灵盾', 9, 11, 15, 0, 0, 0, 0, 0, 2, 22, 200, 24, 300, 26, 500, 40, '英雄');
INSERT INTO `tbl_magics` VALUES (15, 15, '白日门战甲术', 9, 12, 15, 0, 0, 0, 0, 0, 2, 25, 200, 27, 300, 29, 500, 40, '英雄');
INSERT INTO `tbl_magics` VALUES (16, 16, '白日门困魔咒', 10, 14, 10, 0, 0, 5, 0, 0, 2, 28, 200, 30, 300, 32, 500, 50, '英雄');
INSERT INTO `tbl_magics` VALUES (17, 17, '白日门骷髅术', 4, 15, 16, 0, 0, 8, 0, 0, 2, 19, 200, 23, 300, 32, 500, 50, '英雄');
INSERT INTO `tbl_magics` VALUES (18, 18, '白日门隐身', 4, 16, 5, 0, 0, 0, 0, 0, 2, 20, 200, 23, 300, 26, 500, 50, '英雄');
INSERT INTO `tbl_magics` VALUES (19, 19, '白日门群隐', 8, 17, 10, 0, 0, 0, 0, 0, 2, 21, 200, 25, 300, 29, 500, 50, '英雄');
INSERT INTO `tbl_magics` VALUES (20, 20, '白日门诱惑术', 2, 18, 3, 3, 3, 3, 0, 0, 1, 13, 200, 18, 300, 24, 500, 60, '英雄');
INSERT INTO `tbl_magics` VALUES (21, 21, '白日门瞬移', 4, 19, 10, 0, 0, 8, 0, 0, 1, 19, 200, 22, 300, 25, 500, 50, '英雄');
INSERT INTO `tbl_magics` VALUES (22, 22, '白日门火墙', 4, 20, 20, 3, 3, 25, 3, 3, 1, 24, 200, 29, 300, 33, 500, 120, '英雄');
INSERT INTO `tbl_magics` VALUES (23, 23, '白日门爆裂', 2, 21, 15, 8, 8, 10, 6, 6, 1, 22, 200, 27, 300, 31, 500, 60, '英雄');
INSERT INTO `tbl_magics` VALUES (24, 24, '白日门雷光', 4, 22, 35, 10, 30, 20, 10, 30, 1, 30, 200, 32, 300, 34, 500, 60, '英雄');
INSERT INTO `tbl_magics` VALUES (25, 25, '白日门半月', 0, 23, 0, 0, 0, 3, 0, 0, 0, 28, 200, 31, 300, 34, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (26, 26, '白日门烈火', 0, 24, 0, 0, 0, 7, 0, 0, 0, 35, 200, 37, 300, 40, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (27, 27, '白日门野蛮', 0, 25, 15, 0, 0, 0, 0, 0, 0, 30, 200, 34, 300, 39, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (28, 28, '白日门启示', 2, 26, 16, 0, 0, 0, 0, 0, 2, 26, 200, 30, 300, 35, 500, 40, '英雄');
INSERT INTO `tbl_magics` VALUES (29, 29, '白日门群疗', 2, 27, 12, 10, 10, 25, 4, 4, 2, 33, 200, 35, 300, 38, 500, 40, '英雄');
INSERT INTO `tbl_magics` VALUES (30, 30, '白日门神兽术', 4, 28, 16, 0, 0, 24, 0, 0, 2, 35, 200, 37, 300, 40, 500, 120, '英雄');
INSERT INTO `tbl_magics` VALUES (31, 31, '白日门魔法盾', 4, 29, 20, 0, 0, 30, 0, 0, 1, 31, 200, 34, 300, 38, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (32, 32, '白日门圣言术', 2, 30, 50, 0, 0, 40, 0, 0, 1, 32, 200, 35, 300, 39, 500, 120, '英雄');
INSERT INTO `tbl_magics` VALUES (33, 33, '白日门冰咆哮', 2, 31, 12, 12, 12, 30, 12, 12, 1, 35, 200, 37, 300, 40, 500, 60, '英雄');
INSERT INTO `tbl_magics` VALUES (34, 35, '白日门狮吼功', 5, 7, 10, 14, 14, 10, 6, 6, 2, 38, 200, 41, 300, 44, 500, 60, '英雄');
INSERT INTO `tbl_magics` VALUES (35, 41, '白日门狮子吼', 4, 43, 8, 1, 1, 0, 0, 0, 0, 38, 200, 41, 300, 44, 500, 30, '英雄');
INSERT INTO `tbl_magics` VALUES (36, 42, '白日门龙影剑', 0, 0, 0, 0, 0, 3, 0, 0, 0, 38, 200, 41, 300, 44, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (37, 43, '白日门开天斩', 0, 0, 20, 0, 0, 20, 0, 0, 0, 45, 200, 47, 300, 48, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (38, 44, '白日门寒冰掌', 1, 39, 14, 14, 25, 8, 10, 10, 1, 36, 200, 39, 300, 42, 500, 60, '英雄');
INSERT INTO `tbl_magics` VALUES (39, 45, '白日门灭天火', 14, 34, 15, 21, 30, 15, 12, 12, 1, 38, 200, 41, 300, 44, 500, 60, '英雄');
INSERT INTO `tbl_magics` VALUES (40, 46, '白日门分身术', 1, 42, 14, 10, 0, 5, 0, 0, 1, 45, 200, 47, 300, 48, 500, 50, '英雄');
INSERT INTO `tbl_magics` VALUES (41, 48, '白日门气功波', 4, 36, 8, 1, 1, 0, 0, 0, 2, 32, 200, 41, 300, 44, 500, 30, '英雄');
INSERT INTO `tbl_magics` VALUES (42, 50, '白日门真气', 4, 35, 8, 1, 1, 0, 0, 0, 2, 36, 200, 39, 300, 42, 500, 30, '英雄');
INSERT INTO `tbl_magics` VALUES (43, 58, '白日门火雨', 2, 51, 10, 30, 50, 20, 30, 30, 1, 46, 200, 48, 300, 50, 500, 60, '英雄');
INSERT INTO `tbl_magics` VALUES (44, 59, '白日门噬血术', 2, 48, 5, 15, 28, 2, 10, 10, 2, 46, 200, 48, 300, 50, 500, 60, '英雄');
INSERT INTO `tbl_magics` VALUES (45, 60, '破魂斩', 0, 60, 15, 10, 15, 7, 10, 15, 99, 43, 200, 45, 300, 47, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (46, 61, '劈星斩', 7, 61, 30, 15, 25, 7, 8, 15, 99, 43, 200, 45, 300, 47, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (47, 62, '雷霆一击', 7, 62, 30, 25, 35, 7, 8, 15, 99, 43, 200, 45, 300, 47, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (48, 63, '噬魂沼泽', 1, 63, 25, 8, 8, 1, 8, 15, 99, 43, 200, 45, 300, 47, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (49, 64, '末日审判', 7, 64, 40, 15, 15, 10, 12, 15, 99, 43, 200, 45, 300, 47, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (50, 65, '火龙气焰', 7, 65, 40, 30, 45, 6, 10, 10, 99, 43, 200, 45, 300, 47, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (51, 66, '四级英雄盾', 4, 52, 20, 0, 0, 30, 0, 0, 1, 40, 200, 41, 300, 42, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (52, 67, '白日门元力', 4, 50, 20, 0, 0, 0, 0, 0, 99, 1, 200, 1, 300, 1, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (53, 68, '白日门酒气', 4, 66, 20, 0, 0, 0, 0, 0, 99, 40, 200, 41, 300, 42, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (54, 72, '白日门召唤月灵', 4, 41, 12, 8, 0, 0, 0, 0, 2, 45, 200, 47, 300, 50, 500, 60, '英雄');
INSERT INTO `tbl_magics` VALUES (55, 74, '白日门逐日', 0, 53, 0, 0, 0, 7, 0, 0, 0, 46, 200, 48, 300, 50, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (56, 75, '白日门护体神盾', 4, 91, 0, 0, 0, 0, 0, 0, 99, 40, 200, 42, 300, 45, 500, 60, '英雄');
INSERT INTO `tbl_magics` VALUES (57, 200, '怒之攻杀', 0, 150, 5, 12, 12, 2, 3, 3, 0, 4, 200, 8, 300, 17, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (58, 201, '静之攻杀', 0, 180, 5, 10, 10, 2, 3, 3, 99, 14, 200, 18, 300, 30, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (59, 202, '怒之半月', 0, 151, 5, 12, 12, 2, 3, 3, 0, 13, 200, 21, 300, 31, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (60, 203, '静之半月', 0, 181, 5, 10, 10, 2, 3, 3, 99, 22, 200, 36, 300, 47, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (61, 204, '怒之烈火', 0, 153, 5, 12, 12, 2, 3, 3, 0, 70, 200, 78, 300, 86, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (62, 205, '静之烈火', 0, 183, 5, 10, 10, 2, 3, 3, 99, 69, 200, 80, 300, 87, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (63, 206, '怒之逐日', 0, 154, 5, 12, 12, 2, 6, 6, 0, 74, 200, 82, 300, 90, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (64, 207, '静之逐日', 0, 184, 5, 10, 10, 2, 6, 6, 99, 79, 200, 85, 300, 88, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (65, 208, '怒之火球', 0, 158, 5, 12, 12, 2, 3, 3, 1, 4, 200, 11, 300, 16, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (66, 209, '静之火球', 0, 190, 5, 10, 10, 2, 3, 3, 99, 12, 200, 20, 300, 26, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (67, 210, '怒之大火球', 0, 162, 5, 12, 12, 2, 3, 3, 1, 28, 200, 34, 300, 40, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (68, 211, '静之大火球', 0, 194, 5, 10, 10, 2, 3, 3, 99, 28, 200, 34, 300, 43, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (69, 212, '怒之火墙', 0, 163, 5, 12, 12, 2, 3, 3, 1, 32, 200, 46, 300, 52, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (70, 213, '静之火墙', 0, 195, 5, 10, 10, 2, 3, 3, 99, 39, 200, 48, 300, 59, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (71, 214, '怒之地狱火', 0, 159, 5, 12, 12, 2, 3, 3, 1, 9, 200, 18, 300, 22, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (72, 215, '静之地狱火', 0, 190, 5, 10, 10, 2, 3, 3, 99, 16, 200, 24, 300, 33, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (73, 216, '怒之疾光电影', 0, 164, 5, 12, 12, 2, 3, 3, 1, 49, 200, 58, 300, 64, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (74, 217, '静之疾光电影', 0, 196, 5, 10, 10, 2, 3, 3, 99, 42, 200, 57, 300, 65, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (75, 218, '怒之爆裂火焰', 0, 160, 5, 12, 12, 2, 3, 3, 1, 14, 200, 20, 300, 24, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (76, 219, '静之爆裂火焰', 0, 192, 5, 10, 10, 2, 3, 3, 99, 29, 200, 40, 300, 53, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (77, 220, '怒之冰咆哮', 0, 167, 5, 12, 12, 2, 3, 3, 1, 70, 200, 73, 300, 77, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (78, 221, '静之冰咆哮', 0, 199, 5, 10, 10, 2, 3, 3, 99, 71, 200, 77, 300, 83, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (79, 222, '怒之雷电', 0, 161, 5, 12, 12, 2, 3, 3, 1, 26, 200, 37, 300, 43, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (80, 223, '静之雷电', 0, 193, 5, 10, 10, 2, 3, 3, 99, 32, 200, 41, 300, 50, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (81, 224, '怒之地狱雷光', 0, 165, 5, 12, 12, 2, 3, 3, 1, 55, 200, 67, 300, 76, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (82, 225, '静之地狱雷光', 0, 197, 5, 10, 10, 2, 3, 3, 99, 51, 200, 68, 300, 73, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (83, 226, '怒之寒冰掌', 0, 166, 5, 12, 12, 2, 3, 3, 1, 61, 200, 72, 300, 75, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (84, 227, '静之寒冰掌', 0, 198, 5, 10, 10, 2, 3, 3, 99, 61, 200, 76, 300, 81, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (85, 228, '怒之灭天火', 0, 168, 5, 10, 10, 2, 3, 3, 1, 71, 200, 78, 300, 86, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (86, 229, '静之灭天火', 0, 200, 5, 10, 12, 2, 6, 6, 99, 75, 200, 85, 300, 88, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (87, 230, '怒之火符', 0, 156, 5, 12, 12, 2, 6, 6, 2, 21, 200, 47, 300, 60, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (88, 231, '静之火符', 0, 187, 5, 12, 12, 2, 6, 6, 99, 35, 200, 44, 300, 63, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (89, 232, '怒之噬血', 0, 157, 5, 12, 12, 2, 6, 6, 2, 70, 200, 82, 300, 90, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (90, 233, '静之噬血', 0, 189, 5, 12, 12, 2, 6, 6, 99, 79, 200, 84, 300, 89, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (91, 234, '怒之流星火雨', 0, 169, 5, 12, 12, 2, 6, 6, 1, 74, 200, 82, 300, 90, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (92, 235, '静之流星火雨', 0, 201, 5, 12, 12, 2, 6, 6, 99, 80, 200, 83, 300, 89, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (93, 236, '怒之内功剑法', 0, 152, 5, 12, 12, 2, 6, 6, 99, 38, 200, 50, 300, 63, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (94, 237, '静之内功剑法', 0, 182, 5, 12, 12, 2, 6, 6, 99, 40, 200, 52, 300, 65, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (95, 239, '怒之施毒术', 0, 155, 5, 5, 7, 2, 3, 3, 2, 40, 200, 52, 300, 65, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (96, 240, '静之施毒术', 0, 186, 5, 5, 7, 2, 3, 3, 99, 40, 200, 52, 300, 65, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (97, 241, '怒之月灵', 0, 172, 5, 5, 7, 2, 3, 3, 2, 40, 200, 52, 300, 65, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (98, 242, '静之月灵', 0, 188, 5, 5, 7, 2, 3, 3, 99, 40, 200, 52, 300, 65, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (99, 1, '火球术', 1, 1, 4, 8, 8, 1, 2, 2, 1, 7, 200, 11, 300, 16, 500, 60, '');
INSERT INTO `tbl_magics` VALUES (100, 2, '治愈术', 2, 2, 7, 14, 20, 0, 0, 0, 2, 7, 200, 11, 300, 16, 500, 40, '');
INSERT INTO `tbl_magics` VALUES (101, 3, '基本剑术', 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 200, 11, 300, 16, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (102, 4, '精神力战法', 0, 0, 0, 0, 0, 0, 0, 0, 2, 9, 200, 13, 300, 19, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (103, 5, '大火球', 1, 3, 3, 6, 6, 5, 10, 10, 1, 19, 200, 23, 300, 25, 500, 60, '');
INSERT INTO `tbl_magics` VALUES (104, 6, '施毒术', 2, 4, 4, 0, 0, 0, 0, 0, 2, 14, 200, 17, 300, 20, 500, 40, '');
INSERT INTO `tbl_magics` VALUES (105, 7, '攻杀剑术', 0, 5, 0, 0, 0, 0, 0, 0, 0, 19, 200, 22, 300, 24, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (106, 8, '抗拒火环', 4, 6, 8, 1, 1, 0, 0, 0, 1, 12, 200, 15, 300, 19, 500, 30, '');
INSERT INTO `tbl_magics` VALUES (107, 9, '地狱火', 5, 7, 10, 14, 14, 10, 6, 6, 1, 16, 200, 21, 300, 26, 500, 60, '');
INSERT INTO `tbl_magics` VALUES (108, 10, '疾光电影', 6, 8, 25, 12, 12, 20, 12, 12, 1, 26, 200, 29, 300, 32, 500, 100, '');
INSERT INTO `tbl_magics` VALUES (109, 11, '雷电术', 7, 9, 12, 14, 28, 6, 10, 10, 1, 17, 200, 20, 300, 23, 500, 100, '');
INSERT INTO `tbl_magics` VALUES (110, 12, '刺杀剑术', 0, 13, 0, 0, 0, 0, 0, 0, 0, 25, 200, 27, 300, 29, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (111, 13, '灵魂火符', 8, 10, 5, 15, 28, 2, 10, 10, 2, 18, 200, 21, 300, 24, 500, 60, '');
INSERT INTO `tbl_magics` VALUES (112, 14, '幽灵盾', 9, 11, 15, 0, 0, 0, 0, 0, 2, 22, 200, 24, 300, 26, 500, 40, '');
INSERT INTO `tbl_magics` VALUES (113, 15, '神圣战甲术', 9, 12, 15, 0, 0, 0, 0, 0, 2, 25, 200, 27, 300, 29, 500, 40, '');
INSERT INTO `tbl_magics` VALUES (114, 16, '困魔咒', 10, 14, 10, 0, 0, 5, 0, 0, 2, 28, 200, 30, 300, 32, 500, 50, '');
INSERT INTO `tbl_magics` VALUES (115, 17, '召唤骷髅', 4, 15, 16, 0, 0, 8, 0, 0, 2, 19, 200, 23, 300, 26, 500, 50, '');
INSERT INTO `tbl_magics` VALUES (116, 18, '隐身术', 4, 16, 5, 0, 0, 0, 0, 0, 2, 20, 200, 23, 300, 26, 500, 50, '');
INSERT INTO `tbl_magics` VALUES (117, 19, '集体隐身术', 8, 17, 10, 0, 0, 0, 0, 0, 2, 21, 200, 25, 300, 29, 500, 50, '');
INSERT INTO `tbl_magics` VALUES (118, 20, '诱惑之光', 2, 18, 3, 3, 3, 3, 0, 0, 1, 13, 200, 18, 300, 24, 500, 60, '');
INSERT INTO `tbl_magics` VALUES (119, 21, '瞬息移动', 4, 19, 10, 0, 0, 8, 0, 0, 1, 19, 200, 22, 300, 25, 500, 50, '');
INSERT INTO `tbl_magics` VALUES (120, 22, '火墙', 4, 20, 20, 3, 3, 25, 3, 3, 1, 24, 200, 29, 300, 33, 500, 120, '');
INSERT INTO `tbl_magics` VALUES (121, 23, '爆裂火焰', 2, 21, 15, 8, 8, 10, 6, 6, 1, 22, 200, 27, 300, 31, 500, 60, '');
INSERT INTO `tbl_magics` VALUES (122, 24, '地狱雷光', 4, 22, 35, 10, 30, 20, 10, 30, 1, 30, 200, 32, 300, 34, 500, 60, '');
INSERT INTO `tbl_magics` VALUES (123, 25, '半月弯刀', 0, 23, 0, 0, 0, 3, 0, 0, 0, 28, 200, 31, 300, 34, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (124, 26, '烈火剑法', 0, 24, 0, 0, 0, 7, 0, 0, 0, 35, 200, 37, 300, 40, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (125, 27, '野蛮冲撞', 0, 25, 15, 0, 0, 0, 0, 0, 0, 30, 200, 34, 300, 39, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (126, 28, '心灵启示', 2, 26, 16, 0, 0, 0, 0, 0, 2, 26, 200, 30, 300, 35, 500, 40, '');
INSERT INTO `tbl_magics` VALUES (127, 29, '群体治疗术', 2, 27, 12, 10, 10, 25, 4, 4, 2, 33, 200, 35, 300, 38, 500, 40, '');
INSERT INTO `tbl_magics` VALUES (128, 30, '召唤神兽', 4, 28, 16, 0, 0, 24, 0, 0, 2, 35, 200, 37, 300, 40, 500, 120, '');
INSERT INTO `tbl_magics` VALUES (129, 31, '魔法盾', 4, 29, 20, 0, 0, 30, 0, 0, 1, 31, 200, 34, 300, 38, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (130, 32, '圣言术', 2, 30, 50, 0, 0, 40, 0, 0, 1, 32, 200, 35, 300, 39, 500, 120, '');
INSERT INTO `tbl_magics` VALUES (131, 33, '冰咆哮', 2, 31, 12, 12, 12, 30, 12, 12, 1, 35, 200, 37, 300, 40, 500, 60, '');
INSERT INTO `tbl_magics` VALUES (132, 34, '解毒术', 2, 26, 16, 0, 0, 0, 0, 0, 2, 48, 200, 50, 300, 52, 500, 40, '');
INSERT INTO `tbl_magics` VALUES (133, 36, '火焰冰', 1, 1, 14, 12, 24, 10, 6, 6, 1, 48, 200, 50, 300, 52, 500, 60, '');
INSERT INTO `tbl_magics` VALUES (134, 37, '雷神之怒', 7, 9, 20, 14, 25, 6, 10, 12, 1, 50, 200, 52, 300, 55, 500, 100, '');
INSERT INTO `tbl_magics` VALUES (135, 38, '群体施毒术', 2, 27, 12, 10, 10, 30, 4, 4, 2, 48, 200, 50, 300, 52, 500, 40, '');
INSERT INTO `tbl_magics` VALUES (136, 39, '乌龟跑步', 1, 1, 4, 8, 8, 1, 2, 2, 99, 50, 200, 52, 300, 55, 500, 20, '');
INSERT INTO `tbl_magics` VALUES (137, 40, '十方斩', 0, 0, 20, 0, 0, 3, 0, 0, 0, 48, 200, 50, 300, 52, 500, 20, '');
INSERT INTO `tbl_magics` VALUES (138, 41, '狮子吼', 4, 43, 8, 1, 1, 8, 0, 0, 0, 38, 200, 41, 300, 44, 500, 30, '');
INSERT INTO `tbl_magics` VALUES (139, 42, '龙影剑法', 0, 0, 0, 0, 0, 3, 0, 0, 0, 38, 200, 41, 300, 44, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (140, 43, '开天斩', 0, 0, 20, 0, 0, 20, 0, 0, 0, 37, 200, 40, 300, 43, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (141, 44, '寒冰掌', 1, 39, 14, 14, 25, 8, 10, 10, 1, 36, 200, 39, 300, 41, 500, 60, '');
INSERT INTO `tbl_magics` VALUES (142, 45, '灭天火', 14, 34, 15, 21, 30, 15, 12, 12, 1, 38, 200, 41, 300, 44, 500, 60, '');
INSERT INTO `tbl_magics` VALUES (143, 46, '分身术', 1, 42, 14, 10, 0, 5, 0, 0, 99, 50, 200, 52, 300, 55, 500, 50, '');
INSERT INTO `tbl_magics` VALUES (144, 47, '地狱烈焰', 7, 65, 32, 10, 30, 16, 10, 30, 1, 46, 200, 48, 300, 50, 500, 120, '');
INSERT INTO `tbl_magics` VALUES (145, 48, '气功波', 7, 36, 8, 1, 1, 0, 0, 0, 2, 36, 200, 39, 300, 41, 500, 30, '');
INSERT INTO `tbl_magics` VALUES (146, 49, '云寂术', 2, 40, 4, 4, 0, 0, 0, 0, 2, 48, 200, 50, 300, 52, 500, 40, '');
INSERT INTO `tbl_magics` VALUES (147, 50, '无极真气', 4, 35, 8, 1, 1, 4, 0, 0, 2, 38, 200, 41, 300, 44, 500, 30, '');
INSERT INTO `tbl_magics` VALUES (148, 51, '月魂风暴', 2, 47, 10, 12, 12, 30, 14, 14, 2, 50, 200, 52, 300, 55, 500, 30, '');
INSERT INTO `tbl_magics` VALUES (149, 52, '神之诅咒', 9, 46, 15, 0, 0, 0, 0, 0, 2, 48, 200, 50, 300, 52, 500, 30, '');
INSERT INTO `tbl_magics` VALUES (150, 53, '嗜血魔咒(保留)', 2, 48, 4, 4, 10, 20, 3, 6, 2, 50, 200, 52, 300, 55, 500, 30, '');
INSERT INTO `tbl_magics` VALUES (151, 54, '召妖降魔', 2, 49, 4, 4, 0, 0, 0, 0, 2, 18, 200, 20, 300, 22, 500, 30, '');
INSERT INTO `tbl_magics` VALUES (152, 55, '吸星狂杀', 2, 19, 0, 0, 0, 0, 0, 0, 0, 50, 200, 52, 300, 55, 500, 40, '');
INSERT INTO `tbl_magics` VALUES (153, 56, '异形换位', 2, 19, 0, 0, 0, 0, 0, 0, 1, 48, 200, 50, 300, 52, 500, 30, '');
INSERT INTO `tbl_magics` VALUES (154, 57, '回生术', 2, 40, 0, 0, 0, 0, 0, 0, 2, 48, 200, 50, 300, 52, 500, 30, '');
INSERT INTO `tbl_magics` VALUES (155, 58, '流星火雨', 2, 51, 10, 30, 50, 20, 30, 30, 1, 46, 200, 48, 300, 50, 500, 60, '');
INSERT INTO `tbl_magics` VALUES (156, 59, '噬血术', 2, 48, 5, 15, 28, 2, 10, 10, 2, 46, 200, 48, 300, 50, 500, 60, '');
INSERT INTO `tbl_magics` VALUES (157, 66, '四级魔法盾', 4, 52, 20, 0, 0, 30, 0, 0, 1, 40, 200, 41, 300, 42, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (158, 66, '白日门四级法盾', 4, 52, 20, 0, 0, 30, 0, 0, 1, 40, 200, 41, 300, 42, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (159, 67, '先天元力', 4, 50, 20, 0, 0, 0, 0, 0, 99, 50, 200, 52, 300, 55, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (160, 68, '酒气护体', 4, 66, 20, 0, 0, 0, 0, 0, 99, 50, 200, 52, 300, 55, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (161, 69, '倚天辟地', 2, 55, 30, 75, 90, 8, 25, 35, 99, 38, 200, 41, 300, 44, 500, 60, '');
INSERT INTO `tbl_magics` VALUES (162, 71, '四级召唤神兽', 4, 76, 16, 0, 0, 24, 0, 0, 2, 50, 200, 55, 300, 60, 500, 120, '');
INSERT INTO `tbl_magics` VALUES (163, 71, '四级英雄神兽', 4, 76, 16, 0, 0, 24, 0, 0, 2, 50, 200, 53, 300, 56, 500, 120, '英雄');
INSERT INTO `tbl_magics` VALUES (164, 72, '召唤月灵', 4, 41, 12, 0, 0, 8, 0, 0, 2, 45, 200, 47, 300, 50, 500, 60, '');
INSERT INTO `tbl_magics` VALUES (165, 74, '逐日剑法', 0, 53, 0, 0, 0, 7, 0, 0, 0, 46, 200, 48, 300, 50, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (166, 75, '护体神盾', 4, 91, 0, 0, 0, 0, 0, 0, 99, 39, 200, 39, 300, 39, 500, 60, '');
INSERT INTO `tbl_magics` VALUES (167, 76, '三绝杀', 0, 102, 5, 10, 10, 5, 6, 6, 0, 15, 200, 15, 300, 15, 500, 60, '连击');
INSERT INTO `tbl_magics` VALUES (168, 77, '双龙破', 1, 103, 5, 10, 10, 5, 6, 6, 1, 15, 200, 15, 300, 15, 500, 60, '连击');
INSERT INTO `tbl_magics` VALUES (169, 78, '虎啸诀', 8, 104, 5, 10, 10, 5, 6, 6, 2, 15, 200, 15, 300, 15, 500, 60, '连击');
INSERT INTO `tbl_magics` VALUES (170, 79, '追心刺', 0, 105, 5, 15, 15, 5, 8, 8, 0, 33, 200, 33, 300, 33, 500, 60, '连击');
INSERT INTO `tbl_magics` VALUES (171, 80, '凤舞祭', 1, 106, 5, 15, 15, 5, 8, 8, 1, 33, 200, 33, 300, 33, 500, 60, '连击');
INSERT INTO `tbl_magics` VALUES (172, 81, '八卦掌', 1, 107, 5, 15, 15, 5, 8, 8, 2, 33, 200, 33, 300, 33, 500, 60, '连击');
INSERT INTO `tbl_magics` VALUES (173, 82, '断岳斩', 0, 108, 5, 20, 20, 5, 10, 10, 0, 50, 200, 52, 300, 55, 500, 60, '连击');
INSERT INTO `tbl_magics` VALUES (174, 83, '惊雷爆', 1, 109, 5, 20, 20, 5, 10, 10, 1, 50, 200, 52, 300, 55, 500, 60, '连击');
INSERT INTO `tbl_magics` VALUES (175, 84, '三焰咒', 8, 110, 5, 20, 20, 5, 10, 10, 2, 50, 200, 52, 300, 55, 500, 60, '连击');
INSERT INTO `tbl_magics` VALUES (176, 85, '横扫千军', 0, 111, 5, 30, 30, 5, 15, 15, 0, 53, 200, 55, 300, 58, 500, 60, '连击');
INSERT INTO `tbl_magics` VALUES (177, 86, '冰天雪地', 2, 112, 5, 30, 30, 5, 15, 15, 1, 53, 200, 55, 300, 58, 500, 60, '连击');
INSERT INTO `tbl_magics` VALUES (178, 87, '万剑归宗', 1, 113, 5, 30, 30, 5, 15, 15, 2, 53, 200, 55, 300, 58, 500, 60, '连击');
INSERT INTO `tbl_magics` VALUES (179, 88, '四级基本剑术', 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 200, 53, 300, 56, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (180, 88, '四级英雄剑术', 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 200, 53, 300, 56, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (181, 89, '四级刺杀剑术', 0, 54, 0, 0, 0, 0, 0, 0, 0, 50, 200, 53, 300, 56, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (182, 89, '四级英雄刺杀', 0, 54, 0, 0, 0, 0, 0, 0, 0, 50, 200, 53, 300, 56, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (183, 90, '四级半月弯刀', 0, 56, 0, 0, 0, 3, 0, 0, 0, 50, 200, 53, 300, 56, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (184, 90, '四级英雄半月', 0, 56, 0, 0, 0, 3, 0, 0, 0, 50, 200, 53, 300, 56, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (185, 91, '四级雷电术', 7, 75, 12, 14, 28, 6, 10, 10, 1, 50, 200, 53, 300, 56, 500, 100, '');
INSERT INTO `tbl_magics` VALUES (186, 91, '四级英雄雷电术', 7, 75, 12, 14, 28, 6, 10, 10, 1, 50, 200, 53, 300, 56, 500, 100, '英雄');
INSERT INTO `tbl_magics` VALUES (187, 92, '四级流星火雨', 2, 80, 12, 40, 80, 30, 12, 12, 1, 50, 200, 53, 300, 56, 500, 30, '');
INSERT INTO `tbl_magics` VALUES (188, 92, '四级英雄火雨', 2, 80, 12, 40, 80, 30, 12, 12, 1, 50, 200, 53, 300, 56, 500, 30, '英雄');
INSERT INTO `tbl_magics` VALUES (189, 93, '四级施毒术', 2, 77, 4, 0, 0, 0, 0, 0, 2, 50, 200, 53, 300, 56, 500, 40, '');
INSERT INTO `tbl_magics` VALUES (190, 93, '四级英雄施毒术', 2, 77, 4, 0, 0, 0, 0, 0, 2, 50, 200, 53, 300, 56, 500, 40, '英雄');
INSERT INTO `tbl_magics` VALUES (191, 94, '四级噬血术', 2, 74, 5, 50, 60, 2, 10, 10, 2, 50, 200, 53, 300, 56, 500, 60, '');
INSERT INTO `tbl_magics` VALUES (192, 94, '四级英雄噬血术', 2, 74, 5, 50, 60, 2, 10, 10, 2, 50, 200, 53, 300, 56, 500, 60, '英雄');
INSERT INTO `tbl_magics` VALUES (193, 95, '斗转星移', 4, 114, 20, 0, 0, 0, 0, 0, 99, 0, 0, 0, 0, 0, 0, 0, '通用');
INSERT INTO `tbl_magics` VALUES (194, 96, '血魄一击(战)', 0, 81, 20, 75, 90, 10, 12, 20, 0, 50, 200, 53, 300, 56, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (195, 96, '白日门血魄(战)', 0, 81, 20, 75, 90, 10, 12, 20, 0, 50, 200, 53, 300, 56, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (196, 97, '血魄一击(法)', 1, 82, 20, 75, 90, 10, 12, 20, 1, 50, 200, 53, 300, 56, 500, 30, '');
INSERT INTO `tbl_magics` VALUES (197, 97, '白日门血魄(法)', 1, 82, 20, 75, 90, 10, 12, 20, 1, 50, 200, 53, 300, 56, 500, 30, '英雄');
INSERT INTO `tbl_magics` VALUES (198, 98, '血魄一击(道)', 2, 83, 20, 75, 90, 10, 12, 20, 2, 50, 200, 53, 300, 56, 500, 40, '');
INSERT INTO `tbl_magics` VALUES (199, 98, '白日门血魄(道)', 2, 83, 20, 75, 90, 10, 12, 20, 2, 50, 200, 53, 300, 56, 500, 40, '英雄');
INSERT INTO `tbl_magics` VALUES (200, 99, '白日门强身术', 4, 92, 0, 0, 0, 0, 0, 0, 99, 50, 200, 53, 300, 56, 500, 0, '英雄');
INSERT INTO `tbl_magics` VALUES (201, 100, '神秘解读', 4, 95, 0, 0, 0, 0, 0, 0, 99, 28, 200, 35, 300, 42, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (202, 101, '神龙附体', 0, 96, 50, 0, 0, 50, 0, 0, 99, 50, 200, 53, 300, 56, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (203, 102, '唯我独尊', 2, 97, 100, 0, 0, 50, 0, 0, 99, 50, 200, 53, 300, 56, 500, 0, '内功');
INSERT INTO `tbl_magics` VALUES (204, 103, '召唤巨魔', 4, 76, 20, 0, 0, 24, 0, 0, 99, 50, 200, 53, 300, 56, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (205, 500, '破血狂杀', 4, 35, 20, 1, 1, 16, 0, 0, 0, 50, 200, 53, 300, 56, 500, 0, '');
INSERT INTO `tbl_magics` VALUES (206, 501, '迅影离魂', 4, 66, 30, 0, 0, 0, 0, 0, 0, 50, 200, 53, 300, 56, 500, 0, '');

-- ----------------------------
-- Table structure for tbl_monsters
-- ----------------------------
DROP TABLE IF EXISTS `tbl_monsters`;
CREATE TABLE `tbl_monsters`  (
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
) ENGINE = InnoDB AUTO_INCREMENT = 706 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of tbl_monsters
-- ----------------------------
INSERT INTO `tbl_monsters` VALUES (1, '鸡', 51, 11, 160, 5, 0, 0, 5, 5, 0, 0, 0, 1, 1, 0, 0, 10, 3, 500, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (2, '鹿', 52, 11, 161, 12, 0, 0, 15, 25, 0, 0, 0, 2, 4, 0, 0, 7, 4, 500, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (3, '稻草人', 83, 18, 27, 10, 1, 0, 12, 25, 0, 0, 0, 1, 2, 0, 0, 8, 5, 1500, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (4, '稻草人0', 83, 18, 27, 10, 1, 0, 12, 25, 0, 0, 0, 1, 2, 0, 0, 8, 5, 1500, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (5, '多钩猫', 81, 17, 25, 13, 0, 0, 17, 30, 0, 0, 0, 2, 4, 0, 0, 7, 5, 1500, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (6, '多钩猫0', 81, 17, 25, 13, 0, 0, 17, 30, 0, 0, 0, 2, 4, 0, 0, 7, 5, 1500, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (7, '钉耙猫', 81, 17, 26, 13, 0, 0, 18, 32, 0, 0, 0, 2, 4, 0, 0, 7, 5, 1500, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (8, '钉耙猫0', 81, 17, 26, 13, 0, 0, 18, 32, 0, 0, 0, 2, 4, 0, 0, 7, 5, 1500, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (9, '蛤蟆', 83, 19, 162, 12, 0, 0, 15, 20, 0, 0, 0, 0, 5, 0, 0, 13, 6, 1500, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (10, '森林雪人', 83, 10, 1, 16, 0, 0, 30, 36, 0, 2, 0, 7, 10, 0, 0, 12, 6, 1500, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (11, '森林雪人0', 83, 10, 1, 16, 0, 0, 30, 36, 0, 2, 0, 7, 10, 0, 0, 12, 6, 1500, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (12, '毒蜘蛛', 82, 19, 163, 16, 0, 0, 42, 42, 0, 2, 1, 6, 9, 0, 0, 15, 5, 900, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (13, '食人花', 85, 13, 10, 15, 0, 0, 28, 28, 0, 2, 0, 6, 9, 0, 0, 14, 9, 1500, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (14, '半兽人', 83, 19, 100, 15, 0, 0, 20, 30, 0, 1, 0, 4, 9, 0, 0, 15, 6, 1500, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (15, '半兽人0', 83, 19, 100, 15, 0, 0, 20, 30, 0, 1, 0, 4, 9, 0, 0, 15, 8, 1500, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (16, '半兽战士', 81, 19, 101, 22, 0, 0, 90, 100, 0, 3, 1, 5, 12, 0, 0, 15, 9, 1500, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (17, '半兽战士0', 81, 19, 101, 22, 0, 0, 90, 100, 0, 3, 1, 5, 12, 0, 0, 15, 9, 1500, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (18, '半兽勇士', 81, 19, 102, 28, 0, 1, 300, 300, 0, 0, 0, 15, 32, 0, 0, 15, 10, 1500, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (19, '半兽勇士10', 81, 19, 102, 50, 0, 1, 300, 900, 0, 100, 0, 0, 80, 0, 0, 17, 20, 600, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (20, '半兽勇士0', 81, 19, 102, 50, 0, 1, 300, 300, 0, 0, 0, 0, 80, 0, 0, 15, 13, 1500, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (21, '半兽统领', 87, 19, 102, 40, 0, 100, 1000, 1200, 0, 5, 5, 15, 30, 0, 0, 15, 15, 1500, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (22, '蝎子', 84, 32, 83, 18, 0, 0, 45, 45, 0, 2, 0, 7, 16, 0, 0, 15, 9, 1200, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (23, '山洞蝙蝠', 81, 19, 80, 20, 0, 0, 25, 25, 0, 2, 0, 4, 6, 0, 0, 25, 10, 1200, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (24, '山洞蝙蝠0', 81, 19, 80, 20, 0, 0, 25, 25, 0, 2, 0, 4, 6, 0, 0, 25, 10, 1200, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (25, '骷髅', 86, 14, 20, 18, 1, 0, 85, 90, 0, 0, 0, 7, 10, 0, 0, 13, 8, 1500, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (26, '骷髅0', 86, 14, 20, 18, 1, 0, 85, 90, 0, 0, 0, 7, 10, 0, 0, 13, 8, 1500, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (27, '掷斧骷髅', 87, 15, 21, 18, 1, 0, 90, 100, 0, 0, 0, 4, 9, 0, 0, 12, 9, 1500, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (28, '掷斧骷髅0', 87, 15, 21, 18, 1, 0, 90, 100, 0, 0, 0, 4, 9, 0, 0, 12, 9, 1500, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (29, '骷髅战士', 88, 14, 22, 19, 1, 0, 95, 105, 0, 0, 0, 2, 15, 0, 0, 13, 9, 1500, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (30, '骷髅战士0', 88, 14, 22, 19, 1, 0, 95, 105, 0, 0, 0, 2, 15, 0, 0, 13, 9, 1500, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (31, '骷髅战将', 89, 14, 23, 20, 1, 0, 100, 110, 0, 2, 1, 7, 13, 0, 0, 15, 9, 1200, 1, 0, 2300, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (32, '骷髅战将0', 89, 14, 23, 20, 1, 0, 100, 110, 0, 2, 1, 7, 13, 0, 0, 15, 9, 1200, 1, 0, 2300, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (33, '骷髅精灵', 89, 14, 150, 40, 1, 1, 600, 500, 0, 5, 4, 7, 24, 0, 0, 15, 12, 1200, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (34, '洞蛆', 90, 16, 24, 21, 0, 100, 60, 65, 0, 0, 0, 6, 8, 0, 0, 12, 10, 1000, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (35, '沃玛战士', 97, 19, 30, 30, 1, 0, 260, 265, 0, 3, 2, 14, 28, 0, 0, 15, 12, 1000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (36, '沃玛战士0', 97, 19, 30, 30, 1, 0, 260, 265, 0, 3, 2, 14, 28, 0, 0, 15, 12, 1000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (37, '沃玛勇士', 97, 19, 32, 30, 1, 0, 280, 285, 0, 3, 2, 16, 28, 0, 0, 15, 12, 1000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (38, '沃玛勇士0', 97, 19, 32, 30, 1, 0, 280, 285, 0, 3, 2, 16, 28, 0, 0, 15, 12, 1000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (39, '沃玛战将', 97, 19, 33, 30, 1, 0, 280, 285, 0, 3, 2, 15, 29, 0, 0, 15, 12, 1000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (40, '沃玛战将0', 97, 19, 33, 30, 1, 0, 280, 285, 0, 3, 2, 15, 29, 0, 0, 15, 12, 1000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (41, '火焰沃玛', 91, 20, 31, 31, 1, 0, 290, 340, 0, 0, 0, 14, 26, 0, 0, 20, 13, 800, 1, 0, 1700, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (42, '火焰沃玛0', 91, 20, 31, 31, 1, 0, 290, 340, 0, 0, 0, 14, 26, 0, 0, 20, 13, 800, 1, 0, 1700, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (43, '沃玛卫士2', 97, 19, 151, 50, 1, 0, 1, 600, 0, 8, 8, 22, 42, 0, 0, 20, 17, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (44, '沃玛卫士', 97, 19, 151, 50, 1, 0, 1200, 1000, 0, 8, 8, 22, 42, 0, 0, 20, 17, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (45, '沃玛教主', 92, 21, 34, 60, 1, 1, 2500, 2200, 0, 17, 17, 35, 60, 0, 0, 30, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (46, '沃玛教主4', 92, 21, 34, 60, 1, 1, 2500, 2200, 0, 17, 17, 35, 60, 0, 0, 30, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (47, '暗黑战士', 93, 22, 28, 26, 0, 0, 200, 165, 0, 3, 2, 9, 16, 0, 0, 15, 13, 900, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (48, '暗黑战士0', 93, 22, 28, 26, 0, 0, 200, 165, 0, 3, 2, 9, 16, 0, 0, 15, 13, 900, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (49, '粪虫', 106, 52, 29, 26, 1, 1, 180, 155, 0, 3, 2, 9, 17, 0, 0, 15, 11, 1500, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (50, '僵尸1', 94, 40, 40, 25, 1, 1, 160, 155, 0, 0, 0, 12, 16, 0, 0, 13, 11, 2000, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (51, '僵尸10', 94, 40, 40, 25, 1, 1, 160, 155, 0, 0, 0, 12, 16, 0, 0, 13, 11, 2000, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (52, '僵尸17', 94, 40, 40, 25, 1, 1, 80, 155, 0, 1, 1, 24, 32, 0, 0, 13, 11, 2000, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (53, '僵尸2', 95, 41, 50, 25, 1, 0, 160, 155, 0, 2, 1, 8, 17, 0, 0, 15, 11, 2000, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (54, '僵尸20', 95, 41, 50, 25, 1, 0, 160, 155, 0, 2, 1, 8, 17, 0, 0, 15, 11, 2000, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (55, '僵尸27', 95, 41, 50, 25, 1, 0, 80, 155, 0, 4, 2, 16, 34, 0, 0, 15, 11, 2000, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (56, '僵尸3', 96, 42, 51, 25, 1, 0, 160, 155, 0, 2, 1, 6, 17, 0, 0, 15, 11, 2000, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (57, '僵尸30', 96, 42, 51, 25, 1, 0, 160, 155, 0, 2, 1, 6, 17, 0, 0, 15, 11, 2000, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (58, '僵尸37', 96, 42, 51, 25, 1, 0, 80, 155, 0, 4, 2, 12, 34, 0, 0, 15, 11, 2000, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (59, '僵尸4', 96, 42, 52, 25, 1, 0, 160, 155, 0, 2, 1, 6, 17, 0, 0, 15, 11, 2000, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (60, '僵尸40', 96, 42, 52, 25, 1, 0, 160, 155, 0, 2, 1, 6, 17, 0, 0, 15, 11, 2000, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (61, '僵尸47', 96, 42, 52, 25, 1, 0, 80, 155, 0, 4, 2, 12, 34, 0, 0, 15, 11, 2000, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (62, '僵尸5', 96, 42, 53, 25, 1, 0, 160, 155, 0, 2, 1, 6, 17, 0, 0, 15, 11, 1500, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (63, '僵尸50', 96, 42, 53, 25, 1, 0, 160, 155, 0, 2, 1, 6, 17, 0, 0, 15, 11, 1500, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (64, '僵尸57', 96, 42, 53, 25, 1, 0, 80, 155, 0, 4, 2, 12, 34, 0, 0, 15, 11, 1500, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (65, '尸王', 81, 19, 152, 45, 1, 1, 800, 500, 0, 3, 3, 18, 36, 0, 0, 15, 15, 1500, 1, 0, 2800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (66, '尸王2', 81, 19, 152, 45, 1, 1, 800, 500, 0, 3, 3, 18, 36, 0, 0, 15, 15, 1200, 1, 0, 2800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (67, '尸王7', 81, 19, 152, 45, 1, 1, 400, 1000, 0, 6, 6, 36, 72, 0, 0, 15, 15, 1200, 1, 0, 2800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (68, '红蛇', 81, 19, 36, 17, 0, 0, 50, 50, 0, 0, 2, 7, 12, 0, 0, 15, 11, 1200, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (69, '红蛇0', 81, 19, 36, 17, 0, 0, 50, 50, 0, 0, 2, 7, 12, 0, 0, 15, 11, 1200, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (70, '虎蛇', 81, 19, 38, 18, 0, 0, 50, 50, 0, 0, 2, 10, 11, 0, 0, 15, 11, 1200, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (71, '虎蛇0', 81, 19, 38, 18, 0, 0, 50, 50, 0, 0, 2, 10, 11, 0, 0, 15, 11, 1200, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (72, '羊', 52, 19, 43, 13, 0, 0, 20, 20, 0, 0, 0, 1, 3, 0, 0, 10, 7, 1400, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (73, '羊7', 52, 19, 43, 13, 0, 0, 10, 20, 0, 1, 1, 2, 6, 0, 0, 10, 7, 1400, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (74, '牛', 52, 19, 104, 13, 0, 0, 10, 20, 0, 1, 1, 2, 6, 0, 0, 10, 7, 1300, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (75, '猎鹰', 81, 19, 44, 16, 0, 0, 38, 38, 0, 0, 0, 7, 10, 0, 0, 40, 13, 1200, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (76, '猎鹰0', 81, 19, 44, 16, 0, 0, 38, 38, 0, 0, 0, 7, 10, 0, 0, 40, 13, 1200, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (77, '盔甲虫', 81, 19, 45, 16, 0, 0, 37, 50, 0, 0, 0, 4, 10, 0, 0, 12, 7, 1200, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (78, '盔甲虫0', 81, 19, 45, 16, 0, 0, 37, 50, 0, 0, 0, 4, 10, 0, 0, 12, 7, 1200, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (79, '盔甲虫7', 81, 19, 45, 16, 0, 0, 18, 50, 0, 1, 1, 8, 20, 0, 0, 12, 7, 1200, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (80, '沙虫', 82, 19, 48, 16, 0, 0, 42, 50, 0, 2, 1, 6, 9, 0, 0, 15, 12, 1000, 1, 0, 2300, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (81, '沙虫7', 82, 19, 48, 19, 0, 0, 21, 50, 0, 2, 2, 16, 40, 0, 0, 15, 12, 1000, 1, 0, 2300, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (82, '威思而小虫', 82, 19, 49, 17, 0, 0, 51, 54, 0, 0, 0, 8, 11, 0, 0, 15, 13, 1000, 1, 0, 2300, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (83, '威思而小虫7', 82, 19, 49, 17, 0, 0, 25, 54, 0, 1, 1, 16, 22, 0, 0, 15, 13, 1000, 1, 0, 2300, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (84, '多角虫', 81, 19, 90, 16, 0, 0, 42, 52, 0, 0, 0, 7, 8, 0, 0, 13, 10, 1300, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (85, '多角虫0', 81, 19, 90, 16, 0, 0, 42, 52, 0, 0, 0, 7, 8, 0, 0, 13, 10, 1300, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (86, '巨型多角虫', 81, 19, 91, 27, 0, 100, 200, 250, 0, 0, 0, 7, 15, 0, 0, 13, 12, 1000, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (87, '狼', 53, 19, 70, 16, 0, 0, 25, 48, 0, 0, 0, 6, 8, 0, 0, 13, 10, 1200, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (88, '蜈蚣', 81, 19, 73, 26, 0, 0, 230, 230, 0, 0, 5, 12, 17, 0, 0, 15, 12, 1500, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (89, '蜈蚣0', 81, 19, 73, 26, 0, 0, 230, 230, 0, 0, 5, 12, 17, 0, 0, 15, 12, 1500, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (90, '触龙神', 107, 33, 140, 50, 0, 100, 1000, 1000, 0, 0, 2000, 20, 35, 0, 0, 30, 30, 2000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (91, '黑色恶蛆', 81, 19, 74, 28, 0, 0, 180, 230, 0, 5, 0, 10, 14, 0, 0, 15, 12, 500, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (92, '黑色恶蛆0', 81, 19, 74, 28, 0, 0, 180, 230, 0, 5, 0, 10, 14, 0, 0, 15, 12, 500, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (93, '钳虫', 81, 19, 120, 31, 0, 0, 250, 270, 0, 5, 7, 15, 25, 0, 0, 15, 13, 1500, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (94, '钳虫0', 81, 19, 120, 31, 0, 0, 250, 270, 0, 5, 7, 15, 25, 0, 0, 15, 13, 1500, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (95, '邪恶钳虫', 81, 19, 121, 50, 0, 10, 1400, 1000, 0, 20, 10, 22, 45, 0, 0, 15, 17, 700, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (96, '邪恶钳虫2', 81, 19, 121, 50, 0, 10, 1400, 1000, 0, 20, 10, 22, 45, 0, 0, 15, 17, 700, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (97, '跳跳蜂', 81, 19, 81, 26, 1, 0, 210, 210, 0, 3, 3, 12, 18, 0, 0, 15, 12, 1500, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (98, '跳跳蜂0', 81, 19, 81, 26, 1, 0, 210, 210, 0, 3, 3, 12, 18, 0, 0, 15, 12, 1500, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (99, '巨型蠕虫', 81, 19, 82, 26, 1, 1, 230, 200, 0, 3, 3, 15, 18, 0, 0, 15, 12, 1200, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (100, '巨型蠕虫0', 81, 19, 82, 26, 1, 1, 230, 200, 0, 3, 3, 15, 18, 0, 0, 15, 12, 1200, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (101, '角蝇', 103, 43, 41, 30, 0, 0, 300, 200, 0, 0, 6, 0, 0, 0, 0, 1, 1, 1200, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (102, '蝙蝠', 81, 31, 42, 5, 0, 0, 5, 3, 0, 0, 0, 0, 22, 0, 0, 5, 18, 1200, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (103, '楔蛾', 105, 52, 39, 32, 0, 1, 350, 220, 0, 0, 5, 13, 18, 0, 0, 15, 12, 600, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (104, '红野猪', 81, 19, 110, 32, 1, 0, 320, 330, 0, 0, 8, 18, 25, 0, 0, 15, 13, 1200, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (105, '红野猪0', 81, 19, 110, 32, 1, 1, 320, 330, 0, 0, 8, 18, 25, 0, 0, 15, 13, 1200, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (106, '红野猪3', 81, 19, 110, 40, 1, 1, 320, 310, 0, 0, 13, 18, 30, 0, 0, 15, 13, 1200, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (107, '红野猪7', 81, 19, 110, 40, 1, 1, 160, 310, 0, 5, 26, 36, 60, 0, 0, 15, 13, 1200, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (108, '黑野猪', 81, 19, 111, 35, 1, 0, 380, 310, 0, 10, 0, 20, 26, 0, 0, 15, 13, 800, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (109, '黑野猪0', 81, 19, 111, 35, 1, 1, 380, 310, 0, 10, 0, 20, 26, 0, 0, 15, 13, 800, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (110, '黑野猪3', 81, 19, 111, 40, 1, 1, 380, 500, 0, 15, 0, 20, 32, 0, 0, 15, 13, 800, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (111, '黑野猪7', 81, 19, 111, 40, 1, 1, 190, 310, 0, 30, 5, 40, 64, 0, 0, 15, 13, 800, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (112, '白野猪', 81, 19, 112, 50, 1, 10, 1600, 1000, 0, 15, 15, 30, 55, 0, 0, 25, 17, 600, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (113, '白野猪0', 81, 19, 112, 50, 1, 10, 1600, 1000, 0, 18, 18, 15, 70, 0, 0, 25, 17, 600, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (114, '白野猪7', 81, 19, 112, 50, 1, 10, 800, 2000, 0, 36, 36, 30, 140, 0, 0, 25, 17, 600, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (115, '蝎蛇', 81, 19, 130, 35, 1, 0, 360, 330, 0, 5, 3, 22, 28, 0, 0, 15, 13, 600, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (116, '蝎蛇0', 81, 19, 130, 35, 1, 1, 360, 330, 0, 5, 3, 22, 28, 0, 0, 15, 13, 600, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (117, '蝎蛇3', 81, 19, 130, 40, 1, 1, 360, 330, 0, 8, 5, 33, 42, 0, 0, 15, 14, 600, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (118, '蝎蛇7', 81, 19, 130, 40, 1, 1, 180, 330, 0, 16, 10, 44, 56, 0, 0, 15, 14, 600, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (119, '邪恶毒蛇', 81, 19, 164, 50, 1, 10, 1600, 1100, 0, 20, 30, 30, 65, 0, 0, 25, 17, 600, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (120, '大老鼠', 81, 19, 46, 24, 1, 1, 330, 385, 0, 3, 5, 12, 23, 0, 0, 15, 12, 1200, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (121, '大老鼠0', 81, 19, 46, 24, 1, 1, 330, 385, 0, 3, 5, 12, 23, 0, 0, 15, 12, 1200, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (122, '祖玛弓箭手', 104, 45, 47, 40, 1, 1, 370, 385, 0, 10, 10, 12, 18, 0, 0, 15, 13, 1000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (123, '祖玛弓箭手0', 104, 45, 47, 40, 1, 10, 370, 385, 0, 11, 11, 13, 20, 0, 0, 15, 14, 1000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (124, '祖玛弓箭手3', 104, 45, 47, 40, 1, 1, 370, 800, 0, 15, 15, 26, 35, 0, 0, 15, 17, 900, 1, 0, 1600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (125, '祖玛雕像', 101, 47, 61, 42, 1, 1, 450, 495, 0, 12, 12, 20, 32, 0, 0, 17, 13, 900, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (126, '祖玛雕像0', 101, 47, 61, 42, 1, 1, 450, 495, 0, 13, 13, 21, 34, 0, 0, 17, 13, 900, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (127, '祖玛雕像3', 101, 47, 61, 42, 1, 10, 450, 900, 0, 18, 18, 30, 48, 0, 0, 17, 17, 700, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (128, '祖玛卫士', 101, 47, 62, 43, 1, 1, 480, 495, 0, 15, 15, 22, 34, 0, 0, 17, 13, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (129, '祖玛卫士0', 101, 47, 62, 43, 1, 1, 480, 495, 0, 15, 15, 22, 34, 0, 0, 17, 14, 700, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (130, '祖玛卫士3', 101, 47, 62, 46, 1, 10, 480, 1000, 0, 20, 20, 33, 51, 0, 0, 17, 17, 350, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (131, '祖玛卫士00', 101, 47, 62, 50, 1, 100, 2000, 1400, 0, 15, 15, 35, 60, 0, 0, 25, 18, 350, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (132, '祖玛教主', 102, 49, 63, 60, 1, 100, 3000, 3000, 0, 20, 20, 40, 65, 0, 0, 32, 30, 300, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (133, '练功师', 55, 19, 72, 99, 0, 0, 1, 1000, 0, 0, 0, 1, 1, 0, 0, 15, 15, 1200, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (134, '卫士', 11, 12, 0, 99, 0, 1, 1, 9999, 0, 100, 100, 200, 200, 200, 200, 200, 200, 500, 1, 0, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (135, '弓箭手', 112, 45, 71, 99, 0, 0, 1, 2000, 0, 20, 20, 50, 120, 0, 0, 20, 15, 500, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (136, '弓箭守卫', 112, 45, 71, 80, 0, 0, 1, 2000, 0, 30, 30, 50, 120, 0, 0, 18, 15, 500, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (137, '护卫', 52, 45, 71, 99, 0, 0, 1, 9999, 0, 20, 20, 200, 200, 0, 0, 20, 15, 500, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (138, '带刀护卫', 11, 24, 2, 99, 0, 1, 1, 9999, 0, 200, 200, 500, 800, 200, 200, 200, 200, 500, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (139, '恶魔弓箭手', 112, 45, 47, 99, 0, 100, 1, 4000, 30, 30, 30, 50, 120, 0, 0, 18, 15, 500, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (140, '恶魔弓箭手0', 112, 45, 47, 99, 0, 100, 1, 4000, 30, 30, 30, 50, 120, 0, 0, 18, 15, 500, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (141, '沙巴克城门', 110, 99, 900, 60, 0, 0, 1, 10000, 0, 20, 20, 0, 0, 0, 0, 15, 1, 1000, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (142, '沙巴克左城墙', 111, 98, 904, 60, 0, 0, 1, 5000, 0, 20, 99, 0, 0, 0, 0, 15, 1, 1000, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (143, '沙巴克中城墙', 111, 98, 905, 60, 0, 0, 1, 5000, 0, 20, 99, 0, 0, 0, 0, 15, 1, 1000, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (144, '沙巴克右城墙', 111, 98, 906, 60, 0, 0, 1, 5000, 0, 20, 99, 0, 0, 0, 0, 15, 1, 1000, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (145, '变异骷髅', 100, 23, 37, 20, 1, 0, 1, 60, 0, 8, 10, 8, 15, 0, 0, 15, 15, 1200, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (146, '神兽', 113, 54, 170, 32, 1, 0, 5, 100, 0, 8, 6, 6, 20, 0, 0, 10, 10, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (147, '神兽1', 114, 55, 171, 32, 1, 0, 1, 300, 0, 36, 30, 30, 45, 0, 0, 15, 18, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (148, '鹰卫', 104, 45, 71, 19, 0, 10, 1, 2000, 0, 80, 10, 80, 130, 0, 0, 20, 15, 700, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (149, '虎卫', 81, 45, 72, 19, 0, 10, 1, 1500, 0, 10, 80, 50, 125, 0, 0, 20, 15, 700, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (150, '蜜蜂', 81, 31, 42, 45, 0, 0, 100, 100, 0, 6, 10, 15, 30, 0, 0, 15, 18, 1200, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (151, '变异骷髅0', 87, 19, 37, 99, 1, 10, 250, 500, 0, 3, 3, 12, 18, 0, 0, 15, 15, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (152, '神兽2', 100, 55, 171, 32, 100, 100, 800, 1500, 0, 12, 24, 25, 36, 0, 0, 15, 15, 1500, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (153, '神兽3', 100, 55, 171, 32, 100, 100, 800, 1500, 0, 12, 24, 25, 36, 0, 0, 15, 15, 1500, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (154, '神兽0', 81, 55, 171, 60, 100, 100, 1000, 2400, 0, 15, 15, 22, 34, 0, 0, 15, 15, 1500, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (155, '圣兽', 132, 54, 272, 48, 1, 0, 1, 350, 0, 28, 26, 16, 30, 0, 0, 10, 25, 600, 1, 0, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (156, '圣兽1', 133, 55, 273, 48, 1, 0, 1, 350, 0, 56, 50, 50, 60, 0, 0, 15, 25, 600, 1, 0, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (157, '白虎', 81, 19, 222, 50, 1, 1000, 7000, 5000, 0, 100, 200, 80, 150, 0, 0, 24, 30, 500, 1, 0, 900, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (158, '月灵', 108, 100, 172, 47, 0, 100, 1, 600, 0, 30, 32, 40, 80, 20, 30, 20, 20, 600, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (159, '火灵', 108, 100, 330, 53, 0, 100, 1, 800, 0, 40, 45, 50, 120, 30, 50, 35, 50, 500, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (160, '石墓尸王', 95, 41, 50, 37, 1, 100, 3000, 3000, 0, 10, 10, 20, 40, 0, 0, 15, 15, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (161, '千年树妖', 115, 34, 141, 60, 1, 100, 5000, 5000, 0, 20, 0, 20, 50, 0, 0, 35, 35, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (162, '暗之黑锷蜘蛛', 81, 19, 118, 60, 0, 10, 1000, 2800, 0, 10, 20, 25, 60, 0, 0, 15, 19, 500, 4, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (163, '剧毒蜘蛛', 82, 19, 163, 50, 0, 1, 1000, 1200, 0, 10, 15, 22, 35, 0, 0, 15, 8, 900, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (164, '月魔蜘蛛', 105, 19, 113, 52, 0, 100, 500, 800, 0, 10, 10, 30, 45, 0, 0, 15, 17, 500, 4, 0, 1600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (165, '月魔蜘蛛0', 105, 19, 113, 52, 0, 100, 500, 800, 0, 10, 10, 30, 45, 0, 0, 15, 17, 500, 4, 0, 1600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (166, '月魔蜘蛛7', 105, 19, 113, 52, 0, 100, 250, 800, 0, 10, 10, 30, 45, 0, 0, 15, 17, 500, 4, 0, 1600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (167, '暴牙蜘蛛', 93, 22, 114, 50, 0, 0, 450, 500, 0, 5, 5, 15, 20, 0, 0, 15, 17, 700, 1, 0, 1600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (168, '暴牙蜘蛛0', 93, 22, 114, 50, 0, 0, 450, 500, 0, 5, 5, 15, 20, 0, 0, 15, 17, 700, 1, 0, 1600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (169, '暴牙蜘蛛7', 93, 22, 114, 50, 0, 0, 225, 500, 0, 10, 10, 30, 40, 0, 0, 15, 17, 700, 1, 0, 1600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (170, '钢牙蜘蛛', 81, 19, 114, 53, 0, 1, 550, 800, 0, 15, 15, 16, 30, 0, 0, 15, 17, 700, 1, 0, 1700, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (171, '钢牙蜘蛛0', 81, 19, 114, 53, 0, 1, 550, 800, 0, 15, 15, 16, 30, 0, 0, 15, 17, 700, 1, 0, 1700, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (172, '钢牙蜘蛛3', 81, 19, 114, 73, 0, 1, 1000, 1200, 0, 15, 15, 25, 50, 0, 0, 15, 19, 700, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (173, '钢牙蜘蛛7', 81, 19, 114, 73, 0, 1, 500, 1200, 0, 30, 30, 32, 60, 0, 0, 15, 19, 700, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (174, '天狼蜘蛛', 81, 19, 119, 40, 0, 0, 480, 500, 0, 9, 19, 26, 35, 0, 0, 15, 17, 700, 3, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (175, '天狼蜘蛛0', 81, 19, 119, 40, 0, 0, 480, 500, 0, 9, 19, 26, 35, 0, 0, 15, 17, 700, 3, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (176, '天狼蜘蛛3', 81, 19, 119, 60, 0, 1, 500, 600, 0, 10, 20, 26, 35, 0, 0, 15, 19, 700, 3, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (177, '天狼蜘蛛7', 81, 19, 119, 60, 0, 1, 250, 600, 0, 20, 40, 52, 70, 0, 0, 15, 19, 700, 3, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (178, '黑锷蜘蛛', 81, 19, 118, 53, 0, 0, 550, 800, 0, 10, 20, 20, 36, 0, 0, 15, 17, 500, 4, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (179, '黑锷蜘蛛0', 81, 19, 118, 53, 0, 0, 550, 800, 0, 10, 20, 20, 36, 0, 0, 15, 17, 500, 4, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (180, '黑锷蜘蛛3', 81, 19, 118, 73, 0, 1, 1000, 1200, 0, 10, 20, 30, 54, 0, 0, 15, 19, 500, 4, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (181, '黑锷蜘蛛7', 81, 19, 118, 73, 0, 1, 275, 800, 0, 20, 20, 60, 108, 0, 0, 15, 19, 500, 4, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (182, '幻影蜘蛛', 116, 35, 132, 53, 0, 0, 350, 300, 0, 3, 10, 0, 0, 0, 0, 1, 1, 0, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (183, '花吻蜘蛛', 119, 19, 117, 54, 0, 1, 550, 800, 0, 10, 15, 20, 40, 0, 0, 15, 17, 1000, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (184, '花吻蜘蛛0', 119, 19, 117, 54, 0, 1, 550, 800, 0, 10, 15, 20, 40, 0, 0, 15, 17, 1000, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (185, '花吻蜘蛛3', 119, 19, 117, 74, 0, 1, 1000, 1200, 0, 12, 17, 27, 51, 0, 0, 15, 19, 500, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (186, '花吻蜘蛛7', 119, 19, 117, 54, 0, 1, 275, 800, 0, 20, 20, 40, 80, 0, 0, 15, 17, 1000, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (187, '爆裂蜘蛛', 117, 36, 133, 52, 0, 100, 7, 10, 0, 0, 0, 20, 40, 0, 0, 30, 30, 500, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (188, '邪恶巨人', 81, 19, 116, 55, 1, 10, 1500, 5000, 0, 20, 20, 40, 80, 0, 0, 20, 19, 600, 4, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (189, '血僵尸', 81, 19, 115, 55, 1, 10, 1500, 1200, 0, 10, 12, 26, 44, 0, 0, 20, 17, 600, 4, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (190, '血僵尸0', 81, 19, 115, 65, 1, 10, 1500, 1200, 0, 10, 12, 26, 44, 0, 0, 20, 19, 600, 4, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (191, '血巨人', 81, 19, 115, 55, 1, 10, 1000, 1400, 0, 15, 15, 35, 70, 0, 0, 20, 17, 800, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (192, '血巨人0', 81, 19, 115, 55, 1, 10, 1000, 2000, 0, 15, 15, 35, 70, 0, 0, 20, 17, 800, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (193, '双头血魔', 92, 21, 92, 60, 1, 100, 3000, 3000, 0, 20, 20, 30, 55, 0, 0, 22, 18, 500, 5, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (194, '双头金刚', 92, 21, 93, 60, 1, 100, 3000, 3000, 0, 20, 20, 30, 55, 0, 0, 22, 18, 500, 5, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (195, '赤月恶魔', 115, 34, 131, 70, 1, 100, 5000, 5000, 0, 18, 18, 25, 70, 0, 0, 25, 25, 0, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (196, '虹魔蝎卫', 81, 60, 153, 55, 1, 10, 2500, 2000, 0, 26, 3, 28, 52, 55, 0, 40, 25, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (197, '虹魔蝎卫0', 81, 60, 153, 55, 1, 10, 2500, 2000, 0, 26, 3, 28, 52, 55, 0, 40, 25, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (198, '虹魔猪卫', 81, 61, 103, 55, 1, 10, 2000, 1500, 0, 15, 10, 25, 58, 0, 0, 30, 18, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (199, '虹魔猪卫0', 81, 61, 103, 55, 1, 10, 2000, 1500, 0, 15, 10, 25, 58, 0, 0, 30, 18, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (200, '虹魔猪卫1', 81, 61, 103, 55, 1, 10, 2000, 1500, 0, 15, 10, 25, 58, 0, 0, 30, 18, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (201, '虹魔教主', 81, 62, 122, 66, 1, 100, 3000, 3000, 0, 15, 20, 50, 70, 0, 0, 60, 50, 500, 2, 0, 1400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (202, '虹魔教主4', 81, 62, 122, 66, 1, 100, 3000, 3000, 0, 15, 20, 50, 70, 0, 0, 60, 50, 500, 1, 0, 1400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (203, '宝箱', 107, 19, 166, 35, 1, 100, 100, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (204, '宝箱1', 107, 19, 166, 35, 1, 100, 100, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (205, '宝箱2', 107, 19, 166, 35, 1, 100, 100, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (206, '宝箱3', 107, 19, 166, 35, 1, 100, 100, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (207, '宝箱4', 107, 19, 166, 35, 1, 100, 100, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (208, '宝箱5', 107, 19, 166, 35, 1, 100, 100, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (209, '宝箱6', 107, 19, 166, 35, 1, 100, 100, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (210, '宝箱7', 107, 19, 166, 35, 1, 100, 100, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (211, '宝箱8', 107, 34, 166, 50, 0, 100, 100, 100, 0, 0, 0, 4, 30, 0, 0, 15, 8, 900, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (212, '恶灵僵尸', 81, 19, 190, 35, 1, 0, 190, 180, 0, 3, 3, 15, 25, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (213, '恶灵僵尸0', 81, 19, 190, 35, 1, 0, 190, 180, 0, 3, 3, 15, 25, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (214, '恶灵尸王', 81, 64, 191, 35, 1, 0, 420, 500, 0, 3, 5, 20, 38, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (215, '恶灵尸王0', 81, 64, 191, 35, 1, 0, 420, 500, 0, 3, 5, 20, 38, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (216, '骷髅锤兵', 81, 66, 192, 35, 1, 0, 450, 500, 0, 3, 5, 20, 42, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (217, '骷髅锤兵0', 81, 66, 192, 35, 1, 0, 450, 500, 0, 3, 5, 20, 42, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (218, '骷髅长枪兵', 81, 67, 193, 35, 1, 0, 380, 390, 0, 3, 6, 20, 42, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (219, '骷髅长枪兵0', 81, 67, 193, 35, 1, 0, 380, 390, 0, 3, 6, 20, 42, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (220, '骷髅刀斧手', 81, 67, 194, 35, 1, 0, 420, 440, 0, 5, 5, 22, 45, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (221, '骷髅刀斧手0', 81, 67, 194, 35, 1, 0, 420, 440, 0, 5, 5, 22, 45, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (222, '骷髅弓箭手', 104, 67, 195, 35, 1, 0, 190, 190, 0, 2, 2, 15, 25, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (223, '骷髅弓箭手0', 104, 67, 195, 35, 1, 0, 190, 190, 0, 2, 2, 15, 25, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (224, '牛头魔', 81, 19, 200, 30, 0, 0, 320, 330, 0, 5, 5, 25, 35, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (225, '牛头魔0', 81, 19, 200, 30, 0, 0, 320, 330, 0, 5, 5, 25, 35, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (226, '牛魔斗士', 81, 19, 201, 45, 1, 0, 420, 460, 0, 8, 10, 25, 45, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (227, '牛魔斗士0', 81, 19, 201, 45, 1, 0, 420, 460, 0, 8, 10, 25, 45, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (228, '牛魔战士', 81, 19, 202, 45, 1, 0, 390, 420, 0, 8, 7, 20, 45, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (229, '牛魔战士0', 81, 19, 202, 45, 1, 0, 390, 420, 0, 8, 7, 20, 45, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (230, '牛魔侍卫', 81, 19, 203, 45, 1, 0, 500, 500, 0, 10, 10, 22, 45, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (231, '牛魔侍卫0', 81, 19, 203, 45, 1, 0, 500, 500, 0, 10, 10, 22, 45, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (232, '牛魔将军', 81, 19, 204, 45, 1, 10, 780, 750, 0, 12, 15, 30, 50, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (233, '牛魔将军0', 81, 19, 204, 45, 1, 10, 780, 750, 0, 12, 15, 30, 50, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (234, '牛魔法师', 81, 70, 205, 45, 1, 10, 420, 360, 0, 9, 12, 20, 75, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (235, '牛魔法师0', 81, 70, 205, 45, 1, 10, 420, 360, 0, 9, 12, 20, 75, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (236, '牛魔祭司', 81, 71, 206, 45, 1, 10, 750, 750, 0, 10, 13, 40, 65, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (237, '牛魔祭司0', 81, 71, 206, 45, 1, 10, 750, 750, 0, 10, 13, 40, 65, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (238, '黄泉教主', 81, 63, 196, 60, 1, 100, 3000, 3000, 0, 20, 24, 45, 70, 0, 0, 25, 18, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (239, '黄泉教主4', 81, 63, 196, 60, 1, 100, 3000, 3000, 0, 20, 24, 45, 70, 0, 0, 25, 18, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (240, '恶灵教主', 81, 63, 196, 60, 1, 100, 3000, 3000, 0, 20, 24, 45, 70, 0, 0, 25, 18, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (241, '恶灵教主4', 81, 63, 196, 60, 1, 100, 3000, 3000, 0, 20, 24, 45, 70, 0, 0, 25, 18, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (242, '牛魔王', 81, 72, 207, 60, 1, 100, 5000, 3000, 0, 20, 24, 45, 70, 0, 0, 25, 18, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (243, '牛魔王4', 81, 72, 207, 60, 1, 100, 5000, 3000, 0, 20, 24, 45, 70, 0, 0, 25, 18, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (244, '暗之牛魔王', 81, 72, 207, 60, 1, 100, 3000, 1000, 0, 0, 2000, 45, 70, 0, 0, 25, 18, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (245, '暗之骷髅精灵', 89, 14, 150, 50, 1, 1, 600, 4000, 0, 5, 100, 30, 60, 0, 0, 15, 14, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (246, '暗之沃玛教主', 92, 21, 34, 60, 1, 1, 4000, 4000, 0, 20, 20, 40, 65, 0, 0, 30, 20, 400, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (247, '暗之双头血魔', 92, 21, 92, 60, 1, 100, 5000, 5000, 0, 20, 20, 30, 55, 0, 0, 22, 18, 400, 5, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (248, '暗之双头金刚', 105, 52, 93, 60, 1, 100, 5000, 5000, 0, 20, 20, 30, 55, 0, 0, 22, 18, 400, 5, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (249, '暗之虹魔教主', 81, 62, 122, 66, 1, 100, 5000, 5000, 0, 15, 15, 30, 45, 0, 0, 35, 30, 400, 1, 0, 1400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (250, '暗之黄泉教主', 81, 63, 196, 60, 1, 100, 3000, 3000, 0, 20, 24, 45, 70, 0, 0, 25, 18, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (251, '重装使者', 92, 21, 34, 50, 0, 100, 5000, 5000, 0, 35, 25, 65, 75, 0, 0, 20, 15, 400, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (252, '鸡1', 97, 11, 160, 50, 0, 100, 100, 2000, 0, 40, 40, 20, 150, 0, 0, 17, 20, 600, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (253, '鹿1', 97, 11, 161, 50, 0, 100, 100, 1200, 0, 35, 35, 0, 100, 0, 0, 17, 20, 600, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (254, '稻草人1', 83, 18, 27, 50, 1, 100, 100, 1000, 0, 30, 30, 0, 90, 0, 0, 17, 20, 500, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (255, '巨型多角虫1', 81, 19, 91, 27, 0, 100, 100, 900, 0, 0, 100, 0, 80, 0, 0, 17, 20, 600, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (256, '半兽勇士1', 81, 19, 102, 50, 0, 1, 300, 900, 0, 100, 0, 0, 80, 0, 0, 17, 20, 600, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (257, '骷髅精灵1', 89, 14, 150, 50, 1, 1, 100, 800, 0, 0, 100, 0, 60, 0, 0, 17, 20, 600, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (258, '尸王1', 81, 19, 152, 50, 1, 1, 800, 800, 0, 100, 0, 0, 60, 0, 0, 17, 20, 600, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (259, '沃玛卫士1', 97, 19, 151, 50, 1, 1, 100, 600, 0, 20, 20, 0, 50, 0, 0, 17, 19, 600, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (260, '邪恶钳虫1', 81, 19, 121, 60, 0, 1, 100, 600, 0, 20, 20, 0, 50, 0, 0, 17, 18, 600, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (261, '白野猪1', 81, 19, 112, 50, 1, 1, 100, 400, 0, 0, 0, 0, 40, 0, 0, 17, 20, 600, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (262, '邪恶毒蛇1', 81, 19, 164, 50, 1, 1, 100, 200, 0, 0, 0, 0, 30, 0, 0, 17, 18, 600, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (263, '沃玛教主1', 92, 21, 34, 60, 1, 1, 100, 100, 0, 0, 0, 0, 20, 0, 0, 17, 20, 600, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (264, '恶灵僵尸8', 81, 19, 190, 35, 1, 0, 380, 180, 0, 3, 3, 15, 25, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (265, '恶灵尸王8', 81, 19, 143, 50, 1, 0, 840, 500, 0, 3, 5, 20, 38, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (266, '尸王8', 81, 19, 152, 40, 1, 0, 1600, 500, 0, 3, 3, 18, 36, 0, 0, 15, 15, 1500, 1, 0, 2800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (267, '骷髅精灵8', 89, 14, 150, 35, 1, 0, 1200, 1000, 0, 5, 4, 7, 24, 0, 0, 15, 12, 1200, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (268, '红野猪8', 81, 19, 110, 32, 1, 0, 640, 330, 0, 0, 8, 18, 25, 0, 0, 15, 13, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (269, '黑野猪8', 81, 19, 111, 35, 1, 0, 760, 310, 0, 10, 0, 20, 26, 0, 0, 15, 12, 800, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (270, '楔蛾8', 105, 52, 39, 32, 0, 1, 700, 220, 0, 0, 5, 13, 18, 0, 0, 15, 12, 600, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (271, '蝎蛇8', 81, 19, 130, 35, 1, 0, 720, 330, 0, 5, 3, 22, 28, 0, 0, 15, 13, 600, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (272, '火焰沃玛8', 91, 20, 31, 31, 1, 0, 580, 340, 0, 0, 0, 14, 26, 0, 0, 20, 13, 800, 1, 0, 1700, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (273, '沃玛勇士8', 97, 19, 32, 30, 1, 0, 560, 285, 0, 3, 2, 16, 28, 0, 0, 15, 12, 1000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (274, '沃玛战将8', 97, 19, 33, 30, 1, 0, 560, 285, 0, 3, 2, 15, 29, 0, 0, 15, 12, 1000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (275, '沃玛战士8', 97, 19, 30, 30, 1, 0, 520, 265, 0, 3, 2, 14, 28, 0, 0, 15, 12, 1000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (276, '沃玛卫士8', 97, 19, 151, 50, 1, 0, 2400, 2000, 0, 8, 8, 22, 42, 0, 0, 20, 17, 600, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (277, '钳虫8', 81, 19, 120, 31, 0, 0, 500, 270, 0, 5, 7, 15, 25, 0, 0, 15, 13, 1500, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (278, '跳跳蜂8', 81, 19, 81, 26, 1, 0, 460, 210, 0, 3, 3, 12, 18, 0, 0, 15, 12, 1500, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (279, '蜈蚣8', 81, 19, 73, 26, 0, 0, 460, 230, 0, 0, 5, 12, 17, 0, 0, 15, 12, 1500, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (280, '黑色恶蛆8', 81, 19, 74, 28, 0, 0, 360, 230, 0, 5, 0, 10, 14, 0, 0, 15, 12, 500, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (281, '巨型蠕虫8', 81, 19, 82, 26, 1, 0, 460, 300, 0, 3, 3, 15, 18, 0, 0, 15, 12, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (282, '邪恶钳虫8', 81, 19, 121, 60, 0, 0, 2000, 2000, 0, 20, 10, 22, 45, 0, 0, 15, 17, 700, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (283, '触龙神8', 107, 33, 140, 50, 1, 1, 1000, 2000, 0, 0, 2000, 20, 35, 0, 0, 30, 30, 200, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (284, '骷髅长枪兵8', 81, 19, 145, 35, 1, 0, 760, 390, 0, 3, 6, 20, 42, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (285, '骷髅锤兵8', 81, 19, 192, 35, 1, 0, 880, 500, 0, 3, 5, 20, 42, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (286, '骷髅刀斧手8', 81, 19, 194, 35, 1, 0, 840, 440, 0, 5, 5, 22, 45, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (287, '骷髅弓箭手8', 104, 45, 195, 35, 1, 1, 380, 190, 0, 20, 10, 12, 30, 0, 0, 15, 30, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (288, '黄泉教主8', 92, 19, 148, 60, 1, 1, 6000, 8000, 0, 20, 24, 45, 70, 0, 0, 25, 18, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (289, '白野猪8', 81, 19, 112, 50, 1, 0, 1000, 2000, 0, 15, 15, 30, 55, 0, 0, 25, 17, 600, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (290, '邪恶毒蛇8', 81, 19, 164, 50, 1, 0, 2000, 2200, 0, 20, 30, 30, 65, 0, 0, 25, 17, 600, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (291, '石墓尸王8', 95, 41, 50, 90, 1, 0, 3000, 6000, 0, 10, 10, 20, 40, 0, 0, 15, 15, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (292, '祖玛雕像8', 101, 47, 61, 42, 1, 0, 900, 900, 0, 12, 12, 20, 32, 0, 0, 17, 13, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (293, '祖玛弓箭手8', 104, 45, 47, 40, 1, 0, 740, 800, 0, 10, 10, 12, 18, 0, 0, 15, 13, 900, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (294, '祖玛卫士8', 101, 47, 62, 43, 1, 0, 960, 1000, 0, 15, 15, 22, 34, 0, 0, 17, 13, 350, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (295, '祖玛教主8', 102, 49, 63, 60, 1, 100, 5000, 5000, 0, 20, 20, 40, 65, 0, 0, 32, 30, 300, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (296, '虹魔蝎卫8', 81, 60, 180, 48, 1, 0, 4000, 4000, 0, 26, 3, 28, 28, 55, 0, 40, 25, 500, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (297, '虹魔猪卫8', 81, 61, 181, 48, 1, 0, 3000, 3000, 0, 15, 10, 15, 36, 0, 0, 30, 18, 600, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (298, '虹魔教主8', 81, 62, 182, 66, 1, 1, 5000, 6000, 0, 15, 20, 50, 70, 0, 0, 60, 50, 400, 2, 0, 1400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (299, '天狼蜘蛛8', 118, 19, 119, 40, 0, 0, 960, 500, 0, 9, 19, 26, 35, 0, 0, 15, 17, 500, 3, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (300, '钢牙蜘蛛8', 81, 19, 114, 53, 0, 0, 1100, 800, 0, 10, 20, 16, 36, 0, 0, 15, 17, 700, 1, 0, 1700, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (301, '黑锷蜘蛛8', 81, 19, 118, 53, 0, 0, 1100, 800, 0, 10, 20, 20, 36, 0, 0, 15, 17, 500, 4, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (302, '花吻蜘蛛8', 119, 19, 117, 54, 0, 1, 1100, 800, 0, 8, 11, 18, 34, 0, 0, 15, 17, 500, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (303, '月魔蜘蛛8', 105, 19, 113, 52, 0, 1, 1000, 800, 0, 10, 25, 25, 40, 0, 0, 15, 17, 500, 4, 0, 1600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (304, '血僵尸8', 96, 42, 115, 50, 1, 1, 2000, 2400, 0, 10, 10, 15, 40, 0, 0, 22, 18, 500, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (305, '血巨人8', 81, 19, 115, 55, 1, 1, 2000, 2400, 0, 10, 12, 26, 44, 0, 0, 20, 17, 500, 4, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (306, '双头金刚8', 105, 19, 93, 60, 1, 1, 3000, 6000, 0, 20, 20, 30, 55, 0, 0, 22, 18, 400, 5, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (307, '双头血魔8', 81, 19, 92, 60, 1, 1, 3000, 6000, 0, 20, 20, 30, 55, 0, 0, 22, 18, 400, 5, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (308, '牛魔侍卫8', 81, 19, 203, 45, 1, 0, 500, 500, 0, 10, 10, 15, 30, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (309, '牛魔法师8', 94, 70, 205, 45, 1, 0, 840, 360, 0, 9, 12, 15, 45, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (310, '牛魔祭司8', 92, 71, 206, 45, 1, 0, 1500, 750, 0, 10, 13, 30, 45, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (311, '牛魔将军8', 81, 72, 204, 45, 1, 0, 1560, 750, 0, 12, 15, 20, 35, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (312, '牛魔王8', 92, 72, 165, 50, 1, 0, 5000, 5000, 0, 20, 24, 45, 60, 0, 0, 25, 18, 400, 2, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (313, '暗之牛魔王8', 92, 72, 165, 60, 1, 0, 2000, 2000, 0, 5, 2000, 48, 65, 0, 0, 25, 18, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (314, '暗之触龙神8', 107, 33, 140, 55, 0, 1, 2000, 5000, 0, 2000, 0, 30, 45, 0, 0, 30, 30, 200, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (315, '赤月恶魔8', 115, 34, 131, 70, 1, 1, 6000, 10000, 0, 18, 18, 35, 80, 0, 0, 25, 25, 0, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (316, '电僵王8', 94, 40, 40, 50, 1, 0, 3000, 5000, 0, 0, 0, 12, 16, 0, 0, 13, 11, 1000, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (317, '变异史莱姆', 81, 19, 82, 50, 1, 0, 2800, 4000, 0, 3, 3, 15, 18, 0, 0, 15, 12, 600, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (318, '变异史莱姆8', 81, 19, 82, 50, 1, 0, 2800, 4000, 0, 3, 3, 15, 18, 0, 0, 15, 12, 600, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (319, '地藏魔王', 81, 72, 207, 60, 1, 100, 5000, 5000, 0, 50, 80, 55, 85, 0, 0, 25, 18, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (320, '雪人王', 83, 10, 1, 50, 0, 10, 1000, 1800, 0, 15, 15, 25, 60, 0, 0, 12, 7, 600, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (321, '多钩猫王', 81, 17, 25, 50, 0, 10, 1000, 1200, 0, 15, 15, 25, 60, 0, 0, 7, 5, 700, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (322, '钉耙猫王', 81, 17, 26, 50, 0, 10, 1000, 1200, 0, 15, 15, 25, 60, 0, 0, 7, 5, 700, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (323, '电僵王', 94, 40, 40, 50, 1, 10, 1000, 3000, 0, 10, 15, 20, 140, 0, 0, 50, 50, 600, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (324, '地狱犬', 53, 19, 70, 50, 1, 1, 1000, 1400, 0, 30, 50, 50, 100, 0, 0, 25, 35, 600, 1, 0, 1300, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (325, '红蛇王', 81, 19, 36, 50, 1, 1, 1000, 1500, 0, 30, 60, 50, 100, 0, 0, 35, 35, 600, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (326, '虎蛇王', 81, 19, 38, 50, 1, 1, 1000, 1500, 0, 20, 30, 50, 100, 0, 0, 35, 35, 600, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (327, '蜈蚣王', 81, 19, 73, 50, 1, 1, 1000, 2300, 0, 30, 80, 60, 120, 0, 0, 30, 60, 600, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (328, '蝎子王', 84, 32, 83, 50, 1, 1, 1000, 450, 0, 80, 200, 70, 130, 0, 0, 12, 30, 600, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (329, '蛤蟆王', 83, 19, 162, 50, 1, 1, 1000, 2300, 0, 30, 60, 50, 110, 0, 0, 40, 70, 600, 1, 0, 1900, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (330, '楔蛾王', 105, 52, 39, 50, 1, 1, 1000, 3000, 0, 40, 40, 50, 110, 0, 0, 50, 50, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (331, '雪蚕王', 90, 16, 24, 50, 1, 1, 1000, 1800, 0, 40, 40, 50, 110, 0, 0, 18, 38, 500, 1, 0, 2300, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (332, '圣域教主', 137, 92, 34, 60, 1, 0, 3000, 5000, 0, 30, 30, 40, 80, 0, 0, 15, 30, 400, 1, 1200, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (333, '圣域牛魔王', 92, 72, 207, 60, 1, 0, 2000, 4000, 0, 20, 20, 40, 70, 0, 0, 15, 25, 400, 1, 1200, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (334, '圣域修罗', 92, 63, 196, 60, 1, 0, 2000, 3500, 0, 20, 20, 30, 65, 0, 0, 15, 25, 400, 1, 1200, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (335, '圣域魔魂', 81, 53, 153, 60, 1, 0, 2000, 4000, 0, 20, 20, 40, 75, 0, 0, 15, 25, 400, 1, 1200, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (336, '圣域金刚', 53, 19, 93, 60, 1, 0, 2000, 3000, 0, 20, 15, 30, 60, 0, 0, 15, 25, 400, 1, 1200, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (337, '圣域血魔', 92, 21, 92, 60, 1, 0, 2000, 3000, 0, 15, 20, 30, 60, 0, 0, 15, 25, 400, 1, 1200, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (338, '圣域猪卫', 81, 19, 103, 48, 1, 0, 2000, 2500, 0, 20, 20, 22, 47, 0, 0, 15, 25, 600, 1, 1200, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (339, '圣域蝎卫', 81, 53, 153, 50, 1, 0, 2000, 3000, 0, 22, 23, 17, 51, 0, 0, 15, 20, 500, 1, 1200, 1600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (340, '圣域护卫', 97, 19, 151, 52, 1, 0, 1800, 2000, 0, 20, 23, 23, 51, 0, 0, 15, 20, 700, 1, 1200, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (341, '圣域将军', 81, 19, 204, 50, 1, 0, 1600, 1500, 0, 15, 18, 25, 57, 0, 0, 15, 20, 700, 1, 1200, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (342, '圣域祭司', 81, 71, 205, 48, 1, 0, 1500, 1500, 0, 12, 17, 13, 44, 0, 0, 15, 20, 700, 1, 1200, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (343, '圣域蛇王', 81, 19, 164, 50, 1, 0, 2000, 2000, 0, 20, 20, 20, 53, 0, 0, 20, 20, 700, 1, 1200, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (344, '圣域尸王', 81, 19, 152, 55, 1, 0, 2000, 1800, 0, 20, 16, 18, 52, 0, 0, 15, 20, 700, 1, 1200, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (345, '圣域钳虫', 81, 19, 121, 50, 1, 0, 2000, 2500, 0, 20, 20, 16, 55, 0, 0, 15, 20, 700, 1, 1200, 1900, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (346, '圣域月魔', 105, 19, 113, 50, 1, 0, 1000, 1000, 0, 18, 35, 30, 45, 0, 0, 15, 20, 400, 1, 1200, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (347, '圣域半兽人', 83, 19, 100, 45, 1, 0, 500, 500, 0, 8, 12, 9, 23, 0, 0, 15, 6, 800, 1, 1200, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (348, '热血足球', 120, 11, 3, 1, 0, 0, 1, 9999, 0, 100, 100, 0, 0, 0, 0, 10, 10, 400, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (349, '魔龙邪眼', 95, 19, 184, 50, 1, 0, 800, 800, 0, 30, 23, 20, 40, 0, 0, 15, 17, 500, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (350, '魔龙邪眼0', 95, 19, 185, 50, 1, 0, 800, 800, 0, 30, 23, 20, 40, 0, 0, 15, 17, 500, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (351, '魔龙刺蛙', 81, 19, 186, 50, 1, 0, 1000, 1000, 0, 30, 23, 20, 25, 0, 0, 15, 13, 500, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (352, '魔龙刺蛙0', 81, 19, 187, 50, 1, 0, 1000, 1000, 0, 23, 30, 20, 25, 0, 0, 15, 13, 500, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (353, '魔龙血蛙', 81, 19, 183, 50, 1, 0, 600, 600, 0, 30, 23, 20, 32, 0, 0, 15, 17, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (354, '魔龙刀兵', 81, 19, 213, 52, 1, 0, 1200, 1200, 0, 25, 25, 22, 34, 0, 0, 17, 20, 1000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (355, '魔龙刀兵0', 81, 19, 213, 52, 1, 0, 1200, 1200, 0, 28, 28, 26, 40, 0, 0, 17, 20, 1000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (356, '魔龙破甲兵', 81, 19, 214, 52, 1, 0, 1200, 1200, 0, 18, 18, 24, 40, 0, 0, 17, 20, 1000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (357, '魔龙破甲兵0', 81, 19, 214, 52, 1, 0, 1200, 1200, 0, 22, 22, 28, 45, 0, 0, 17, 20, 1000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (358, '魔龙射手', 104, 45, 215, 52, 1, 0, 1000, 1000, 0, 15, 13, 18, 27, 0, 0, 15, 13, 800, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (359, '魔龙射手0', 104, 45, 215, 52, 1, 0, 1000, 1000, 0, 26, 16, 22, 33, 0, 0, 15, 13, 800, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (360, '魔龙巨蛾', 81, 19, 210, 55, 1, 10, 6000, 6000, 0, 30, 45, 35, 75, 0, 0, 15, 20, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (361, '魔龙巨蛾0', 81, 19, 210, 55, 1, 10, 6000, 6000, 0, 30, 45, 35, 75, 0, 0, 15, 20, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (362, '魔龙战将', 92, 21, 212, 55, 1, 10, 6000, 6000, 0, 35, 35, 55, 90, 0, 0, 15, 20, 500, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (363, '魔龙战将0', 92, 21, 212, 55, 1, 10, 6000, 6000, 0, 35, 35, 55, 90, 0, 0, 15, 20, 500, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (364, '魔龙力士', 90, 16, 211, 55, 1, 10, 6000, 6000, 0, 40, 30, 40, 80, 0, 0, 15, 20, 600, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (365, '魔龙力士0', 51, 11, 211, 55, 1, 10, 6000, 6000, 0, 40, 40, 20, 40, 0, 0, 15, 20, 600, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (366, '魔龙力士1', 118, 19, 211, 55, 1, 10, 2000, 2000, 0, 20, 15, 20, 40, 0, 0, 15, 13, 600, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (367, '魔龙教主', 101, 78, 218, 80, 1, 100, 12000, 12000, 0, 50, 40, 120, 150, 0, 0, 30, 30, 400, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (368, '魔龙树妖', 115, 34, 141, 80, 1, 100, 12000, 12000, 0, 50, 40, 90, 150, 0, 0, 50, 50, 0, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (369, '暗之魔龙教主', 101, 78, 218, 100, 0, 100, 32000, 32000, 0, 60, 60, 150, 180, 0, 0, 30, 30, 400, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (370, '天之沃玛战士', 97, 19, 30, 30, 1, 0, 1040, 1060, 0, 3, 2, 14, 28, 0, 0, 15, 12, 1000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (371, '天之沃玛勇士', 97, 19, 32, 30, 1, 0, 1080, 1140, 0, 3, 2, 16, 28, 0, 0, 15, 12, 1000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (372, '天之沃玛战将', 97, 19, 33, 30, 1, 0, 1080, 1140, 0, 3, 2, 15, 29, 0, 0, 15, 12, 1000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (373, '天之火焰沃玛', 91, 20, 31, 31, 1, 0, 1160, 1360, 0, 0, 0, 14, 26, 0, 0, 20, 13, 800, 1, 0, 1700, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (374, '天之红野猪', 81, 19, 110, 32, 1, 0, 1240, 1320, 0, 0, 8, 18, 25, 0, 0, 15, 13, 1200, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (375, '天之黑野猪', 81, 19, 111, 35, 1, 0, 1200, 1240, 0, 10, 0, 20, 26, 0, 0, 15, 13, 800, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (376, '天之黑色恶蛆', 81, 19, 74, 28, 0, 0, 720, 920, 0, 5, 0, 10, 14, 0, 0, 15, 12, 500, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (377, '天之楔蛾', 105, 52, 39, 32, 0, 1, 1400, 880, 0, 0, 5, 13, 18, 0, 0, 15, 12, 600, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (378, '天之骷髅锤兵', 81, 66, 192, 35, 1, 0, 1760, 1760, 0, 3, 5, 20, 42, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (379, '天之骷髅长枪兵', 81, 67, 193, 35, 1, 0, 1560, 1560, 0, 3, 6, 20, 42, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (380, '天之骷髅刀斧手', 81, 67, 194, 35, 1, 0, 1560, 1560, 0, 5, 5, 22, 45, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (381, '天之骷髅弓箭手', 104, 67, 195, 35, 1, 0, 1520, 1520, 0, 2, 2, 15, 25, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (382, '天之牛魔斗士', 81, 19, 201, 45, 1, 0, 1800, 1840, 0, 8, 10, 25, 45, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (383, '天之牛魔侍卫', 81, 19, 203, 45, 1, 0, 2000, 2000, 0, 10, 10, 22, 45, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (384, '天之牛魔将军', 81, 19, 204, 45, 1, 0, 3000, 3120, 0, 12, 15, 30, 50, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (385, '天之牛魔法师', 81, 70, 205, 45, 1, 0, 1680, 1440, 0, 9, 12, 20, 75, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (386, '天之牛魔祭司', 81, 71, 206, 45, 1, 0, 3000, 3000, 0, 10, 13, 40, 65, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (387, '天之祖玛弓箭手', 104, 45, 47, 40, 1, 1, 2800, 3000, 0, 10, 10, 12, 18, 0, 0, 15, 13, 1000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (388, '天之祖玛雕像', 101, 47, 61, 42, 1, 1, 3200, 3600, 0, 12, 12, 20, 32, 0, 0, 17, 13, 900, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (389, '天之祖玛卫士', 101, 47, 62, 43, 1, 1, 3600, 4000, 0, 15, 15, 22, 34, 0, 0, 17, 13, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (390, '天之沃玛卫士', 97, 19, 151, 50, 1, 0, 4000, 4000, 0, 8, 8, 22, 42, 0, 0, 20, 17, 600, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (391, '天之白野猪', 81, 19, 112, 60, 1, 1, 3200, 4000, 0, 15, 15, 30, 55, 0, 0, 25, 17, 500, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (392, '天之邪恶毒蛇', 81, 19, 164, 60, 1, 1, 4400, 4400, 0, 20, 30, 30, 65, 0, 0, 25, 17, 400, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (393, '天之骷髅精灵', 89, 14, 150, 60, 1, 1, 3000, 5000, 0, 15, 14, 24, 58, 0, 0, 15, 12, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (394, '天之牛魔王', 81, 72, 207, 70, 1, 0, 10000, 12000, 0, 20, 24, 45, 70, 0, 0, 25, 18, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (395, '天之虹魔教主', 81, 62, 122, 70, 1, 1, 10000, 12000, 0, 15, 20, 50, 70, 0, 0, 60, 50, 400, 2, 0, 1400, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (396, '天之双头血魔', 92, 21, 92, 70, 1, 1, 12000, 12000, 0, 20, 20, 30, 55, 0, 0, 22, 18, 400, 5, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (397, '天之双头金刚', 105, 52, 93, 70, 1, 1, 12000, 12000, 0, 20, 20, 30, 55, 0, 0, 22, 18, 400, 5, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (398, '天之魔龙巨蛾', 81, 19, 210, 80, 0, 1, 12000, 24000, 0, 30, 45, 35, 75, 0, 0, 15, 20, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (399, '天之魔龙力士', 90, 16, 211, 80, 0, 1, 12000, 24000, 0, 40, 30, 40, 80, 0, 0, 15, 20, 600, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (400, '天之魔龙战将', 92, 21, 212, 80, 0, 1, 12000, 24000, 0, 35, 35, 55, 90, 0, 0, 15, 20, 500, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (401, '天之魔神', 81, 63, 196, 70, 1, 0, 8000, 8000, 0, 20, 24, 45, 70, 0, 0, 25, 18, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (402, '天之魔影巨人', 81, 78, 218, 100, 1, 100, 32000, 32000, 0, 50, 50, 100, 150, 0, 0, 30, 30, 400, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (403, '卧龙守将1', 150, 19, 0, 104, 0, 100, 1000, 1000, 10, 10, 10, 40, 40, 40, 15, 17, 650, 800, 0, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (404, '卧龙守将2', 150, 19, 0, 104, 0, 100, 1000, 1000, 10, 10, 10, 40, 40, 40, 15, 17, 650, 800, 0, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (405, '卧龙守将3', 150, 19, 0, 104, 0, 100, 1000, 1000, 10, 10, 10, 40, 60, 40, 15, 17, 650, 800, 0, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (406, '卧龙守将4', 150, 19, 0, 104, 0, 100, 1000, 1000, 10, 10, 20, 50, 80, 0, 15, 17, 500, 800, 0, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (407, '卧龙守将5', 150, 19, 0, 104, 0, 100, 1000, 1000, 10, 10, 20, 70, 80, 0, 15, 17, 500, 800, 0, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (408, '卧龙守将6', 150, 19, 0, 104, 0, 100, 1000, 1000, 10, 10, 20, 70, 80, 0, 15, 17, 500, 800, 0, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (409, '卧龙守将7', 150, 19, 0, 110, 0, 100, 1000, 1000, 10, 10, 10, 50, 50, 0, 15, 17, 650, 800, 0, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (410, '卧龙守将8', 150, 19, 0, 110, 0, 100, 1000, 1000, 10, 10, 10, 70, 80, 0, 15, 17, 650, 800, 0, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (411, '卧龙守将9', 150, 19, 0, 110, 0, 100, 1000, 1000, 10, 10, 10, 70, 70, 0, 15, 17, 650, 500, 0, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (412, '卧龙守将10', 150, 19, 0, 104, 0, 100, 1000, 1000, 10, 10, 20, 70, 80, 0, 15, 17, 500, 800, 0, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (413, '卧龙守将11', 150, 19, 0, 104, 0, 100, 1000, 1000, 10, 10, 10, 40, 40, 40, 15, 17, 650, 800, 0, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (414, '卧龙守将12', 150, 19, 0, 104, 0, 100, 1000, 1000, 10, 10, 10, 40, 40, 40, 15, 17, 650, 800, 0, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (415, '卧龙守将13', 150, 19, 0, 104, 0, 100, 1000, 1000, 10, 10, 20, 70, 80, 0, 15, 17, 500, 800, 0, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (416, '卧龙守将14', 150, 19, 0, 104, 0, 100, 1000, 1000, 10, 10, 20, 70, 80, 0, 15, 17, 500, 800, 0, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (417, '卧龙守将15', 150, 19, 0, 110, 0, 100, 1000, 1000, 10, 10, 10, 70, 70, 0, 15, 17, 650, 800, 0, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (418, '卧龙守将16', 150, 19, 0, 110, 0, 100, 1000, 1000, 10, 10, 0, 70, 70, 0, 15, 17, 650, 800, 0, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (419, '卧龙道尊', 150, 19, 0, 110, 0, 100, 1200, 3000, 20, 0, 0, 50, 60, 0, 15, 17, 650, 700, 0, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (420, '卧龙道尊1', 150, 19, 0, 110, 0, 100, 2500, 3000, 20, 0, 0, 50, 60, 0, 15, 17, 650, 700, 0, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (421, '卧龙道尊2', 150, 19, 0, 110, 0, 100, 2500, 3000, 20, 0, 0, 50, 60, 0, 15, 17, 650, 700, 0, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (422, '卧龙道尊3', 150, 19, 0, 110, 0, 100, 2500, 3000, 20, 0, 0, 50, 60, 0, 15, 17, 650, 700, 0, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (423, '卧龙道尊4', 150, 19, 0, 110, 0, 100, 2500, 3000, 20, 0, 0, 50, 60, 0, 15, 17, 650, 700, 0, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (424, '卧龙道尊5', 150, 19, 0, 110, 0, 100, 2500, 3000, 20, 0, 0, 50, 60, 0, 15, 17, 650, 700, 0, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (425, '卧龙魔将', 150, 19, 0, 135, 0, 100, 1200, 1500, 20, 0, 0, 80, 90, 0, 15, 17, 650, 700, 0, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (426, '卧龙魔将1', 150, 19, 0, 135, 0, 100, 1200, 1500, 20, 0, 0, 80, 90, 0, 15, 17, 650, 700, 0, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (427, '卧龙魔将2', 150, 19, 0, 135, 0, 100, 1200, 1500, 20, 0, 0, 80, 90, 0, 15, 17, 650, 700, 0, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (428, '卧龙魔将3', 150, 19, 0, 135, 0, 100, 1200, 1500, 20, 0, 0, 80, 90, 0, 15, 17, 650, 700, 0, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (429, '卧龙魔将4', 150, 19, 0, 135, 0, 100, 1200, 1500, 20, 0, 0, 80, 90, 0, 15, 17, 650, 700, 0, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (430, '卧龙魔将5', 150, 19, 0, 135, 0, 100, 1200, 1500, 20, 0, 0, 80, 90, 0, 15, 17, 650, 700, 0, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (431, '卧龙战将', 150, 19, 0, 93, 0, 100, 2500, 3000, 20, 0, 0, 80, 90, 0, 15, 17, 500, 800, 0, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (432, '卧龙战将1', 150, 19, 0, 93, 0, 100, 2500, 3000, 20, 0, 0, 80, 90, 0, 15, 17, 500, 800, 0, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (433, '卧龙战将2', 150, 19, 0, 93, 0, 100, 2500, 3000, 20, 0, 0, 80, 90, 0, 15, 17, 500, 800, 0, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (434, '卧龙战将3', 150, 19, 0, 93, 0, 100, 2500, 3000, 20, 0, 0, 80, 90, 0, 15, 17, 500, 800, 0, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (435, '卧龙战将4', 150, 19, 0, 93, 0, 100, 2500, 3000, 20, 0, 0, 80, 0, 0, 15, 17, 500, 800, 0, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (436, '卧龙战将5', 150, 19, 0, 93, 0, 100, 2500, 3000, 20, 0, 0, 80, 0, 0, 15, 17, 500, 800, 0, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (437, '卧龙战将6', 150, 19, 0, 93, 0, 100, 2500, 3000, 20, 0, 0, 80, 0, 0, 15, 17, 500, 800, 0, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (438, '飞鸿（卧龙）', 150, 19, 0, 100, 0, 200, 3000, 6000, 2000, 0, 0, 0, 0, 30, 60, 35, 50, 700, 1, 0, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (439, '风云（卧龙）', 150, 19, 0, 100, 0, 200, 3000, 6000, 2000, 0, 0, 0, 0, 30, 60, 35, 50, 700, 1, 0, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (440, '青城（卧龙）', 150, 19, 0, 100, 0, 200, 3000, 6000, 2000, 0, 0, 0, 0, 30, 60, 35, 50, 700, 1, 0, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (441, '新月（卧龙）', 150, 19, 0, 100, 0, 200, 3000, 6000, 2000, 0, 0, 0, 0, 30, 60, 35, 50, 700, 1, 0, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (442, '御风（卧龙）', 150, 19, 0, 100, 0, 200, 3000, 6000, 2000, 0, 0, 0, 0, 30, 60, 35, 50, 700, 1, 0, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (443, '斩浪（卧龙）', 150, 19, 0, 100, 0, 200, 3000, 6000, 2000, 0, 0, 0, 0, 30, 60, 35, 50, 700, 1, 0, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (444, '卧龙庄主', 150, 19, 0, 100, 0, 200, 8000, 12000, 5000, 0, 0, 50, 100, 0, 88, 35, 50, 700, 1, 0, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (445, '黑牙蜘蛛', 121, 19, 236, 52, 0, 0, 3000, 500, 0, 22, 32, 60, 70, 0, 0, 22, 20, 700, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (446, '绿魔蜘蛛', 121, 19, 232, 60, 1, 10, 10000, 10000, 0, 45, 32, 60, 70, 0, 0, 24, 20, 400, 1, 0, 900, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (447, '巨镰蜘蛛', 121, 19, 230, 60, 1, 10, 3000, 3000, 0, 32, 45, 65, 85, 0, 0, 24, 22, 700, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (448, '巨镰蜘蛛0', 121, 19, 230, 60, 1, 10, 10000, 10000, 0, 32, 45, 60, 70, 0, 0, 24, 24, 400, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (449, '金杖蜘蛛', 124, 92, 231, 60, 1, 10, 4000, 4000, 0, 28, 41, 68, 92, 0, 0, 18, 22, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (450, '金杖蜘蛛0', 124, 92, 231, 60, 1, 10, 1000, 10000, 0, 28, 41, 72, 100, 0, 0, 18, 22, 500, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (451, '圣殿卫士', 123, 91, 221, 70, 1, 100, 12000, 12000, 0, 38, 50, 60, 120, 0, 0, 24, 30, 500, 1, 0, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (452, '狂热火蜥蜴', 122, 19, 220, 70, 1, 100, 12000, 12000, 0, 35, 45, 80, 100, 0, 0, 24, 50, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (453, '雷炎蛛王', 125, 93, 237, 100, 1, 100, 20000, 20000, 0, 65, 65, 100, 150, 0, 0, 35, 18, 400, 1, 0, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (454, '无显紫色粪虫', 81, 19, 239, 60, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 30, 700, 1, 0, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (455, '蓝牙蜘蛛', 121, 19, 239, 53, 1, 0, 1200, 1200, 0, 26, 36, 60, 80, 0, 0, 24, 30, 700, 1, 0, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (456, '火龙蜘蛛', 81, 19, 223, 55, 1, 0, 4000, 4500, 0, 38, 42, 80, 95, 0, 0, 24, 19, 600, 1, 0, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (457, '火龙蓝背蜘蛛', 121, 19, 238, 55, 1, 0, 3000, 3500, 0, 38, 42, 70, 105, 0, 0, 24, 30, 600, 1, 0, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (458, '火龙巨蛾', 121, 19, 235, 62, 1, 10, 18000, 18000, 0, 45, 52, 90, 140, 0, 0, 24, 20, 400, 1, 0, 900, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (459, '火龙护卫', 101, 47, 234, 62, 1, 10, 15000, 18000, 0, 42, 52, 90, 140, 0, 0, 24, 30, 500, 1, 0, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (460, '火龙黄龙', 81, 19, 233, 62, 1, 10, 15000, 18000, 0, 42, 52, 90, 140, 0, 0, 24, 30, 400, 1, 0, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (461, '火龙圣兽', 86, 63, 222, 62, 1, 10, 15000, 18000, 0, 42, 52, 90, 140, 0, 0, 20, 20, 400, 1, 0, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (462, '火龙守护兽', 129, 95, 802, 100, 1, 100, 0, 18000, 0, 30, 30, 40, 45, 0, 0, 17, 15, 900, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (463, '火龙神0', 107, 83, 90, 100, 0, 100, 32000, 32000, 0, 60, 60, 120, 160, 0, 0, 25, 30, 0, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (464, '灵魂收割者', 98, 96, 240, 70, 1, 100, 20000, 20000, 0, 22, 32, 100, 150, 0, 0, 17, 15, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (465, '蓝影刀客', 98, 97, 241, 70, 1, 100, 20000, 20000, 0, 22, 32, 100, 150, 0, 0, 17, 15, 400, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (466, '恶魔蝙蝠', 127, 19, 80, 100, 0, 100, 2000, 400, 0, 1000, 1000, 500, 500, 0, 0, 17, 15, 900, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (467, '火龙神', 128, 83, 800, 100, 1, 100, 30000, 32000, 0, 65, 65, 120, 180, 0, 0, 17, 15, 900, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (468, '雪域毛人', 81, 19, 265, 55, 0, 0, 1500, 1500, 0, 30, 60, 50, 100, 0, 0, 15, 40, 1000, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (469, '雪域野人', 81, 19, 258, 55, 0, 0, 1500, 1500, 0, 30, 60, 50, 100, 0, 0, 15, 40, 1000, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (470, '雪域羊人', 81, 19, 259, 55, 0, 0, 1500, 1500, 0, 30, 60, 50, 100, 0, 0, 15, 40, 1000, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (471, '雪域冰狼', 81, 19, 261, 55, 0, 0, 1500, 1300, 0, 30, 60, 50, 100, 0, 0, 15, 40, 1000, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (472, '雪域冰甲虫', 81, 19, 260, 55, 0, 0, 1200, 1200, 0, 30, 60, 50, 100, 0, 0, 15, 40, 1000, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (473, '雪域五毒魔', 130, 94, 252, 58, 0, 10, 1500, 1500, 0, 35, 60, 60, 120, 0, 0, 15, 40, 800, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (474, '雪域灭天魔', 126, 94, 253, 58, 0, 10, 1500, 1500, 0, 40, 80, 60, 120, 0, 0, 15, 40, 800, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (475, '雪域寒冰魔', 131, 94, 254, 58, 0, 10, 1500, 1500, 0, 35, 80, 60, 120, 0, 0, 15, 40, 800, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (476, '雪域战将', 138, 105, 250, 65, 0, 100, 1900, 6000, 0, 35, 60, 80, 180, 0, 0, 20, 40, 700, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (477, '雪域侍卫', 137, 102, 251, 65, 0, 100, 1500, 4800, 0, 30, 60, 80, 180, 0, 0, 20, 40, 700, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (478, '雪域力士', 81, 19, 262, 65, 0, 100, 1500, 4500, 0, 35, 60, 80, 180, 0, 0, 20, 40, 700, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (479, '雪域卫士', 141, 104, 263, 65, 0, 100, 1500, 4500, 0, 35, 60, 80, 180, 0, 0, 20, 40, 700, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (480, '雪域天将', 140, 106, 255, 55, 0, 100, 1500, 2500, 0, 50, 85, 50, 100, 0, 0, 20, 40, 700, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (481, '雪域天将1', 140, 106, 255, 60, 0, 100, 2000, 4000, 0, 30, 50, 60, 120, 0, 0, 20, 40, 600, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (482, '雪域天将2', 140, 106, 255, 65, 0, 100, 4000, 6000, 0, 30, 50, 80, 150, 0, 0, 25, 40, 500, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (483, '雪域天将3', 140, 106, 255, 70, 0, 100, 5000, 8000, 0, 30, 50, 90, 180, 0, 0, 25, 40, 400, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (484, '雪域天将4', 140, 106, 255, 80, 0, 100, 8000, 10000, 0, 30, 50, 110, 200, 0, 0, 30, 40, 300, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (485, '雪域魔王', 139, 107, 256, 100, 0, 100, 12000, 20000, 0, 100, 135, 120, 220, 0, 0, 30, 40, 300, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (486, '七点白蛇', 81, 19, 525, 50, 1, 10, 1100, 1200, 0, 8, 12, 20, 45, 0, 0, 15, 12, 700, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (487, '疯狂魔神盗', 152, 45, 518, 80, 1, 100, 9000, 25000, 0, 80, 140, 130, 230, 0, 0, 30, 40, 500, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (488, '巨象兽', 81, 19, 536, 55, 1, 0, 2500, 2600, 0, 55, 32, 120, 160, 0, 0, 15, 12, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (489, '东魔神怪', 81, 19, 517, 55, 1, 0, 1800, 1800, 0, 32, 55, 60, 150, 0, 0, 15, 12, 600, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (490, '猿猴战士', 81, 19, 537, 55, 1, 0, 1600, 1500, 0, 32, 45, 80, 120, 0, 0, 15, 12, 400, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (491, '蚂蚁将军', 81, 19, 520, 80, 1, 100, 12000, 30000, 0, 50, 100, 150, 230, 0, 0, 30, 12, 500, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (492, '盔甲蚂蚁', 81, 19, 521, 55, 1, 10, 750, 680, 0, 45, 20, 50, 120, 0, 0, 15, 12, 700, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (493, '蚂蚁战士', 81, 19, 522, 50, 1, 0, 580, 560, 0, 20, 20, 30, 110, 0, 0, 15, 12, 1200, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (494, '爆毒蚂蚁', 130, 94, 523, 55, 1, 10, 780, 720, 0, 20, 50, 40, 140, 0, 0, 15, 12, 900, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (495, '蚂蚁道士', 126, 94, 524, 55, 1, 10, 720, 620, 0, 20, 50, 40, 130, 0, 0, 15, 12, 900, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (496, '震天魔神', 140, 106, 529, 100, 0, 100, 30000, 50000, 0, 130, 180, 150, 250, 0, 0, 40, 12, 500, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (497, '震天首将', 92, 19, 514, 80, 0, 100, 10000, 20000, 0, 110, 120, 130, 200, 0, 0, 30, 12, 600, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (498, '地牢紫红女神', 137, 102, 77, 60, 0, 10, 2500, 2300, 0, 40, 60, 50, 150, 0, 0, 15, 12, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (499, '地牢绿荫女神', 137, 102, 79, 60, 0, 10, 2200, 1800, 0, 45, 55, 60, 140, 0, 0, 15, 12, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (500, '石像狮子', 131, 94, 76, 55, 0, 10, 1800, 1500, 0, 45, 55, 45, 130, 0, 0, 15, 12, 800, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (501, '火焰狮子', 131, 94, 75, 55, 0, 10, 1500, 1300, 0, 40, 60, 50, 120, 0, 0, 15, 12, 800, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (502, '火焰狮子9', 131, 94, 75, 55, 0, 10, 1500, 1300, 0, 40, 60, 50, 120, 0, 0, 15, 12, 800, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (503, '霸王教主', 140, 106, 499, 100, 0, 100, 50000, 80000, 0, 120, 200, 250, 330, 0, 0, 14, 40, 500, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (504, '霸王守卫', 139, 107, 509, 100, 0, 100, 18000, 30000, 0, 80, 100, 220, 280, 0, 0, 20, 20, 600, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (505, '爆毒神魔', 92, 19, 490, 70, 0, 100, 2800, 2600, 0, 25, 50, 60, 320, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (506, '触角神魔', 92, 19, 491, 70, 0, 100, 2600, 2500, 0, 50, 25, 60, 320, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (507, '海神将领', 105, 19, 492, 70, 0, 100, 2500, 2400, 0, 30, 60, 70, 220, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (508, '海神将领9', 105, 19, 492, 70, 0, 100, 2500, 2400, 0, 30, 60, 70, 220, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (509, '轻甲守卫', 81, 19, 493, 70, 0, 100, 2300, 2150, 0, 20, 40, 90, 180, 0, 0, 15, 20, 700, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (510, '红衣法师', 122, 19, 494, 70, 0, 100, 3000, 2800, 0, 40, 70, 90, 180, 0, 0, 15, 20, 900, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (511, '恶形鬼', 81, 19, 495, 70, 0, 100, 2600, 2400, 0, 30, 50, 80, 200, 0, 0, 15, 20, 900, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (512, '神舰守卫', 81, 62, 496, 70, 0, 100, 3500, 3000, 0, 60, 90, 160, 320, 0, 0, 30, 20, 700, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (513, '神舰守卫9', 81, 62, 496, 70, 0, 100, 3500, 3000, 0, 60, 90, 160, 320, 0, 0, 30, 20, 700, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (514, '犬猴魔', 81, 19, 497, 70, 0, 100, 2500, 2300, 0, 30, 50, 110, 170, 0, 0, 15, 20, 500, 1, 0, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (515, '霸王魔灵', 96, 42, 498, 70, 0, 100, 5500, 5000, 0, 30, 60, 110, 190, 0, 0, 15, 20, 400, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (516, '霸王魔灵9', 96, 42, 498, 70, 0, 100, 5500, 5000, 0, 30, 60, 110, 190, 0, 0, 15, 20, 400, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (517, '黑衣法师', 124, 92, 500, 70, 0, 100, 3500, 3200, 0, 40, 80, 100, 190, 0, 0, 15, 20, 900, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (518, '寒冰守护神', 140, 106, 109, 100, 1, 100, 80000, 100000, 0, 130, 220, 260, 350, 0, 0, 40, 50, 500, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (519, '冰宫勾魂', 137, 102, 89, 80, 1, 10, 20000, 32000, 0, 80, 120, 230, 300, 0, 0, 30, 20, 600, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (520, '冰宫野猪', 81, 19, 516, 60, 1, 10, 3200, 3300, 0, 60, 60, 100, 180, 0, 0, 15, 12, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (521, '冰宫战士', 81, 19, 515, 60, 1, 10, 3000, 3200, 0, 60, 90, 120, 190, 0, 0, 15, 12, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (522, '冰宫冰人', 95, 41, 54, 60, 1, 10, 3500, 3600, 0, 100, 100, 150, 220, 0, 0, 15, 12, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (523, '冰宫弓箭手', 104, 45, 530, 80, 1, 10, 25000, 30000, 0, 120, 120, 250, 300, 0, 0, 30, 20, 500, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (524, '冰宫骑士', 81, 19, 531, 80, 1, 10, 25000, 36000, 0, 120, 80, 180, 300, 0, 0, 30, 20, 300, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (525, '冰宫斧兵', 81, 19, 532, 60, 1, 10, 3300, 3500, 0, 50, 90, 120, 200, 0, 0, 15, 12, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (526, '冰宫绿魔', 130, 94, 533, 80, 1, 10, 25000, 28000, 0, 80, 120, 220, 300, 0, 0, 30, 20, 600, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (527, '冰宫战将', 81, 19, 534, 60, 1, 10, 3600, 3800, 0, 90, 60, 130, 220, 0, 0, 15, 12, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (528, '冰宫士兵', 81, 19, 535, 60, 1, 10, 2800, 3000, 0, 60, 90, 100, 190, 0, 0, 15, 12, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (529, '诺玛教主', 140, 106, 519, 100, 1, 100, 100000, 120000, 0, 150, 230, 280, 380, 0, 0, 30, 20, 500, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (530, '诺玛斧兵', 81, 19, 501, 80, 1, 100, 6000, 8500, 0, 140, 120, 170, 300, 0, 0, 20, 20, 600, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (531, '诺玛司令', 94, 40, 502, 80, 1, 100, 6200, 8600, 0, 100, 170, 200, 270, 0, 0, 20, 20, 600, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (532, '诺玛骑兵', 81, 71, 503, 80, 1, 100, 6100, 8500, 0, 120, 140, 150, 290, 0, 0, 20, 20, 400, 1, 0, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (533, '诺玛装甲兵', 81, 19, 504, 80, 1, 100, 6300, 8700, 0, 170, 200, 140, 240, 0, 0, 20, 20, 600, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (534, '诺玛抛石兵', 124, 92, 505, 80, 1, 100, 5800, 8400, 0, 100, 120, 170, 270, 0, 0, 20, 20, 700, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (535, '诺玛王', 92, 70, 513, 80, 1, 100, 10000, 30000, 0, 150, 120, 180, 300, 0, 0, 20, 20, 500, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (536, '沙漠石人', 95, 41, 55, 70, 1, 10, 2800, 2500, 0, 120, 70, 150, 250, 0, 0, 20, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (537, '诺玛法老', 142, 109, 506, 70, 0, 10, 2500, 2300, 0, 70, 120, 140, 220, 0, 0, 20, 20, 1000, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (538, '诺玛将士', 81, 19, 507, 65, 1, 0, 2600, 2100, 0, 60, 100, 120, 180, 0, 0, 20, 20, 1200, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (539, '诺玛战士', 81, 19, 508, 65, 1, 0, 2300, 1900, 0, 55, 90, 115, 160, 0, 0, 20, 20, 1200, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (540, '诺玛小兵', 81, 19, 510, 65, 1, 0, 2200, 1700, 0, 45, 95, 100, 160, 0, 0, 20, 20, 1200, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (541, '诺玛士卒', 81, 19, 511, 65, 1, 0, 2200, 1800, 0, 60, 80, 110, 150, 0, 0, 20, 20, 1200, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (542, '诺玛', 81, 19, 512, 65, 1, 0, 1800, 1500, 0, 50, 60, 90, 140, 0, 0, 20, 20, 1200, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (543, '赤狐', 142, 109, 321, 80, 0, 0, 2000, 3000, 0, 80, 120, 80, 154, 0, 0, 15, 40, 1000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (544, '金爪赤狐王', 143, 109, 321, 599, 0, 0, 50000, 100000, 0, 150, 198, 174, 267, 0, 0, 25, 40, 600, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (545, '素狐', 144, 110, 322, 80, 0, 0, 3000, 3500, 0, 80, 90, 74, 140, 0, 0, 15, 40, 1000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (546, '玉面素狐王', 145, 110, 322, 599, 0, 0, 50000, 120000, 0, 142, 178, 153, 261, 0, 0, 25, 40, 600, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (547, '黑狐', 146, 108, 320, 80, 0, 0, 4000, 5000, 0, 120, 80, 90, 150, 0, 0, 15, 40, 900, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (548, '火尾黑狐王', 147, 108, 320, 599, 0, 0, 50000, 150000, 0, 192, 162, 170, 253, 0, 0, 25, 40, 600, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (549, '九尾魂石', 151, 112, 324, 999, 1, 1, 3000, 2500, 0, 245, 180, 267, 467, 0, 0, 25, 40, 0, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (550, '九尾魂石1', 151, 112, 324, 999, 1, 1, 40000, 150000, 0, 245, 180, 467, 670, 0, 0, 25, 40, 0, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (551, '弓箭大师', 152, 45, 323, 80, 0, 0, 8000, 8000, 0, 30, 50, 60, 80, 0, 0, 40, 40, 400, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (552, '恶魔弓手', 152, 45, 293, 80, 0, 0, 2000, 2000, 0, 80, 80, 90, 150, 0, 0, 40, 40, 600, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (553, '狐月之眼', 148, 111, 325, 80, 0, 0, 2500, 2500, 0, 92, 90, 499, 499, 0, 0, 15, 40, 1000, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (554, '狐月魔眼', 149, 111, 326, 80, 0, 0, 3000, 3000, 0, 100, 94, 499, 499, 0, 0, 15, 40, 1000, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (555, '狐月角虫', 152, 22, 328, 80, 0, 0, 5000, 8000, 0, 92, 90, 80, 157, 0, 0, 15, 40, 1000, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (556, '狐月虎虫', 152, 22, 329, 80, 0, 0, 5000, 8000, 0, 92, 90, 80, 157, 0, 0, 15, 40, 1000, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (557, '狐月天珠', 153, 113, 327, 999, 1, 1, 100000, 200000, 0, 220, 380, 660, 880, 0, 0, 25, 40, 0, 1, 0, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (558, '★★任务怪★★', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (559, '暗之祖玛教主', 102, 49, 63, 60, 1, 100, 10000, 10000, 0, 30, 35, 120, 150, 0, 0, 20, 20, 300, 1, 500, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (560, '骷髅精灵4', 89, 14, 150, 40, 1, 1, 600, 500, 0, 5, 4, 7, 24, 0, 0, 15, 12, 1200, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (561, '骷髅精灵10', 89, 14, 150, 50, 1, 1, 600, 1200, 0, 5, 4, 7, 35, 0, 0, 17, 20, 600, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (562, '火蛇王', 81, 19, 233, 100, 1, 100, 1000, 1800, 0, 10, 20, 20, 50, 0, 0, 24, 30, 700, 1, 0, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (563, '暗之火蛇王', 105, 52, 233, 100, 1, 100, 1500, 3000, 0, 15, 25, 25, 65, 0, 0, 24, 30, 700, 1, 0, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (564, '武夫子', 150, 19, 0, 30, 0, 100, 1, 2000, 5000, 0, 0, 5, 7, 0, 0, 35, 50, 700, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (565, '悦来客栈老板娘', 150, 19, 0, 40, 0, 100, 1, 3000, 10000, 10, 10, 6, 8, 6, 0, 35, 50, 700, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (566, '牢房守卫', 81, 45, 72, 80, 0, 0, 10, 2000, 0, 0, 5, 25, 45, 0, 0, 20, 15, 800, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (567, '牢房守卫0', 104, 45, 71, 80, 0, 0, 10, 1000, 0, 100, 100, 100, 200, 0, 0, 20, 15, 800, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (568, '酒虫', 90, 16, 24, 21, 0, 100, 60, 65, 0, 0, 0, 6, 8, 0, 0, 12, 10, 1000, 1, 0, 2500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (569, '地蛙', 95, 19, 184, 1, 0, 1, 1, 1, 0, 0, 0, 3, 3, 0, 0, 3, 4, 500, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (570, '地龙', 107, 33, 140, 70, 0, 1, 500, 3000, 0, 0, 20, 20, 25, 40, 0, 30, 30, 2000, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (571, '密道巨蛾', 81, 19, 210, 70, 0, 1, 1, 2000, 0, 0, 20, 20, 30, 45, 0, 17, 13, 700, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (572, '家丁', 81, 45, 72, 70, 0, 0, 1, 2400, 15, 5, 15, 45, 65, 0, 0, 20, 15, 800, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (573, '圣殿守卫', 81, 45, 72, 80, 0, 0, 10, 2000, 0, 0, 5, 25, 45, 0, 0, 20, 15, 800, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (574, '天关守卫0', 81, 45, 72, 80, 0, 0, 10, 5000, 0, 10, 25, 45, 75, 0, 0, 20, 15, 600, 1, 0, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (575, '天关守卫', 81, 45, 72, 80, 0, 0, 10, 2000, 0, 0, 5, 25, 45, 0, 0, 20, 15, 800, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (576, '天关教主', 81, 45, 72, 100, 0, 100, 5000, 12000, 0, 15, 25, 35, 65, 0, 0, 20, 15, 300, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (577, '庄园管家', 150, 19, 0, 50, 0, 10, 2000, 2000, 20000, 5, 10, 5, 10, 0, 15, 17, 50, 800, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (578, '庄园管家0', 150, 19, 0, 50, 0, 10, 1000, 1000, 20000, 5, 10, 5, 10, 0, 15, 17, 50, 800, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (579, '庄园管家1', 150, 19, 0, 50, 0, 10, 1500, 1500, 20000, 5, 10, 5, 10, 0, 15, 17, 50, 800, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (580, '庄园庄主', 150, 19, 0, 100, 0, 200, 12000, 12000, 20000, 5, 10, 5, 10, 0, 88, 35, 50, 700, 1, 0, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (581, '幻境英雄', 150, 19, 0, 60, 0, 100, 1, 8000, 10000, 10, 10, 6, 7, 6, 0, 35, 50, 500, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (582, '幻境英雄0', 150, 19, 0, 60, 0, 100, 1, 4000, 10000, 10, 10, 6, 7, 6, 0, 35, 50, 500, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (583, '幻境英雄1', 150, 19, 0, 60, 0, 100, 1, 6000, 10000, 10, 10, 6, 7, 6, 0, 35, 50, 500, 1, 0, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (584, '遗失之城守卫', 150, 19, 0, 60, 0, 100, 1, 8000, 10000, 10, 10, 6, 7, 6, 0, 35, 50, 500, 1, 0, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (585, '遗失的法师', 150, 19, 0, 60, 0, 100, 1, 362, 20000, 0, 0, 0, 0, 6, 0, 35, 50, 600, 1, 0, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (586, '遗失的法师0', 150, 19, 0, 60, 0, 100, 1, 362, 20000, 0, 0, 0, 0, 6, 0, 35, 50, 600, 1, 0, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (587, '遗失的道士', 150, 19, 0, 60, 0, 100, 1, 764, 20000, 0, 0, 0, 0, 0, 0, 35, 50, 600, 1, 0, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (588, '遗失的道士0', 150, 19, 0, 60, 0, 100, 1, 764, 20000, 0, 0, 0, 0, 0, 0, 35, 50, 600, 1, 0, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (589, '遗失的战士', 150, 19, 0, 60, 0, 100, 1, 1364, 20000, 0, 0, 0, 0, 0, 0, 35, 50, 600, 1, 0, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (590, '遗失的战士0', 150, 19, 0, 60, 0, 100, 1, 1364, 20000, 0, 0, 0, 0, 0, 0, 35, 50, 600, 1, 0, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (591, '天尊', 150, 19, 0, 70, 0, 100, 1, 3000, 20000, 0, 0, 5, 7, 6, 0, 35, 50, 600, 1, 0, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (592, '天虹法师', 150, 19, 0, 70, 0, 100, 1, 2000, 20000, 0, 0, 5, 7, 6, 0, 35, 50, 600, 1, 0, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (593, '教头', 150, 19, 0, 70, 0, 100, 1, 10000, 20000, 0, 0, 5, 7, 6, 0, 35, 50, 600, 1, 0, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (594, '皇城守卫', 150, 19, 0, 200, 0, 100, 1, 32000, 32000, 9999, 9999, 9999, 9999, 0, 0, 35, 50, 100, 1, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (595, '守卫(沙巴克)', 150, 19, 0, 70, 0, 100, 1, 10000, 20000, 10, 10, 5, 7, 6, 0, 35, 50, 600, 1, 0, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (596, '战士(沙巴克)', 150, 19, 0, 100, 0, 100, 1, 5464, 20000, 10, 10, 5, 7, 6, 0, 35, 50, 600, 1, 0, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (597, '战士(沙巴克)0', 150, 19, 0, 100, 0, 100, 1, 5464, 20000, 10, 10, 5, 7, 6, 0, 35, 50, 600, 1, 0, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (598, '法师(沙巴克)', 150, 19, 0, 100, 0, 100, 1, 1861, 20000, 10, 10, 5, 7, 6, 0, 35, 50, 600, 1, 0, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (599, '法师(沙巴克)0', 150, 19, 0, 100, 0, 100, 1, 1861, 20000, 10, 10, 5, 7, 6, 0, 35, 50, 600, 1, 0, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (600, '道士(沙巴克)', 150, 19, 0, 100, 0, 100, 1, 2931, 20000, 10, 10, 5, 7, 6, 0, 35, 50, 600, 1, 0, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (601, '道士(沙巴克)0', 150, 19, 0, 100, 0, 100, 1, 2931, 20000, 10, 10, 5, 7, 6, 0, 35, 50, 600, 1, 0, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (602, '城主(沙巴克)', 150, 19, 0, 200, 0, 100, 1, 12000, 20000, 10, 10, 5, 7, 6, 0, 35, 50, 600, 1, 0, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (603, '尸王4', 81, 19, 152, 45, 1, 1, 800, 500, 0, 3, 3, 18, 36, 0, 0, 15, 15, 1200, 1, 0, 2800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (604, '尸王10', 81, 19, 152, 50, 1, 1, 800, 1800, 0, 5, 5, 20, 40, 0, 0, 15, 15, 600, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (605, '沃玛森林铁匠', 150, 19, 0, 60, 0, 100, 500, 8000, 20000, 3, 2, 2, 5, 0, 0, 15, 20, 600, 1, 600, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (606, '道士(火焰沃玛)', 150, 19, 0, 60, 0, 100, 500, 2000, 20000, 3, 2, 2, 5, 0, 0, 15, 20, 600, 1, 600, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (607, '超级黑野猪', 81, 19, 111, 80, 1, 1, 1000, 1000, 0, 15, 0, 30, 45, 0, 0, 25, 17, 800, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (608, '超级红野猪', 81, 19, 110, 80, 1, 1, 1000, 1000, 0, 0, 15, 30, 45, 0, 0, 25, 17, 800, 1, 0, 1800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (609, '超级白野猪', 81, 19, 112, 100, 1, 100, 3000, 5000, 0, 15, 15, 35, 65, 0, 0, 25, 17, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (610, '祭祀长', 150, 19, 0, 60, 0, 100, 500, 2000, 20000, 3, 2, 2, 5, 0, 0, 15, 20, 600, 1, 600, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (611, '合成师', 150, 19, 0, 60, 0, 100, 500, 5000, 2000, 3, 2, 2, 5, 0, 0, 15, 20, 600, 1, 600, 1000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (612, '密室守卫', 81, 45, 72, 80, 0, 0, 500, 3000, 0, 10, 25, 45, 75, 0, 0, 20, 15, 500, 1, 0, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (613, '封魔谷谷主', 150, 19, 0, 100, 0, 100, 500, 5000, 0, 0, 0, 0, 0, 0, 0, 15, 20, 500, 1, 600, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (614, '封魔谷守卫', 150, 19, 0, 60, 0, 100, 500, 3000, 0, 0, 0, 0, 0, 0, 0, 15, 20, 500, 1, 600, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (615, '封魔谷守卫0', 150, 19, 0, 60, 0, 100, 500, 2000, 0, 0, 0, 0, 0, 0, 0, 15, 20, 500, 1, 600, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (616, '封魔谷守卫1', 150, 19, 0, 60, 0, 100, 500, 1500, 0, 0, 0, 0, 0, 0, 0, 15, 20, 500, 1, 600, 800, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (617, '冰眼巨魔', 99, 107, 267, 80, 0, 0, 8000, 8000, 0, 30, 50, 60, 80, 0, 0, 15, 40, 1000, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (618, '冰眼巨魔1', 99, 107, 268, 80, 0, 0, 8000, 15000, 0, 30, 50, 80, 120, 0, 0, 15, 40, 1000, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (619, '冰眼巨魔2', 99, 107, 256, 80, 0, 0, 8000, 30000, 0, 30, 50, 120, 180, 0, 0, 15, 40, 1000, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (620, '冰眼巨魔3', 134, 107, 256, 80, 0, 0, 1, 30000, 0, 40, 100, 150, 250, 0, 0, 25, 40, 1200, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (621, '比奇守卫', 150, 19, 0, 40, 0, 100, 500, 50000, 30000, 3000, 3000, 0, 0, 0, 0, 100, 100, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (622, '土城守卫', 150, 19, 0, 40, 0, 100, 500, 50000, 30000, 3000, 3000, 0, 0, 0, 0, 15, 20, 1000, 1, 0, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (623, '白日门道士0', 150, 19, 0, 50, 0, 100, 500, 3000, 30000, 0, 0, 0, 0, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (624, '神秘人', 150, 19, 0, 40, 0, 100, 100, 2000, 30000, 0, 0, 0, 0, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (625, '神秘老人', 150, 19, 0, 40, 0, 100, 100, 1000, 30000, 0, 0, 0, 0, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (626, '季正', 150, 19, 0, 100, 0, 100, 500, 5000, 30000, 0, 0, 0, 0, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (627, '募兵官', 150, 19, 0, 100, 0, 100, 500, 12000, 30000, 0, 0, 0, 0, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (628, '无影', 150, 19, 0, 100, 0, 100, 500, 20000, 30000, 200, 200, 0, 0, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (629, '法师英雄', 150, 19, 0, 100, 0, 100, 1, 2000, 10000, 0, 0, 0, 0, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (630, '沙头盔店老板0', 150, 19, 0, 1, 0, 0, 1, 19, 0, 0, 0, 0, 0, 0, 0, 15, 20, 800, 1, 0, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (631, '沙头盔店老板', 150, 19, 0, 100, 0, 100, 1, 12000, 10000, 0, 0, 0, 0, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (632, '战士英雄', 150, 19, 0, 100, 0, 100, 1, 5000, 30000, 0, 0, 0, 0, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (633, '战士英雄0', 150, 19, 0, 100, 0, 100, 1, 5000, 30000, 0, 0, 0, 0, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (634, '天之神匠', 150, 19, 0, 100, 0, 100, 1, 12000, 30000, 0, 0, 0, 0, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (635, '酒神弟子', 150, 19, 0, 100, 0, 100, 1, 50000, 30000, 2000, 2000, 60, 180, 0, 0, 15, 20, 600, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (636, '酒神弟子0', 150, 19, 0, 50, 0, 100, 1, 10000, 30000, 0, 0, 0, 0, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (637, '封魔谷炼药师', 150, 19, 0, 1, 0, 0, 1, 100, 0, 0, 0, 0, 0, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (638, '庄园仓库', 150, 19, 0, 100, 0, 100, 1, 10000, 30000, 0, 0, 0, 0, 0, 0, 15, 20, 800, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (639, '天庭守卫', 150, 19, 0, 100, 0, 100, 1, 5000, 30000, 0, 0, 0, 0, 0, 0, 15, 20, 800, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (640, '天关教主0', 150, 19, 0, 100, 0, 100, 1, 20000, 30000, 0, 0, 0, 0, 0, 0, 15, 20, 800, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (641, '父亲', 150, 19, 0, 100, 0, 100, 1, 3000, 0, 0, 0, 0, 0, 0, 0, 15, 20, 900, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (642, '庄园管家2', 150, 19, 0, 100, 0, 100, 1, 8000, 30000, 0, 0, 0, 0, 0, 0, 15, 20, 800, 1, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (643, '沙铁匠铺老板', 150, 19, 0, 50, 0, 100, 1, 2000, 0, 0, 0, 0, 0, 0, 0, 15, 20, 900, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (644, '测试57', 81, 19, 57, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (645, '测试58', 81, 19, 58, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (646, '测试59', 81, 19, 59, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (647, '测试64', 81, 19, 64, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (648, '测试65', 81, 19, 65, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (649, '测试66', 81, 19, 66, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (650, '测试67', 81, 19, 67, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (651, '测试68', 81, 19, 68, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (652, '测试69', 81, 19, 69, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (653, '测试85', 81, 19, 85, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (654, '测试86', 81, 19, 86, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (655, '测试87', 81, 19, 87, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (656, '测试88', 81, 19, 88, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (657, '测试94', 81, 19, 94, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (658, '测试95', 81, 19, 95, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (659, '测试96', 81, 19, 96, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (660, '测试97', 81, 19, 97, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (661, '测试98', 81, 19, 98, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (662, '测试99', 81, 19, 99, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (663, '测试105', 81, 19, 105, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (664, '测试106', 81, 19, 106, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (665, '测试107', 81, 19, 107, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (666, '测试108', 81, 19, 108, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (667, '测试124', 81, 19, 124, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (668, '测试125', 81, 19, 125, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (669, '测试126', 81, 19, 126, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (670, '测试127', 81, 19, 127, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (671, '测试128', 81, 19, 128, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (672, '测试129', 81, 19, 129, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (673, '测试134', 81, 19, 134, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (674, '测试135', 81, 19, 135, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (675, '测试136', 81, 19, 136, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (676, '测试137', 81, 19, 137, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (677, '测试138', 81, 19, 138, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (678, '测试139', 81, 19, 139, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (679, '测试155', 81, 19, 155, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (680, '测试156', 81, 19, 156, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (681, '测试157', 81, 19, 157, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (682, '测试158', 115, 34, 158, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 0, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (683, '测试159', 81, 19, 159, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (684, '测试526', 81, 19, 526, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (685, '测试527', 81, 19, 527, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (686, '测试528', 81, 19, 528, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (687, '测试538', 81, 19, 538, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (688, '测试539', 81, 19, 539, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (689, '黑魔雕王', 139, 107, 299, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (690, '测试298', 81, 19, 298, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (691, '测试297', 81, 19, 297, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (692, '测试296', 81, 19, 296, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (693, '测试295', 81, 19, 295, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (694, '测试294', 81, 19, 294, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (695, '测试293', 81, 19, 293, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (696, '测试300', 81, 19, 300, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (697, '测试301', 81, 19, 301, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (698, '测试302', 81, 19, 302, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (699, '测试303', 81, 19, 303, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (700, '测试304', 81, 19, 304, 5, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (701, '测试305', 81, 19, 305, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (702, '测试306', 81, 19, 306, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (703, '测试307', 81, 19, 307, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (704, '测试308', 81, 19, 308, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `tbl_monsters` VALUES (705, '测试309', 81, 19, 309, 50, 1, 0, 1000, 1000, 0, 0, 0, 20, 50, 0, 0, 15, 20, 800, 1, 0, 1500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

-- ----------------------------
-- Table structure for tbl_quest
-- ----------------------------
DROP TABLE IF EXISTS `tbl_quest`;
CREATE TABLE `tbl_quest`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_PLAYERID` int(11) NULL DEFAULT NULL,
  `FLD_QUESTOPENINDEX` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_QUESTFININDEX` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `FLD_QUEST` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '任务标志' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of tbl_quest
-- ----------------------------

-- ----------------------------
-- Table structure for tbl_stditems
-- ----------------------------
DROP TABLE IF EXISTS `tbl_stditems`;
CREATE TABLE `tbl_stditems`  (
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
) ENGINE = InnoDB AUTO_INCREMENT = 1001 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of tbl_stditems
-- ----------------------------
INSERT INTO `tbl_stditems` VALUES (1, '★药品杂货★', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (2, '命运之书', 41, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (3, '金创药(小量)', 0, 0, 1, 102, 0, 0, 398, 1, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (4, '魔法药(小量)', 0, 0, 1, 103, 0, 0, 394, 1, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (5, '金创药(中量)', 0, 0, 2, 104, 0, 0, 400, 1, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 200, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (6, '魔法药(中量)', 0, 0, 2, 105, 0, 0, 396, 1, 0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 200, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (7, '强效金创药', 0, 0, 3, 100, 0, 0, 28, 1, 90, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 540, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (8, '强效魔法药', 0, 0, 3, 101, 0, 0, 29, 1, 0, 0, 150, 0, 0, 0, 0, 0, 0, 0, 0, 0, 540, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (9, '金创药(大量)', 0, 0, 5, 162, 0, 0, 813, 1, 180, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1080, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (10, '魔法药(大量)', 0, 0, 5, 163, 0, 0, 814, 1, 0, 0, 300, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1080, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (11, '金创药(特量)', 0, 0, 4, 156, 0, 0, 5022, 1, 350, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2200, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (12, '魔法药(特量)', 0, 0, 4, 157, 0, 0, 5023, 1, 0, 0, 600, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2200, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (13, '金创药(小)包', 31, 102, 8, 1, 0, 0, 399, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 680, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (14, '魔法药(小)包', 31, 103, 8, 2, 0, 0, 395, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 680, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (15, '金创药(中)包', 31, 104, 14, 1, 0, 0, 401, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (16, '魔法药(中)包', 31, 105, 14, 2, 0, 0, 397, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (17, '超级金创药', 31, 100, 20, 1, 0, 0, 313, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (18, '超级魔法药', 31, 101, 20, 2, 0, 0, 314, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (19, '金创药(大)包', 31, 162, 25, 1, 0, 0, 815, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (20, '魔法药(大)包', 31, 163, 25, 2, 0, 0, 816, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (21, '金创药(特)包', 31, 156, 32, 1, 0, 0, 5024, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (22, '魔法药(特)包', 31, 157, 32, 2, 0, 0, 5025, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (23, '太阳水', 0, 1, 1, 0, 0, 0, 16, 0, 30, 0, 40, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (24, '强效太阳水', 0, 1, 2, 117, 0, 0, 312, 0, 50, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (25, '万年雪霜', 0, 1, 2, 118, 0, 0, 260, 1, 100, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (26, '疗伤药', 0, 1, 1, 119, 0, 0, 14, 1, 200, 0, 350, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (27, '强效太阳水包', 31, 117, 6, 3, 0, 0, 1007, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 13000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (28, '万年雪霜包', 31, 118, 6, 3, 0, 0, 1008, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (29, '疗伤药包', 31, 119, 6, 3, 0, 0, 1009, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (30, '灰色药粉(少量)', 25, 1, 1, 0, 0, 0, 251, 10000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3000, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (31, '黄色药粉(少量)', 25, 2, 1, 0, 0, 0, 250, 10000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3000, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (32, '灰色药粉(中量)', 25, 1, 2, 0, 0, 0, 251, 20000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 5000, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (33, '灰色药粉(大量)', 25, 1, 3, 0, 0, 0, 251, 50000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 8000, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (34, '黄色药粉(中量)', 25, 2, 2, 0, 0, 0, 250, 20000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 5000, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (35, '黄色药粉(大量)', 25, 2, 3, 0, 0, 0, 250, 50000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 8000, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (36, '随机传送卷', 3, 2, 1, 107, 0, 0, 404, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 100, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (37, '地牢逃脱卷', 3, 1, 1, 106, 0, 0, 408, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 100, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (38, '回城卷', 3, 3, 1, 108, 0, 0, 402, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (39, '行会回城卷', 3, 5, 1, 109, 0, 0, 406, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 800, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (40, '地牢逃脱卷包', 31, 106, 8, 0, 0, 0, 409, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 800, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (41, '随机传送卷包', 31, 107, 8, 0, 0, 0, 405, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 800, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (42, '回城卷包', 31, 108, 8, 0, 0, 0, 403, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (43, '行会回城卷包', 31, 109, 8, 0, 0, 0, 407, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (44, '筹码包', 31, 164, 14, 0, 0, 0, 390, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6200, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (45, '护身符', 25, 5, 1, 158, 0, 0, 270, 30000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (46, '护身符(大)', 25, 5, 2, 0, 0, 0, 270, 60000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (47, '打捆护身符', 31, 158, 6, 3, 0, 0, 466, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (48, '蜡烛', 30, 0, 1, 0, 0, 0, 130, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 10000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (49, '火把', 30, 0, 3, 0, 0, 0, 131, 20000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (50, '肉', 40, 0, 3, 0, 0, 0, 1, 10000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2000, 10000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (51, '鸡肉', 40, 0, 1, 0, 0, 0, 13, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 80, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (52, '干肉', 1, 0, 1, 0, 0, 0, 5, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 10000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (53, '食人树叶', 42, 0, 1, 0, 0, 0, 255, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (54, '毒蜘蛛牙齿', 42, 0, 1, 0, 0, 0, 253, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (55, '食人树果实', 42, 0, 1, 0, 0, 0, 256, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (56, '蝎子尾巴', 42, 0, 1, 0, 0, 0, 254, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (57, '蛆卵', 42, 0, 1, 0, 0, 0, 252, 60, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (58, '沃玛号角', 44, 0, 1, 0, 0, 0, 261, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (59, '祖玛头像', 44, 1, 1, 0, 0, 0, 271, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (60, '金条', 31, 0, 10, 45, 0, 0, 117, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (61, '金砖', 31, 0, 52, 46, 0, 0, 121, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (62, '金盒', 31, 0, 110, 47, 0, 0, 122, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (63, '金箱子', 31, 0, 500, 58, 0, 0, 1105, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (64, '铜矿', 43, 0, 4, 0, 0, 0, 286, 10000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (65, '铁矿', 43, 0, 4, 0, 0, 0, 281, 10000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (66, '银矿', 43, 0, 4, 0, 0, 0, 285, 10000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (67, '金矿', 43, 0, 4, 0, 0, 0, 280, 20000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (68, '黑铁矿石', 43, 0, 4, 0, 0, 0, 284, 30000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (69, '战神油', 3, 10, 1, 0, 0, 0, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (70, '祝福油', 3, 4, 1, 0, 0, 0, 26, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (71, '修复油', 3, 9, 1, 0, 0, 0, 27, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 800, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (72, '攻击神水', 3, 12, 3, 0, 0, 0, 425, 0, 0, 0, 0, 180, 5, 0, 0, 0, 0, 0, 0, 0, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (73, '魔力神水', 3, 12, 3, 0, 0, 0, 423, 0, 0, 0, 0, 180, 0, 0, 5, 0, 0, 0, 0, 0, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (74, '精神神水', 3, 12, 3, 0, 0, 0, 421, 0, 0, 0, 0, 180, 0, 0, 0, 0, 5, 0, 0, 0, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (75, '疾速神水', 3, 12, 3, 0, 0, 0, 420, 0, 0, 1, 0, 180, 0, 0, 0, 0, 0, 0, 0, 0, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (76, '体力神水', 3, 12, 3, 0, 0, 0, 424, 0, 50, 0, 0, 120, 0, 0, 0, 0, 0, 0, 0, 0, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (77, '魔法力神水', 3, 12, 3, 0, 0, 0, 422, 0, 0, 0, 50, 120, 0, 0, 0, 0, 0, 0, 0, 0, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (78, '攻击神水(中)', 3, 12, 3, 0, 0, 0, 425, 0, 0, 0, 0, 360, 8, 0, 0, 0, 0, 0, 0, 0, 4000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (79, '魔力神水(中)', 3, 12, 3, 0, 0, 0, 423, 0, 0, 0, 0, 360, 0, 0, 8, 0, 0, 0, 0, 0, 4000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (80, '精神神水(中)', 3, 12, 3, 0, 0, 0, 421, 0, 0, 0, 0, 360, 0, 0, 0, 0, 8, 0, 0, 0, 4000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (81, '疾速神水(中)', 3, 12, 3, 0, 0, 0, 420, 0, 0, 2, 0, 360, 0, 0, 0, 0, 0, 0, 0, 0, 4000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (82, '体力神水(中)', 3, 12, 3, 0, 0, 0, 424, 0, 100, 0, 0, 240, 0, 0, 0, 0, 0, 0, 0, 0, 4000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (83, '魔法力神水(中)', 3, 12, 3, 0, 0, 0, 422, 0, 0, 0, 100, 240, 0, 0, 0, 0, 0, 0, 0, 0, 4000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (84, '强效攻击神水', 3, 12, 3, 0, 0, 0, 425, 0, 0, 0, 0, 720, 10, 0, 0, 0, 0, 0, 0, 0, 6000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (85, '强效魔力神水', 3, 12, 3, 0, 0, 0, 423, 0, 0, 0, 0, 720, 0, 0, 10, 0, 0, 0, 0, 0, 6000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (86, '强效精神神水', 3, 12, 3, 0, 0, 0, 421, 0, 0, 0, 0, 720, 0, 0, 0, 0, 10, 0, 0, 0, 6000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (87, '强效疾速神水', 3, 12, 3, 0, 0, 0, 420, 0, 0, 3, 0, 720, 0, 0, 0, 0, 0, 0, 0, 0, 6000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (88, '强效体力神水', 3, 12, 3, 0, 0, 0, 424, 0, 200, 0, 0, 480, 0, 0, 0, 0, 0, 0, 0, 0, 6000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (89, '强效魔法力神水', 3, 12, 3, 0, 0, 0, 422, 0, 0, 0, 200, 480, 0, 0, 0, 0, 0, 0, 0, 0, 6000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (90, '超级攻击神水', 3, 12, 3, 0, 0, 0, 425, 0, 0, 0, 0, 1200, 20, 0, 0, 0, 0, 0, 0, 0, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (91, '超级魔力神水', 3, 12, 3, 0, 0, 0, 423, 0, 0, 0, 0, 1200, 0, 0, 20, 0, 0, 0, 0, 0, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (92, '超级精神神水', 3, 12, 3, 0, 0, 0, 421, 0, 0, 0, 0, 1200, 0, 0, 0, 0, 20, 0, 0, 0, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (93, '超级体力神水', 3, 12, 3, 0, 0, 0, 424, 0, 500, 0, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (94, '超级魔法力神水', 3, 12, 3, 0, 0, 0, 422, 0, 0, 0, 500, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (95, '超级疾速神水', 3, 12, 3, 0, 0, 0, 420, 0, 0, 4, 0, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (96, '★衣服头盔★', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (97, '布衣(男)', 10, 1, 5, 0, 0, 0, 60, 5000, 0, 2, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 400, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (98, '布衣(女)', 11, 1, 5, 0, 0, 0, 80, 5000, 0, 2, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 400, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (99, '轻型盔甲(男)', 10, 2, 8, 0, 0, 0, 61, 8000, 3, 3, 1, 2, 0, 0, 0, 0, 0, 0, 0, 11, 3000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (100, '轻型盔甲(女)', 11, 2, 8, 0, 0, 0, 81, 8000, 3, 3, 1, 2, 0, 0, 0, 0, 0, 0, 0, 11, 3000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (101, '中型盔甲(男)', 10, 2, 18, 0, 0, 0, 61, 12000, 3, 5, 1, 2, 0, 0, 0, 0, 0, 0, 0, 16, 5500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (102, '中型盔甲(女)', 11, 2, 18, 0, 0, 0, 81, 12000, 3, 5, 1, 2, 0, 0, 0, 0, 0, 0, 0, 16, 5500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (103, '重盔甲(男)', 10, 3, 23, 0, 0, 0, 62, 25000, 4, 7, 2, 3, 0, 0, 0, 0, 0, 0, 0, 22, 12000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (104, '重盔甲(女)', 11, 3, 23, 0, 0, 0, 82, 25000, 4, 7, 2, 3, 0, 0, 0, 0, 0, 0, 0, 22, 12000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (105, '魔法长袍(男)', 10, 4, 12, 0, 0, 0, 63, 20000, 3, 5, 3, 4, 0, 0, 0, 2, 0, 0, 0, 22, 12000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (106, '魔法长袍(女)', 11, 4, 12, 0, 0, 0, 83, 20000, 3, 5, 3, 4, 0, 0, 0, 2, 0, 0, 0, 22, 12000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (107, '灵魂战衣(男)', 10, 5, 15, 0, 0, 0, 64, 20000, 3, 6, 3, 3, 0, 0, 0, 0, 0, 2, 0, 22, 12000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (108, '灵魂战衣(女)', 11, 5, 15, 0, 0, 0, 84, 20000, 3, 6, 3, 3, 0, 0, 0, 0, 0, 2, 0, 22, 12000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (109, '战神盔甲(男)', 10, 3, 45, 0, 0, 0, 62, 30000, 5, 9, 3, 5, 0, 0, 0, 0, 0, 0, 1, 46, 35000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (110, '战神盔甲(女)', 11, 3, 45, 0, 0, 0, 82, 30000, 5, 9, 3, 5, 0, 0, 0, 0, 0, 0, 1, 46, 35000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (111, '幽灵战衣(男)', 10, 5, 26, 0, 0, 0, 64, 28000, 4, 7, 3, 3, 0, 0, 0, 0, 1, 4, 3, 27, 35000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (112, '幽灵战衣(女)', 11, 5, 26, 0, 0, 0, 84, 28000, 4, 7, 3, 3, 0, 0, 0, 0, 1, 4, 3, 27, 35000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (113, '恶魔长袍(男)', 10, 4, 19, 0, 0, 0, 63, 28000, 4, 7, 3, 4, 0, 0, 1, 4, 0, 0, 2, 28, 35000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (114, '恶魔长袍(女)', 11, 4, 19, 0, 0, 0, 83, 28000, 4, 7, 3, 4, 0, 0, 1, 4, 0, 0, 2, 28, 35000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (115, '天魔神甲', 10, 6, 62, 0, 0, 0, 85, 35000, 5, 12, 4, 7, 1, 2, 0, 0, 0, 0, 0, 40, 35000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (116, '圣战宝甲', 11, 6, 62, 0, 0, 0, 88, 35000, 5, 12, 4, 7, 1, 2, 0, 0, 0, 0, 0, 40, 35000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (117, '天尊道袍', 10, 8, 37, 0, 0, 0, 87, 30000, 4, 9, 4, 6, 0, 0, 0, 0, 2, 5, 0, 40, 35000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (118, '天师长袍', 11, 8, 37, 0, 0, 0, 90, 30000, 4, 9, 4, 6, 0, 0, 0, 0, 2, 5, 0, 40, 35000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (119, '法神披风', 10, 7, 21, 0, 0, 0, 86, 26000, 4, 9, 4, 6, 0, 0, 2, 5, 0, 0, 0, 40, 35000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (120, '霓裳羽衣', 11, 7, 21, 0, 0, 0, 89, 26000, 4, 9, 4, 6, 0, 0, 2, 5, 0, 0, 0, 40, 35000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (121, '上古神甲', 10, 6, 62, 0, 0, 0, 85, 35000, 5, 13, 4, 8, 1, 2, 0, 0, 0, 0, 5, 50, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (122, '上古宝甲', 11, 6, 62, 0, 0, 0, 88, 35000, 5, 13, 4, 8, 1, 2, 0, 0, 0, 0, 5, 50, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (123, '上古道袍', 10, 8, 37, 0, 0, 0, 87, 30000, 4, 10, 4, 7, 0, 0, 0, 0, 2, 5, 5, 50, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (124, '上古长袍', 11, 8, 37, 0, 0, 0, 90, 30000, 4, 10, 4, 7, 0, 0, 0, 0, 2, 5, 5, 50, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (125, '上古披风', 10, 7, 21, 0, 0, 0, 86, 26000, 4, 10, 4, 7, 0, 0, 2, 5, 0, 0, 5, 50, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (126, '上古羽衣', 11, 7, 21, 0, 0, 0, 89, 26000, 4, 10, 4, 7, 0, 0, 2, 5, 0, 0, 5, 50, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (127, '雷霆战甲(男)', 10, 9, 60, 15, 0, 0, 869, 60000, 5, 12, 5, 8, 1, 3, 0, 0, 0, 0, 0, 42, 45000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (128, '雷霆战甲(女)', 11, 9, 60, 16, 0, 0, 870, 60000, 5, 12, 5, 8, 1, 3, 0, 0, 0, 0, 0, 42, 45000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (129, '光芒道袍(男)', 10, 9, 35, 1, 0, 0, 590, 50000, 5, 12, 5, 8, 0, 0, 0, 0, 3, 5, 0, 42, 45000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (130, '光芒道袍(女)', 11, 9, 35, 2, 0, 0, 591, 50000, 5, 12, 5, 8, 0, 0, 0, 0, 3, 5, 0, 42, 45000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (131, '烈焰魔衣(男)', 10, 9, 20, 5, 0, 0, 871, 40000, 5, 12, 5, 8, 0, 0, 3, 5, 0, 0, 0, 42, 45000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (132, '烈焰魔衣(女)', 11, 9, 20, 6, 0, 0, 872, 40000, 5, 12, 5, 8, 0, 0, 3, 5, 0, 0, 0, 42, 45000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (133, '凤天魔甲', 10, 11, 23, 50, 0, 0, 596, 50000, 8, 15, 8, 8, 2, 2, 3, 5, 3, 5, 0, 45, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (134, '凰天魔衣', 11, 11, 23, 50, 0, 0, 606, 50000, 8, 15, 8, 8, 2, 2, 3, 5, 3, 5, 0, 45, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (135, '凰神战铠', 10, 32, 40, 50, 0, 0, 1590, 50000, 8, 13, 8, 10, 2, 4, 0, 0, 0, 0, 0, 43, 70000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (136, '凤神战铠', 11, 32, 40, 50, 0, 0, 1600, 50000, 8, 13, 8, 10, 2, 4, 0, 0, 0, 0, 0, 43, 70000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (137, '幽冥道袍', 10, 34, 30, 50, 0, 0, 1593, 40000, 8, 13, 8, 10, 0, 0, 0, 0, 3, 7, 0, 43, 70000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (138, '幽冥仙袍', 11, 34, 30, 50, 0, 0, 1603, 40000, 8, 13, 8, 10, 0, 0, 0, 0, 3, 7, 0, 43, 70000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (139, '魂帝凰袍', 10, 33, 20, 50, 0, 0, 1591, 30000, 8, 13, 8, 10, 0, 0, 3, 7, 0, 0, 0, 43, 70000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (140, '魂帝凤袍', 11, 33, 20, 50, 0, 0, 1601, 30000, 8, 13, 8, 10, 0, 0, 3, 7, 0, 0, 0, 43, 70000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (141, '天龙圣衣(男)', 10, 10, 25, 3, 0, 0, 595, 50000, 9, 16, 8, 10, 2, 3, 3, 6, 3, 6, 0, 47, 65000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (142, '天龙圣衣(女)', 11, 10, 25, 4, 0, 0, 605, 50000, 9, 16, 8, 10, 2, 3, 3, 6, 3, 6, 0, 47, 65000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (143, '狂雷战甲(男)', 10, 25, 50, 15, 0, 0, 1380, 50000, 9, 15, 6, 12, 2, 5, 0, 0, 0, 0, 0, 50, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (144, '狂雷战甲(女)', 11, 25, 50, 16, 0, 0, 1390, 50000, 9, 15, 6, 12, 2, 5, 0, 0, 0, 0, 0, 50, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (145, '通云道袍(男)', 10, 27, 30, 1, 0, 0, 1382, 40000, 9, 15, 6, 12, 0, 0, 0, 0, 3, 8, 0, 50, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (146, '通云道袍(女)', 11, 27, 30, 2, 0, 0, 1392, 40000, 9, 15, 6, 12, 0, 0, 0, 0, 3, 8, 0, 50, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (147, '逆火魔袍(男)', 10, 26, 20, 5, 0, 0, 1381, 30000, 9, 15, 6, 12, 0, 0, 3, 8, 0, 0, 0, 50, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (148, '逆火魔袍(女)', 11, 26, 20, 6, 0, 0, 1391, 30000, 9, 15, 6, 12, 0, 0, 3, 8, 0, 0, 0, 50, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (149, '王者战甲(男)', 10, 28, 60, 50, 0, 0, 1396, 55000, 10, 18, 10, 15, 1, 6, 0, 0, 0, 0, 0, 52, 85000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (150, '王者战甲(女)', 11, 28, 60, 50, 0, 0, 1397, 55000, 10, 18, 10, 15, 1, 6, 0, 0, 0, 0, 0, 52, 85000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (151, '王者道袍(男)', 10, 30, 45, 50, 0, 0, 1398, 42000, 10, 18, 10, 15, 0, 0, 0, 0, 2, 9, 0, 52, 85000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (152, '王者道袍(女)', 11, 30, 45, 50, 0, 0, 1399, 42000, 10, 18, 10, 15, 0, 0, 0, 0, 2, 9, 0, 52, 85000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (153, '王者魔衣(男)', 10, 29, 20, 50, 0, 0, 1394, 32000, 10, 18, 10, 15, 0, 0, 2, 9, 0, 0, 0, 52, 85000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (154, '王者魔衣(女)', 11, 29, 20, 50, 0, 0, 1395, 32000, 10, 18, 10, 15, 0, 0, 2, 9, 0, 0, 0, 52, 85000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (155, '主宰神甲(男)', 10, 54, 25, 28, 0, 0, 2420, 60000, 12, 20, 12, 16, 3, 6, 4, 9, 4, 10, 0, 55, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (156, '主宰神甲(女)', 11, 54, 25, 29, 0, 0, 2421, 60000, 12, 20, 12, 16, 3, 6, 4, 9, 4, 10, 0, 55, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (157, '龙鳞战甲(男)', 10, 75, 70, 0, 0, 0, 5173, 56000, 13, 25, 12, 16, 2, 7, 0, 0, 0, 0, 0, 58, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (158, '龙鳞战甲(女)', 11, 75, 70, 0, 0, 0, 5174, 56000, 13, 25, 12, 16, 2, 7, 0, 0, 0, 0, 0, 58, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (159, '天极道衣(男)', 10, 77, 40, 0, 0, 0, 5175, 45000, 13, 23, 12, 18, 0, 0, 0, 0, 3, 12, 0, 58, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (160, '天极道衣(女)', 11, 77, 40, 0, 0, 0, 5176, 45000, 13, 23, 12, 18, 0, 0, 0, 0, 3, 12, 0, 58, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (161, '袁灵法衣(男)', 10, 76, 22, 0, 0, 0, 5177, 33000, 13, 20, 12, 20, 0, 0, 3, 11, 0, 0, 0, 58, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (162, '袁灵法衣(女)', 11, 76, 22, 0, 0, 0, 5178, 33000, 13, 20, 12, 20, 0, 0, 3, 11, 0, 0, 0, 58, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (163, '勇霖宝甲(男)', 10, 35, 80, 24, 0, 0, 1900, 60000, 15, 28, 13, 18, 2, 8, 0, 0, 0, 0, 0, 60, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (164, '勇霖宝甲(女)', 11, 35, 80, 25, 0, 0, 1910, 60000, 15, 28, 13, 18, 2, 8, 0, 0, 0, 0, 0, 60, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (165, '赤贯道衣(男)', 10, 31, 50, 50, 0, 0, 1400, 60000, 15, 25, 13, 20, 0, 0, 0, 0, 3, 13, 0, 60, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (166, '赤贯道衣(女)', 11, 31, 50, 50, 0, 0, 1401, 60000, 15, 25, 13, 20, 0, 0, 0, 0, 3, 13, 0, 60, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (167, '邪魔炎甲(男)', 10, 36, 25, 26, 0, 0, 2240, 60000, 15, 23, 13, 22, 0, 0, 3, 12, 0, 0, 0, 60, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (168, '邪魔炎甲(女)', 11, 36, 25, 27, 0, 0, 2241, 60000, 15, 23, 13, 22, 0, 0, 3, 12, 0, 0, 0, 60, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (169, '明光凰甲', 10, 78, 25, 42, 0, 0, 5179, 60000, 26, 42, 23, 35, 0, 0, 0, 0, 0, 0, 0, 50, 200000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (170, '明光凤衣', 11, 78, 25, 42, 0, 0, 5180, 60000, 26, 42, 23, 35, 0, 0, 0, 0, 0, 0, 0, 50, 200000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (171, '麒麟宝铠(男)', 10, 56, 90, 50, 0, 0, 2462, 60000, 16, 32, 15, 20, 3, 9, 0, 0, 0, 0, 0, 63, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (172, '麒麟宝铠(女)', 11, 56, 90, 50, 0, 0, 2463, 60000, 16, 32, 15, 20, 3, 9, 0, 0, 0, 0, 0, 63, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (173, '阴阳圣衣(男)', 10, 58, 55, 50, 0, 0, 2590, 60000, 16, 28, 15, 22, 0, 0, 0, 0, 4, 14, 0, 63, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (174, '阴阳圣衣(女)', 11, 58, 55, 50, 0, 0, 2591, 60000, 16, 28, 15, 22, 0, 0, 0, 0, 4, 14, 0, 63, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (175, '仙风神袍(男)', 10, 55, 28, 50, 0, 0, 2460, 60000, 16, 25, 15, 24, 0, 0, 4, 13, 0, 0, 0, 63, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (176, '仙风神袍(女)', 11, 55, 28, 50, 0, 0, 2461, 60000, 16, 25, 15, 24, 0, 0, 4, 13, 0, 0, 0, 63, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (177, '天赐战甲(男)', 10, 79, 100, 50, 0, 0, 5181, 60000, 18, 36, 16, 22, 4, 10, 0, 0, 0, 0, 0, 65, 200000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (178, '天赐战甲(女)', 11, 79, 100, 50, 0, 0, 5182, 60000, 18, 36, 16, 22, 4, 10, 0, 0, 0, 0, 0, 65, 200000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (179, '龙血宝甲(男)', 10, 80, 60, 50, 0, 0, 5183, 50000, 18, 32, 16, 24, 0, 0, 0, 0, 4, 15, 0, 65, 200000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (180, '龙血宝甲(女)', 11, 80, 60, 50, 0, 0, 5184, 50000, 18, 32, 16, 24, 0, 0, 0, 0, 4, 15, 0, 65, 200000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (181, '魔鬼战甲(男)', 10, 81, 30, 50, 0, 0, 5185, 40000, 18, 28, 16, 26, 0, 0, 5, 14, 0, 0, 0, 65, 200000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (182, '魔鬼战甲(女)', 11, 81, 30, 50, 0, 0, 5186, 40000, 18, 28, 16, 26, 0, 0, 5, 14, 0, 0, 0, 65, 200000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (183, '传奇神甲(男)', 10, 57, 30, 30, 0, 0, 2540, 60000, 22, 38, 18, 25, 5, 12, 0, 0, 0, 0, 0, 68, 150000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (184, '传奇神甲(女)', 11, 57, 30, 31, 0, 0, 2542, 60000, 22, 38, 18, 25, 5, 12, 0, 0, 0, 0, 0, 68, 150000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (185, '日月神甲(男)', 10, 57, 30, 30, 0, 0, 2540, 50000, 20, 35, 19, 28, 0, 0, 0, 0, 5, 16, 0, 68, 150000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (186, '日月神甲(女)', 11, 57, 30, 31, 0, 0, 2542, 50000, 20, 35, 19, 28, 0, 0, 0, 0, 5, 16, 0, 68, 150000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (187, '震天魔衣(男)', 10, 57, 30, 30, 0, 0, 2540, 45000, 19, 33, 19, 30, 0, 0, 6, 16, 0, 0, 0, 68, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (188, '震天魔衣(女)', 11, 57, 30, 31, 0, 0, 2542, 45000, 19, 33, 19, 30, 0, 0, 6, 16, 0, 0, 0, 68, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (189, '皓月神甲(男)', 10, 60, 35, 0, 0, 0, 2770, 60000, 25, 40, 22, 32, 8, 15, 8, 18, 8, 22, 0, 70, 200000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (190, '皓月神甲(女)', 11, 60, 35, 0, 0, 0, 2771, 60000, 25, 40, 22, 32, 8, 15, 8, 18, 8, 22, 0, 70, 200000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (191, '★★头盔★★', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (192, '青铜头盔', 15, 0, 4, 0, 0, 0, 100, 8000, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (193, '魔法头盔', 15, 0, 4, 0, 0, 0, 100, 8000, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 14, 1800, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (194, '道士头盔', 15, 0, 3, 0, 0, 0, 106, 8000, 1, 2, 2, 3, 0, 0, 0, 0, 0, 0, 0, 24, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (195, '骷髅头盔', 15, 0, 5, 0, 0, 0, 103, 8000, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 1, 30, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (196, '天之黑铁头盔', 15, 0, 20, 0, 0, 0, 344, 10000, 4, 5, 2, 3, 0, 1, 0, 0, 0, 0, 5, 30, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (197, '黑铁头盔(极品)', 15, 0, 20, 0, 0, 0, 344, 10000, 4, 5, 2, 3, 0, 1, 0, 0, 0, 0, 1, 46, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (198, '圣龙盔', 15, 0, 3, 0, 0, 0, 743, 7000, 5, 6, 0, 0, 0, 2, 0, 0, 0, 0, 0, 42, 30000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (199, '魔龙盔', 15, 0, 2, 0, 0, 0, 743, 7000, 5, 6, 0, 0, 0, 0, 0, 2, 0, 0, 0, 42, 30000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (200, '天龙盔', 15, 0, 20, 0, 0, 0, 743, 7000, 5, 6, 0, 0, 0, 0, 0, 0, 0, 2, 0, 42, 30000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (201, '黄金头盔(战)', 15, 0, 5, 0, 0, 0, 1113, 10000, 5, 6, 3, 4, 1, 2, 0, 0, 0, 0, 0, 40, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (202, '黄金头盔(道)', 15, 0, 5, 0, 0, 0, 1113, 10000, 5, 6, 3, 4, 0, 0, 0, 0, 1, 2, 0, 40, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (203, '黄金头盔(法)', 15, 0, 5, 0, 0, 0, 1113, 10000, 5, 6, 3, 4, 0, 0, 1, 2, 0, 0, 0, 40, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (204, '赤金头盔', 15, 0, 5, 0, 0, 0, 1117, 10000, 5, 10, 8, 12, 0, 0, 0, 0, 0, 0, 0, 40, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (205, '斗笠', 16, 0, 1, 0, 0, 0, 1019, 30000, 0, 2, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (206, '★武器★', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (207, '木剑', 5, 1, 7, 0, 0, 0, 30, 4000, 0, 0, 0, 0, 2, 5, 0, 0, 0, 0, 0, 1, 50, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (208, '匕首', 5, 6, 5, 0, 0, 0, 35, 10000, 0, 0, 0, 0, 4, 5, 0, 0, 0, 0, 0, 1, 500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (209, '乌木剑', 5, 1, 8, 0, 0, 0, 43, 7000, 0, 0, 0, 0, 4, 8, 0, 1, 0, 0, 0, 1, 4000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (210, '青铜剑', 5, 2, 9, 0, 0, 0, 31, 6000, 0, 0, 0, 0, 3, 7, 0, 0, 0, 0, 0, 5, 900, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (211, '短剑', 5, 4, 9, 0, 0, 0, 33, 8000, 0, 0, 0, 0, 3, 11, 0, 0, 0, 0, 0, 10, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (212, '铁剑', 5, 2, 10, 0, 0, 0, 36, 10000, 0, 0, 0, 0, 5, 9, 0, 0, 0, 0, 0, 10, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (213, '鹤嘴锄', 6, 19, 10, 0, 0, 0, 50, 10000, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 15, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (214, '青铜斧', 5, 3, 10, 0, 0, 0, 32, 10000, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 13, 1500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (215, '八荒', 5, 15, 25, 27, 0, 0, 44, 18000, 0, 0, 0, 0, 4, 12, 0, 0, 0, 0, 0, 15, 4500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (216, '凌风', 5, 5, 20, 0, 0, 0, 34, 18000, 0, 0, 0, 0, 6, 12, 0, 0, 0, 0, 0, 19, 4500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (217, '破魂', 5, 6, 8, 0, 0, 0, 51, 11000, 0, 2, 0, 0, 8, 10, 0, 0, 0, 0, 0, 20, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (218, '斩马刀', 5, 10, 27, 0, 0, 0, 37, 19000, 0, 0, 0, 0, 5, 15, 0, 0, 0, 0, 0, 20, 6000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (219, '修罗', 5, 7, 40, 0, 0, 0, 40, 25000, 0, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 22, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (220, '凝霜', 5, 13, 20, 0, 0, 0, 45, 20000, 0, 0, 0, 0, 10, 13, 0, 0, 0, 0, 0, 25, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (221, '炼狱', 6, 11, 60, 0, 0, 0, 41, 28000, 0, 0, 0, 0, 0, 25, 0, 0, 0, 0, 0, 26, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (222, '井中月', 5, 17, 58, 0, 0, 0, 48, 30000, 0, 0, 0, 0, 7, 22, 0, 0, 0, 0, 0, 28, 30000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (223, '裁决之杖', 6, 24, 80, 0, 0, 0, 55, 32000, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 30, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (224, '命运之刃', 5, 29, 48, 0, 0, 0, 65, 20000, 0, 0, 0, 0, 12, 16, 0, 0, 0, 0, 0, 35, 23000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (225, '屠龙', 5, 26, 99, 0, 0, 0, 57, 33000, 0, 0, 0, 0, 5, 35, 0, 0, 0, 0, 0, 34, 70000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (226, '半月', 5, 16, 16, 0, 0, 0, 46, 14000, 0, 0, 0, 0, 5, 10, 0, 1, 1, 1, 0, 15, 3000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (227, '降魔', 5, 14, 20, 0, 0, 0, 47, 17000, 0, 1, 0, 0, 6, 11, 0, 0, 1, 2, 0, 20, 9000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (228, '银蛇', 5, 9, 26, 0, 0, 0, 38, 24000, 0, 1, 0, 0, 7, 14, 0, 0, 1, 3, 0, 26, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (229, '无极棍', 5, 21, 15, 0, 0, 0, 52, 25000, 0, 0, 0, 0, 8, 16, 0, 0, 3, 5, 3, 25, 40000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (230, '龙纹剑', 5, 25, 40, 0, 0, 0, 56, 22000, 0, 0, 0, 0, 8, 20, 0, 0, 3, 6, 0, 35, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (231, '海魂', 6, 8, 12, 0, 0, 0, 39, 12000, 0, 0, 0, 0, 3, 10, 1, 2, 0, 0, 0, 15, 3000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (232, '偃月', 6, 18, 13, 0, 0, 0, 49, 12000, 0, 0, 0, 0, 4, 10, 1, 3, 0, 0, 0, 20, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (233, '魔杖', 6, 12, 10, 0, 0, 0, 42, 15000, 0, 0, 0, 0, 5, 9, 2, 5, 0, 0, 0, 26, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (234, '骨玉权杖', 6, 28, 20, 0, 0, 0, 59, 18000, 0, 0, 0, 0, 6, 12, 2, 6, 0, 0, 0, 35, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (235, '血饮', 5, 22, 12, 0, 0, 0, 53, 20000, 0, 5, 0, 0, 6, 16, 3, 5, 0, 0, 2, 27, 40000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (236, '嗜魂法杖', 6, 27, 26, 0, 0, 0, 58, 10000, 0, 0, 0, 0, 6, 13, 2, 8, 0, 0, 0, 35, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (237, '龙牙', 5, 31, 25, 0, 0, 0, 69, 28000, 0, 5, 0, 0, 10, 18, 3, 6, 0, 0, 2, 28, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (238, '怒斩', 5, 32, 85, 0, 0, 0, 70, 35000, 0, 3, 0, 0, 12, 26, 0, 0, 0, 0, 1, 46, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (239, '逍遥扇', 5, 33, 45, 0, 0, 0, 71, 30000, 0, 0, 0, 0, 5, 13, 0, 0, 4, 10, 0, 35, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (240, '霸者之刃', 5, 29, 28, 0, 0, 0, 65, 35000, 0, 2, 0, 0, 6, 32, 2, 7, 3, 8, 0, 46, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (241, '天之血饮', 5, 22, 12, 0, 0, 0, 53, 20000, 0, 5, 0, 0, 6, 16, 4, 8, 0, 0, 5, 30, 60000, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (242, '天之骨玉权杖', 6, 28, 20, 0, 0, 0, 59, 18000, 0, 0, 0, 0, 6, 12, 3, 9, 0, 0, 5, 30, 60000, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (243, '天之龙纹剑', 5, 25, 40, 0, 0, 0, 56, 22000, 0, 0, 0, 0, 8, 20, 0, 0, 4, 9, 5, 30, 60000, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (244, '天之裁决之杖', 6, 24, 80, 0, 0, 0, 55, 32000, 0, 0, 0, 0, 1, 33, 0, 0, 0, 0, 5, 30, 60000, 500, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (245, '天之怒斩', 5, 32, 85, 0, 0, 0, 70, 35000, 0, 3, 0, 0, 13, 29, 0, 0, 0, 0, 5, 50, 60000, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (246, '天之龙牙', 5, 31, 25, 0, -1, 0, 69, 28000, 0, 5, 0, 0, 12, 18, 4, 9, 0, 0, 5, 50, 60000, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (247, '天之逍遥扇', 5, 33, 45, 0, 0, 0, 71, 30000, 1, 0, 0, 0, 5, 13, 0, 0, 4, 8, 5, 50, 60000, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (248, '天之嗜魂法杖', 6, 27, 26, 0, 0, 0, 58, 10000, 0, 0, 0, 0, 6, 13, 3, 8, 0, 0, 5, 100, 60000, 1200, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (249, '天之屠龙', 5, 26, 99, 0, 0, 0, 57, 33000, 0, 0, 0, 0, 5, 36, 0, 0, 0, 0, 5, 100, 60000, 2000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (250, '开天', 5, 34, 73, 0, 0, 0, 72, 35000, 0, 0, 0, 0, 6, 40, 0, 0, 0, 0, 0, 43, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (251, '玄天', 5, 36, 44, 0, -4, 0, 74, 25000, 0, 1, 0, 0, 8, 31, 0, 0, 5, 12, 0, 43, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (252, '镇天', 5, 35, 26, 0, 0, 0, 73, 20000, 0, 0, 0, 0, 7, 25, 5, 12, 0, 0, 0, 43, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (253, '龙炎之刃', 5, 59, 26, 0, 0, 0, 1413, 35000, 0, 2, 0, 0, 5, 37, 3, 10, 4, 12, 0, 45, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (254, '天龙圣剑', 5, 37, 20, 0, 0, 0, 828, 60000, 0, 2, 0, 0, 8, 43, 8, 15, 8, 15, 0, 47, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (255, '炎龙刃', 5, 54, 52, 0, -5, 0, 1408, 40000, 0, 2, 0, 0, 8, 45, 0, 0, 0, 0, 0, 50, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (256, '青龙刺', 5, 52, 32, 0, -5, 0, 1406, 35000, 0, 2, 0, 0, 8, 33, 0, 0, 6, 18, 0, 50, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (257, '雷龙杖', 5, 53, 28, 0, -5, 0, 1407, 25000, 0, 2, 0, 0, 7, 27, 8, 18, 0, 0, 0, 50, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (258, '王者之刃', 5, 61, 80, 0, 0, 0, 1420, 40000, 0, 0, 0, 0, 8, 52, 0, 0, 0, 0, 0, 52, 150000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (259, '王者之剑', 5, 62, 55, 0, -5, 0, 1421, 30000, 0, 1, 0, 0, 8, 32, 0, 0, 6, 23, 0, 52, 150000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (260, '王者之杖', 6, 56, 25, 0, 0, 0, 1410, 28000, 0, 0, 0, 0, 7, 26, 8, 21, 0, 0, 0, 52, 150000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (261, '主宰神剑', 5, 78, 48, 0, 0, 0, 2423, 60000, 0, 3, 0, 0, 12, 58, 12, 24, 13, 26, 0, 55, 200000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (262, '开天战廓', 6, 55, 80, 0, 0, 0, 1409, 90000, 0, 3, 0, 0, 6, 66, 0, 0, 0, 0, 0, 58, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (263, '乾坤', 5, 57, 50, 0, 0, 0, 1411, 90000, 0, 3, 0, 0, 10, 33, 0, 0, 8, 29, 0, 58, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (264, '玄灵青蟒', 5, 50, 35, 240, 0, 0, 1404, 90000, 0, 3, 0, 0, 10, 33, 8, 28, 0, 0, 0, 58, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (265, '热血之刃', 5, 51, 26, 0, 0, 0, 1405, 35000, 0, 0, 0, 0, 15, 55, 10, 22, 10, 23, 0, 50, 150000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (266, '霸天鬼斩', 6, 58, 100, 0, 0, 0, 1412, 45000, 0, 0, 0, 0, 10, 75, 0, 0, 0, 0, 0, 60, 130000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (267, '鹰魂神刃', 5, 54, 30, 0, 0, 0, 1408, 30000, 0, 5, 0, 0, 9, 36, 0, 0, 10, 33, 0, 60, 130000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (268, '冥神之刃', 5, 70, 20, 0, 0, 0, 2180, 20000, 0, 0, 0, 0, 8, 36, 9, 32, 0, 0, 0, 60, 130000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (269, '倚天剑', 5, 68, 25, 192, 0, 0, 1880, 30000, 0, 2, 0, 0, 5, 40, 7, 13, 7, 13, 0, 55, 200000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (270, '霹雷', 5, 41, 60, 0, 0, 0, 5170, 50000, 0, 3, 0, 0, 12, 85, 0, 0, 0, 0, 0, 63, 180000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (271, '泰轮佛尘', 5, 42, 45, 0, -5, 0, 5171, 33000, 0, 3, 0, 0, 12, 25, 0, 0, 13, 40, 0, 63, 180000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (272, '天狼刀', 5, 43, 25, 0, 0, 0, 5172, 28000, 0, 5, 0, 0, 12, 30, 12, 38, 0, 0, 0, 63, 180000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (273, '破山剑', 5, 92, 99, 0, 0, 0, 5048, 45000, 0, 5, 0, 0, 22, 98, 0, 0, 0, 0, 0, 65, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (274, '邪魔血刀', 5, 93, 50, 0, -8, 0, 5049, 30000, 0, 5, 0, 0, 22, 32, 0, 0, 14, 46, 0, 65, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (275, '天神法杖', 5, 94, 30, 0, 0, 0, 5050, 16000, 0, 7, 0, 0, 8, 16, 12, 45, 0, 0, 0, 65, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (276, '传奇神剑', 6, 80, 150, 199, 0, 0, 2523, 60000, 0, 5, 0, 0, 15, 120, 0, 0, 0, 0, 0, 68, 200000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (277, '日月神扇', 5, 81, 60, 199, -5, 0, 2524, 35000, 0, 5, 0, 0, 9, 35, 0, 0, 15, 53, 0, 68, 200000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (278, '震天神杖', 6, 82, 28, 199, 0, 0, 2525, 30000, 0, 5, 0, 0, 8, 28, 13, 50, 0, 0, 0, 68, 200000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (279, '皓月神剑', 6, 83, 180, 200, 0, 0, 2765, 60000, 0, 7, 0, 11, 18, 130, 0, 0, 0, 0, 0, 70, 250000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (280, '皓月神扇', 5, 84, 65, 200, 0, 0, 2766, 35000, 0, 7, 0, 0, 12, 38, 0, 0, 15, 65, 0, 70, 250000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (281, '皓月神杖', 6, 85, 30, 200, 0, 0, 2767, 30000, 0, 7, 0, 0, 10, 36, 15, 60, 0, 0, 0, 70, 250000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (282, '★首饰★', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (283, '古铜戒指', 22, 0, 1, 0, 0, 0, 145, 5000, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 3, 500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (284, '玻璃戒指', 22, 0, 1, 0, 0, 0, 143, 3000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 7, 800, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (285, '六角戒指', 22, 0, 1, 0, 0, 0, 144, 6000, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 7, 800, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (286, '牛角戒指', 22, 0, 1, 0, 0, 0, 140, 6000, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 9, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (287, '蓝色水晶戒指', 22, 0, 1, 0, 0, 0, 142, 10000, 0, 0, 0, 2, 1, 0, 0, 0, 0, 0, 0, 16, 1500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (288, '生铁戒指', 22, 0, 1, 0, 0, 0, 141, 5000, 0, 2, 0, 0, 0, 0, 0, 1, 0, 0, 2, 9, 3000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (289, '黑色水晶戒指', 22, 0, 1, 0, 0, 0, 163, 5000, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 20, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (290, '珍珠戒指', 22, 0, 1, 0, 0, 0, 161, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 20, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (291, '蛇眼戒指', 22, 0, 1, 0, 0, 0, 165, 5000, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 20, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (292, '金戒指', 22, 0, 1, 0, 0, 0, 148, 5000, 0, 0, 0, 3, 1, 1, 1, 1, 1, 1, 0, 20, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (293, '骷髅戒指', 22, 0, 1, 0, 0, 0, 177, 5000, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 1, 30, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (294, '道德戒指', 23, 0, 1, 0, 0, 0, 153, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 0, 23, 4000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (295, '魅力戒指', 23, 0, 1, 0, 0, 0, 159, 5000, 0, 0, 0, 0, 0, 0, 1, 2, 0, 0, 0, 23, 4000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (296, '降妖除魔戒指', 22, 0, 1, 0, 0, 0, 152, 5000, 0, 0, 0, 4, 1, 2, 1, 2, 1, 2, 0, 25, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (297, '珊瑚戒指', 22, 0, 1, 0, 0, 0, 164, 5000, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 25, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (298, '金项链', 20, 0, 1, 0, 0, 0, 222, 8000, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 2, 500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (299, '传统项链', 20, 0, 1, 0, 0, 0, 237, 8000, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (300, '黑色水晶项链', 20, 0, 1, 0, 0, 0, 225, 8000, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 13, 1500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (301, '黄色水晶项链', 20, 0, 1, 0, 0, 0, 221, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 13, 1200, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (302, '黑檀项链', 20, 0, 1, 0, 0, 0, 220, 8000, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 13, 1200, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (303, '灯笼项链', 19, 0, 1, 0, 0, 0, 233, 8000, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 18, 3000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (304, '白色虎齿项链', 19, 0, 1, 0, 0, 0, 230, 7000, 0, 2, 0, 0, 0, 0, 0, 0, 1, 1, 3, 11, 3500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (305, '琥珀项链', 20, 0, 1, 0, 0, 0, 236, 6000, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 17, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (306, '魔鬼项链', 20, 0, 1, 0, 0, 0, 235, 8000, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 17, 2500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (307, '凤凰明珠', 20, 0, 1, 0, 0, 0, 243, 7000, 0, 1, 0, 0, 0, 0, 0, 0, 1, 2, 0, 17, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (308, '白金项链', 20, 0, 1, 0, 0, 0, 231, 6000, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 10, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (309, '蓝翡翠项链', 20, 0, 1, 0, 0, 0, 246, 8000, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 23, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (310, '竹笛', 20, 0, 1, 0, 0, 0, 242, 7000, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 0, 24, 6000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (311, '放大镜', 20, 0, 1, 0, 0, 0, 245, 6000, 0, 0, 0, 0, 0, 0, 1, 3, 0, 0, 0, 24, 6000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (312, '铁手镯', 24, 0, 1, 0, 0, 0, 180, 4000, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 800, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (313, '小手镯', 26, 0, 1, 0, 0, 0, 192, 5000, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 5, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (314, '银手镯', 24, 0, 2, 0, 0, 0, 202, 7000, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 7, 1500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (315, '大手镯', 26, 0, 2, 0, 0, 0, 203, 10000, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (316, '钢手镯', 26, 0, 1, 0, 0, 0, 180, 5000, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 8, 1800, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (317, '皮制手套', 26, 0, 2, 0, 0, 0, 190, 6000, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 1500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (318, '坚固手套', 26, 0, 3, 0, 0, 0, 191, 8000, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 6000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (319, '金手镯', 26, 0, 1, 0, 0, 0, 207, 7000, 0, 0, 2, 3, 0, 1, 0, 0, 0, 0, 0, 23, 6000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (320, '魔法手镯', 26, 0, 1, 0, 0, 0, 205, 7000, 0, 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 18, 4000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (321, '魔力手镯', 26, 0, 2, 0, 0, 0, 290, 4000, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 20, 4000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (322, '道士手镯', 26, 0, 1, 0, 0, 0, 198, 7000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 19, 4000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (323, '黑檀手镯', 26, 0, 1, 0, 0, 0, 211, 6000, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 19, 4000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (324, '死神手套', 26, 0, 2, 0, 0, 0, 188, 8000, 0, 0, 0, 0, 1, 2, 0, 0, 0, 0, 0, 22, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (325, '龙之戒指', 22, 0, 1, 0, 0, 0, 169, 5000, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 1, 37, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (326, '铂金戒指', 22, 0, 1, 0, 0, 0, 173, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 3, 17, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (327, '红宝石戒指', 22, 0, 1, 0, 0, 0, 166, 5000, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 2, 17, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (328, '幽灵手套', 26, 0, 5, 0, 0, 0, 186, 8000, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (329, '三眼手镯', 26, 0, 1, 0, 0, 0, 208, 7000, 1, 1, 0, 0, 0, 0, 0, 0, 1, 3, 3, 22, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (330, '思贝儿手镯', 26, 0, 1, 0, 0, 0, 210, 6000, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 26, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (331, '幽灵项链', 20, 0, 1, 0, 0, 0, 219, 8000, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 24, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (332, '生命项链', 20, 0, 1, 0, 0, 0, 320, 6000, 0, 1, 0, 0, 0, 0, 1, 5, 0, 0, 2, 25, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (333, '天珠项链', 20, 0, 1, 0, 0, 0, 218, 7000, 0, 0, 0, 0, 0, 0, 0, 0, 1, 5, 3, 22, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (334, '力量戒指', 22, 0, 3, 0, 0, 0, 336, 6000, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 1, 46, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (335, '泰坦戒指', 22, 0, 2, 0, 0, 0, 182, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 2, 6, 3, 24, 15, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (336, '紫碧螺', 22, 0, 2, 0, 0, 0, 183, 5000, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 2, 24, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (337, '骑士手镯', 26, 0, 2, 0, 0, 0, 209, 8000, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 1, 39, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (338, '心灵手镯', 26, 0, 1, 0, 0, 0, 328, 7000, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 24, 13000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (339, '龙之手镯', 26, 0, 3, 0, 0, 0, 214, 6000, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 2, 28, 12000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (340, '阎罗手套', 26, 0, 10, 0, 0, 0, 187, 8000, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (341, '绿色项链', 20, 0, 1, 0, 0, 0, 240, 8000, 0, 0, 0, 0, 2, 5, 0, 0, 0, 0, 1, 37, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (342, '灵魂项链', 20, 0, 1, 0, 0, 0, 239, 7000, 0, 2, 0, 0, 0, 0, 0, 0, 1, 6, 3, 23, 18000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (343, '恶魔铃铛', 20, 0, 1, 0, 0, 0, 244, 6000, 0, 2, 0, 0, 0, 0, 0, 7, 0, 0, 2, 27, 18000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (344, '黑铁头盔', 15, 0, 20, 0, 0, 0, 344, 10000, 4, 5, 2, 3, 0, 0, 0, 0, 0, 0, 1, 46, 40000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (345, '避邪手镯', 24, 0, 1, 0, 0, 0, 200, 7000, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 19, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (346, '躲避手链', 20, 0, 1, 0, 0, 0, 238, 8000, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 3, 12, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (347, '夏普儿手镯', 24, 0, 1, 0, 0, 0, 204, 7000, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 16, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (348, '狂风戒指', 23, 0, 1, 0, 0, 0, 162, 5000, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 16, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (349, '狂风项链', 21, 0, 1, 0, 0, 0, 228, 8000, 2, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 19, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (350, '传送戒指', 22, 112, 1, 0, 0, 0, 172, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (351, '隐身戒指', 22, 111, 1, 0, 0, 0, 174, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (352, '麻痹戒指', 22, 113, 1, 0, 0, 0, 168, 5000, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 16, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (353, '复活戒指', 22, 114, 1, 0, 0, 0, 175, 5000, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 16, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (354, '火焰戒指', 22, 115, 1, 0, 0, 0, 171, 5000, 0, 0, 0, 0, 0, 0, 1, 2, 0, 0, 0, 16, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (355, '防御戒指', 22, 116, 1, 0, 0, 0, 167, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 0, 16, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (356, '求婚戒指', 22, 0, 1, 0, 0, 0, 170, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 16, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (357, '护身戒指', 22, 118, 1, 0, 0, 0, 176, 5000, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 16, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (358, '超负载戒指', 22, 119, 1, 0, 0, 0, 170, 5000, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 16, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (359, '技巧项链', 20, 120, 1, 0, 0, 0, 241, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 16, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (360, '探测项链', 20, 121, 1, 0, 0, 0, 248, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (361, '祈福项链', 20, 117, 1, 0, 0, 0, 218, 7000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 22, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (362, '★技能书★', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (363, '基本剑术', 4, 0, 1, 0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (364, '攻杀剑术', 4, 0, 1, 0, 0, 0, 0, 19, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (365, '刺杀剑术', 4, 0, 1, 0, 0, 0, 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (366, '半月弯刀', 4, 0, 1, 0, 0, 0, 0, 28, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (367, '野蛮冲撞', 4, 0, 1, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (368, '烈火剑法', 4, 0, 1, 0, 0, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (369, '治愈术', 4, 2, 1, 0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (370, '精神力战法', 4, 2, 1, 0, 0, 0, 0, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (371, '施毒术', 4, 2, 1, 0, 0, 0, 0, 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (372, '灵魂火符', 4, 2, 1, 0, 0, 0, 0, 18, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (373, '召唤骷髅', 4, 2, 1, 0, 0, 0, 0, 19, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 19, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (374, '隐身术', 4, 2, 1, 0, 0, 0, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (375, '集体隐身术', 4, 2, 1, 0, 0, 0, 0, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (376, '幽灵盾', 4, 2, 1, 0, 0, 0, 0, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (377, '神圣战甲术', 4, 2, 1, 0, 0, 0, 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (378, '心灵启示', 4, 2, 1, 0, 0, 0, 0, 26, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (379, '困魔咒', 4, 2, 1, 0, 0, 0, 0, 28, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (380, '群体治疗术', 4, 2, 1, 0, 0, 0, 0, 33, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (381, '召唤神兽', 4, 2, 1, 0, 0, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (382, '火球术', 4, 1, 1, 0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (383, '抗拒火环', 4, 1, 1, 0, 0, 0, 0, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (384, '诱惑之光', 4, 1, 1, 0, 0, 0, 0, 13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (385, '地狱火', 4, 1, 1, 0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (386, '雷电术', 4, 1, 1, 0, 0, 0, 0, 17, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (387, '瞬息移动', 4, 1, 1, 0, 0, 0, 0, 19, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (388, '大火球', 4, 1, 1, 0, 0, 0, 0, 19, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (389, '爆裂火焰', 4, 1, 1, 0, 0, 0, 0, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (390, '火墙', 4, 1, 1, 0, 0, 0, 0, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (391, '疾光电影', 4, 1, 1, 0, 0, 0, 0, 26, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (392, '地狱雷光', 4, 1, 1, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (393, '魔法盾', 4, 1, 1, 0, 0, 0, 0, 31, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (394, '圣言术', 4, 1, 1, 0, 0, 0, 0, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (395, '冰咆哮', 4, 1, 1, 0, 0, 0, 0, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (396, '★内功书★', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (397, '怒之攻杀', 4, 0, 1, 0, 0, 0, 1582, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (398, '静之攻杀', 4, 99, 1, 0, 0, 0, 1582, 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (399, '怒之半月', 4, 0, 1, 0, 0, 0, 1582, 13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (400, '静之半月', 4, 99, 1, 0, 0, 0, 1582, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (401, '怒之烈火', 4, 0, 1, 0, 0, 0, 1582, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (402, '静之烈火', 4, 99, 1, 0, 0, 0, 1582, 69, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (403, '怒之逐日', 4, 0, 1, 0, 0, 0, 1582, 74, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (404, '静之逐日', 4, 99, 1, 0, 0, 0, 1582, 79, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (405, '怒之火球', 4, 1, 1, 0, 0, 0, 1582, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (406, '静之火球', 4, 99, 1, 0, 0, 0, 1582, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (407, '怒之大火球', 4, 1, 1, 0, 0, 0, 1582, 28, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (408, '静之大火球', 4, 99, 1, 0, 0, 0, 1582, 28, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (409, '怒之火墙', 4, 1, 1, 0, 0, 0, 1582, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (410, '静之火墙', 4, 99, 1, 0, 0, 0, 1582, 39, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (411, '怒之地狱火', 4, 1, 1, 0, 0, 0, 1582, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (412, '静之地狱火', 4, 99, 1, 0, 0, 0, 1582, 16, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (413, '怒之疾光电影', 4, 1, 1, 0, 0, 0, 1582, 49, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (414, '静之疾光电影', 4, 99, 1, 0, 0, 0, 1582, 42, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (415, '怒之爆裂火焰', 4, 1, 1, 0, 0, 0, 1582, 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (416, '静之爆裂火焰', 4, 99, 1, 0, 0, 0, 1582, 29, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (417, '怒之冰咆哮', 4, 1, 1, 0, 0, 0, 1582, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (418, '静之冰咆哮', 4, 99, 1, 0, 0, 0, 1582, 71, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (419, '怒之雷电', 4, 1, 1, 0, 0, 0, 1582, 26, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (420, '静之雷电', 4, 99, 1, 0, 0, 0, 1582, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (421, '怒之地狱雷光', 4, 1, 1, 0, 0, 0, 1582, 55, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (422, '静之地狱雷光', 4, 99, 1, 0, 0, 0, 1582, 51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (423, '怒之寒冰掌', 4, 1, 1, 0, 0, 0, 1582, 61, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (424, '静之寒冰掌', 4, 99, 1, 0, 0, 0, 1582, 61, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (425, '怒之灭天火', 4, 1, 1, 0, 0, 0, 1582, 71, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (426, '静之灭天火', 4, 99, 1, 0, 0, 0, 1582, 75, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (427, '怒之施毒术', 4, 2, 1, 1, 0, 0, 1582, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 239, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (428, '静之施毒术', 4, 99, 1, 1, 0, 0, 1582, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 240, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (429, '怒之月灵', 4, 2, 1, 1, 0, 0, 1582, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 72, 241, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (430, '静之月灵', 4, 99, 1, 1, 0, 0, 1582, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 72, 242, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (431, '怒之火符', 4, 2, 1, 0, 0, 0, 1582, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (432, '静之火符', 4, 99, 1, 0, 0, 0, 1582, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (433, '怒之噬血', 4, 2, 1, 0, 0, 0, 1582, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (434, '静之噬血', 4, 99, 1, 0, 0, 0, 1582, 79, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (435, '怒之流星火雨', 4, 1, 1, 0, 0, 0, 1582, 74, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (436, '静之流星火雨', 4, 99, 1, 0, 0, 0, 1582, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (437, '怒之内功剑法', 4, 99, 1, 0, 0, 0, 1582, 38, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (438, '静之内功剑法', 4, 99, 1, 0, 0, 0, 1582, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (439, '玛法内功心法', 44, 3, 1, 0, 0, 0, 1137, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100000, 5, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (440, '★英雄书★', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (441, '白日门火球术', 4, 1, 1, 0, 0, 0, 1144, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 550, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (442, '白日门治愈术', 4, 2, 1, 0, 0, 0, 1144, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 550, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (443, '白日门剑术', 4, 0, 1, 0, 0, 0, 1144, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 550, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (444, '白日门战法', 4, 2, 1, 0, 0, 0, 1144, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 550, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (445, '白日门抗拒', 4, 1, 1, 0, 0, 0, 1144, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 550, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (446, '白日门诱惑术', 4, 1, 1, 0, 0, 0, 1144, 13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (447, '白日门施毒术', 4, 2, 1, 0, 0, 0, 1144, 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1100, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (448, '白日门地狱火', 4, 1, 1, 0, 0, 0, 1144, 16, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (449, '白日门雷电术', 4, 1, 1, 0, 0, 0, 1144, 17, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1100, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (450, '白日门火符', 4, 2, 1, 0, 0, 0, 1144, 18, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1100, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (451, '白日门大火球', 4, 1, 1, 0, 0, 0, 1144, 19, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2200, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (452, '白日门攻杀', 4, 0, 1, 0, 0, 0, 1144, 19, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (453, '白日门骷髅术', 4, 2, 1, 0, 0, 0, 1144, 19, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (454, '白日门瞬移', 4, 1, 1, 0, 0, 0, 1144, 19, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (455, '白日门隐身', 4, 2, 1, 0, 0, 0, 1144, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (456, '白日门群隐', 4, 2, 1, 0, 0, 0, 1144, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (457, '白日门幽灵盾', 4, 2, 1, 0, 0, 0, 1144, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (458, '白日门爆裂', 4, 1, 1, 0, 0, 0, 1144, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (459, '白日门火墙', 4, 1, 1, 0, 0, 0, 1144, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (460, '白日门战甲术', 4, 2, 1, 0, 0, 0, 1144, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (461, '白日门刺杀', 4, 0, 1, 0, 0, 0, 1144, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (462, '白日门疾光', 4, 1, 1, 0, 0, 0, 1144, 26, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (463, '白日门困魔咒', 4, 2, 1, 0, 0, 0, 1144, 28, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (464, '白日门启示', 4, 2, 1, 0, 0, 0, 1144, 26, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (465, '白日门半月', 4, 0, 1, 0, 0, 0, 1144, 28, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (466, '白日门气功波', 4, 2, 1, 0, 0, 0, 1144, 29, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (467, '白日门野蛮', 4, 0, 1, 0, 0, 0, 1144, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (468, '白日门雷光', 4, 1, 1, 0, 0, 0, 1144, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (469, '白日门魔法盾', 4, 1, 1, 0, 0, 0, 1144, 31, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (470, '白日门圣言术', 4, 1, 1, 0, 0, 0, 1144, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (471, '白日门烈火', 4, 0, 1, 0, 0, 0, 1144, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (472, '白日门群疗', 4, 2, 1, 0, 0, 0, 1144, 33, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (473, '白日门神兽术', 4, 2, 1, 0, 0, 0, 1144, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (474, '白日门冰咆哮', 4, 1, 1, 0, 0, 0, 1144, 35, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (475, '白日门真气', 4, 2, 1, 0, 0, 0, 1144, 36, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (476, '白日门寒冰掌', 4, 1, 1, 0, 0, 0, 1144, 36, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (477, '白日门灭天火', 4, 1, 1, 0, 0, 0, 1144, 38, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (478, '白日门狮子吼', 4, 0, 1, 0, 0, 0, 1144, 38, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (479, '四级英雄盾', 31, 0, 1, 54, 0, 0, 1144, 38, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (480, '白日门酒气', 4, 0, 1, 0, 0, 0, 1144, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (481, '白日门元力', 4, 0, 1, 0, 0, 0, 1144, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (482, '白日门逐日', 4, 0, 1, 0, 0, 0, 1144, 46, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (483, '白日门火雨', 4, 1, 1, 0, 0, 0, 1144, 46, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (484, '白日门噬血术', 4, 2, 1, 0, 0, 0, 1144, 46, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (485, '白日门龙影剑', 4, 0, 1, 0, 0, 0, 1144, 46, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (486, '白日门开天斩', 4, 0, 1, 0, 0, 0, 1144, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (487, '白日门召唤月灵', 4, 2, 1, 0, 0, 0, 1144, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (488, '白日门分身术', 4, 1, 1, 0, 0, 0, 1144, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (489, '白日门护体神盾', 4, 2, 1, 0, 0, 0, 1144, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (490, '破魂斩', 4, 3, 1, 60, 0, 0, 1144, 43, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (491, '劈星斩', 4, 3, 1, 61, 0, 0, 1144, 43, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (492, '雷霆一击', 4, 3, 1, 62, 0, 0, 1144, 43, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (493, '噬魂沼泽', 4, 3, 1, 63, 0, 0, 1144, 43, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (494, '末日审判', 4, 3, 1, 64, 0, 0, 1144, 43, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (495, '火龙气焰', 4, 3, 1, 65, 0, 0, 1144, 43, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (496, '★高级书★', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (497, '先天元力', 4, 99, 1, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (498, '酒气护体', 4, 99, 1, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (499, '狮子吼', 4, 0, 1, 0, 0, 0, 0, 38, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (500, '逐日剑法', 4, 0, 1, 0, 0, 0, 0, 46, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (501, '龙影剑法', 4, 0, 1, 0, 0, 0, 1144, 42, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (502, '开天斩', 4, 0, 1, 0, 0, 0, 1144, 42, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (503, '气功波', 4, 2, 1, 0, 0, 0, 0, 29, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (504, '无极真气', 4, 2, 1, 0, 0, 0, 0, 38, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (505, '护体神盾', 4, 2, 1, 0, 0, 0, 1144, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (506, '召唤月灵', 4, 2, 1, 0, 0, 0, 1144, 45, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (507, '噬血术', 4, 2, 1, 0, 0, 0, 0, 46, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (508, '寒冰掌', 4, 1, 1, 0, 0, 0, 0, 36, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (509, '灭天火', 4, 1, 1, 0, 0, 0, 0, 38, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (510, '流星火雨', 4, 1, 1, 0, 0, 0, 0, 46, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (511, '四级基本剑术', 4, 0, 1, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 88, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (512, '四级英雄剑术', 4, 0, 1, 0, 0, 0, 1144, 50, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 88, 2000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (513, '四级刺杀剑术', 4, 0, 1, 0, 0, 0, 0, 54, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 89, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (514, '四级英雄刺杀', 4, 0, 1, 0, 0, 0, 1144, 54, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 12, 89, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (515, '四级半月弯刀', 4, 0, 1, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 90, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (516, '四级英雄半月', 4, 0, 1, 0, 0, 0, 1144, 50, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 90, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (517, '四级魔法盾', 31, 0, 1, 53, 0, 0, 1144, 38, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (518, '四级雷电术', 4, 1, 1, 0, 0, 0, 0, 54, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 11, 91, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (519, '四级英雄雷电术', 4, 1, 1, 0, 0, 0, 1144, 54, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 11, 91, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (520, '四级流星火雨', 4, 1, 1, 0, 0, 0, 0, 54, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 58, 92, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (521, '四级英雄火雨', 4, 1, 1, 0, 0, 0, 1144, 54, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 58, 92, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (522, '四级施毒术', 4, 2, 1, 0, 0, 0, 0, 54, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 93, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (523, '四级英雄施毒术', 4, 2, 1, 0, 0, 0, 1144, 54, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 93, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (524, '四级噬血术', 4, 2, 1, 0, 0, 0, 0, 54, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 59, 94, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (525, '四级英雄噬血术', 4, 2, 1, 0, 0, 0, 1144, 54, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 59, 94, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (526, '四级召唤神兽', 4, 2, 1, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 71, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (527, '四级英雄神兽', 4, 2, 1, 0, 0, 0, 1144, 50, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 71, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (528, '四级烈火', 4, 0, 1, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 26, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (529, '四级灭天火', 4, 1, 1, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 45, 45, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (530, '四级火符', 4, 2, 1, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 13, 13, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (531, '四级破魂斩', 4, 3, 1, 0, 0, 0, 1144, 50, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 60, 60, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (532, '四级火龙气焰', 4, 3, 1, 0, 0, 0, 1144, 50, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65, 65, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (533, '四级噬魂沼泽', 4, 3, 1, 0, 0, 0, 1144, 50, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 63, 63, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (534, '四级雷霆一击', 4, 3, 1, 0, 0, 0, 1144, 50, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 62, 62, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (535, '四级劈星斩', 4, 3, 1, 0, 0, 0, 1144, 50, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 61, 61, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (536, '四级末日审判', 4, 3, 1, 0, 0, 0, 1144, 50, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 64, 64, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (537, '血魄一击(战)', 4, 0, 1, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 96, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (538, '血魄一击(法)', 4, 1, 1, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 97, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (539, '血魄一击(道)', 4, 2, 1, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 98, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (540, '白日门血魄(战)', 4, 0, 1, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 96, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (541, '白日门血魄(法)', 4, 1, 1, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 97, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (542, '白日门血魄(道)', 4, 2, 1, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 98, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (543, '三绝杀', 4, 0, 1, 0, 0, 0, 1144, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (544, '双龙破', 4, 1, 1, 0, 0, 0, 1144, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (545, '虎啸诀', 4, 2, 1, 0, 0, 0, 1144, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (546, '追心刺', 4, 0, 1, 0, 0, 0, 1144, 33, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (547, '凤舞祭', 4, 1, 1, 0, 0, 0, 1144, 33, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (548, '八卦掌', 4, 2, 1, 0, 0, 0, 1144, 33, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (549, '断岳斩', 4, 0, 1, 0, 0, 0, 1144, 52, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (550, '惊雷爆', 4, 1, 1, 0, 0, 0, 1144, 52, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (551, '三焰咒', 4, 2, 1, 0, 0, 0, 1144, 52, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (552, '横扫千军', 4, 0, 1, 0, 0, 0, 1144, 95, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (553, '冰天雪地', 4, 1, 1, 0, 0, 0, 1144, 95, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (554, '万剑归宗', 4, 2, 1, 0, 0, 0, 1144, 95, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 500, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (555, '★新增技能★', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (556, '解毒术', 4, 2, 1, 0, 0, 0, 1144, 48, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (557, '火焰冰', 4, 1, 1, 0, 0, 0, 1144, 48, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (558, '雷神之怒', 4, 1, 1, 0, 0, 0, 1144, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (559, '群体施毒术', 4, 2, 1, 0, 0, 0, 1144, 48, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (560, '破血狂杀', 4, 0, 1, 0, 0, 0, 1144, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (561, '十方斩', 4, 0, 1, 0, 0, 0, 1144, 48, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (562, '分身术', 4, 1, 1, 0, 0, 0, 1144, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (563, '地狱烈焰', 4, 1, 1, 0, 0, 0, 1144, 48, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (564, '云寂术', 4, 2, 1, 0, 0, 0, 1144, 48, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (565, '月魂风暴', 4, 2, 1, 0, 0, 0, 1144, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (566, '神之诅咒', 4, 2, 1, 0, 0, 0, 1144, 48, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (567, '迅影离魂', 4, 0, 1, 0, 0, 0, 1144, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (568, '召妖降魔', 4, 2, 1, 0, 0, 0, 1144, 18, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (569, '吸星狂杀', 4, 0, 1, 0, 0, 0, 1144, 50, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (570, '异形换位', 4, 1, 1, 0, 0, 0, 1144, 48, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (571, '回生术', 4, 2, 1, 0, 0, 0, 1144, 48, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (572, '★腰带鞋子★', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (573, '兽皮腰带', 54, 0, 1, 0, 0, 0, 550, 6000, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 2000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (574, '铁腰带', 54, 0, 1, 0, 0, 0, 551, 6000, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 22, 3000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (575, '青铜腰带', 54, 0, 1, 0, 0, 0, 552, 8000, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 5000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (576, '钢铁腰带', 54, 0, 2, 0, 0, 0, 553, 10000, 0, 3, 0, 1, 0, 0, 0, 0, 0, 0, 0, 35, 8000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (577, '布鞋', 52, 0, 1, 3, 0, 0, 560, 6000, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 2000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (578, '鹿皮靴', 52, 0, 1, 4, 0, 0, 561, 6000, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 22, 3000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (579, '紫绸靴', 52, 0, 1, 5, 0, 0, 562, 8000, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 5000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (580, '避魂靴', 52, 0, 2, 6, 0, 0, 563, 8000, 1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 35, 8000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (581, '★荣誉勋章★', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (582, '荣誉勋章11号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 3000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (583, '荣誉勋章12号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 7, 3000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (584, '荣誉勋章13号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 7, 3000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (585, '荣誉勋章14号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 7, 3000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (586, '荣誉勋章15号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 7, 3000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (587, '荣誉勋章21号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (588, '荣誉勋章22号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 15, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (589, '荣誉勋章23号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 15, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (590, '荣誉勋章24号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 15, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (591, '荣誉勋章25号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 15, 5000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (592, '荣誉勋章31号', 30, 0, 1, 0, 1, 0, 393, 50000, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (593, '荣誉勋章32号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 0, 1, 2, 0, 0, 0, 0, 0, 0, 0, 18, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (594, '荣誉勋章33号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 0, 0, 0, 1, 2, 0, 0, 0, 0, 0, 18, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (595, '荣誉勋章34号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 0, 0, 0, 0, 0, 1, 2, 0, 0, 0, 18, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (596, '荣誉勋章35号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 0, 18, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (597, '荣誉勋章41号', 30, 0, 1, 0, 1, 0, 393, 50000, 1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (598, '荣誉勋章42号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 0, 1, 3, 0, 0, 0, 0, 0, 0, 0, 25, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (599, '荣誉勋章43号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 0, 0, 0, 1, 3, 0, 0, 0, 0, 0, 25, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (600, '荣誉勋章44号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 0, 0, 0, 0, 0, 1, 3, 0, 0, 0, 25, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (601, '荣誉勋章45号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 0, 25, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (602, '荣誉勋章51号', 30, 0, 1, 0, 1, 0, 393, 50000, 1, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 28, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (603, '荣誉勋章52号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 0, 1, 4, 0, 0, 0, 0, 0, 0, 0, 28, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (604, '荣誉勋章53号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 0, 0, 0, 1, 4, 0, 0, 0, 0, 0, 28, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (605, '荣誉勋章54号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 0, 0, 0, 0, 0, 1, 4, 0, 0, 0, 28, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (606, '荣誉勋章55号', 30, 0, 1, 0, 1, 0, 393, 50000, 0, 0, 0, 0, 0, 0, 0, 0, 1, 4, 0, 28, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (607, '★经典套装★', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (608, '赤血魔剑', 5, 30, 22, 138, 0, 0, 66, 22000, 0, 1, 0, 0, 15, 10, 4, 3, 4, 2, 0, 35, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (609, '魔血戒指', 22, 133, 2, 25, 0, 0, 430, 4000, 0, 0, 0, 0, 1, 3, 0, 0, 1, 0, 0, 33, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (610, '魔血手镯', 26, 133, 2, 25, 0, 0, 429, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 33, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (611, '魔血项链', 20, 135, 2, 25, 0, 0, 428, 4000, 0, 0, 0, 0, 2, 2, 1, 3, 0, 0, 0, 33, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (612, '虹魔戒指', 22, 136, 2, 5, 0, 0, 433, 4000, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 33, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (613, '虹魔手镯', 26, 137, 2, 5, 0, 0, 434, 4000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 33, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (614, '虹魔项链', 20, 138, 2, 5, 0, 0, 432, 4000, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 33, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (615, '记忆戒指', 22, 122, 1, 0, 0, 0, 178, 7000, 0, 0, 0, 0, 0, 0, 0, 0, 1, 3, 0, 26, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (616, '记忆项链', 19, 123, 1, 0, 0, 0, 247, 8000, 0, 0, 0, 0, 2, 4, 0, 0, 0, 0, 0, 26, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (617, '记忆手镯', 24, 124, 1, 0, 0, 0, 212, 6000, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 26, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (618, '记忆头盔', 15, 125, 7, 0, 0, 0, 109, 8000, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (619, '祈祷之刃', 5, 23, 10, 0, 0, 8, 54, 20000, 0, 3, 0, 1, 8, 20, 0, 0, 0, 0, 0, 20, 30000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (620, '祈祷手镯', 26, 126, 1, 0, 0, 8, 213, 5000, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 15, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (621, '祈祷项链', 21, 127, 1, 0, 0, 8, 249, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 15, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (622, '祈祷戒指', 23, 128, 1, 0, 0, 8, 179, 5000, 0, 0, 0, 0, 0, 0, 1, 5, 0, 0, 0, 15, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (623, '祈祷头盔', 15, 129, 1, 0, 0, 8, 110, 5000, 3, 4, 1, 2, 0, 0, 0, 0, 0, 0, 0, 18, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (624, '神秘戒指', 22, 130, 1, 0, 0, 0, 181, 5000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 100, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (625, '神秘腰带', 26, 131, 1, 0, 0, 0, 215, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 100, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (626, '神秘头盔', 15, 132, 10, 0, 0, 0, 111, 10000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 100, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (627, '神水', 0, 2, 1, 0, 0, 0, 119, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (628, '圣战头盔', 15, 0, 20, 0, 0, 0, 104, 7000, 4, 5, 2, 3, 0, 1, 0, 0, 0, 0, 1, 40, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (629, '圣战项链', 20, 0, 2, 0, 0, 0, 229, 7000, 0, 0, 0, 0, 3, 6, 0, 0, 0, 0, 1, 40, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (630, '圣战手镯', 26, 0, 2, 0, 0, 0, 196, 7000, 0, 1, 0, 0, 2, 3, 0, 0, 0, 0, 1, 40, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (631, '圣战戒指', 22, 0, 2, 0, 0, 0, 147, 7000, 0, 0, 0, 1, 0, 7, 0, 0, 0, 0, 1, 40, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (632, '法神头盔', 15, 0, 2, 0, 0, 0, 101, 7000, 4, 4, 1, 2, 0, 0, 0, 1, 0, 0, 2, 28, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (633, '法神项链', 20, 0, 1, 0, 0, 0, 226, 7000, 1, 2, 0, 0, 0, 0, 1, 8, 0, 0, 2, 28, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (634, '法神手镯', 26, 0, 1, 0, 0, 0, 197, 7000, 0, 1, 0, 0, 0, 0, 0, 4, 0, 0, 2, 28, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (635, '法神戒指', 22, 0, 1, 0, 0, 0, 158, 7000, 0, 0, 0, 1, 0, 0, 1, 6, 0, 0, 2, 28, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (636, '天尊头盔', 15, 0, 3, 0, 0, 0, 102, 7000, 4, 4, 1, 2, 0, 0, 0, 0, 0, 1, 3, 25, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (637, '天尊项链', 20, 0, 1, 0, 0, 0, 234, 7000, 1, 2, 0, 0, 0, 0, 0, 0, 2, 7, 3, 25, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (638, '天尊手镯', 26, 0, 1, 0, 0, 0, 195, 7000, 1, 2, 0, 0, 0, 0, 0, 0, 1, 4, 3, 25, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (639, '天尊戒指', 22, 0, 1, 0, 0, 0, 157, 7000, 0, 0, 0, 1, 0, 0, 0, 0, 2, 7, 3, 25, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (640, '天之圣战头盔', 15, 0, 20, 0, 0, 0, 104, 7000, 5, 5, 2, 3, 0, 1, 0, 0, 0, 0, 5, 30, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (641, '天之圣战项链', 20, 0, 2, 0, 0, 0, 229, 7000, 0, 0, 0, 0, 4, 6, 0, 0, 0, 0, 5, 30, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (642, '天之圣战手镯', 26, 0, 2, 0, 0, 0, 196, 7000, 0, 2, 0, 0, 2, 3, 0, 0, 0, 0, 5, 30, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (643, '天之圣战戒指', 22, 0, 2, 0, 0, 0, 147, 7000, 0, 0, 0, 2, 0, 7, 0, 0, 0, 0, 5, 30, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (644, '天之天尊头盔', 15, 0, 3, 0, 0, 0, 102, 7000, 4, 5, 2, 3, 0, 0, 0, 0, 0, 1, 5, 30, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (645, '天之天尊项链', 20, 0, 1, 0, 0, 0, 234, 7000, 1, 3, 0, 0, 0, 0, 0, 0, 3, 7, 5, 30, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (646, '天之天尊手镯', 26, 0, 1, 0, 0, 0, 195, 7000, 1, 3, 0, 0, 0, 0, 0, 0, 1, 4, 5, 30, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (647, '天之天尊戒指', 22, 0, 1, 0, 0, 0, 157, 7000, 0, 0, 0, 2, 0, 0, 0, 0, 2, 7, 5, 30, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (648, '天之法神头盔', 15, 0, 2, 0, 0, 0, 101, 7000, 4, 5, 2, 3, 0, 0, 0, 1, 0, 0, 5, 30, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (649, '天之法神项链', 20, 0, 1, 0, 0, 0, 226, 7000, 1, 2, 0, 0, 0, 0, 2, 8, 0, 0, 5, 30, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (650, '天之法神手镯', 26, 0, 1, 0, 0, 0, 197, 7000, 0, 2, 0, 0, 0, 0, 0, 4, 0, 0, 5, 30, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (651, '天之法神戒指', 22, 0, 1, 0, 0, 0, 158, 7000, 0, 0, 0, 2, 0, 0, 1, 6, 0, 0, 5, 30, 60000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (652, '雷霆项链', 20, 0, 1, 0, 0, 0, 781, 7000, 0, 0, 0, 0, 3, 7, 0, 0, 0, 0, 1, 45, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (653, '雷霆护腕', 26, 0, 1, 0, 0, 0, 766, 7000, 0, 2, 0, 2, 1, 4, 0, 0, 0, 0, 1, 45, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (654, '雷霆战戒', 22, 0, 1, 0, 0, 0, 773, 7000, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 1, 45, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (655, '雷霆腰带', 54, 0, 2, 0, 0, 0, 761, 7000, 2, 3, 0, 1, 1, 0, 0, 0, 0, 0, 1, 45, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (656, '雷霆战靴', 52, 0, 2, 3, 0, 0, 752, 7000, 0, 3, 0, 0, 1, 0, 0, 0, 0, 0, 1, 45, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (657, '光芒项链', 20, 0, 1, 0, 0, 0, 779, 6000, 0, 3, 0, 0, 0, 0, 0, 0, 2, 8, 3, 28, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (658, '光芒护腕', 26, 0, 1, 0, 0, 0, 764, 6000, 1, 3, 0, 0, 0, 0, 0, 0, 2, 4, 3, 28, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (659, '光芒道戒', 22, 0, 1, 0, 0, 0, 772, 6000, 0, 2, 0, 0, 0, 0, 0, 0, 1, 8, 3, 28, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (660, '光芒腰带', 54, 0, 2, 0, 0, 0, 760, 6000, 2, 2, 0, 1, 0, 0, 0, 0, 1, 0, 3, 28, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (661, '光芒道靴', 52, 0, 1, 4, 0, 0, 751, 6000, 0, 3, 0, 0, 0, 0, 0, 0, 1, 0, 3, 28, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (662, '烈焰项链', 20, 0, 1, 0, 0, 0, 775, 5000, 0, 3, 0, 0, 0, 0, 1, 9, 0, 0, 2, 30, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (663, '烈焰护腕', 26, 0, 1, 0, 0, 0, 762, 6000, 0, 2, 0, 0, 0, 0, 2, 4, 0, 0, 2, 30, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (664, '烈焰魔戒', 22, 0, 1, 0, 0, 0, 771, 5000, 0, 1, 0, 0, 0, 0, 1, 7, 0, 0, 2, 30, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (665, '烈焰腰带', 54, 0, 2, 0, 0, 0, 759, 5000, 2, 2, 0, 1, 0, 0, 1, 0, 0, 0, 2, 30, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (666, '烈焰魔靴', 52, 0, 1, 5, 0, 0, 750, 5000, 0, 3, 0, 0, 0, 0, 1, 0, 0, 0, 2, 30, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (667, '战神项链', 19, 0, 3, 0, 0, 0, 682, 8000, 0, 0, 0, 1, 3, 8, 0, 0, 0, 0, 1, 54, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (668, '战神手镯', 26, 0, 2, 0, 0, 0, 676, 8000, 0, 1, 0, 0, 1, 5, 0, 0, 0, 0, 1, 54, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (669, '战神戒指', 22, 0, 2, 0, 0, 0, 670, 8000, 0, 0, 0, 2, 1, 9, 0, 0, 0, 0, 1, 54, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (670, '真魂项链', 19, 0, 2, 0, 0, 0, 684, 7000, 0, 2, 0, 0, 0, 0, 0, 0, 3, 10, 3, 30, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (671, '真魂手镯', 24, 0, 1, 0, 0, 0, 680, 7000, 0, 1, 0, 2, 0, 0, 0, 0, 2, 5, 3, 30, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (672, '真魂戒指', 22, 0, 1, 0, 0, 0, 674, 7000, 0, 0, 0, 2, 0, 0, 0, 0, 1, 9, 3, 30, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (673, '圣魔项链', 19, 0, 1, 0, 0, 0, 683, 6000, 0, 3, 0, 0, 0, 0, 3, 10, 0, 0, 2, 32, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (674, '圣魔手镯', 26, 0, 1, 0, 0, 0, 678, 6000, 0, 2, 0, 2, 0, 0, 2, 5, 0, 0, 2, 32, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (675, '圣魔戒指', 22, 0, 1, 0, 0, 0, 672, 6000, 0, 0, 0, 2, 0, 0, 1, 8, 0, 0, 2, 32, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (676, '誓言腰带(战)', 54, 0, 2, 0, 0, 0, 556, 10000, 2, 3, 2, 3, 0, 2, 0, 0, 0, 0, 0, 45, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (677, '誓言腰带(法)', 54, 0, 2, 0, 0, 0, 556, 10000, 2, 3, 2, 3, 0, 0, 0, 2, 0, 0, 0, 45, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (678, '誓言腰带(道)', 54, 0, 2, 0, 0, 0, 556, 8000, 2, 3, 2, 3, 0, 0, 0, 0, 0, 2, 0, 45, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (679, '传说魔靴(战)', 52, 0, 2, 5, 0, 0, 564, 8000, 2, 3, 2, 3, 0, 2, 0, 0, 0, 0, 0, 45, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (680, '传说魔靴(法)', 52, 0, 1, 7, 0, 0, 564, 5000, 2, 3, 2, 3, 0, 0, 0, 2, 0, 0, 0, 45, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (681, '传说魔靴(道)', 52, 0, 1, 6, 0, 0, 564, 6000, 2, 3, 2, 3, 0, 0, 0, 0, 0, 2, 0, 45, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (682, '预言头盔(战)', 15, 0, 20, 0, 0, 0, 107, 7000, 2, 3, 5, 5, 0, 2, 0, 0, 0, 0, 0, 45, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (683, '预言头盔(法)', 15, 0, 2, 0, 0, 0, 107, 7000, 2, 3, 5, 5, 0, 0, 0, 2, 0, 0, 0, 45, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (684, '预言头盔(道)', 15, 0, 3, 0, 0, 0, 107, 7000, 2, 3, 5, 5, 0, 0, 0, 0, 0, 2, 0, 45, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (685, '强化雷霆项链', 20, 0, 1, 0, 0, 0, 776, 8000, 0, 0, 0, 0, 3, 8, 0, 0, 0, 0, 1, 45, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (686, '强化雷霆护腕', 26, 0, 1, 0, 0, 0, 767, 7000, 0, 2, 0, 2, 1, 5, 0, 0, 0, 0, 1, 45, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (687, '强化雷霆战戒', 22, 0, 1, 0, 0, 0, 769, 8000, 0, 0, 0, 0, 0, 9, 0, 0, 0, 0, 1, 45, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (688, '强化雷霆腰带', 54, 0, 2, 0, 0, 0, 758, 8000, 2, 3, 0, 1, 1, 1, 0, 0, 0, 0, 1, 45, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (689, '强化雷霆战靴', 52, 0, 2, 3, 0, 0, 755, 8000, 0, 3, 0, 0, 1, 1, 0, 0, 0, 0, 1, 45, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (690, '强化光芒项链', 20, 0, 1, 0, 0, 0, 777, 6000, 1, 4, 0, 0, 0, 0, 0, 0, 3, 9, 3, 28, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (691, '强化光芒护腕', 26, 0, 1, 0, 0, 0, 765, 6000, 0, 3, 0, 0, 0, 0, 0, 0, 2, 5, 3, 28, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (692, '强化光芒道戒', 22, 0, 1, 0, 0, 0, 768, 6000, 1, 2, 0, 0, 0, 0, 0, 0, 1, 9, 3, 28, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (693, '强化光芒腰带', 54, 0, 2, 0, 0, 0, 756, 6000, 2, 2, 0, 1, 0, 0, 0, 0, 1, 1, 3, 28, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (694, '强化光芒道靴', 52, 0, 1, 4, 0, 0, 754, 6000, 0, 3, 0, 0, 0, 0, 0, 0, 1, 1, 3, 28, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (695, '强化烈焰项链', 20, 0, 1, 0, 0, 0, 778, 5000, 1, 3, 0, 1, 0, 0, 2, 10, 0, 0, 2, 30, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (696, '强化烈焰护腕', 26, 0, 1, 0, 0, 0, 763, 6000, 0, 2, 0, 0, 0, 0, 2, 5, 0, 0, 2, 30, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (697, '强化烈焰魔戒', 22, 0, 1, 0, 0, 0, 770, 6000, 0, 2, 0, 0, 0, 0, 1, 8, 0, 0, 2, 30, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (698, '强化烈焰腰带', 54, 0, 2, 0, 0, 0, 757, 7000, 2, 2, 0, 1, 0, 0, 1, 1, 0, 0, 2, 30, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (699, '强化烈焰魔靴', 52, 0, 1, 5, 0, 0, 753, 5000, 0, 3, 0, 0, 0, 0, 1, 1, 0, 0, 2, 30, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (700, '★终极套装★', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (701, '奔雷项链', 20, 0, 3, 0, 0, 0, 1152, 10000, 0, 3, 0, 0, 3, 8, 0, 0, 0, 0, 0, 42, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (702, '奔雷护腕', 26, 0, 1, 0, 0, 0, 1151, 10000, 0, 1, 1, 4, 0, 4, 0, 0, 0, 0, 0, 42, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (703, '奔雷战戒', 22, 0, 2, 0, 0, 0, 1150, 10000, 0, 1, 0, 0, 2, 8, 0, 0, 0, 0, 0, 42, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (704, '极光项链', 19, 0, 2, 0, 0, 0, 1158, 10000, 0, 2, 0, 0, 0, 0, 0, 0, 1, 8, 0, 42, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (705, '极光护腕', 24, 0, 1, 0, 0, 0, 1157, 10000, 0, 2, 0, 0, 0, 0, 0, 0, 1, 5, 0, 42, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (706, '极光道戒', 22, 0, 1, 0, 0, 0, 1156, 10000, 0, 1, 0, 0, 0, 0, 0, 0, 2, 9, 0, 42, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (707, '怒焰项链', 20, 0, 3, 0, 0, 0, 1155, 10000, 0, 3, 0, 0, 0, 0, 0, 9, 0, 0, 0, 42, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (708, '怒焰护腕', 24, 0, 1, 0, 0, 0, 1154, 10000, 0, 2, 0, 2, 0, 0, 2, 4, 0, 0, 0, 42, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (709, '怒焰魔戒', 22, 0, 1, 0, 0, 0, 1153, 10000, 0, 1, 0, 0, 0, 0, 2, 8, 0, 0, 0, 42, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (710, '狂战手镯', 26, 0, 2, 0, 0, 0, 677, 12000, 0, 2, 0, 0, 1, 5, 0, 0, 0, 0, 0, 43, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (711, '太极手镯', 24, 0, 1, 0, 0, 0, 681, 10000, 0, 2, 0, 2, 0, 0, 0, 1, 0, 5, 0, 43, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (712, '混世手镯', 24, 0, 1, 0, 0, 0, 679, 10000, 0, 2, 0, 2, 0, 0, 1, 5, 0, 0, 0, 43, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (713, '狂战戒指', 22, 0, 2, 0, 0, 0, 671, 10000, 0, 2, 0, 3, 1, 8, 0, 0, 0, 0, 0, 43, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (714, '太极戒指', 22, 0, 1, 0, 0, 0, 675, 10000, 0, 2, 0, 3, 0, 0, 0, 0, 1, 9, 0, 43, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (715, '混世戒指', 22, 0, 1, 0, 0, 0, 673, 10000, 0, 2, 0, 3, 0, 0, 1, 8, 0, 0, 0, 43, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (716, '狂战手镯(煅)', 26, 0, 2, 0, 0, 0, 677, 12000, 3, 6, 3, 6, 3, 5, 0, 0, 0, 0, 0, 45, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (717, '混世手镯(煅)', 26, 0, 1, 0, 0, 0, 679, 10000, 2, 5, 2, 5, 0, 0, 5, 5, 0, 0, 0, 45, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (718, '太极手镯(煅)', 24, 0, 1, 0, 0, 0, 681, 10000, 0, 4, 0, 5, 0, 0, 0, 0, 5, 5, 0, 45, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (719, '狂战戒指(煅)', 22, 0, 2, 0, 0, 0, 671, 10000, 0, 0, 2, 5, 4, 9, 0, 0, 0, 0, 0, 45, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (720, '太极戒指(煅)', 22, 0, 1, 0, 0, 0, 675, 10000, 2, 6, 0, 0, 0, 0, 0, 0, 4, 9, 0, 45, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (721, '混世戒指(煅)', 22, 0, 1, 0, 0, 0, 673, 10000, 2, 5, 0, 0, 0, 0, 4, 9, 0, 0, 0, 45, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (722, '星王战盔', 15, 0, 1, 0, 0, 0, 105, 10000, 4, 6, 4, 5, 0, 3, 0, 0, 0, 0, 0, 47, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (723, '星王道盔', 15, 0, 1, 0, 0, 0, 105, 10000, 4, 6, 4, 5, 0, 0, 0, 0, 0, 3, 0, 47, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (724, '星王法冠', 15, 0, 1, 0, 0, 0, 105, 10000, 4, 6, 4, 5, 0, 0, 0, 3, 0, 0, 0, 47, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (725, '星王项链(战)', 19, 0, 3, 0, 0, 0, 1223, 8000, 0, 0, 0, 1, 3, 9, 0, 0, 0, 0, 0, 47, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (726, '星王项链(道)', 19, 0, 3, 0, 0, 0, 1223, 8000, 0, 2, 0, 0, 0, 0, 0, 0, 1, 9, 0, 47, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (727, '星王项链(法)', 20, 0, 3, 0, 0, 0, 1223, 6000, 0, 0, 0, 3, 0, 0, 0, 10, 0, 0, 0, 47, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (728, '星王护腕(战)', 26, 0, 1, 0, 0, 0, 1222, 10000, 0, 1, 0, 0, 1, 6, 0, 0, 0, 0, 0, 47, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (729, '星王护腕(道)', 24, 0, 1, 0, 0, 0, 1222, 10000, 0, 0, 0, 2, 0, 0, 0, 0, 1, 6, 0, 47, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (730, '星王护腕(法)', 26, 0, 1, 0, 0, 0, 1222, 10000, 0, 2, 0, 2, 0, 0, 1, 6, 0, 0, 0, 47, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (731, '星王战戒', 22, 0, 2, 0, 0, 0, 1221, 8000, 0, 0, 0, 1, 0, 10, 0, 0, 0, 0, 0, 47, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (732, '星王道戒', 22, 0, 2, 0, 0, 0, 1221, 8000, 0, 0, 0, 1, 0, 0, 0, 0, 2, 10, 0, 47, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (733, '星王法戒', 22, 0, 2, 0, 0, 0, 1221, 6000, 0, 0, 0, 1, 0, 0, 2, 9, 0, 0, 0, 47, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (734, '星王腰带(战)', 54, 0, 2, 0, 0, 0, 555, 10000, 0, 2, 0, 2, 0, 3, 0, 0, 0, 0, 0, 47, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (735, '星王腰带(道)', 54, 0, 2, 0, 0, 0, 555, 10000, 0, 2, 0, 2, 0, 0, 0, 0, 0, 3, 0, 47, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (736, '星王腰带(法)', 54, 0, 2, 0, 0, 0, 555, 10000, 0, 2, 0, 2, 0, 0, 0, 3, 0, 0, 0, 47, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (737, '星王战靴', 52, 0, 2, 5, 0, 0, 565, 10000, 0, 2, 0, 2, 0, 3, 0, 0, 0, 0, 0, 47, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (738, '星王道靴', 52, 0, 1, 6, 0, 0, 565, 10000, 0, 2, 0, 2, 0, 0, 0, 0, 0, 3, 0, 47, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (739, '星王法靴', 52, 0, 1, 7, 0, 0, 565, 10000, 0, 2, 0, 2, 0, 0, 0, 3, 0, 0, 0, 47, 90000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (740, '狂雷战盔', 15, 188, 20, 0, 1, 0, 1265, 8000, 4, 6, 4, 5, 1, 4, 0, 0, 0, 0, 0, 50, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (741, '狂雷项链', 19, 188, 1, 0, 1, 0, 1262, 8000, 0, 0, 0, 1, 3, 9, 0, 0, 0, 0, 0, 50, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (742, '狂雷护腕', 26, 188, 1, 0, 1, 0, 1261, 8000, 0, 1, 0, 0, 1, 6, 0, 0, 0, 0, 0, 50, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (743, '狂雷战戒', 22, 188, 1, 0, 1, 0, 1260, 8000, 0, 0, 0, 1, 0, 10, 0, 0, 0, 0, 0, 50, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (744, '狂雷腰带', 54, 188, 2, 0, 1, 0, 1264, 8000, 3, 4, 3, 4, 1, 3, 0, 0, 0, 0, 0, 50, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (745, '狂雷战靴', 52, 188, 2, 6, 1, 0, 1263, 8000, 3, 4, 3, 4, 1, 3, 0, 0, 0, 0, 0, 50, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (746, '通云道盔', 15, 188, 3, 0, 1, 0, 1255, 8000, 4, 6, 4, 5, 0, 0, 0, 0, 1, 4, 0, 50, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (747, '通云项链', 19, 188, 1, 0, 1, 0, 1252, 8000, 0, 2, 0, 0, 0, 0, 0, 0, 1, 9, 0, 50, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (748, '通云护腕', 24, 188, 1, 0, 1, 0, 1251, 8000, 0, 0, 0, 2, 0, 0, 0, 0, 1, 6, 0, 50, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (749, '通云道戒', 22, 188, 1, 0, 1, 0, 1250, 8000, 0, 0, 0, 1, 0, 0, 0, 0, 2, 10, 0, 50, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (750, '通云腰带', 54, 188, 1, 0, 1, 0, 1254, 8000, 3, 4, 3, 4, 0, 0, 0, 0, 1, 3, 0, 50, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (751, '通云道靴', 52, 188, 1, 6, 1, 0, 1253, 8000, 3, 4, 3, 4, 0, 0, 0, 0, 1, 3, 0, 50, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (752, '逆火魔盔', 15, 188, 2, 0, 1, 0, 1245, 8000, 4, 6, 4, 5, 0, 0, 1, 4, 0, 0, 0, 50, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (753, '逆火项链', 20, 188, 1, 0, 1, 0, 1242, 8000, 0, 0, 0, 3, 0, 0, 0, 10, 0, 0, 0, 50, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (754, '逆火护腕', 26, 188, 1, 0, 1, 0, 1241, 8000, 0, 2, 0, 2, 0, 0, 1, 6, 0, 0, 0, 50, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (755, '逆火魔戒', 22, 188, 1, 0, 1, 0, 1240, 8000, 0, 0, 0, 1, 0, 0, 2, 9, 0, 0, 0, 50, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (756, '逆火腰带', 54, 188, 1, 0, 1, 0, 1244, 8000, 3, 4, 3, 4, 0, 0, 1, 3, 0, 0, 0, 50, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (757, '逆火魔靴', 52, 188, 1, 6, 1, 0, 1243, 8000, 3, 4, 3, 4, 0, 0, 1, 3, 0, 0, 0, 50, 100000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (758, '王者战盔', 15, 188, 30, 1, 2, 0, 1369, 9000, 5, 7, 4, 6, 1, 5, 0, 0, 0, 0, 0, 52, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (759, '王者道盔', 15, 188, 8, 2, 2, 0, 1369, 9000, 5, 7, 4, 6, 0, 0, 0, 0, 1, 5, 0, 52, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (760, '王者魔盔', 15, 188, 10, 3, 2, 0, 1369, 9000, 5, 7, 4, 6, 0, 0, 1, 5, 0, 0, 0, 52, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (761, '王者项链(战)', 19, 188, 3, 0, 2, 0, 1366, 8000, 0, 0, 0, 2, 3, 10, 0, 0, 0, 0, 0, 52, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (762, '王者项链(道)', 19, 188, 2, 0, 2, 0, 1366, 7000, 0, 0, 0, 2, 0, 0, 0, 0, 1, 10, 0, 52, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (763, '王者项链(法)', 19, 188, 1, 0, 2, 0, 1366, 6000, 0, 0, 0, 2, 0, 0, 0, 11, 0, 0, 0, 52, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (764, '王者护腕(战)', 26, 188, 2, 0, 2, 0, 1365, 8000, 0, 2, 0, 0, 1, 7, 0, 0, 0, 0, 0, 52, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (765, '王者护腕(道)', 24, 188, 1, 0, 2, 0, 1365, 7000, 0, 0, 0, 2, 0, 0, 0, 0, 1, 7, 0, 52, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (766, '王者护腕(法)', 26, 188, 1, 0, 2, 0, 1365, 6000, 0, 2, 0, 2, 0, 0, 1, 7, 0, 0, 0, 52, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (767, '王者战戒', 22, 188, 2, 0, 2, 0, 1364, 8000, 0, 0, 0, 2, 0, 11, 0, 0, 0, 0, 0, 52, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (768, '王者道戒', 22, 188, 1, 0, 2, 0, 1364, 7000, 0, 0, 0, 2, 0, 0, 0, 0, 2, 11, 0, 52, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (769, '王者魔戒', 22, 188, 1, 0, 2, 0, 1364, 6000, 0, 0, 0, 2, 0, 0, 2, 10, 0, 0, 0, 52, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (770, '王者腰带(战)', 54, 188, 1, 0, 2, 0, 1368, 8000, 3, 5, 3, 5, 2, 3, 0, 0, 0, 0, 0, 52, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (771, '王者腰带(道)', 54, 188, 1, 0, 2, 0, 1368, 8000, 3, 5, 3, 5, 0, 0, 0, 0, 2, 3, 0, 52, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (772, '王者腰带(法)', 54, 188, 1, 0, 2, 0, 1368, 8000, 3, 5, 3, 5, 0, 0, 2, 3, 0, 0, 0, 52, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (773, '王者战靴', 52, 188, 2, 6, 2, 0, 1367, 8000, 3, 5, 3, 5, 2, 3, 0, 0, 0, 0, 0, 52, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (774, '王者道靴', 52, 188, 2, 6, 2, 0, 1367, 8000, 3, 5, 3, 5, 0, 0, 0, 0, 2, 3, 0, 52, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (775, '王者魔靴', 52, 188, 2, 6, 2, 0, 1367, 8000, 3, 5, 3, 5, 0, 0, 2, 3, 0, 0, 0, 52, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (776, '主宰之冠', 15, 188, 1, 0, 3, 0, 2415, 10000, 5, 8, 4, 7, 1, 6, 1, 6, 1, 6, 0, 55, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (777, '主宰斗笠', 16, 5, 1, 0, 0, 0, 2422, 6000, 2, 3, 2, 3, 1, 2, 1, 2, 1, 2, 0, 55, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (778, '主宰面巾', 16, 2, 1, 1, 0, 0, 1714, 8000, 1, 2, 1, 2, 0, 1, 0, 1, 0, 1, 0, 55, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (779, '主宰项链', 19, 188, 1, 0, 3, 0, 2412, 8000, 0, 0, 0, 2, 3, 11, 0, 12, 1, 11, 0, 55, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (780, '主宰护腕', 26, 188, 1, 0, 3, 0, 2411, 8000, 0, 2, 0, 2, 1, 8, 1, 8, 1, 8, 0, 55, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (781, '主宰之戒', 22, 188, 1, 0, 3, 0, 2410, 8000, 0, 0, 0, 2, 0, 12, 2, 11, 2, 12, 0, 55, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (782, '主宰腰带', 54, 188, 1, 0, 3, 0, 2414, 8000, 3, 6, 3, 6, 2, 4, 2, 4, 2, 4, 0, 55, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (783, '主宰之靴', 52, 188, 1, 8, 3, 0, 2413, 10000, 3, 6, 3, 6, 2, 4, 2, 4, 2, 4, 0, 55, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (784, '主宰勋章', 30, 188, 1, 0, 3, 0, 2416, 50000, 2, 7, 2, 7, 2, 7, 2, 7, 2, 7, 0, 55, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (785, '追风戒指', 23, 188, 3, 0, 3, 0, 1060, 7000, 1, 0, 0, 0, 0, 13, 0, 0, 0, 0, 0, 56, 130000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (786, '追风手镯', 24, 188, 3, 0, 3, 0, 1069, 7000, 0, 3, 0, 3, 1, 9, 0, 0, 0, 0, 0, 56, 130000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (787, '追风项链', 21, 188, 3, 0, 3, 0, 1075, 7000, 1, 0, 0, 0, 3, 12, 0, 0, 0, 0, 0, 56, 130000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (788, '追魂戒指', 22, 188, 2, 0, 3, 0, 1062, 7000, 0, 4, 0, 4, 0, 0, 0, 0, 2, 13, 0, 56, 130000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (789, '追魂手镯', 24, 188, 2, 0, 3, 0, 1071, 7000, 0, 3, 0, 3, 0, 0, 0, 0, 1, 9, 0, 56, 130000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (790, '追魂项链', 21, 188, 2, 0, 3, 0, 1077, 7000, 0, 4, 0, 0, 0, 0, 0, 0, 2, 12, 0, 56, 130000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (791, '追魔戒指', 23, 188, 1, 0, 3, 0, 1061, 7000, 0, 3, 0, 3, 0, 0, 2, 12, 0, 0, 0, 56, 130000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (792, '追魔手镯', 24, 188, 1, 0, 3, 0, 1070, 7000, 0, 3, 0, 3, 0, 0, 1, 9, 0, 0, 0, 56, 130000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (793, '追魔项链', 21, 188, 1, 0, 3, 0, 1076, 7000, 0, 0, 0, 4, 0, 0, 0, 13, 0, 0, 0, 56, 130000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (794, '龙鳞戒指', 22, 188, 5, 0, 3, 0, 1063, 8000, 0, 0, 0, 3, 0, 14, 0, 0, 0, 0, 18, 58, 130000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (795, '龙鳞手镯', 26, 188, 5, 0, 3, 0, 1066, 8000, 0, 4, 0, 0, 1, 10, 0, 0, 0, 0, 18, 58, 130000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (796, '龙鳞项链', 20, 188, 5, 0, 3, 0, 1072, 8000, 0, 0, 0, 3, 3, 13, 0, 0, 0, 0, 18, 58, 130000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (797, '龙鳞头盔', 15, 188, 10, 0, 3, 0, 1114, 8000, 6, 9, 4, 7, 2, 7, 0, 0, 0, 0, 18, 58, 130000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (798, '天极戒指', 22, 188, 3, 0, 3, 0, 1065, 8000, 0, 0, 0, 3, 0, 0, 0, 0, 2, 14, 18, 58, 130000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (799, '天极手镯', 26, 188, 3, 0, 3, 0, 1068, 8000, 0, 2, 0, 2, 0, 0, 0, 0, 1, 10, 18, 58, 130000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (800, '天极项链', 20, 188, 3, 0, 3, 0, 1074, 8000, 0, 0, 0, 3, 0, 0, 0, 0, 3, 13, 18, 58, 130000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (801, '天极头盔', 15, 188, 8, 0, 3, 0, 1116, 8000, 6, 9, 5, 8, 0, 0, 0, 0, 2, 7, 18, 58, 130000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (802, '袁灵戒指', 22, 188, 2, 0, 3, 0, 1064, 8000, 0, 0, 0, 3, 0, 0, 3, 13, 0, 0, 18, 58, 130000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (803, '袁灵手镯', 26, 188, 2, 0, 3, 0, 1067, 8000, 0, 0, 0, 4, 0, 0, 2, 10, 0, 0, 18, 58, 130000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (804, '袁灵项链', 20, 188, 2, 0, 3, 0, 1073, 8000, 0, 0, 0, 3, 0, 0, 1, 14, 0, 0, 18, 58, 130000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (805, '袁灵头盔', 15, 188, 5, 0, 3, 0, 1115, 8000, 5, 8, 6, 9, 0, 0, 2, 7, 0, 0, 18, 58, 130000, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (806, '破坏戒指', 22, 188, 10, 0, 3, 0, 1169, 10000, 0, 0, 0, 0, 2, 19, 0, 0, 0, 0, 0, 50, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (807, '破坏手镯', 26, 188, 10, 0, 3, 0, 1170, 10000, 0, 0, 0, 0, 3, 15, 0, 0, 0, 0, 0, 50, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (808, '破坏项链', 20, 188, 10, 0, 3, 0, 1171, 10000, 0, 0, 0, 0, 5, 16, 0, 0, 0, 0, 0, 50, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (809, '天鸣戒指', 22, 188, 3, 0, 5, 0, 1208, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 3, 19, 0, 50, 130000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (810, '天鸣手镯', 26, 188, 5, 0, 5, 0, 1209, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 5, 15, 0, 50, 130000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (811, '天鸣项链', 20, 188, 1, 0, 5, 0, 1210, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 8, 17, 0, 50, 130000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (812, '天鸣靴子', 52, 188, 2, 10, 5, 0, 1211, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 2, 8, 0, 50, 130000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (813, '天鸣腰带', 54, 188, 2, 10, 5, 0, 1212, 8000, 0, 0, 0, 0, 0, 0, 0, 0, 2, 8, 0, 50, 130000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (814, '赤龙戒指', 22, 188, 1, 0, 3, 0, 1213, 8000, 0, 0, 0, 0, 0, 0, 4, 16, 0, 0, 0, 50, 130000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (815, '赤龙护腕', 26, 188, 2, 0, 3, 0, 1214, 8000, 0, 0, 0, 0, 0, 0, 3, 12, 0, 0, 0, 50, 130000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (816, '赤龙项链', 20, 188, 1, 0, 3, 0, 1215, 8000, 0, 0, 0, 0, 0, 0, 3, 16, 0, 0, 0, 50, 130000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (817, '赤龙靴子', 52, 188, 5, 10, 3, 0, 1216, 8000, 0, 0, 0, 0, 0, 0, 3, 8, 0, 0, 0, 50, 130000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (818, '赤龙腰带', 54, 188, 3, 0, 3, 0, 1217, 8000, 0, 0, 0, 0, 0, 0, 3, 8, 0, 0, 0, 60, 130000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (819, '疾风戒指', 23, 188, 3, 0, 3, 0, 1193, 7000, 1, 0, 0, 0, 1, 15, 0, 0, 0, 0, 18, 60, 130000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (820, '疾风手镯', 24, 188, 3, 0, 3, 0, 1194, 7000, 0, 4, 0, 4, 2, 11, 0, 0, 0, 0, 18, 60, 130000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (821, '疾风项链', 21, 188, 3, 0, 3, 0, 1195, 7000, 2, 0, 0, 0, 4, 13, 0, 0, 0, 0, 18, 60, 130000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (822, '疾风战靴', 52, 188, 5, 8, 3, 0, 1196, 7000, 4, 7, 4, 7, 1, 3, 0, 0, 0, 0, 18, 60, 130000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (823, '疾风腰带', 54, 188, 5, 0, 3, 0, 1197, 7000, 4, 7, 4, 7, 1, 3, 0, 0, 0, 0, 18, 60, 130000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (824, '神合戒指', 22, 188, 2, 0, 3, 0, 1198, 7000, 0, 6, 0, 6, 0, 0, 0, 0, 3, 15, 18, 60, 130000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (825, '神合手镯', 24, 188, 2, 0, 3, 0, 1199, 7000, 0, 4, 0, 4, 0, 0, 0, 0, 2, 12, 18, 60, 130000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (826, '神合项链', 19, 188, 2, 0, 3, 0, 1200, 7000, 0, 5, 0, 0, 0, 0, 0, 0, 4, 15, 18, 60, 130000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (827, '神合道靴', 52, 188, 5, 8, 3, 0, 1201, 7000, 4, 7, 4, 7, 0, 0, 0, 0, 1, 3, 18, 60, 130000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (828, '神合腰带', 54, 188, 5, 0, 3, 0, 1202, 7000, 4, 7, 4, 7, 0, 0, 0, 0, 1, 3, 18, 60, 130000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (829, '怒血戒指', 23, 188, 2, 0, 3, 0, 1203, 7000, 0, 2, 0, 2, 0, 0, 4, 14, 0, 0, 18, 60, 130000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (830, '怒血手镯', 24, 188, 2, 0, 3, 0, 1204, 7000, 0, 3, 0, 3, 0, 0, 3, 11, 0, 0, 18, 60, 130000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (831, '怒血项链', 19, 188, 2, 0, 3, 0, 1205, 7000, 0, 0, 0, 5, 0, 0, 2, 15, 0, 0, 18, 60, 130000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (832, '怒血魔靴', 52, 188, 3, 8, 3, 0, 1206, 7000, 4, 7, 4, 7, 0, 0, 1, 3, 0, 0, 18, 60, 130000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (833, '怒血腰带', 54, 188, 3, 0, 3, 0, 1207, 7000, 4, 7, 4, 7, 0, 0, 1, 3, 0, 0, 18, 60, 130000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (834, '帝王戒指', 22, 188, 3, 0, 4, 0, 1790, 8000, 0, 0, 0, 4, 2, 16, 0, 0, 0, 0, 18, 63, 150000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (835, '帝王护腕', 26, 188, 3, 0, 4, 0, 1791, 8000, 0, 5, 0, 0, 3, 12, 0, 0, 0, 0, 18, 63, 150000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (836, '帝王项链', 20, 188, 3, 0, 4, 0, 1794, 8000, 0, 0, 0, 4, 5, 14, 0, 0, 0, 0, 18, 63, 150000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (837, '帝王头盔', 15, 188, 10, 0, 4, 0, 1795, 8000, 7, 10, 5, 8, 2, 8, 0, 0, 0, 0, 18, 63, 150000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (838, '帝王腰带', 54, 188, 5, 0, 4, 0, 1792, 8000, 4, 7, 4, 7, 3, 5, 0, 0, 0, 0, 18, 63, 150000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (839, '帝王靴子', 52, 188, 5, 10, 4, 0, 1793, 8000, 4, 7, 4, 7, 3, 5, 0, 0, 0, 0, 18, 63, 150000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (840, '玄灵戒指', 22, 188, 2, 0, 4, 0, 1810, 8000, 0, 0, 0, 4, 0, 0, 0, 0, 4, 17, 18, 63, 150000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (841, '玄灵护腕', 26, 188, 2, 0, 4, 0, 1811, 8000, 0, 3, 0, 2, 0, 0, 0, 0, 3, 13, 18, 63, 150000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (842, '玄灵项链', 20, 188, 2, 0, 4, 0, 1814, 8000, 0, 0, 0, 4, 0, 0, 0, 0, 5, 16, 18, 63, 150000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (843, '玄灵头盔', 15, 188, 8, 0, 4, 0, 1815, 8000, 7, 10, 6, 9, 0, 0, 0, 0, 2, 8, 18, 63, 150000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (844, '玄灵腰带', 54, 188, 5, 0, 4, 0, 1812, 8000, 4, 7, 4, 7, 0, 0, 0, 0, 3, 5, 18, 63, 150000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (845, '玄灵靴子', 52, 188, 5, 10, 4, 0, 1813, 8000, 4, 7, 4, 7, 0, 0, 0, 0, 3, 5, 18, 63, 150000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (846, '心魔戒指', 22, 188, 2, 0, 4, 0, 1800, 8000, 0, 0, 0, 4, 0, 0, 5, 15, 0, 0, 18, 63, 150000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (847, '心魔护腕', 26, 188, 2, 0, 4, 0, 1801, 8000, 0, 4, 0, 4, 0, 0, 4, 12, 0, 0, 18, 63, 150000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (848, '心魔项链', 20, 188, 2, 0, 4, 0, 1804, 8000, 0, 0, 0, 4, 0, 0, 3, 16, 0, 0, 18, 63, 150000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (849, '心魔头盔', 15, 188, 5, 0, 4, 0, 1805, 8000, 6, 9, 6, 9, 0, 0, 2, 8, 0, 0, 18, 63, 150000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (850, '心魔腰带', 54, 188, 3, 0, 4, 0, 1802, 8000, 4, 7, 4, 7, 0, 0, 3, 5, 0, 0, 18, 63, 150000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (851, '心魔靴子', 52, 188, 3, 10, 4, 0, 1803, 8000, 4, 7, 4, 7, 0, 0, 3, 5, 0, 0, 18, 63, 150000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (852, '破荒戒指', 22, 188, 3, 0, 5, 0, 1840, 8000, 0, 3, 0, 3, 3, 17, 0, 0, 0, 0, 18, 65, 160000, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (853, '破荒护腕', 24, 188, 3, 0, 5, 0, 1841, 8000, 0, 5, 0, 5, 4, 13, 0, 0, 0, 0, 18, 65, 160000, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (854, '破荒项链', 20, 188, 3, 0, 5, 0, 1842, 8000, 0, 0, 0, 5, 6, 15, 0, 0, 0, 0, 18, 65, 160000, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (855, '破荒头盔', 15, 188, 10, 0, 5, 0, 1843, 8000, 8, 12, 6, 10, 3, 9, 0, 0, 0, 0, 18, 65, 160000, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (856, '破荒腰带', 54, 188, 5, 0, 5, 0, 1845, 8000, 5, 8, 5, 8, 4, 6, 0, 0, 0, 0, 18, 65, 160000, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (857, '破荒靴子', 52, 188, 5, 12, 5, 0, 1844, 8000, 5, 8, 5, 8, 4, 6, 0, 0, 0, 0, 18, 65, 160000, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (858, '破荒勋章', 30, 188, 1, 0, 1, 0, 1846, 50000, 2, 8, 0, 0, 2, 8, 0, 0, 0, 0, 18, 65, 160000, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (859, '天机戒指', 22, 188, 2, 0, 5, 0, 1850, 8000, 0, 3, 0, 3, 0, 0, 0, 0, 5, 18, 18, 65, 160000, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (860, '天机护腕', 24, 188, 2, 0, 5, 0, 1851, 8000, 0, 4, 0, 4, 0, 0, 0, 0, 4, 14, 18, 65, 160000, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (861, '天机项链', 20, 188, 2, 0, 5, 0, 1852, 8000, 0, 0, 0, 4, 0, 0, 0, 0, 6, 17, 18, 65, 160000, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (862, '天机头盔', 15, 188, 8, 0, 5, 0, 1853, 8000, 8, 12, 7, 11, 0, 0, 0, 0, 3, 9, 18, 65, 160000, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (863, '天机腰带', 54, 188, 5, 0, 5, 0, 1855, 8000, 5, 8, 5, 8, 0, 0, 0, 0, 4, 6, 18, 65, 160000, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (864, '天机靴子', 52, 188, 5, 12, 5, 0, 1854, 8000, 5, 8, 5, 8, 0, 0, 0, 0, 4, 6, 18, 65, 160000, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (865, '天机勋章', 30, 188, 1, 0, 1, 0, 1856, 50000, 2, 8, 2, 8, 0, 0, 0, 0, 2, 8, 18, 65, 160000, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (866, '魔令戒指', 22, 188, 2, 0, 5, 0, 1860, 8000, 0, 4, 0, 4, 0, 0, 6, 16, 0, 0, 18, 65, 160000, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (867, '魔令护腕', 26, 188, 2, 0, 5, 0, 1861, 8000, 0, 4, 0, 4, 0, 0, 5, 13, 0, 0, 18, 65, 160000, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (868, '魔令项链', 20, 188, 2, 0, 5, 0, 1862, 8000, 0, 0, 0, 4, 0, 0, 4, 17, 0, 0, 18, 65, 160000, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (869, '魔令头盔', 15, 188, 5, 0, 5, 0, 1863, 8000, 7, 11, 7, 10, 0, 0, 3, 9, 0, 0, 18, 65, 160000, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (870, '魔令腰带', 54, 188, 3, 0, 5, 0, 1865, 8000, 5, 8, 5, 8, 0, 0, 4, 6, 0, 0, 18, 65, 160000, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (871, '魔令靴子', 52, 188, 3, 12, 5, 0, 1864, 8000, 5, 8, 5, 8, 0, 0, 4, 6, 0, 0, 18, 65, 160000, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (872, '魔令勋章', 30, 188, 1, 0, 1, 0, 1866, 50000, 0, 0, 2, 8, 0, 0, 2, 8, 0, 0, 18, 65, 160000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (873, '震天戒指', 22, 188, 2, 0, 6, 0, 2131, 10000, 0, 3, 0, 3, 0, 16, 2, 15, 2, 16, 18, 68, 180000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (874, '震天护腕', 26, 188, 2, 0, 6, 0, 2130, 10000, 0, 4, 0, 4, 1, 12, 1, 12, 1, 12, 18, 68, 180000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (875, '震天项链', 20, 188, 2, 0, 6, 0, 2132, 10000, 0, 2, 0, 2, 3, 15, 0, 16, 3, 15, 18, 68, 180000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (876, '震天头盔', 15, 188, 5, 0, 6, 0, 2133, 10000, 8, 13, 8, 13, 0, 0, 3, 9, 0, 0, 18, 68, 180000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (877, '震天面巾', 16, 2, 1, 1, 0, 0, 1710, 8000, 1, 3, 1, 3, 0, 0, 0, 3, 0, 0, 18, 68, 180000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (878, '震天腰带', 54, 188, 3, 0, 6, 0, 2135, 10000, 6, 9, 6, 9, 0, 0, 5, 7, 0, 0, 18, 68, 180000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (879, '震天靴子', 52, 188, 3, 15, 6, 0, 2134, 10000, 6, 9, 6, 9, 0, 0, 5, 7, 0, 0, 18, 68, 180000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (880, '震天勋章', 30, 188, 1, 0, 1, 0, 2136, 50000, 0, 5, 0, 5, 2, 9, 2, 9, 2, 9, 18, 68, 180000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (881, '日月戒指', 22, 188, 2, 0, 6, 0, 2270, 10000, 0, 4, 0, 4, 0, 0, 0, 0, 6, 19, 18, 68, 180000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (882, '日月护腕', 26, 188, 2, 0, 6, 0, 2271, 10000, 0, 5, 0, 5, 0, 0, 0, 0, 5, 15, 18, 68, 180000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (883, '日月项链', 19, 188, 2, 0, 6, 0, 2273, 10000, 0, 0, 0, 3, 0, 0, 0, 0, 7, 18, 18, 68, 180000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (884, '日月头盔', 15, 188, 5, 0, 6, 0, 2272, 10000, 9, 14, 8, 13, 0, 0, 0, 0, 3, 9, 18, 68, 180000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (885, '日月面巾', 16, 2, 1, 1, 0, 0, 1711, 8000, 1, 3, 1, 3, 0, 0, 0, 0, 1, 3, 18, 68, 18000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (886, '日月腰带', 54, 188, 3, 0, 6, 0, 2275, 10000, 6, 9, 6, 9, 0, 0, 0, 0, 5, 7, 18, 68, 180000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (887, '日月靴子', 52, 188, 3, 15, 6, 0, 2274, 10000, 6, 9, 6, 9, 0, 0, 0, 0, 5, 7, 18, 68, 180000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (888, '传奇之冠', 15, 188, 5, 0, 6, 0, 2515, 10000, 9, 14, 7, 12, 3, 9, 0, 0, 0, 0, 18, 68, 200000, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (889, '传奇斗笠', 16, 5, 1, 0, 0, 0, 2522, 6000, 2, 4, 2, 4, 1, 3, 1, 3, 1, 3, 18, 68, 200000, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (890, '传奇面巾', 16, 2, 1, 1, 0, 0, 2517, 8000, 1, 3, 1, 3, 0, 3, 0, 0, 0, 0, 18, 68, 200000, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (891, '传奇项链', 19, 188, 1, 0, 6, 0, 2512, 8000, 0, 0, 0, 3, 7, 16, 0, 0, 0, 0, 0, 68, 200000, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (892, '传奇护腕', 24, 188, 1, 0, 6, 0, 2511, 8000, 0, 6, 0, 8, 5, 14, 0, 0, 0, 0, 0, 68, 200000, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (893, '传奇之戒', 22, 188, 1, 0, 6, 0, 2510, 8000, 0, 4, 0, 4, 4, 18, 0, 0, 0, 0, 0, 68, 200000, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (894, '传奇腰带', 54, 188, 1, 0, 6, 0, 2514, 8000, 6, 9, 6, 9, 5, 7, 0, 0, 0, 0, 18, 68, 200000, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (895, '传奇之靴', 52, 188, 1, 20, 6, 0, 2513, 10000, 6, 9, 6, 9, 5, 7, 0, 0, 0, 0, 18, 68, 200000, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (896, '传奇勋章', 30, 188, 1, 0, 1, 0, 2516, 50000, 1, 8, 1, 8, 1, 10, 1, 10, 1, 10, 18, 68, 200000, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (897, '皓月项链', 19, 188, 1, 0, 10, 0, 2774, 12000, 0, 0, 0, 4, 5, 17, 3, 17, 5, 17, 18, 70, 250000, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (898, '皓月护腕', 26, 188, 1, 0, 10, 0, 2773, 12000, 0, 5, 0, 5, 3, 15, 5, 15, 5, 15, 18, 70, 250000, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (899, '皓月戒指', 22, 188, 1, 0, 10, 0, 2772, 12000, 0, 6, 0, 6, 2, 19, 5, 19, 5, 20, 18, 70, 250000, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (900, '皓月腰带', 54, 188, 1, 30, 10, 0, 2776, 12000, 4, 9, 5, 8, 3, 9, 3, 9, 3, 9, 18, 70, 250000, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (901, '皓月靴子', 52, 188, 1, 30, 10, 0, 2775, 12000, 4, 9, 5, 8, 3, 10, 3, 10, 3, 10, 18, 70, 250000, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (902, '皓月头盔', 15, 188, 10, 0, 10, 0, 2777, 12000, 6, 12, 6, 10, 3, 10, 3, 10, 3, 10, 18, 70, 250000, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (903, '皓月勋章', 30, 188, 1, 0, 1, 0, 2779, 12000, 1, 10, 1, 10, 1, 12, 1, 12, 1, 12, 18, 70, 250000, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (904, '皓月面具', 16, 2, 1, 0, 0, 0, 2778, 12000, 3, 6, 3, 6, 1, 5, 1, 5, 1, 5, 18, 70, 250000, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (905, '★任务奖励★', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (906, '双刃剑', 5, 13, 18, 0, 0, 0, 5135, 20000, 0, 0, 0, 0, 8, 16, 0, 0, 0, 0, 0, 22, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (907, '炽炎双刃剑', 5, 13, 18, 0, 0, 0, 5138, 20000, 0, 0, 0, 11, 10, 16, 0, 0, 0, 0, 0, 23, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (908, '毒魔双刃剑', 5, 13, 18, 0, 3, 0, 5136, 20000, 0, 0, 0, 0, 8, 16, 0, 0, 0, 3, 0, 23, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (909, '寒冰双刃剑', 5, 13, 18, 0, 0, 0, 5137, 20000, 0, 1, 0, 0, 8, 16, 0, 4, 0, 0, 0, 23, 8000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (910, '祖玛炼狱', 6, 11, 60, 0, 0, 0, 5147, 28000, 0, 0, 0, 11, 0, 26, 0, 0, 0, 0, 0, 26, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (911, '诅咒炼狱', 6, 11, 60, 0, 0, 0, 5148, 28000, 0, 0, 0, 1, 0, 25, 0, 0, 0, 0, 0, 26, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (912, '沃玛炼狱', 6, 11, 60, 0, 0, 0, 5149, 28000, 0, 1, 0, 0, 0, 24, 0, 0, 0, 0, 0, 26, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (913, '潘夜炼狱', 6, 11, 60, 0, 0, 0, 5150, 28000, 0, 0, 0, 0, 0, 26, 0, 0, 0, 0, 0, 26, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (914, '祖玛井中月', 5, 17, 58, 0, 0, 0, 5123, 30000, 0, 0, 0, 11, 8, 23, 0, 0, 0, 0, 0, 28, 30000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (915, '诅咒井中月', 5, 17, 58, 0, 0, 0, 5124, 30000, 0, 0, 0, 1, 10, 21, 0, 0, 0, 0, 0, 28, 30000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (916, '沃玛井中月', 5, 17, 58, 0, 0, 0, 5125, 30000, 0, 1, 0, 0, 10, 21, 0, 0, 0, 0, 0, 28, 30000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (917, '潘夜井中月', 5, 17, 58, 0, 0, 0, 5126, 30000, 0, 0, 0, 0, 7, 23, 0, 0, 0, 0, 0, 28, 30000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (918, '祖玛裁决之杖', 6, 24, 80, 0, 0, 0, 5163, 32000, 0, 0, 0, 11, 0, 33, 0, 0, 0, 0, 0, 30, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (919, '诅咒裁决之杖', 6, 24, 80, 0, 0, 0, 5164, 32000, 0, 0, 0, 1, 0, 28, 0, 0, 0, 0, 0, 30, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (920, '沃玛裁决之杖', 6, 24, 80, 0, 0, 0, 5165, 32000, 0, 1, 0, 0, 0, 28, 0, 0, 0, 0, 0, 30, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (921, '潘夜裁决之杖', 6, 24, 80, 0, 0, 0, 5166, 32000, 0, 0, 0, 0, 0, 33, 0, 0, 0, 0, 0, 30, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (922, '祖玛屠龙', 5, 86, 99, 0, 0, 0, 5131, 33000, 0, 0, 0, 11, 5, 37, 0, 0, 0, 0, 0, 34, 70000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (923, '诅咒屠龙', 5, 86, 99, 0, 0, 0, 5132, 33000, 0, 0, 0, 1, 5, 33, 0, 0, 0, 0, 0, 34, 70000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (924, '沃玛屠龙', 5, 86, 99, 0, 0, 0, 5133, 33000, 0, 1, 0, 0, 5, 33, 0, 0, 0, 0, 0, 34, 70000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (925, '潘夜屠龙', 5, 86, 99, 0, 0, 0, 5134, 33000, 0, 0, 0, 0, 5, 37, 0, 0, 0, 0, 0, 34, 70000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (926, '祖玛银蛇', 5, 9, 26, 0, 3, 0, 5143, 24000, 0, 2, 0, 0, 7, 14, 0, 0, 1, 4, 0, 26, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (927, '诅咒银蛇', 5, 9, 26, 0, 0, 0, 5144, 24000, 0, 0, 0, 0, 7, 14, 0, 0, 1, 3, 0, 26, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (928, '沃玛银蛇', 5, 9, 26, 0, 0, 0, 5145, 24000, 0, 2, 0, 0, 7, 14, 0, 0, 1, 3, 0, 26, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (929, '潘夜银蛇', 5, 9, 26, 0, 0, 0, 5146, 24000, 0, 1, 0, 0, 7, 14, 0, 0, 1, 4, 0, 26, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (930, '祖玛无极棍', 5, 21, 15, 0, 3, 0, 5151, 25000, 0, 1, 0, 0, 8, 16, 0, 0, 2, 6, 3, 25, 40000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (931, '诅咒无极棍', 5, 21, 15, 0, 0, 0, 5152, 25000, 0, 0, 0, 0, 8, 16, 0, 0, 2, 4, 3, 25, 40000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (932, '沃玛无极棍', 5, 21, 15, 0, 0, 0, 5153, 25000, 0, 1, 0, 0, 8, 16, 0, 0, 2, 5, 3, 25, 40000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (933, '潘夜无极棍', 5, 21, 15, 0, 0, 0, 5154, 25000, 0, 0, 0, 0, 8, 16, 0, 0, 2, 6, 3, 25, 40000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (934, '祖玛龙纹剑', 5, 25, 40, 0, 5, 0, 5139, 22000, 0, 2, 0, 0, 8, 20, 0, 0, 3, 8, 0, 35, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (935, '诅咒龙纹剑', 5, 25, 40, 0, 0, 0, 5140, 22000, 0, 1, 0, 0, 8, 20, 0, 0, 3, 5, 0, 35, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (936, '沃玛龙纹剑', 5, 25, 40, 0, 0, 0, 5141, 22000, 0, 1, 0, 0, 8, 20, 0, 0, 3, 7, 0, 35, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (937, '潘夜龙纹剑', 5, 25, 40, 0, 0, 0, 5142, 22000, 0, 0, 0, 0, 8, 20, 0, 0, 3, 8, 0, 35, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (938, '祖玛魔杖', 6, 12, 10, 0, 0, 0, 5159, 15000, 0, 1, 0, 0, 5, 9, 1, 6, 0, 0, 0, 26, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (939, '诅咒魔杖', 6, 12, 10, 0, 0, 0, 5160, 15000, 0, 0, 0, 0, 5, 9, 2, 4, 0, 0, 0, 26, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (940, '沃玛魔杖', 6, 12, 10, 0, 0, 0, 5161, 15000, 0, 1, 0, 0, 5, 9, 2, 4, 0, 0, 0, 26, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (941, '潘夜魔杖', 6, 12, 10, 0, 0, 0, 5162, 15000, 0, 0, 0, 0, 5, 9, 1, 6, 0, 0, 0, 26, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (942, '祖玛骨玉权杖', 6, 28, 20, 0, 0, 0, 5127, 18000, 0, 2, 0, 0, 6, 12, 2, 7, 0, 0, 0, 35, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (943, '诅咒骨玉权杖', 6, 28, 20, 0, 0, 0, 5128, 18000, 0, 0, 0, 0, 6, 12, 3, 5, 0, 0, 0, 35, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (944, '沃玛骨玉权杖', 6, 28, 20, 0, 0, 0, 5129, 18000, 0, 1, 0, 0, 6, 12, 3, 5, 0, 0, 0, 35, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (945, '潘夜骨玉权杖', 6, 28, 20, 0, 0, 0, 5130, 18000, 0, 0, 0, 0, 6, 12, 2, 7, 0, 0, 0, 35, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (946, '祖玛嗜魂法杖', 6, 27, 26, 0, 0, 0, 5155, 10000, 0, 3, 0, 0, 6, 13, 2, 10, 0, 0, 0, 35, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (947, '诅咒嗜魂法杖', 6, 27, 26, 0, 0, 0, 5156, 10000, 0, 0, 0, 0, 6, 13, 3, 6, 0, 0, 0, 35, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (948, '沃玛嗜魂法杖', 6, 27, 26, 0, 0, 0, 5157, 10000, 0, 2, 0, 0, 6, 13, 3, 6, 0, 0, 0, 35, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (949, '潘夜嗜魂法杖', 6, 27, 26, 0, 0, 0, 5158, 10000, 0, 0, 0, 0, 6, 13, 2, 10, 0, 0, 0, 35, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (950, '黄金裁决之杖', 6, 63, 80, 0, -5, 0, 1687, 42000, 0, 0, 0, 0, 0, 40, 0, 0, 0, 0, 0, 42, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (951, '黄金屠龙', 5, 69, 99, 0, 0, 0, 1787, 60000, 0, 0, 0, 0, 1, 45, 0, 0, 0, 0, 0, 48, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (952, '黄金青龙刺', 5, 66, 35, 0, 0, 0, 1786, 45000, 0, 2, 0, 0, 8, 16, 2, 12, 2, 12, 0, 42, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (953, '黄金乾坤', 5, 65, 25, 0, 0, 0, 1781, 30000, 0, 3, 0, 0, 10, 33, 0, 0, 5, 17, 0, 48, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (954, '黄金嗜魂法杖', 5, 64, 13, 0, 0, 0, 1780, 60000, 0, 0, 0, 0, 6, 13, 6, 16, 0, 0, 0, 48, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (955, '屠龙刀', 5, 86, 99, 0, 0, 0, 5042, 33000, 0, 0, 0, 11, 5, 50, 0, 0, 0, 0, 0, 53, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (956, '无名刀', 5, 87, 40, 0, 0, 0, 5043, 22000, 0, 2, 0, 0, 8, 20, 0, 0, 2, 22, 0, 53, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (957, '飞龙卦', 5, 88, 26, 0, 0, 0, 5044, 12000, 0, 3, 0, 0, 6, 13, 3, 18, 0, 0, 0, 53, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (958, '旋风流星刀', 5, 89, 60, 0, 0, 0, 5045, 40000, 0, 0, 0, 11, 8, 60, 0, 0, 0, 0, 0, 56, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (959, '封魔剑', 5, 90, 40, 0, -5, 0, 5046, 28000, 0, 0, 0, 0, 10, 28, 0, 0, 5, 28, 0, 56, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (960, '飞魂魔刃', 5, 91, 28, 0, 0, 0, 5047, 15000, 0, 1, 0, 0, 8, 15, 5, 26, 0, 0, 0, 56, 80000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (961, '翔空剑', 5, 38, 80, 0, 0, 0, 5167, 45000, 0, 0, 0, 11, 0, 100, 0, 0, 0, 0, 0, 60, 150000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (962, '铁轮', 6, 39, 28, 0, 0, 0, 5168, 25000, 0, 3, 0, 0, 8, 22, 16, 47, 0, 0, 0, 60, 150000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (963, '阴阳刀', 5, 40, 45, 0, -5, 0, 5169, 30000, 0, 2, 0, 0, 15, 33, 0, 0, 18, 48, 0, 60, 150000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (964, '飞龙剑', 5, 95, 99, 0, 0, 0, 5051, 42000, 0, 0, 0, 12, 0, 105, 0, 0, 0, 0, 0, 65, 120000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (965, '影魅之刃', 5, 96, 35, 0, -8, 0, 5052, 45000, 0, 5, 0, 0, 6, 32, 7, 56, 8, 60, 0, 65, 150000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (966, '强化魔法头盔', 15, 0, 2, 0, 0, 0, 5026, 8000, 1, 1, 2, 3, 0, 0, 0, 0, 0, 0, 18, 12, 1800, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (967, '天藤头盔', 15, 0, 3, 0, 0, 0, 5027, 8000, 3, 4, 1, 2, 0, 0, 0, 0, 0, 0, 18, 22, 5000, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (968, '沃玛头盔', 15, 0, 5, 0, 0, 0, 5028, 8000, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 18, 26, 8000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (969, '魔力头盔', 15, 0, 6, 0, 0, 0, 5029, 10000, 1, 2, 5, 8, 0, 0, 0, 0, 0, 0, 18, 33, 20000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (970, '龙血头盔', 15, 188, 3, 0, 1, 0, 5030, 5000, 5, 6, 0, 0, 0, 0, 0, 1, 0, 0, 0, 40, 25000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (971, '幻影头盔', 15, 188, 5, 0, 1, 0, 5031, 6000, 5, 6, 0, 0, 0, 0, 0, 0, 0, 1, 0, 40, 25000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (972, '神御头盔', 15, 188, 10, 0, 1, 0, 5032, 8000, 5, 6, 0, 0, 0, 1, 0, 0, 0, 0, 0, 40, 25000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (973, '霸龙头盔', 15, 188, 5, 0, 1, 0, 5034, 10000, 0, 0, 5, 8, 0, 2, 0, 2, 0, 2, 0, 42, 30000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (974, '行者帽', 15, 188, 3, 0, 1, 0, 5035, 8000, 3, 4, 5, 8, 0, 1, 0, 1, 0, 1, 0, 42, 30000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (975, '明王头盔', 15, 188, 5, 0, 1, 0, 5036, 80000, 3, 5, 0, 0, 1, 4, 1, 4, 1, 4, 0, 47, 35000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (976, '虎面头盔', 15, 188, 5, 0, 2, 0, 5037, 10000, 8, 15, 0, 0, 3, 9, 3, 9, 3, 9, 0, 56, 40000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (977, '赤龙头盔', 15, 188, 5, 0, 2, 0, 5038, 12000, 0, 0, 12, 18, 4, 7, 4, 7, 4, 7, 0, 60, 50000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (978, '牛头面具', 16, 188, 1, 0, 1, 0, 5033, 8000, 2, 2, 1, 2, 0, 0, 0, 0, 0, 0, 0, 48, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (979, '红色铁布衫(男)', 10, 50, 5, 0, 0, 0, 2340, 22000, 4, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (980, '橙色铁布衫(男)', 10, 51, 5, 0, 0, 0, 2341, 22000, 5, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 13, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (981, '黄色铁布衫(男)', 10, 52, 8, 0, 0, 0, 2342, 22000, 6, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (982, '蓝色铁布衫(男)', 10, 53, 8, 0, 0, 0, 2343, 22000, 6, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (983, '红色铁布衫(女)', 11, 50, 10, 0, 0, 0, 2344, 22000, 4, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 9, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (984, '橙色铁布衫(女)', 11, 51, 10, 0, 0, 0, 2345, 22000, 5, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 13, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (985, '黄色铁布衫(女)', 11, 52, 15, 0, 0, 0, 2346, 22000, 6, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (986, '蓝色铁布衫(女)', 11, 53, 15, 0, 0, 0, 2347, 22000, 6, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 18, 15000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (987, '天掌靴子', 52, 188, 2, 10, 6, 0, 5039, 8000, 0, 3, 0, 3, 0, 0, 0, 0, 0, 0, 18, 42, 20000, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (988, '赤飞靴子', 52, 188, 3, 15, 8, 0, 5040, 9000, 2, 4, 2, 4, 0, 0, 0, 0, 0, 0, 18, 58, 30000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (989, '黑皮靴子', 52, 188, 4, 20, 12, 0, 5041, 10000, 6, 8, 6, 8, 0, 0, 0, 0, 0, 0, 18, 65, 40000, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (990, '铁护腕', 26, 188, 1, 0, 1, 0, 514, 7000, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 1000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (991, '玉玻璃戒指', 23, 0, 1, 0, 0, 0, 512, 7000, 0, 2, 0, 3, 0, 0, 0, 0, 0, 0, 0, 15, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (992, '蓝玉项链', 20, 0, 1, 0, 0, 0, 223, 8000, 0, 0, 0, 2, 2, 1, 0, 0, 0, 0, 0, 17, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (993, '绿玉项链', 20, 0, 1, 0, 0, 0, 224, 8000, 0, 0, 0, 2, 0, 0, 1, 2, 0, 0, 0, 17, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (994, '白玉项链', 20, 0, 1, 0, 0, 0, 227, 8000, 0, 0, 0, 2, 0, 0, 0, 0, 1, 2, 0, 17, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (995, '铂金手镯', 26, 188, 1, 0, 1, 0, 201, 6000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 3, 17, 10000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (996, '紫色白玉项链', 21, 0, 1, 0, 0, 0, 519, 7000, 0, 3, 0, 3, 0, 0, 1, 3, 0, 0, 0, 18, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (997, '邪眼手镯', 24, 188, 1, 0, 1, 0, 517, 7000, 0, 1, 0, 0, 0, 0, 0, 0, 0, 3, 3, 22, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (998, '紫翡翠项链', 21, 0, 1, 0, 0, 0, 518, 7000, 0, 5, 0, 0, 3, 0, 0, 0, 0, 0, 0, 22, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (999, '玄铁手套', 26, 188, 1, 0, 1, 0, 515, 7000, 0, 3, 0, 3, 0, 0, 0, 0, 0, 0, 1, 24, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);
INSERT INTO `tbl_stditems` VALUES (1000, '邪魔手镯', 26, 188, 1, 0, 1, 0, 516, 7000, 0, 2, 0, 0, 0, 0, 0, 1, 0, 0, 2, 24, 20000, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 100, 251, 0, 0);

-- ----------------------------
-- Table structure for tbl_storages
-- ----------------------------
DROP TABLE IF EXISTS `tbl_storages`;
CREATE TABLE `tbl_storages`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_PLAYERID` int(11) NULL DEFAULT NULL,
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
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '仓库物品' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of tbl_storages
-- ----------------------------

-- ----------------------------
-- Table structure for tbl_upgradeinfo
-- ----------------------------
DROP TABLE IF EXISTS `tbl_upgradeinfo`;
CREATE TABLE `tbl_upgradeinfo`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_PLAYERID` int(11) NULL DEFAULT NULL COMMENT '玩家ID',
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
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '武器升级数据' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of tbl_upgradeinfo
-- ----------------------------

SET FOREIGN_KEY_CHECKS = 1;
