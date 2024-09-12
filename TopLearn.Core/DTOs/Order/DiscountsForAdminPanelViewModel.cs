using System;
using System.Collections.Generic;
using System.Text;
using TopLearn.DataLayer.Entities.Order;

namespace TopLearn.Core.DTOs.Order
{
    public class DiscountsForAdminPanelViewModel
    {
        public List<Discount> Discount { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }
}
