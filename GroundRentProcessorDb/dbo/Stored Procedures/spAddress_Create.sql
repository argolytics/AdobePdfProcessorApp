CREATE PROCEDURE [dbo].[spAddress_Create]
	@AccountId NVARCHAR(50),
	@IsRedeemed BIT
AS
SET NOCOUNT ON;
	IF EXISTS (SELECT [AccountId] FROM dbo.[Address] WHERE [AccountId] = @AccountId)
	BEGIN
	UPDATE dbo.[Address] set
	[AccountId] = @AccountId,
	[IsRedeemed] = @IsRedeemed
END
ELSE
BEGIN
	INSERT INTO dbo.[Address](
	[AccountId],
	[IsRedeemed])

	VALUES(
	@AccountId,
	@IsRedeemed)
END