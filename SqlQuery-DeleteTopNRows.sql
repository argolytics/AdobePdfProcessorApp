DELETE FROM BaltimoreCity1 WHERE AccountId IN 
(SELECT TOP 14000 AccountId FROM BaltimoreCity1 ORDER BY AccountId)