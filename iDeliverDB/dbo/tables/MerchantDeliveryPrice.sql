CREATE TABLE [dbo].[MerchantDeliveryPrice]
(
	[ID] BIGINT IDENTITY(1,1) NOT NULL, 
    [MerchantBranchID] BIGINT NOT NULL, 
    [LocationID] BIGINT,
    [Distance] FLOAT,
    [Amount] MONEY NOT NULL,
    [IsDeleted] [bit] NOT NULL DEFAULT 0,
	[ModifiedDate] [datetime] NOT NULL DEFAULT GETUTCDATE(),
	[CreationDate] [datetime] NOT NULL DEFAULT GETUTCDATE(), 
    CONSTRAINT [PK_MerchantDeliveryPrice] PRIMARY KEY ([ID] ASC),
    CONSTRAINT [FK_MerchantDeliveryPrice_MerchantBranch] FOREIGN KEY([MerchantBranchID]) REFERENCES [dbo].[MerchantBranch] ([ID]),
    CONSTRAINT [FK_MerchantDeliveryPrice_Location] FOREIGN KEY([LocationID]) REFERENCES [dbo].[Location] ([ID])

)
