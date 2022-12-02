CREATE PROCEDURE [dbo].[spGroundRentBaltimoreCity_CreateOrUpdateSDATIsGroundRent]
	@AccountId NVARCHAR(16),
    @IsGroundRent BIT
AS
SET NOCOUNT ON;
	
BEGIN
	IF EXISTS (SELECT [AccountId] FROM dbo.[GroundRentBaltimoreCity] 
	WHERE [AccountId] = @AccountId)
BEGIN
	UPDATE dbo.[GroundRentBaltimoreCity] SET
	[AccountId] = @AccountId,
    [IsGroundRent] = @IsGroundRent
	WHERE [AccountId] = @AccountId
END
ELSE
BEGIN
	INSERT INTO dbo.[GroundRentBaltimoreCity](
	[AccountId],
    [IsGroundRent])

	VALUES(
	@AccountId,
    @IsGroundRent)
END
END