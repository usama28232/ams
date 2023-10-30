USE [aspnet-ams-20160117192111]
GO

/****** Object:  Table [dbo].[InvoiceDetails]    Script Date: 10/16/2023 7:19:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[InvoiceDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Invid] [int] NOT NULL,
	[Item] [nvarchar](max) NULL,
	[Quantity] [decimal](18, 2) NULL,
	[Price] [decimal](18, 2) NULL,
	[Total] [decimal](18, 2) NULL,
	[Inv_InvoiceID] [int] NULL,
 CONSTRAINT [PK_dbo.InvoiceDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[InvoiceDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.InvoiceDetails_dbo.Invoices_Inv_InvoiceID] FOREIGN KEY([Inv_InvoiceID])
REFERENCES [dbo].[Invoices] ([InvoiceID])
GO

ALTER TABLE [dbo].[InvoiceDetails] CHECK CONSTRAINT [FK_dbo.InvoiceDetails_dbo.Invoices_Inv_InvoiceID]
GO


