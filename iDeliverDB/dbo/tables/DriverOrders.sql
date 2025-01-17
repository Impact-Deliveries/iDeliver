﻿CREATE TABLE [dbo].[DriverOrders]
(
	[ID] BIGINT IDENTITY(1,1) NOT NULL,
	[DriverID] BIGINT NOT NULL, 
	[OrderID] BIGINT NOT NULL, 
	[Status] SMALLINT NOT NULL, 
	[Note] NVARCHAR(300) NULL,
	[ModifiedDate] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
	[CreationDate] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
	[IsDeleted] BIT NOT NULL DEFAULT 0,  
	CONSTRAINT [PK_DriverOrders] PRIMARY KEY ([ID] ASC), 
	CONSTRAINT [FK_DriverOrders_Driver] FOREIGN KEY([DriverID]) REFERENCES [dbo].[Driver] ([ID]),
	CONSTRAINT [FK_DriverOrders_Order] FOREIGN KEY([OrderID]) REFERENCES [dbo].[Order] ([ID])
)
