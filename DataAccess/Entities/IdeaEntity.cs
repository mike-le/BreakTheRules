using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTR.DataAccess.Entities
{
    [Table("ApiIdeas")]
    public class IdeaEntity
    {
        [Key]
        public int PostId { get; set; }
        
        [NotMapped]
        public int Score {
            get
            {
                return (Votes != null) ? this.Votes.GroupBy(v => v.Owner).Sum(s => s.OrderByDescending(v=>v.SubmitDt).First().Direction) : 0;
            }
        }

        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime SubmitDt { get; set; }
        
        public DateTime ModifiedDt { get; set; }

        [Required]
        public string Owner { get; set; }

        [NotMapped]
        public int UserVoteDirection { get; set; }

        public virtual List<CommentEntity> Comments { get; set; }

        public List<Status> IdeaStatus { get; set; }

        public List<Vote> Votes { get; set; }

        [Required]
        public int ThemeId { get; set; }
        public ThemeEntity Theme { get; set; }


    }

}
