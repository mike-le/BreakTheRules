using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTR.DataAccess.Entities
{
    [Table("Statuses")]
    public class Status
    {
        [Key]
        public int StatusId { get; set; }

        public StatusType StatusCode { get; set; }
        public string Response { get; set; }
        public DateTime SubmitDt { get; set; }

        public int IdeaId { get; set;  }
        [InverseProperty("IdeaStatus")]
        public IdeaEntity Idea { get; set; }
        public Notification Notification { get; set; }
    }
    public enum StatusType
    {
        Submitted,
        UnderReview,
        Approved,
        Implemented,
        Closed
    }
}
