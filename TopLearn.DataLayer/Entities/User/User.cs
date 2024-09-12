using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TopLearn.DataLayer.Entities.Course;
using TopLearn.DataLayer.Entities.Order;

namespace TopLearn.DataLayer.Entities.User
{
    public class User
    {
        public User()
        {

        }

        [Key]
        public int UserId { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "فیلد {0} نمی تواند بیش از {1} کاراکتر باشد.")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "فیلد {0} نمی تواند بیش از {1} کاراکتر باشد.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد.")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "فیلد {0} نمی تواند بیش از {1} کاراکتر باشد.")]
        public string Password { get; set; }

        [Display(Name = "کد فعالسازی")]
        [MaxLength(50, ErrorMessage = "فیلد {0} نمی تواند بیش از {1} کاراکتر باشد.")]
        public string ActiveCode { get; set; }

        [Display(Name = "وضعیت حساب")]
        public bool IsActive { get; set; }

        [Display(Name = "آواتار")]
        [MaxLength(200, ErrorMessage = "فیلد {0} نمی تواند بیش از {1} کاراکتر باشد.")]
        public string UserAvatar { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        public DateTime RegisterDate { get; set; }

        public bool IsBanned { get; set; }

        #region Relations

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Wallet.Wallet> Wallets { get; set; }
        public ICollection<Course.Course> Courses { get; set; }
        public ICollection<Order.Order> MyProperty { get; set; }
        public ICollection<UserCourse> UserCourses { get; set; }
        public ICollection<UserDiscountCode> UserDiscountCodes { get; set; }
        public ICollection<CourseComment> CourseComments { get; set; }
        public ICollection<CourseVote> CourseVotes { get; set; }
        public ICollection<Question.Question> Questions { get; set; }

        #endregion
    }
}
