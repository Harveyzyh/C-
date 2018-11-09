/*
 Navicat Premium Data Transfer

 Source Server         : 198
 Source Server Type    : SQL Server
 Source Server Version : 11002100
 Source Host           : 192.168.0.198:1433
 Source Catalog        : ROBOT_TEST
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 11002100
 File Encoding         : 65001

 Date: 27/09/2018 15:31:11
*/


-- ----------------------------
-- Table structure for SCHEDULE
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[SCHEDULE]') AND type IN ('U'))
	DROP TABLE [dbo].[SCHEDULE]
GO

CREATE TABLE [dbo].[SCHEDULE] (
  [KEY_ID] int IDENTITY(1,1) NOT NULL,
  [CREATOR] varchar(50) COLLATE Chinese_PRC_CS_AS NOT NULL,
  [CREATE_DATE] varchar(50) COLLATE Chinese_PRC_CS_AS NOT NULL,
  [MODIFIER] varchar(50) COLLATE Chinese_PRC_CS_AS NULL,
  [MODI_DATE] varchar(50) COLLATE Chinese_PRC_CS_AS NULL,
  [SC001] varchar(100) COLLATE Chinese_PRC_CS_AS NOT NULL,
  [SC002] varchar(100) COLLATE Chinese_PRC_CS_AS NULL,
  [SC003] varchar(100) COLLATE Chinese_PRC_CS_AS NOT NULL,
  [SC004] varchar(500) COLLATE Chinese_PRC_CS_AS NULL,
  [SC005] varchar(500) COLLATE Chinese_PRC_CS_AS NULL,
  [SC006] varchar(500) COLLATE Chinese_PRC_CS_AS NULL,
  [SC007] varchar(100) COLLATE Chinese_PRC_CS_AS NULL,
  [SC008] varchar(100) COLLATE Chinese_PRC_CS_AS NULL,
  [SC009] varchar(100) COLLATE Chinese_PRC_CS_AS NULL,
  [SC010] varchar(500) COLLATE Chinese_PRC_CS_AS NULL,
  [SC011] varchar(500) COLLATE Chinese_PRC_CS_AS NULL,
  [SC012] varchar(500) COLLATE Chinese_PRC_CS_AS NULL,
  [SC013] varchar(50) COLLATE Chinese_PRC_CS_AS NOT NULL,
  [SC014] varchar(50) COLLATE Chinese_PRC_CS_AS NULL,
  [SC015] varchar(500) COLLATE Chinese_PRC_CS_AS NULL,
  [SC016] varchar(500) COLLATE Chinese_PRC_CS_AS NULL,
  [SC017] varchar(500) COLLATE Chinese_PRC_CS_AS NULL,
  [SC018] varchar(50) COLLATE Chinese_PRC_CS_AS NULL,
  [SC019] varchar(50) COLLATE Chinese_PRC_CS_AS NULL,
  [SC020] varchar(50) COLLATE Chinese_PRC_CS_AS NULL,
  [SC021] varchar(100) COLLATE Chinese_PRC_CS_AS NULL,
  [SC022] varchar(100) COLLATE Chinese_PRC_CS_AS NULL,
  [SC023] varchar(50) COLLATE Chinese_PRC_CS_AS NULL,
  [SC024] varchar(50) COLLATE Chinese_PRC_CS_AS NULL,
  [SC025] varchar(50) COLLATE Chinese_PRC_CS_AS NULL,
  [SC026] varchar(50) COLLATE Chinese_PRC_CS_AS NULL,
  [SC027] varchar(50) COLLATE Chinese_PRC_CS_AS NULL,
  [SC028] varchar(50) COLLATE Chinese_PRC_CS_AS NULL,
  [SC029] varchar(50) COLLATE Chinese_PRC_CS_AS NULL,
  [SC030] varchar(50) COLLATE Chinese_PRC_CS_AS NULL,
  [SC031] varchar(50) COLLATE Chinese_PRC_CS_AS NULL,
  [SC032] varchar(50) COLLATE Chinese_PRC_CS_AS NULL,
  [SC033] bit NOT NULL,
  [SC034] varchar(500) COLLATE Chinese_PRC_CS_AS NULL,
  [SC035] varchar(500) COLLATE Chinese_PRC_CS_AS DEFAULT ('A,B,C') NULL,
  [SC036] varchar(500) COLLATE Chinese_PRC_CS_AS NULL,
  [SC037] varchar(500) COLLATE Chinese_PRC_CS_AS NULL,
  [SC038] varchar(500) COLLATE Chinese_PRC_CS_AS NULL,
  [SC039] varchar(500) COLLATE Chinese_PRC_CS_AS NULL
)
GO

ALTER TABLE [dbo].[SCHEDULE] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'自增',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'KEY_ID'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建者',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'CREATOR'
GO

EXEC sp_addextendedproperty
'MS_Description', N'创建时间',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'CREATE_DATE'
GO

EXEC sp_addextendedproperty
'MS_Description', N'修改者',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'MODIFIER'
GO

EXEC sp_addextendedproperty
'MS_Description', N'修改时间',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'MODI_DATE'
GO

EXEC sp_addextendedproperty
'MS_Description', N'生产单号',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC001'
GO

EXEC sp_addextendedproperty
'MS_Description', N'订单类型',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC002'
GO

EXEC sp_addextendedproperty
'MS_Description', N'上线时间',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC003'
GO

EXEC sp_addextendedproperty
'MS_Description', N'客户名称',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC004'
GO

EXEC sp_addextendedproperty
'MS_Description', N'注意事项',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC005'
GO

EXEC sp_addextendedproperty
'MS_Description', N'变更原因',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC006'
GO

EXEC sp_addextendedproperty
'MS_Description', N'出货日',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC007'
GO

EXEC sp_addextendedproperty
'MS_Description', N'验货日',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC008'
GO

EXEC sp_addextendedproperty
'MS_Description', N'PO#',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC009'
GO

EXEC sp_addextendedproperty
'MS_Description', N'品名',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC010'
GO

EXEC sp_addextendedproperty
'MS_Description', N'保友品名',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC011'
GO

EXEC sp_addextendedproperty
'MS_Description', N'规格',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC012'
GO

EXEC sp_addextendedproperty
'MS_Description', N'订单数量',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC013'
GO

EXEC sp_addextendedproperty
'MS_Description', N'赠品测试量',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC014'
GO

EXEC sp_addextendedproperty
'MS_Description', N'配置方案',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC015'
GO

EXEC sp_addextendedproperty
'MS_Description', N'配置方案描述',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC016'
GO

EXEC sp_addextendedproperty
'MS_Description', N'描述备注',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC017'
GO

EXEC sp_addextendedproperty
'MS_Description', N'柜型柜数',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC018'
GO

EXEC sp_addextendedproperty
'MS_Description', N'目的地',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC019'
GO

EXEC sp_addextendedproperty
'MS_Description', N'业务员',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC020'
GO

EXEC sp_addextendedproperty
'MS_Description', N'客户单号',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC021'
GO

EXEC sp_addextendedproperty
'MS_Description', N'客户品号',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC022'
GO

EXEC sp_addextendedproperty
'MS_Description', N'生产车间',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC023'
GO

EXEC sp_addextendedproperty
'MS_Description', N'客户编码',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC024'
GO

EXEC sp_addextendedproperty
'MS_Description', N'电商编码',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC025'
GO

EXEC sp_addextendedproperty
'MS_Description', N'急单',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC026'
GO

EXEC sp_addextendedproperty
'MS_Description', N'订单日期',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC027'
GO

EXEC sp_addextendedproperty
'MS_Description', N'生产线别',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC028'
GO

EXEC sp_addextendedproperty
'MS_Description', N'已打包数量',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC029'
GO

EXEC sp_addextendedproperty
'MS_Description', N'状态',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC030'
GO

EXEC sp_addextendedproperty
'MS_Description', N'开始时间',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC031'
GO

EXEC sp_addextendedproperty
'MS_Description', N'完成时间',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC032'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否完成',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC033'
GO

EXEC sp_addextendedproperty
'MS_Description', N'备用1',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC034'
GO

EXEC sp_addextendedproperty
'MS_Description', N'备用2',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC035'
GO

EXEC sp_addextendedproperty
'MS_Description', N'纸箱尺寸',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC036'
GO

EXEC sp_addextendedproperty
'MS_Description', N'有菜鸟条码',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC037'
GO

EXEC sp_addextendedproperty
'MS_Description', N'备用5',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC038'
GO

EXEC sp_addextendedproperty
'MS_Description', N'备用6',
'SCHEMA', N'dbo',
'TABLE', N'SCHEDULE',
'COLUMN', N'SC039'
GO


-- ----------------------------
-- Uniques structure for table SCHEDULE
-- ----------------------------
ALTER TABLE [dbo].[SCHEDULE] ADD CONSTRAINT [SC001] UNIQUE NONCLUSTERED ([SC001] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table SCHEDULE
-- ----------------------------
ALTER TABLE [dbo].[SCHEDULE] ADD CONSTRAINT [PK__SCHEDULE__C41F3ED0689DCDFF] PRIMARY KEY CLUSTERED ([KEY_ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = OFF, ALLOW_PAGE_LOCKS = OFF)  
ON [PRIMARY]
GO

