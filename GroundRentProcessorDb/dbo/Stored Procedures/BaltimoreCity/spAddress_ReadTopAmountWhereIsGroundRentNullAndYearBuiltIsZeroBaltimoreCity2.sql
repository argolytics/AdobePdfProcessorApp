CREATE PROCEDURE [dbo].[spAddress_ReadTopAmountWhereIsGroundRentNullAndYearBuiltIsZeroBaltimoreCity2]
@Amount smallint
AS
begin
	select top (@Amount) [AccountId], [Ward], [Section], [Block], [Lot]
	
	FROM dbo.[BaltimoreCity2] where [IsGroundRent] is null and [YearBuilt] = 0
End