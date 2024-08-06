using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoWeb.Models;
using MongoDB.Bson;


namespace MongoWeb.Repositores
{
    public class TodoRepository : ITodoRepository
    {
        public readonly IMongoCollection<Products> collection;
        public readonly IMongoCollection<Users> collectionUser;
        public readonly IMongoCollection<Order> collectionOrder; // Thêm IMongoCollection cho Order


        public TodoRepository(IMongoCollection<Products> database, IMongoCollection<Users> userCollection , IMongoCollection<Order> orderCollection)
        {
            collection = database;
            collectionUser = userCollection;
            collectionOrder = orderCollection;
        }
        public void Add(Products products)
        {
            collection.InsertOne(products);
        }
        public List<Products> GetAll()
        {
            return collection.Find(_ => true).ToList();
        }

        // User Methods


        public List<string> GetProductCategories()
        {
            var distinctCategories = collection.Distinct<string>("category", FilterDefinition<Products>.Empty).ToList();
            return distinctCategories;
        }
        public Products GetById(string id) // Thực hiện truy vấn theo ID
        {
            return collection.Find(p => p.ProductId == id).FirstOrDefault();
        }
        public List<Products> SearchProducts(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                // Trả về tất cả sản phẩm nếu query rỗng hoặc null
                return collection.Find(_ => true).ToList(); // Trả về tất cả sản phẩm
            }

            // Loại bỏ khoảng trắng thừa từ query
            query = query.Trim();

            // Tạo biểu thức chính quy với tùy chọn không phân biệt chữ hoa chữ thường
            var filter = Builders<Products>.Filter.Regex("ProductName", new MongoDB.Bson.BsonRegularExpression($".*{query}.*", "i"));

            // Thực hiện tìm kiếm với filter
            return collection.Find(filter).ToList();


          

        }
        public void Login(string gmail, string password)
        {
            var user = collectionUser.Find(u => u.Email == gmail && u.Password == password).FirstOrDefault();
            if (user == null)
            {
                throw new Exception("Invalid login credentials.");
            }

        }

        public void AddUser(Users user)
        {
            collectionUser.InsertOne(user);
        }

        public List<Users> GetAllUsers()
        {
            return collectionUser.Find(_ => true).ToList();
        }

        public Users GetUserById(string id)
        {
            return collectionUser.Find(user => user.Id == new ObjectId(id)).FirstOrDefault();
        }

        public void UpdateUser(Users user)
        {
            collectionUser.ReplaceOne(u => u.Id == user.Id, user);
        }

        public void DeleteUser(string id)
        {
            collectionUser.DeleteOne(user => user.Id == new ObjectId(id));


        }

        //Cart
        public List<CartItem> GetCartItems()
        {
            if (HttpContext.Current.Session["Cart"] == null)
            {
                HttpContext.Current.Session["Cart"] = new List<CartItem>();
            }
            return (List<CartItem>)HttpContext.Current.Session["Cart"];
        }

        public void AddToCart(ObjectId userId, Products product, int quantity)
        {
            var cartItems = GetCartItems();
            var cartItem = cartItems.FirstOrDefault(item => item.Product.Id == product.Id);

            if (cartItem == null)
            {
                cartItems.Add(new CartItem
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            HttpContext.Current.Session["Cart"] = cartItems;
        }

        public void RemoveFromCart(ObjectId userId, ObjectId productId)
        {
            var cartItems = GetCartItems();
            var cartItem = cartItems.FirstOrDefault(item => item.Product.Id == productId);

            if (cartItem != null)
            {
                cartItems.Remove(cartItem);
            }

            HttpContext.Current.Session["Cart"] = cartItems;
        }

        public void ClearCart(ObjectId userId)
        {
            HttpContext.Current.Session["Cart"] = new List<CartItem>();
        }


        //order
        //public void AddOrder(Order order)
        //{
        //    collectionOrder.InsertOne(order);
        //}

        //public Order GetOrderById(string id)
        //{
        //    return collectionOrder.Find(o => o.Id == id).FirstOrDefault();
        //}

        //public List<Order> GetAllOrders()
        //{
        //    return collectionOrder.Find(_ => true).ToList();
        //}

        //public void UpdateOrder(Order order)
        //{
        //    collectionOrder.ReplaceOne(o => o.Id == order.Id, order);
        //}

        //public void DeleteOrder(string orderId)
        //{
        //    collectionOrder.DeleteOne(o => o.Id == orderId);
        //}

        //public void PlaceOrder(string customerName, string shippingAddress, string paymentMethod)
        //{
        //    var cartItems = GetCartItems();
        //    if (!cartItems.Any())
        //    {
        //        throw new Exception("Cart is empty.");
        //    }

        //    var orderId = Guid.NewGuid().ToString();
        //    var totalAmount = cartItems.Sum(item => item.Product.Price * item.Quantity);

        //    var order = new Order
        //    {
        //        OrderId = orderId,
        //        CustomerName = customerName,
        //        OrderDate = DateTime.UtcNow,
        //        TotalAmount = totalAmount,
        //        Items = cartItems.Select(item => new OrderItem
        //        {
        //            ProductId = item.Product.ProductId,
        //            Quantity = item.Quantity
        //        }).ToList(),
        //        ShippingAddress = shippingAddress,
        //        PaymentMethod = paymentMethod,
        //        Status = "Pending"
        //    };

        //    AddOrder(order);
        //    ClearCart(ObjectId.Empty);
        //}
        public void PlaceOrder(Order order)
        {
            collectionOrder.InsertOne(order);
        }

        public Order GetOrderById(string id)
        {
            return collectionOrder.Find(o => o.OrderId == id).FirstOrDefault();
        }

        public List<Order> GetAllOrders(string email)
        {
            return collectionOrder.Find(order => order.Email == email).ToList();
        }

        public void UpdateOrder(Order order)
        {
            collectionOrder.ReplaceOne(o => o.OrderId == order.OrderId, order);
        }

        public void DeleteOrder(string orderId)
        {
            collectionOrder.DeleteOne(o => o.OrderId == orderId);
        }




    }
}