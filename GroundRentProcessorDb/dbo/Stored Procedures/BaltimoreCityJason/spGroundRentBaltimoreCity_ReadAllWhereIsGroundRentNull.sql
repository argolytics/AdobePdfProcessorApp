CREATE PROCEDURE [dbo].[spGroundRentBaltimoreCity_ReadAllWhereIsGroundRentNull]
AS
begin
	select [AccountId], [Ward], [Section], [Block], [Lot]
	
	FROM dbo.[GroundRentBaltimoreCity] where [IsGroundRent] is null
End