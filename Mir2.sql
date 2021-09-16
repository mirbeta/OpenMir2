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

 Date: 17/09/2021 00:18:57
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for TBL_ACCOUNT
-- ----------------------------
DROP TABLE IF EXISTS `TBL_ACCOUNT`;
CREATE TABLE `TBL_ACCOUNT` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_LOGINID` varchar(50) DEFAULT NULL,
  `FLD_PASSWORD` varchar(50) DEFAULT NULL,
  `FLD_USERNAME` varchar(255) DEFAULT NULL,
  `FLD_ACTIONTICK` int(5) DEFAULT NULL,
  `FLD_ERRORCOUNT` int(5) DEFAULT NULL,
  `FLD_SSNO` varchar(50) DEFAULT NULL,
  `FLD_PHONE` varchar(50) DEFAULT NULL,
  `FLD_QUIZ1` varchar(50) DEFAULT NULL,
  `FLD_ANSWER1` varchar(50) DEFAULT NULL,
  `FLD_EMAIL` varchar(50) DEFAULT NULL,
  `FLD_QUIZ2` varchar(50) DEFAULT NULL,
  `FLD_ANSWER2` varchar(50) DEFAULT NULL,
  `FLD_BIRTHDAY` varchar(50) DEFAULT NULL,
  `FLD_MOBILEPHONE` varchar(50) DEFAULT NULL,
  `FLD_DELETED` tinyint(2) DEFAULT NULL,
  `FLD_CREATEDATE` datetime DEFAULT NULL,
  `FLD_LASTUPDATE` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=549 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for TBL_BONUSABILITY
-- ----------------------------
DROP TABLE IF EXISTS `TBL_BONUSABILITY`;
CREATE TABLE `TBL_BONUSABILITY` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_CHARNAME` varchar(255) DEFAULT NULL,
  `FLD_AC` varchar(255) DEFAULT NULL,
  `FLD_MAC` varchar(255) DEFAULT NULL,
  `FLD_DC` varchar(255) DEFAULT NULL,
  `FLD_MC` varchar(255) DEFAULT NULL,
  `FLD_SC` varchar(255) DEFAULT NULL,
  `FLD_HP` varchar(255) DEFAULT NULL,
  `FLD_MP` varchar(255) DEFAULT NULL,
  `FLD_HIT` varchar(255) DEFAULT NULL,
  `FLD_SPEED` double DEFAULT NULL,
  `FLD_RESERVED` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for TBL_CHARACTER
-- ----------------------------
DROP TABLE IF EXISTS `TBL_CHARACTER`;
CREATE TABLE `TBL_CHARACTER` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_LoginID` bigint(11) DEFAULT NULL,
  `FLD_CharName` varchar(255) DEFAULT NULL,
  `FLD_MapName` varchar(255) DEFAULT NULL,
  `FLD_CX` int(255) DEFAULT NULL,
  `FLD_CY` int(255) DEFAULT NULL,
  `FLD_Level` int(255) DEFAULT NULL,
  `FLD_Dir` int(255) DEFAULT NULL,
  `FLD_Hair` int(255) DEFAULT NULL,
  `FLD_Sex` int(255) DEFAULT NULL,
  `FLD_Job` int(255) DEFAULT NULL,
  `FLD_Gold` int(255) DEFAULT NULL COMMENT '金币数',
  `FLD_GamePoint` int(255) DEFAULT NULL,
  `FLD_HomeMap` varchar(255) DEFAULT NULL,
  `FLD_HomeX` int(255) DEFAULT NULL,
  `FLD_HomeY` int(255) DEFAULT NULL,
  `FLD_PkPoint` int(5) DEFAULT NULL COMMENT 'PK值',
  `FLD_ReLevel` tinyint(2) DEFAULT NULL COMMENT '转生次数',
  `FLD_AttatckMode` tinyint(2) DEFAULT NULL COMMENT '攻击模式',
  `FLD_FightZoneDie` varchar(255) DEFAULT NULL,
  `FLD_FightZoneDieCount` varchar(255) DEFAULT NULL COMMENT '行会战争死亡次数',
  `FLD_BodyLuck` double(10,3) DEFAULT NULL COMMENT '幸运值',
  `FLD_IncHealth` int(5) DEFAULT NULL,
  `FLD_IncSpell` int(5) DEFAULT NULL,
  `FLD_IncHealing` int(5) DEFAULT NULL,
  `FLD_CreditPoint` int(5) DEFAULT NULL COMMENT '声望点数',
  `FLD_BonusPoint` int(5) DEFAULT NULL,
  `FLD_HungerStatus` int(5) DEFAULT NULL,
  `FLD_PayMentPoint` int(5) DEFAULT NULL,
  `FLD_LockLogon` tinyint(1) DEFAULT NULL COMMENT '是否锁定登陆',
  `FLD_MarryCount` int(5) DEFAULT NULL COMMENT '结婚次数',
  `FLD_AllowGroupReCall` tinyint(1) DEFAULT NULL COMMENT '是否允许组队传送',
  `FLD_AllowGuildReCall` tinyint(1) DEFAULT NULL COMMENT '是否允许行会传送',
  `FLD_IsMaster` tinyint(1) DEFAULT NULL,
  `FLD_MasterName` varchar(50) DEFAULT NULL,
  `FLD_DearName` varchar(50) DEFAULT NULL,
  `FLD_StoragePwd` varchar(50) DEFAULT NULL,
  `FLD_Deleted` tinyint(1) DEFAULT NULL,
  `FLD_CREATEDATE` datetime DEFAULT NULL,
  `FLD_LASTUPDATE` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for TBL_ITEM
-- ----------------------------
DROP TABLE IF EXISTS `TBL_ITEM`;
CREATE TABLE `TBL_ITEM` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_CHARNAME` varchar(50) DEFAULT NULL,
  `FLD_POSITION` int(10) DEFAULT NULL,
  `FLD_MAKEINDEX` int(20) DEFAULT NULL,
  `FLD_STDINDEX` int(10) DEFAULT NULL,
  `FLD_DURA` int(10) DEFAULT NULL,
  `FLD_DURAMAX` int(10) DEFAULT NULL,
  `FLD_VALUE0` int(10) DEFAULT NULL,
  `FLD_VALUE1` int(10) DEFAULT NULL,
  `FLD_VALUE2` int(10) DEFAULT NULL,
  `FLD_VALUE3` int(10) DEFAULT NULL,
  `FLD_VALUE4` int(10) DEFAULT NULL,
  `FLD_VALUE5` int(10) DEFAULT NULL,
  `FLD_VALUE6` int(10) DEFAULT NULL,
  `FLD_VALUE7` int(10) DEFAULT NULL,
  `FLD_VALUE8` int(10) DEFAULT NULL,
  `FLD_VALUE9` int(10) DEFAULT NULL,
  `FLD_VALUE10` int(10) DEFAULT NULL,
  `FLD_VALUE11` int(10) DEFAULT NULL,
  `FLD_VALUE12` int(10) DEFAULT NULL,
  `FLD_VALUE13` int(10) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for TBL_MAGIC
-- ----------------------------
DROP TABLE IF EXISTS `TBL_MAGIC`;
CREATE TABLE `TBL_MAGIC` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_CHARNAME` varchar(255) DEFAULT NULL,
  `FLD_MAGICID` int(11) DEFAULT NULL,
  `FLD_TYPE` varchar(255) DEFAULT NULL,
  `FLD_LEVEL` varchar(255) DEFAULT NULL,
  `FLD_USEKEY` varchar(255) DEFAULT NULL,
  `FLD_CURRTRAIN` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for TBL_QUEST
-- ----------------------------
DROP TABLE IF EXISTS `TBL_QUEST`;
CREATE TABLE `TBL_QUEST` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_CHARNAME` varchar(255) DEFAULT NULL,
  `FLD_QUESTOPENINDEX` varchar(255) DEFAULT NULL,
  `FLD_QUESTFININDEX` varchar(255) DEFAULT NULL,
  `FLD_QUEST` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for TBL_STORAGE
-- ----------------------------
DROP TABLE IF EXISTS `TBL_STORAGE`;
CREATE TABLE `TBL_STORAGE` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `FLD_CHARNAME` varchar(50) DEFAULT NULL,
  `FLD_POSITION` int(10) DEFAULT NULL,
  `FLD_MAKEINDEX` int(20) DEFAULT NULL,
  `FLD_STDINDEX` int(10) DEFAULT NULL,
  `FLD_DURA` int(10) DEFAULT NULL,
  `FLD_DURAMAX` int(10) DEFAULT NULL,
  `FLD_VALUE0` int(10) DEFAULT NULL,
  `FLD_VALUE1` int(10) DEFAULT NULL,
  `FLD_VALUE2` int(10) DEFAULT NULL,
  `FLD_VALUE3` int(10) DEFAULT NULL,
  `FLD_VALUE4` int(10) DEFAULT NULL,
  `FLD_VALUE5` int(10) DEFAULT NULL,
  `FLD_VALUE6` int(10) DEFAULT NULL,
  `FLD_VALUE7` int(10) DEFAULT NULL,
  `FLD_VALUE8` int(10) DEFAULT NULL,
  `FLD_VALUE9` int(10) DEFAULT NULL,
  `FLD_VALUE10` int(10) DEFAULT NULL,
  `FLD_VALUE11` int(10) DEFAULT NULL,
  `FLD_VALUE12` int(10) DEFAULT NULL,
  `FLD_VALUE13` int(10) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

SET FOREIGN_KEY_CHECKS = 1;
