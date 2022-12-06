CREATE PROCEDURE [dbo].[spAddress_ReadTopAmountWhereIsGroundRentNullAndYearBuiltIsZeroBaltimoreCity1]
@Amount smallint
AS
begin
	select top (@Amount) [AccountId], [Ward], [Section], [Block], [Lot]
	
	FROM dbo.[BaltimoreCity1] where [IsGroundRent] is null and [YearBuilt] = 0
End