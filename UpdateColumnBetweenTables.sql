update dbo.[Address] 
set 
dbo.[Address].IsGroundRent = groundRentTable.IsGroundRent
from dbo.[GroundRentBaltimoreCity] groundRentTable
where groundRentTable.AccountId = dbo.[Address].AccountId;