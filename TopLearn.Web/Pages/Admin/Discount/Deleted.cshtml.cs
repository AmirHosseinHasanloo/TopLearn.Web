using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs.Order;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Discount
{
    [PermissionChecker(14)]
    public class DeletedModel : PageModel
    {
        private IOrderService _orderService;
        public DeletedModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty]
        public DiscountsForAdminPanelViewModel DiscountViewModel { get; set; }

        public void OnGet(int pageId = 1)
        {
            DiscountViewModel = _orderService.GetAllDeletedDiscountsForAdmin(pageId);

        }
    }
}
