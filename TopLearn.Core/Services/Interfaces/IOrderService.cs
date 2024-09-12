using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using TopLearn.Core.DTOs;
using TopLearn.Core.DTOs.Order;
using TopLearn.DataLayer.Entities.Order;

namespace TopLearn.Core.Services.Interfaces
{
    public interface IOrderService
    {
        int AddOrder(string UserName, int courseId);
        void AddDiscount(Discount discount);
        Discount GetDiscountById(int discountId);
        Discount GetDeletedDiscountById(int discountId);
        void UpdateDiscount(Discount discount);
        void DeleteDiscountCode(int discountId);
        void RecoveryDiscountCode(int discountId);
        void UpdateOrderPrice(int orderId);
        void UpdateOrder(Order order);
        Order GetOrderForUserPanel(string UserName, int orderId);
        Order GetOrderById(int orderId);
        List<Discount> GetAllDiscounts();
        DiscountsForAdminPanelViewModel GetAllDeletedDiscountsForAdmin(int pageId = 1);
        DiscountsForAdminPanelViewModel GetAllDiscountsForAdmin(int pageId = 1);
        bool FinallyOrder(string userName, int orderId);
        bool IsExistsDiscountCode(string discountCode);
        DiscountType UseDiscount(int orderId, string discountCode);
        UserOrdersForUserPanelViewModel GetAllOrdersForUserPanel(string UserName, int pageId = 1);


    }
}
