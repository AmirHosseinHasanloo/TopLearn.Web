using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Immutable;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Discount
{
    [PermissionChecker(13)]
    public class DeleteModel : PageModel
    {
        private IOrderService _OrderService;

        public DeleteModel(IOrderService OrderService)
        {
            _OrderService = OrderService;
        }

        [BindProperty]
        public TopLearn.DataLayer.Entities.Order.Discount Discount { get; set; }
        public void OnGet(int id)
        {
            Discount = _OrderService.GetDiscountById(id);
        }

        public IActionResult OnPost()
        {
            // Delete Discount Code

            _OrderService.DeleteDiscountCode(Discount.DiscountId);
            return RedirectToPage("Index");
        }
    }
}
