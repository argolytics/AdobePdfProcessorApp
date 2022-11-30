CREATE PROCEDURE [dbo].[spAddress_ReadTop10000WhereIsGroundRentNull]
AS
begin
	select top 10000 [AccountId], [Ward], [Section], [Block], [Lot]
	
	FROM dbo.[Address] where [IsGroundRent] is null
End