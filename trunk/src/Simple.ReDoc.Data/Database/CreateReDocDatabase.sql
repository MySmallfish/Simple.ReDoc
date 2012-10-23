USE [ReDoc]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 23/10/2012 22:38:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TanantId] [int] NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[LastModified] [datetime] NULL,
	[Name] [nvarchar](300) NOT NULL,
	[IdNumber] [nvarchar](20) NULL,
	[Phone] [nvarchar](20) NULL,
	[Email] [nvarchar](300) NULL,
	[Address] [nvarchar](300) NULL,
	[City] [nvarchar](300) NULL,
	[UniqueId] [uniqueidentifier] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Property]    Script Date: 23/10/2012 22:38:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Property](
	[Id] [int] NOT NULL,
	[SellerId] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[LastModified] [datetime] NULL,
	[Rooms] [int] NULL,
	[Floor] [nvarchar](50) NULL,
	[Address] [nvarchar](300) NULL,
	[City] [nvarchar](100) NULL,
	[PercentsRate] [float] NULL,
	[AmountRate] [float] NULL,
	[UniqueId] [uniqueidentifier] NOT NULL,
	[TenantId] [int] NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Property] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PropertyDisplay]    Script Date: 23/10/2012 22:38:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyDisplay](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UniqueId] [uniqueidentifier] NOT NULL,
	[TenantId] [int] NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastModified] [datetime] NULL,
	[PropertyId] [int] NOT NULL,
	[BuyerId] [int] NOT NULL,
	[PercentsRate] [float] NULL,
	[AmountRate] [float] NULL,
	[IsExclusive] [bit] NOT NULL,
	[Signature] [nvarchar](max) NULL,
 CONSTRAINT [PK_PropertyDisplay] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Role]    Script Date: 23/10/2012 22:38:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](100) NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SellerAgreement]    Script Date: 23/10/2012 22:38:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerAgreement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UniqueId] [uniqueidentifier] NOT NULL,
	[TenantId] [int] NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastModified] [datetime] NULL,
	[PropertyId] [int] NOT NULL,
	[PercentsRate] [float] NULL,
	[AmountRate] [float] NULL,
	[Signature] [nvarchar](max) NULL,
 CONSTRAINT [PK_SellerAgreement] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Settings]    Script Date: 23/10/2012 22:38:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[TenantId] [int] NOT NULL,
	[GroupName] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Value] [nvarchar](500) NULL,
	[Username] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
	[TenantId] ASC,
	[GroupName] ASC,
	[Name] ASC,
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tenant]    Script Date: 23/10/2012 22:38:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tenant](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Host] [nvarchar](300) NOT NULL,
	[AdminEmail] [nvarchar](300) NULL,
 CONSTRAINT [PK_Tenant] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 23/10/2012 22:38:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[Username] [nvarchar](100) NOT NULL,
	[PasswordHash] [char](86) NOT NULL,
	[PasswordSalt] [char](10) NOT NULL,
	[Email] [nvarchar](300) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[ImageUrl] [nvarchar](300) NULL,
	[LastLogin] [datetime] NULL,
	[LastSync] [datetime] NULL,
	[CreatedAt] [datetime] NULL,
	[Enabled] [bit] NOT NULL,
	[PasswordChangeRequired] [bit] NOT NULL,
	[TenantId] [int] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Username] ASC,
	[TenantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserInRole]    Script Date: 23/10/2012 22:38:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserInRole](
	[RoleName] [nvarchar](100) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[TenantId] [int] NOT NULL,
 CONSTRAINT [PK_UsersInRoles] PRIMARY KEY CLUSTERED 
(
	[RoleName] ASC,
	[Username] ASC,
	[TenantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_User] FOREIGN KEY([Username], [TanantId])
REFERENCES [dbo].[User] ([Username], [TenantId])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_User]
GO
ALTER TABLE [dbo].[Property]  WITH CHECK ADD  CONSTRAINT [FK_Property_User] FOREIGN KEY([Username], [TenantId])
REFERENCES [dbo].[User] ([Username], [TenantId])
GO
ALTER TABLE [dbo].[Property] CHECK CONSTRAINT [FK_Property_User]
GO
ALTER TABLE [dbo].[PropertyDisplay]  WITH CHECK ADD  CONSTRAINT [FK_PropertyDisplay_Customer] FOREIGN KEY([BuyerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[PropertyDisplay] CHECK CONSTRAINT [FK_PropertyDisplay_Customer]
GO
ALTER TABLE [dbo].[PropertyDisplay]  WITH CHECK ADD  CONSTRAINT [FK_PropertyDisplay_Property] FOREIGN KEY([PropertyId])
REFERENCES [dbo].[Property] ([Id])
GO
ALTER TABLE [dbo].[PropertyDisplay] CHECK CONSTRAINT [FK_PropertyDisplay_Property]
GO
ALTER TABLE [dbo].[PropertyDisplay]  WITH CHECK ADD  CONSTRAINT [FK_PropertyDisplay_User] FOREIGN KEY([Username], [TenantId])
REFERENCES [dbo].[User] ([Username], [TenantId])
GO
ALTER TABLE [dbo].[PropertyDisplay] CHECK CONSTRAINT [FK_PropertyDisplay_User]
GO
ALTER TABLE [dbo].[Settings]  WITH CHECK ADD  CONSTRAINT [FK_Settings_User] FOREIGN KEY([Username], [TenantId])
REFERENCES [dbo].[User] ([Username], [TenantId])
GO
ALTER TABLE [dbo].[Settings] CHECK CONSTRAINT [FK_Settings_User]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Tenant] FOREIGN KEY([TenantId])
REFERENCES [dbo].[Tenant] ([Id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Tenant]
GO
ALTER TABLE [dbo].[UserInRole]  WITH CHECK ADD  CONSTRAINT [FK_UserInRole_Role] FOREIGN KEY([RoleName])
REFERENCES [dbo].[Role] ([RoleName])
GO
ALTER TABLE [dbo].[UserInRole] CHECK CONSTRAINT [FK_UserInRole_Role]
GO
ALTER TABLE [dbo].[UserInRole]  WITH CHECK ADD  CONSTRAINT [FK_UserInRole_User] FOREIGN KEY([Username], [TenantId])
REFERENCES [dbo].[User] ([Username], [TenantId])
GO
ALTER TABLE [dbo].[UserInRole] CHECK CONSTRAINT [FK_UserInRole_User]
GO
