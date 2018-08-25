using BTR.DataAccess.Entities;

namespace BTR
{
    public class EmailConfig
    {
        public string Recipients { get; set; }
        public string FromAddress { get; set; }
        public string ReplyTo { get; set; }
        public string BodyText { get; set;}
        public string SubjectText { get; set; }
        public string ReminderBodyText { get; set; }
        public string ReminderSubjectText { get; set; }
    }
    public class EmailUtil
    {
        public static string FormatThemeString(string FormatString, ThemeEntity Theme)
        {
            return FormatString
                .Replace("<Title>", Theme.Title)
                .Replace("<ThemeId>", Theme.ThemeId.ToString())
                .Replace("<Owner>", Theme.Owner)
                .Replace("<OpenDt>", Theme.OpenDt.ToString())
                .Replace("<CloseDt>", Theme.CloseDt.ToString())
                .Replace("<Description>", Theme.Description);
        }
    }
}
