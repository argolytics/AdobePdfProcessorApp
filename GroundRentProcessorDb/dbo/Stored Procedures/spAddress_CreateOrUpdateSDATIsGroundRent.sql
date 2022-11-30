CREATE PROCEDURE [dbo].[spAddress_CreateOrUpdateSDATIsGroundRent]
	@AccountId NVARCHAR(16),
    @IsGroundRent BIT
AS
SET NOCOUNT ON;
	
BEGIN
	IF EXISTS (SELECT [AccountId] FROM dbo.[Address] 
	WHERE [AccountId] = @AccountId)
BEGIN
	UPDATE dbo.[Address] SET
	[AccountId] = @AccountId,
    [IsGroundRent] = @IsGroundRent
	WHERE [AccountId] = @AccountId
END
ELSE
BEGIN
	INSERT INTO dbo.[Address](
	[AccountId],
    [IsGroundRent])

	VALUES(
	@AccountId,
    @IsGroundRent)
END
END