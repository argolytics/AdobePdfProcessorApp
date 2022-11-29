﻿CREATE PROCEDURE [dbo].[spAddress_CreateOrUpdateSDATRedeemedFile]
	@AccountId NVARCHAR(16),
    @IsRedeemed BIT
AS
SET NOCOUNT ON;
	
BEGIN
	IF EXISTS (SELECT [AccountId] FROM dbo.[Address] 
	WHERE [AccountId] = @AccountId)
BEGIN
	UPDATE dbo.[Address] SET
	[AccountId] = @AccountId,
    [IsRedeemed] = @IsRedeemed
	WHERE [AccountId] = @AccountId
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
END