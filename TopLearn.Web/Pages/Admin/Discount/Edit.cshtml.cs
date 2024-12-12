using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System;
using TopLearn.Core.Services.Interfaces;
using TopLearn.Core.Security;

namespace TopLearn.Web.Pages.Admin.Discount
{
    [PermissionChecker(14)]
    public class EditModel : PageModel
    {

        private IOrderService _OrderService;

        public EditModel(IOrderService OrderService)
        {
            _OrderService = OrderService;
        }


        [BindProperty]
        public DataLayer.Entities.Order.Discount Discount { get; set; }

        public void OnGet(int id)
        {
            Discount = _OrderService.GetDiscountById(id);
        }

        public IActionResult OnPost(string startDate = "", string endDate = "")
        {
            if (startDate != "")
            {
                string[] stDate = startDate.Split('/');

                Discount.StartDate = new DateTime(int.Parse(stDate[0])
                    , int.Parse(stDate[1]), int.Parse(stDate[2])
                    , new PersianCalendar());
            }


            if (endDate != "")
            {
                string[] edDate = endDate.Split('/');

                Discount.EndDate = new DateTime(int.Parse(edDate[0])
                    , int.Parse(edDate[1]), int.Parse(edDate[2])
                    , new PersianCalendar());
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Update Discount in db =>
            _OrderService.UpdateDiscount(Discount);

            return RedirectToPage("Index");
        }
    }
}
