USE [aspnet-ams-20160117192111]
GO

/****** Object:  Table [dbo].[PaymentAdjustments]    Script Date: 10/16/2023 7:19:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PaymentAdjustments](
	[AdjustmentID] [int] IDENTITY(1,1) NOT NULL,
	[CusSupID] [int] NOT NULL,
	[Amount] [decimal](18, 2) NULL,
	[Remarks] [nvarchar](max) NULL,
	[type] [nvarchar](1) NULL,
	[Date] [datetime] NULL,
 CONSTRAINT [PK_dbo.PaymentAdjustments] PRIMARY KEY CLUSTERED 
(
	[AdjustmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


