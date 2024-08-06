using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MongoDB.Driver;
using MongoWeb.Controllers;
using MongoWeb.Models;
using MongoWeb.Repositores;
using MongoWeb.Services;

namespace MongoWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            try
            {
                // Khởi tạo kết nối MongoDB
                var client = new MongoClient("mongodb://localhost:27017/");
                var database = client.GetDatabase("Cua_Hang_My_Pham");

                // Khởi tạo các collection
                var productCollection = database.GetCollection<Products>("Products");
                var userCollection = database.GetCollection<Users>("Users");
                var orderCollection = database.GetCollection<Order>("Orders");

                // Thiết lập Dependency Resolver
                DependencyResolver.SetResolver(new MyDependencyResolver(productCollection, userCollection, orderCollection));
            }
            catch (MongoConnectionException ex)
            {
                // Xử lý lỗi kết nối MongoDB
                Console.WriteLine($"Lỗi kết nối MongoDB: {ex.Message}");
                // Bạn có thể ghi log hoặc thông báo cho người dùng tại đây
                // Ví dụ: Ghi log vào một file log hoặc thông báo trên giao diện người dùng
                throw new InvalidOperationException("Không thể kết nối đến MongoDB", ex);
            }
            catch (MongoException ex)
            {
                // Xử lý lỗi chung liên quan đến MongoDB
                Console.WriteLine($"Lỗi MongoDB: {ex.Message}");
                // Bạn có thể ghi log hoặc thông báo cho người dùng tại đây
                throw new InvalidOperationException("Lỗi khi làm việc với MongoDB", ex);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi chung không phải liên quan đến MongoDB
                Console.WriteLine($"Lỗi không xác định: {ex.Message}");
                // Bạn có thể ghi log hoặc thông báo cho người dùng tại đây
                throw new InvalidOperationException("Lỗi không xác định", ex);
            }
        }

        public class MyDependencyResolver : IDependencyResolver
        {
            private readonly IMongoCollection<Products> productCollection;
            private readonly IMongoCollection<Users> userCollection;
            private readonly IMongoCollection<Order> orderCollection;

            public MyDependencyResolver(IMongoCollection<Products> productCollection, IMongoCollection<Users> userCollection, IMongoCollection<Order> orderCollection)
            {
                this.productCollection = productCollection;
                this.userCollection = userCollection;
                this.orderCollection = orderCollection;
            }

            public object GetService(Type serviceType)
            {
                if (serviceType == typeof(HomeController))
                {
                    var repository = new TodoRepository(productCollection, userCollection, orderCollection);
                    var addTodo = new AddTodo(repository);
                    var getAllTodos = new GetAll(repository);
                    var login = new Login(repository);
                    return new HomeController(addTodo, getAllTodos, login);
                }
                if (serviceType == typeof(CartController))
                {
                    var repository = new TodoRepository(productCollection, userCollection, orderCollection);
                    var cartService = new Cart(repository);
                    return new CartController(cartService, repository);
                }
                return null;
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                return Enumerable.Empty<object>();
            }
        }
    }
}
