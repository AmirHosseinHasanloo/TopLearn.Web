using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TopLearn.DataLayer.Entities.Order;

namespace TopLearn.DataLayer.Entities.User
{
    public class UserDiscountCode
    {
        [Key]
        public int UserDiscountId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int DiscountId { get; set; }


        #region Relations
        public User User { get; set; }
        public Discount Discount { get; set; }

        #endregion
    }
}
