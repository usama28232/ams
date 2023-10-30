USE [aspnet-ams-20160117192111]
GO

/****** Object:  Table [dbo].[Ledgers]    Script Date: 10/16/2023 7:19:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Ledgers](
	[LedgerID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[Particular] [nvarchar](max) NULL,
	[Debit] [int] NOT NULL,
	[Credit] [int] NOT NULL,
	[Balance] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[BillNo] [nvarchar](max) NULL,
	[isCheque] [int] NOT NULL,
	[isSeen] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Ledgers] PRIMARY KEY CLUSTERED 
(
	[LedgerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Ledgers] ADD  DEFAULT ((0)) FOR [CustomerId]
GO

ALTER TABLE [dbo].[Ledgers] ADD  DEFAULT ((0)) FOR [isCheque]
GO

ALTER TABLE [dbo].[Ledgers] ADD  DEFAULT ((0)) FOR [isSeen]
GO


