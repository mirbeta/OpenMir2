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

 Date: 26/11/2022 07:52:36
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for characters
-- ----------------------------
DROP TABLE IF EXISTS `characters`;
CREATE TABLE `characters` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `ServerIndex` int(11) NOT NULL DEFAULT '0' COMMENT '服务器编号',
  `LoginID` varchar(20) DEFAULT NULL COMMENT '登陆账号',
  `ChrName` varchar(50) DEFAULT NULL COMMENT '角色名称',
  `MapName` varchar(10) DEFAULT NULL COMMENT '所在地图名称',
  `CX` int(11) DEFAULT '0' COMMENT '所在地图坐标X',
  `CY` int(11) DEFAULT '0' COMMENT '所在地图坐标Y',
  `Level` int(11) DEFAULT '0' COMMENT '等级',
  `Dir` tinyint(2) DEFAULT '0' COMMENT '所在方向',
  `Hair` tinyint(1) DEFAULT '0' COMMENT '发型',
  `Sex` tinyint(1) DEFAULT '0' COMMENT '性别（0:男 1:女）',
  `Job` tinyint(2) DEFAULT '0' COMMENT '职业（0:战士 1:法师 2:道士）',
  `Gold` int(11) DEFAULT '0' COMMENT '金币数',
  `GamePoint` int(11) DEFAULT '0' COMMENT '金刚石',
  `HomeMap` varchar(10) DEFAULT '0' COMMENT '回城地图',
  `HomeX` int(5) DEFAULT '0' COMMENT '回城坐标X',
  `HomeY` int(5) DEFAULT '0' COMMENT '回城坐标Y',
  `PkPoint` int(5) DEFAULT '0' COMMENT 'PK值',
  `ReLevel` tinyint(2) DEFAULT '0' COMMENT '转生次数',
  `AttatckMode` tinyint(2) DEFAULT '0' COMMENT '攻击模式',
  `FightZoneDieCount` tinyint(3) DEFAULT '0' COMMENT '行会战争死亡次数',
  `BodyLuck` double(10,3) DEFAULT '0.000' COMMENT '幸运值',
  `IncHealth` tinyint(3) DEFAULT '0',
  `IncSpell` tinyint(3) DEFAULT '0',
  `IncHealing` tinyint(3) DEFAULT '0',
  `CreditPoint` tinyint(3) DEFAULT '0' COMMENT '声望点数',
  `BonusPoint` int(5) DEFAULT '0' COMMENT '奖励点数',
  `HungerStatus` int(5) DEFAULT '0' COMMENT '状态',
  `PayMentPoint` int(5) DEFAULT '0',
  `LockLogon` tinyint(1) DEFAULT '0' COMMENT '是否锁定登陆',
  `MarryCount` int(5) DEFAULT '0' COMMENT '结婚次数',
  `AllowGroup` tinyint(1) DEFAULT '0' COMMENT '是否允许组队',
  `AllowGroupReCall` tinyint(1) DEFAULT '0' COMMENT '是否允许组队传送',
  `GroupRcallTime` int(5) DEFAULT '0' COMMENT '组队传送间隔',
  `AllowGuildReCall` tinyint(1) DEFAULT '0' COMMENT '是否允许行会传送',
  `IsMaster` tinyint(1) DEFAULT '0' COMMENT '是否收徒',
  `MasterName` varchar(50) DEFAULT NULL COMMENT '师傅名称',
  `DearName` varchar(50) DEFAULT NULL COMMENT '配偶名称',
  `StoragePwd` varchar(50) DEFAULT NULL COMMENT '仓库密码',
  `Deleted` tinyint(1) DEFAULT NULL COMMENT '是否删除',
  `CREATEDATE` datetime DEFAULT NULL COMMENT '创建日期',
  `LASTUPDATE` datetime DEFAULT NULL COMMENT '修改日期',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=12215 DEFAULT CHARSET=utf8 COMMENT='角色信息';

-- ----------------------------
-- Table structure for characters_ablity
-- ----------------------------
DROP TABLE IF EXISTS `characters_ablity`;
CREATE TABLE `characters_ablity` (
  `PlayerId` bigint(20) NOT NULL COMMENT '角色ID',
  `Level` tinyint(1) unsigned NOT NULL COMMENT '当前等级',
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
  `WearWeight` tinyint(1) unsigned NOT NULL COMMENT '当前腕力',
  `MaxWearWeight` tinyint(1) unsigned NOT NULL COMMENT '最大腕力',
  `HandWeight` tinyint(1) unsigned NOT NULL COMMENT '当前负重',
  `MaxHandWeight` tinyint(1) unsigned NOT NULL COMMENT '最大负重',
  `ModifyTime` datetime DEFAULT NULL COMMENT '最后修改时间',
  PRIMARY KEY (`PlayerId`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='基础属性';

-- ----------------------------
-- Table structure for characters_bagitem
-- ----------------------------
DROP TABLE IF EXISTS `characters_bagitem`;
CREATE TABLE `characters_bagitem` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `PlayerId` bigint(20) NOT NULL COMMENT '角色ID',
  `Position` int(10) NOT NULL DEFAULT '0' COMMENT '位置',
  `MakeIndex` int(20) NOT NULL DEFAULT '0' COMMENT '物品唯一ID',
  `StdIndex` int(10) NOT NULL DEFAULT '0' COMMENT '物品编号',
  `Dura` int(10) NOT NULL DEFAULT '0' COMMENT '当前持久',
  `DuraMax` int(10) NOT NULL DEFAULT '0' COMMENT '最大持久',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=275771 DEFAULT CHARSET=utf8 COMMENT='包裹';

-- ----------------------------
-- Table structure for characters_bonusability
-- ----------------------------
DROP TABLE IF EXISTS `characters_bonusability`;
CREATE TABLE `characters_bonusability` (
  `PLAYERID` bigint(20) NOT NULL,
  `AC` int(11) NOT NULL,
  `MAC` int(11) NOT NULL,
  `DC` int(11) NOT NULL,
  `MC` int(11) NOT NULL,
  `SC` int(11) NOT NULL,
  `HP` int(11) NOT NULL,
  `MP` int(11) NOT NULL,
  `HIT` int(11) NOT NULL,
  `SPEED` int(11) NOT NULL,
  `RESERVED` int(11) NOT NULL,
  PRIMARY KEY (`PLAYERID`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='奖励属性';

-- ----------------------------
-- Table structure for characters_indexes
-- ----------------------------
DROP TABLE IF EXISTS `characters_indexes`;
CREATE TABLE `characters_indexes` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Account` varchar(255) NOT NULL COMMENT '登陆账号',
  `ChrName` varchar(255) NOT NULL COMMENT '角色名称',
  `SelectID` tinyint(2) NOT NULL COMMENT '是否激活（当前选择角色）',
  `IsDeleted` tinyint(2) NOT NULL COMMENT '是否删除（0:正常 1:删除 2:创建角色异常）',
  `CreateDate` datetime NOT NULL COMMENT '创建时间',
  `ModifyDate` datetime NOT NULL COMMENT '修改时间',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=12218 DEFAULT CHARSET=utf8 COMMENT='角色索引';

-- ----------------------------
-- Table structure for characters_item
-- ----------------------------
DROP TABLE IF EXISTS `characters_item`;
CREATE TABLE `characters_item` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `PlayerId` bigint(20) NOT NULL COMMENT '角色ID',
  `Position` int(10) NOT NULL DEFAULT '0' COMMENT '穿戴位置',
  `MakeIndex` int(20) NOT NULL DEFAULT '0' COMMENT '物品唯一ID',
  `StdIndex` int(10) NOT NULL DEFAULT '0' COMMENT '物品编号',
  `Dura` int(10) NOT NULL DEFAULT '0' COMMENT '当前持久',
  `DuraMax` int(10) NOT NULL DEFAULT '0' COMMENT '最大持久',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=77936 DEFAULT CHARSET=utf8 COMMENT='穿戴物品';

-- ----------------------------
-- Table structure for characters_item_attr
-- ----------------------------
DROP TABLE IF EXISTS `characters_item_attr`;
CREATE TABLE `characters_item_attr` (
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
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='装备附加属性';

-- ----------------------------
-- Table structure for characters_magic
-- ----------------------------
DROP TABLE IF EXISTS `characters_magic`;
CREATE TABLE `characters_magic` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `PlayerId` bigint(20) NOT NULL,
  `MagicId` int(11) NOT NULL COMMENT '技能ID',
  `Level` tinyint(2) NOT NULL COMMENT '技能等级',
  `UseKey` char(2) NOT NULL COMMENT '技能按键',
  `CurrTrain` int(11) NOT NULL COMMENT '当前经验',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=354 DEFAULT CHARSET=utf8 COMMENT='技能';

-- ----------------------------
-- Table structure for characters_quest
-- ----------------------------
DROP TABLE IF EXISTS `characters_quest`;
CREATE TABLE `characters_quest` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `PLAYERID` int(11) DEFAULT NULL,
  `QUESTOPENINDEX` varchar(255) DEFAULT NULL,
  `QUESTFININDEX` varchar(255) DEFAULT NULL,
  `QUEST` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='任务标志';

-- ----------------------------
-- Table structure for characters_status
-- ----------------------------
DROP TABLE IF EXISTS `characters_status`;
CREATE TABLE `characters_status` (
  `PlayerId` bigint(20) NOT NULL,
  `Status0` int(12) DEFAULT '0',
  `Status1` int(12) DEFAULT '0',
  `Status2` int(12) DEFAULT '0',
  `Status3` int(12) DEFAULT '0',
  `Status4` int(12) DEFAULT '0',
  `Status5` int(12) DEFAULT '0',
  `Status6` int(12) DEFAULT '0',
  `Status7` int(12) DEFAULT '0',
  `Status8` int(12) DEFAULT '0',
  `Status9` int(12) DEFAULT '0',
  `Status10` int(12) DEFAULT '0',
  `Status11` int(12) DEFAULT '0',
  `Status12` int(12) DEFAULT '0',
  `Status13` int(12) DEFAULT '0',
  `Status14` int(12) DEFAULT '0',
  `Status15` int(12) DEFAULT '0',
  PRIMARY KEY (`PlayerId`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='人物状态值';

-- ----------------------------
-- Table structure for characters_storageitem
-- ----------------------------
DROP TABLE IF EXISTS `characters_storageitem`;
CREATE TABLE `characters_storageitem` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `PlayerId` bigint(20) NOT NULL COMMENT '角色ID',
  `Position` int(10) NOT NULL DEFAULT '0' COMMENT '位置',
  `MakeIndex` int(20) NOT NULL DEFAULT '0' COMMENT '物品唯一ID',
  `StdIndex` int(10) NOT NULL DEFAULT '0' COMMENT '物品编号',
  `Dura` int(10) NOT NULL DEFAULT '0' COMMENT '当前持久',
  `DuraMax` int(10) NOT NULL DEFAULT '0' COMMENT '最大持久',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=299751 DEFAULT CHARSET=utf8 COMMENT='角色仓库物品';

SET FOREIGN_KEY_CHECKS = 1;
