DELETE FROM BaltimoreCity2 WHERE AccountId NOT IN 
(SELECT TOP 14000 AccountId FROM BaltimoreCity2 ORDER BY AccountId)