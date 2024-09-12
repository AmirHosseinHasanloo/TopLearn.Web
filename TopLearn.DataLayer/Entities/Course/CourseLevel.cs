using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TopLearn.DataLayer.Entities.Course
{
    public class CourseLevel
    {
        [Key]
        public int LevelId { get; set; }

        [Required]
        [MaxLength(100)]
        public string LevelTitle { get; set; }

        #region Relation

        public ICollection<Course> Courses { get; set; }

        #endregion

    }
}
