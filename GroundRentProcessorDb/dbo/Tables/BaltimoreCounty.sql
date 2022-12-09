CREATE TABLE [dbo].[BaltimoreCounty]
(
	[AccountId]  NCHAR (16) NULL,
    [AccountNumber] NCHAR (16) NULL,
    [Ward]  NCHAR (2) NULL,
    [Section]  NCHAR (2) NULL,
    [Block]  NCHAR (5) NULL,
    [Lot]  NCHAR (4) NULL,
    [LandUseCode] NCHAR(1) NULL,
    [YearBuilt] SMALLINT NULL,
    [IsGroundRent] BIT NULL,
    [IsRedeemed] BIT NULL
)
