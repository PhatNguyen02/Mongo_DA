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

        public TodoRepository(IMongoCollection<Products> database, IMongoCollection<Users> userCollection)
        {
            collection = database;
            collectionUser = userCollection;
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


    }
}