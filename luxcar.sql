USE [master]
GO
/****** Object:  Database [LuxCaRent]    Script Date: 21.06.2024 22:11:03 ******/
CREATE DATABASE [LuxCaRent]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LuxCaRent', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\LuxCaRent.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LuxCaRent_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\LuxCaRent_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [LuxCaRent] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LuxCaRent].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LuxCaRent] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LuxCaRent] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LuxCaRent] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LuxCaRent] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LuxCaRent] SET ARITHABORT OFF 
GO
ALTER DATABASE [LuxCaRent] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LuxCaRent] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LuxCaRent] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LuxCaRent] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LuxCaRent] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LuxCaRent] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LuxCaRent] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LuxCaRent] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LuxCaRent] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LuxCaRent] SET  DISABLE_BROKER 
GO
ALTER DATABASE [LuxCaRent] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LuxCaRent] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LuxCaRent] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LuxCaRent] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LuxCaRent] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LuxCaRent] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LuxCaRent] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LuxCaRent] SET RECOVERY FULL 
GO
ALTER DATABASE [LuxCaRent] SET  MULTI_USER 
GO
ALTER DATABASE [LuxCaRent] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LuxCaRent] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LuxCaRent] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LuxCaRent] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LuxCaRent] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LuxCaRent] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'LuxCaRent', N'ON'
GO
ALTER DATABASE [LuxCaRent] SET QUERY_STORE = ON
GO
ALTER DATABASE [LuxCaRent] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [LuxCaRent]
GO
/****** Object:  Table [dbo].[Flota2]    Script Date: 21.06.2024 22:11:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Flota2](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Marka] [varchar](50) NOT NULL,
	[Model] [varchar](50) NULL,
	[Vin] [varchar](50) NOT NULL,
	[Przebieg] [varchar](50) NOT NULL,
	[Numer] [varchar](50) NULL,
	[Paliwo] [varchar](50) NULL,
	[Poj_Silnika] [varchar](50) NULL,
	[Cena] [int] NOT NULL,
	[Dostepny] [int] NOT NULL,
	[Akt_Zaj] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User2]    Script Date: 21.06.2024 22:11:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User2](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Login] [varchar](50) NOT NULL,
	[Haslo] [varchar](50) NOT NULL,
	[Admin] [int] NOT NULL,
	[email] [varchar](50) NOT NULL,
	[Imie] [varchar](50) NOT NULL,
	[Nazwisko] [varchar](50) NOT NULL,
	[NumerTele] [varchar](50) NOT NULL,
	[PanPani] [int] NULL,
	[Ile_aut] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wynajem2]    Script Date: 21.06.2024 22:11:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wynajem2](
	[id_wyn] [int] IDENTITY(1,1) NOT NULL,
	[id_user] [int] NOT NULL,
	[id_auto] [int] NOT NULL,
	[Data_wyp] [date] NOT NULL,
	[Data_odsta] [date] NOT NULL,
	[potwierdzenie] [int] NOT NULL,
	[id_admin] [int] NULL,
	[Data_praw_odsta] [date] NULL,
	[Odstawione] [int] NOT NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Flota2] ON 

INSERT [dbo].[Flota2] ([id], [Marka], [Model], [Vin], [Przebieg], [Numer], [Paliwo], [Poj_Silnika], [Cena], [Dostepny], [Akt_Zaj]) VALUES (1, N'Mercedes', N'W204', N'VGDF231230103201', N'10000', N'KR12234', N'Benzyna', N'6.3', 1200, 1, 0)
INSERT [dbo].[Flota2] ([id], [Marka], [Model], [Vin], [Przebieg], [Numer], [Paliwo], [Poj_Silnika], [Cena], [Dostepny], [Akt_Zaj]) VALUES (2, N'Ford', N'Mustang', N'WDF2301203210', N'82922', N'KR21152', N'Benzyna', N'5.0', 988, 1, 0)
INSERT [dbo].[Flota2] ([id], [Marka], [Model], [Vin], [Przebieg], [Numer], [Paliwo], [Poj_Silnika], [Cena], [Dostepny], [Akt_Zaj]) VALUES (4, N'Fiat', N'Punto', N'GRS20302032302', N'10000', N'KR23FD2', N'Benzyna', N'1.2', 200, 1, 0)
INSERT [dbo].[Flota2] ([id], [Marka], [Model], [Vin], [Przebieg], [Numer], [Paliwo], [Poj_Silnika], [Cena], [Dostepny], [Akt_Zaj]) VALUES (5, N'BMW', N'E36', N'SDAW233412320', N'1009000', N'KR2SMDC', N'Diesel', N'2.2', 20, 1, 0)
SET IDENTITY_INSERT [dbo].[Flota2] OFF
GO
SET IDENTITY_INSERT [dbo].[User2] ON 

INSERT [dbo].[User2] ([id], [Login], [Haslo], [Admin], [email], [Imie], [Nazwisko], [NumerTele], [PanPani], [Ile_aut]) VALUES (1, N'Admin', N'Demo', 1, N'admin@demo.pl', N'Adminus', N'Demos', N'111222333', 1, 0)
INSERT [dbo].[User2] ([id], [Login], [Haslo], [Admin], [email], [Imie], [Nazwisko], [NumerTele], [PanPani], [Ile_aut]) VALUES (2, N'User', N'Demo', 0, N'user@demo.pl', N'Userus', N'Demous', N'000111222', 0, 0)
SET IDENTITY_INSERT [dbo].[User2] OFF
GO
ALTER TABLE [dbo].[Flota2] ADD  CONSTRAINT [DF_Flota2_Dostepny]  DEFAULT ((1)) FOR [Dostepny]
GO
ALTER TABLE [dbo].[Flota2] ADD  CONSTRAINT [DF_Flota2_Akt_Zaj]  DEFAULT ((0)) FOR [Akt_Zaj]
GO
ALTER TABLE [dbo].[User2] ADD  CONSTRAINT [DF_User2_Admin]  DEFAULT ((0)) FOR [Admin]
GO
ALTER TABLE [dbo].[User2] ADD  CONSTRAINT [DF_User2_Ile_aut]  DEFAULT ((0)) FOR [Ile_aut]
GO
ALTER TABLE [dbo].[Wynajem2] ADD  CONSTRAINT [DF_Table_1_oczekuje]  DEFAULT ((0)) FOR [potwierdzenie]
GO
ALTER TABLE [dbo].[Wynajem2] ADD  CONSTRAINT [DF_Wynajem2_id_admin]  DEFAULT ((0)) FOR [id_admin]
GO
ALTER TABLE [dbo].[Wynajem2] ADD  CONSTRAINT [DF_Wynajem2_Odstawione]  DEFAULT ((0)) FOR [Odstawione]
GO
USE [master]
GO
ALTER DATABASE [LuxCaRent] SET  READ_WRITE 
GO
