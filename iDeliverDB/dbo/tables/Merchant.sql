CREATE TABLE [dbo].[Merchant]
(
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[OrganizationID] [bigint] NOT NULL,
	[MerchantName] [nvarchar](50) NULL,
	[Overview] [text] NOT NULL,
	[Mobile] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL DEFAULT 1,
	[ModifiedDate] [datetime] NOT NULL DEFAULT GETUTCDATE(),
	[CreationDate] [datetime] NOT NULL DEFAULT GETUTCDATE(), 
    [Owner] NVARCHAR(100) NULL, 
    [Position] NVARCHAR(100) NULL, 
    [QutationNumber] NVARCHAR(100) NULL, 
    [OwnerNumber] NVARCHAR(50) NULL, 
    CONSTRAINT [PK_Merchant] PRIMARY KEY ([ID] ASC),
	CONSTRAINT [FK_Merchant_Organization] FOREIGN KEY([OrganizationID]) REFERENCES [dbo].[Organization] ([ID])
)