CREATE PROCEDURE [dbo].[spAddress_Update]
	@AccountId nvarchar(50),
	@IsRedeemed BIT

AS
begin
	set nocount on;

	update dbo.[Address] set
	[AccountId] = @AccountId,
	[IsRedeemed] = @IsRedeemed

	where AccountId = @AccountId
end