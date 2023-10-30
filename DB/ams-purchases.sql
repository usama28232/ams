USE [aspnet-ams-20160117192111]
GO

/****** Object:  Table [dbo].[Purchases]    Script Date: 10/16/2023 7:20:35 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Purchases](
	[PurchaseID] [int] IDENTITY(1,1) NOT NULL,
	[BillNo] [nvarchar](max) NULL,
	[Date] [datetime] NULL,
	[SupId] [int] NOT NULL,
	[GAmount] [decimal](18, 2) NULL,
	[Received] [decimal](18, 2) NULL,
	[Balance] [decimal](18, 2) NULL,
	[Sup_SupplierID] [int] NULL,
 CONSTRAINT [PK_dbo.Purchases] PRIMARY KEY CLUSTERED 
(
	[PurchaseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Purchases]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Purchases_dbo.Suppliers_Sup_SupplierID] FOREIGN KEY([Sup_SupplierID])
REFERENCES [dbo].[Suppliers] ([SupplierID])
GO

ALTER TABLE [dbo].[Purchases] CHECK CONSTRAINT [FK_dbo.Purchases_dbo.Suppliers_Sup_SupplierID]
GO


