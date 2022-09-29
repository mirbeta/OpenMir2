/*
 Navicat Premium Data Transfer

 Source Server         : 10.10.0.199
 Source Server Type    : MySQL
 Source Server Version : 50727
 Source Host           : 10.10.0.199:3306
 Source Schema         : mir2_db

 Target Server Type    : MySQL
 Target Server Version : 50727
 File Encoding         : 65001

 Date: 29/09/2022 20:05:54
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for characters
-- ----------------------------
DROP TABLE IF EXISTS `characters`;
CREATE TABLE `characters`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `ServerIndex` int(11) NOT NULL DEFAULT 0 COMMENT '服务器编号',
  `LoginID` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '登陆账号',
  `CharName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '角色名称',
  `MapName` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '所在地图名称',
  `CX` int(11) NULL DEFAULT 0 COMMENT '所在地图坐标X',
  `CY` int(11) NULL DEFAULT 0 COMMENT '所在地图坐标Y',
  `Level` int(11) NULL DEFAULT 0 COMMENT '等级',
  `Dir` tinyint(2) NULL DEFAULT 0 COMMENT '所在方向',
  `Hair` tinyint(1) NULL DEFAULT 0 COMMENT '发型',
  `Sex` tinyint(1) NULL DEFAULT 0 COMMENT '性别（0:男 1:女）',
  `Job` tinyint(1) NULL DEFAULT 0 COMMENT '职业（0:战士 1:法师 2:道士）',
  `Gold` int(11) NULL DEFAULT 0 COMMENT '金币数',
  `GamePoint` int(11) NULL DEFAULT 0 COMMENT '金刚石',
  `HomeMap` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '0' COMMENT '回城地图',
  `HomeX` int(5) NULL DEFAULT 0 COMMENT '回城坐标X',
  `HomeY` int(5) NULL DEFAULT 0 COMMENT '回城坐标Y',
  `PkPoint` int(5) NULL DEFAULT 0 COMMENT 'PK值',
  `ReLevel` tinyint(2) NULL DEFAULT 0 COMMENT '转生次数',
  `AttatckMode` tinyint(2) NULL DEFAULT 0 COMMENT '攻击模式',
  `FightZoneDieCount` tinyint(3) NULL DEFAULT 0 COMMENT '行会战争死亡次数',
  `BodyLuck` double(10, 3) NULL DEFAULT 0.000 COMMENT '幸运值',
  `IncHealth` tinyint(3) NULL DEFAULT 0,
  `IncSpell` tinyint(3) NULL DEFAULT 0,
  `IncHealing` tinyint(3) NULL DEFAULT 0,
  `CreditPoint` tinyint(3) NULL DEFAULT 0 COMMENT '声望点数',
  `BonusPoint` int(5) NULL DEFAULT 0 COMMENT '奖励点数',
  `HungerStatus` int(5) NULL DEFAULT 0 COMMENT '状态',
  `PayMentPoint` int(5) NULL DEFAULT 0,
  `LockLogon` tinyint(1) NULL DEFAULT 0 COMMENT '是否锁定登陆',
  `MarryCount` int(5) NULL DEFAULT 0 COMMENT '结婚次数',
  `AllowGroup` tinyint(1) NULL DEFAULT 0 COMMENT '是否允许组队',
  `AllowGroupReCall` tinyint(1) NULL DEFAULT 0 COMMENT '是否允许组队传送',
  `GroupRcallTime` int(5) NULL DEFAULT 0 COMMENT '组队传送间隔',
  `AllowGuildReCall` tinyint(1) NULL DEFAULT 0 COMMENT '是否允许行会传送',
  `IsMaster` tinyint(1) NULL DEFAULT 0 COMMENT '是否收徒',
  `MasterName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '师傅名称',
  `DearName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '配偶名称',
  `StoragePwd` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '仓库密码',
  `Deleted` tinyint(1) NULL DEFAULT NULL COMMENT '是否删除',
  `CREATEDATE` datetime NULL DEFAULT NULL COMMENT '创建日期',
  `LASTUPDATE` datetime NULL DEFAULT NULL COMMENT '修改日期',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6222 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '玩家' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of characters
-- ----------------------------
INSERT INTO `characters` VALUES (1, 0, '1', 'gm01', '3', 283, 349, 50, 0, 1, 0, 0, 891095, 0, '3', 330, 330, 0, 0, 0, 0, -244.658, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, 0, '2022-08-03 01:33:16', '2022-09-29 19:58:09');
INSERT INTO `characters` VALUES (6221, 1, 'admin', '深山老毒', NULL, 0, 0, 0, 0, 1, 0, 2, 0, 0, NULL, 0, 0, 0, 0, 0, 0, 0.000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, 0, '2022-09-13 13:28:09', '2022-09-13 13:28:09');

-- ----------------------------
-- Table structure for characters_ablity
-- ----------------------------
DROP TABLE IF EXISTS `characters_ablity`;
CREATE TABLE `characters_ablity`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `PlayerId` bigint(20) NOT NULL COMMENT '角色ID',
  `Level` tinyint(1) UNSIGNED NOT NULL COMMENT '当前等级',
  `Ac` smallint(11) NOT NULL COMMENT '攻击防御',
  `Mac` smallint(11) NOT NULL COMMENT '魔法防御',
  `Dc` smallint(11) NOT NULL COMMENT '物理攻击力',
  `Mc` smallint(11) NOT NULL COMMENT '魔法攻击力',
  `Sc` smallint(11) NOT NULL COMMENT '道术攻击力',
  `Hp` smallint(11) NOT NULL COMMENT '当前HP',
  `Mp` smallint(11) NOT NULL COMMENT '当前MP',
  `MaxHP` smallint(11) NOT NULL COMMENT '最大HP',
  `MAxMP` smallint(11) NOT NULL COMMENT '最大MP',
  `Exp` int(11) NOT NULL COMMENT '当前经验',
  `MaxExp` int(11) NOT NULL COMMENT '升级经验',
  `Weight` smallint(11) NOT NULL COMMENT '当前包裹重量',
  `MaxWeight` smallint(11) NOT NULL COMMENT '最大包裹重量',
  `WearWeight` tinyint(1) UNSIGNED NOT NULL COMMENT '当前腕力',
  `MaxWearWeight` tinyint(1) UNSIGNED NOT NULL COMMENT '最大腕力',
  `HandWeight` tinyint(1) UNSIGNED NOT NULL COMMENT '当前负重',
  `MaxHandWeight` tinyint(1) UNSIGNED NOT NULL COMMENT '最大负重',
  `ModifyTime` datetime NULL DEFAULT NULL COMMENT '最后修改时间',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6222 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '基础属性' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of characters_ablity
-- ----------------------------
INSERT INTO `characters_ablity` VALUES (1, 1, 50, 50, 0, 0, 0, 0, 939, 186, 0, 0, 2671, 200000000, 2, 850, 0, 115, 7, 162, '2022-09-29 19:58:09');
INSERT INTO `characters_ablity` VALUES (6221, 6221, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL);

-- ----------------------------
-- Table structure for characters_bagitem
-- ----------------------------
DROP TABLE IF EXISTS `characters_bagitem`;
CREATE TABLE `characters_bagitem`  (
  `PlayerId` bigint(20) NULL DEFAULT NULL COMMENT '角色ID',
  `ChrName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '角色名称',
  `Position` int(10) NULL DEFAULT 0 COMMENT '位置',
  `MakeIndex` int(20) NULL DEFAULT 0 COMMENT '物品唯一ID',
  `StdIndex` int(10) NULL DEFAULT 0 COMMENT '物品编号',
  `Dura` int(10) NULL DEFAULT 0 COMMENT '当前持久',
  `DuraMax` int(10) NULL DEFAULT 0 COMMENT '最大持久'
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '包裹' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of characters_bagitem
-- ----------------------------
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 0, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 1, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 2, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 3, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 4, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 5, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 6, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 7, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 8, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 9, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 10, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 11, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 12, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 13, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 14, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 15, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 16, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 17, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 18, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 19, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 20, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 21, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 22, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 23, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 24, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 25, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 26, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 27, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 28, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 29, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 30, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 31, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 32, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 33, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 34, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 35, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 36, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 37, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 38, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 39, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 40, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 41, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 42, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 43, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 44, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 'gm01', 45, 0, 0, 0, 0);

-- ----------------------------
-- Table structure for characters_bonusability
-- ----------------------------
DROP TABLE IF EXISTS `characters_bonusability`;
CREATE TABLE `characters_bonusability`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `PLAYERID` int(50) NULL DEFAULT NULL,
  `AC` int(11) NULL DEFAULT NULL,
  `MAC` int(11) NULL DEFAULT NULL,
  `DC` int(11) NULL DEFAULT NULL,
  `MC` int(11) NULL DEFAULT NULL,
  `SC` int(11) NULL DEFAULT NULL,
  `HP` int(11) NULL DEFAULT NULL,
  `MP` int(11) NULL DEFAULT NULL,
  `HIT` int(11) NULL DEFAULT NULL,
  `SPEED` int(11) NULL DEFAULT NULL,
  `RESERVED` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '奖励属性' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of characters_bonusability
-- ----------------------------

-- ----------------------------
-- Table structure for characters_indexes
-- ----------------------------
DROP TABLE IF EXISTS `characters_indexes`;
CREATE TABLE `characters_indexes`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Account` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ChrName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `SelectID` tinyint(2) NULL DEFAULT NULL,
  `IsDeleted` tinyint(1) NULL DEFAULT NULL,
  `CreateDate` datetime NULL DEFAULT NULL,
  `ModifyDate` datetime NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6222 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '角色索引' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of characters_indexes
-- ----------------------------
INSERT INTO `characters_indexes` VALUES (1, 'admin', 'gm01', 1, 0, '2022-08-03 01:33:16', '2022-09-06 21:18:50');
INSERT INTO `characters_indexes` VALUES (6221, 'admin', '深山老毒', 0, 0, '2022-09-13 13:28:09', '2022-09-13 13:28:09');

-- ----------------------------
-- Table structure for characters_item
-- ----------------------------
DROP TABLE IF EXISTS `characters_item`;
CREATE TABLE `characters_item`  (
  `PlayerId` bigint(20) NULL DEFAULT NULL COMMENT '角色ID',
  `ChrName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '角色名称',
  `Position` int(10) NULL DEFAULT 0 COMMENT '穿戴位置',
  `MakeIndex` int(20) NULL DEFAULT 0 COMMENT '物品唯一ID',
  `StdIndex` int(10) NULL DEFAULT 0 COMMENT '物品编号',
  `Dura` int(10) NULL DEFAULT 0 COMMENT '当前持久',
  `DuraMax` int(10) NULL DEFAULT 0 COMMENT '最大持久'
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '穿戴物品' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of characters_item
-- ----------------------------
INSERT INTO `characters_item` VALUES (1, 'gm01', 0, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (1, 'gm01', 1, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (1, 'gm01', 2, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (1, 'gm01', 3, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (1, 'gm01', 4, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (1, 'gm01', 5, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (1, 'gm01', 6, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (1, 'gm01', 7, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (1, 'gm01', 8, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (1, 'gm01', 9, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (1, 'gm01', 10, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (1, 'gm01', 11, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (1, 'gm01', 12, 0, 0, 0, 0);

-- ----------------------------
-- Table structure for characters_item_attr
-- ----------------------------
DROP TABLE IF EXISTS `characters_item_attr`;
CREATE TABLE `characters_item_attr`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `PlayerId` bigint(20) NOT NULL,
  `MakeIndex` bigint(20) NOT NULL,
  `Value0` tinyint(2) NOT NULL,
  `Value1` tinyint(2) NOT NULL,
  `Value2` tinyint(2) NOT NULL,
  `Value3` tinyint(2) NOT NULL,
  `Value4` tinyint(2) NOT NULL,
  `Value5` tinyint(2) NOT NULL,
  `Value6` tinyint(2) NOT NULL,
  `Value7` tinyint(2) NOT NULL,
  `Value8` tinyint(2) NOT NULL,
  `Value9` tinyint(2) NOT NULL,
  `Value10` tinyint(2) NOT NULL,
  `Value11` tinyint(2) NOT NULL,
  `Value12` tinyint(2) NOT NULL,
  `Value13` tinyint(2) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci COMMENT = '装备附加属性' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of characters_item_attr
-- ----------------------------

-- ----------------------------
-- Table structure for characters_magic
-- ----------------------------
DROP TABLE IF EXISTS `characters_magic`;
CREATE TABLE `characters_magic`  (
  `PlayerId` int(11) NOT NULL,
  `MagicId` int(11) NOT NULL COMMENT '技能ID',
  `Level` int(11) NOT NULL COMMENT '技能等级',
  `UseKey` char(2) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '技能按键',
  `CurrTrain` int(11) NOT NULL COMMENT '当前经验'
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '技能' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of characters_magic
-- ----------------------------

-- ----------------------------
-- Table structure for characters_quest
-- ----------------------------
DROP TABLE IF EXISTS `characters_quest`;
CREATE TABLE `characters_quest`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `PLAYERID` int(11) NULL DEFAULT NULL,
  `QUESTOPENINDEX` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `QUESTFININDEX` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `QUEST` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '任务标志' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of characters_quest
-- ----------------------------

-- ----------------------------
-- Table structure for characters_status
-- ----------------------------
DROP TABLE IF EXISTS `characters_status`;
CREATE TABLE `characters_status`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `PlayerId` bigint(20) NULL DEFAULT NULL,
  `ChrName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Status` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 36812 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '人物状态值' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of characters_status
-- ----------------------------
INSERT INTO `characters_status` VALUES (36811, 1, 'gm01', '0/0/0/0/0/0/0/0/0/0/0/0/0/0/0');

-- ----------------------------
-- Table structure for characters_storageitem
-- ----------------------------
DROP TABLE IF EXISTS `characters_storageitem`;
CREATE TABLE `characters_storageitem`  (
  `PlayerId` bigint(20) NULL DEFAULT NULL COMMENT '角色ID',
  `ChrName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '角色名称',
  `Position` int(10) NULL DEFAULT 0 COMMENT '位置',
  `MakeIndex` int(20) NULL DEFAULT 0 COMMENT '物品唯一ID',
  `StdIndex` int(10) NULL DEFAULT 0 COMMENT '物品编号',
  `Dura` int(10) NULL DEFAULT 0 COMMENT '当前持久',
  `DuraMax` int(10) NULL DEFAULT 0 COMMENT '最大持久'
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '角色仓库物品' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of characters_storageitem
-- ----------------------------
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 0, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 1, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 2, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 3, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 4, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 5, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 6, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 7, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 8, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 9, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 10, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 11, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 12, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 13, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 14, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 15, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 16, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 17, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 18, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 19, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 20, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 21, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 22, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 23, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 24, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 25, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 26, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 27, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 28, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 29, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 30, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 31, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 32, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 33, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 34, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 35, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 36, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 37, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 38, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 39, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 40, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 41, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 42, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 43, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 44, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 45, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 46, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 47, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 48, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 'gm01', 49, 0, 0, 0, 0);

SET FOREIGN_KEY_CHECKS = 1;
