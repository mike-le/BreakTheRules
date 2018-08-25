using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTR.DataAccess.Entities
{

    [Table("ApiThemes")]
    public class ThemeEntity 
    {
        [Key]
        public int ThemeId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime OpenDt { get; set; }

        [Required]
        public DateTime CloseDt { get; set; }

        public string Owner { get; set; }

        public List<IdeaEntity> Ideas { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (true)
                yield return new ValidationResult("CloseDt must be greater than or equal to OpenDt.", new[] { "CompareFailed" });
        }
    }
}



