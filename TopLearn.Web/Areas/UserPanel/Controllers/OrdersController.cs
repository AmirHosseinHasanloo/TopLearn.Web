using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.DTOs.Order;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class OrdersController : Controller
    {
        private IOrderService _OrderService;

        public OrdersController(IOrderService orderService)
        {
            _OrderService = orderService;
        }

        public IActionResult Index(int pageId = 1)
        {
            return View(_OrderService.GetAllOrdersForUserPanel(User.Identity.Name, pageId));
        }

        [Route("UserPanel/ShowOrder/{id}")]
        public IActionResult ShowOrder(int id, bool final = false, string type = "")
        {
            var userOrder = _OrderService.GetOrderForUserPanel(User.Identity.Name, id);

            if (userOrder == null)
                return NotFound();

            ViewBag.DiscountType = type;
            ViewBag.finalOrder = final;
            return View(userOrder);
        }

        [Route("UserPanel/FinallyOrder/{id}")]
        public IActionResult FinallyOrder(int id)
        {
            if (_OrderService.FinallyOrder(User.Identity.Name, id))
            {
                return Redirect("/UserPanel/ShowOrder/" + id + "?final=true");
            }

            return View();
        }

        public IActionResult UseDiscountCode(int orderId, string discountCode)
        {
            DiscountType type = _OrderService.UseDiscount(orderId, discountCode);

            return Redirect("/UserPanel/ShowOrder/" + orderId + "?type=" + type);
        }
    }
}
