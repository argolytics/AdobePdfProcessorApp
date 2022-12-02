CREATE PROCEDURE [dbo].[spGroundRentBaltimoreCity_ReadNext10000WhereIsGroundRentNull]
AS
begin
	select [AccountId], [Ward], [Section], [Block], [Lot]
	
	from dbo.[GroundRentBaltimoreCity] where [IsGroundRent] is null
	
	order by [AccountId] offset 10000 rows fetch next 10000 rows only;
End