using MongoWeb.Models;
using MongoWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MongoWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private AddTodo addTodo;
        private GetAll getAllTodos;

        public HomeController(AddTodo addTodo, GetAll getAllTodos)
        {
            this.addTodo = addTodo;
            this.getAllTodos = getAllTodos;
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
    }
}