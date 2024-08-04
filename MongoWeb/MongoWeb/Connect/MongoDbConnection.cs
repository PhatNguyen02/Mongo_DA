using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;


namespace MongoWeb.Connect
{
    public class MongoDbConnection
    {
       
            private readonly string _connectionString;
            private readonly string _databaseName;

            // Constructor để thiết lập kết nối MongoDB
            public MongoDbConnection()
            {
                // Thay đổi connection string và tên database theo yêu cầu của bạn
                _connectionString = "mongodb://localhost:27017"; // Connection string cho MongoDB
                _databaseName = "Cua_Hang_My_Pham"; // Tên cơ sở dữ liệu MongoDB
            }

            // Phương thức để tạo và trả về đối tượng MongoClient
            public IMongoClient CreateClient()
            {
                return new MongoClient(_connectionString);
            }

            // Phương thức để tạo và trả về đối tượng IMongoDatabase
            public IMongoDatabase GetDatabase()
            {
                var client = CreateClient();
                return client.GetDatabase(_databaseName);
            }

            // Phương thức để tạo và trả về đối tượng IMongoCollection<T>
            public IMongoCollection<T> GetCollection<T>(string collectionName)
            {
                var database = GetDatabase();
                return database.GetCollection<T>(collectionName);
            }
        }
    
}