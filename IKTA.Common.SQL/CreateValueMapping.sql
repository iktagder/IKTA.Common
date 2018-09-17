USE [BizTalkResources]
GO
/****** Object:  Table [dbo].[ValueMapping]    Script Date: 25.02.2018 19:53:05 ******/
DROP TABLE [dbo].[ValueMapping]
GO
/****** Object:  Table [dbo].[ValueMapping]    Script Date: 25.02.2018 19:53:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ValueMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FromSystem] [nvarchar](50) NOT NULL,
	[FromField] [nvarchar](255) NOT NULL,
	[FromValue] [nvarchar](255) NOT NULL,
	[ToSystem] [nvarchar](50) NOT NULL,
	[ToField] [nvarchar](255) NOT NULL,
	[ToValue] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_ValueMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO