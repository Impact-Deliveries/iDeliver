CREATE TABLE [dbo].[Attachment]
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
	   [ModuleID] BIGINT NOT NULL, 
    [ModuleType] INT NOT NULL, 
    [Path] NVARCHAR(500) NOT NULL, 
    [FileName] NVARCHAR(100) NOT NULL, 
    [Extension] NCHAR(50) NOT NULL, 
      [AttachmentType] INT NOT NULL, 
    [CreationDate] DATETIME NOT NULL, 
    [IsDeleted] BIT NOT NULL, 
    [CreatorID] BIGINT NULL, 
    [GroupID] NCHAR(10) NULL, 
    CONSTRAINT [PK_Attachment] PRIMARY KEY ([ID] ASC)
)
