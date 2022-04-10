CREATE TABLE [dbo].[DriverDetails]
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
   [DriverID] INT NOT NULL, 
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
    [CreationDate] DATETIME NULL, 
    [IsDeleted] BIT NULL, 
    CONSTRAINT [PK_DriverDetails] PRIMARY KEY ([ID] ASC), 
   CONSTRAINT [FK_DriverDetails_Driver] FOREIGN KEY([DriverID]) REFERENCES [dbo].[Driver] ([ID])
)
