USE [aspnet-ams-20160117192111]
GO

/****** Object:  Table [dbo].[PurchaseDetails]    Script Date: 10/16/2023 7:20:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PurchaseDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PurID] [int] NOT NULL,
	[Item] [nvarchar](max) NULL,
	[Quantity] [decimal](18, 2) NULL,
	[Price] [decimal](18, 2) NULL,
	[Total] [decimal](18, 2) NULL,
	[Pur_PurchaseID] [int] NULL,
 CONSTRAINT [PK_dbo.PurchaseDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[PurchaseDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PurchaseDetails_dbo.Purchases_Pur_PurchaseID] FOREIGN KEY([Pur_PurchaseID])
REFERENCES [dbo].[Purchases] ([PurchaseID])
GO

ALTER TABLE [dbo].[PurchaseDetails] CHECK CONSTRAINT [FK_dbo.PurchaseDetails_dbo.Purchases_Pur_PurchaseID]
GO


