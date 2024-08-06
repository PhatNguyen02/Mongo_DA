using MongoWeb.Models;
using MongoWeb.Repositores;
using MongoWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.UI;
using X.PagedList;
using X.PagedList.Extensions;


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
        public ActionResult Index(int page = 1)
        {

            int pageSize = 8;

            var allProducts = getAllTodos.Excute();
            var categories = getAllTodos.GetProductCategories();

            // Phân trang danh sách sản phẩm
            var pagedProducts = allProducts
                .OrderBy(p => p.ProductId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalItems = allProducts.Count();

            ViewBag.Categories = categories;
            ViewBag.TotalPages = Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;

            return View(pagedProducts);

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

        public ActionResult Details(string id)
        {
            var product = getAllTodos.GetById(id); // Sử dụng phương thức GetById để lấy thông tin chi tiết sản phẩm
            if (product == null)
            {
                return HttpNotFound(); // Trả về lỗi 404 nếu không tìm thấy sản phẩm
            }
            return View(product); // Trả về view hiển thị chi tiết sản phẩm
        }
        [HttpGet]
        public ActionResult Search(string query, int page = 1)
        {
            int pageSize = 8;

            // Sử dụng TodoRepository để tìm kiếm
            var searchResults = getAllTodos.Search(query); // Sử dụng SearchProducts thay vì Search

            // Phân trang kết quả tìm kiếm
            var pagedResults = searchResults
                .OrderBy(p => p.ProductId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalItems = searchResults.Count();

            // Cung cấp dữ liệu cho view
            ViewBag.Query = query;
            ViewBag.TotalPages = Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;

            // Trả về view Index với kết quả tìm kiếm
            return View("Index", pagedResults);
        }


        public ActionResult Login()
        {
            ViewBag.UserName = Session["UserName"] as string ?? "Guest";
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
                   

                    // Lưu thông tin người dùng vào Session
                    Session["UserName"] = model.Email; // user là đối tượng chứa thông tin người dùng, ví dụ: tên người dùng, email, v.v.

                    var userName = Session["UserName"] as string;

                    // Truyền thông tin vào ViewBag
                    ViewBag.UserName = Session["Username"] ?? "Guest";
                    //ViewBag.UserName = userName ?? "Guest";

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }
            return View(model);
        }
        public ActionResult Logout(LoginViewModel model)
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