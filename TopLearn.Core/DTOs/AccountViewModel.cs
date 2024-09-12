using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;
using System.Text;

namespace TopLearn.Core.DTOs
{
    public class RegisterViewModel
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

        [Display(Name = "تکرار رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "فیلد {0} نمی تواند بیش از {1} کاراکتر باشد.")]
        [Compare("Password", ErrorMessage = "رمز عبور تکرار شده صحیح نیست")]
        public string RePassword { get; set; }
    }

    public class LoginViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "فیلد {0} نمی تواند بیش از {1} کاراکتر باشد.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد.")]
        public string Email { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "فیلد {0} نمی تواند بیش از {1} کاراکتر باشد.")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "فیلد {0} نمی تواند بیش از {1} کاراکتر باشد.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد.")]
        public string Email { get; set; }
    }

    public class ResetPasswordViewModel
    {
        public string ActiveCode { get; set; }
        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "فیلد {0} نمی تواند بیش از {1} کاراکتر باشد.")]
        public string Password { get; set; }

        [Display(Name = "تکرار رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "فیلد {0} نمی تواند بیش از {1} کاراکتر باشد.")]
        [Compare("Password", ErrorMessage = "رمز عبور تکرار شده صحیح نیست")]
        public string RePassword { get; set; }
    }

    public class SideBarUserPanelViewModel
    {
        public string UserName { get; set; }
        public DateTime RegisterDate { get; set; }
        public string UserAvatar { get; set; }
    }
}
