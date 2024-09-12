using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TopLearn.Core.DTOs
{
    public class UsersForAdminViewModel
    {
        public List<DataLayer.Entities.User.User> Users { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }

    public class UserOrdersForUserPanelViewModel
    {
        public List<DataLayer.Entities.Order.Order> Orders { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }

    public class CreateUserViewModel
    {
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

        [Display(Name = "پروفایل")]
        public IFormFile UserAvatar { get; set; }
    }

    public class EditUserViewModel
    {
        public int UserId { get; set; }
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

        [Display(Name = "پروفایل")]
        public IFormFile UserAvatar { get; set; }

        [Display(Name = "نقش های کاربر")]
        public List<int> UserRoles { get; set; }

        [Display(Name = "تصویر پروفایل فعلی")]
        public string AvatarName { get; set; }
    }
}
