using MongoDB.Bson;
using MongoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoWeb
{
    public interface ITodoRepository
    {
        void Add(Products products);
        List<Products> GetAll();

        List<string> GetProductCategories();
        Products GetById(string id);
        List<Products> SearchProducts(string query);

        void Login(string email, string password);

        List<CartItem> GetCartItems();

        Users GetUserById(string id);
        void ClearCart(ObjectId UserId);

        // Order Methods

         void PlaceOrder(Order order);
        Order GetOrderById(string id);
        List<Order> GetAllOrders(string email);
        void UpdateOrder(Order order);
        void DeleteOrder(string orderId);
    }

}
