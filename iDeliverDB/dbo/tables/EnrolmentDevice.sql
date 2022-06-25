CREATE TABLE [dbo].[EnrolmentDevice]
(
	[ID] BIGINT IDENTITY(1,1) NOT NULL, 
    [EnrolmentID] BIGINT NOT NULL,
    [DeviceID] NVARCHAR(50) NULL, 
    [DeviceName] NVARCHAR(100) NULL,  
    [DeviceToken] NVARCHAR(MAX) NULL, 
    [DeviceType] SMALLINT NOT NULL, 
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
    [CreationDate] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [ModifiedDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [PK_Enrolment_Device] PRIMARY KEY ([ID] ASC), 
    CONSTRAINT [FK_Enrolment_Device] FOREIGN KEY([EnrolmentID]) REFERENCES [dbo].[Enrolment] ([ID]),
)
