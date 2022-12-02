CREATE PROCEDURE [dbo].[spGroundRentBaltimoreCity_Delete]
	@AccountId nvarchar(50)
AS
BEGIN
	DELETE FROM dbo.[GroundRentBaltimoreCity] WHERE AccountId = @AccountId;
END