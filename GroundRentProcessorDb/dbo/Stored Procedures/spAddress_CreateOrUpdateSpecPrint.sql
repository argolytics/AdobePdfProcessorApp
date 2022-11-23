CREATE PROCEDURE [dbo].[spAddress_CreateOrUpdateSpecPrint]
	@AccountId NVARCHAR(50),
	@CapitalizedGroundRent1Amount INT,
	@CapitalizedGroundRent2Amount INT,
	@CapitalizedGroundRent3Amount INT
AS
SET NOCOUNT ON;
	
BEGIN
	IF EXISTS (SELECT [AccountId] FROM dbo.[Address] 
	WHERE [AccountId] = @AccountId)
BEGIN
	UPDATE dbo.[Address] SET
	[AccountId] = @AccountId,
	[CapitalizedGroundRent1Amount] = @CapitalizedGroundRent1Amount,
	[CapitalizedGroundRent2Amount] = @CapitalizedGroundRent2Amount,
	[CapitalizedGroundRent3Amount] = @CapitalizedGroundRent3Amount
	WHERE [AccountId] = @AccountId
END
ELSE
BEGIN
	INSERT INTO dbo.[Address](
	[AccountId],
	[CapitalizedGroundRent1Amount],
	[CapitalizedGroundRent2Amount],
	[CapitalizedGroundRent3Amount])

	VALUES(
	@AccountId,
	@CapitalizedGroundRent1Amount,
	@CapitalizedGroundRent2Amount,
	@CapitalizedGroundRent3Amount)
END
END