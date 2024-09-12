using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopLearn.DataLayer.Entities.Course
{
    public class CourseGroup
    {
        [Key]
        public int GroupId { get; set; }
        [Display(Name = "عنوان گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "فیلد {0} نمی تواند بیش از {1} کاراکتر باشد.")]
        public string GroupTitle { get; set; }
        [Display(Name = "آیا گروه حذف شده؟")]
        public bool IsDeleted { get; set; }
        [Display(Name = "گروه اصلی")]
        public int? ParentId { get; set; }

        #region Relations 

        [ForeignKey("ParentId")]
        public ICollection<CourseGroup> CourseGroups { get; set; }

        [InverseProperty("CourseGroup")]
        public ICollection<Course> Courses { get; set; }

        [InverseProperty("SubGroup")]
        public ICollection<Course> SubGroup { get; set; }

        #endregion
    }
}
