CREATE TABLE [dbo].[DriverCase]
(
	[ID] BIGINT IDENTITY(1,1) NOT NULL, 
    [DriverID] BIGINT NOT NULL, 
    [Status] SMALLINT NOT NULL, 
    [IsOnline] BIT NOT NULL DEFAULT 0,
    [Latitude] NVARCHAR(50) NULL, 
    [Longitude] NVARCHAR(50) NULL, 
    [ModifiedDate] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreationDate] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_DriverCase] PRIMARY KEY ([ID] ASC), 
    CONSTRAINT [FK_DriverCase_Driver] FOREIGN KEY([DriverID]) REFERENCES [dbo].[Driver] ([ID])
)
