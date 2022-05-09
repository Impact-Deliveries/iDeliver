CREATE TABLE [dbo].[DriverDetails]
(
	[ID] BIGINT IDENTITY(1,1) NOT NULL,
    [DriverID] BIGINT NOT NULL, 
    [JobTime] INT NULL, 
    [FromTime] DATETIME NULL, 
    [ToTime] DATETIME NULL, 
    [StartJob] DATETIME NULL, 
    [College] NVARCHAR(50) NULL, 
    [University] NVARCHAR(50) NULL, 
    [Major] NVARCHAR(50) NULL, 
    [GraduationYear] NVARCHAR(50) NULL, 
    [Estimate] NVARCHAR(50) NULL, 
    [AvancedStudies] NVARCHAR(50) NULL, 
    [ModifiedDate] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreationDate] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_DriverDetails] PRIMARY KEY ([ID] ASC), 
    CONSTRAINT [FK_DriverDetails_Driver] FOREIGN KEY([DriverID]) REFERENCES [dbo].[Driver] ([ID])
)
