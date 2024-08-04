using MongoWeb.Models;
using MongoWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using System.Web.UI.WebControls;

namespace MongoWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private AddTodo addTodo;
        private GetAll getAllTodos;
        private Login login;

        public HomeController(AddTodo addTodo, GetAll getAllTodos , Login login)
        {
            this.addTodo = addTodo;
            this.getAllTodos = getAllTodos;
            this.login = login;
        }
        public ActionResult Index()
        {
            var todos = getAllTodos.Excute();
            return View(todos);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Products todo)
        {
            if (ModelState.IsValid)
            {
                addTodo.Excute(todo);
                return RedirectToAction("Index");
            }
            return View(todo);
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isAuthenticated = login.Authenticate(model.Email, model.Password);
                if (isAuthenticated)
                {
                    // Lưu thông tin người dùng vào Session hoặc Cookie nếu cần
                    // Ví dụ: Session["User"] = user;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }
            return View(model);
        }
    }
}