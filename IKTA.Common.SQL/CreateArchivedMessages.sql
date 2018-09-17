USE [BizTalkResources]
GO

/****** Object:  Table [dbo].[ArchivedMessages]    Script Date: 15.01.2018 10:40:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ArchivedMessages]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ArchivedMessages](
	[ArchivedId] [int] IDENTITY(1,1) NOT NULL,
	[InterchangeId] [nvarchar](40) NULL,
	[MessageId] [nvarchar](40) NULL,
	[ReceivePortName] [nvarchar](255) NULL,
	[MessageContext] [nvarchar](max) NULL,
	[MessageContent] [nvarchar](max) NULL,
	[ArchivedDateTime] [datetime] NOT NULL CONSTRAINT [DF_ArchivedMessages_ArchivedDateTime]  DEFAULT (getdate()),
 CONSTRAINT [PK_ArchivedMessages2] PRIMARY KEY CLUSTERED 
(
	[ArchivedId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
