USE [aspnet-ams-20160117192111]
GO

/****** Object:  Table [dbo].[Suppliers]    Script Date: 10/16/2023 7:20:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Suppliers](
	[SupplierID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[City] [nvarchar](max) NULL,
	[Contact] [nvarchar](max) NULL,
	[Contact1] [nvarchar](max) NULL,
	[Contact2] [nvarchar](max) NULL,
	[Contact3] [nvarchar](max) NULL,
	[AlertStatus] [int] NOT NULL,
	[AlertDays] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Suppliers] PRIMARY KEY CLUSTERED 
(
	[SupplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


