CREATE PROCEDURE [dbo].[spGroundRentBaltimoreCity_CreateOrUpdateSDATRedeemedFile]
	@AccountId NVARCHAR(16),
    @IsRedeemed BIT
AS
SET NOCOUNT ON;
	
BEGIN
	IF EXISTS (SELECT [AccountId] FROM dbo.[GroundRentBaltimoreCity] 
	WHERE [AccountId] = @AccountId)
BEGIN
	UPDATE dbo.[GroundRentBaltimoreCity] SET
	[AccountId] = @AccountId,
    [IsRedeemed] = @IsRedeemed
	WHERE [AccountId] = @AccountId
END
ELSE
BEGIN
	INSERT INTO dbo.[GroundRentBaltimoreCity](
	[AccountId],
    [IsRedeemed])

	VALUES(
	@AccountId,
    @IsRedeemed)
END
END