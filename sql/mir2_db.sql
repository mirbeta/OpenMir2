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

 Date: 10/10/2022 00:09:51
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
) ENGINE=InnoDB AUTO_INCREMENT=6227 DEFAULT CHARSET=utf8 COMMENT='角色信息';

-- ----------------------------
-- Records of characters
-- ----------------------------
BEGIN;
INSERT INTO `characters` (`Id`, `ServerIndex`, `LoginID`, `ChrName`, `MapName`, `CX`, `CY`, `Level`, `Dir`, `Hair`, `Sex`, `Job`, `Gold`, `GamePoint`, `HomeMap`, `HomeX`, `HomeY`, `PkPoint`, `ReLevel`, `AttatckMode`, `FightZoneDieCount`, `BodyLuck`, `IncHealth`, `IncSpell`, `IncHealing`, `CreditPoint`, `BonusPoint`, `HungerStatus`, `PayMentPoint`, `LockLogon`, `MarryCount`, `AllowGroup`, `AllowGroupReCall`, `GroupRcallTime`, `AllowGuildReCall`, `IsMaster`, `MasterName`, `DearName`, `StoragePwd`, `Deleted`, `CREATEDATE`, `LASTUPDATE`) VALUES (1, 0, 'admin', 'gm01', '3', 327, 332, 50, 2, 1, 0, 0, 890095, 0, '3', 330, 330, 0, 0, 0, 0, -492.920, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, 0, '2022-08-03 01:33:16', '2022-10-09 23:18:55');
INSERT INTO `characters` (`Id`, `ServerIndex`, `LoginID`, `ChrName`, `MapName`, `CX`, `CY`, `Level`, `Dir`, `Hair`, `Sex`, `Job`, `Gold`, `GamePoint`, `HomeMap`, `HomeX`, `HomeY`, `PkPoint`, `ReLevel`, `AttatckMode`, `FightZoneDieCount`, `BodyLuck`, `IncHealth`, `IncSpell`, `IncHealing`, `CreditPoint`, `BonusPoint`, `HungerStatus`, `PayMentPoint`, `LockLogon`, `MarryCount`, `AllowGroup`, `AllowGroupReCall`, `GroupRcallTime`, `AllowGuildReCall`, `IsMaster`, `MasterName`, `DearName`, `StoragePwd`, `Deleted`, `CREATEDATE`, `LASTUPDATE`) VALUES (6221, 0, 'admin', '深山老毒', '0', 607, 584, 1, 4, 1, 0, 2, 0, 0, '0', 289, 618, 0, 0, 0, 0, 0.000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, 0, '2022-09-13 13:28:09', '2022-10-09 20:06:59');
INSERT INTO `characters` (`Id`, `ServerIndex`, `LoginID`, `ChrName`, `MapName`, `CX`, `CY`, `Level`, `Dir`, `Hair`, `Sex`, `Job`, `Gold`, `GamePoint`, `HomeMap`, `HomeX`, `HomeY`, `PkPoint`, `ReLevel`, `AttatckMode`, `FightZoneDieCount`, `BodyLuck`, `IncHealth`, `IncSpell`, `IncHealing`, `CreditPoint`, `BonusPoint`, `HungerStatus`, `PayMentPoint`, `LockLogon`, `MarryCount`, `AllowGroup`, `AllowGroupReCall`, `GroupRcallTime`, `AllowGuildReCall`, `IsMaster`, `MasterName`, `DearName`, `StoragePwd`, `Deleted`, `CREATEDATE`, `LASTUPDATE`) VALUES (6222, 0, 'admin', '123456', '0', 294, 651, 1, 4, 1, 0, 1, 0, 0, '0', 289, 618, 0, 0, 0, 0, 0.000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, 0, '2022-10-09 20:13:29', '2022-10-09 20:15:18');
INSERT INTO `characters` (`Id`, `ServerIndex`, `LoginID`, `ChrName`, `MapName`, `CX`, `CY`, `Level`, `Dir`, `Hair`, `Sex`, `Job`, `Gold`, `GamePoint`, `HomeMap`, `HomeX`, `HomeY`, `PkPoint`, `ReLevel`, `AttatckMode`, `FightZoneDieCount`, `BodyLuck`, `IncHealth`, `IncSpell`, `IncHealing`, `CreditPoint`, `BonusPoint`, `HungerStatus`, `PayMentPoint`, `LockLogon`, `MarryCount`, `AllowGroup`, `AllowGroupReCall`, `GroupRcallTime`, `AllowGuildReCall`, `IsMaster`, `MasterName`, `DearName`, `StoragePwd`, `Deleted`, `CREATEDATE`, `LASTUPDATE`) VALUES (6223, 0, 'admin', 'gm02', '0141', 4, 11, 1, 3, 1, 0, 2, 0, 0, '0', 289, 618, 0, 0, 0, 0, 0.000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, 0, '2022-10-09 20:54:56', '2022-10-09 20:56:34');
INSERT INTO `characters` (`Id`, `ServerIndex`, `LoginID`, `ChrName`, `MapName`, `CX`, `CY`, `Level`, `Dir`, `Hair`, `Sex`, `Job`, `Gold`, `GamePoint`, `HomeMap`, `HomeX`, `HomeY`, `PkPoint`, `ReLevel`, `AttatckMode`, `FightZoneDieCount`, `BodyLuck`, `IncHealth`, `IncSpell`, `IncHealing`, `CreditPoint`, `BonusPoint`, `HungerStatus`, `PayMentPoint`, `LockLogon`, `MarryCount`, `AllowGroup`, `AllowGroupReCall`, `GroupRcallTime`, `AllowGuildReCall`, `IsMaster`, `MasterName`, `DearName`, `StoragePwd`, `Deleted`, `CREATEDATE`, `LASTUPDATE`) VALUES (6224, 0, 'admin', 'gm03', '0', 649, 630, 1, 0, 1, 0, 2, 0, 0, '0', 289, 618, 0, 0, 0, 0, 0.000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, 0, '2022-10-09 20:56:43', '2022-10-09 20:57:15');
INSERT INTO `characters` (`Id`, `ServerIndex`, `LoginID`, `ChrName`, `MapName`, `CX`, `CY`, `Level`, `Dir`, `Hair`, `Sex`, `Job`, `Gold`, `GamePoint`, `HomeMap`, `HomeX`, `HomeY`, `PkPoint`, `ReLevel`, `AttatckMode`, `FightZoneDieCount`, `BodyLuck`, `IncHealth`, `IncSpell`, `IncHealing`, `CreditPoint`, `BonusPoint`, `HungerStatus`, `PayMentPoint`, `LockLogon`, `MarryCount`, `AllowGroup`, `AllowGroupReCall`, `GroupRcallTime`, `AllowGuildReCall`, `IsMaster`, `MasterName`, `DearName`, `StoragePwd`, `Deleted`, `CREATEDATE`, `LASTUPDATE`) VALUES (6225, 0, 'admin', 'gm04', '0132', 11, 12, 1, 4, 1, 0, 2, 0, 0, '0', 289, 618, 0, 0, 0, 0, 0.000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, 0, '2022-10-09 20:58:08', '2022-10-09 21:27:39');
INSERT INTO `characters` (`Id`, `ServerIndex`, `LoginID`, `ChrName`, `MapName`, `CX`, `CY`, `Level`, `Dir`, `Hair`, `Sex`, `Job`, `Gold`, `GamePoint`, `HomeMap`, `HomeX`, `HomeY`, `PkPoint`, `ReLevel`, `AttatckMode`, `FightZoneDieCount`, `BodyLuck`, `IncHealth`, `IncSpell`, `IncHealing`, `CreditPoint`, `BonusPoint`, `HungerStatus`, `PayMentPoint`, `LockLogon`, `MarryCount`, `AllowGroup`, `AllowGroupReCall`, `GroupRcallTime`, `AllowGuildReCall`, `IsMaster`, `MasterName`, `DearName`, `StoragePwd`, `Deleted`, `CREATEDATE`, `LASTUPDATE`) VALUES (6226, 0, 'admin', '7788', '0125', 8, 15, 1, 0, 1, 0, 2, 0, 0, '0', 289, 618, 0, 0, 0, 0, 0.000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, 0, '2022-10-09 23:50:30', '2022-10-10 00:00:01');
COMMIT;

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
-- Records of characters_ablity
-- ----------------------------
BEGIN;
INSERT INTO `characters_ablity` (`PlayerId`, `Level`, `Ac`, `Mac`, `Dc`, `Mc`, `Sc`, `Hp`, `Mp`, `MaxHP`, `MAxMP`, `Exp`, `MaxExp`, `Weight`, `MaxWeight`, `WearWeight`, `MaxWearWeight`, `HandWeight`, `MaxHandWeight`, `ModifyTime`) VALUES (1, 50, 50, 0, 0, 0, 0, 939, 186, 939, 186, 3540, 200000000, 0, 850, 92, 115, 80, 162, '2022-10-09 23:18:55');
INSERT INTO `characters_ablity` (`PlayerId`, `Level`, `Ac`, `Mac`, `Dc`, `Mc`, `Sc`, `Hp`, `Mp`, `MaxHP`, `MAxMP`, `Exp`, `MaxExp`, `Weight`, `MaxWeight`, `WearWeight`, `MaxWearWeight`, `HandWeight`, `MaxHandWeight`, `ModifyTime`) VALUES (6221, 1, 1, 0, 0, 0, 0, 16, 17, 16, 17, 0, 10, 0, 50, 0, 15, 0, 12, '2022-10-09 20:06:59');
INSERT INTO `characters_ablity` (`PlayerId`, `Level`, `Ac`, `Mac`, `Dc`, `Mc`, `Sc`, `Hp`, `Mp`, `MaxHP`, `MAxMP`, `Exp`, `MaxExp`, `Weight`, `MaxWeight`, `WearWeight`, `MaxWearWeight`, `HandWeight`, `MaxHandWeight`, `ModifyTime`) VALUES (6222, 1, 1, 0, 0, 0, 0, 13, 17, 16, 17, 0, 10, 0, 50, 0, 15, 0, 12, '2022-10-09 20:15:18');
INSERT INTO `characters_ablity` (`PlayerId`, `Level`, `Ac`, `Mac`, `Dc`, `Mc`, `Sc`, `Hp`, `Mp`, `MaxHP`, `MAxMP`, `Exp`, `MaxExp`, `Weight`, `MaxWeight`, `WearWeight`, `MaxWearWeight`, `HandWeight`, `MaxHandWeight`, `ModifyTime`) VALUES (6223, 1, 1, 0, 0, 0, 0, 6, 16, 16, 17, 0, 10, 0, 50, 0, 15, 0, 12, '2022-10-09 20:56:34');
INSERT INTO `characters_ablity` (`PlayerId`, `Level`, `Ac`, `Mac`, `Dc`, `Mc`, `Sc`, `Hp`, `Mp`, `MaxHP`, `MAxMP`, `Exp`, `MaxExp`, `Weight`, `MaxWeight`, `WearWeight`, `MaxWearWeight`, `HandWeight`, `MaxHandWeight`, `ModifyTime`) VALUES (6224, 1, 1, 0, 0, 0, 0, 16, 16, 16, 17, 0, 10, 14, 50, 0, 15, 0, 12, '2022-10-09 20:57:15');
INSERT INTO `characters_ablity` (`PlayerId`, `Level`, `Ac`, `Mac`, `Dc`, `Mc`, `Sc`, `Hp`, `Mp`, `MaxHP`, `MAxMP`, `Exp`, `MaxExp`, `Weight`, `MaxWeight`, `WearWeight`, `MaxWearWeight`, `HandWeight`, `MaxHandWeight`, `ModifyTime`) VALUES (6225, 1, 1, 0, 0, 0, 0, 17, 13, 17, 13, 0, 10, 0, 50, 0, 15, 0, 12, '2022-10-09 21:27:39');
INSERT INTO `characters_ablity` (`PlayerId`, `Level`, `Ac`, `Mac`, `Dc`, `Mc`, `Sc`, `Hp`, `Mp`, `MaxHP`, `MAxMP`, `Exp`, `MaxExp`, `Weight`, `MaxWeight`, `WearWeight`, `MaxWearWeight`, `HandWeight`, `MaxHandWeight`, `ModifyTime`) VALUES (6226, 1, 1, 0, 0, 0, 0, 17, 13, 17, 13, 0, 10, 62, 50, 5, 15, 8, 12, '2022-10-10 00:00:01');
COMMIT;

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
) ENGINE=InnoDB AUTO_INCREMENT=323 DEFAULT CHARSET=utf8 COMMENT='包裹';

-- ----------------------------
-- Records of characters_bagitem
-- ----------------------------
BEGIN;
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (1, 1, 0, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (2, 1, 1, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (3, 1, 2, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (4, 1, 3, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (5, 1, 4, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (6, 1, 5, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (7, 1, 6, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (8, 1, 7, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (9, 1, 8, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (10, 1, 9, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (11, 1, 10, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (12, 1, 11, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (13, 1, 12, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (14, 1, 13, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (15, 1, 14, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (16, 1, 15, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (17, 1, 16, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (18, 1, 17, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (19, 1, 18, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (20, 1, 19, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (21, 1, 20, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (22, 1, 21, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (23, 1, 22, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (24, 1, 23, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (25, 1, 24, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (26, 1, 25, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (27, 1, 26, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (28, 1, 27, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (29, 1, 28, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (30, 1, 29, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (31, 1, 30, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (32, 1, 31, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (33, 1, 32, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (34, 1, 33, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (35, 1, 34, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (36, 1, 35, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (37, 1, 36, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (38, 1, 37, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (39, 1, 38, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (40, 1, 39, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (41, 1, 40, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (42, 1, 41, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (43, 1, 42, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (44, 1, 43, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (45, 1, 44, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (46, 1, 45, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (47, 6221, 0, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (48, 6221, 1, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (49, 6221, 2, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (50, 6221, 3, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (51, 6221, 4, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (52, 6221, 5, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (53, 6221, 6, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (54, 6221, 7, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (55, 6221, 8, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (56, 6221, 9, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (57, 6221, 10, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (58, 6221, 11, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (59, 6221, 12, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (60, 6221, 13, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (61, 6221, 14, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (62, 6221, 15, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (63, 6221, 16, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (64, 6221, 17, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (65, 6221, 18, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (66, 6221, 19, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (67, 6221, 20, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (68, 6221, 21, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (69, 6221, 22, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (70, 6221, 23, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (71, 6221, 24, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (72, 6221, 25, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (73, 6221, 26, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (74, 6221, 27, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (75, 6221, 28, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (76, 6221, 29, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (77, 6221, 30, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (78, 6221, 31, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (79, 6221, 32, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (80, 6221, 33, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (81, 6221, 34, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (82, 6221, 35, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (83, 6221, 36, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (84, 6221, 37, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (85, 6221, 38, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (86, 6221, 39, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (87, 6221, 40, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (88, 6221, 41, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (89, 6221, 42, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (90, 6221, 43, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (91, 6221, 44, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (92, 6221, 45, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (93, 6222, 0, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (94, 6222, 1, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (95, 6222, 2, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (96, 6222, 3, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (97, 6222, 4, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (98, 6222, 5, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (99, 6222, 6, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (100, 6222, 7, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (101, 6222, 8, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (102, 6222, 9, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (103, 6222, 10, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (104, 6222, 11, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (105, 6222, 12, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (106, 6222, 13, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (107, 6222, 14, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (108, 6222, 15, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (109, 6222, 16, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (110, 6222, 17, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (111, 6222, 18, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (112, 6222, 19, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (113, 6222, 20, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (114, 6222, 21, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (115, 6222, 22, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (116, 6222, 23, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (117, 6222, 24, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (118, 6222, 25, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (119, 6222, 26, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (120, 6222, 27, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (121, 6222, 28, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (122, 6222, 29, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (123, 6222, 30, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (124, 6222, 31, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (125, 6222, 32, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (126, 6222, 33, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (127, 6222, 34, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (128, 6222, 35, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (129, 6222, 36, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (130, 6222, 37, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (131, 6222, 38, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (132, 6222, 39, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (133, 6222, 40, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (134, 6222, 41, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (135, 6222, 42, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (136, 6222, 43, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (137, 6222, 44, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (138, 6222, 45, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (139, 6223, 0, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (140, 6223, 1, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (141, 6223, 2, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (142, 6223, 3, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (143, 6223, 4, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (144, 6223, 5, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (145, 6223, 6, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (146, 6223, 7, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (147, 6223, 8, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (148, 6223, 9, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (149, 6223, 10, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (150, 6223, 11, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (151, 6223, 12, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (152, 6223, 13, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (153, 6223, 14, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (154, 6223, 15, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (155, 6223, 16, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (156, 6223, 17, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (157, 6223, 18, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (158, 6223, 19, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (159, 6223, 20, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (160, 6223, 21, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (161, 6223, 22, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (162, 6223, 23, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (163, 6223, 24, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (164, 6223, 25, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (165, 6223, 26, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (166, 6223, 27, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (167, 6223, 28, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (168, 6223, 29, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (169, 6223, 30, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (170, 6223, 31, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (171, 6223, 32, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (172, 6223, 33, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (173, 6223, 34, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (174, 6223, 35, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (175, 6223, 36, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (176, 6223, 37, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (177, 6223, 38, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (178, 6223, 39, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (179, 6223, 40, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (180, 6223, 41, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (181, 6223, 42, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (182, 6223, 43, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (183, 6223, 44, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (184, 6223, 45, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (185, 6224, 0, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (186, 6224, 1, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (187, 6224, 2, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (188, 6224, 3, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (189, 6224, 4, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (190, 6224, 5, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (191, 6224, 6, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (192, 6224, 7, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (193, 6224, 8, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (194, 6224, 9, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (195, 6224, 10, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (196, 6224, 11, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (197, 6224, 12, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (198, 6224, 13, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (199, 6224, 14, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (200, 6224, 15, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (201, 6224, 16, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (202, 6224, 17, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (203, 6224, 18, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (204, 6224, 19, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (205, 6224, 20, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (206, 6224, 21, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (207, 6224, 22, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (208, 6224, 23, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (209, 6224, 24, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (210, 6224, 25, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (211, 6224, 26, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (212, 6224, 27, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (213, 6224, 28, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (214, 6224, 29, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (215, 6224, 30, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (216, 6224, 31, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (217, 6224, 32, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (218, 6224, 33, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (219, 6224, 34, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (220, 6224, 35, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (221, 6224, 36, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (222, 6224, 37, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (223, 6224, 38, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (224, 6224, 39, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (225, 6224, 40, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (226, 6224, 41, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (227, 6224, 42, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (228, 6224, 43, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (229, 6224, 44, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (230, 6224, 45, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (231, 6225, 0, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (232, 6225, 1, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (233, 6225, 2, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (234, 6225, 3, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (235, 6225, 4, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (236, 6225, 5, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (237, 6225, 6, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (238, 6225, 7, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (239, 6225, 8, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (240, 6225, 9, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (241, 6225, 10, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (242, 6225, 11, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (243, 6225, 12, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (244, 6225, 13, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (245, 6225, 14, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (246, 6225, 15, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (247, 6225, 16, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (248, 6225, 17, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (249, 6225, 18, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (250, 6225, 19, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (251, 6225, 20, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (252, 6225, 21, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (253, 6225, 22, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (254, 6225, 23, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (255, 6225, 24, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (256, 6225, 25, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (257, 6225, 26, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (258, 6225, 27, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (259, 6225, 28, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (260, 6225, 29, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (261, 6225, 30, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (262, 6225, 31, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (263, 6225, 32, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (264, 6225, 33, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (265, 6225, 34, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (266, 6225, 35, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (267, 6225, 36, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (268, 6225, 37, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (269, 6225, 38, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (270, 6225, 39, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (271, 6225, 40, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (272, 6225, 41, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (273, 6225, 42, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (274, 6225, 43, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (275, 6225, 44, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (276, 6225, 45, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (277, 6226, 0, 1073741873, 115, 35000, 35000);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (278, 6226, 1, 1073741875, 117, 30000, 30000);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (279, 6226, 2, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (280, 6226, 3, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (281, 6226, 4, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (282, 6226, 5, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (283, 6226, 6, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (284, 6226, 7, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (285, 6226, 8, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (286, 6226, 9, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (287, 6226, 10, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (288, 6226, 11, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (289, 6226, 12, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (290, 6226, 13, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (291, 6226, 14, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (292, 6226, 15, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (293, 6226, 16, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (294, 6226, 17, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (295, 6226, 18, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (296, 6226, 19, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (297, 6226, 20, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (298, 6226, 21, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (299, 6226, 22, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (300, 6226, 23, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (301, 6226, 24, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (302, 6226, 25, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (303, 6226, 26, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (304, 6226, 27, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (305, 6226, 28, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (306, 6226, 29, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (307, 6226, 30, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (308, 6226, 31, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (309, 6226, 32, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (310, 6226, 33, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (311, 6226, 34, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (312, 6226, 35, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (313, 6226, 36, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (314, 6226, 37, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (315, 6226, 38, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (316, 6226, 39, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (317, 6226, 40, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (318, 6226, 41, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (319, 6226, 42, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (320, 6226, 43, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (321, 6226, 44, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (322, 6226, 45, 0, 0, 0, 0);
COMMIT;

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
-- Records of characters_bonusability
-- ----------------------------
BEGIN;
COMMIT;

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
) ENGINE=InnoDB AUTO_INCREMENT=6230 DEFAULT CHARSET=utf8 COMMENT='角色索引';

-- ----------------------------
-- Records of characters_indexes
-- ----------------------------
BEGIN;
INSERT INTO `characters_indexes` (`Id`, `Account`, `ChrName`, `SelectID`, `IsDeleted`, `CreateDate`, `ModifyDate`) VALUES (1, 'admin', 'gm01', 0, 0, '2022-08-03 01:33:16', '2022-10-09 23:50:45');
INSERT INTO `characters_indexes` (`Id`, `Account`, `ChrName`, `SelectID`, `IsDeleted`, `CreateDate`, `ModifyDate`) VALUES (6221, 'admin', '深山老毒', 0, 1, '2022-09-13 13:28:09', '2022-10-09 20:13:16');
INSERT INTO `characters_indexes` (`Id`, `Account`, `ChrName`, `SelectID`, `IsDeleted`, `CreateDate`, `ModifyDate`) VALUES (6222, 'admin', '123456', 0, 1, '2022-10-09 20:13:29', '2022-10-09 20:55:00');
INSERT INTO `characters_indexes` (`Id`, `Account`, `ChrName`, `SelectID`, `IsDeleted`, `CreateDate`, `ModifyDate`) VALUES (6223, 'admin', 'gm02', 0, 1, '2022-10-09 20:54:56', '2022-10-09 20:56:46');
INSERT INTO `characters_indexes` (`Id`, `Account`, `ChrName`, `SelectID`, `IsDeleted`, `CreateDate`, `ModifyDate`) VALUES (6224, 'admin', 'gm03', 0, 1, '2022-10-09 20:56:43', '2022-10-09 20:58:10');
INSERT INTO `characters_indexes` (`Id`, `Account`, `ChrName`, `SelectID`, `IsDeleted`, `CreateDate`, `ModifyDate`) VALUES (6225, 'admin', 'gm04', 0, 1, '2022-10-09 20:57:37', '2022-10-09 22:30:58');
INSERT INTO `characters_indexes` (`Id`, `Account`, `ChrName`, `SelectID`, `IsDeleted`, `CreateDate`, `ModifyDate`) VALUES (6226, 'admin', '7788', 0, 2, '2022-10-09 22:30:49', '2022-10-09 22:30:49');
INSERT INTO `characters_indexes` (`Id`, `Account`, `ChrName`, `SelectID`, `IsDeleted`, `CreateDate`, `ModifyDate`) VALUES (6227, 'admin', '7788', 0, 2, '2022-10-09 23:42:40', '2022-10-09 23:42:40');
INSERT INTO `characters_indexes` (`Id`, `Account`, `ChrName`, `SelectID`, `IsDeleted`, `CreateDate`, `ModifyDate`) VALUES (6228, 'admin', '7788', 0, 2, '2022-10-09 23:47:10', '2022-10-09 23:47:10');
INSERT INTO `characters_indexes` (`Id`, `Account`, `ChrName`, `SelectID`, `IsDeleted`, `CreateDate`, `ModifyDate`) VALUES (6229, 'admin', '7788', 1, 0, '2022-10-09 23:50:17', '2022-10-10 00:00:04');
COMMIT;

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
) ENGINE=InnoDB AUTO_INCREMENT=92 DEFAULT CHARSET=utf8 COMMENT='穿戴物品';

-- ----------------------------
-- Records of characters_item
-- ----------------------------
BEGIN;
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (1, 1, 0, 1073741879, 115, 33680, 35000);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (2, 1, 1, 1073741873, 223, 31795, 32000);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (3, 1, 2, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (4, 1, 3, 1073741878, 628, 6809, 7000);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (5, 1, 4, 1073741877, 629, 6878, 7000);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (6, 1, 5, 1073741875, 630, 6842, 7000);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (7, 1, 6, 1073741876, 630, 6839, 7000);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (8, 1, 7, 1073741873, 631, 6865, 7000);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (9, 1, 8, 1073741874, 631, 6841, 7000);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (10, 1, 9, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (11, 1, 10, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (12, 1, 11, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (13, 1, 12, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (14, 6221, 0, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (15, 6221, 1, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (16, 6221, 2, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (17, 6221, 3, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (18, 6221, 4, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (19, 6221, 5, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (20, 6221, 6, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (21, 6221, 7, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (22, 6221, 8, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (23, 6221, 9, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (24, 6221, 10, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (25, 6221, 11, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (26, 6221, 12, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (27, 6222, 0, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (28, 6222, 1, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (29, 6222, 2, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (30, 6222, 3, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (31, 6222, 4, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (32, 6222, 5, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (33, 6222, 6, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (34, 6222, 7, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (35, 6222, 8, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (36, 6222, 9, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (37, 6222, 10, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (38, 6222, 11, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (39, 6222, 12, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (40, 6223, 0, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (41, 6223, 1, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (42, 6223, 2, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (43, 6223, 3, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (44, 6223, 4, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (45, 6223, 5, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (46, 6223, 6, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (47, 6223, 7, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (48, 6223, 8, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (49, 6223, 9, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (50, 6223, 10, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (51, 6223, 11, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (52, 6223, 12, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (53, 6224, 0, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (54, 6224, 1, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (55, 6224, 2, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (56, 6224, 3, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (57, 6224, 4, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (58, 6224, 5, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (59, 6224, 6, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (60, 6224, 7, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (61, 6224, 8, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (62, 6224, 9, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (63, 6224, 10, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (64, 6224, 11, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (65, 6224, 12, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (66, 6225, 0, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (67, 6225, 1, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (68, 6225, 2, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (69, 6225, 3, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (70, 6225, 4, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (71, 6225, 5, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (72, 6225, 6, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (73, 6225, 7, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (74, 6225, 8, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (75, 6225, 9, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (76, 6225, 10, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (77, 6225, 11, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (78, 6225, 12, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (79, 6226, 0, 465243, 97, 5000, 5000);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (80, 6226, 1, 465242, 207, 4000, 4000);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (81, 6226, 2, 1073741874, 48, 7659, 8000);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (82, 6226, 3, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (83, 6226, 4, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (84, 6226, 5, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (85, 6226, 6, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (86, 6226, 7, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (87, 6226, 8, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (88, 6226, 9, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (89, 6226, 10, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (90, 6226, 11, 0, 0, 0, 0);
INSERT INTO `characters_item` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (91, 6226, 12, 0, 0, 0, 0);
COMMIT;

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
-- Records of characters_item_attr
-- ----------------------------
BEGIN;
COMMIT;

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='技能';

-- ----------------------------
-- Records of characters_magic
-- ----------------------------
BEGIN;
COMMIT;

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
-- Records of characters_quest
-- ----------------------------
BEGIN;
COMMIT;

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
-- Records of characters_status
-- ----------------------------
BEGIN;
INSERT INTO `characters_status` (`PlayerId`, `Status0`, `Status1`, `Status2`, `Status3`, `Status4`, `Status5`, `Status6`, `Status7`, `Status8`, `Status9`, `Status10`, `Status11`, `Status12`, `Status13`, `Status14`, `Status15`) VALUES (1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `characters_status` (`PlayerId`, `Status0`, `Status1`, `Status2`, `Status3`, `Status4`, `Status5`, `Status6`, `Status7`, `Status8`, `Status9`, `Status10`, `Status11`, `Status12`, `Status13`, `Status14`, `Status15`) VALUES (6222, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `characters_status` (`PlayerId`, `Status0`, `Status1`, `Status2`, `Status3`, `Status4`, `Status5`, `Status6`, `Status7`, `Status8`, `Status9`, `Status10`, `Status11`, `Status12`, `Status13`, `Status14`, `Status15`) VALUES (6223, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `characters_status` (`PlayerId`, `Status0`, `Status1`, `Status2`, `Status3`, `Status4`, `Status5`, `Status6`, `Status7`, `Status8`, `Status9`, `Status10`, `Status11`, `Status12`, `Status13`, `Status14`, `Status15`) VALUES (6224, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `characters_status` (`PlayerId`, `Status0`, `Status1`, `Status2`, `Status3`, `Status4`, `Status5`, `Status6`, `Status7`, `Status8`, `Status9`, `Status10`, `Status11`, `Status12`, `Status13`, `Status14`, `Status15`) VALUES (6225, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `characters_status` (`PlayerId`, `Status0`, `Status1`, `Status2`, `Status3`, `Status4`, `Status5`, `Status6`, `Status7`, `Status8`, `Status9`, `Status10`, `Status11`, `Status12`, `Status13`, `Status14`, `Status15`) VALUES (6226, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
COMMIT;

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
) ENGINE=InnoDB AUTO_INCREMENT=351 DEFAULT CHARSET=utf8 COMMENT='角色仓库物品';

-- ----------------------------
-- Records of characters_storageitem
-- ----------------------------
BEGIN;
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (1, 1, 0, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (2, 1, 1, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (3, 1, 2, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (4, 1, 3, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (5, 1, 4, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (6, 1, 5, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (7, 1, 6, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (8, 1, 7, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (9, 1, 8, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (10, 1, 9, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (11, 1, 10, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (12, 1, 11, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (13, 1, 12, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (14, 1, 13, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (15, 1, 14, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (16, 1, 15, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (17, 1, 16, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (18, 1, 17, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (19, 1, 18, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (20, 1, 19, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (21, 1, 20, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (22, 1, 21, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (23, 1, 22, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (24, 1, 23, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (25, 1, 24, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (26, 1, 25, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (27, 1, 26, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (28, 1, 27, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (29, 1, 28, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (30, 1, 29, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (31, 1, 30, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (32, 1, 31, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (33, 1, 32, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (34, 1, 33, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (35, 1, 34, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (36, 1, 35, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (37, 1, 36, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (38, 1, 37, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (39, 1, 38, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (40, 1, 39, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (41, 1, 40, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (42, 1, 41, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (43, 1, 42, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (44, 1, 43, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (45, 1, 44, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (46, 1, 45, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (47, 1, 46, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (48, 1, 47, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (49, 1, 48, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (50, 1, 49, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (51, 6221, 0, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (52, 6221, 1, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (53, 6221, 2, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (54, 6221, 3, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (55, 6221, 4, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (56, 6221, 5, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (57, 6221, 6, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (58, 6221, 7, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (59, 6221, 8, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (60, 6221, 9, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (61, 6221, 10, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (62, 6221, 11, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (63, 6221, 12, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (64, 6221, 13, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (65, 6221, 14, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (66, 6221, 15, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (67, 6221, 16, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (68, 6221, 17, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (69, 6221, 18, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (70, 6221, 19, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (71, 6221, 20, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (72, 6221, 21, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (73, 6221, 22, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (74, 6221, 23, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (75, 6221, 24, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (76, 6221, 25, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (77, 6221, 26, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (78, 6221, 27, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (79, 6221, 28, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (80, 6221, 29, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (81, 6221, 30, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (82, 6221, 31, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (83, 6221, 32, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (84, 6221, 33, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (85, 6221, 34, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (86, 6221, 35, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (87, 6221, 36, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (88, 6221, 37, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (89, 6221, 38, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (90, 6221, 39, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (91, 6221, 40, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (92, 6221, 41, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (93, 6221, 42, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (94, 6221, 43, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (95, 6221, 44, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (96, 6221, 45, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (97, 6221, 46, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (98, 6221, 47, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (99, 6221, 48, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (100, 6221, 49, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (101, 6222, 0, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (102, 6222, 1, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (103, 6222, 2, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (104, 6222, 3, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (105, 6222, 4, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (106, 6222, 5, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (107, 6222, 6, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (108, 6222, 7, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (109, 6222, 8, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (110, 6222, 9, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (111, 6222, 10, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (112, 6222, 11, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (113, 6222, 12, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (114, 6222, 13, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (115, 6222, 14, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (116, 6222, 15, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (117, 6222, 16, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (118, 6222, 17, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (119, 6222, 18, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (120, 6222, 19, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (121, 6222, 20, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (122, 6222, 21, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (123, 6222, 22, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (124, 6222, 23, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (125, 6222, 24, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (126, 6222, 25, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (127, 6222, 26, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (128, 6222, 27, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (129, 6222, 28, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (130, 6222, 29, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (131, 6222, 30, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (132, 6222, 31, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (133, 6222, 32, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (134, 6222, 33, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (135, 6222, 34, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (136, 6222, 35, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (137, 6222, 36, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (138, 6222, 37, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (139, 6222, 38, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (140, 6222, 39, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (141, 6222, 40, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (142, 6222, 41, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (143, 6222, 42, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (144, 6222, 43, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (145, 6222, 44, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (146, 6222, 45, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (147, 6222, 46, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (148, 6222, 47, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (149, 6222, 48, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (150, 6222, 49, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (151, 6223, 0, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (152, 6223, 1, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (153, 6223, 2, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (154, 6223, 3, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (155, 6223, 4, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (156, 6223, 5, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (157, 6223, 6, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (158, 6223, 7, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (159, 6223, 8, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (160, 6223, 9, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (161, 6223, 10, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (162, 6223, 11, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (163, 6223, 12, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (164, 6223, 13, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (165, 6223, 14, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (166, 6223, 15, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (167, 6223, 16, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (168, 6223, 17, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (169, 6223, 18, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (170, 6223, 19, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (171, 6223, 20, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (172, 6223, 21, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (173, 6223, 22, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (174, 6223, 23, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (175, 6223, 24, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (176, 6223, 25, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (177, 6223, 26, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (178, 6223, 27, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (179, 6223, 28, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (180, 6223, 29, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (181, 6223, 30, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (182, 6223, 31, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (183, 6223, 32, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (184, 6223, 33, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (185, 6223, 34, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (186, 6223, 35, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (187, 6223, 36, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (188, 6223, 37, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (189, 6223, 38, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (190, 6223, 39, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (191, 6223, 40, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (192, 6223, 41, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (193, 6223, 42, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (194, 6223, 43, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (195, 6223, 44, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (196, 6223, 45, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (197, 6223, 46, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (198, 6223, 47, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (199, 6223, 48, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (200, 6223, 49, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (201, 6224, 0, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (202, 6224, 1, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (203, 6224, 2, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (204, 6224, 3, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (205, 6224, 4, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (206, 6224, 5, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (207, 6224, 6, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (208, 6224, 7, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (209, 6224, 8, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (210, 6224, 9, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (211, 6224, 10, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (212, 6224, 11, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (213, 6224, 12, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (214, 6224, 13, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (215, 6224, 14, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (216, 6224, 15, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (217, 6224, 16, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (218, 6224, 17, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (219, 6224, 18, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (220, 6224, 19, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (221, 6224, 20, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (222, 6224, 21, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (223, 6224, 22, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (224, 6224, 23, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (225, 6224, 24, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (226, 6224, 25, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (227, 6224, 26, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (228, 6224, 27, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (229, 6224, 28, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (230, 6224, 29, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (231, 6224, 30, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (232, 6224, 31, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (233, 6224, 32, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (234, 6224, 33, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (235, 6224, 34, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (236, 6224, 35, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (237, 6224, 36, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (238, 6224, 37, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (239, 6224, 38, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (240, 6224, 39, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (241, 6224, 40, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (242, 6224, 41, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (243, 6224, 42, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (244, 6224, 43, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (245, 6224, 44, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (246, 6224, 45, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (247, 6224, 46, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (248, 6224, 47, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (249, 6224, 48, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (250, 6224, 49, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (251, 6225, 0, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (252, 6225, 1, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (253, 6225, 2, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (254, 6225, 3, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (255, 6225, 4, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (256, 6225, 5, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (257, 6225, 6, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (258, 6225, 7, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (259, 6225, 8, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (260, 6225, 9, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (261, 6225, 10, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (262, 6225, 11, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (263, 6225, 12, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (264, 6225, 13, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (265, 6225, 14, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (266, 6225, 15, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (267, 6225, 16, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (268, 6225, 17, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (269, 6225, 18, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (270, 6225, 19, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (271, 6225, 20, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (272, 6225, 21, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (273, 6225, 22, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (274, 6225, 23, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (275, 6225, 24, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (276, 6225, 25, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (277, 6225, 26, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (278, 6225, 27, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (279, 6225, 28, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (280, 6225, 29, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (281, 6225, 30, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (282, 6225, 31, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (283, 6225, 32, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (284, 6225, 33, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (285, 6225, 34, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (286, 6225, 35, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (287, 6225, 36, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (288, 6225, 37, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (289, 6225, 38, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (290, 6225, 39, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (291, 6225, 40, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (292, 6225, 41, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (293, 6225, 42, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (294, 6225, 43, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (295, 6225, 44, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (296, 6225, 45, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (297, 6225, 46, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (298, 6225, 47, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (299, 6225, 48, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (300, 6225, 49, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (301, 6226, 0, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (302, 6226, 1, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (303, 6226, 2, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (304, 6226, 3, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (305, 6226, 4, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (306, 6226, 5, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (307, 6226, 6, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (308, 6226, 7, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (309, 6226, 8, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (310, 6226, 9, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (311, 6226, 10, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (312, 6226, 11, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (313, 6226, 12, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (314, 6226, 13, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (315, 6226, 14, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (316, 6226, 15, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (317, 6226, 16, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (318, 6226, 17, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (319, 6226, 18, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (320, 6226, 19, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (321, 6226, 20, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (322, 6226, 21, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (323, 6226, 22, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (324, 6226, 23, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (325, 6226, 24, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (326, 6226, 25, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (327, 6226, 26, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (328, 6226, 27, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (329, 6226, 28, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (330, 6226, 29, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (331, 6226, 30, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (332, 6226, 31, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (333, 6226, 32, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (334, 6226, 33, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (335, 6226, 34, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (336, 6226, 35, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (337, 6226, 36, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (338, 6226, 37, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (339, 6226, 38, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (340, 6226, 39, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (341, 6226, 40, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (342, 6226, 41, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (343, 6226, 42, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (344, 6226, 43, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (345, 6226, 44, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (346, 6226, 45, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (347, 6226, 46, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (348, 6226, 47, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (349, 6226, 48, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` (`Id`, `PlayerId`, `Position`, `MakeIndex`, `StdIndex`, `Dura`, `DuraMax`) VALUES (350, 6226, 49, 0, 0, 0, 0);
COMMIT;

SET FOREIGN_KEY_CHECKS = 1;
