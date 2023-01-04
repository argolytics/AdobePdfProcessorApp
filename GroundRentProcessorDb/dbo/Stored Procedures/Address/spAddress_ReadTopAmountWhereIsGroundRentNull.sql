CREATE PROCEDURE [dbo].[spAddress_ReadTopAmountWhereIsGroundRentNull]
@Amount smallint
AS
begin
	select top (@Amount) [AccountId], [Ward], [Section], [Block], [Lot]
	
	FROM dbo.[Address] where [IsGroundRent] is null
End