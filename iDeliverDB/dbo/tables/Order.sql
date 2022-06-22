CREATE TABLE [dbo].[Order]
(
	[ID] BIGINT IDENTITY(1,1) NOT NULL,
	 [MerchantBranchID] BIGINT NOT NULL, 
	
    [TotalAmount] MONEY NOT NULL, 
    [DeliveryAmount] MONEY NOT NULL DEFAULT 0, 
    [Status] SMALLINT NOT NULL, 
    [Note] NVARCHAR(300) NULL,
    [ModifiedDate] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreationDate] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
      [MerchantDeliveryPriceID] BIGINT NULL, 
    [ClientName] NVARCHAR(50) NULL, 
    [ClientNumber] NVARCHAR(50) NULL, 
    CONSTRAINT [PK_Order] PRIMARY KEY ([ID] ASC), 
    CONSTRAINT [FK_Order_MerchantBranch] FOREIGN KEY([MerchantBranchID]) REFERENCES [dbo].[MerchantBranch] ([ID]),
	CONSTRAINT [FK_MerchantDeliveryPrice_Order] FOREIGN KEY([MerchantDeliveryPriceID]) REFERENCES [dbo].[MerchantDeliveryPrice] ([ID])

)
