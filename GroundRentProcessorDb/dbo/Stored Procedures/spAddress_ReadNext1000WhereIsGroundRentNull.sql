CREATE PROCEDURE [dbo].[spAddress_ReadNext1000WhereIsGroundRentNull]
AS
begin
	select [AccountId], [Ward], [Section], [Block], [Lot]
	
	from dbo.[Address] where [IsGroundRent] is null
	
	order by [AccountId] offset 1000 rows fetch next 1000 rows only;
End