using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopLearn.Core.DTOs;
using TopLearn.Core.DTOs.Order;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.Course;
using TopLearn.DataLayer.Entities.Order;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;

namespace TopLearn.Core.Services
{
    public class OrderService : IOrderService
    {
        private TopLearnContext _context;

        IUserPanelService _userService;

        public OrderService(TopLearnContext context, IUserPanelService userService)
        {
            _context = context;
            _userService = userService;
        }

        public void AddDiscount(Discount discount)
        {
            _context.Discount.Add(discount);
            _context.SaveChanges();
        }

        public int AddOrder(string UserName, int courseId)
        {
            int userId = _userService.GetUserIdByUserName(UserName);

            Order order = _context.Order
                .SingleOrDefault(o => o.UserId == userId && !o.IsFainaly);


            Course course = _context.Course.Find(courseId);
            if (order == null)
            {
                order = new Order()
                {
                    UserId = userId,
                    IsFainaly = false,
                    CreateDate = DateTime.Now,
                    OrderSum = course.CoursePrice,
                    OrderDetails = new List<OrderDetail>()
                    {
                        new OrderDetail()
                        {
                            Count = 1,
                            CourseId = courseId,
                            Price = course.CoursePrice,
                        }
                    }
                };

                _context.Order.Add(order);
                _context.SaveChanges();
            }
            else
            {
                OrderDetail orderDetail = _context.OrderDetail
                    .FirstOrDefault(od => od.OrderId == order.OrderId && od.CourseId == courseId);

                if (orderDetail != null)
                {
                    orderDetail.Count += 1;
                    _context.OrderDetail.Update(orderDetail);
                    _context.Order.Update(order);
                    _context.SaveChanges();
                }

                else
                {
                    orderDetail = new OrderDetail()
                    {
                        CourseId = course.CourseId,
                        OrderId = order.OrderId,
                        Count = 1,
                        Price = course.CoursePrice
                    };

                    _context.OrderDetail.Add(orderDetail);
                    _context.SaveChanges();
                }

                UpdateOrderPrice(order.OrderId);
            }
            return order.OrderId;
        }

        public void DeleteDiscountCode(int discountId)
        {
            var Discount = GetDiscountById(discountId);
            Discount.IsDeleted = true;

            _context.Discount.Update(Discount);
            _context.SaveChanges();
        }

        public bool FinallyOrder(string userName, int orderId)
        {
            int userId = _userService.GetUserIdByUserName(userName);
            var order = _context.Order.Include(od => od.OrderDetails).ThenInclude(od => od.Course)
                .FirstOrDefault(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null || order.IsFainaly)
                return false;


            if (_userService.BalanceUserWallet(userName) >= order.OrderSum)
            {
                order.IsFainaly = true;
                _userService.AddWallet(new Wallet()
                {
                    Amount = order.OrderSum,
                    CreateDate = DateTime.Now,
                    Description = "فاکتور شماره #" + order.OrderId,
                    IsPayed = true,
                    TypeId = 2,
                    UserId = userId,
                });

                foreach (var detail in order.OrderDetails)
                {
                    _context.UserCourse.Add(new UserCourse
                    {
                        CourseId = detail.CourseId,
                        UserId = userId,
                    });
                }
                _context.Order.Update(order);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public DiscountsForAdminPanelViewModel GetAllDeletedDiscountsForAdmin(int pageId = 1)
        {
            IQueryable<Discount> Discounts = _context.Discount.Where(d => d.IsDeleted).IgnoreQueryFilters();

            int take = 20;
            int skip = (pageId - 1) * take;

            var DiscountsViewModel = new DiscountsForAdminPanelViewModel()
            {
                CurrentPage = pageId,
                PageCount = Discounts.Count() / take,
                Discount = Discounts.Skip(skip).Take(take).ToList()
            };

            return DiscountsViewModel;
        }

        public List<Discount> GetAllDiscounts()
        {
            return _context.Discount.ToList();
        }

        public DiscountsForAdminPanelViewModel GetAllDiscountsForAdmin(int pageId = 1)
        {
            IQueryable<Discount> Discounts = _context.Discount;

            int take = 20;
            int skip = (pageId - 1) * take;

            var DiscountsViewModel = new DiscountsForAdminPanelViewModel()
            {
                CurrentPage = pageId,
                PageCount = Discounts.Count() / take,
                Discount = Discounts.Skip(skip).Take(take).ToList()
            };

            return DiscountsViewModel;
        }

        public UserOrdersForUserPanelViewModel GetAllOrdersForUserPanel(string UserName, int pageId = 1)
        {
            int userId = _userService.GetUserIdByUserName(UserName);

            IQueryable<Order> order = _context.Order.Where(o => o.UserId == userId);
            int take = 12;
            int skip = (pageId - 1) * take;
            UserOrdersForUserPanelViewModel OrdersViewModel = new UserOrdersForUserPanelViewModel();

            OrdersViewModel.PageCount = order.Count() / take;
            OrdersViewModel.CurrentPage = pageId;
            OrdersViewModel.Orders = order.Skip(skip).Take(take).ToList();

            return OrdersViewModel;
        }

        public Discount GetDeletedDiscountById(int discountId)
        {
            return _context.Discount.IgnoreQueryFilters()
                .SingleOrDefault(d => d.DiscountId == discountId);
        }

        public Discount GetDiscountById(int discountId)
        {
            return _context.Discount.Find(discountId);
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Order.Find(orderId);
        }

        public Order GetOrderForUserPanel(string UserName, int orderId)
        {
            int userId = _userService.GetUserIdByUserName(UserName);

            return _context.Order.Include(o => o.OrderDetails).ThenInclude(o => o.Course)
                .FirstOrDefault(o => o.UserId == userId && o.OrderId == orderId);
        }

        public bool IsExistsDiscountCode(string discountCode)
        {
            return _context.Discount.Any(d => d.DiscountCode == discountCode);
        }

        public void RecoveryDiscountCode(int discountId)
        {
            var Discount = _context.Discount.IgnoreQueryFilters().SingleOrDefault(d => d.DiscountId == discountId);
            Discount.IsDeleted = false;

            UpdateDiscount(Discount);
        }

        public void UpdateDiscount(Discount discount)
        {
            _context.Discount.Update(discount);
            _context.SaveChanges();
        }

        public void UpdateOrder(Order order)
        {
            _context.Update(order);
            _context.SaveChanges();
        }

        public void UpdateOrderPrice(int orderId)
        {
            var order = _context.Order.Find(orderId);
            order.OrderSum = _context.OrderDetail.
                Where(od => od.OrderId == orderId).
                Sum(od => od.Price * od.Count);

            _context.Order.Update(order);
            _context.SaveChanges();
        }

        public DiscountType UseDiscount(int orderId, string discountCode)
        {
            var discount = _context.Discount.SingleOrDefault(d => d.DiscountCode == discountCode);

            if (discount == null)
            {
                return DiscountType.NotFound;
            }

            if (discount.StartDate != null && discount.StartDate >= DateTime.Now)
            {
                return DiscountType.ExpierDate;
            }

            if (discount.EndDate != null && discount.EndDate <= DateTime.Now)
            {
                return DiscountType.ExpierDate;
            }

            if (discount.UsableCount != null && discount.UsableCount < 1)
            {
                return DiscountType.IsFinished;
            }

            // Accept discount code to user order =>

            Order order = GetOrderById(orderId);

            if (_context.UserDiscountCode.Any(d => d.UserId == order.UserId && d.DiscountId == discount.DiscountId))
            {
                return DiscountType.UserUsed;
            }

            order.OrderSum = order.OrderSum - (order.OrderSum * discount.DiscountPersent) / 100;

            UpdateOrder(order);

            if (discount.UsableCount != null)
            {
                discount.UsableCount -= 1;
            }

            _context.Discount.Update(discount);

            // Add This Discount code to user's used discount's list =>

            _context.UserDiscountCode.Add(new UserDiscountCode
            {
                DiscountId = discount.DiscountId,
                UserId = order.UserId,
            });

            _context.SaveChanges();

            return DiscountType.Success;
        }
    }
}
