DELETE FROM GroundRentBaltimoreCityAmanda WHERE AccountId IN 
(SELECT TOP 60000 AccountId FROM GroundRentBaltimoreCityAmanda ORDER BY AccountId)