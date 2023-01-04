﻿CREATE PROCEDURE [dbo].[spBaltimoreCounty_CreateOrUpdateSpecPrintFile]
	@AccountId NCHAR(16),
	@AccountNumber NCHAR(16),
    @Ward NCHAR (2),
	@LandUseCode NCHAR (1),
	@YearBuilt SMALLINT
AS
SET NOCOUNT ON;
	
BEGIN
	IF EXISTS (SELECT [AccountId] FROM dbo.[BaltimoreCounty] 
	WHERE [AccountId] = @AccountId)
BEGIN
	UPDATE dbo.[BaltimoreCounty] SET
	[AccountId] = @AccountId,
	[AccountNumber] = @AccountNumber,
    [Ward] = @Ward,
	[LandUseCode] = @LandUseCode,
	[YearBuilt] = @YearBuilt
    
	WHERE [AccountId] = @AccountId
END
ELSE
BEGIN
	INSERT INTO dbo.[BaltimoreCounty](
	[AccountId],
	[AccountNumber],
    [Ward],
	[LandUseCode],
	[YearBuilt])

	VALUES(
	@AccountId,
	@AccountNumber,
	@Ward,
	@LandUseCode,
	@YearBuilt)
END
END