USE [master]
GO
/****** Object:  Database [db_uc_projet]    Script Date: 29/05/2025 07:35:45 ******/
CREATE DATABASE [db_uc_projet]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'db_uc_projet', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\db_uc_projet.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'db_uc_projet_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\db_uc_projet_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [db_uc_projet].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [db_uc_projet] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [db_uc_projet] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [db_uc_projet] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [db_uc_projet] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [db_uc_projet] SET ARITHABORT OFF 
GO
ALTER DATABASE [db_uc_projet] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [db_uc_projet] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [db_uc_projet] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [db_uc_projet] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [db_uc_projet] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [db_uc_projet] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [db_uc_projet] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [db_uc_projet] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [db_uc_projet] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [db_uc_projet] SET  DISABLE_BROKER 
GO
ALTER DATABASE [db_uc_projet] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [db_uc_projet] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [db_uc_projet] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [db_uc_projet] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [db_uc_projet] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [db_uc_projet] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [db_uc_projet] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [db_uc_projet] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [db_uc_projet] SET  MULTI_USER 
GO
ALTER DATABASE [db_uc_projet] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [db_uc_projet] SET DB_CHAINING OFF 
GO
ALTER DATABASE [db_uc_projet] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [db_uc_projet] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [db_uc_projet] SET DELAYED_DURABILITY = DISABLED 
GO
USE [db_uc_projet]
GO
/****** Object:  Table [dbo].[Buses]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Buses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BusNumber] [int] NULL,
	[BusCode] [int] NULL,
	[VoltageMag] [float] NULL,
	[VoltageAngle] [float] NULL,
	[LoadMW] [float] NULL,
	[LoadMVar] [float] NULL,
	[GenMW] [float] NULL,
	[GenMVar] [float] NULL,
	[Qmin] [float] NULL,
	[Qmax] [float] NULL,
	[QcQl] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cost]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cost](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Alpha] [float] NULL,
	[Beta] [float] NULL,
	[Gamma] [float] NULL,
	[Pmin] [float] NULL,
	[Pmax] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lines]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lines](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Resistance] [float] NULL,
	[Reactance] [float] NULL,
	[HalfB] [float] NULL,
	[LineCode] [int] NULL,
	[BusFrom] [nvarchar](50) NULL,
	[BusTo] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ParametresTechniques]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ParametresTechniques](
	[ParametreId] [int] IDENTITY(1,1) NOT NULL,
	[ChargeElementaire] [float] NULL,
	[ConstanteBoltzmann] [float] NULL,
	[TempReference] [float] NULL,
	[IrradianceReference] [float] NULL,
	[NbCellulesSerie] [int] NULL,
	[Isc] [float] NULL,
	[Voc] [float] NULL,
	[Impp] [float] NULL,
	[Vmpp] [float] NULL,
	[Pmp] [float] NULL,
	[CoeffTemperature] [float] NULL,
	[ResistanceSerie] [float] NULL,
	[ResistanceParallele] [float] NULL,
	[FacteurIdealite] [float] NULL,
	[NbPanneauxSerie] [int] NULL,
	[NbChainesParallele] [int] NULL,
	[NOCT] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[ParametreId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_BUS]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_BUS](
	[CODE] [nvarchar](50) NOT NULL,
	[NIVEAU] [float] NULL,
	[TYPE] [nvarchar](50) NULL,
	[ID_REGION] [int] NULL,
	[CHARGE_p] [float] NULL,
	[CHARGE_q] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[CODE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_GENERATOUR]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_GENERATOUR](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ID_WILAYA] [int] NULL,
	[NAME] [nvarchar](50) NULL,
	[TYPE] [nvarchar](50) NULL,
	[A] [float] NULL,
	[B] [float] NULL,
	[C] [float] NULL,
	[PMIN] [float] NULL,
	[PMAX] [float] NULL,
	[MUT] [float] NULL,
	[MDT] [float] NULL,
	[HSC] [float] NULL,
	[CSC] [float] NULL,
	[CS] [float] NULL,
	[IS] [float] NULL,
	[MUST_RUNN] [float] NULL,
	[ID_BUS] [nvarchar](50) NULL,
 CONSTRAINT [PK_TB_GENERATOUR] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_Load]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_Load](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[HOURS] [float] NULL,
	[Load] [float] NULL,
 CONSTRAINT [PK_TB_Load] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_PLACEMENT_CENTRALE_PV]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_PLACEMENT_CENTRALE_PV](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NAME] [nvarchar](50) NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
	[technique_id] [int] NULL,
	[puissance] [float] NULL,
	[id_bus] [nvarchar](50) NULL,
 CONSTRAINT [PK_TB_PLACEMENT_CENTRALE_PV] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_PV]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_PV](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[HEURES] [int] NULL,
	[PRODUCTION] [float] NULL,
 CONSTRAINT [PK_TB_PV] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_REGION]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_REGION](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NAME] [nvarchar](50) NULL,
 CONSTRAINT [PK_TB_REGION] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_transfo]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_transfo](
	[Code_transfo] [nvarchar](50) NOT NULL,
	[bus_from] [nvarchar](50) NULL,
	[bus_to] [nvarchar](50) NULL,
	[R_pu] [float] NULL,
	[X_pu] [float] NULL,
	[tap_Mag] [float] NULL,
	[tap_angle] [float] NULL,
 CONSTRAINT [PK_TB_transfo] PRIMARY KEY CLUSTERED 
(
	[Code_transfo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TB_WIND]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_WIND](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[HEURES] [int] NULL,
	[PRODUCTION] [float] NULL,
 CONSTRAINT [PK_TB_WIND] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Lines]  WITH CHECK ADD  CONSTRAINT [FK_Lines_TB_BUS] FOREIGN KEY([BusFrom])
REFERENCES [dbo].[TB_BUS] ([CODE])
GO
ALTER TABLE [dbo].[Lines] CHECK CONSTRAINT [FK_Lines_TB_BUS]
GO
ALTER TABLE [dbo].[Lines]  WITH CHECK ADD  CONSTRAINT [FK_Lines_TB_BUS1] FOREIGN KEY([BusTo])
REFERENCES [dbo].[TB_BUS] ([CODE])
GO
ALTER TABLE [dbo].[Lines] CHECK CONSTRAINT [FK_Lines_TB_BUS1]
GO
ALTER TABLE [dbo].[TB_BUS]  WITH CHECK ADD  CONSTRAINT [FK_TB_BUS_TB_REGION] FOREIGN KEY([ID_REGION])
REFERENCES [dbo].[TB_REGION] ([ID])
GO
ALTER TABLE [dbo].[TB_BUS] CHECK CONSTRAINT [FK_TB_BUS_TB_REGION]
GO
ALTER TABLE [dbo].[TB_GENERATOUR]  WITH CHECK ADD  CONSTRAINT [FK_TB_GENERATOUR_TB_BUS] FOREIGN KEY([ID_BUS])
REFERENCES [dbo].[TB_BUS] ([CODE])
GO
ALTER TABLE [dbo].[TB_GENERATOUR] CHECK CONSTRAINT [FK_TB_GENERATOUR_TB_BUS]
GO
ALTER TABLE [dbo].[TB_PLACEMENT_CENTRALE_PV]  WITH CHECK ADD  CONSTRAINT [FK_TB_PLACEMENT_CENTRALE_PV_ParametresTechniques] FOREIGN KEY([technique_id])
REFERENCES [dbo].[ParametresTechniques] ([ParametreId])
GO
ALTER TABLE [dbo].[TB_PLACEMENT_CENTRALE_PV] CHECK CONSTRAINT [FK_TB_PLACEMENT_CENTRALE_PV_ParametresTechniques]
GO
ALTER TABLE [dbo].[TB_PLACEMENT_CENTRALE_PV]  WITH CHECK ADD  CONSTRAINT [FK_TB_PLACEMENT_CENTRALE_PV_TB_BUS] FOREIGN KEY([id_bus])
REFERENCES [dbo].[TB_BUS] ([CODE])
GO
ALTER TABLE [dbo].[TB_PLACEMENT_CENTRALE_PV] CHECK CONSTRAINT [FK_TB_PLACEMENT_CENTRALE_PV_TB_BUS]
GO
ALTER TABLE [dbo].[TB_transfo]  WITH CHECK ADD  CONSTRAINT [FK_TB_transfo_TB_BUS] FOREIGN KEY([bus_from])
REFERENCES [dbo].[TB_BUS] ([CODE])
GO
ALTER TABLE [dbo].[TB_transfo] CHECK CONSTRAINT [FK_TB_transfo_TB_BUS]
GO
ALTER TABLE [dbo].[TB_transfo]  WITH CHECK ADD  CONSTRAINT [FK_TB_transfo_TB_BUS1] FOREIGN KEY([bus_to])
REFERENCES [dbo].[TB_BUS] ([CODE])
GO
ALTER TABLE [dbo].[TB_transfo] CHECK CONSTRAINT [FK_TB_transfo_TB_BUS1]
GO
/****** Object:  StoredProcedure [dbo].[add_generateur]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[add_generateur]
@ID_WILAYA int,
@NAME varchar(30),
@TYPE varchar(30),
@A float,
@B float,
@C float,
@Pmin float,
@Pmax float,
@mut float,
@mdt float,
@hsc float,
@csc float,
@cs float,
@is float,
@mustrun float 
as

INSERT INTO [TB_GENERATOUR]
           ([ID_WILAYA]
           ,[NAME]
           ,[TYPE]
           ,[A]
           ,[B]
           ,[C]
           ,[PMIN]
           ,[PMAX]
           ,[MUT]
           ,[MDT]
           ,[HSC]
           ,[CSC]
           ,[CS]
           ,[IS]
           ,[MUST_RUNN])
     VALUES
           (@ID_WILAYA
           ,@NAME
           ,@TYPE
           ,@A
           ,@B
           ,@C
           ,@Pmin
           ,@Pmax
           ,@mut
           ,@mdt
           ,@hsc
           ,@csc
           ,@cs
           ,@is
           ,@mustrun)
GO
/****** Object:  StoredProcedure [dbo].[cmb_bus]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[cmb_bus]
as
SELECT *  
  FROM [dbo].[TB_BUS]

GO
/****** Object:  StoredProcedure [dbo].[cmb_technique]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[cmb_technique] 
as
SELECT *
  FROM [dbo].[ParametresTechniques]
 


GO
/****** Object:  StoredProcedure [dbo].[get_central_with_parame]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[get_central_with_parame]
as
SELECT [ID]
      ,[NAME]
	  ,puissance
      ,[Latitude]
      ,[Longitude]
      ,[technique_id] 
	  ,[ChargeElementaire]
      ,[ConstanteBoltzmann]
      ,[TempReference]
      ,[IrradianceReference]
      ,[NbCellulesSerie]
      ,[Isc]
      ,[Voc]
      ,[Impp]
      ,[Vmpp]
      ,[Pmp]
      ,[CoeffTemperature]
      ,[ResistanceSerie]
      ,[ResistanceParallele]
      ,[FacteurIdealite]
      ,[NbPanneauxSerie]
      ,[NbChainesParallele]
      ,[NOCT]  
  FROM [dbo].[TB_PLACEMENT_CENTRALE_PV]
  inner join ParametresTechniques on ParametresTechniques.ParametreId = TB_PLACEMENT_CENTRALE_PV.technique_id

GO
/****** Object:  StoredProcedure [dbo].[get_Data]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[get_Data]
@mustrune float 
as
SELECT [ID], 
       [PMIN], 
       [PMAX], 
       [A], 
       [B], 
       [C], 
       [MDT], 
       [MUT], 
       [CS], 
       [IS], 
       [MUST_RUNN], 
       [HSC], 
       [CSC]
FROM [dbo].[TB_GENERATOUR]
where MUST_RUNN!=@mustrune
ORDER BY [B] ASC  -- ترتيب البيانات بناءً على عمود B

GO
/****** Object:  StoredProcedure [dbo].[get_list_Cost]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[get_list_Cost]
as 
SELECT [Id]
      ,[Alpha]
      ,[Beta]
      ,[Gamma]
  FROM [dbo].[Cost]
GO
/****** Object:  StoredProcedure [dbo].[get_list_limite]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[get_list_limite]
as 
SELECT [Id]
	  ,Pmin
	  ,Pmax
  FROM [dbo].[Cost]
GO
/****** Object:  StoredProcedure [dbo].[get_production_pv]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[get_production_pv]
as
SELECT [ID]
      ,[HEURES] as'Time'
      ,[PRODUCTION] as'Production'
  FROM [dbo].[TB_PV]

GO
/****** Object:  StoredProcedure [dbo].[get_region]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[get_region]
as
SELECT [ID] as'ID'
      ,[NAME] as'Region'
  FROM [dbo].[TB_REGION]

GO
/****** Object:  StoredProcedure [dbo].[get_sum_puissance_min_and_max]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[get_sum_puissance_min_and_max]
as 
SELECT Sum(Pmin)
	  ,Sum(Pmax)
  FROM [dbo].[Cost]
GO
/****** Object:  StoredProcedure [dbo].[get_tb_bus]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[get_tb_bus]
as
SELECT [Id]
      ,[BusNumber]
      ,[BusCode]
      ,[VoltageMag]
      ,[VoltageAngle]
      ,[LoadMW]
      ,[LoadMVar]
      ,[GenMW]
      ,[GenMVar]
      ,[Qmin]
      ,[Qmax]
      ,[QcQl]
  FROM [dbo].[Buses]
GO
/****** Object:  StoredProcedure [dbo].[get_tb_bus1]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[get_tb_bus1]
as
SELECT TB_REGION.[ID]
      ,TB_REGION.[NAME] as'Nom'
      ,[NIVEAU] as'Niveau'
      ,[TYPE] as'Type'
      ,[ID_REGION] as'ID de région'
	  ,TB_REGION.NAME as'Région'
      ,[CHARGE_p] as'Charge P'
      ,[CHARGE_q] as'Charge Q'
  FROM [dbo].[TB_BUS]
  inner join TB_REGION on TB_REGION.ID = TB_BUS.[ID_REGION] 

GO
/****** Object:  StoredProcedure [dbo].[get_tb_Cost]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[get_tb_Cost]
as 
SELECT [Id]
      ,[Alpha]
      ,[Beta]
      ,[Gamma]
	  ,Pmin
	  ,Pmax
  FROM [dbo].[Cost]

GO
/****** Object:  StoredProcedure [dbo].[get_tb_line]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[get_tb_line]
as
SELECT [Id]
      ,[BusFrom]
      ,[BusTo]
      ,[Resistance]
      ,[Reactance]
      ,[HalfB]
      ,[LineCode]
  FROM [dbo].[Lines]

GO
/****** Object:  StoredProcedure [dbo].[get_tb_load]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[get_tb_load]
as
SELECT [ID] 
      ,[HOURS] as'Time'
      ,[Load] as'charge'
  FROM [dbo].[TB_Load]

GO
/****** Object:  StoredProcedure [dbo].[get_tb_win]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[get_tb_win]
as
SELECT [ID]
      ,[HEURES] as'Time'
      ,[PRODUCTION] as'Production'
  FROM [dbo].[TB_WIND]

GO
/****** Object:  StoredProcedure [dbo].[GetGeneratourData]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetGeneratourData]
AS
BEGIN 
    SELECT 
          [ID] as'Units',
          [ID_WILAYA] as'WILAYA',
          [NAME] as'Name',
          [TYPE] as'TYPE',
          [A] as'a',
          [B] as'b',
          [C] as'c',
          [PMIN] as'Pmin',
          [PMAX] as'Pmax',
          [MUT], 
          [MDT],
          [HSC],
          [CSC],
          [CS] ,
          [IS] ,
          [MUST_RUNN] as'Must run'
    FROM [dbo].[TB_GENERATOUR]
END 

GO
/****** Object:  StoredProcedure [dbo].[GetGeneratourData_must_run]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[GetGeneratourData_must_run]
AS
BEGIN 
    SELECT 
          [ID] as'Units',
          [ID_WILAYA] as'WILAYA',
          [NAME] as'Name',
          [TYPE] as'TYPE',
          [A] as'a',
          [B] as'b',
          [C] as'c',
          [PMIN] as'Pmin',
          [PMAX] as'Pmax',
          [MUT], 
          [MDT],
          [HSC],
          [CSC],
          [CS] ,
          [IS] ,
          [MUST_RUNN] as'Must run'
    FROM [dbo].[TB_GENERATOUR]
	ORDER BY [MUST_RUNN] DESC 
END 

GO
/****** Object:  StoredProcedure [dbo].[INSERT_PLACEMENT]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[INSERT_PLACEMENT]
 @NAME		nvarchar(50)
,@Latitude	float
,@Longitude float
,@puissance		float
,@technique_id	int
,@id_bus		nvarchar(50)
AS
INSERT INTO [dbo].[TB_PLACEMENT_CENTRALE_PV]
           ([NAME]
           ,[Latitude]
           ,[Longitude]
		   ,puissance
		   ,technique_id
		   ,id_bus)
     VALUES
           (
		     @NAME		
			,@Latitude	
			,@Longitude 
			,@puissance		
			,@technique_id	
			,@id_bus		
		    )

GO
/****** Object:  StoredProcedure [dbo].[insert_pv]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[insert_pv]
@HEUR int,
@PRODUCTION float

as

INSERT INTO [TB_PV]
           ([HEURES]
           ,[PRODUCTION])
     VALUES
           (@HEUR,
           @PRODUCTION)

GO
/****** Object:  StoredProcedure [dbo].[insert_region_data]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[insert_region_data]
@NAME nvarchar(50) 
as
INSERT INTO [dbo].[TB_REGION]
           ([NAME])
     VALUES
           (@NAME)

GO
/****** Object:  StoredProcedure [dbo].[InsertBus]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertBus]
    @BusNumber INT,
    @BusCode INT,
    @VoltageMag FLOAT,
    @VoltageAngle FLOAT,
    @LoadMW FLOAT,
    @LoadMVar FLOAT,
    @GenMW FLOAT,
    @GenMVar FLOAT,
    @Qmin FLOAT,
    @Qmax FLOAT,
    @QcQl FLOAT
AS
BEGIN
    INSERT INTO Buses (BusNumber, BusCode, VoltageMag, VoltageAngle, LoadMW, LoadMVar, GenMW, GenMVar, Qmin, Qmax, QcQl)
    VALUES (@BusNumber, @BusCode, @VoltageMag, @VoltageAngle, @LoadMW, @LoadMVar, @GenMW, @GenMVar, @Qmin, @Qmax, @QcQl);
END;

GO
/****** Object:  StoredProcedure [dbo].[InsertCost]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertCost]
    @Alpha FLOAT,
    @Beta FLOAT,
    @Gamma FLOAT,
	@Pmin float,
	@Pmax float
AS
BEGIN
    INSERT INTO Cost (Alpha, Beta, Gamma,Pmin,Pmax)
    VALUES (@Alpha, @Beta, @Gamma,@Pmin , @Pmax);
END;

GO
/****** Object:  StoredProcedure [dbo].[InsertLine]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertLine]
    @BusFrom INT,
    @BusTo INT,
    @Resistance FLOAT,
    @Reactance FLOAT,
    @HalfB FLOAT,
    @LineCode INT
AS
BEGIN
    INSERT INTO Lines (BusFrom, BusTo, Resistance, Reactance, HalfB, LineCode)
    VALUES (@BusFrom, @BusTo, @Resistance, @Reactance, @HalfB, @LineCode);
END;

GO
/****** Object:  StoredProcedure [dbo].[insertWin]    Script Date: 29/05/2025 07:35:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[insertWin] 
@Heur int,
@Production float

as

INSERT INTO [TB_WIND]
           ([HEURES]
           ,[PRODUCTION])
     VALUES
           (@Heur,
		   @Production)



GO
USE [master]
GO
ALTER DATABASE [db_uc_projet] SET  READ_WRITE 
GO
