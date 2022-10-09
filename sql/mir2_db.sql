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

 Date: 09/10/2022 21:00:40
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
  `ChrName` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '角色名称',
  `MapName` varchar(10) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '所在地图名称',
  `CX` int(11) NULL DEFAULT 0 COMMENT '所在地图坐标X',
  `CY` int(11) NULL DEFAULT 0 COMMENT '所在地图坐标Y',
  `Level` int(11) NULL DEFAULT 0 COMMENT '等级',
  `Dir` tinyint(2) NULL DEFAULT 0 COMMENT '所在方向',
  `Hair` tinyint(1) NULL DEFAULT 0 COMMENT '发型',
  `Sex` tinyint(1) NULL DEFAULT 0 COMMENT '性别（0:男 1:女）',
  `Job` tinyint(2) NULL DEFAULT 0 COMMENT '职业（0:战士 1:法师 2:道士）',
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
) ENGINE = InnoDB AUTO_INCREMENT = 6226 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '玩家' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of characters
-- ----------------------------
INSERT INTO `characters` VALUES (1, 0, 'admin', 'gm01', '0', 330, 262, 50, 4, 1, 0, 0, 891095, 0, '0', 289, 618, 0, 0, 0, 0, -492.920, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, 0, '2022-08-03 01:33:16', '2022-10-09 20:59:48');
INSERT INTO `characters` VALUES (6221, 0, 'admin', '深山老毒', '0', 607, 584, 1, 4, 1, 0, 2, 0, 0, '0', 289, 618, 0, 0, 0, 0, 0.000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, 0, '2022-09-13 13:28:09', '2022-10-09 20:06:59');
INSERT INTO `characters` VALUES (6222, 0, 'admin', '123456', '0', 294, 651, 1, 4, 1, 0, 1, 0, 0, '0', 289, 618, 0, 0, 0, 0, 0.000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, 0, '2022-10-09 20:13:29', '2022-10-09 20:15:18');
INSERT INTO `characters` VALUES (6223, 0, 'admin', 'gm02', '0141', 4, 11, 1, 3, 1, 0, 2, 0, 0, '0', 289, 618, 0, 0, 0, 0, 0.000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, 0, '2022-10-09 20:54:56', '2022-10-09 20:56:34');
INSERT INTO `characters` VALUES (6224, 0, 'admin', 'gm03', '0', 649, 630, 1, 0, 1, 0, 2, 0, 0, '0', 289, 618, 0, 0, 0, 0, 0.000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, 0, '2022-10-09 20:56:43', '2022-10-09 20:57:15');
INSERT INTO `characters` VALUES (6225, 0, 'admin', 'gm04', NULL, 0, 0, 0, 0, 2, 0, 2, 0, 0, NULL, 0, 0, 0, 0, 0, 0, 0.000, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL, NULL, NULL, 0, '2022-10-09 20:58:08', '2022-10-09 20:58:08');

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
) ENGINE = InnoDB AUTO_INCREMENT = 6226 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '基础属性' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of characters_ablity
-- ----------------------------
INSERT INTO `characters_ablity` VALUES (1, 1, 50, 50, 0, 0, 0, 0, 939, 186, 939, 186, 3540, 200000000, 0, 850, 92, 115, 80, 162, '2022-10-09 20:59:48');
INSERT INTO `characters_ablity` VALUES (6221, 6221, 1, 1, 0, 0, 0, 0, 16, 17, 16, 17, 0, 10, 0, 50, 0, 15, 0, 12, '2022-10-09 20:06:59');
INSERT INTO `characters_ablity` VALUES (6222, 6222, 1, 1, 0, 0, 0, 0, 13, 17, 16, 17, 0, 10, 0, 50, 0, 15, 0, 12, '2022-10-09 20:15:18');
INSERT INTO `characters_ablity` VALUES (6223, 6223, 1, 1, 0, 0, 0, 0, 6, 16, 16, 17, 0, 10, 0, 50, 0, 15, 0, 12, '2022-10-09 20:56:34');
INSERT INTO `characters_ablity` VALUES (6224, 6224, 1, 1, 0, 0, 0, 0, 16, 16, 16, 17, 0, 10, 14, 50, 0, 15, 0, 12, '2022-10-09 20:57:15');
INSERT INTO `characters_ablity` VALUES (6225, 6225, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, NULL);

-- ----------------------------
-- Table structure for characters_bagitem
-- ----------------------------
DROP TABLE IF EXISTS `characters_bagitem`;
CREATE TABLE `characters_bagitem`  (
  `PlayerId` bigint(20) NOT NULL COMMENT '角色ID',
  `Position` int(10) NOT NULL DEFAULT 0 COMMENT '位置',
  `MakeIndex` int(20) NOT NULL DEFAULT 0 COMMENT '物品唯一ID',
  `StdIndex` int(10) NOT NULL DEFAULT 0 COMMENT '物品编号',
  `Dura` int(10) NOT NULL DEFAULT 0 COMMENT '当前持久',
  `DuraMax` int(10) NOT NULL DEFAULT 0 COMMENT '最大持久'
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '包裹' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of characters_bagitem
-- ----------------------------
INSERT INTO `characters_bagitem` VALUES (1, 0, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 1, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 2, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 3, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 4, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 5, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 6, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 7, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 8, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 9, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 10, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 11, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 12, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 13, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 14, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 15, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 16, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 17, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 18, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 19, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 20, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 21, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 22, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 23, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 24, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 25, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 26, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 27, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 28, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 29, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 30, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 31, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 32, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 33, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 34, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 35, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 36, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 37, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 38, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 39, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 40, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 41, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 42, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 43, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 44, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (1, 45, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 0, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 1, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 2, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 3, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 4, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 5, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 6, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 7, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 8, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 9, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 10, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 11, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 12, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 13, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 14, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 15, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 16, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 17, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 18, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 19, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 20, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 21, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 22, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 23, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 24, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 25, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 26, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 27, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 28, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 29, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 30, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 31, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 32, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 33, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 34, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 35, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 36, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 37, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 38, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 39, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 40, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 41, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 42, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 43, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 44, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6221, 45, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 0, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 1, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 2, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 3, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 4, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 5, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 6, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 7, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 8, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 9, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 10, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 11, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 12, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 13, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 14, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 15, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 16, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 17, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 18, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 19, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 20, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 21, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 22, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 23, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 24, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 25, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 26, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 27, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 28, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 29, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 30, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 31, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 32, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 33, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 34, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 35, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 36, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 37, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 38, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 39, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 40, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 41, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 42, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 43, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 44, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6222, 45, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 0, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 1, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 2, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 3, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 4, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 5, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 6, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 7, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 8, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 9, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 10, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 11, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 12, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 13, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 14, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 15, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 16, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 17, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 18, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 19, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 20, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 21, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 22, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 23, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 24, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 25, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 26, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 27, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 28, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 29, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 30, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 31, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 32, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 33, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 34, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 35, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 36, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 37, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 38, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 39, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 40, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 41, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 42, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 43, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 44, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6223, 45, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 0, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 1, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 2, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 3, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 4, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 5, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 6, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 7, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 8, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 9, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 10, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 11, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 12, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 13, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 14, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 15, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 16, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 17, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 18, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 19, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 20, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 21, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 22, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 23, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 24, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 25, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 26, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 27, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 28, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 29, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 30, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 31, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 32, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 33, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 34, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 35, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 36, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 37, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 38, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 39, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 40, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 41, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 42, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 43, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 44, 0, 0, 0, 0);
INSERT INTO `characters_bagitem` VALUES (6224, 45, 0, 0, 0, 0);

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
) ENGINE = InnoDB AUTO_INCREMENT = 6226 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '角色索引' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of characters_indexes
-- ----------------------------
INSERT INTO `characters_indexes` VALUES (1, 'admin', 'gm01', 0, 0, '2022-08-03 01:33:16', '2022-10-09 21:00:01');
INSERT INTO `characters_indexes` VALUES (6221, 'admin', '深山老毒', 0, 1, '2022-09-13 13:28:09', '2022-10-09 20:13:16');
INSERT INTO `characters_indexes` VALUES (6222, 'admin', '123456', 0, 1, '2022-10-09 20:13:29', '2022-10-09 20:55:00');
INSERT INTO `characters_indexes` VALUES (6223, 'admin', 'gm02', 0, 1, '2022-10-09 20:54:56', '2022-10-09 20:56:46');
INSERT INTO `characters_indexes` VALUES (6224, 'admin', 'gm03', 0, 1, '2022-10-09 20:56:43', '2022-10-09 20:58:10');
INSERT INTO `characters_indexes` VALUES (6225, 'admin', 'gm04', 1, 0, '2022-10-09 20:57:37', '2022-10-09 21:00:01');

-- ----------------------------
-- Table structure for characters_item
-- ----------------------------
DROP TABLE IF EXISTS `characters_item`;
CREATE TABLE `characters_item`  (
  `PlayerId` bigint(20) NOT NULL COMMENT '角色ID',
  `Position` int(10) NOT NULL DEFAULT 0 COMMENT '穿戴位置',
  `MakeIndex` int(20) NOT NULL DEFAULT 0 COMMENT '物品唯一ID',
  `StdIndex` int(10) NOT NULL DEFAULT 0 COMMENT '物品编号',
  `Dura` int(10) NOT NULL DEFAULT 0 COMMENT '当前持久',
  `DuraMax` int(10) NOT NULL DEFAULT 0 COMMENT '最大持久'
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '穿戴物品' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of characters_item
-- ----------------------------
INSERT INTO `characters_item` VALUES (1, 0, 1073741879, 115, 33680, 35000);
INSERT INTO `characters_item` VALUES (1, 1, 1073741873, 223, 31795, 32000);
INSERT INTO `characters_item` VALUES (1, 2, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (1, 3, 1073741878, 628, 6809, 7000);
INSERT INTO `characters_item` VALUES (1, 4, 1073741877, 629, 6878, 7000);
INSERT INTO `characters_item` VALUES (1, 5, 1073741875, 630, 6842, 7000);
INSERT INTO `characters_item` VALUES (1, 6, 1073741876, 630, 6839, 7000);
INSERT INTO `characters_item` VALUES (1, 7, 1073741873, 631, 6865, 7000);
INSERT INTO `characters_item` VALUES (1, 8, 1073741874, 631, 6841, 7000);
INSERT INTO `characters_item` VALUES (1, 9, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (1, 10, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (1, 11, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (1, 12, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6221, 0, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6221, 1, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6221, 2, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6221, 3, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6221, 4, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6221, 5, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6221, 6, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6221, 7, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6221, 8, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6221, 9, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6221, 10, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6221, 11, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6221, 12, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6222, 0, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6222, 1, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6222, 2, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6222, 3, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6222, 4, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6222, 5, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6222, 6, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6222, 7, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6222, 8, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6222, 9, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6222, 10, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6222, 11, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6222, 12, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6223, 0, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6223, 1, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6223, 2, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6223, 3, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6223, 4, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6223, 5, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6223, 6, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6223, 7, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6223, 8, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6223, 9, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6223, 10, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6223, 11, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6223, 12, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6224, 0, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6224, 1, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6224, 2, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6224, 3, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6224, 4, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6224, 5, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6224, 6, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6224, 7, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6224, 8, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6224, 9, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6224, 10, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6224, 11, 0, 0, 0, 0);
INSERT INTO `characters_item` VALUES (6224, 12, 0, 0, 0, 0);

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
  `Level` tinyint(2) NOT NULL COMMENT '技能等级',
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
  `Status0` int(12) NULL DEFAULT 0,
  `Status1` int(12) NULL DEFAULT 0,
  `Status2` int(12) NULL DEFAULT 0,
  `Status3` int(12) NULL DEFAULT 0,
  `Status4` int(12) NULL DEFAULT 0,
  `Status5` int(12) NULL DEFAULT 0,
  `Status6` int(12) NULL DEFAULT 0,
  `Status7` int(12) NULL DEFAULT 0,
  `Status8` int(12) NULL DEFAULT 0,
  `Status9` int(12) NULL DEFAULT 0,
  `Status10` int(12) NULL DEFAULT 0,
  `Status11` int(12) NULL DEFAULT 0,
  `Status12` int(12) NULL DEFAULT 0,
  `Status13` int(12) NULL DEFAULT 0,
  `Status14` int(12) NULL DEFAULT 0,
  `Status15` int(12) NULL DEFAULT 0,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 36819 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '人物状态值' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of characters_status
-- ----------------------------
INSERT INTO `characters_status` VALUES (36814, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `characters_status` VALUES (36815, 6222, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `characters_status` VALUES (36816, 6223, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `characters_status` VALUES (36817, 6224, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
INSERT INTO `characters_status` VALUES (36818, 6225, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

-- ----------------------------
-- Table structure for characters_storageitem
-- ----------------------------
DROP TABLE IF EXISTS `characters_storageitem`;
CREATE TABLE `characters_storageitem`  (
  `PlayerId` bigint(20) NOT NULL COMMENT '角色ID',
  `Position` int(10) NOT NULL DEFAULT 0 COMMENT '位置',
  `MakeIndex` int(20) NOT NULL DEFAULT 0 COMMENT '物品唯一ID',
  `StdIndex` int(10) NOT NULL DEFAULT 0 COMMENT '物品编号',
  `Dura` int(10) NOT NULL DEFAULT 0 COMMENT '当前持久',
  `DuraMax` int(10) NOT NULL DEFAULT 0 COMMENT '最大持久'
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '角色仓库物品' ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of characters_storageitem
-- ----------------------------
INSERT INTO `characters_storageitem` VALUES (1, 0, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 1, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 2, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 3, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 4, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 5, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 6, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 7, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 8, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 9, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 10, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 11, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 12, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 13, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 14, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 15, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 16, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 17, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 18, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 19, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 20, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 21, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 22, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 23, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 24, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 25, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 26, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 27, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 28, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 29, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 30, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 31, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 32, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 33, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 34, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 35, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 36, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 37, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 38, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 39, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 40, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 41, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 42, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 43, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 44, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 45, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 46, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 47, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 48, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (1, 49, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 0, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 1, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 2, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 3, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 4, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 5, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 6, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 7, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 8, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 9, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 10, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 11, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 12, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 13, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 14, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 15, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 16, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 17, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 18, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 19, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 20, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 21, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 22, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 23, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 24, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 25, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 26, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 27, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 28, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 29, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 30, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 31, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 32, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 33, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 34, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 35, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 36, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 37, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 38, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 39, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 40, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 41, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 42, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 43, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 44, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 45, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 46, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 47, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 48, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6221, 49, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 0, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 1, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 2, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 3, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 4, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 5, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 6, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 7, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 8, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 9, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 10, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 11, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 12, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 13, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 14, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 15, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 16, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 17, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 18, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 19, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 20, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 21, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 22, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 23, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 24, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 25, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 26, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 27, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 28, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 29, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 30, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 31, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 32, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 33, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 34, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 35, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 36, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 37, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 38, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 39, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 40, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 41, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 42, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 43, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 44, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 45, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 46, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 47, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 48, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6222, 49, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 0, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 1, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 2, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 3, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 4, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 5, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 6, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 7, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 8, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 9, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 10, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 11, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 12, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 13, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 14, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 15, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 16, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 17, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 18, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 19, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 20, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 21, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 22, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 23, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 24, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 25, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 26, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 27, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 28, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 29, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 30, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 31, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 32, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 33, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 34, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 35, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 36, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 37, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 38, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 39, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 40, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 41, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 42, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 43, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 44, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 45, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 46, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 47, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 48, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6223, 49, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 0, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 1, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 2, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 3, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 4, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 5, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 6, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 7, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 8, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 9, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 10, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 11, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 12, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 13, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 14, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 15, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 16, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 17, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 18, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 19, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 20, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 21, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 22, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 23, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 24, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 25, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 26, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 27, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 28, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 29, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 30, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 31, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 32, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 33, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 34, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 35, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 36, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 37, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 38, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 39, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 40, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 41, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 42, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 43, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 44, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 45, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 46, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 47, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 48, 0, 0, 0, 0);
INSERT INTO `characters_storageitem` VALUES (6224, 49, 0, 0, 0, 0);

SET FOREIGN_KEY_CHECKS = 1;
