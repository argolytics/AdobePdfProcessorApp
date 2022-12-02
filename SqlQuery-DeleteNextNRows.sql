DELETE FROM GroundRentBaltimoreCity WHERE AccountId NOT IN 
(SELECT TOP 60000 AccountId FROM GroundRentBaltimoreCity ORDER BY AccountId)