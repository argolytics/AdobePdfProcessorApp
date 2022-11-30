CREATE PROCEDURE [dbo].[spAddress_ReadTop10WhereIsGroundRentNull]
AS
begin
	select top 10 [AccountId], [Ward], [Section], [Block], [Lot]
	
	FROM dbo.[Address] where [IsGroundRent] is null
End