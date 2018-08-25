using System;
using System.ComponentModel.DataAnnotations;

namespace BTR.DataAccess.Entities
{
    public class Audit
    {
        [Key]
        public int OwnerId { get; set; }

        public DateTime ChangeDt { get; set; }

        public string TableName { get; set; }

        public string ChangeOwner { get; set; }

        public string KeyValues { get; set; }

        public string OldValues { get; set; }

        public string NewValues { get; set; }
    }
}
