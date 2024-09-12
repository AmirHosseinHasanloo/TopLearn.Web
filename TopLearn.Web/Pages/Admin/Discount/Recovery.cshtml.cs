using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Discount
{
    [PermissionChecker(15)]
    public class RecoveryModel : PageModel
    {
        private IOrderService _OrderService;

        public RecoveryModel(IOrderService OrderService)
        {
            _OrderService = OrderService;
        }

        [BindProperty]
        public TopLearn.DataLayer.Entities.Order.Discount Discount { get; set; }
        public void OnGet(int id)
        {
            Discount = _OrderService.GetDeletedDiscountById(id);
        }

        public IActionResult OnPost()
        {

            _OrderService.RecoveryDiscountCode(Discount.DiscountId);

            return RedirectToPage("Index");
        }
    }
}
