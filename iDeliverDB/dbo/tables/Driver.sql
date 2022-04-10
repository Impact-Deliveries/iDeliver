CREATE TABLE [dbo].[Driver]
(
	[ID] [int] IDENTITY(1,1) NOT NULL,
    [EnrolmentID] [bigint] NOT NULL,
    [FirstName] NVARCHAR(50) NULL, 
    [SecondName] NVARCHAR(50) NULL, 
    [LastName] NVARCHAR(50) NULL, 
    [Address] NVARCHAR(200) NULL, 
    [Mobile] INT NULL, 
    [Mobile2] INT NULL, 
    [Birthday] DATETIME NULL, 
    [SocialStatus] INT NULL, 
    [CreationDate] DATETIME NULL, 
    [IsDeleted] BIT NULL, 
    [IsActive] BIT NULL, 
    [IsHaveProblem] BIT NULL, 
    [Reason] NVARCHAR(500) NULL,
   CONSTRAINT [PK_Driver] PRIMARY KEY ([ID] ASC), 
   CONSTRAINT [FK_Driver_Enrolment] FOREIGN KEY([EnrolmentID]) REFERENCES [dbo].[Enrolment] ([ID]),

)
