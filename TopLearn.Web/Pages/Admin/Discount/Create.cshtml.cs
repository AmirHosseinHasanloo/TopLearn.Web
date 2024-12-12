using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Globalization;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Discount
{
    [PermissionChecker(13)]
    public class CreateModel : PageModel
    {
        private IOrderService _OrderService;

        public CreateModel(IOrderService OrderService)
        {
            _OrderService = OrderService;
        }


        [BindProperty]
        public DataLayer.Entities.Order.Discount Discount { get; set; }

        public void OnGet()
        {

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

            if (!ModelState.IsValid && _OrderService.IsExistsDiscountCode(Discount.DiscountCode))
            {
                return Page();
            }

            //Add Discount to db =>
            _OrderService.AddDiscount(Discount);

            return RedirectToPage("Index");
        }


        public IActionResult OnGetCheckdiscountCode(string discountCode)
        {
            return Content(_OrderService.IsExistsDiscountCode(discountCode).ToString());
        }

    }
}
