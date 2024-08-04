using MongoDB.Driver;
using MongoWeb.Controllers;
using MongoWeb.Models;
using MongoWeb.Repositores;
using MongoWeb.Services;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MongoWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Khởi tạo kết nối MongoDB
            var client = new MongoClient("mongodb://localhost:27017/");
            //var database = client.GetDatabase("Cua_Hang_My_Pham");
            var database = client.GetDatabase("test");
            var todoCollection = database.GetCollection<Products>("Products");
            var userCollection = database.GetCollection<Users>("Users");


            var userCollection = database.GetCollection<Users>("Users");
            //// Khởi tạo TodoSqlRepository cho SQL Server
            //string sqlServerConnectionString = "Server=LAPTOP-FD3P69GF;Database=TODO;Integrated Security=True;";
            //ITodoRepository todoRepository = new TodoSqlRepository(sqlServerConnectionString);

            //// Khởi tạo TodoFileRepository
            //string csvFilePath = "E:\\ChuBieu\\cstodo\\cstodo\\todo_task.todos.csv";
            //ITodoRepository csvRepository = new TodoFileRepository(csvFilePath);

            // Thiết lập Dependency Resolver
            DependencyResolver.SetResolver(new MyDependencyResolver(todoCollection, userCollection));

        }
        public class MyDependencyResolver : IDependencyResolver
        {
            private IMongoCollection<Products> todoCollection;
            private IMongoCollection<Users> userCollection;
            //private ITodoRepository todoRepository;
            //private ITodoRepository csvRepository;

            public MyDependencyResolver(IMongoCollection<Products> todoCollection, IMongoCollection<Users> userCollection)
            {
                this.todoCollection = todoCollection;
                this.userCollection = userCollection;
                //this.todoRepository = todoRepository;
                //this.csvRepository = csvRepository;
            }

            

            public object GetService(Type serviceType)
            {
                if (serviceType == typeof(HomeController))
                {
                    var repository = new TodoRepository(todoCollection, userCollection);
                    //var userrepository = new TodoRepository(userCollection);
                    //var addTodo = new AddTodo(repository);
                    //var getAllTodos = new GetAllTodos(repository);
                    //var repository = csvRepository;
                    var addTodo = new AddTodo(repository);
                    var getAllTodos = new GetAll(repository);
                    var login = new Login(repository);
                    //return new TodoController(repository);
                    return new HomeController(addTodo, getAllTodos, login);
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
