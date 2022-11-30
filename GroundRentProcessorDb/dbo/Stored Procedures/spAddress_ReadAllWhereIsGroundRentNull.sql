CREATE PROCEDURE [dbo].[spAddress_ReadAllWhereIsGroundRentNull]
AS
begin
	select [AccountId], [Ward], [Section], [Block], [Lot]
	
	FROM dbo.[Address] where [IsGroundRent] is null
End