CREATE PROCEDURE [dbo].[spAddress_ReadByAccountId]
	@AccountId nvarchar(50)
AS
begin
	select [AccountId], [Ward], [Section], [Block], [Lot]

	FROM dbo.[Address]
	WHERE AccountId = @AccountId;
End