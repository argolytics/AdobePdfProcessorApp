CREATE PROCEDURE [dbo].[spGroundRentBaltimoreCityAmanda_ReadTop60000WhereIsGroundRentNull]
AS
begin
	select top 60000 [AccountId], [Ward], [Section], [Block], [Lot]
	
	FROM dbo.[GroundRentBaltimoreCityAmanda] where [IsGroundRent] is null
End