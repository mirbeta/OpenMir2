/*
 Navicat Premium Data Transfer

 Source Server         : 10.10.0.199
 Source Server Type    : MySQL
 Source Server Version : 50727
 Source Host           : 10.10.0.199:3306
 Source Schema         : mir2_account

 Target Server Type    : MySQL
 Target Server Version : 50727
 File Encoding         : 65001

 Date: 29/09/2022 20:05:43
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for account
-- ----------------------------
DROP TABLE IF EXISTS `account`;
CREATE TABLE `account`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `LOGINID` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `PASSWORD` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `USERNAME` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ACTIONTICK` int(5) NULL DEFAULT NULL,
  `ERRORCOUNT` int(5) NULL DEFAULT NULL,
  `SSNO` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `PHONE` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `QUIZ1` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ANSWER1` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `EMAIL` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `QUIZ2` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `ANSWER2` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `BIRTHDAY` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `MOBILEPHONE` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `DELETED` tinyint(2) NULL DEFAULT NULL,
  `CREATEDATE` datetime NULL DEFAULT NULL,
  `LASTUPDATE` datetime NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 13149 CHARACTER SET = utf8 COLLATE = utf8_general_ci COMMENT = '账号' ROW_FORMAT = DYNAMIC;

-- ----------------------------
-- Records of account
-- ----------------------------

SET FOREIGN_KEY_CHECKS = 1;
