CREATE PROCEDURE [dbo].[spAddress_Delete]
	@AccountId nchar(16)
AS
BEGIN
	DELETE FROM dbo.[Address] WHERE AccountId = @AccountId;
END