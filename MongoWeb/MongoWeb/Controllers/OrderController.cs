//using MongoWeb.Models;
//using MongoWeb.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace MongoWeb.Controllers
//{
//    public class OrderController : Controller
//    {
//        // GET: Order
//        //private readonly Orderservice _orderService;
//        private ITodoRepository _repository;
//        public OrderController()
//        {
//            // Khởi tạo Orderservice với ITodoRepository (có thể được tiêm qua Dependency Injection)
//            var repository = _repository; // Khởi tạo hoặc lấy ITodoRepository từ DI container
//            _orderService = new Orderservice(repository);
//        }

//        // GET: Order/Details/5
//        public ActionResult Details(string id)
//        {
//            var order = _orderService.GetOrderById(id);
//            if (order == null)
//            {
//                return HttpNotFound();
//            }
//            return View(order);
//        }

//        // POST: Order/PlaceOrder
//        [HttpPost]
//        public ActionResult PlaceOrder(string userId, List<CartItem> cartItems, string shippingAddress, string paymentMethod)
//        {
//            try
//            {
//                _orderService.PlaceOrder(userId, cartItems, shippingAddress, paymentMethod);
//                TempData["SuccessMessage"] = "Order placed successfully!";
//                return RedirectToAction("OrderSuccess");
//            }
//            catch (ArgumentException ex)
//            {
//                TempData["ErrorMessage"] = ex.Message;
//                return RedirectToAction("Cart"); // Giả sử bạn có phương thức Cart để hiển thị giỏ hàng
//            }
//            catch (Exception ex)
//            {
//                TempData["ErrorMessage"] = "An error occurred while placing the order.";
//                return RedirectToAction("Cart");
//            }
//        }

//        public ActionResult OrderSuccess()
//        {
//            return View();
//        }
//    }
//}