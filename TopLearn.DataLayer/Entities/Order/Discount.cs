using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.DataLayer.Entities.Order
{
    public class Discount
    {
        [Key]
        public int DiscountId { get; set; }

        [Display(Name = "کد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(40, ErrorMessage = "فیلد {0} نمی تواند بیش از {1} کاراکتر باشد.")]
        public string DiscountCode { get; set; }

        [Display(Name = "درصد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int DiscountPersent { get; set; }

        [Display(Name = "تعداد تخفیف")]
        public int? UsableCount { get; set; }

        [Display(Name = "تاریخ شروع تخفیف")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "تاریخ اتمام تخفیف")]
        public DateTime? EndDate { get; set; }

        public bool IsDeleted { get; set; }

        #region Relation
        public ICollection<UserDiscountCode> UserDiscountCodes { get; set; }

        #endregion
    }
}
