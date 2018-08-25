-- Break The Rules
-- Create notification for statuses whose topics are closed,
-- when all of the following are true,
-- 1) the status is still in "Submitted" (will have MostRecentUpdate before the close date of the theme),
-- 2) the status' most recent update is outside of the last two weeks,
-- 3) the status code is not 3 or 4 (implemented or declined), and
-- 4) the status does not already have a notification (Notf.StatusId IS NULL means that the status doesn't have a corresponding notification in the table yet).
USE BTR

INSERT INTO Notifications
SELECT GetDate(), Ignored.StatusId, 0 
FROM
	(SELECT Notf.StatusId AS NotifStatusId, S_a.IdeaId, S_a.MostRecentUpdate, S_b.StatusCode, S_b.StatusId, S_b.Response, Ide.[Owner], Ide.[Message], The.Title, The.CloseDt 
		FROM  
			(SELECT Max(SubmitDt) AS MostRecentUpdate, S_a.IdeaId From Statuses S_a GROUP BY IdeaId) AS S_a
			LEFT JOIN Statuses S_b ON S_a.IdeaId = S_b.IdeaId AND S_a.MostRecentUpdate = S_b.SubmitDt
			LEFT JOIN ApiIdeAS Ide ON Ide.PostId = S_a.IdeaId
			LEFT JOIN ApiThemes The ON The.ThemeId = Ide.ThemeId
			LEFT JOIN Notifications Notf ON Notf.StatusId = S_b.StatusId
		WHERE (S_b.StatusCode <> 4 AND S_b.StatusCode <> 3) 
			AND The.CloseDt < GETDATE() 
			AND (S_a.MostRecentUpdate < The.CloseDt OR S_a.MostRecentUpdate < DATEADD(WEEK, -2, GETDATE())) 
			AND Notf.StatusId IS NULL
	) AS Ignored 
-- if notification goes two weeks without being removed, escalate to executives
UPDATE  Notifications SET IsExec = 1 WHERE createDt < DATEADD(WEEK, -2, GETDATE())  AND IsExec = 0
