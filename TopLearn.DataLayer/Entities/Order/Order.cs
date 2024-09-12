using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TopLearn.DataLayer.Entities.Order
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int OrderSum { get; set; }

        public bool IsFainaly { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        #region Relation

        public User.User User { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

        #endregion
    }
}
