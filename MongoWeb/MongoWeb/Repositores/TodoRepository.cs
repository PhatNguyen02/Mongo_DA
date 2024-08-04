using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoWeb.Models;

namespace MongoWeb.Repositores
{
    public class TodoRepository : ITodoRepository
    {
        public readonly IMongoCollection<Products> collection;

        public TodoRepository(IMongoCollection<Products> database)
        {
            collection = database;
        }
        public void Add(Products products)
        {
            collection.InsertOne(products);
        }
        public List<Products> GetAll()
        {
            return collection.Find(_ => true).ToList();
        }
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

    }
}