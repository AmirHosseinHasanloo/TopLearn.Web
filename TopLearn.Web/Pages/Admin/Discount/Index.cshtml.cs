using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs.Order;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Discount
{
    [PermissionChecker(10)]
    public class IndexModel : PageModel
    {
        private IOrderService _orderService;

        public IndexModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty]
        public DiscountsForAdminPanelViewModel DiscountViewModel { get; set; }

        public void OnGet(int pageId = 1)
        {
            DiscountViewModel = _orderService.GetAllDiscountsForAdmin(pageId);

        }
    }
}
