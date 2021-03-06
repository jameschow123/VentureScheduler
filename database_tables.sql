USE [MVCDemoDB]
GO
/****** Object:  Table [dbo].[Backend]    Script Date: 4/4/2020 2:04:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Backend](
	[backendId] [int] IDENTITY(1,1) NOT NULL,
	[partId] [int] NOT NULL,
	[processName] [nvarchar](max) NULL,
	[duration] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[backendId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Line]    Script Date: 4/4/2020 2:04:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Line](
	[lineId] [int] IDENTITY(1,1) NOT NULL,
	[lineName] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[lineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ManufacturingTime]    Script Date: 4/4/2020 2:04:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ManufacturingTime](
	[lineId] [int] NOT NULL,
	[partId] [int] NOT NULL,
	[manufacturingTime] [int] NOT NULL,
 CONSTRAINT [PK_ManufacturingTime] PRIMARY KEY CLUSTERED 
(
	[lineId] ASC,
	[partId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 4/4/2020 2:04:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[orderId] [int] NOT NULL,
	[partId] [int] NULL,
	[projectName] [nvarchar](500) NULL,
	[lastMaterialDate] [datetime] NULL,
	[shipDate] [datetime] NULL,
	[quantity] [int] NULL,
	[status] [nvarchar](500) NULL,
	[priority] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[orderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Part]    Script Date: 4/4/2020 2:04:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Part](
	[partId] [int] IDENTITY(1,1) NOT NULL,
	[partName] [varchar](max) NULL,
	[side] [nchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[partId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Schedule]    Script Date: 4/4/2020 2:04:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schedule](
	[orderId] [int] NOT NULL,
	[partId] [int] NOT NULL,
	[lineId] [int] NOT NULL,
	[backendId] [int] NOT NULL,
	[BEDate] [datetime] NOT NULL,
	[EarliestStartDate] [datetime] NOT NULL,
	[PlannedStartDate] [datetime] NOT NULL,
	[LatestStartDate] [datetime] NOT NULL,
	[SMTStart] [datetime] NOT NULL,
	[SMTEnd] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[orderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Order] ADD  DEFAULT ('unscheduled') FOR [status]
GO
ALTER TABLE [dbo].[Order] ADD  DEFAULT ((3)) FOR [priority]
GO
ALTER TABLE [dbo].[Backend]  WITH CHECK ADD FOREIGN KEY([partId])
REFERENCES [dbo].[Part] ([partId])
GO
ALTER TABLE [dbo].[Backend]  WITH CHECK ADD FOREIGN KEY([partId])
REFERENCES [dbo].[Part] ([partId])
GO
ALTER TABLE [dbo].[Backend]  WITH CHECK ADD FOREIGN KEY([partId])
REFERENCES [dbo].[Part] ([partId])
GO
ALTER TABLE [dbo].[ManufacturingTime]  WITH NOCHECK ADD FOREIGN KEY([lineId])
REFERENCES [dbo].[Line] ([lineId])
GO
ALTER TABLE [dbo].[ManufacturingTime]  WITH NOCHECK ADD FOREIGN KEY([lineId])
REFERENCES [dbo].[Line] ([lineId])
GO
ALTER TABLE [dbo].[ManufacturingTime]  WITH NOCHECK ADD FOREIGN KEY([lineId])
REFERENCES [dbo].[Line] ([lineId])
GO
ALTER TABLE [dbo].[ManufacturingTime]  WITH CHECK ADD FOREIGN KEY([partId])
REFERENCES [dbo].[Part] ([partId])
GO
ALTER TABLE [dbo].[ManufacturingTime]  WITH CHECK ADD FOREIGN KEY([partId])
REFERENCES [dbo].[Part] ([partId])
GO
ALTER TABLE [dbo].[ManufacturingTime]  WITH CHECK ADD FOREIGN KEY([partId])
REFERENCES [dbo].[Part] ([partId])
GO
ALTER TABLE [dbo].[Order]  WITH NOCHECK ADD FOREIGN KEY([partId])
REFERENCES [dbo].[Part] ([partId])
GO
ALTER TABLE [dbo].[Order]  WITH NOCHECK ADD FOREIGN KEY([partId])
REFERENCES [dbo].[Part] ([partId])
GO
ALTER TABLE [dbo].[Order]  WITH NOCHECK ADD FOREIGN KEY([partId])
REFERENCES [dbo].[Part] ([partId])
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD FOREIGN KEY([backendId])
REFERENCES [dbo].[Backend] ([backendId])
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD FOREIGN KEY([backendId])
REFERENCES [dbo].[Backend] ([backendId])
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD FOREIGN KEY([backendId])
REFERENCES [dbo].[Backend] ([backendId])
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD FOREIGN KEY([lineId])
REFERENCES [dbo].[Line] ([lineId])
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD FOREIGN KEY([lineId])
REFERENCES [dbo].[Line] ([lineId])
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD FOREIGN KEY([lineId])
REFERENCES [dbo].[Line] ([lineId])
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD FOREIGN KEY([orderId])
REFERENCES [dbo].[Order] ([orderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD FOREIGN KEY([partId])
REFERENCES [dbo].[Part] ([partId])
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD FOREIGN KEY([partId])
REFERENCES [dbo].[Part] ([partId])
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD FOREIGN KEY([partId])
REFERENCES [dbo].[Part] ([partId])
GO
