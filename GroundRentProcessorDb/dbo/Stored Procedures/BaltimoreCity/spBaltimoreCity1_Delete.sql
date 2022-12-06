CREATE PROCEDURE [dbo].[spBaltimoreCity1_Delete]
	@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[BaltimoreCity1] WHERE AccountId = @AccountId;
END