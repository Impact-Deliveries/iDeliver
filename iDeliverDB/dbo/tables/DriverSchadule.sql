CREATE TABLE [dbo].[DriverSchadule]
(
	[ID] BIGINT IDENTITY(1,1) NOT NULL,
	[DriverID] BIGINT NOT NULL, 
	[DayID] INT NOT NULL, 
    [CreationDate] DATETIME NULL, 
    [IsDeleted] BIT NULL, 
    CONSTRAINT [PK_DriverSchadule] PRIMARY KEY ([ID] ASC), 
	CONSTRAINT [FK_DriverSchadule_Driver] FOREIGN KEY([DriverID]) REFERENCES [dbo].[Driver] ([ID])
)
