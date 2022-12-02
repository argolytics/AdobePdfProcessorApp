CREATE PROCEDURE [dbo].[spGroundRentBaltimoreCity_ReadByAccountId]
	@AccountId nvarchar(16)
AS
begin
	select [AccountId], [Ward], [Section], [Block], [Lot]

	FROM dbo.[GroundRentBaltimoreCity]
	WHERE AccountId = @AccountId;
End