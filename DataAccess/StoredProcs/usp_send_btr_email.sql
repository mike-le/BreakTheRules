USE [BTR]
GO
/****** Object:  StoredProcedure [dbo].[usp_send_btr_email]    Script Date: 7/26/2018 1:35:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Caleb "The Intern" Webber
-- Create date: 7/25/2018
-- Description:	Just a wrapper for sp_send_dbmail
-- =============================================
ALTER PROCEDURE [dbo].[usp_send_btr_email]
	-- Add the parameters for the stored procedure here
	@FromAddress nvarchar(100),
	@ReplyTo nvarchar(100),
	@Recipients nvarchar(100),
	@BodyText nvarchar(500),
	@SubjectText nvarchar(80)
AS

BEGIN
	SET NOCOUNT ON;

	EXEC msdb.dbo.sp_send_dbmail
		@from_address = @FromAddress,
		@reply_to = @ReplyTo,
		@recipients = @Recipients,
		@body = @BodyText,
		@subject = @SubjectText
END
