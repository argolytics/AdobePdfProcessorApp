CREATE PROCEDURE [dbo].[spAddress_ReadTop100WhereIsGroundRentNull]
AS
begin
	select top 100 [AccountId], [Ward], [Section], [Block], [Lot]
	
	FROM dbo.[Address] where [IsGroundRent] is null
End