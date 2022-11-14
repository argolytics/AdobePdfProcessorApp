CREATE PROCEDURE [dbo].[spAddress_Delete]
	@AccountId nvarchar(50)
AS
BEGIN
	DELETE FROM dbo.[Address] WHERE AccountId = @AccountId;
END