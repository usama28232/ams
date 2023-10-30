USE [aspnet-ams-20160117192111]
GO

/****** Object:  Table [dbo].[Invoices]    Script Date: 10/16/2023 7:19:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Invoices](
	[InvoiceID] [int] IDENTITY(1,1) NOT NULL,
	[BillNo] [nvarchar](max) NULL,
	[Date] [datetime] NULL,
	[CSId] [int] NOT NULL,
	[GAmount] [decimal](18, 2) NULL,
	[Received] [decimal](18, 2) NULL,
	[Balance] [decimal](18, 2) NULL,
	[Cus_CustomerID] [int] NULL,
 CONSTRAINT [PK_dbo.Invoices] PRIMARY KEY CLUSTERED 
(
	[InvoiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Invoices]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Invoices_dbo.Customers_Cus_CustomerID] FOREIGN KEY([Cus_CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO

ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_dbo.Invoices_dbo.Customers_Cus_CustomerID]
GO


