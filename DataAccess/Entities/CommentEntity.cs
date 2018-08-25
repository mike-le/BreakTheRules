using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTR.DataAccess.Entities
{
    [Table("ApiComments")]
    public class CommentEntity
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        public string Message { get; set; }
        [NotMapped]
        public int Score
        {
            get
            {
                return (Votes != null) ? this.Votes.GroupBy(v => v.Owner).Sum(s => s.OrderByDescending(v => v.SubmitDt).First().Direction) : 0;
            }
        }

        [Required]
        public DateTime SubmitDt { get; set; }
        public DateTime ModifiedDt { get; set; }

        [Required]
        public string Owner { get; set; }
        
        [NotMapped]
        public int UserVoteDirection { get; set; }

        public int? ParentIdeaId { get; set; }
        public IdeaEntity ParentIdea { get; set; }

        public int? ParentCommentId { get; set; }
        public CommentEntity ParentComment { get; set; }

        public virtual List<CommentEntity> Comments { get; set; }
        public List<Vote> Votes { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ParentCommentId == null && ParentIdeaId == null)
                yield return new ValidationResult("Either ParentIdeaId or ParentCommentId must be not null.");
        }

        
    }
}
