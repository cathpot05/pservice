/****** Object:  Table [dbo].[serviceaccess]    Script Date: 08/04/2017 15:46:14 ******/
CREATE TABLE [dbo].[serviceaccess](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MemberID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NULL,
	[Status] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

/****** Object:  Table [dbo].[product]    Script Date: 08/04/2017 15:46:13 ******/
SET ANSI_PADDING ON

CREATE TABLE [dbo].[product](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [varchar](255) NOT NULL,
	[PublicIPAddress] [varchar](255) NULL,
	[HostName] [varchar](255) NULL,
	[Type] [varchar](255) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateDeactivated] [datetime] NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON),
UNIQUE NONCLUSTERED 
(
	[ProductName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

SET ANSI_PADDING OFF

/****** Object:  Table [dbo].[member]    Script Date: 08/04/2017 15:46:11 ******/
SET ANSI_PADDING ON

CREATE TABLE [dbo].[member](
	[MemberID] [int] IDENTITY(1,1) NOT NULL,
	[EmailAdd] [varchar](50) NOT NULL,
	[ProductID] [int] NULL,
	[Pass] [varchar](150) NOT NULL,
	[Username] [varchar](20) NOT NULL,
	[Birthday] [varchar](50) NOT NULL,
	[Gender] [varchar](50) NOT NULL,
	[Country] [varchar](50) NOT NULL,
	[Language] [varchar](50) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NULL,
	[Status] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MemberID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

SET ANSI_PADDING OFF

/****** Object:  Table [dbo].[deviceinfo]    Script Date: 08/04/2017 15:46:08 ******/
SET ANSI_PADDING ON

CREATE TABLE [dbo].[deviceinfo](
	[ControlNumber] [int] IDENTITY(1,1) NOT NULL,
	[MemberID] [int] NOT NULL,
	[EmailAddress] [varchar](50) NOT NULL,
	[SWVersion] [varchar](50) NULL,
	[SerialNumber] [varchar](max) NULL,
	[ModelName] [varchar](50) NULL,
	[UUID_EMMC] [varchar](max) NULL,
	[BrandID] [int] NULL,
	[BrandName] [varchar](max) NULL,
	[MacAddress] [varchar](max) NULL,
	[UUID2_Cipher] [nvarchar](max) NULL,
	[UUID2_Plain] [nvarchar](max) NULL,
	[GroupID] [int] NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NULL,
	[Status] [int] NOT NULL,
	[LatestVersion] [varchar](10) NULL,
	[UpdateStatus] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ControlNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

SET ANSI_PADDING OFF
