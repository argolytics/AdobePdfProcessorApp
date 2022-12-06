CREATE PROCEDURE [dbo].[spAddress_CreateOrUpdateSpecPrintFileForBaltimoreCity]
	@AccountId NCHAR(16),
	@Ward  NCHAR (2),
    @Section  NCHAR (2),
    @Block  NCHAR (5),
    @Lot  NCHAR (4),
    @LandUseCode NCHAR(1),
    @YearBuilt SMALLINT
AS
SET NOCOUNT ON;
	
BEGIN
	IF EXISTS (SELECT [AccountId] FROM dbo.[Address] 
	WHERE [AccountId] = @AccountId)
BEGIN
	UPDATE dbo.[GroundRentBaltimoreCity] SET
	[AccountId] = @AccountId,
	[Ward] = @Ward,
    [Section] = @Section,
    [Block] = @Block,
    [Lot] = @Lot,
    [LandUseCode] = @LandUseCode,
    [YearBuilt] = @YearBuilt
	WHERE [AccountId] = @AccountId
END
ELSE
BEGIN
	INSERT INTO dbo.[GroundRentBaltimoreCity](
	[AccountId],
	[Ward],
    [Section],
    [Block],
    [Lot],
    [LandUseCode],
    [YearBuilt])

	VALUES(
	@AccountId,
	@Ward,
    @Section,
    @Block,
    @Lot,
    @LandUseCode,
    @YearBuilt)
END
END