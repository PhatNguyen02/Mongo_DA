using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoWeb.Models;
using MongoDB.Bson;
using System.Web.Helpers;


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
        //Them sp
        public void Add(Products products)
        {
            collection.InsertOne(products);
        }
        //Lay tat ca sp
        public List<Products> GetAll()
        {
            return collection.Find(_ => true).ToList();
        }
        //Lay sp theo id
        public Products GetById(string id) // Thực hiện truy vấn theo ID
        {
            return collection.Find(p => p.ProductId == id).FirstOrDefault();
        }

        // Xóa sản phẩm theo ID
        public void Delete(string id)
        {
            var product = GetById(id);
            if (product != null)
            {
                var filter = Builders<Products>.Filter.Eq(p => p.ProductId, id);
                collection.DeleteOne(filter);
            }
        }

        // Sửa sản phẩm theo ID
        public void UpdateProduct(string id, Products updatedProduct)
        {
            var filter = Builders<Products>.Filter.Eq(u => u.ProductId, id);
            var updateDefinition = Builders<Products>.Update
                .Set(u => u.ProductName, updatedProduct.ProductName)
                .Set(u => u.BrandName, updatedProduct.BrandName)
                .Set(u => u.ProductImage, updatedProduct.ProductImage)
                .Set(u => u.Price, updatedProduct.Price)
                .Set(u => u.Category, updatedProduct.Category)
                .Set(u => u.Description, updatedProduct.Description)
                .Set(u => u.CountInStock, updatedProduct.CountInStock);

            var updateResult = collection.UpdateOne(filter, updateDefinition);

            if (!updateResult.IsAcknowledged || updateResult.ModifiedCount == 0)
            {
                throw new Exception("Update failed");
            }
        }

        //Phat sửa
        //public void UpdateUser(string id, Users updatedUser)
        //{
        //    var filter = Builders<Users>.Filter.Eq(u => u.UserId, id);
        //    var updateDefinition = Builders<Users>.Update
        //        .Set(u => u.Username, updatedUser.Username)
        //        .Set(u => u.FullName, updatedUser.FullName)
        //        .Set(u => u.Email, updatedUser.Email)
        //        .Set(u => u.Password, updatedUser.Password)
        //        .Set(u => u.PhoneNumber, updatedUser.PhoneNumber)
        //        .Set(u => u.Address, updatedUser.Address)
        //        .Set(u => u.DateRegistered, updatedUser.DateRegistered);

        //    var updateResult = collectionUser.UpdateOne(filter, updateDefinition);

        //    if (!updateResult.IsAcknowledged || updateResult.ModifiedCount == 0)
        //    {
        //        throw new Exception("Update failed");
        //    }
        //}

        // User Methods
        //Them user
        public void Add(Users users)
        {
            collectionUser.InsertOne(users);
        }
        //Lay tat ca user
        public List<Users> GetAllUser()
        {
            return collectionUser.Find(_ => true).ToList();
        }
        //Lay user theo id
        public Users GetUserById(string id) // Thực hiện truy vấn theo ID
        {
            return collectionUser.Find(p => p.UserId == id).FirstOrDefault();
        }

        // Xóa sản phẩm theo ID
        public void DeleteUser(string id)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                var filter = Builders<Users>.Filter.Eq(p => p.UserId, id);
                collectionUser.DeleteOne(filter);
            }
        }

        // Sửa User theo ID
        //public void UpdateUser(string id, Users updatedUser)
        //{
        //    var existingProduct = GetUserById(id);
        //    if (existingProduct != null)
        //    {
        //        var filter = Builders<Users>.Filter.Eq(p => p.UserId, id);
        //        collectionUser.ReplaceOne(filter, updatedUser);
        //    }
        //}
        //Lay danh muc sp
        public List<string> GetProductCategories()
        {
            var distinctCategories = collection.Distinct<string>("category", FilterDefinition<Products>.Empty).ToList();
            return distinctCategories;
        }

        

        //Tim sp
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

        //Login
        public void Login(string gmail, string password)
        {
            var user = collectionUser.Find(u => u.Email == gmail && u.Password == password).FirstOrDefault();
            if (user == null)
            {
                throw new Exception("Invalid login credentials.");
            }

        }

        //Get UserId mới
        public string GetLastUserId()
        {
            var lastUser = collectionUser.Find(FilterDefinition<Users>.Empty)
                                          .SortByDescending(u => u.DateRegistered)
                                          .FirstOrDefault();
            return lastUser?.UserId;
        }
        //Get PductId mới
        public string GetLastProductId()
        {
            var lastProduct = collection.Find(FilterDefinition<Products>.Empty)
                                        .SortByDescending(p => p.ProductId)
                                        .FirstOrDefault();
            return lastProduct?.ProductId;
        }
        public void Register(Register model)
        {
            var user = new Users
            {
                UserId = model.UserId,
                Username = model.Username,
                FullName = model.FullName,
                Email = model.Email,
                Password = model.Password,
                PhoneNumber = model.PhoneNumber,
                Role = model.Role,
                Address = model.Address,
                DateRegistered = model.DateRegistered,
            };

            collectionUser.InsertOne(user);
        }


        public List<Users> GetAllUsers()
        {
            return collectionUser.Find(_ => true).ToList();
        }

        public Users Getuser(string id)
        {
            return collectionUser.Find(user => user.Id == new ObjectId(id)).FirstOrDefault();
        }

        public void Deleteuser(string id)
        {
            collectionUser.DeleteOne(user => user.Id == new ObjectId(id));
        }

        public void UpdateUser(string id, Users updatedUser)
        {
            var filter = Builders<Users>.Filter.Eq(u => u.UserId, id);
            var updateDefinition = Builders<Users>.Update
                .Set(u => u.Username, updatedUser.Username)
                .Set(u => u.FullName, updatedUser.FullName)
                .Set(u => u.Email, updatedUser.Email)
                .Set(u => u.Password, updatedUser.Password)
                .Set(u => u.PhoneNumber, updatedUser.PhoneNumber)
                .Set(u => u.Address, updatedUser.Address)
                .Set(u => u.DateRegistered, updatedUser.DateRegistered);

            var updateResult = collectionUser.UpdateOne(filter, updateDefinition);

            if (!updateResult.IsAcknowledged || updateResult.ModifiedCount == 0)
            {
                throw new Exception("Update failed");
            }
        }

    }
}