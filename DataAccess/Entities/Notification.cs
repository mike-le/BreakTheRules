using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BTR.DataAccess.Entities
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public DateTime createDt { get; set; }

        public int StatusId { get; set; }
        public Status Status { get; set; }
        public bool IsExec { get; set; }
    }
}
