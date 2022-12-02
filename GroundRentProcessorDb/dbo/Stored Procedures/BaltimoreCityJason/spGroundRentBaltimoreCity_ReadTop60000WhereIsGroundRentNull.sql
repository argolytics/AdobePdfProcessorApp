CREATE PROCEDURE [dbo].[spGroundRentBaltimoreCity_ReadTop60000WhereIsGroundRentNull]
AS
begin
	select top 60000 [AccountId], [Ward], [Section], [Block], [Lot]
	
	FROM dbo.[GroundRentBaltimoreCity] where [IsGroundRent] is null
End