CREATE PROCEDURE [dbo].[spAddress_CreateOrUpdateSpecPrintFileForBaltimoreCounty]
	@AccountId NCHAR(20),
	@AccountNumber NCHAR(20),
    @Ward NCHAR (2),
	@LandUseCode NCHAR (1),
	@YearBuilt SMALLINT
AS
SET NOCOUNT ON;
	
BEGIN
	IF EXISTS (SELECT [AccountId] FROM dbo.[Address] 
	WHERE [AccountId] = @AccountId)
BEGIN
	UPDATE dbo.[GroundRentBaltimoreCounty] SET
	[AccountId] = @AccountId,
	[AccountNumber] = @AccountNumber,
    [Ward] = @Ward,
	[LandUseCode] = @LandUseCode,
	[YearBuilt] = @YearBuilt
    
	WHERE [AccountId] = @AccountId
END
ELSE
BEGIN
	INSERT INTO dbo.[GroundRentBaltimoreCounty](
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