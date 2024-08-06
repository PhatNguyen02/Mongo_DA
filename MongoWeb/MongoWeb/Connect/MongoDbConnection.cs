//using System;
//using MongoDB.Driver;

//namespace MongoWeb.Connect
//{
//    public class MongoDbConnection
//    {
//        private readonly string _connectionString;
//        private readonly string _databaseName;

//        // Constructor để thiết lập kết nối MongoDB
//        public MongoDbConnection()
//        {
//            // Thay đổi connection string và tên database theo yêu cầu của bạn
//            _connectionString = "mongodb://localhost:27017"; // Connection string cho MongoDB
//            _databaseName = "Cua_Hang_My_Pham"; // Tên cơ sở dữ liệu MongoDB
//        }

//        // Phương thức để tạo và trả về đối tượng MongoClient
//        public IMongoClient CreateClient()
//        {
//            try
//            {
//                return new MongoClient(_connectionString);
//            }
//            catch (Exception ex)
//            {
//                // Xử lý lỗi kết nối MongoDB ở đây
//                Console.WriteLine($"Lỗi khi tạo MongoClient: {ex.Message}");
//                // Có thể ném lỗi lên để xử lý ở nơi gọi
//                throw new InvalidOperationException("Không thể tạo MongoClient", ex);
//            }
//        }

//        // Phương thức để tạo và trả về đối tượng IMongoDatabase
//        public IMongoDatabase GetDatabase()
//        {
//            try
//            {
//                var client = CreateClient();
//                return client.GetDatabase(_databaseName);
//            }
//            catch (Exception ex)
//            {
//                // Xử lý lỗi khi lấy cơ sở dữ liệu
//                Console.WriteLine($"Lỗi khi lấy cơ sở dữ liệu: {ex.Message}");
//                // Có thể ném lỗi lên để xử lý ở nơi gọi
//                throw new InvalidOperationException("Không thể lấy cơ sở dữ liệu MongoDB", ex);
//            }
//        }

//        // Phương thức để tạo và trả về đối tượng IMongoCollection<T>
//        public IMongoCollection<T> GetCollection<T>(string collectionName)
//        {
//            try
//            {
//                var database = GetDatabase();
//                return database.GetCollection<T>(collectionName);
//            }
//            catch (Exception ex)
//            {
//                // Xử lý lỗi khi lấy collection
//                Console.WriteLine($"Lỗi khi lấy collection: {ex.Message}");
//                // Có thể ném lỗi lên để xử lý ở nơi gọi
//                throw new InvalidOperationException($"Không thể lấy collection '{collectionName}'", ex);
//            }
//        }
//    }
//}
