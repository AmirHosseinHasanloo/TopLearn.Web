using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.DataLayer.Entities.User
{
    public class UserCourse
    {
        [Key]
        public int UserCourseId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int CourseId { get; set; }


        #region Relations

        public Course.Course Course { get; set; }
        public User User { get; set; }

        #endregion
    }
}
