using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using System.Text;

namespace TopLearn.DataLayer.Entities.Wallet
{
    public class Wallet
    {
        public Wallet()
        {

        }

        [Key]
        public int WalletId { get; set; }
        [Display(Name = "نوع تراکنش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int TypeId { get; set; }
        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int UserId { get; set; }
        [Display(Name = "مبلغ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Amount { get; set; }
        [Display(Name = "توضیحات")]
        [MaxLength(200, ErrorMessage = "فیلد {0} نمی تواند بیش از {1} کاراکتر باشد.")]
        public string Description { get; set; }
        [Display(Name = "وضعیت تراکنش")]
        public bool IsPayed { get; set; }
        [Display(Name = "تاریخ تراکنش")]
        public DateTime CreateDate { get; set; }

        //Navigation properties
        [ForeignKey("TypeId")]
        public virtual WalletType WalletType { get; set; }
        public virtual User.User User { get; set; }

    }
}
