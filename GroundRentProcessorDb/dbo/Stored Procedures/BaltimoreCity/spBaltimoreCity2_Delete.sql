CREATE PROCEDURE [dbo].[spBaltimoreCity2_Delete]
	@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[BaltimoreCity2] WHERE AccountId = @AccountId;
END