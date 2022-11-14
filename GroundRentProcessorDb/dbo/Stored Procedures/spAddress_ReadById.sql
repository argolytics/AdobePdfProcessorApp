CREATE PROCEDURE [dbo].[spAddress_ReadById]
	@AccountId nvarchar(50)
AS
begin
	select [AccountId], [IsRedeemed]

	FROM dbo.[Address]
	WHERE AccountId = @AccountId;
End