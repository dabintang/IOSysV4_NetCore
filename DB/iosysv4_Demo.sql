/*
Navicat MySQL Data Transfer

Source Server         : iosysv4_demo
Source Server Version : 80012
Source Host           : 127.0.0.1:3306
Source Database       : iosysv4_demo

Target Server Type    : MYSQL
Target Server Version : 80012
File Encoding         : 65001

Date: 2019-10-12 19:23:55
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for amountaccount
-- ----------------------------
DROP TABLE IF EXISTS `amountaccount`;
CREATE TABLE `amountaccount` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FamilyID` int(11) NOT NULL,
  `Name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Amount` decimal(18,2) NOT NULL,
  `InitAmount` decimal(18,2) NOT NULL,
  `SortWeight` int(11) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `Remark` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreatorID` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `UpdateID` int(11) DEFAULT NULL,
  `UpdateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID` (`ID`),
  KEY `FamilyID` (`FamilyID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of amountaccount
-- ----------------------------
INSERT INTO `amountaccount` VALUES ('1', '2', '现金', '0.00', '0.00', '0', '1', '', '5', '2019-08-17 09:09:40', null, null);
INSERT INTO `amountaccount` VALUES ('2', '2', '支付宝', '0.00', '0.00', '0', '1', '', '5', '2019-08-17 09:09:47', null, null);
INSERT INTO `amountaccount` VALUES ('3', '2', '微信', '0.00', '0.00', '0', '1', '', '5', '2019-08-17 09:09:55', null, null);
INSERT INTO `amountaccount` VALUES ('4', '2', '建行', '0.00', '0.00', '0', '1', '', '5', '2019-08-17 09:10:37', '5', '2019-08-03 09:13:57');

-- ----------------------------
-- Table structure for amountaccounttransfer
-- ----------------------------
DROP TABLE IF EXISTS `amountaccounttransfer`;
CREATE TABLE `amountaccounttransfer` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FamilyID` int(11) NOT NULL,
  `TransferDate` datetime NOT NULL,
  `FromAmountAccountID` int(11) NOT NULL,
  `ToAmountAccountID` int(11) NOT NULL,
  `Amount` decimal(18,2) NOT NULL,
  `Remark` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreatorID` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `UpdateID` int(11) DEFAULT NULL,
  `UpdateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID` (`ID`),
  KEY `FamilyID` (`FamilyID`,`TransferDate`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of amountaccounttransfer
-- ----------------------------

-- ----------------------------
-- Table structure for borrowrepay
-- ----------------------------
DROP TABLE IF EXISTS `borrowrepay`;
CREATE TABLE `borrowrepay` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FamilyID` int(11) NOT NULL,
  `BRDate` datetime NOT NULL,
  `Target` varchar(128) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `BRType` int(11) NOT NULL,
  `AmountAccountID` int(11) NOT NULL,
  `Amount` decimal(18,2) NOT NULL,
  `Remark` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreatorID` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `UpdateID` int(11) DEFAULT NULL,
  `UpdateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID` (`ID`),
  KEY `FamilyID` (`FamilyID`,`BRDate`),
  KEY `FamilyID_2` (`FamilyID`,`Target`,`BRDate`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of borrowrepay
-- ----------------------------

-- ----------------------------
-- Table structure for family
-- ----------------------------
DROP TABLE IF EXISTS `family`;
CREATE TABLE `family` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Remark` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreatorID` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `UpdateID` int(11) DEFAULT NULL,
  `UpdateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of family
-- ----------------------------
INSERT INTO `family` VALUES ('2', '测试家庭', '测试的收支系统', '0', '2019-08-17 21:00:00', null, null);
INSERT INTO `family` VALUES ('3', '管理员之家', '管理员是没有实际家庭', '0', '2019-08-17 23:33:11', null, null);

-- ----------------------------
-- Table structure for income
-- ----------------------------
DROP TABLE IF EXISTS `income`;
CREATE TABLE `income` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FamilyID` int(11) NOT NULL,
  `InDate` datetime NOT NULL,
  `InTypeID` int(11) NOT NULL,
  `AmountAccountID` int(11) NOT NULL,
  `Amount` decimal(18,2) NOT NULL,
  `Remark` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreatorID` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `UpdateID` int(11) DEFAULT NULL,
  `UpdateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID` (`ID`),
  KEY `FamilyID` (`FamilyID`,`InDate`,`InTypeID`,`AmountAccountID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of income
-- ----------------------------

-- ----------------------------
-- Table structure for intype
-- ----------------------------
DROP TABLE IF EXISTS `intype`;
CREATE TABLE `intype` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FamilyID` int(11) NOT NULL,
  `Name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `AmountAccountID` int(11) NOT NULL,
  `SortWeight` int(11) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `Remark` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreatorID` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `UpdateID` int(11) DEFAULT NULL,
  `UpdateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID` (`ID`),
  KEY `FamilyID` (`FamilyID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of intype
-- ----------------------------
INSERT INTO `intype` VALUES ('1', '2', '工资', '4', '0', '1', '', '5', '2019-08-17 09:11:51', null, null);
INSERT INTO `intype` VALUES ('2', '2', '盘盈', '1', '0', '1', '盘点发现钱多了时用', '5', '2019-08-17 09:12:27', '5', '2019-08-03 09:12:56');
INSERT INTO `intype` VALUES ('3', '2', '其他', '1', '0', '1', '', '5', '2019-08-17 09:13:11', null, null);

-- ----------------------------
-- Table structure for loginlog
-- ----------------------------
DROP TABLE IF EXISTS `loginlog`;
CREATE TABLE `loginlog` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) NOT NULL,
  `Token` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `IP` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `LoginTime` datetime NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for outcategory
-- ----------------------------
DROP TABLE IF EXISTS `outcategory`;
CREATE TABLE `outcategory` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FamilyID` int(11) NOT NULL,
  `Name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `SortWeight` int(11) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `Remark` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreatorID` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `UpdateID` int(11) DEFAULT NULL,
  `UpdateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID` (`ID`),
  KEY `FamilyID` (`FamilyID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of outcategory
-- ----------------------------
INSERT INTO `outcategory` VALUES ('1', '2', '餐饮', '0', '1', '', '5', '2019-08-17 09:14:39', null, null);
INSERT INTO `outcategory` VALUES ('2', '2', '居家', '0', '1', '', '5', '2019-08-17 09:15:09', null, null);
INSERT INTO `outcategory` VALUES ('3', '2', '娱乐', '0', '1', '', '5', '2019-08-17 09:15:26', null, null);
INSERT INTO `outcategory` VALUES ('4', '2', '其他', '0', '1', '其他', '5', '2019-08-17 09:19:50', null, null);

-- ----------------------------
-- Table structure for output
-- ----------------------------
DROP TABLE IF EXISTS `output`;
CREATE TABLE `output` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FamilyID` int(11) NOT NULL,
  `OutDate` datetime NOT NULL,
  `OutTypeID` int(11) NOT NULL,
  `AmountAccountID` int(11) NOT NULL,
  `Amount` decimal(18,2) NOT NULL,
  `Remark` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreatorID` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `UpdateID` int(11) DEFAULT NULL,
  `UpdateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID` (`ID`),
  KEY `FamilyID` (`FamilyID`,`OutDate`,`OutTypeID`,`AmountAccountID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of output
-- ----------------------------

-- ----------------------------
-- Table structure for outtype
-- ----------------------------
DROP TABLE IF EXISTS `outtype`;
CREATE TABLE `outtype` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FamilyID` int(11) NOT NULL,
  `OutCategoryID` int(11) NOT NULL,
  `Name` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `AmountAccountID` int(11) NOT NULL,
  `SortWeight` int(11) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `Remark` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreatorID` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `UpdateID` int(11) DEFAULT NULL,
  `UpdateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID` (`ID`),
  KEY `FamilyID` (`FamilyID`,`OutCategoryID`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of outtype
-- ----------------------------
INSERT INTO `outtype` VALUES ('1', '2', '4', '其他', '1', '0', '1', '', '5', '2019-08-17 09:19:59', null, null);
INSERT INTO `outtype` VALUES ('2', '2', '3', '门票', '2', '0', '1', '', '5', '2019-08-17 09:20:32', null, null);
INSERT INTO `outtype` VALUES ('3', '2', '3', '电影', '3', '0', '1', '', '5', '2019-08-17 09:20:48', null, null);
INSERT INTO `outtype` VALUES ('4', '2', '2', '房租', '4', '0', '1', '', '5', '2019-08-17 09:21:04', null, null);
INSERT INTO `outtype` VALUES ('5', '2', '2', '家具', '2', '0', '1', '', '5', '2019-08-17 09:21:14', null, null);
INSERT INTO `outtype` VALUES ('6', '2', '2', '煤气', '3', '0', '1', '', '5', '2019-08-17 09:21:54', null, null);
INSERT INTO `outtype` VALUES ('7', '2', '1', '在外吃喝', '2', '0', '1', '', '5', '2019-08-17 09:22:14', null, null);
INSERT INTO `outtype` VALUES ('8', '2', '1', '食材', '2', '0', '1', '', '5', '2019-08-17 09:22:25', null, null);
INSERT INTO `outtype` VALUES ('9', '2', '1', '水果零食', '2', '0', '1', '', '5', '2019-08-17 09:22:42', null, null);
INSERT INTO `outtype` VALUES ('10', '2', '4', '盘亏', '2', '0', '1', '盘点亏损时用', '5', '2019-08-18 18:38:15', null, null);

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `FamilyID` int(11) NOT NULL,
  `LoginName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `NickName` varchar(64) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Password` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Mobile` varchar(64) NOT NULL,
  `Email` varchar(64) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `IsDelete` tinyint(1) NOT NULL,
  `Remark` varchar(1024) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreatorID` int(11) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `UpdateID` int(11) DEFAULT NULL,
  `UpdateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID` (`ID`),
  KEY `FamilyID` (`FamilyID`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of user
-- ----------------------------
INSERT INTO `user` VALUES ('4', '3', 'admin', '超级管理员', '5F-4D-CC-3B-5A-A7-65-D6-1D-83-27-DE-B8-82-CF-99', '12345678901', 'xx@qq.com', '1', '0', '', '0', '2019-08-17 15:11:00', null, null);
INSERT INTO `user` VALUES ('5', '2', 'user', '用户昵称', '5F-4D-CC-3B-5A-A7-65-D6-1D-83-27-DE-B8-82-CF-99', '12345678901', 'xx@qq.com', '1', '0', '', '0', '2019-08-17 21:05:00', null, null);

-- ----------------------------
-- Table structure for __migrationhistory
-- ----------------------------
DROP TABLE IF EXISTS `__migrationhistory`;
CREATE TABLE `__migrationhistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ContextKey` varchar(300) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Model` longblob NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`MigrationId`,`ContextKey`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of __migrationhistory
-- ----------------------------
