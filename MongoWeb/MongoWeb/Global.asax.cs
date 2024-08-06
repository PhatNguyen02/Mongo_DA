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
using Unity.Mvc5;
using Unity;

namespace MongoWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Initialize Unity container
            var container = new UnityContainer();

            // Initialize MongoDB connection
            var client = new MongoClient("mongodb://localhost:27017/");
            var database = client.GetDatabase("Cua_Hang_My_Pham");
            var todoCollection = database.GetCollection<Products>("Products");
            var userCollection = database.GetCollection<Users>("Users");

            // Register MongoDB collections
            container.RegisterInstance(todoCollection);
            container.RegisterInstance(userCollection);

            // Register repositories and services
            container.RegisterType<ITodoRepository, TodoRepository>();
            container.RegisterType<Models.Register>(); // Ensure this matches your actual service

            // Set up Dependency Resolver
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

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
                    var register = new Login(repository);
                    var userService = new UserService(repository);
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
