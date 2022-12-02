CREATE PROCEDURE [dbo].[spGroundRentBaltimoreCityAmanda_CreateOrUpdateSDATIsGroundRent]
	@AccountId NVARCHAR(16),
    @IsGroundRent BIT
AS
SET NOCOUNT ON;
	
BEGIN
	IF EXISTS (SELECT [AccountId] FROM dbo.[GroundRentBaltimoreCityAmanda] 
	WHERE [AccountId] = @AccountId)
BEGIN
	UPDATE dbo.[GroundRentBaltimoreCityAmanda] SET
	[AccountId] = @AccountId,
    [IsGroundRent] = @IsGroundRent
	WHERE [AccountId] = @AccountId
END
ELSE
BEGIN
	INSERT INTO dbo.[GroundRentBaltimoreCityAmanda](
	[AccountId],
    [IsGroundRent])

	VALUES(
	@AccountId,
    @IsGroundRent)
END
END