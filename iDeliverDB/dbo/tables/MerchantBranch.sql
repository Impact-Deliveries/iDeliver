CREATE TABLE [dbo].[MerchantBranch]
(
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[MerchantID] [bigint] NOT NULL,
	[BranchName] [nvarchar](50) NULL,
	[Overview] [text] NULL,
	[BranchPicture] [nvarchar](50) NULL,
	[LocationID] [bigint] NOT NULL,
	[Address] [nvarchar](50) NULL,
	[Latitude] [nvarchar](50) NULL,
	[Longitude] [nvarchar](50) NULL,
	[Mobile] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL DEFAULT 1,
	[ModifiedDate] [datetime] NOT NULL DEFAULT GETUTCDATE(),
	[CreationDate] [datetime] NOT NULL DEFAULT GETUTCDATE(), 
    [DeliveryPriceOffer] MONEY NOT NULL DEFAULT 0, 
    [DeliveryStatus] SMALLINT NOT NULL DEFAULT 1, 
    CONSTRAINT [PK_MerchantBranch] PRIMARY KEY ([ID] ASC),
	CONSTRAINT [FK_MerchantBranch_Merchant] FOREIGN KEY([MerchantID]) REFERENCES [dbo].[Merchant] ([ID]),
	CONSTRAINT [FK_MerchantBranch_Location] FOREIGN KEY([LocationID]) REFERENCES [dbo].[Location] ([ID])
)
