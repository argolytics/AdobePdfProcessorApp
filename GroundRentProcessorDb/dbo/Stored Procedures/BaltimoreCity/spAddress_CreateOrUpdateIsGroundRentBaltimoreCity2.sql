CREATE PROCEDURE [dbo].[spAddress_CreateOrUpdateIsGroundRentBaltimoreCity2]
	@AccountId NCHAR(16),
    @IsGroundRent BIT
AS
SET NOCOUNT ON;
	
BEGIN
	IF EXISTS (SELECT [AccountId] FROM dbo.[BaltimoreCity2] 
	WHERE [AccountId] = @AccountId)
BEGIN
	UPDATE dbo.[BaltimoreCity2] SET
	[AccountId] = @AccountId,
    [IsGroundRent] = @IsGroundRent
	WHERE [AccountId] = @AccountId
END
ELSE
BEGIN
	INSERT INTO dbo.[BaltimoreCity2](
	[AccountId],
    [IsGroundRent])

	VALUES(
	@AccountId,
    @IsGroundRent)
END
END