using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TopLearn.DataLayer.Entities.Course
{
    public class CourseComment
    {
        [Key]
        public int CommentId { get; set; }

        [Display(Name = "دوره")]
        [Required]
        public int CourseId { get; set; }

        [Display(Name = "کاربر")]
        [Required]
        public int UserId { get; set; }

        [Display(Name = "نظر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(700, ErrorMessage = "فیلد {0} نمی تواند بیش از {1} کاراکتر باشد.")]
        public string Comment { get; set; }

        [Display(Name = "حذف شده")]
        [Required]
        public bool IsDeleted { get; set; }
        [Display(Name = "تایید شده توسط ادمین")]
        [Required]
        public bool IsAdminRead { get; set; }

        [Display(Name ="تاریخ")]
        [Required]
        public DateTime CreateDate { get; set; }

        public int? ParentId { get; set; }


        #region Relations
        public User.User User { get; set; }
        public Course Course { get; set; }
        #endregion
    }
}
