using System;
using System.Collections.Generic;
using System.Text;

namespace BTR.DataAccess.Entities
{
    public class Vote
    {
        public int VoteId { get; set; }
        public int Direction { get; set; }
        public DateTime SubmitDt { get; set; }
        public string Owner { get; set; }
        
        // not null if vote belongs to an idea
        public int? IdeaId { get; set; }
        public IdeaEntity Idea { get; set; }

        // not null if vote belongs to a comment
        public int? CommentId { get; set; }
        public CommentEntity Comment { get; set; }
    }
}
