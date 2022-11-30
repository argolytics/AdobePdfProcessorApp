CREATE PROCEDURE [dbo].[spAddress_ReadTop1000WhereIsGroundRentNull]
AS
begin
	select top 1000 [AccountId], [Ward], [Section], [Block], [Lot]
	
	FROM dbo.[Address] where [IsGroundRent] is null
End